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
    public partial class frmRejectionInRegister : Form
    {
        #region PublicVariables and instance

        /// <summary>
        /// Public variable declaration part
        /// </summary>
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        RejectionInMasterSP SpRejectionInMaster = new RejectionInMasterSP();
        
        
        /// <summary>
        /// Creates an instance of frmRejectionInRegister class
        /// </summary>
        public frmRejectionInRegister()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void RegisterGridFill()
        {
            RejectionInMasterSP SpRejectionInMaster = new RejectionInMasterSP();
            try
            {
                decimal decvouchertypeId = 0;
                decimal decLedgerId = 0;
                string strInvoiceNo = string.Empty;
                strInvoiceNo = (txtRejectionInNo.Text.Trim() == string.Empty) ? string.Empty : txtRejectionInNo.Text;
                decLedgerId = (cmbCashorParty.SelectedIndex == 0 || cmbCashorParty.SelectedIndex == -1) ? -1 : Convert.ToDecimal(cmbCashorParty.SelectedValue.ToString());
                decvouchertypeId = (cmbVoucherType.SelectedIndex == 0 || cmbVoucherType.SelectedIndex == -1) ? -1 : Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                DateTime FromDate = this.dtpFromDate.Value;
                DateTime ToDate = this.dtpToDate.Value;
                DataTable dtbl = new DataTable();
                dgvRejectionInRegister.DataSource = SpRejectionInMaster.RejectionInRegisterFill(FromDate, ToDate, decLedgerId, strInvoiceNo, decvouchertypeId);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash/Party combobox
        /// </summary>
        /// <param name="cmbCashorParty"></param>
        public void CashOrPartyComboFill()
        {
            TransactionsGeneralFill transactionGeneralFillObj = new TransactionsGeneralFill();
            try
            {
                transactionGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbCashorParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG2:" + ex.Message;
            }
        }
        public void VoucherTypeComboFill()
        {

            TransactionsGeneralFill transactionGeneralFillObj = new TransactionsGeneralFill();
            try
            {
                transactionGeneralFillObj.VoucherTypeComboFill(cmbVoucherType, "Rejection In", true);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG3:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="dtp"></param>
        public void DateValidation(TextBox txt, DateTimePicker dtp)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txt);
                if (txt.Text == String.Empty)
                {
                    txt.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtp.Value = DateTime.Parse(txt.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG4:" + ex.Message;
            }
        }
        public void ViewGridDetails()
        {
            bool isFromRegister = false;
            try
            {
                if (dgvRejectionInRegister.CurrentRow != null)
                {
                    isFromRegister = true;
                    decimal decMasterId = Convert.ToDecimal(dgvRejectionInRegister.CurrentRow.Cells["dgvtxtRejectionInMaterId"].Value.ToString());
                    frmRejectionIn frmRejectionInObj = new frmRejectionIn();
                    frmRejectionIn frmRejectionInOpen = Application.OpenForms["frmRejectionIn"] as frmRejectionIn;
                    if (frmRejectionInOpen == null)
                    {
                        frmRejectionInObj.MdiParent = formMDI.MDIObj;
                        frmRejectionInObj.CallFromRejectionInRegister(this, Convert.ToDecimal(dgvRejectionInRegister.CurrentRow.Cells["dgvtxtRejectionInMaterId"].Value.ToString()), isFromRegister);
                    }
                    else
                    {
                        frmRejectionInOpen.CallFromRejectionInRegister(this, Convert.ToDecimal(dgvRejectionInRegister.CurrentRow.Cells["dgvtxtRejectionInMaterId"].Value.ToString()), isFromRegister);
                        frmRejectionInOpen.MdiParent = formMDI.MDIObj;
                        frmRejectionInOpen.BringToFront();
                        if (frmRejectionInOpen.WindowState == FormWindowState.Minimized)
                        {
                            frmRejectionInOpen.WindowState = FormWindowState.Normal;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG5:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRejectionInRegister_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFromDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                CashOrPartyComboFill();
                VoucherTypeComboFill();
                RegisterGridFill();
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG6:" + ex.Message;
            }
        }
        /// <summary>
        /// Fiils txtFromDate textbox on dtpfromDate Datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = dtpFromDate.Value.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG7:" + ex.Message;
            }
        }
        /// <summary>
        ///  Datevalidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation(txtFromDate, dtpFromDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG8:" + ex.Message;
            }
        }
        /// <summary>
        ///  Fiils txtTodate textbox on dtpTodate Datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtToDate.Text = dtpToDate.Value.ToString("dd-MMM-yyyy");
                txtToDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG9:" + ex.Message;
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
                DateValidation(txtToDate, dtpToDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG10:" + ex.Message;
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
                frmRejectionInRegister_Load(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG11:" + ex.Message;
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
                ViewGridDetails();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG12:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding voucher on cell double click in Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionInRegister_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    btnViewDetails_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG13:" + ex.Message;
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG14:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
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
                    txtToDate.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtFromDate.SelectionStart = 0;
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG15:" + ex.Message;
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
                    cmbCashorParty.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.SelectionStart != 0)
                    {
                        txtFromDate.SelectionStart = txtFromDate.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG16:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashorParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbCashorParty.SelectionStart == 0)
                    {
                        txtToDate.Focus();
                        txtToDate.SelectionStart = txtToDate.Text.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG17:" + ex.Message;
            }
        }

        /// <summary>
        /// Event to Navigate from Voucher Type ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtRejectionInNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbCashorParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG18:" + ex.Message;
            }

        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRejectionInNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnViewDetails.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtRejectionInNo.SelectionStart == 0)
                    {
                        cmbVoucherType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG19:" + ex.Message;
            }
        }
 
      
        /// <summary>
        /// Event occuring in Enter Key Press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRejectionInRegister_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvRejectionInRegister.Rows.Count > 0)
                    {
                        ViewGridDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG20:" + ex.Message;
            }
        }
      
        /// <summary>
        ///  Event to Search in Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                RegisterGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RIREG21:" + ex.Message;
            }
        }
        #endregion
 


    }
}
