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
    public partial class frmDeliveryNoteReport : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        VoucherTypeSP spVoucherType = new VoucherTypeSP();
        DeliveryNoteMasterSP spDeliveryNoteMaster = new DeliveryNoteMasterSP();
        TransactionsGeneralFill TransactionGenerateFillObj = new TransactionsGeneralFill();
        SalesOrderMasterSP spSalesOrderMaster = new SalesOrderMasterSP();
        SalesQuotationMasterSP spQuotationMaster = new SalesQuotationMasterSP();
        VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
        decimal decVoucherTypes = 0;
        string strTypeOfVoucher = string.Empty;
        #endregion
        #region Functions
        /// <summary>
        /// Create an instance for  frmDeliveryNoteReport class
        /// </summary>
        public frmDeliveryNoteReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                DataTable dtblVoucher = new DataTable();
                dtblVoucher = spVoucherType.VoucherTypeSelectionComboFill("Delivery Note");
                cmbVoucherType.DataSource = dtblVoucher;
                DataRow drawselect = dtblVoucher.NewRow();
                drawselect["voucherTypeId"] = 0;
                drawselect["voucherTypeName"] = "All";
                dtblVoucher.Rows.InsertAt(drawselect, 0);
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash / party combobox
        /// </summary>
        public void CashOrpartyComboFill()
        {
            try
            {
                TransactionGenerateFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashOrParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Salesman combobox
        /// </summary>
        public void SalesmanComboFill()
        {
            try
            {
                DataTable dtblSalesMan = new DataTable();
                dtblSalesMan = TransactionGenerateFillObj.SalesmanViewAllForComboFill(cmbSalesMan, true);
                cmbSalesMan.DataSource = dtblSalesMan;
                cmbSalesMan.ValueMember = "employeeId";
                cmbSalesMan.DisplayMember = "employeeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void GridFill()
        {
            decimal decVoucherTypeId = 0;
            try
            {
                DateTime fromDate = this.dtpFromDate.Value;
                DateTime toDate = this.dtpToDate.Value;
                string strDeliveryMode = cmbDeliveryMode.Text;
                string strInvoiceNo = cmbOrderNo.Text;
                if (cmbCashOrParty.SelectedValue != null && cmbSalesMan.SelectedValue != null)
                {
                    if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbSalesMan.SelectedValue.ToString() != "System.Data.DataRowView" && cmbStatus.Text != "")
                    {
                        if (cmbVoucherType.Text == "All")
                        {
                            decVoucherTypeId = 0;
                        }
                        else
                        {
                            decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                        }
                        if (cmbOrderNo.Text == string.Empty)
                        {
                            strInvoiceNo = "0";
                        }
                        else
                        {
                            strInvoiceNo = cmbOrderNo.Text;
                        }
                        DataTable dtblReport = new DataTable();
                        dtblReport = spDeliveryNoteMaster.DeliveryNoteReportGridFill(fromDate, toDate, Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), Convert.ToDecimal(cmbSalesMan.SelectedValue.ToString()), txtProductCode.Text, txtVoucherNo.Text, decVoucherTypeId, cmbStatus.Text, PublicVariables._inNoOfDecimalPlaces, strDeliveryMode, strInvoiceNo,Convert.ToDecimal(cmbDeliveryMode.SelectedValue.ToString()));
                        dgvDeliveryNoteReport.DataSource = dtblReport;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Voucher combobox
        /// </summary>
        public void VoucherComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                dtbl = spDeliveryNoteMaster.VoucherTypeViewAllCorrespondingToSalesOrderAndSalesQuotation();
                cmbDeliveryMode.DataSource = dtbl;
                DataRow drawselect = dtbl.NewRow();
                drawselect["voucherTypeId"] = 0;
                drawselect["voucherTypeName"] = "All";
                dtbl.Rows.InsertAt(drawselect, 0);
                cmbDeliveryMode.ValueMember = "voucherTypeId";
                cmbDeliveryMode.DisplayMember = "voucherTypeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill AgainstOrder combobox
        /// </summary>
        public void AgainstOrderComboFill()
        {
            try
            {
                bool isEveryComboFill = false;
                SalesOrderMasterSP spSalesOrder = new SalesOrderMasterSP();
                SalesQuotationMasterSP spQuotationMaster = new SalesQuotationMasterSP();
                DataTable dtblOrderFill = new DataTable();
                if (cmbCashOrParty.SelectedValue.ToString() != null && cmbDeliveryMode.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    cmbOrderNo.Text = string.Empty;
                    if (strTypeOfVoucher == "Sales Order")
                    {
                        dtblOrderFill = spSalesOrderMaster.GetSalesOrderInvoiceNumberCorrespondingToLedgerId(Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), Convert.ToDecimal(cmbDeliveryMode.SelectedValue.ToString()));
                        DataRow dr = dtblOrderFill.NewRow();
                        dr[0] = "0";
                        dr[1] = string.Empty;
                        dtblOrderFill.Rows.InsertAt(dr, 0);
                        cmbOrderNo.DataSource = dtblOrderFill;
                        if (dtblOrderFill.Rows.Count > 0)
                        {
                            cmbOrderNo.DisplayMember = "invoiceNo";
                            cmbOrderNo.ValueMember = "salesOrderMasterId";
                            cmbOrderNo.SelectedIndex = 0;
                        }
                    }
                    else if (strTypeOfVoucher == "Sales Quotation")
                    {
                        dtblOrderFill = spQuotationMaster.GetSalesQuotationNumberCorrespondingToLedger(Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), Convert.ToDecimal(cmbDeliveryMode.SelectedValue.ToString()));
                        DataRow dr = dtblOrderFill.NewRow();
                        dr[0] = "0";
                        dr[1] = string.Empty;
                        dtblOrderFill.Rows.InsertAt(dr, 0);
                        cmbOrderNo.DataSource = dtblOrderFill;
                        if (dtblOrderFill.Rows.Count > 0)
                        {
                            cmbOrderNo.DisplayMember = "invoiceNo";
                            cmbOrderNo.ValueMember = "quotationMasterId";
                        }
                    }
                    else
                    {
                        GridFill();
                    }
                }
                isEveryComboFill = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the form controls
        /// </summary>
        public void Clear()
        {
            try
            {
                txtFromDate.Text = dtpFromDate.Value.ToString("dd-MMM-yyyy");
                txtToDate.Text = dtpToDate.Value.ToString("dd-MMM-yyyy");
                cmbVoucherType.Text = "All";
                txtVoucherNo.Text = string.Empty;
                cmbCashOrParty.Text = "All";
                cmbSalesMan.Text = "All";
                cmbDeliveryMode.Text = "All";
                cmbOrderNo.Text = string.Empty;
                txtProductCode.Text = string.Empty;
                cmbStatus.Text = "All";
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP7:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// When form load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDeliveryNoteReport_Load(object sender, EventArgs e)
        {
            try
            {
                cmbStatus.Text = "All";
                CashOrpartyComboFill();
                VoucherComboFill();
                VoucherTypeComboFill();
                SalesmanComboFill();
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP8:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click to view curresponding details to updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDeliveryNoteReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    frmDeliveryNote frmDeliveryNoteObj = new frmDeliveryNote();
                    frmDeliveryNoteObj.MdiParent = formMDI.MDIObj;

                    frmDeliveryNote open = Application.OpenForms["frmDeliveryNote"] as frmDeliveryNote;
                    if (open == null)
                    {
                        frmDeliveryNoteObj.WindowState = FormWindowState.Normal;
                        frmDeliveryNoteObj.MdiParent = formMDI.MDIObj;
                        frmDeliveryNoteObj.CallFromDeliveryNoteReport(this, Convert.ToDecimal(dgvDeliveryNoteReport.CurrentRow.Cells["dgvtxtDeliveryNoteMasterId"].Value.ToString()));
                    }
                    else
                    {
                        open.MdiParent = formMDI.MDIObj;
                        open.BringToFront();
                        open.CallFromDeliveryNoteReport(this, Convert.ToDecimal(dgvDeliveryNoteReport.CurrentRow.Cells["dgvtxtDeliveryNoteMasterId"].Value.ToString()));
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP9:" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click, to print the selected details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            decimal decVoucherTypeId = 0;
            try
            {
                string strDeliveryMode = cmbDeliveryMode.Text;
                string strInvoiceNo = cmbOrderNo.Text;
                DeliveryNoteMasterSP spDeliveryNoteMaster = new DeliveryNoteMasterSP();
                if (dgvDeliveryNoteReport.RowCount > 0)
                {
                    if (cmbVoucherType.Text == "All")
                    {
                        decVoucherTypeId = 0;
                    }
                    else
                    {
                        decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                    }
                    DataSet dsDeliveryNoteReport = spDeliveryNoteMaster.DeliveryNoteReportPrinting(1, Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), decVoucherTypeId, this.dtpFromDate.Value, this.dtpToDate.Value, cmbStatus.Text, strDeliveryMode,strInvoiceNo);
                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.DeliveryNoteReportPrinting(dsDeliveryNoteReport);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP10:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DNREP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the order no combobox  based on the deliverymode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDeliveryMode_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDeliveryMode.SelectedIndex != 0)
                {
                    cmbOrderNo.Enabled = true;
                    DataTable dtbl = new DataTable();
                    if (cmbDeliveryMode.SelectedValue.ToString() != "System.Data.DataRowView")
                    {
                        decVoucherTypes = Convert.ToDecimal(cmbDeliveryMode.SelectedValue.ToString());
                    }
                    if (cmbVoucherType.SelectedValue != null)
                    {
                        infoVoucherType = spVoucherType.TypeOfVoucherBasedOnVoucherTypeId(Convert.ToDecimal(cmbDeliveryMode.SelectedValue.ToString()));
                        strTypeOfVoucher = infoVoucherType.TypeOfVoucher;
                        AgainstOrderComboFill();
                    }
                }
                else
                {
                    cmbOrderNo.DataSource = null;
                    cmbOrderNo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the Fromdate formate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate = this.dtpFromDate.Value;
                txtFromDate.Text = fromDate.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP13:" + ex.Message;
            }
        }
        /// <summary>
        /// /// Function to fill the Todate formate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime toDate = this.dtpToDate.Value;
                txtToDate.Text = toDate.ToString("dd-MMM-yyyy");
                txtToDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and Set the date format
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
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpFromDate.Value = Convert.ToDateTime(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP15:" + ex.Message;
            }
        }
        /// <summary>
        /// For date validation and Set the date format
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
                formMDI.infoError.ErrorString = "DNREP16:" + ex.Message;
            }
        }
        /// <summary>
        /// Butten Search click for grid fill 
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
                formMDI.infoError.ErrorString = "DNREP17:" + ex.Message;
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
                ex.ExportExcel(dgvDeliveryNoteReport, "Delivery Note Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP18:" + ex.Message;
            }
        }
        #endregion
        #region Navigations
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDeliveryNoteReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "DNREP19:" + ex.Message;
            }
        }
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
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP20:" + ex.Message;
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
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP21:" + ex.Message;
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
                    txtVoucherNo.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP22:" + ex.Message;
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
                    cmbCashOrParty.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.TextLength == 0)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP23:" + ex.Message;
            }
        }
        /// <summary>
        ///  For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSalesMan.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP24:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalesMan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDeliveryMode.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbCashOrParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP25:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDeliveryMode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbDeliveryMode.Text != "All")
                    {
                        cmbOrderNo.Focus();
                    }
                    else
                    {
                        txtProductCode.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbSalesMan.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP26:" + ex.Message;
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
                    txtProductCode.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbDeliveryMode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP27:" + ex.Message;
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
                    cmbStatus.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.TextLength == 0)
                    {
                        if (cmbDeliveryMode.Text != "NA")
                        {
                            cmbOrderNo.Focus();
                        }
                        else
                        {
                            cmbDeliveryMode.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP28:" + ex.Message;
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
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtProductCode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP29:" + ex.Message;
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
                    cmbStatus.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP30:" + ex.Message;
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
                    btnPrint.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP31:" + ex.Message;
            }
        }
        private void dgvDeliveryNoteReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvDeliveryNoteReport.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvDeliveryNoteReport.CurrentCell.ColumnIndex, dgvDeliveryNoteReport.CurrentCell.RowIndex);
                        dgvDeliveryNoteReport_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DNREP32:" + ex.Message;
            }
        }
        #endregion

    }
}
