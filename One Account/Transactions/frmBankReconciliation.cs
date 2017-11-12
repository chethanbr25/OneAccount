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
    public partial class frmBankReconciliation : Form
    {
        #region Public Variables
        #endregion
        #region Functions
        /// <summary>
        /// Create an instance of a frmBankReconciliation Class
        /// </summary>
        public frmBankReconciliation()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Here reset the form
        /// </summary>
        public void clear()
        {
            try
            {
                bankAccountComboFill();
                cmbStatus.SelectedIndex = -1;
                /*-------date setting at the time of loading--------*/
                dtpStatementFrom.MinDate = PublicVariables._dtFromDate;
                dtpStatementFrom.MaxDate = PublicVariables._dtToDate;
                dtpStatementFrom.Value = PublicVariables._dtCurrentDate;
                dtpStatrmentTo.MinDate = PublicVariables._dtFromDate;
                dtpStatrmentTo.MaxDate = PublicVariables._dtToDate;
                dtpStatrmentTo.Value = PublicVariables._dtCurrentDate;
                /*--------------------------------------------------*/
                txtBalanceBankCr.Text = string.Empty;
                txtBalanceBankDr.Text = string.Empty;
                txtBalanceCompanyCr.Text = string.Empty;
                txtBalanceCompnyDr.Text = string.Empty;
                txtDifferenceCr.Text = string.Empty;
                txtDifferenceDr.Text = string.Empty;
                txtBalanceBankCr.Text = string.Empty;
                txtBalanceBankDr.Text = string.Empty;
                txtBalanceCompanyCr.Text = string.Empty;
                txtBalanceCompnyDr.Text = string.Empty;
                txtDifferenceCr.Text = string.Empty;
                txtDifferenceDr.Text = string.Empty;
                dgvBankReconciliation.Rows.Clear();
                BankReconciliationFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR1:" + ex.Message;
            }
        }
        /// <summary>
        /// BankAccount Combobox fill function
        /// </summary>
        public void bankAccountComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                dtbl = obj.BankComboFill();
                cmbBankAccount.DataSource = dtbl;
                cmbBankAccount.ValueMember = "ledgerId";
                cmbBankAccount.DisplayMember = "ledgerName";
                cmbBankAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR2:" + ex.Message;
            }
        }
        /// <summary>
        /// BankReconciliation Grid Fill function
        /// </summary>
        public void BankReconciliationFill()
        {
            try
            {
                BankReconciliationInfo infoBankReconciliation = new BankReconciliationInfo();
                BankReconciliationSP spBankReconciliation = new BankReconciliationSP();
                dgvBankReconciliation.Rows.Clear();
                DataTable dtblBank = new DataTable();
                if (cmbBankAccount.SelectedIndex > -1)
                {
                    if (cmbStatus.Text == "Reconciled")
                    {
                        dtblBank = spBankReconciliation.BankReconciliationFillReconcile(Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString()), Convert.ToDateTime(txtStatementFrom.Text), Convert.ToDateTime(txtStatementTo.Text));
                        if (dtblBank.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtblBank.Rows.Count; i++)
                            {
                                dgvBankReconciliation.Rows.Add();
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtSlNo"].Value = dtblBank.Rows[i]["Sl No"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtDate"].Value = dtblBank.Rows[i]["date"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtParticular"].Value = dtblBank.Rows[i]["ledgerName"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtVoucherType"].Value = dtblBank.Rows[i]["voucherTypeName"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtVoucherNo"].Value = dtblBank.Rows[i]["voucherNo"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtChequeNo"].Value = dtblBank.Rows[i]["chequeNo"].ToString();
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtChequeDate"].Value = dtblBank.Rows[i]["chequeDate"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtDeposit"].Value = dtblBank.Rows[i]["debit"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtWithdraw"].Value = dtblBank.Rows[i]["credit"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtStatementDate"].Value = dtblBank.Rows[i]["statementDate"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtLedgerPostingId"].Value = dtblBank.Rows[i]["ledgerPostingId"];
                            }
                        }
                    }
                    else
                    {
                        dtblBank = spBankReconciliation.BankReconciliationUnrecocile(Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString()), Convert.ToDateTime(txtStatementFrom.Text), Convert.ToDateTime(txtStatementTo.Text));
                        if (dtblBank.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtblBank.Rows.Count; i++)
                            {
                                dgvBankReconciliation.Rows.Add();
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtSlNo"].Value = dtblBank.Rows[i]["Sl No"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtDate"].Value = dtblBank.Rows[i]["date"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtParticular"].Value = dtblBank.Rows[i]["ledgerName"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtVoucherType"].Value = dtblBank.Rows[i]["voucherTypeName"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtVoucherNo"].Value = dtblBank.Rows[i]["voucherNo"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtChequeNo"].Value = dtblBank.Rows[i]["chequeNo"].ToString();
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtChequeDate"].Value = dtblBank.Rows[i]["chequeDate"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtDeposit"].Value = dtblBank.Rows[i]["debit"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtWithdraw"].Value = dtblBank.Rows[i]["credit"];
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtStatementDate"].Value = string.Empty;
                                dgvBankReconciliation.Rows[i].Cells["dgvtxtLedgerPostingId"].Value = dtblBank.Rows[i]["ledgerPostingId"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking invalid entries for Save or Update and Save and Update function
        /// </summary>
        public void saveOrEdit()
        {
            try
            {
                BankReconciliationInfo infoBankReconciliation = new BankReconciliationInfo();
                BankReconciliationSP spBankReconciliation = new BankReconciliationSP();
                foreach (DataGridViewRow dgv in dgvBankReconciliation.Rows)
                {
                    if (dgv.Cells["dgvtxtStatementDate"].Value != null && Convert.ToDecimal(dgv.Cells["dgvtxtLedgerPostingId"].Value.ToString()) != 0 && dgv.Cells["dgvtxtStatementDate"].Value.ToString() != string.Empty)
                    {
                        infoBankReconciliation.LedgerPostingId = decimal.Parse(dgv.Cells["dgvtxtLedgerPostingId"].Value.ToString());
                        infoBankReconciliation.StatementDate = Convert.ToDateTime((dgv.Cells["dgvtxtStatementDate"].Value.ToString()));
                        infoBankReconciliation.Extra1 = string.Empty;
                        infoBankReconciliation.Extra2 = string.Empty;
                        infoBankReconciliation.ExtraDate = PublicVariables._dtCurrentDate;
                        decimal decReconcileId = spBankReconciliation.BankReconciliationLedgerPostingId(Convert.ToDecimal(dgv.Cells["dgvtxtLedgerPostingId"].Value.ToString()));
                        if (decReconcileId != 0)
                        {
                            infoBankReconciliation.ReconcileId = decReconcileId;
                            spBankReconciliation.BankReconciliationEdit(infoBankReconciliation);
                        }
                        else
                            spBankReconciliation.BankReconciliationAdd(infoBankReconciliation);
                    }
                    else
                    {
                        if (Convert.ToDecimal(dgv.Cells["dgvtxtLedgerPostingId"].Value.ToString()) != 0)
                        {
                            decimal decReconcileId = spBankReconciliation.BankReconciliationLedgerPostingId(Convert.ToDecimal(dgv.Cells["dgvtxtLedgerPostingId"].Value.ToString()));
                            if (decReconcileId != 0)
                            {
                                spBankReconciliation.BankReconciliationDelete(decReconcileId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Find sum of Dr And Cr balance will be calculate here
        /// </summary>
        public void FindTotal()
        {
            try
            {
                decimal DrCompany = 0;
                decimal CrCompany = 0;
                decimal DrDifference = 0;
                decimal DrBank = 0;
                decimal CrBank = 0;
                decimal CrDifference = 0;
                if (dgvBankReconciliation.RowCount > 0)
                {
                    foreach (DataGridViewRow dgvRow in dgvBankReconciliation.Rows)
                    {
                        DrCompany += Convert.ToDecimal(dgvRow.Cells["dgvtxtDeposit"].Value.ToString());
                        CrCompany += Convert.ToDecimal(dgvRow.Cells["dgvtxtWithdraw"].Value.ToString());
                        if (dgvRow.Cells["dgvtxtStatementDate"].Value != null && dgvRow.Cells["dgvtxtStatementDate"].Value.ToString() != string.Empty)
                        {
                            DrBank += Convert.ToDecimal(dgvRow.Cells["dgvtxtDeposit"].Value.ToString());
                            CrBank += Convert.ToDecimal(dgvRow.Cells["dgvtxtWithdraw"].Value.ToString());
                        }
                    }
                }
                txtBalanceCompnyDr.Text = DrCompany.ToString();
                txtBalanceCompanyCr.Text = CrCompany.ToString();
                txtBalanceBankDr.Text = DrBank.ToString();
                txtBalanceBankCr.Text = CrBank.ToString();
                DrDifference = DrCompany - DrBank;
                CrDifference = CrCompany - CrBank;
                txtDifferenceDr.Text = DrDifference.ToString();
                txtDifferenceCr.Text = CrDifference.ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR5:" + ex.Message;
            }
        }
        #endregion
        #region Events
        
        /// <summary>
        /// Form keydown for Quick Access for save and close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBankReconciliation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR7:" + ex.Message;
            }
        }
        /// <summary>
        /// When Form Bank Reconciliation Load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBankReconciliation_Load(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR8:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR9:" + ex.Message;
            }
        }
        /// <summary>
        /// Date time picker value change for setting the new date in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpStatementFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpStatementFrom.Value;
                this.txtStatementFrom.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR10:" + ex.Message;
            }
        }
        /// <summary>
        /// text box StatementFrom leave for change date in date time picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStatementFrom_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtStatementFrom);
                if (txtStatementFrom.Text == string.Empty)
                {
                    txtStatementFrom.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpStatementFrom.Value = Convert.ToDateTime(txtStatementFrom.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR11:" + ex.Message;
            }
        }
        /// <summary>
        /// Date time picker StatrmentTo value change for setting the new date in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpStatrmentTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpStatrmentTo.Value;
                this.txtStatementTo.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR12:" + ex.Message;
            }
        }
        /// <summary>
        /// text box StatementTo leave for change date in date time picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStatementTo_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtStatementTo);
                if (txtStatementTo.Text == string.Empty)
                {
                    txtStatementTo.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpStatrmentTo.Value = Convert.ToDateTime(txtStatementTo.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR13:" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    if (dgvBankReconciliation.RowCount > 0)
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            if (Messages.SaveMessage())
                            {
                                saveOrEdit();
                                FindTotal();
                                Messages.SavedMessage();
                            }
                        }
                        else
                        {
                            saveOrEdit();
                            FindTotal();
                            Messages.SavedMessage();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("No row to save");
                    }
                    clear();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR14:" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBankAccount.SelectedIndex > -1)
                {
                    if (cmbStatus.SelectedIndex > -1)
                    {
                        if (dtpStatrmentTo.Value >= dtpStatementFrom.Value)
                        {
                            BankReconciliationFill();
                        }
                        else
                        {
                            Messages.InformationMessage("Statement date to less than statement date from");
                            txtStatementTo.Focus();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Please select status");
                        cmbStatus.Focus();
                    }
                }
                else
                {
                    Messages.InformationMessage("Please select bank account");
                    cmbBankAccount.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR15:" + ex.Message;
            }
        }
        /// <summary>
        /// Bank reconciliation Cell value changed event for Set the statement date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBankReconciliation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    DateValidation objVal = new DateValidation();
                    TextBox txtDate = new TextBox();
                    if (!dgvBankReconciliation.Rows[e.RowIndex].Cells["dgvtxtStatementDate"].ReadOnly)
                    {
                        if (dgvBankReconciliation.Rows[e.RowIndex].Cells["dgvtxtStatementDate"].Value != null && dgvBankReconciliation.Rows[e.RowIndex].Cells["dgvtxtStatementDate"].Value.ToString() != string.Empty)
                        {
                            txtDate.Text = dgvBankReconciliation.Rows[e.RowIndex].Cells["dgvtxtStatementDate"].Value.ToString();
                            bool isDate = objVal.DateValidationFunction(txtDate);
                            if (isDate)
                            {
                                dgvBankReconciliation.Rows[e.RowIndex].Cells["dgvtxtStatementDate"].Value = txtDate.Text;
                            }
                            else
                            {
                                dgvBankReconciliation.Rows[e.RowIndex].Cells["dgvtxtStatementDate"].Value = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR16:" + ex.Message;
            }
        }
        /// <summary>
        /// When the state of a Gridview cell changes in relation to a change in its contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBankReconciliation_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvBankReconciliation.IsCurrentCellDirty)
                {
                    dgvBankReconciliation.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR17:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the Total Amount calculation In dataGridView CellEndEdit Event,its call once complete the Editing of a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBankReconciliation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                FindTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR18:" + ex.Message;
            }
        }
        #endregion
        #region Navigations
        /// <summary>
        /// For EnterKey Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbStatus.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR19:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtStatementFrom.Focus();
                    txtStatementFrom.SelectionStart = 0;
                    txtStatementFrom.SelectionLength = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbBankAccount.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR20:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStatementFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtStatementTo.Focus();
                    txtStatementTo.SelectionStart = 0;
                    txtStatementTo.SelectionLength = 0;
                }
                if (txtStatementFrom.Text == string.Empty || txtStatementFrom.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbStatus.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR21:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStatementTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtStatementTo.Text == string.Empty || txtStatementTo.SelectionStart == 0)
                    {
                        txtStatementFrom.Focus();
                        txtStatementFrom.SelectionLength = 0;
                        txtStatementTo.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR22:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvBankReconciliation.RowCount > 0)
                    {
                        dgvBankReconciliation.Focus();
                    }
                    else
                    {
                        btnSave.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtStatementTo.Focus();
                    txtStatementTo.SelectionStart = 0;
                    txtStatementTo.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR23:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBankReconciliation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    SendKeys.Send("{tab}");
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvBankReconciliation.CurrentCell == dgvBankReconciliation.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        btnSearch.Focus();
                        dgvBankReconciliation.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR24:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnClear.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvBankReconciliation.RowCount > 0)
                    {
                        dgvBankReconciliation.Focus();
                    }
                    else
                    {
                        btnSearch.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR25:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnClose.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR26:" + ex.Message;
            }
        }
        /// <summary>
        /// For BackSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnClear.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BR27:" + ex.Message;
            }
        }
        #endregion
    }
}
