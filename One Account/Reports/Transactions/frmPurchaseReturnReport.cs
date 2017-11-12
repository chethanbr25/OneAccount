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
    public partial class frmPurchaseReturnReport : Form
    {

        #region Public Variable

        string strFrmDate = string.Empty;
        string strToDate = string.Empty;
        decimal decInvoice = 0;
        string strVoucherNo = string.Empty;
        decimal decLedgerID = 0, decVoucherTypeId = 0;
        PurchaseReturnDetailsSP spPurchaseReturnDetails = new PurchaseReturnDetailsSP();
        PurchaseReturnMasterSP spPurchaseReturnMaster = new PurchaseReturnMasterSP();
        PurchaseReturnMasterInfo infoPurchaseReturnDetails = new PurchaseReturnMasterInfo();
        PurchaseReturnDetailsInfo infoPurchaseReturnMaster = new PurchaseReturnDetailsInfo();
        DataTable dtbl = new DataTable();
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        ProductSP spProduct = new ProductSP();
        bool isFromReport = false;
        DataTable dtblReg = new DataTable();

        #endregion

        #region Function
        /// <summary>
        /// Create instance of frmJournalVoucher
        /// </summary>
        public frmPurchaseReturnReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeCombofill()
        {
            try
            {
               
                dtbl = spPurchaseReturnDetails.VoucherTypeComboFillForPurchaseReturn();
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
                
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill CashOrParty combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                
                TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, true);
               

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill InvoiceNo combobox
        /// </summary>
        public void InvoiceNoComboFill()
        {

            decimal decLedgerId = 0;
            decimal decVoucherId = 0;
            try
            {
                DataTable dtbl = new DataTable();
                decLedgerId = (cmbCashOrParty.SelectedIndex == 0 || cmbCashOrParty.SelectedIndex == -1) ? -1 : Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                decVoucherId = (cmbVoucherType.SelectedIndex == 0 || cmbVoucherType.SelectedIndex == -1) ? -1 : Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                dtbl = spPurchaseReturnMaster.GetInvoiceNoCorrespondingtoLedgerForPurchaseReturnReport(decLedgerId, decVoucherId);              
                if (dtbl != null)
                {
                    cmbInvoiceNo.DataSource = dtbl;
                    cmbInvoiceNo.DisplayMember = "invoiceNo";
                    cmbInvoiceNo.ValueMember = "purchaseMasterId";
                    DataRow dr = dtbl.NewRow();
                    dr["purchaseMasterId"] = 0;
                    dr["invoiceNo"] = "All";
                    dtbl.Rows.InsertAt(dr, 0);
                    cmbInvoiceNo.SelectedIndex = 0;

                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT3:" + ex.Message;
            }
        }

       /// <summary>
       /// Function to clear the controls
       /// </summary>
        public void Clear()
        {
            try
            {

                dtpFrmDate.Value = PublicVariables._dtFromDate;
                dtpFrmDate.MinDate = PublicVariables._dtFromDate;
                dtpFrmDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                
                CashOrPartyComboFill();
                VoucherTypeCombofill();

                
             

                PurchaseReturnReportGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void PurchaseReturnReportGridFill()
        {
            string strProductCode = string.Empty;
            try
            {
                if (cmbCashOrParty.SelectedValue != null && cmbInvoiceNo.SelectedValue != null && cmbVoucherType.SelectedValue != null)
                {
                    if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbInvoiceNo.SelectedValue.ToString() != "System.Data.DataRowView" && cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView")
                    {

                        if (txtFromDate.Text != string.Empty)
                        {
                            strFrmDate = txtFromDate.Text;
                        }
                        else
                        {
                            strFrmDate = string.Empty;
                        }
                        if (txtToDate.Text != string.Empty)
                        {
                            strToDate = txtToDate.Text;
                        }
                        else
                        {
                            strToDate = string.Empty;
                        }
                        if (cmbCashOrParty.SelectedIndex > -1)
                        {
                            decLedgerID = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                        }
                        else
                        {
                            decLedgerID = -1;
                        }
                        if (cmbVoucherType.SelectedValue.ToString() != string.Empty || cmbVoucherType.SelectedIndex > -1)
                        {
                            decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                        }
                        else
                        {
                            decVoucherTypeId = -1;
                        }

                        if (cmbInvoiceNo.SelectedValue.ToString() != string.Empty || cmbInvoiceNo.SelectedIndex > -1)
                        {

                            decInvoice = Convert.ToDecimal(cmbInvoiceNo.SelectedValue.ToString());
                        }
                        else
                        {
                            decInvoice = -1;
                        }
                        if (txtVoucherNo.Text != string.Empty)
                        {
                            strVoucherNo = txtVoucherNo.Text;
                        }
                        else
                        {
                            strVoucherNo = string.Empty;
                        }
                        if (txtProductCode.Text != string.Empty)
                        {
                            strProductCode = txtProductCode.Text;
                        }
                        else
                        {
                            strProductCode = string.Empty;
                        }
                        dtblReg = spPurchaseReturnMaster.PurchaseReturnReportGridFill(Convert.ToDateTime(strFrmDate), Convert.ToDateTime(strToDate), decLedgerID, decVoucherTypeId, decInvoice, strProductCode, strVoucherNo);
                        dgvPurchaseReturnReport.DataSource = dtblReg;
                        if (dtblReg.Rows.Count > 0)
                        {
                            decimal decTotal = 0;
                            for (int i = 0; i < dtblReg.Rows.Count; i++)
                            {
                                if (dtblReg.Rows[i]["totalAmount"].ToString() != null)
                                {
                                    decTotal = decTotal + Convert.ToDecimal(dtblReg.Rows[i]["totalAmount"].ToString());
                                }

                            }
                            decTotal = Math.Round(decTotal, 2);
                            txtTotalAmount.Text = decTotal.ToString();
                        }
                        else
                        {
                            txtTotalAmount.Text = "0.00";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT5:" + ex.Message;
            }
        }

        #endregion

        #region Event
        
        /// <summary>
        /// On frmPurchaseReturnReport form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseReturnReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT6:" + ex.Message;
            }
        }
        /// <summary>
        /// On textbox leave event for Data validation
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
                dtpFrmDate.Value = Convert.ToDateTime(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT7:" + ex.Message;
            }
        }
        /// <summary>
        /// On textbox leave event for Data validation
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
                formMDI.infoError.ErrorString = "PRRT8:" + ex.Message;
            }
        }
        /// <summary>
        /// On datetime picker valuechanged event for dtpFrmDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFrmDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFrmDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
              
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT9:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PRRT10:" + ex.Message;
            }
        }
        /// <summary>
        /// On Invoice Number combobox fill while selecting the CashOrParty combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCashOrParty.SelectedIndex > -1)
                {
                    if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashOrParty.Text != "System.Data.DataRowView")
                    {                     
                        InvoiceNoComboFill();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT11:" + ex.Message;
            }
            
        }
        /// <summary>
        /// On call the frmPurchaseReturn form for fill the details of Purchase return
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReturnReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvPurchaseReturnReport.CurrentRow != null)
                {

                    isFromReport = true;
                  
                   
                    if (e.ColumnIndex > -1 && e.RowIndex > -1)
                    {
                        frmPurchaseReturn frmPurchaseReturnObj = new frmPurchaseReturn();
                        frmPurchaseReturnObj.MdiParent = formMDI.MDIObj;
                        frmPurchaseReturn frmPurchaseReturnObj1 = Application.OpenForms["frmPurchaseReturn"] as frmPurchaseReturn;
                        if (frmPurchaseReturnObj1 == null)
                        {
                            frmPurchaseReturnObj.CallFromPurchaseReturnReport(this, Convert.ToDecimal(dgvPurchaseReturnReport.CurrentRow.Cells["dgvtxtPurchaseReturnMasterId"].Value.ToString()), isFromReport);
                        }
                        else
                        {
                            frmPurchaseReturnObj1.CallFromPurchaseReturnReport(this, Convert.ToDecimal(dgvPurchaseReturnReport.CurrentRow.Cells["dgvtxtPurchaseReturnMasterId"].Value.ToString()), isFromReport);
                            frmPurchaseReturnObj1.BringToFront();
                            if (frmPurchaseReturnObj1.WindowState == FormWindowState.Minimized)
                            {
                                frmPurchaseReturnObj1.WindowState = FormWindowState.Normal;
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT12:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click to print the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPurchaseReturnReport.Rows.Count > 0)
                {
                    if (dgvPurchaseReturnReport.Rows.Count > 0)
                    {
                        DataSet dsPurchaseReturnReport = new DataSet();
                        CompanySP spCompany = new CompanySP();
                        DataTable dtblGrid = dtblReg.Copy();
                        DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                        dsPurchaseReturnReport.Tables.Add(dtblGrid);
                        dsPurchaseReturnReport.Tables.Add(dtblCompany);
                        frmReport frmReport = new frmReport();
                        frmReport.MdiParent = formMDI.MDIObj;
                        frmReport.PurchaseReturnReportPrinting(dsPurchaseReturnReport, txtTotalAmount.Text);
                    }
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
           
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT13:" + ex.Message;
            }
            
        }
        /// <summary>
        /// Escape Key navigation of frmPurchaseReturnReport form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseReturnReport_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (e.KeyChar == 27)
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
                formMDI.infoError.ErrorString = "PRRT14:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button click for filter the data
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
                    PurchaseReturnReportGridFill();
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    PurchaseReturnReportGridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT15:" + ex.Message;
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
                txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                CashOrPartyComboFill();
                VoucherTypeCombofill();
             
                txtProductCode.Text = string.Empty;
                txtVoucherNo.Text = string.Empty;

                PurchaseReturnReportGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT16:" + ex.Message;
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
                ex.ExportExcel(dgvPurchaseReturnReport, "Purchase Return Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT17:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Key navigation of text box control for txtFromDate
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
                }              
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT18:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of text box control for txtToDate
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
                    if (txtToDate.Text == string.Empty && txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT19:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of combo box control for cmbVoucherType
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
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT20:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of text box control for txtVoucherNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrParty.Focus();                    
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
                formMDI.infoError.ErrorString = "PRRT21:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of combo box control for cmbCashOrParty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if (e.KeyCode == Keys.Enter)
                {
                    cmbInvoiceNo.Focus();                   
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                    txtVoucherNo.SelectionStart = 0;
                    txtVoucherNo.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT22:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of combo box control for cmbInvoiceNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.Focus();
                    
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbCashOrParty.Focus();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT23:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of combo box control for cmbProductCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              if (e.KeyCode == Keys.Enter)
                {
                    dgvPurchaseReturnReport.Focus();
                    
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbInvoiceNo.Focus();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT24:" + ex.Message;
            }
        }
        /// <summary>
        /// Key navigation of datagridview control for dgvPurchaseReturnReport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReturnReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint_Click(sender, e);
                    
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtProductCode.Focus();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT25:" + ex.Message;
            }
        }
     
        /// <summary>
        /// Key navigation of button control for btnPrint 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT26:" + ex.Message;
            }

        }
        /// <summary>
        /// Key navigation of button control for btnReset 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PRRT27:" + ex.Message;
            }

        }
        /// <summary>
        /// Key navigation of button control for btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtProductCode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRRT28:" + ex.Message;
            }
        }
        #endregion

    }
}
