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
    public partial class frmSalesReport : Form
    {
        #region Publicvariables
        /// <summary>
        /// Public varaible declaration part
        /// </summary>
        SalesDetailsSP spSalesDetails = new SalesDetailsSP();
        SalesMasterSP spSalesMaster = new SalesMasterSP();
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        int maxSerialNo = 0;
        int inCurrenRowIndex = 0;// To keep row index while returning from voucher
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmSalesReport class
        /// </summary>
        public frmSalesReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                VoucherTypeComboFill();
                CashorPartyComboFill();
                salesManComboFill();
                ModelNoComboFill();
                AreaComboFill();
                RouteComboFill();
                dtpFrmDate.Value = PublicVariables._dtFromDate;
                dtpFrmDate.MinDate = PublicVariables._dtFromDate;
                dtpFrmDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                cmbVoucherType.SelectedIndex = 0;
                cmbCashOrParty.SelectedIndex = 0;
                cmbArea.SelectedIndex = 0;
                cmbModelNo.SelectedIndex = 0;
                cmbRoute.SelectedIndex = 0;
                cmbSalesMan.SelectedIndex = 0;
                cmbSalesMode.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                txtProductCode.Text = string.Empty;
                txtVoucherNo.Text = string.Empty;
                txtProductName.Text = string.Empty;
                gridFill();
                txtFromDate.Select();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            DataTable dtbl = new DataTable();
            try
            {
                dtbl = spSalesDetails.VoucherTypeNameComboFillForSalesInvoiceRegister();
                cmbVoucherType.DataSource = dtbl;
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash/party combobox
        /// </summary>
        public void CashorPartyComboFill()
        {
            try
            {
                TransactionGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashOrParty, true);
                cmbCashOrParty.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Salesman combobox
        /// </summary>
        public void salesManComboFill()
        {
            try
            {
                TransactionGeneralFillObj.SalesmanViewAllForComboFill(cmbSalesMan, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill ModelNo combobox
        /// </summary>
        public void ModelNoComboFill()
        {
            try
            {
                TransactionGeneralFillObj.ModelNoViewAll(cmbModelNo, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Area combobox
        /// </summary>
        public void AreaComboFill()
        {
            try
            {
                TransactionGeneralFillObj.AreaViewAll(cmbArea, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Route combobox
        /// </summary>
        public void RouteComboFill()
        {
            try
            {
                TransactionGeneralFillObj.RouteViewAll(cmbRoute, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void gridFill()
        {
            try
            {
                decimal decVoucherTypeId = 0;
                decimal decLedgerId = 0;
                decimal decRouteId = 0;
                decimal decAreaId = 0;
                decimal decEmployeeId = 0;
                decimal decModelNo = 0;
                string strStatus = string.Empty;
                string strProductName = string.Empty;
                string strProductCode = string.Empty;
                string strVoucherNo = string.Empty;
                string strSalesMode = string.Empty;
                DateTime dtFromdate = this.dtpFrmDate.Value;
                DateTime dtTodate = this.dtpToDate.Value;
                if (cmbVoucherType.SelectedIndex > -1)
                {
                    if (cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView" && cmbVoucherType.Text != "System.Data.DataRowView")
                    {
                        if (cmbVoucherType.SelectedIndex == 0 || cmbVoucherType.SelectedIndex == -1)
                        {
                            decVoucherTypeId = 0;
                        }
                        else
                        {
                            decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                        }
                    }
                }
                if (cmbCashOrParty.SelectedIndex > -1)
                {
                    if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashOrParty.Text != "System.Data.DataRowView")
                    {
                        if (cmbCashOrParty.SelectedIndex == 0 || cmbCashOrParty.SelectedIndex == -1)
                        {
                            decLedgerId = 0;
                        }
                        else
                        {
                            decLedgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                        }
                    }
                }
                if (cmbArea.SelectedIndex > -1)
                {
                    if (cmbArea.SelectedValue.ToString() != "System.Data.DataRowView" && cmbArea.Text != "System.Data.DataRowView")
                    {
                        if (cmbArea.SelectedIndex == 0 || cmbArea.SelectedIndex == -1)
                        {
                            decAreaId = 0;
                        }
                        else
                        {
                            decAreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                        }
                    }
                }
                if (cmbSalesMan.SelectedIndex > -1)
                {
                    if (cmbSalesMan.SelectedValue.ToString() != "System.Data.DataRowView" && cmbSalesMan.Text != "System.Data.DataRowView")
                    {
                        if (cmbSalesMan.SelectedIndex == 0 || cmbSalesMan.SelectedIndex == -1)
                        {
                            decEmployeeId = 0;
                        }
                        else
                        {
                            decEmployeeId = Convert.ToDecimal(cmbSalesMan.SelectedValue.ToString());
                        }
                    }
                }
                if (cmbRoute.SelectedIndex > -1)
                {
                    if (cmbRoute.SelectedValue.ToString() != "System.Data.DataRowView" && cmbRoute.Text != "System.Data.DataRowView")
                    {
                        if (cmbRoute.SelectedIndex == 0 || cmbRoute.SelectedIndex == -1)
                        {
                            decRouteId = 0;
                        }
                        else
                        {
                            decRouteId = Convert.ToDecimal(cmbRoute.SelectedValue.ToString());
                        }
                    }
                }
                if (cmbModelNo.SelectedIndex > -1)
                {
                    if (cmbModelNo.SelectedValue.ToString() != "System.Data.DataRowView" && cmbRoute.Text != "System.Data.DataRowView")
                    {
                        if (cmbModelNo.SelectedIndex == 0 || cmbModelNo.SelectedIndex == -1)
                        {
                            decModelNo = 0;
                        }
                        else
                        {
                            decModelNo = Convert.ToDecimal(cmbModelNo.SelectedValue.ToString());
                        }
                    }
                }
                if (txtVoucherNo.Text == "")
                {
                    strVoucherNo = string.Empty;
                }
                else
                {
                    strVoucherNo = txtVoucherNo.Text;
                }
                if (txtProductCode.Text == "")
                {
                    strProductCode = string.Empty;
                }
                else
                {
                    strProductCode = txtProductCode.Text;
                }
                if (txtProductName.Text == "")
                {
                    strProductName = string.Empty;
                }
                else
                {
                    strProductName = txtProductName.Text;
                }
                if (cmbSalesMode.SelectedIndex == 0 || cmbSalesMode.SelectedIndex == -1)
                {
                    strSalesMode = "All";
                }
                else
                {
                    strSalesMode = cmbSalesMode.Text;
                }
                if (cmbStatus.SelectedIndex == 0 || cmbStatus.SelectedIndex == -1)
                {
                    strStatus = "All";
                }
                else
                {
                    strStatus = cmbStatus.Text;
                }
                DataTable dtbl = new DataTable();
                dtbl = spSalesMaster.SalesInvoiceReportFill(dtFromdate, dtTodate, decVoucherTypeId, decLedgerId, decAreaId, strSalesMode, decEmployeeId, strProductName, strVoucherNo, strStatus, decRouteId, decModelNo, strProductCode);
                dgvSIReport.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate serial number in datagridview
        /// </summary>
        public void SerialNo()
        {
            try
            {
                int inCount = 1;
                foreach (DataGridViewRow row in dgvSIReport.Rows)
                {
                    row.Cells["dgvtxtSlNo"].Value = inCount.ToString();
                    inCount++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT9:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Datevalidation
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
                dtpFrmDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT10:" + ex.Message;
            }
        }
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
                formMDI.infoError.ErrorString = "SIRPT11:" + ex.Message;
            }
        }
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
                string strdate = txtTodate.Text;
                dtpToDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT12:" + ex.Message;
            }
        }
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtTodate.Text = date.ToString("dd-MMM-yyyy");
                txtTodate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT13:" + ex.Message;
            }
        }
        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT14:" + ex.Message;
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT15:" + ex.Message;
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateValidation ObjValidation = new DateValidation();
                ObjValidation.DateValidationFunction(txtTodate);
                if (Convert.ToDateTime(txtTodate.Text) < Convert.ToDateTime(txtFromDate.Text))
                {
                    MessageBox.Show("todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    txtFromDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                    DateTime dt;
                    DateTime.TryParse(txtTodate.Text, out dt);
                    dtpToDate.Value = dt;
                    gridFill();
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(txtTodate.Text, out dt);
                    dtpToDate.Value = dt;
                    gridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT16:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding voucher for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSIReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvSIReport.CurrentRow != null)
                    {
                        inCurrenRowIndex = dgvSIReport.CurrentRow.Index;
                        if (bool.Parse(dgvSIReport.CurrentRow.Cells["dgvtxtPOS"].Value.ToString()))
                        {
                            frmPOS objfrmpos;
                            decimal dcRegister = Convert.ToDecimal(dgvSIReport.CurrentRow.Cells["dgvtxtSalesMasterId"].Value.ToString());
                            objfrmpos = Application.OpenForms["frmPOS"] as frmPOS;
                            if (objfrmpos == null)
                            {
                                objfrmpos = new frmPOS();
                                objfrmpos.MdiParent = formMDI.MDIObj;
                                objfrmpos.Show();
                                objfrmpos.CallFromSalesInvoiceReport(dcRegister, this);
                                objfrmpos.WindowState = FormWindowState.Normal;
                            }
                            else
                            {
                                objfrmpos.CallFromSalesInvoiceReport(dcRegister, this);
                                if (objfrmpos.WindowState == FormWindowState.Minimized)
                                {
                                    objfrmpos.WindowState = FormWindowState.Normal;
                                }
                            }
                        }
                        else
                        {
                            frmSalesInvoice objfrmSalesInvoice = new frmSalesInvoice();
                            frmSalesInvoice open = Application.OpenForms["frmSalesInvoice"] as frmSalesInvoice;
                            decimal dcRegister = Convert.ToDecimal(dgvSIReport.CurrentRow.Cells["dgvtxtSalesMasterId"].Value.ToString());
                            if (open == null)
                            {
                                objfrmSalesInvoice.WindowState = FormWindowState.Normal;
                                objfrmSalesInvoice.MdiParent = formMDI.MDIObj;
                                objfrmSalesInvoice.Show();
                                objfrmSalesInvoice.CallFromSalesInvoiceReport(this, dcRegister);
                            }
                            else
                            {
                                open.CallFromSalesInvoiceReport(this, dcRegister);
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
                formMDI.infoError.ErrorString = "SIRPT17:" + ex.Message;
            }
        }
        /// <summary>
        /// Generates serial no in Datagridview on rows added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSIReport_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SerialNo();
                maxSerialNo++;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT18:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSIReport.RowCount > 0)
                {
                    DataSet ds = new DataSet();
                    ds = spSalesMaster.salesInvoiceReportPrint(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtTodate.Text), Convert.ToDecimal(cmbVoucherType.SelectedValue), Convert.ToDecimal(cmbCashOrParty.SelectedValue), Convert.ToDecimal(cmbArea.SelectedValue), cmbSalesMode.Text, Convert.ToDecimal(cmbSalesMan.SelectedValue), txtProductName.Text, txtVoucherNo.Text, cmbStatus.Text, Convert.ToDecimal(cmbRoute.SelectedValue), Convert.ToDecimal(cmbModelNo.SelectedValue), txtProductCode.Text, 1);
                    frmReport reportobj = new frmReport();
                    reportobj.MdiParent = formMDI.MDIObj;
                    reportobj.SalesInvoiceReportPrinting(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT19:" + ex.Message;
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
                ex.ExportExcel(dgvSIReport, "Sales Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT20:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalesReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SIRPT21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtTodate.SelectionStart = txtTodate.Text.Length;
                    txtTodate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                    cmbVoucherType.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtTodate.SelectionLength != 11)
                    {
                        if (txtTodate.Text == string.Empty || txtTodate.SelectionStart == 0)
                        {
                            txtFromDate.Focus();
                            txtFromDate.SelectionStart = 0;
                            txtFromDate.SelectionLength = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                    txtTodate.SelectionStart = 0;
                    txtTodate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                    cmbVoucherType.SelectionStart = 0;
                }
                if (txtVoucherNo.Text == string.Empty || txtVoucherNo.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbVoucherType.Focus();
                        cmbVoucherType.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbStatus.Focus();
                    cmbStatus.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT26:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbArea.Focus();
                    cmbArea.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbCashOrParty.Focus();
                    cmbCashOrParty.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT27:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbArea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbRoute.Focus();
                    cmbRoute.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbStatus.Focus();
                    cmbStatus.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT28:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRoute_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSalesMode.Focus();
                    cmbSalesMode.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbArea.Focus();
                    cmbArea.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT29:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalesMode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbModelNo.Focus();
                    cmbModelNo.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbRoute.Focus();
                    cmbRoute.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT30:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModelNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSalesMan.Focus();
                    cmbSalesMan.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbSalesMode.Focus();
                    cmbSalesMode.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT31:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalesMan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbModelNo.Focus();
                    cmbModelNo.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT32:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        cmbSalesMan.Focus();
                        cmbSalesMan.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT33:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (txtProductName.Text == string.Empty || txtProductName.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtProductCode.Focus();
                        txtProductCode.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SIRPT34:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyUp(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SIRPT35:" + ex.Message;
            }
        }
        #endregion

      
    }
}
