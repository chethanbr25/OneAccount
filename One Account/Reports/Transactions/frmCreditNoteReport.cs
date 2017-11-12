﻿//This is a source code or part of Oneaccount project
//Copyright (C) 2013  Oneaccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace One_Account
{
    public partial class frmCreditNoteReport : Form
    {
       
        #region Functions
        /// <summary>
        /// Create an instance for frmCreditNoteReport class
        /// </summary>
        public frmCreditNoteReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to clear the controls in the form
        /// </summary>
        public void Clear()
        {
            try
            {
                FinancialYearDate();
                VoucherTypeComboLoad();
                AccountLedgerComboFill();
                dtpFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                dtpToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
                txtFromDate.SelectionStart = txtFromDate.TextLength;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP1:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboLoad()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblVouchetType = new DataTable();
                dtblVouchetType = spVoucherType.VoucherTypeSelectionComboFill("Credit Note");
                DataRow dr = dtblVouchetType.NewRow();
                dr[0] = 0;
                dr[1] = "ALL";
                dtblVouchetType.Rows.InsertAt(dr, 0);
                cmbVoucherType.DataSource = dtblVouchetType;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill AccountLedger combobox
        /// </summary>
        public void AccountLedgerComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                AccountLedgerSP spaccountledger = new AccountLedgerSP();
                dtbl = spaccountledger.AccountLedgerViewAll();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[2] = "ALL";
                dtbl.Rows.InsertAt(dr, 0);
                cmbAccountLedger.DataSource = dtbl;
                cmbAccountLedger.ValueMember = "ledgerId";
                cmbAccountLedger.DisplayMember = "ledgerName";
                cmbAccountLedger.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP3:" + ex.Message;
            }
        }
        /// <summary>
        /// To set the From date and todate in corresponding fields
        /// </summary>
        public void FinancialYearDate()
        {
            try
            {
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;

                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();

                infoComapany = spCompany.CompanyView(1);
                DateTime dtFromDate = infoComapany.CurrentDate;
                dtpFromDate.Value = dtFromDate;
                dtpFromDate.Text = dtFromDate.ToString("dd-MMM-yyyy");
                dtpFromDate.Value = Convert.ToDateTime(txtFromDate.Text);
                txtFromDate.Focus();
                txtFromDate.SelectAll();
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;

                infoComapany = spCompany.CompanyView(1);
                DateTime dtToDate = infoComapany.CurrentDate;
                dtpToDate.Value = dtToDate;
                dtpToDate.Text = dtToDate.ToString("dd-MMM-yyyy");
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
                txtToDate.Focus();
                txtToDate.SelectAll();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void Search()
        {
            try
            {
                if (cmbVoucherType.Items.Count != 0 && cmbAccountLedger.Items.Count != 0)
                {
                    if ((cmbAccountLedger.SelectedValue.ToString() != "System.Data.DataRowView") && (cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView"))
                    {
                        if (txtFromDate.Text.Trim() != string.Empty && txtToDate.Text.Trim() != string.Empty)
                        {
                            string strFromDate = txtFromDate.Text;
                            string strToDate = txtToDate.Text;
                            decimal decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                            decimal decLedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());

                            DataTable dtblCreditNoteReport = new DataTable();
                            CreditNoteMasterSP spCreditNoteMaster = new CreditNoteMasterSP();
                            dtblCreditNoteReport = spCreditNoteMaster.CreditNoteReportSearch(strFromDate, strToDate, decVoucherTypeId, decLedgerId);
                            dgvCreditNoteReport.DataSource = dtblCreditNoteReport;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print the Report
        /// </summary>
        public void Print()
        {
            try
            {
                string strFromDate = txtFromDate.Text;
                string strToDate = txtToDate.Text;
                decimal decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                decimal decLedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());

                CreditNoteMasterSP SpCreditNoteMaster = new CreditNoteMasterSP();
                DataSet dsCreditNoteReport = SpCreditNoteMaster.CreditNoteReportPrinting(strFromDate, strToDate, decVoucherTypeId, decLedgerId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.CreditNoteReportPrinting(dsCreditNoteReport);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP6:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// When form load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCreditNoteReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
                Search();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP7:" + ex.Message;
            }
        }
        /// <summary>
        /// To set the Fromdate textbox value as dtp's selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFromDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP8:" + ex.Message;
            }
        }
        /// <summary>
        /// To set the Todate textbox value as dtp's selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtToDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP9:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set the text value as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtFromDate);
                if (!isInvalid)
                {
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                string date = txtFromDate.Text;
                dtpFromDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP10:" + ex.Message;
            }
        }
        /// <summary>
        /// /// Date validation and set the text value as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtToDate);
                if (!isInvalid)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtToDate.Text;
                dtpToDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Reset button click, call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP12:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click, call the Search function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateValidation ObjValidation = new DateValidation();
                ObjValidation.DateValidationFunction(txtToDate);


                if (Convert.ToDateTime(txtToDate.Text) < Convert.ToDateTime(txtFromDate.Text))
                {
                    MessageBox.Show("todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");

                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    Search();
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    Search();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP13:" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click, call the print function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCreditNoteReport.RowCount > 0)
                {
                    Print();
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click to view the creditNote form with curresponding value to updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCreditNoteReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvCreditNoteReport.CurrentRow.Index == e.RowIndex)
                    {
                        decimal decMasterId = Convert.ToDecimal(dgvCreditNoteReport.Rows[e.RowIndex].Cells["dgvtxtCreditNoteMasterId"].Value.ToString());

                        frmCreditNote frmCreditNoteObj = new frmCreditNote();

                        frmCreditNoteObj.MdiParent = formMDI.MDIObj;

                        frmCreditNote open = Application.OpenForms["frmCreditNote"] as frmCreditNote;
                        if (open == null)
                        {
                            frmCreditNoteObj.WindowState = FormWindowState.Normal;
                            frmCreditNoteObj.MdiParent = formMDI.MDIObj;
                            frmCreditNoteObj.CallFromCreditNoteReport(this, decMasterId);
                        }
                        else
                        {
                            open.MdiParent = formMDI.MDIObj;
                            open.BringToFront();
                            open.CallFromCreditNoteReport(this, decMasterId);
                            if (open.WindowState == FormWindowState.Minimized)
                            {
                                open.WindowState = FormWindowState.Normal;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP15:" + ex.Message;
            }
        }

        /// <summary>
        /// On 'Export' button click to export the report to Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportNew ex = new ExportNew();
                ex.ExportExcel(dgvCreditNoteReport, "Credit Note Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP16:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCreditNoteReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose)
                    {
                        Messages.CloseMessage(this);
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP17:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = txtToDate.TextLength;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP18:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.SelectionStart == 0 || txtToDate.Text == string.Empty)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = txtFromDate.TextLength;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP19:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountLedger.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = txtToDate.Text.Length;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP20:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP21:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbAccountLedger.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP22:" + ex.Message;
            }
        }

        /// <summary>
        ///  For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCreditNoteReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP23:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CRNTREP24:" + ex.Message;
            }
        }

        #endregion
   

    }
}
