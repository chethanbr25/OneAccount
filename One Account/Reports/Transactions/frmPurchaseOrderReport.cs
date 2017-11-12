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
    public partial class frmPurchaseOrderReport : Form
    {
        #region Public variables
        string strInvoiceNo = string.Empty;
        string strStatus = string.Empty;
        decimal decLedgerId = 0;
        decimal decVoucherTypeId = 0;
        bool isPendingOrder = false;
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        PurchaseOrderDetailsSP spPurchaseOrderDetails = new PurchaseOrderDetailsSP();
         DataTable dtblPurchaseOrderRegister = new DataTable();
        DataTable dtbl = new DataTable();
       
        #endregion
        #region Functions
        /// <summary>
        /// Create instance of frmJournalVoucher
        /// </summary>
        public frmPurchaseOrderReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// On 'Clear' button click for clear the controls
        /// </summary>
        public void Clear()
        {
            try
            {
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtToDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                txtFromDate.Select();
                CashOrPartyComboFill();
                VoucherTypeCombofill();
                cmbStatus.SelectedIndex = 0;
                txtVoucherNo.Text = string.Empty;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill CashOrParty combo box
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                        TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill VoucherType combo box
        /// </summary>
        public void VoucherTypeCombofill()
        {
            try
            {
                        dtbl = spPurchaseOrderDetails.VoucherTypeCombofillforPurchaseOrderReport();
                        cmbVoucherType.DataSource = dtbl;
                        cmbVoucherType.ValueMember = "voucherTypeId";
                        cmbVoucherType.DisplayMember = "voucherTypeName";
                        cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill gridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
                if (txtVoucherNo.Text.Trim() == string.Empty)
                {
                    strInvoiceNo = "-1";
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
                dtblPurchaseOrderRegister = spPurchaseOrderMaster.PurchaseOrdeReportViewAll(strInvoiceNo, decLedgerId, decVoucherTypeId, FromDate, ToDate, strStatus);
           
                if (dtblPurchaseOrderRegister.Rows.Count > 0)
                {
                    decimal decTotal = 0;
                    for (int i = 0; i < dtblPurchaseOrderRegister.Rows.Count; i++)
                    {
                        if (dtblPurchaseOrderRegister.Rows[i]["totalAmount"].ToString() != null)
                        {
                            decTotal = decTotal + Convert.ToDecimal(dtblPurchaseOrderRegister.Rows[i]["totalAmount"].ToString());
                        }
                    }
                    decTotal = Math.Round(decTotal, 2);
                    txtTotalAmount.Text = decTotal.ToString();
                }
                else
                {
                    txtTotalAmount.Text = "0.00";
                }
                dgvPurchaseOrderReport.DataSource = dtblPurchaseOrderRegister;
                if (dgvPurchaseOrderReport.Columns.Count > 0)
                {
                    dgvPurchaseOrderReport.Columns["dgvtxtTotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP4:" + ex.Message;
            }
        }
       
        #endregion
        #region Events
        /// <summary>
        /// On 'Print' button click to print the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPurchaseOrderReport.Rows.Count > 0)
                {
                    PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
                    DataSet dsPurchaseOrderReport = spPurchaseOrderMaster.PurchaseOrderReportPrinting(1, strInvoiceNo, decLedgerId, decVoucherTypeId, this.dtpFromDate.Value, this.dtpToDate.Value, strStatus);
                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.PurchaseOrderReportPrinting(dsPurchaseOrderReport, txtTotalAmount.Text);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP5:" + ex.Message;
            }
        }
        /// <summary>
        /// On datetime picker valuechanged event for dtpFromDate
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
                formMDI.infoError.ErrorString = "PORP6:" + ex.Message;
            }
        }
        /// <summary>
        /// On frmPurchaseOrderReport form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseOrderReport_Load(object sender, EventArgs e)
        {
            try
            {
               Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP7:" + ex.Message;
            }
        }
        /// <summary>
        /// On datetime picker valuechanged event for dtpToDate
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
                formMDI.infoError.ErrorString = "PORP8:" + ex.Message;
            }
        } 
        /// <summary>
        /// On textbox leave event for Date validation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation ObjValidation = new DateValidation();
                ObjValidation.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                }
                DateTime dt;
                DateTime.TryParse(txtFromDate.Text, out dt);
                dtpFromDate.Value = dt;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP9:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button click
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
                    GridFill();
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
                formMDI.infoError.ErrorString = "PORP10:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview cell double click event to get the masterId for updation and deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseOrderReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvPurchaseOrderReport.CurrentRow != null && dgvPurchaseOrderReport.CurrentRow.Cells["dgvtxtPurchaseOrderMasterId"].Value != null && dgvPurchaseOrderReport.CurrentRow.Cells["dgvtxtPurchaseOrderMasterId"].Value.ToString() != "")
                {
                    frmPurchaseOrder frmPurchaseOrderObj = new frmPurchaseOrder();
                    frmPurchaseOrderObj.MdiParent = formMDI.MDIObj;
                    frmPurchaseOrder open = Application.OpenForms["frmPurchaseOrder"] as frmPurchaseOrder;
                    if (open == null)
                    {
                        frmPurchaseOrderObj.WindowState = FormWindowState.Normal;
                        frmPurchaseOrderObj.MdiParent = formMDI.MDIObj;
                        frmPurchaseOrderObj.CallFromPurchaseOrderReport(this, Convert.ToDecimal(dgvPurchaseOrderReport.CurrentRow.Cells["dgvtxtPurchaseOrderMasterId"].Value.ToString()), isPendingOrder);
               
                    }
                    else
                    {
                        open.CallFromPurchaseOrderReport(this, Convert.ToDecimal(dgvPurchaseOrderReport.CurrentRow.Cells["dgvtxtPurchaseOrderMasterId"].Value.ToString()), isPendingOrder);
               
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP11:" + ex.Message;
            }
        }
        /// <summary>
        /// On textbox leave event for Date validation 
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
                formMDI.infoError.ErrorString = "PORP12:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Reset' button click to clear the controls
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
                formMDI.infoError.ErrorString = "PORP13:" + ex.Message;
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
                ex.ExportExcel(dgvPurchaseOrderReport, "Purchase Order Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP14:" + ex.Message;
            }
        }
        #endregion
        #region Navigations
        /// <summary>
        /// Key Navigation for txtFromDate textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtToDate.Enabled)
                    {
                        txtToDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP15:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation for txtToDate textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
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
                    if (txtFromDate.Enabled)
                    {
                        if (txtToDate.Text == string.Empty || txtToDate.SelectionStart == 0)
                        {
                            txtFromDate.Focus();
                            txtFromDate.SelectionLength = 0;
                            txtFromDate.SelectionStart = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP16:" + ex.Message;
            }
        }
        /// <summary>
        /// Key Navigation for cmbCashOrParty combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
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
                    if (txtToDate.Enabled)
                    {
                        txtToDate.Focus();
                        txtToDate.SelectionLength = 0;
                        txtToDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP17:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation for cmbStatus combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbVoucherType.Enabled)
                    {
                        cmbVoucherType.Focus();
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
                formMDI.infoError.ErrorString = "PORP18:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation for cmbVoucherType combobox
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
                    if (cmbStatus.Enabled)
                    {
                        cmbStatus.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP19:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation for cmbVoucherNo combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvPurchaseOrderReport.Enabled)
                    {
                        dgvPurchaseOrderReport.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbVoucherType.Enabled)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PORP20:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation for frmPurchaseOrderReport form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseOrderReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PORP21:" + ex.Message;
            }
        }
        #endregion

     
    }
}
