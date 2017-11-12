//This is a source code or part of Oneaccount project
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
    public partial class frmPurchaseReturnRegister : Form
    {
        #region Variables
        /// <summary>
        ///  public variable declaration part
        /// </summary>

        bool isFromRegister = false;
        PurchaseReturnMasterSP spPurchaseReturnMaster = new PurchaseReturnMasterSP();
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        #endregion

        #region Functions
        /// <summary>
        /// Creates an instance of frmPurchaseReturnRegister class
        /// </summary>
        public frmPurchaseReturnRegister()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to Clear
        /// </summary>
        public void Clear()
        {
            string strVoucherType = string.Empty;
            try
            {
                dtpFromDate.Value = PublicVariables._dtCurrentDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                CashOrPartyComboFill();
                txtFromDate.Focus();
                VoucherTypeComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill DataGridView while searching
        /// </summary>
        public void GridFill()
        {
            string strInvoiceNo = string.Empty;
            decimal decAgainstInvoiceNo = 0;
            decimal decLedgerId = 0;
            decimal decVoucherType = 0;
            DataTable dtblPurchaseReturnRegister = new DataTable();
            try
            {
                strInvoiceNo = (txtReturnNo.Text.Trim() == string.Empty) ? string.Empty : txtReturnNo.Text;
                decLedgerId = (cmbCashOrParty.SelectedIndex == 0 || cmbCashOrParty.SelectedIndex == -1) ? -1 : Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                decAgainstInvoiceNo = (cmbInvoiceNo.SelectedIndex == 0 || cmbInvoiceNo.SelectedIndex == -1) ? 0 : Convert.ToDecimal(cmbInvoiceNo.SelectedValue.ToString());
                decVoucherType = (cmbVoucherType.SelectedIndex == 0 || cmbVoucherType.SelectedIndex == -1) ? -1 : Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                DateTime FromDate = this.dtpFromDate.Value;
                DateTime ToDate = this.dtpToDate.Value;
                dtblPurchaseReturnRegister = spPurchaseReturnMaster.PurchaseReturnRegisterFill(FromDate, ToDate, decLedgerId, strInvoiceNo, decAgainstInvoiceNo, decVoucherType);
                dgvPurchaseReturnRegister.DataSource = dtblPurchaseReturnRegister;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Cash or Party combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Invoice No combobox
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
                formMDI.infoError.ErrorString = "PRR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Voucher Type combobox
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                PurchaseReturnDetailsSP spPurchaseReturn = new PurchaseReturnDetailsSP();
                cmbVoucherType.DataSource = spPurchaseReturn.VoucherTypeComboFillForPurchaseReturn();
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR5:" + ex.Message;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Calls corresponding voucher on cell double click in Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReturnRegister_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvPurchaseReturnRegister.CurrentRow != null)
                {
                    if (e.RowIndex > -1 && e.ColumnIndex > -1)
                    {
                        isFromRegister = true;
                        frmPurchaseReturn frmPurchaseReturnObj = new frmPurchaseReturn();
                        frmPurchaseReturn frmPurchaseReturnObj1 = Application.OpenForms["frmPurchaseReturn"] as frmPurchaseReturn;
                        if (frmPurchaseReturnObj1 == null)
                        {
                            frmPurchaseReturnObj.MdiParent = formMDI.MDIObj;
                            frmPurchaseReturnObj.CallFromPurchaseReturnRegister(this, Convert.ToDecimal(dgvPurchaseReturnRegister.CurrentRow.Cells["dgvtxtPurchaseReturnMasterId"].Value.ToString()), isFromRegister);
                        }
                        else
                        {
                            frmPurchaseReturnObj1.CallFromPurchaseReturnRegister(this, Convert.ToDecimal(dgvPurchaseReturnRegister.CurrentRow.Cells["dgvtxtPurchaseReturnMasterId"].Value.ToString()), isFromRegister);
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
                formMDI.infoError.ErrorString = "PRR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseReturnRegister_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR7:" + ex.Message;
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
                txtReturnNo.Text = string.Empty;
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR8:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PRR9:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PRR10:" + ex.Message;
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
                DateTime date = this.dtpFromDate.Value;
                this.txtFromDate.Text = date.ToString("dd-MMM-yyyy");
                txtFromDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR11:" + ex.Message;
            }
        }
        /// <summary>
        /// Fiils txtTodate textbox on dtpTodate Datetimepicker ValueChanged
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
                formMDI.infoError.ErrorString = "PRR12:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills invoiceno combobox on cmbCashOrParty combobox SelectedValueChanged
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
                formMDI.infoError.ErrorString = "PRR13:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls corresponding voucher on ViewDetails button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPurchaseReturnRegister.CurrentRow != null)
                {
                    DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvPurchaseReturnRegister.CurrentCell.ColumnIndex, dgvPurchaseReturnRegister.CurrentCell.RowIndex);
                    dgvPurchaseReturnRegister_CellDoubleClick(sender, ex);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR14:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PRR15:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PRR16:" + ex.Message;
            }
        }
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseReturnRegister_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 27)
                {
                    if (Messages.CloseConfirmation())
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR17:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills invoiceno combobox on cmbVoucherType combobox SelectedValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbVoucherType.SelectedIndex > -1)
                {
                    if (cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashOrParty.Text != "System.Data.DataRowView")
                    {
                        InvoiceNoComboFill();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR18:" + ex.Message;
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
                    if (txtToDate.Enabled == true)
                    {
                        txtToDate.Focus();
                        txtToDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR19:" + ex.Message;
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
                    cmbCashOrParty.Focus();
                }
                if (txtToDate.Text == string.Empty || txtToDate.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR20:" + ex.Message;
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
                    cmbVoucherType.Focus();
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
                formMDI.infoError.ErrorString = "PRR21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtReturnNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReturnNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvPurchaseReturnRegister.Focus();
                }
                if (txtReturnNo.Text == string.Empty || txtReturnNo.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbInvoiceNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPurchaseReturnRegister_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnView_Click(sender, e);
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtReturnNo.Focus();
                    txtReturnNo.SelectionStart = txtReturnNo.TextLength;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR24:" + ex.Message;
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
                    cmbInvoiceNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbCashOrParty.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PRR25:" + ex.Message;
            }
        }
        #endregion
            
    }
}
