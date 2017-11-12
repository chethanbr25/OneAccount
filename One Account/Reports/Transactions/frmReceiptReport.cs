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
    public partial class frmReceiptReport : Form
    {
        #region Functions
        /// <summary>
        /// Create an instance for frmReceiptReport class
        /// </summary>
        public frmReceiptReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to print the details
        /// </summary>
        public void Print()
        {
            try
            {
                ReceiptMasterSP SpReceiptMaster = new ReceiptMasterSP();
                DataSet dsReceiptReport = SpReceiptMaster.ReceiptReportPrinting(Convert.ToDateTime(dtpFromDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), Convert.ToDecimal(cmbLedger.SelectedValue), Convert.ToDecimal(cmbVoucherType.SelectedValue), Convert.ToDecimal(cmbCashOrBank.SelectedValue), 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.ReceiptReportPrinting(dsReceiptReport);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash Or Bank combobox
        /// </summary>
        public void CashOrBankComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                dtbl = Obj.BankOrCashComboFill(false);
                DataRow dr = dtbl.NewRow();
                dr[1] = 0;
                dr[0] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbCashOrBank.DataSource = dtbl;
                cmbCashOrBank.ValueMember = "ledgerId";
                cmbCashOrBank.DisplayMember = "ledgerName";
                cmbCashOrBank.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP2:" + ex.Message;
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
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                dtbl = obj.AccountLedgerComboFill();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[2] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbLedger.DataSource = dtbl;
                cmbLedger.ValueMember = "ledgerId";
                cmbLedger.DisplayMember = "ledgerName";
                cmbLedger.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Voucher no combobox
        /// </summary>
        public void VoucherComboFill()
        {
            try
            {
                VoucherTypeSP SpVoucherType = new VoucherTypeSP();
                DataTable dtbl = new DataTable();
                dtbl = SpVoucherType.VoucherTypeViewAll();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid based on the Search condition
        /// </summary>
        public void GridFill()
        {
            try
            {
                ReceiptMasterSP SpPaymentMaster = new ReceiptMasterSP();
                DataTable dtbl = new DataTable();
                if (cmbLedger.Items.Count != 0 && cmbVoucherType.Items.Count != 0 && cmbCashOrBank.Items.Count != 0)
                {
                    if ((cmbLedger.SelectedValue.ToString() != "System.Data.DataRowView") && (cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView") && (cmbCashOrBank.SelectedValue.ToString() != "System.Data.DataRowView"))
                    {
                        if (txtFromDate.Text.Trim() != string.Empty && txtToDate.Text.Trim() != string.Empty)
                        {
                            dtbl = SpPaymentMaster.ReceiptReportSearch(Convert.ToDateTime(dtpFromDate.Value.ToString()), Convert.ToDateTime(dtpToDate.Value.ToString()), Convert.ToDecimal(cmbLedger.SelectedValue), Convert.ToDecimal(cmbVoucherType.SelectedValue), Convert.ToDecimal(cmbCashOrBank.SelectedValue));
                            dgvReceiptReport.DataSource = dtbl;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the controls in form
        /// </summary>
        public void Clear()
        {
            try
            {
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                VoucherComboFill();
                AccountLedgerComboFill();
                CashOrBankComboFill();
                GridFill();
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmReceiptVoucher for updation
        /// </summary>
        /// <param name="frmReceiptVoucher"></param>
        public void CallFromReceiptVoucher(frmReceiptVoucher frmReceiptVoucher)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP7:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// For date validation and set the dtp value as text box value
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
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtFromDate.Text;
                dtpFromDate.Value = Convert.ToDateTime(strdate.ToString());
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP8:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and set the dtp value as text box value
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
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtToDate.Text;
                dtpToDate.Value = Convert.ToDateTime(strdate.ToString());
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP9:" + ex.Message;
            }
        }
        /// <summary>
        /// when Form load  call the clear function to clear the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReceptReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP10:" + ex.Message;
            }
        }
        /// <summary>
        /// Det the textbox value as dtp's selected value
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
                formMDI.infoError.ErrorString = "RRP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Det the textbox value as dtp's selected value
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
                formMDI.infoError.ErrorString = "RRP12:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click for Updation in selected item frmReceiptVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceiptReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvReceiptReport.CurrentRow != null)
                {
                    if (dgvReceiptReport.CurrentRow.Index == e.RowIndex)
                    {
                        if (dgvReceiptReport.CurrentRow.Cells["dgvtxtReceiptMasterId"].Value != null && dgvReceiptReport.CurrentRow.Cells["dgvtxtReceiptMasterId"].Value.ToString() != string.Empty)
                        {
                            frmReceiptVoucher frmReceiptVoucherObj = new frmReceiptVoucher();
                            frmReceiptVoucherObj.MdiParent = formMDI.MDIObj;
                            decimal decReceiptmasterId = Convert.ToDecimal(dgvReceiptReport.CurrentRow.Cells["dgvtxtreceiptMasterId"].Value.ToString());
                            frmReceiptVoucher open = Application.OpenForms["frmReceiptVoucher"] as frmReceiptVoucher;
                            if (open == null)
                            {
                                frmReceiptVoucherObj.WindowState = FormWindowState.Normal;
                                frmReceiptVoucherObj.MdiParent = formMDI.MDIObj;
                                frmReceiptVoucherObj.CallFromReceiptReport(this, decReceiptmasterId);
                            }
                            else
                            {
                                open.MdiParent = formMDI.MDIObj;
                                open.BringToFront();
                                open.CallFromReceiptReport(this, decReceiptmasterId);
                                if (open.WindowState == FormWindowState.Minimized)
                                {
                                    open.WindowState = FormWindowState.Normal;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP13:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click call the gridfill
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
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP14:" + ex.Message;
            }
        }
        /// <summary>
        /// clear button click, call the clear function
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
                formMDI.infoError.ErrorString = "RRP15:" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click, call the Print function 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReceiptReport.RowCount > 0)
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
                formMDI.infoError.ErrorString = "RRP16:" + ex.Message;
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
                ex.ExportExcel(dgvReceiptReport, "Receipt Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP17:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
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
                    txtToDate.SelectionLength = 0;
                    txtToDate.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP18:" + ex.Message;
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
                    cmbVoucherType.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtFromDate.Focus();
                    txtFromDate.SelectionLength = 0;
                    txtFromDate.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP19:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrBank.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP20:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrBank_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbLedger.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP21:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbCashOrBank.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP22:" + ex.Message;
            }
        }
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReceiptReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (Messages.CloseConfirmation())
                        this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RRP23:" + ex.Message;
            }
        }
        #endregion


    }
}
