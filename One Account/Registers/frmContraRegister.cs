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
    public partial class frmContraRegister : Form
    {
        #region Functions
        /// <summary>
        /// Creates an instance of frmContraRegister class
        /// </summary>
        public frmContraRegister()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Bank/Cash combobox
        /// </summary>
        public void BankOrCashComboFill()
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
                cmbBankOrCash.DataSource = dtbl;
                cmbBankOrCash.ValueMember = "ledgerId";
                cmbBankOrCash.DisplayMember = "ledgerName";
                cmbBankOrCash.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void Gridfill()
        {
            try
            {
                string strType = string.Empty;
                if (rbtnDeposit.Checked)
                {
                    strType = "Deposit";
                }
                else
                {
                    strType = "Withdraw";
                }
                ContraMasterSP spContraMaster = new ContraMasterSP();
                DataTable dtbl = new DataTable();
                dtbl = spContraMaster.ContraVoucherRegisterSearch(Convert.ToDateTime(dtpFromDate.Value), Convert.ToDateTime(dtpToDate.Value), txtVoucherNo.Text.Trim(), cmbBankOrCash.Text, strType);
                dgvContraRegister.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void Clear()
        {
            try
            {
                rbtnDeposit.Checked = true;            
                dtpFromDate.Value = PublicVariables._dtCurrentDate;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtFromDate.Select();
                txtVoucherNo.Text = string.Empty;
                
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR3:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraRegister_Load(object sender, EventArgs e)
        {
            try
            {
                BankOrCashComboFill();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Calls ContraVoucher to view details and for updation on cell doubleclick in Datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraRegister_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvContraRegister.CurrentRow.Index == e.RowIndex)
                    {
                        if (dgvContraRegister.CurrentRow.Index != -1 && dgvContraRegister.CurrentCell.ColumnIndex != 0)
                        {
                            btnViewDetails_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR5:" + ex.Message;
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
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR6:" + ex.Message;
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
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strdate = txtFromDate.Text.Trim();
                dtpFromDate.Value = Convert.ToDateTime(strdate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtFromDate textbox on dtpFromDate ValueChanged
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
                formMDI.infoError.ErrorString = "CR8:" + ex.Message;
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
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strdate = txtToDate.Text.Trim();
                dtpToDate.Value = Convert.ToDateTime(strdate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR9:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtToDate textbox on dtpToDate ValueChanged
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
                formMDI.infoError.ErrorString = "CR10:" + ex.Message;
            }
        }
        /// <summary>
        /// On "Search' button click
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
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR11:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Reset button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                BankOrCashComboFill();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR12:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'ViewDetails button click calls CountraVoucher to view details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvContraRegister.CurrentRow != null)
                {
                    frmContraVoucher objfrmContraVoucher = new frmContraVoucher();
                    decimal decMasterId = Convert.ToDecimal(dgvContraRegister.CurrentRow.Cells["dgvtxtMasterId"].Value.ToString());
                    frmContraVoucher open = Application.OpenForms["frmContraVoucher"] as frmContraVoucher;
                    if (open == null)
                    {
                        objfrmContraVoucher.WindowState = FormWindowState.Normal;
                        objfrmContraVoucher.MdiParent = formMDI.MDIObj;
                        objfrmContraVoucher.CallFromContraRegister(this, decMasterId);
                    }
                    else
                    {
                        open.MdiParent = formMDI.MDIObj;
                        open.BringToFront();
                        open.CallFromContraRegister(this, decMasterId);
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR13:" + ex.Message;
            }
        }
        # endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraRegister_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "CR14:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDeposit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnWithdrawal.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR15:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and BackSpace key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnWithdrawal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFromDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    rbtnDeposit.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR16:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and BackSpace key navigation
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
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR17:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and BackSpace key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBankOrCash.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text == string.Empty || txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR18:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and BackSpace key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBankOrCash_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "CR19:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and BackSpace key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnRefresh.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherNo.Text.Trim() == string.Empty || txtVoucherNo.SelectionStart == 0)
                    {
                        cmbBankOrCash.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CR20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter and BackSpace key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraRegister_KeyDown(object sender, KeyEventArgs e)
        {
            
                try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvContraRegister.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvContraRegister.CurrentCell.ColumnIndex, dgvContraRegister.CurrentCell.RowIndex);
                        dgvContraRegister_CellDoubleClick(sender, ex);
                    }
                }
            
                else if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNo.Focus();
                    txtVoucherNo.SelectionStart = txtVoucherNo.TextLength;
                }
            }
                catch (Exception ex)
                {
                    formMDI.infoError.ErrorString = "CR21:" + ex.Message;
                }
        }

        #endregion
    }
}
