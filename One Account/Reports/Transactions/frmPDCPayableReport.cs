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
    public partial class frmPDCPayableReport : Form
    {
        
        #region Functions
        /// <summary>
        /// to create an instance for frmPDCPayableReport
        /// </summary>
        public frmPDCPayableReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to clear the form controls
        /// </summary>
        public void Clear()
        {
            try
            {
                AccountLedgerComboFill();
                
                txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                dtpFrmDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                dtpTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                txtcheckdateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtcheckdateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpCheckDateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpCheckdateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
                cmbStatus.SelectedIndex = 0;
                cmbAccountLedger.SelectedIndex = -1;
                txtFromDate.SelectionStart = txtFromDate.TextLength;
                VoucherTypeComboFill();
                FinancialYearDate();
                txtFromDate.Select();
                txtcheckNo.Clear();
                txtVoucherNo.Clear();
                Search();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP1:" + ex.Message;
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
                dr[2] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbAccountLedger.DataSource = dtbl;
                cmbAccountLedger.ValueMember = "ledgerId";
                cmbAccountLedger.DisplayMember = "ledgerName";
                cmbAccountLedger.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblVouchetType = new DataTable();
                dtblVouchetType = spVoucherType.VoucherTypeSelectionComboFill("Pdc payable");
                DataRow dr = dtblVouchetType.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtblVouchetType.Rows.InsertAt(dr, 0);
                cmbVoucherType.DataSource = dtblVouchetType;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP3:" + ex.Message;
            }
        }
        /// <summary>
        /// to set the from date and todate based on settings
        /// </summary>
        public void FinancialYearDate()
        {
            try
            {
                dtpFrmDate.MinDate = PublicVariables._dtFromDate;
                dtpFrmDate.MaxDate = PublicVariables._dtToDate;
                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();
                infoComapany = spCompany.CompanyView(1);
                DateTime dtFromDate = PublicVariables._dtFromDate;
                dtpFrmDate.Value = dtFromDate;
                dtpFrmDate.Text = dtFromDate.ToString("dd-MMM-yyyy");
                dtpFrmDate.Value = Convert.ToDateTime(txtFromDate.Text);
                txtFromDate.Focus();
                txtFromDate.SelectAll();
                dtpTodate.MinDate = PublicVariables._dtFromDate;
                dtpTodate.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtToDate = infoComapany.CurrentDate;
                dtpTodate.Value = dtToDate;
                dtpTodate.Text = dtToDate.ToString("dd-MMM-yyyy");
                dtpTodate.Value = Convert.ToDateTime(txtTodate.Text);
                txtTodate.Focus();
                txtTodate.SelectAll();
                dtpCheckDateFrom.MinDate = PublicVariables._dtFromDate;
                dtpCheckDateFrom.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtCheckdateFrom = infoComapany.CurrentDate;
                dtpCheckDateFrom.Value = dtCheckdateFrom;
                dtpCheckDateFrom.Text = dtCheckdateFrom.ToString("dd-MMM-yyyy");
                dtpCheckDateFrom.Value = Convert.ToDateTime(txtcheckdateFrom.Text);
                txtcheckdateFrom.Focus();
                txtcheckdateFrom.SelectAll();
                dtpCheckdateTo.MinDate = PublicVariables._dtFromDate;
                dtpCheckdateTo.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtCheckdateTo = infoComapany.CurrentDate;
                dtpCheckdateTo.Value = dtCheckdateTo;
                dtpCheckdateTo.Text = dtCheckdateTo.ToString("dd-MMM-yyyy");
                dtpCheckdateTo.Value = Convert.ToDateTime(txtcheckdateTo.Text);
                txtcheckdateTo.Focus();
                txtcheckdateTo.SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to search the details
        /// </summary>
        public void Search()
        {
            try
            {
                if (cmbAccountLedger.Text.Trim() == string.Empty)
                {
                    cmbAccountLedger.Text = "All";
                }
                else if (cmbVoucherType.Text.Trim() == string.Empty)
                {
                    cmbVoucherType.Text = "All";
                }
                else if (cmbStatus.Text.Trim() == string.Empty)
                {
                    cmbStatus.Text = "All";
                }
                DataTable dtblPDCReport = new DataTable();
                PDCPayableMasterSP sppdcpayable = new PDCPayableMasterSP();
                dtblPDCReport = sppdcpayable.PdcPayableReportSearch(Convert.ToDateTime(dtpFrmDate.Value.ToString()), Convert.ToDateTime(dtpTodate.Value.ToString()), cmbVoucherType.Text.ToString(), cmbAccountLedger.Text.ToString(), Convert.ToDateTime(dtpCheckDateFrom.Value.ToString()), Convert.ToDateTime(dtpCheckdateTo.Value.ToString()), txtcheckNo.Text.Trim(), txtVoucherNo.Text.Trim(), cmbStatus.Text.Trim());
                dgvPDCPayableReport.DataSource = dtblPDCReport;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print the details
        /// </summary>
        public void Print()
        {
            try
            {
                string strFromDate = txtFromDate.Text.ToString();
                string strToDate = txtTodate.Text.ToString();
                decimal decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                decimal decLedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                PDCPayableMasterSP sppdcpayable = new PDCPayableMasterSP();
                DataSet dsPdcPayableReport = sppdcpayable.PdcpayableReportPrinting(Convert.ToDateTime(dtpFrmDate.Value.ToString()), Convert.ToDateTime(dtpTodate.Value.ToString()), cmbVoucherType.Text.ToString(), cmbAccountLedger.Text.ToString(), Convert.ToDateTime(dtpCheckDateFrom.Value.ToString()), Convert.ToDateTime(dtpCheckdateTo.Value.ToString()), txtcheckNo.Text.Trim(), txtVoucherNo.Text.Trim(), cmbStatus.Text.Trim(), 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.PdcpayablereportReportPrinting(dsPdcPayableReport);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP6:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// To set the text FromDate as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFrmDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFrmDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP7:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set the dtp value as textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpTodate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpTodate.Value;
                if (date >= Convert.ToDateTime(txtFromDate.Text))
                {
                    this.txtTodate.Text = date.ToString("dd-MMM-yyyy");
                }
                else
                {
                    txtTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP8:" + ex.Message;
            }
        }
        /// <summary>
        /// when form load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPDCPayableReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP9:" + ex.Message;
            }
        }
        /// <summary>
        /// To set the text FromDate as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpCheckDateFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpCheckDateFrom.Value;
                this.txtcheckdateFrom.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP10:" + ex.Message;
            }
        }
        /// <summary>
        /// To set the text FromDate as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpCheckdateTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpCheckdateTo.Value;
                this.txtcheckdateTo.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click, call the search function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateValidation ObjValidation = new DateValidation();
                ObjValidation.DateValidationFunction(txtTodate);
                ObjValidation.DateValidationFunction(txtcheckdateTo);
                if (Convert.ToDateTime(txtTodate.Text) < Convert.ToDateTime(txtFromDate.Text))
                {
                    if (Convert.ToDateTime(txtcheckdateTo.Text) < Convert.ToDateTime(txtcheckdateFrom.Text))
                    {
                        MessageBox.Show("todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtcheckdateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        txtcheckdateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    }
                    MessageBox.Show("todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                    Search();
                }
                else
                {
                    Search();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP12:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PPREP13:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click for updation of selected item in frmPdcPayable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPDCPayableReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    frmPdcPayable frmpdcpayableobj = new frmPdcPayable();
                    decimal decMasterId = Convert.ToDecimal(dgvPDCPayableReport.Rows[e.RowIndex].Cells["dgvpdcPayableMasterId"].Value.ToString());
                    frmPdcPayable open = Application.OpenForms["frmPdcPayable"] as frmPdcPayable;
                    if (open == null)
                    {
                        frmpdcpayableobj.WindowState = FormWindowState.Normal;
                        frmpdcpayableobj = new frmPdcPayable();
                        frmpdcpayableobj.MdiParent = formMDI.MDIObj;
                        frmpdcpayableobj.CallFromPdcPayableReport(this, decMasterId);
                    }
                    else
                    {
                        open.CallFromPdcPayableReport(this, decMasterId);
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click, call the print function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPDCPayableReport.RowCount > 0)
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
                formMDI.infoError.ErrorString = "PPREP15:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and set the dtp's value as textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTodate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtTodate);
                if (txtTodate.Text == string.Empty)
                {
                    txtTodate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                if (Convert.ToDateTime(txtTodate.Text) <= Convert.ToDateTime(txtFromDate.Text))
                {
                    txtTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                    DateTime dt;
                    DateTime.TryParse(txtTodate.Text, out dt);
                    dtpTodate.Value = dt;
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtTodate.Text, out dt);
                    dtpTodate.Value = dt;
                }
                string strdate = txtTodate.Text;
                dtpTodate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP16:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and set the dtp's value as textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtFromDate.Text;
                dtpFrmDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP17:" + ex.Message;
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
                ex.ExportExcel(dgvPDCPayableReport, "PDC Payable Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP18:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Form keydown for Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPDCPayableReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PPREP19:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtTodate.Focus();
                    txtTodate.SelectionStart = txtTodate.TextLength;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtFromDate.SelectionStart == 0 || txtFromDate.Text == string.Empty)
                    {
                        txtFromDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP20:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTodate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtTodate.SelectionStart == 0 || txtTodate.Text == string.Empty)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP21:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtTodate.Focus();
                    txtTodate.SelectionStart = 0;
                    txtTodate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP22:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbStatus.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP23:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountLedger.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.SelectionStart == 0 || txtVoucherNo.Text == string.Empty)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP24:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtcheckdateFrom.Focus();
                    txtcheckdateFrom.SelectionStart = txtcheckdateFrom.Text.Length;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbAccountLedger.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP25:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckdateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtcheckdateTo.Focus();
                    txtcheckdateTo.SelectionStart = txtcheckdateTo.Text.Length;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtcheckdateFrom.SelectionStart == 0 || txtcheckdateFrom.Text == string.Empty)
                    {
                        cmbStatus.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP26:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckdateTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtcheckNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtcheckdateTo.SelectionStart == 0 || txtcheckdateTo.Text == string.Empty)
                    {
                        txtcheckdateFrom.Focus();
                        txtcheckdateFrom.SelectionStart = 0;
                        txtcheckdateFrom.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP27:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtcheckNo.Text == "" || txtcheckNo.Text == string.Empty)
                    {
                        txtcheckdateTo.Focus();
                        txtcheckdateTo.SelectionStart = 0;
                        txtcheckdateTo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP28:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvPDCPayableReport.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtcheckNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP29:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPDCPayableReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    btnprint.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP30:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckdateTo_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtcheckdateTo);
                if (txtcheckdateTo.Text == string.Empty)
                {
                    txtcheckdateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtcheckdateTo.Text;
                dtpCheckdateTo.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP31:" + ex.Message;
            }
        }
        /// <summary>
        /// For enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckdateFrom_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtcheckdateFrom);
                if (txtcheckdateFrom.Text == string.Empty)
                {
                    txtcheckdateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtcheckdateFrom.Text;
                dtpCheckDateFrom.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PPREP32:" + ex.Message;
            }
        }
   
        #endregion

    }
}
