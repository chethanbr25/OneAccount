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
    public partial class frmMaterialReceiptReport : Form
    {
        #region Public Variables
        string strOrderNo = string.Empty;
        string strInvoiceNo = string.Empty;
        string strproductCode = string.Empty;
        string strStatus = string.Empty;
        decimal decLedgerId = 0;
        decimal decVoucherTypeId = 0;
        decimal decOrderId = 0;
        bool isDontExecuteCashorParty = false;
        DataTable dtbl = new DataTable();
        #endregion
        #region Functions
        /// <summary>
        /// Create an instance for frmMaterialReceiptReport class
        /// </summary>
        public frmMaterialReceiptReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Clear function to reset the form controls
        /// </summary>
        public void Clear()
        {
            try
            {
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                CashOrPartyComboFill();
                VoucherTypeCombofill();
                cmbStatus.SelectedIndex = 0;
                txtVoucherNo.Text = string.Empty;
                txtProductCode.Text = string.Empty;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP1:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeCombofill()
        {
            try
            {
                MaterialReceiptDetailsSP spMaterialReceiptDetails = new MaterialReceiptDetailsSP();
                dtbl = spMaterialReceiptDetails.VoucherTypeCombofillforMaterialReceipt();
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP2:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill Cash Or Party combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
                isDontExecuteCashorParty = true;
                TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, true);
                isDontExecuteCashorParty = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP3:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill OrderNo combobox
        /// </summary>
        /// <param name="decLedger"></param>
        public void OrderNoComboFill(decimal decLedger)
        {
            try
            {
                DataTable dtbl = new DataTable();
                MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
                dtbl = spMaterialReceiptMaster.GetOrderNoCorrespondingtoLedgerForMaterialReceiptRpt(Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()));
                DataRow drow = dtbl.NewRow();
                drow["invoiceNo"] = "All";
                drow["purchaseOrderMasterId"] = -1;
                dtbl.Rows.InsertAt(drow, 0);
                if (dtbl != null)
                {
                    cmbOrderNo.DataSource = dtbl;
                    cmbOrderNo.DisplayMember = "invoiceNo";
                    cmbOrderNo.ValueMember = "purchaseOrderMasterId";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid based on the search condition
        /// </summary>
        public void GridFill()
        {
            try
            {
                DataTable dtblMaterialReceiptRegister = new DataTable();
                MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
                if (cmbOrderNo.SelectedIndex == 0 || cmbOrderNo.SelectedIndex == -1)
                {
                    decOrderId = -1;
                }
                else
                {
                    decOrderId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                if (txtVoucherNo.Text.Trim() == string.Empty)
                {
                    strInvoiceNo = string.Empty;
                }
                else
                {
                    strInvoiceNo = txtVoucherNo.Text;
                }
                if (cmbCashOrParty.SelectedIndex == 0 || cmbCashOrParty.SelectedIndex == -1)
                {
                    decLedgerId = -1;
                }
                else
                {
                    decLedgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                }
                if (cmbVoucherType.SelectedIndex == 0 || cmbVoucherType.SelectedIndex == -1)
                {
                    decVoucherTypeId = -1;
                }
                else
                {
                    decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                }
                if (txtProductCode.Text.Trim() == string.Empty)
                {
                    strproductCode = string.Empty;
                }
                else
                {
                    strproductCode = txtProductCode.Text;
                }
                if (cmbStatus.SelectedIndex == 0 || cmbStatus.SelectedIndex == -1)
                {
                    strStatus = "All";
                }
                else
                {
                    strStatus = cmbStatus.SelectedItem.ToString();
                }
                DateTime FromDate = this.dtpFromDate.Value;
                DateTime ToDate = this.dtpToDate.Value;
                dtblMaterialReceiptRegister = spMaterialReceiptMaster.MaterialReceiptReportViewAll(decOrderId, strInvoiceNo, decLedgerId, decVoucherTypeId, strproductCode, FromDate, ToDate, strStatus);
                dgvMaterialReceiptReport.DataSource = dtblMaterialReceiptRegister;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP5:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMaterialReceiptReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP6:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set the new value into datetime picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objVal = new DateValidation();
                bool isInvalid = objVal.DateValidationFunction(txtFromDate);
                if (!isInvalid)
                {
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                dtpFromDate.Value = Convert.ToDateTime(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP7:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set the new value into datetime picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objVal = new DateValidation();
                bool isInvalid = objVal.DateValidationFunction(txtToDate);
                if (!isInvalid)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP8:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the dtp value into textbox, if its chsnged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtToDate.Text = date.ToString("dd-MMM-yyyy");
                txtToDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP9:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the dtp value into textbox, if its chsnged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFromDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP10:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the order no combofill based on the cash or party
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbOrderNo.DataSource = null;
                if (!isDontExecuteCashorParty)
                {
                    if (cmbCashOrParty.SelectedValue != null)
                    {
                        OrderNoComboFill(Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click, call the gridfill function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP12:" + ex.Message;
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
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP13:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click to open details in form frmMaterialReceipt to updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMaterialReceiptReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvMaterialReceiptReport.CurrentRow != null && dgvMaterialReceiptReport.CurrentRow.Cells["dgvtxtMaterialReceiptMasterId"].Value != null && dgvMaterialReceiptReport.CurrentRow.Cells["dgvtxtMaterialReceiptMasterId"].Value.ToString() != string.Empty)
                    {
                        if (dgvMaterialReceiptReport.CurrentRow != null)
                        {
                            frmMaterialReceipt frmmaterialReceiptObj = new frmMaterialReceipt();
                            frmMaterialReceipt frmmaterialReceiptOpen = Application.OpenForms["frmmaterialReceipt"] as frmMaterialReceipt;
                            if (frmmaterialReceiptOpen == null)
                            {
                                frmmaterialReceiptObj.MdiParent = formMDI.MDIObj;
                                frmmaterialReceiptObj.CallFromMaterialReceiptReport(this, Convert.ToDecimal(dgvMaterialReceiptReport.CurrentRow.Cells["dgvtxtMaterialReceiptMasterId"].Value.ToString()), Convert.ToDecimal(dgvMaterialReceiptReport.CurrentRow.Cells["dgvtxtPOVoucherTypeId"].Value.ToString()));
                            }
                            else
                            {
                                frmmaterialReceiptOpen.CallFromMaterialReceiptReport(this, Convert.ToDecimal(dgvMaterialReceiptReport.CurrentRow.Cells["dgvtxtMaterialReceiptMasterId"].Value.ToString()), Convert.ToDecimal(dgvMaterialReceiptReport.CurrentRow.Cells["dgvtxtPOVoucherTypeId"].Value.ToString()));
                                frmmaterialReceiptOpen.BringToFront();
                                if (frmmaterialReceiptOpen.WindowState == FormWindowState.Minimized)
                                {
                                    frmmaterialReceiptOpen.WindowState = FormWindowState.Normal;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMaterialReceiptReport.RowCount > 0)
                {
                    MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
                    DataSet dsMaterialReceiptReport = spMaterialReceiptMaster.MaterialReceiptReportPrinting(1, strInvoiceNo, strStatus, decLedgerId, strproductCode, decVoucherTypeId, this.dtpFromDate.Value, this.dtpToDate.Value, decOrderId);
                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.MaterialReceiptReportPrinting(dsMaterialReceiptReport);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP15:" + ex.Message;
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
                ex.ExportExcel(dgvMaterialReceiptReport, "Material Receipt Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP16:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// For enter key navigation
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
                formMDI.infoError.ErrorString = "MRREP17:" + ex.Message;
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
                formMDI.infoError.ErrorString = "MRREP18:" + ex.Message;
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
                    if (txtVoucherNo.Enabled)
                    {
                        txtVoucherNo.Focus();
                    }
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
                formMDI.infoError.ErrorString = "MRREP19:" + ex.Message;
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
                    if (cmbCashOrParty.Enabled)
                    {
                        cmbCashOrParty.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Text == string.Empty || txtVoucherNo.SelectionStart == 0)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP20:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbOrderNo.Enabled)
                    {
                        cmbOrderNo.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Enabled)
                    {
                        txtVoucherNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP21:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOrderNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbStatus.Enabled)
                    {
                        cmbStatus.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbCashOrParty.Enabled)
                    {
                        cmbCashOrParty.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP22:" + ex.Message;
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
                    if (txtProductCode.Enabled)
                    {
                        txtProductCode.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbOrderNo.Enabled)
                    {
                        cmbOrderNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP23:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (btnSearch.Enabled)
                    {
                        btnSearch.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        cmbStatus.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP24:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMaterialReceiptReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP25:" + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {
                    btnReset.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Enabled)
                    {
                        txtProductCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP26:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvMaterialReceiptReport.Enabled)
                    {
                        dgvMaterialReceiptReport.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MRREP27:" + ex.Message;
            }
        }
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMaterialReceiptReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "MRREP28:" + ex.Message;
            }
        }
        #endregion

    }
}
