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
    public partial class frmPDCRecievableReport : Form
    {
        #region Function
        /// <summary>
        /// Create an instance for frmPDCRecievableReport class
        /// </summary>
        public frmPDCRecievableReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to Set the from date and todate based on the settings
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
                txtFromDate.Text = dtFromDate.ToString("dd-MMM-yyyy");
                dtpFrmDate.Value = Convert.ToDateTime(txtFromDate.Text);
                txtFromDate.Focus();
                txtFromDate.SelectAll();
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtToDate = infoComapany.CurrentDate;
                dtpToDate.Value = dtToDate;
                txtToDate.Text = dtToDate.ToString("dd-MMM-yyyy");
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
                txtToDate.Focus();
                txtToDate.SelectAll();
                dtpCheckDateFrom.MinDate = PublicVariables._dtFromDate;
                dtpCheckDateFrom.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtCheckdateFrom = infoComapany.CurrentDate;
                dtpCheckDateFrom.Value = dtCheckdateFrom;
                txtcheckdateFrom.Text = dtCheckdateFrom.ToString("dd-MMM-yyyy");
                dtpCheckDateFrom.Value = Convert.ToDateTime(txtcheckdateFrom.Text);
                txtcheckdateFrom.Focus();
                txtcheckdateFrom.SelectAll();
                dtpCheckDateTo.MinDate = PublicVariables._dtFromDate;
                dtpCheckDateTo.MaxDate = PublicVariables._dtToDate;
                infoComapany = spCompany.CompanyView(1);
                DateTime dtCheckdateTo = infoComapany.CurrentDate;
                dtpCheckDateTo.Value = dtCheckdateTo;
                txtCheckDateTo.Text = dtCheckdateTo.ToString("dd-MMM-yyyy");
                dtpCheckDateTo.Value = Convert.ToDateTime(txtCheckDateTo.Text);
                txtCheckDateTo.Focus();
                txtCheckDateTo.SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the form controls
        /// </summary>
        public void Clear()
        {
            try
            {
                AccountLedgerComboFill();
                VoucherTypeComboFill();
                cmbAccountLedger.SelectedIndex = -1;
                dtpFrmDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpCheckDateFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpCheckDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtVoucherNo.Clear();
                txtCheckNo.Clear();
                txtFromDate.Focus();
                Search();
                cmbStatus.SelectedIndex = 0;
                txtFromDate.SelectionStart = txtFromDate.TextLength;
                FinancialYearDate();
                txtFromDate.Select();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP2:" + ex.Message;
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
                PDCPayableMasterSP sppdcpayable = new PDCPayableMasterSP();
                dtbl = sppdcpayable.AccountLedgerComboFill(false);
                DataRow dr = dtbl.NewRow();
                dr["ledgerId"] = 0;
                dr["ledgerName"] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbAccountLedger.DataSource = dtbl;
                cmbAccountLedger.ValueMember = "ledgerId";
                cmbAccountLedger.DisplayMember = "ledgerName";
                cmbAccountLedger.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP3:" + ex.Message;
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
                dtblVouchetType = spVoucherType.VoucherTypeSelectionComboFill("PDC Receivable");
                DataRow dr = dtblVouchetType.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtblVouchetType.Rows.InsertAt(dr, 0);
                cmbVouchertype.DataSource = dtblVouchetType;
                cmbVouchertype.ValueMember = "voucherTypeId";
                cmbVouchertype.DisplayMember = "voucherTypeName";
                cmbVouchertype.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to search based on the condition
        /// </summary>
        public void Search()
        {
            try
            {
                if (cmbAccountLedger.Text.Trim() == string.Empty)
                {
                    cmbAccountLedger.Text = "All";
                }
                else if (cmbVouchertype.Text.Trim() == string.Empty)
                {
                    cmbVouchertype.Text = "All";
                }
                else if (cmbStatus.Text.Trim() == string.Empty)
                {
                    cmbStatus.Text = "All";
                }
                DataTable dtblPDCReport = new DataTable();
                PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
                dtblPDCReport = sppdcreceivable.PdcReceivableReportSearch(Convert.ToDateTime(dtpFrmDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), cmbVouchertype.Text.ToString(), cmbAccountLedger.Text.ToString(), Convert.ToDateTime(dtpCheckDateFrom.Value.ToString()), Convert.ToDateTime(dtpCheckDateTo.Value.ToString()), txtCheckNo.Text.Trim(), txtVoucherNo.Text.Trim(), cmbStatus.Text.Trim());
                dgvPdcReceivableSearch.DataSource = dtblPDCReport;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print the form
        /// </summary>
        public void Print()
        {
            try
            {
                string strFromDate = txtFromDate.Text.ToString();
                string strToDate = txtToDate.ToString();
                decimal decVoucherTypeId = Convert.ToDecimal(cmbVouchertype.SelectedValue.ToString());
                decimal decLedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                PDCReceivableMasterSP SpPdcreceivable = new PDCReceivableMasterSP();
                DataSet dsPdcReceivableReport = SpPdcreceivable.PdcreceivableReportPrinting(Convert.ToDateTime(dtpFrmDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), cmbVouchertype.Text.ToString(), cmbAccountLedger.Text.ToString(), Convert.ToDateTime(dtpCheckDateFrom.Value.ToString()), Convert.ToDateTime(dtpCheckDateTo.Value.ToString()), txtCheckNo.Text.Trim(), txtVoucherNo.Text.Trim(), cmbStatus.Text.Trim(), 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.PdcreceivablereportReportPrinting(dsPdcReceivableReport);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP6:" + ex.Message;
            }
        }
    
        #endregion
        #region Events
        /// <summary>
        /// Form load, cxall the clear function to clear the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPDCRecievableReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP7:" + ex.Message;
            }
        }
        /// <summary>
        /// set the textbox value as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFrmDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFrmDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
                Search();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP8:" + ex.Message;
            }
        }
        /// <summary>
        /// set the textbox value as dtp's value
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
                formMDI.infoError.ErrorString = "PRREP9:" + ex.Message;
            }
        }
        /// <summary>
        /// set the textbox value as dtp's value
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
                formMDI.infoError.ErrorString = "PRREP10:" + ex.Message;
            }
        }
        /// <summary>
        /// set the textbox value as dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpCheckDateTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpCheckDateTo.Value;
                this.txtCheckDateTo.Text = date.ToString("dd-MMM-yyyy");
                Search();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click call the search function
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
                    Search();
                }
                else
                {
                    Search();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP12:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PRREP13:" + ex.Message;
            }
        }
        /// <summary>
        /// cell double click for edit the selected item updation in frmPdcReceivable form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPdcReceivableSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    frmPdcReceivable frmpdcReceivableObj = new frmPdcReceivable();
                    decimal decMasterId = Convert.ToDecimal(dgvPdcReceivableSearch.Rows[e.RowIndex].Cells["pdcReceivableMasterId"].Value.ToString());
                    frmPdcReceivable open = Application.OpenForms["frmPdcReceivable"] as frmPdcReceivable;
                    if (open == null)
                    {
                        frmpdcReceivableObj.WindowState = FormWindowState.Normal;
                        frmpdcReceivableObj.MdiParent = formMDI.MDIObj;
                        frmpdcReceivableObj.CallFromPdcReceivableReport(this, decMasterId);
                    }
                    else
                    {
                        open.CallFromPdcReceivableReport(this, decMasterId);
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click to call the print function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPdcReceivableSearch.RowCount > 0)
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
                formMDI.infoError.ErrorString = "PRREP15:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and set dtp value as textbox value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtToDate.Text;
                dtpToDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP16:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and set dtp value as textbox value
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
                formMDI.infoError.ErrorString = "PRREP17:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and set dtp value as textbox value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCheckDateTo_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtCheckDateTo);
                if (txtCheckDateTo.Text == string.Empty)
                {
                    txtCheckDateTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtCheckDateTo.Text;
                dtpCheckDateTo.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP18:" + ex.Message;
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
                ex.ExportExcel(dgvPdcReceivableSearch, "PDC Receivable Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP19:" + ex.Message;
            }
        }
        #endregion
        #region  Navigation
        /// <summary>
        /// For enter key and backspace navigation
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
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP20:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "PRREP21:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVouchertype.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.SelectionStart == 0 || txtToDate.Text == string.Empty)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP22:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVouchertype_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP23:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Text.Trim() == string.Empty || txtVoucherNo.SelectionStart == 0)
                    {
                        cmbVouchertype.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP24:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "PRREP25:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                    txtcheckdateFrom.SelectionStart = 0;
                    txtcheckdateFrom.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbAccountLedger.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP26:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckdateFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCheckDateTo.Focus();
                    txtCheckDateTo.SelectionStart = 0;
                    txtCheckDateTo.SelectionLength = 0;
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
                formMDI.infoError.ErrorString = "PRREP27:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCheckDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCheckNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtCheckDateTo.SelectionStart == 0 || txtCheckDateTo.Text == string.Empty)
                    {
                        txtcheckdateFrom.Focus();
                        txtcheckdateFrom.SelectionStart = 0;
                        txtcheckdateFrom.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP28:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCheckNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtCheckNo.Text == string.Empty || txtCheckNo.SelectionStart == 0)
                    {
                        txtCheckDateTo.Focus();
                        txtCheckDateTo.SelectionStart = 0;
                        txtCheckDateTo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP29:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPDCRecievableReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PRREP30:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPdcReceivableSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP31:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtCheckNo.Focus();
                    txtCheckNo.SelectionLength = 0;
                    txtCheckNo.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRREP32:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "PRREP33:" + ex.Message;
            }
        }
        #endregion

       
    }
}
