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


    public partial class frmPurchaseReport : Form
    {


        #region Functons
        /// <summary>
        /// Create an instance for frmPurchaseReport class
        /// </summary>
        public frmPurchaseReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to clear the form control
        /// </summary>
        public void Clear()
        {
            try
            {
                txtProductCode.Text = string.Empty;
                AgainstVoucherTypeComboFill("All");
                rbtnVoucherDate.Checked = true;
                cmbPurchaseMode.SelectedIndex = 0;
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                cmbCashOrParty.SelectedIndex = 0;
                cmbPurchaseMode.SelectedIndex = 0;
                cmbVoucherType.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                txtProductName.Text = string.Empty;
                txtVoucherNo.Text = string.Empty;
                txtOrderNo.Text = string.Empty;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill CashOrParty combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
                TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill VoucherType combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            VoucherTypeSP spVoucherType = new VoucherTypeSP();
            DataTable dtbl = new DataTable();
            try
            {
                dtbl = spVoucherType.VoucherTypeSelectionComboFill("Purchase Invoice");
                DataRow drow = dtbl.NewRow();
                drow["voucherTypeId"] = 0;
                drow["voucherTypeName"] = "All";
                dtbl.Rows.InsertAt(drow, 0);
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill AgainstVoucherType combobox
        /// </summary>
        /// <param name="strVoucherType"></param>
        public void AgainstVoucherTypeComboFill(string strVoucherType)
        {
            VoucherTypeSP spVoucherType = new VoucherTypeSP();
            DataTable dtbl = new DataTable();
            try
            {
                dtbl = spVoucherType.VoucherTypeSelectionComboFill(strVoucherType);
                DataRow drow = dtbl.NewRow();
                drow["voucherTypeId"] = 0;
                drow["voucherTypeName"] = "All";
                dtbl.Rows.InsertAt(drow, 0);
                cmbAgainstVoucherType.DataSource = dtbl;
                cmbAgainstVoucherType.ValueMember = "voucherTypeId";
                cmbAgainstVoucherType.DisplayMember = "voucherTypeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid based on condition
        /// </summary>
        public void GridFill()
        {
            decimal decTotalAmount = 0;
            string strColumn = string.Empty;
            DataTable dtbl = new DataTable();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            try
            {

                if (rbtnInvoiceDate.Checked)
                {
                    strColumn = rbtnInvoiceDate.Text;
                }
                else
                {
                    strColumn = rbtnVoucherDate.Text;
                }
                dtbl = spPurchaseMaster.PurchaseInvoiceReportFill(1, strColumn, dtpFromDate.Value, dtpToDate.Value,
                   Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), cmbStatus.Text, cmbPurchaseMode.Text,
                   Convert.ToDecimal(cmbAgainstVoucherType.SelectedValue.ToString()), txtOrderNo.Text,
                   Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()), txtVoucherNo.Text, txtProductCode.Text, txtProductName.Text);
                dgvPurchaseReport.DataSource = dtbl;
                foreach (DataGridViewRow dgvrow in dgvPurchaseReport.Rows)
                {
                    if (dgvrow.Cells["dgvtxtBillAmount"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtBillAmount"].Value.ToString() != string.Empty)
                        {
                            decTotalAmount = decTotalAmount + Convert.ToDecimal(dgvrow.Cells["dgvtxtBillAmount"].Value.ToString());
                        }
                    }
                }
                txtTotalAmount.Text = Math.Round(decTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate the serial no in grid
        /// </summary>
        public void SerialNo()
        {
            try
            {
                foreach (DataGridViewRow row in dgvPurchaseReport.Rows)
                {
                    row.Cells["dgvtxtSlNo"].Value = row.Index + 1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP6:" + ex.Message;
            }
        }
        #endregion

        #region Events


        /// <summary>
        /// Form load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseReport_Load(object sender, EventArgs e)
        {
            try
            {
                CashOrPartyComboFill();
                VoucherTypeComboFill();
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP7:" + ex.Message;
            }
            
        }
        /// <summary>
        /// Set the textbox date  as dtp's selected value
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
                formMDI.infoError.ErrorString = "PIREP8:" + ex.Message;
            }
        }
/// <summary>
/// Date validation and set dtp's value as textbox value
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
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtFromDate.Text;
                dtpFromDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP9:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the textbox date  as dtp's selected value
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
                formMDI.infoError.ErrorString = "PIREP10:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set dtp's value as textbox value
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
                formMDI.infoError.ErrorString = "PIREP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click to call the gridfill function
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
                formMDI.infoError.ErrorString = "PIREP12:" + ex.Message;
            }
        }
        /// <summary>
        /// Reset button click to call the clear function
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
                formMDI.infoError.ErrorString = "PIREP13:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill the Against VoucherType Combo based on purchase mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurchaseMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPurchaseMode.Text == "NA")
                {
                    lblOrderNo.Visible = false;
                    txtOrderNo.Visible = false;
                    lblAgainstVoucherType.Visible = false;
                    cmbAgainstVoucherType.Visible = false;
                }
                else if (cmbPurchaseMode.Text == "All")
                {
                    lblOrderNo.Visible = false;
                    txtOrderNo.Visible = false;
                    lblAgainstVoucherType.Visible = false;
                    cmbAgainstVoucherType.Visible = false;
                }
                else if (cmbPurchaseMode.Text == "Against PurchaseOrder")
                {
                    lblOrderNo.Text = "Order No";
                    lblOrderNo.Visible = true;
                    txtOrderNo.Visible = true;
                    lblAgainstVoucherType.Visible = true;
                    cmbAgainstVoucherType.Visible = true;
                    AgainstVoucherTypeComboFill("Purchase Order");
                }

                else if (cmbPurchaseMode.Text == "Against MaterialReceipt")
                {
                    lblOrderNo.Text = "Receipt No";
                    lblOrderNo.Visible = true;
                    txtOrderNo.Visible = true;
                    lblAgainstVoucherType.Visible = true;
                    cmbAgainstVoucherType.Visible = true;
                    AgainstVoucherTypeComboFill("Material Receipt");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP14:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the print in butten print click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {

                PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
                string strColumn = string.Empty;
                GridFill();
                if (dgvPurchaseReport.RowCount > 0)
                {
                    if (rbtnInvoiceDate.Checked)
                    {
                        strColumn = rbtnInvoiceDate.Text;
                    }
                    else
                    {
                        strColumn = rbtnVoucherDate.Text;
                    }
                    DataSet dsPurchaseReport = spPurchaseMaster.PurchaseInvoiceReportPrinting(1, strColumn, dtpFromDate.Value, dtpToDate.Value,
                    Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), cmbStatus.Text, cmbPurchaseMode.Text,
                    Convert.ToDecimal(cmbAgainstVoucherType.SelectedValue.ToString()), txtOrderNo.Text,
                    Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()), txtVoucherNo.Text, txtProductCode.Text, txtProductName.Text);
                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.PurchaseReportPrinting(dsPurchaseReport, txtTotalAmount.Text);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP15:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click for updation of selected item in frmPurchaseInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {


                    if (dgvPurchaseReport.CurrentRow != null && dgvPurchaseReport.CurrentRow.Cells["dgvtxtPurchaseMasterId"].Value != null && dgvPurchaseReport.CurrentRow.Cells["dgvtxtPurchaseMasterId"].Value.ToString() != string.Empty)
                    {

                        if (dgvPurchaseReport.CurrentRow != null)
                        {

                            frmPurchaseInvoice frmPurchaseInvoiceObj = new frmPurchaseInvoice();
                            frmPurchaseInvoice frmPurchaseInvoiceOpen = Application.OpenForms["frmPurchaseInvoice"] as frmPurchaseInvoice;

                            if (frmPurchaseInvoiceOpen == null)
                            {
                                frmPurchaseInvoiceObj.MdiParent = formMDI.MDIObj;
                                frmPurchaseInvoiceObj.CallFromPurchaseReport(this, Convert.ToDecimal(dgvPurchaseReport.CurrentRow.Cells["dgvtxtPurchaseMasterId"].Value.ToString()));
                            }
                            else
                            {
                                frmPurchaseInvoiceOpen.CallFromPurchaseReport(this, Convert.ToDecimal(dgvPurchaseReport.CurrentRow.Cells["dgvtxtPurchaseMasterId"].Value.ToString()));
                                frmPurchaseInvoiceOpen.BringToFront();
                                if (frmPurchaseInvoiceOpen.WindowState == FormWindowState.Minimized)
                                {
                                    frmPurchaseInvoiceOpen.WindowState = FormWindowState.Normal;
                                }
                            }

                        }

                    }


                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP16:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the serial no function 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReport_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP17:" + ex.Message;
            }
        }
        /// <summary>
        ///  Date validation and set dtp's value as textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtToDate.Text == string.Empty && !txtToDate.Focused)
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP18:" + ex.Message;
            }
        }
        /// <summary>
        ///  Date validation and set dtp's value as textbox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text == string.Empty && !txtFromDate.Focused)
                {
                    DateValidation obj = new DateValidation();
                    bool isInvalid = obj.DateValidationFunction(txtFromDate);
                    if (!isInvalid)
                    {
                        txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    }
                    string date = txtFromDate.Text;
                    dtpFromDate.Value = Convert.ToDateTime(date);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP19:" + ex.Message;
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
                ex.ExportExcel(dgvPurchaseReport, "Purchase Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP20:" + ex.Message;
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PIREP21:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnVoucherDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnInvoiceDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP22:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnInvoiceDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFromDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnVoucherDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP23:" + ex.Message;
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
                if (e.KeyCode == Keys.Back)
                {
                    if (txtFromDate.SelectionStart == 0)
                    {
                        rbtnInvoiceDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP24:" + ex.Message;
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
                    cmbCashOrParty.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP25:" + ex.Message;
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
                    cmbStatus.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP26:" + ex.Message;
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
                    cmbPurchaseMode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbCashOrParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP27:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurchaseMode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbAgainstVoucherType.Visible)
                    {
                        cmbAgainstVoucherType.Focus();
                    }
                    else
                    {
                        cmbVoucherType.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbStatus.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP28:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAgainstVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtOrderNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbPurchaseMode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP29:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOrderNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtOrderNo.SelectionStart == 0)
                    {
                        cmbAgainstVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP30:" + ex.Message;
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
                if (e.KeyCode == Keys.Back)
                {
                    if (txtOrderNo.Visible)
                    {
                        txtOrderNo.Focus();
                    }
                    else
                    {
                        cmbPurchaseMode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP31:" + ex.Message;
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
                    txtProductCode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.SelectionStart == 0)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP32:" + ex.Message;
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
                    txtProductName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.SelectionStart == 0)
                    {
                        txtVoucherNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP33:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductName.SelectionStart == 0)
                    {
                        txtProductCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP34:" + ex.Message;
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

                }
                if (e.KeyCode == Keys.Back)
                {
                    txtProductName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP35:" + ex.Message;
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

                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP36:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP37:" + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {

                }
                if (e.KeyCode == Keys.Back)
                {
                    dgvPurchaseReport.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PIREP38:" + ex.Message;
            }
        }
        
        #endregion


        
    }
}
