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
    public partial class frmSalesQuotationRegister : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        SalesQuotationMasterSP SpSalesQuotationMaster = new SalesQuotationMasterSP();//USED TO FILL SALES QUOTATION GRID
        DataTable dtblSalesQuotationMasterRegister = new DataTable();
        string strCondition = string.Empty;
        decimal LedgerId = 0;
        TransactionsGeneralFill transactionGeneralFillObj = new TransactionsGeneralFill();
        DateValidation dateValidationObj = new DateValidation();
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmSalesQuotationRegister class 
        /// </summary>
        public frmSalesQuotationRegister()
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
                dtpFromDate.Value = PublicVariables._dtCurrentDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpFromDate.CustomFormat = "dd-MMM-yyyy";
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.CustomFormat = "dd-MMM-yyyy";
                rbtnAll.Checked = true;
                rbtnPending.Checked = false;
                rbtnApproved.Checked = false;
                txtFromDate.Focus();
                txtQuotationNo.Text = string.Empty;
                CashOrPartyComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                string strInvoiceNo = string.Empty;
                if (txtQuotationNo.Text.Trim() == string.Empty)
                {
                    txtQuotationNo.Text = string.Empty;
                }
                else
                {
                    strInvoiceNo = txtQuotationNo.Text;
                }
                if (rbtnAll.Checked)
                {
                    strCondition = "All";
                }
                if (rbtnPending.Checked)
                {
                    strCondition = "False";
                }
                if (rbtnApproved.Checked)
                {
                    strCondition = "True";
                }
                if (cmbCashOrParty.SelectedIndex == 0 || cmbCashOrParty.SelectedIndex == -1)
                {
                    LedgerId = -1;
                }
                else
                {
                    LedgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                }
                DateTime FromDate = this.dtpFromDate.Value;
                DateTime ToDate = this.dtpToDate.Value;
                dtblSalesQuotationMasterRegister = SpSalesQuotationMaster.SalesQuotationRegisterSearch(strInvoiceNo, LedgerId, FromDate, ToDate, strCondition);
                dgvSalesQuotationRegister.DataSource = dtblSalesQuotationMasterRegister;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash/Party combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                transactionGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashOrParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG3:" + ex.Message;
            }
        }
        
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalesQuotationRegister_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG4:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Reset' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG5:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                {
                    if (Convert.ToDateTime(txtToDate.Text) < Convert.ToDateTime(txtFromDate.Text))
                    {
                        MessageBox.Show("Todate should be greater than fromdate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        DateTime dt;
                        DateTime.TryParse(txtToDate.Text, out dt);
                        dtpToDate.Value = dt;
                        dtpFromDate.Value = dt;
                    }
                }
                else if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString();
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString();
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpToDate.Value = dt;
                    dtpFromDate.Value = dt;
                }
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG6:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtFromDate texbox on dtpFromDate datetimepicker ValueChanged
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
                formMDI.infoError.ErrorString = "SQREG7:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtToDate texbox on dtpToDate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtToDate.Text = date.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG8:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding voucher on cell double click in Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSalesQuotationRegister_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    btnViewDetails_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG9:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding voucher on ViewDetails button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSalesQuotationRegister.CurrentRow != null)
                {
                    frmSalesQuotation frmSalesQuotationObj = new frmSalesQuotation();
                    frmSalesQuotation frmSalesQuotationOpen = Application.OpenForms["frmSalesQuotation"] as frmSalesQuotation;
                    if (frmSalesQuotationOpen == null)
                    {
                        frmSalesQuotationObj.MdiParent = formMDI.MDIObj;
                        frmSalesQuotationObj.WindowState = FormWindowState.Normal;
                        frmSalesQuotationObj.CallFRomSalesQuotationRegister(this, Convert.ToDecimal(dgvSalesQuotationRegister.CurrentRow.Cells["dgvtxtquotationMasterId"].Value));
                    }
                    else
                    {
                        frmSalesQuotationOpen.CallFRomSalesQuotationRegister(this, Convert.ToDecimal(dgvSalesQuotationRegister.CurrentRow.Cells["dgvtxtquotationMasterId"].Value));
                        frmSalesQuotationOpen.BringToFront();
                        if (frmSalesQuotationOpen.WindowState == FormWindowState.Minimized)
                        {
                            frmSalesQuotationOpen.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG10:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Messages.CloseConfirmation())
                    this.Close();
                else
                    this.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG11:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                bool isInvalid = dateValidationObj.DateValidationFunction(txtFromDate);
                if (!isInvalid)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpFromDate.Value = Convert.ToDateTime(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG12:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                string strFromDate = txtFromDate.Text;
                string strToDate = txtToDate.Text;
                bool isInvalid = dateValidationObj.DateValidationFunction(txtToDate);
                if (!isInvalid)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG13:" + ex.Message;
            }
        }
        #endregion
        #region navigation
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
                    if (txtToDate.Enabled == true)
                    {
                        txtToDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG14:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (rbtnAll.Enabled)
                    {
                        rbtnAll.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtFromDate.Focus();
                    txtFromDate.SelectionStart = 0;
                    txtFromDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG15:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnAll_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnApproved.Select();
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
                formMDI.infoError.ErrorString = "SQREG16:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnApproved_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (rbtnPending.Enabled == true)
                    {
                        rbtnPending.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnAll.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG17:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnPending_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrParty.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnApproved.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG18:" + ex.Message;
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
                    txtQuotationNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnPending.Enabled)
                    {
                        rbtnPending.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG19:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnRefresh.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtQuotationNo.Focus();
                    txtQuotationNo.SelectionStart = 0;
                    txtQuotationNo.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvSalesQuotationRegister.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnPending.Enabled == true)
                    {
                        btnReset.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuotationNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnRefresh.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtQuotationNo.Text == string.Empty || txtQuotationNo.SelectionStart == 0)
                    {
                        cmbCashOrParty.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SQREG22:" + ex.Message;
            }
        }
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSalesQuotationRegister_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SQREG23:" + ex.Message;
            }
        }
        #endregion
    }
}
