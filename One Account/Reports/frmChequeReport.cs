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
using System.Collections;
namespace One_Account
{
    public partial class frmChequeReport : Form
    {
        #  region Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        bool isIssued = false;
        string strVoucherType;
        decimal decMasterId;
        #endregion
        #  region Functions
        /// <summary>
        /// Create an Instance of a frmChequeReport class
        /// </summary>
        public frmChequeReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void ChequeReportFillGrid()
        {
            try
            {
                if (rbtnPayed.Checked)
                {
                    isIssued = true;
                }
                else if (rbtnReceived.Checked)
                {
                    isIssued = false;
                }
                decimal decTotalAmount = 0;
                string strTotalAmount = Math.Round(decTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
                PDCPayableMasterSP spPDCPayable = new PDCPayableMasterSP();
                DataTable dtbl = new DataTable();
                decimal decParty = decimal.Parse(cmbParty.SelectedValue.ToString());
                dtbl = spPDCPayable.ChequeReportGridFill(decParty, txtChequeNo.Text, dtpIssueFromDate.Value, dtpIssueToDate.Value, dtpChequeFromDate.Value, dtpChequeToDate.Value, isIssued);
                if (dtbl.Rows.Count > 0)
                {
                    strTotalAmount = dtbl.Compute("Sum(Amount)", string.Empty).ToString();
                    dtbl.Rows.Add();
                    dtbl.Rows[dtbl.Rows.Count - 1]["Amount"] = strTotalAmount;
                    dtbl.Rows[dtbl.Rows.Count - 1]["Party"] = "Total :";
                }
                dgvChequeReport.DataSource = dtbl;
                dgvChequeReport.ClearSelection();
                if (dgvChequeReport.Rows.Count > 0)
                {
                    dgvChequeReport.Columns["ledgerId"].Visible = false;
                    dgvChequeReport.Rows[dgvChequeReport.Rows.Count - 1].DefaultCellStyle.Font = new Font(dgvChequeReport.Font, FontStyle.Bold);
                    dgvChequeReport.Rows[dgvChequeReport.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill party combobox
        /// </summary>
        public void PartyComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                PDCPayableMasterSP spPDCPayable = new PDCPayableMasterSP();
                dtbl = spPDCPayable.ChequeReportPartyComboFill();
                DataRow dr = dtbl.NewRow();
                dr[0] = "All";
                dr[1] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbParty.DataSource = dtbl;
                cmbParty.DisplayMember = "ledgerName";
                cmbParty.ValueMember = "ledgerId";
                cmbParty.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                PartyComboFill();
                cmbParty.SelectedIndex = 0;
                dtpChequeFromDate.Value = PublicVariables._dtFromDate;
                dtpChequeToDate.Value = PublicVariables._dtCurrentDate;
                dtpIssueFromDate.Value = PublicVariables._dtFromDate;
                dtpIssueToDate.Value = PublicVariables._dtCurrentDate;
                txtChequeNo.Text = string.Empty;
                rbtnPayed.Checked = true;
                ChequeReportFillGrid();
                
                cmbParty.Focus();
                
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR3:" + ex.Message;
            }
        }
       
        /// <summary>
        /// Convert DataGridview data to a DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataTable dtblChequeReport = new DataTable();
            try
            {
                dtblChequeReport.Columns.Add("Voucher Type");
                dtblChequeReport.Columns.Add("Voucher No");
                dtblChequeReport.Columns.Add("Issued Date");
                dtblChequeReport.Columns.Add("Party");
                dtblChequeReport.Columns.Add("Amount");
                dtblChequeReport.Columns.Add("Cheque No");
                dtblChequeReport.Columns.Add("Cheque Date");
                DataRow drow = null;
                foreach (DataGridViewRow dr in dgvChequeReport.Rows)
                {
                    drow = dtblChequeReport.NewRow();
                    drow["Voucher Type"] = dr.Cells["VoucherType"].Value;
                    drow["Voucher No"] = dr.Cells["VoucherNo"].Value;
                    drow["Issued Date"] = dr.Cells["IssuedDate"].Value;
                    drow["Amount"] = dr.Cells["Amount"].Value;
                    drow["Party"] = dr.Cells["Party"].Value;
                    drow["Cheque No"] = dr.Cells["ChequeNo"].Value;
                    drow["Cheque Date"] = dr.Cells["ChequeDate"].Value;
                    dtblChequeReport.Rows.Add(drow);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR4:" + ex.Message;
            }
            return dtblChequeReport;
        }
       
        /// <summary>
        /// Convert DataTable to DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            FinancialStatementSP spfinancial = new FinancialStatementSP();
            DataSet dsChequeReport = new DataSet();
            DataTable dtblChequeReport = GetDataTable();
            DataTable dtblCompany = new DataTable();
            try
            {
                dtblCompany = spfinancial.CashflowReportPrintCompany(1);//(PublicVariables._decCurrentCompanyId);
                dsChequeReport.Tables.Add(dtblChequeReport);
                dsChequeReport.Tables.Add(dtblCompany);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR5:" + ex.Message;
            }
            return dsChequeReport;
        }
         
        /// <summary>
        /// Print  For   CrystalReport
        /// </summary>
        public void Print()
        {
            try
            {
                DataSet dsChequeReport = GetDataSet();
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.ChequeReportPrinting(dsChequeReport);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR6:" + ex.Message;
            }
        }
        #endregion
        #region Events
        
        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChequeReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR7:" + ex.Message;
            }
        }
        /// <summary>
        /// On search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIssueFromDate.Text != string.Empty && txtIssueToDate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtIssueToDate.Text) < Convert.ToDateTime(txtIssueFromDate.Text))
                    {
                        MessageBox.Show("IssueTodate should be greater than Issuefromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtIssueToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        txtIssueFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                        DateTime dt;
                        DateTime.TryParse(txtIssueToDate.Text, out dt);
                        dtpIssueToDate.Value = dt;
                        dtpIssueFromDate.Value = dt;
                    }
                }
                else if (txtIssueFromDate.Text == string.Empty)
                {
                    txtIssueFromDate.Text = PublicVariables._dtFromDate.ToString();
                    txtIssueToDate.Text = PublicVariables._dtCurrentDate.ToString();
                    DateTime dt;
                    DateTime.TryParse(txtIssueToDate.Text, out dt);
                    dtpIssueToDate.Value = dt;
                    dtpIssueFromDate.Value = dt;
                }
                if (txtChequeFromDate.Text != string.Empty && txtChequeToDate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtChequeToDate.Text) < Convert.ToDateTime(txtChequeFromDate.Text))
                    {
                        MessageBox.Show("ChequeTodate should be greater than Chequefromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtChequeToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        txtChequeFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                        DateTime dt;
                        DateTime.TryParse(txtIssueToDate.Text, out dt);
                        dtpChequeFromDate.Value = dt;
                        dtpChequeToDate.Value = dt;
                    }
                }
                else if (txtIssueFromDate.Text == string.Empty)
                {
                    txtChequeFromDate.Text = PublicVariables._dtFromDate.ToString();
                    txtChequeToDate.Text = PublicVariables._dtCurrentDate.ToString();
                    DateTime dt;
                    DateTime.TryParse(txtChequeToDate.Text, out dt);
                    dtpChequeToDate.Value = dt;
                    dtpChequeFromDate.Value = dt;
                }
                ChequeReportFillGrid();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR8:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtIssueFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIssueFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation DateValidationObj = new DateValidation();
                DateValidationObj.DateValidationFunction(txtIssueFromDate);
                if (txtIssueFromDate.Text == string.Empty)
                {
                    txtIssueFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strDate = txtIssueFromDate.Text;
                dtpIssueFromDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR9:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtIssueToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIssueToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation DateValidationObj = new DateValidation();
                DateValidationObj.DateValidationFunction(txtIssueToDate);
                if (txtIssueToDate.Text == string.Empty)
                {
                    txtIssueToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strDate = txtIssueToDate.Text;
                dtpIssueToDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR10:" + ex.Message;
            }
        }
        /// <summary>
        /// On valuechange of dtpIssueFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpIssueFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpIssueFromDate.Value;
                this.txtIssueFromDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR11:" + ex.Message;
            }
        }
        /// <summary>
        /// On valuechange of dtpIssueToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpIssueToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpIssueToDate.Value;
                this.txtIssueToDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR12:" + ex.Message;
            }
        }
        /// <summary>
        /// On valuechange of dtpChequeFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpChequeFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpChequeFromDate.Value;
                this.txtChequeFromDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR13:" + ex.Message;
            }
        }
        /// <summary>
        /// On valuechange of dtpChequeToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpChequeToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpChequeToDate.Value;
                this.txtChequeToDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR14:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtChequeFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation DateValidationObj = new DateValidation();
                DateValidationObj.DateValidationFunction(txtChequeFromDate);
                if (txtChequeFromDate.Text == string.Empty)
                {
                    txtChequeFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strDate = txtChequeFromDate.Text;
                dtpChequeFromDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR15:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtChequeToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation DateValidationObj = new DateValidation();
                DateValidationObj.DateValidationFunction(txtChequeToDate);
                if (txtChequeToDate.Text == string.Empty)
                {
                    txtChequeToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strDate = txtChequeToDate.Text;
                dtpChequeToDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR16:" + ex.Message;
            }
        }
        /// <summary>
        /// On reset button click
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
                formMDI.infoError.ErrorString = "RCR17:" + ex.Message;
            }
        }
        /// <summary>
        /// On print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChequeReport.Rows.Count > 0)
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
                formMDI.infoError.ErrorString = "RCR18:" + ex.Message;
            }
        }
        /// <summary>
        /// When doubleclicking on the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvChequeReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvChequeReport.CurrentRow.Index == e.RowIndex)
                    {
                        int inI = dgvChequeReport.CurrentCell.RowIndex;
                        if (dgvChequeReport.Rows[inI].Cells["dgvtxtMasterId"].Value != null && dgvChequeReport.Rows[inI].Cells["dgvtxtMasterId"].Value.ToString() != string.Empty)
                        {
                            strVoucherType = dgvChequeReport.Rows[inI].Cells["VoucherType"].Value.ToString();
                            decMasterId = Convert.ToDecimal(dgvChequeReport.Rows[inI].Cells["dgvtxtMasterId"].Value.ToString());
                            if (strVoucherType == "Contra Voucher")
                            {
                                frmContraVoucher frmContraVoucher = new frmContraVoucher();
                                frmContraVoucher = Application.OpenForms["frmContraVoucher"] as frmContraVoucher;
                                if (frmContraVoucher == null)
                                {
                                    frmContraVoucher = new frmContraVoucher();
                                    frmContraVoucher.MdiParent = formMDI.MDIObj;
                                    frmContraVoucher.CallFromChequeReport(this, decMasterId);
                                }
                            }
                            else if (strVoucherType == "Payment Voucher")
                            {
                                frmPaymentVoucher frmPaymentVoucher = new frmPaymentVoucher();
                                frmPaymentVoucher = Application.OpenForms["frmPaymentVoucher"] as frmPaymentVoucher;
                                if (frmPaymentVoucher == null)
                                {
                                    frmPaymentVoucher = new frmPaymentVoucher();
                                    frmPaymentVoucher.MdiParent = formMDI.MDIObj;
                                    frmPaymentVoucher.CallFromChequeReport(this, decMasterId);
                                }
                            }
                            else if (strVoucherType == "Receipt Voucher")
                            {
                                frmReceiptVoucher frmReceiptVoucher = new frmReceiptVoucher();
                                frmReceiptVoucher = Application.OpenForms["frmReceiptVoucher"] as frmReceiptVoucher;
                                if (frmReceiptVoucher == null)
                                {
                                    frmReceiptVoucher = new frmReceiptVoucher();
                                    frmReceiptVoucher.MdiParent = formMDI.MDIObj;
                                    frmReceiptVoucher.CallFromChequeReport(this, decMasterId);
                                }
                            }
                            else if (strVoucherType == "PDC Receivable")
                            {
                                frmPdcReceivable frmPdcReceivable = new frmPdcReceivable();
                                frmPdcReceivable = Application.OpenForms["frmPdcReceivable"] as frmPdcReceivable;
                                if (frmPdcReceivable == null)
                                {
                                    frmPdcReceivable = new frmPdcReceivable();
                                    frmPdcReceivable.MdiParent = formMDI.MDIObj;
                                    frmPdcReceivable.CallFromChequeReport(this, decMasterId);
                                }
                            }
                            else if (strVoucherType == "PDC Payable")
                            {
                                frmPdcPayable frmPdcPayable = new frmPdcPayable();
                                frmPdcPayable = Application.OpenForms["frmPdcPayable"] as frmPdcPayable;
                                if (frmPdcPayable == null)
                                {
                                    frmPdcPayable = new frmPdcPayable();
                                    frmPdcPayable.MdiParent = formMDI.MDIObj;
                                    frmPdcPayable.CallFromChequeReport(this, decMasterId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR19:" + ex.Message;
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
                ex.ExportExcel(dgvChequeReport, "Cheque Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR20:" + ex.Message;
            }
        }
        # endregion
        #region navigation
        /// <summary>
        /// Enterkey and backspace navigation of cmbParty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtChequeNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtChequeNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtIssueFromDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtChequeNo.Text.Trim() == string.Empty || txtChequeNo.SelectionStart == 0)
                    {
                        cmbParty.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtIssueToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIssueToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtChequeFromDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtIssueToDate.Text == string.Empty || txtIssueToDate.SelectionStart == 0)
                    {
                        txtIssueFromDate.Focus();
                        txtIssueFromDate.SelectionStart = 0;
                        txtIssueFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtIssueFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIssueFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtIssueToDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtIssueFromDate.Text == string.Empty || txtIssueFromDate.SelectionStart == 0)
                    {
                        txtChequeNo.Focus();
                        txtChequeNo.SelectionStart = 0;
                        txtChequeNo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtChequeFromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtChequeToDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtChequeFromDate.Text == string.Empty || txtChequeFromDate.SelectionStart == 0)
                    {
                        txtIssueToDate.Focus();
                        txtIssueToDate.SelectionStart = 0;
                        txtIssueToDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtChequeToDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtChequeToDate.Text == string.Empty || txtChequeToDate.SelectionStart == 0)
                    {
                        txtChequeFromDate.Focus();
                        txtChequeFromDate.SelectionStart = 0;
                        txtChequeFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RCR26:" + ex.Message;
            }
        }
        /// <summary>
        /// For shortcut keys
        /// Esc for formclosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChequeReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "RCR27:" + ex.Message;
            }
        }
        #endregion      
       
    }
}
