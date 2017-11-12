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

    public partial class frmBillallocation : Form
    {
        #region Variables

        /// <summary>
        /// Public variable declaration part
        /// </summary>
        public decimal decVoucherTypeId;
        public string strTypeOfVoucher;
        public string strVoucherNo;

        #endregion

        #region Function

        /// <summary>
        /// Create instance of frmBillallocation
        /// </summary>
        public frmBillallocation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to set the fields on load and clear
        /// </summary>
        public void DefaultLoadFun()
        {
            try
            {
                dtpfromdate.Value = PublicVariables._dtCurrentDate;
                dtpfromdate.MinDate = PublicVariables._dtFromDate;
                dtpfromdate.MaxDate = PublicVariables._dtToDate;
                dtpfromdate.CustomFormat = "dd-MMMM-yyyy";
                dtptodate.Value = PublicVariables._dtCurrentDate;
                dtptodate.MinDate = PublicVariables._dtFromDate;
                dtptodate.MaxDate = PublicVariables._dtToDate;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA1:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill accountgroup combobox
        /// </summary>
        /// <param name="cmbAccountGroup"></param>
        public void AccountGroupComboFill(ComboBox cmbAccountGroup)
        {
            try
            {
                cmbAccountGroup.Enabled = true;
                AccountGroupSP spAccountGroup = new AccountGroupSP();
                spAccountGroup.BillAllocationAccountGroupFill(cmbAccountGroup, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill accountledger combobox
        /// </summary>
        /// <param name="cmbAccountLedger"></param>
        public void AccountLedgerComboFill(ComboBox cmbAccountLedger)
        {
            try
            {
                cmbAccountLedger.Enabled = true;
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                spAccountLedger.BillAllocationLedgerFill(cmbAccountLedger, cmbAccountGroup.Text, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA3:" + ex.Message;
            }
        }

        /// <summary>
        /// Function for serialnumber generation
        /// </summary>
        public void SerialNumber()
        {

            int inCount = 1;
            try
            {
                foreach (DataGridViewRow row in dgvBillAllocation.Rows)
                {
                    row.Cells["dgvtxtslno"].Value = inCount.ToString();
                    inCount++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA4:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void clear()
        {
            try
            {
                AccountGroupComboFill(cmbAccountGroup);
                cmbAccountGroup.SelectedIndex = 0;
                txtfromdate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txttodate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                btnReset.Enabled = true;
                BillAllocationGridFill();
                txtfromdate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA5:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the grid based on the Search keys
        /// </summary>
        public void BillAllocationGridFill()
        {
            try
            {
                PartyBalanceSP sppartybalance = new PartyBalanceSP();
                if (cmbAccountGroup.SelectedIndex > -1 && cmbAccountLedger.SelectedIndex > -1)
                {
                    if (cmbAccountGroup.SelectedItem.ToString() != "System.Data.DataRowView" || cmbAccountGroup.Text != "System.Data.DataRowView" && cmbAccountLedger.SelectedItem.ToString() != "System.Data.DataRowView" || cmbAccountLedger.Text != "System.Data.DataRowView")
                    {
                        if (Convert.ToDateTime(txtfromdate.Text) <= Convert.ToDateTime(txttodate.Text))
                        {
                            dgvBillAllocation.DataSource = sppartybalance.BillAllocationSearch(Convert.ToDateTime(txtfromdate.Text), Convert.ToDateTime(txttodate.Text), cmbAccountGroup.Text, cmbAccountLedger.Text);
                        }
                        else
                        {

                            Messages.InformationMessage("Fromdate should be less than or equal to todate");
                            txttodate.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA6:" + ex.Message;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBillallocation_Load(object sender, EventArgs e)
        {
            try
            {
                AccountGroupComboFill(cmbAccountGroup);
                cmbAccountGroup.SelectedIndex = 0;
                DefaultLoadFun();
                btnReset.Enabled = true;
                BillAllocationGridFill();
                txtfromdate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA7:" + ex.Message;
            }
        }
        
        /// <summary>
        /// On changing the selected accountgroup,the accountledger combobox will fill corresponding ledgers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AccountLedgerComboFill(cmbAccountLedger);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA8:" + ex.Message;
            }

        }

        /// <summary>
        /// Insert serial number while adding new rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBillAllocation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SerialNumber();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA9:" + ex.Message;
            }

        }


        /// <summary>
        /// on changing the date from dtpfromdate, it set the date in txtfromdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpfromdate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpfromdate.Value;
                this.txtfromdate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA10:" + ex.Message;
            }
        }


        /// <summary>
        /// validate the date while leaving from txtfromdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtfromdate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtfromdate);
                if (txtfromdate.Text == string.Empty)
                {
                    txtfromdate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----

                string strdate = txtfromdate.Text;
                dtpfromdate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA11:" + ex.Message;
            }
        }

        /// <summary>
        /// on changing the date from dtptodate, it set the date in txttodate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtptodate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtptodate.Value;
                this.txttodate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA12:" + ex.Message;
            }
        }

        /// <summary>
        /// validate the date while leaving from txttodate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txttodate_Leave(object sender, EventArgs e)
        {
            try
            {
                //    ---for change date in Date time picker----
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txttodate);
                if (txttodate.Text == string.Empty)
                {
                    txttodate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txttodate.Text;
                dtptodate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA13:" + ex.Message;
            }

        }

        /// <summary>
        /// On search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BillAllocationGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA14:" + ex.Message;
            }

        }

        /// <summary>
        /// On double clicking the cell in grid, it loads the corresponding Voucher to update or delete the entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBillAllocation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    decVoucherTypeId = Convert.ToDecimal(dgvBillAllocation.CurrentRow.Cells["voucherTypeId"].Value.ToString());

                    strTypeOfVoucher = dgvBillAllocation.CurrentRow.Cells["typeOfVoucher"].Value.ToString();
                    strVoucherNo = dgvBillAllocation.CurrentRow.Cells["voucherNo"].Value.ToString();
                    if (strTypeOfVoucher == "PDC Payable")
                    {
                        PDCPayableMasterSP sp = new PDCPayableMasterSP();
                        decimal decMasterId = sp.PdcPayableMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmPdcPayable frmpdcPayableObj = new frmPdcPayable();
                        frmpdcPayableObj = Application.OpenForms["frmPdcPayable"] as frmPdcPayable;

                        if (frmpdcPayableObj == null)
                        {
                            frmpdcPayableObj = new frmPdcPayable();
                            frmpdcPayableObj.MdiParent = formMDI.MDIObj;

                            frmpdcPayableObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                    if (strTypeOfVoucher == "PDC Receivable")
                    {
                        PDCReceivableMasterSP sp = new PDCReceivableMasterSP();
                        decimal decMasterId = sp.PdcReceivableMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmPdcReceivable frmPdcReceivableObj = new frmPdcReceivable();
                        frmPdcReceivableObj = Application.OpenForms["frmPdcReceivable"] as frmPdcReceivable;
                        if (frmPdcReceivableObj == null)
                        {
                            frmPdcReceivableObj = new frmPdcReceivable();
                            frmPdcReceivableObj.MdiParent = formMDI.MDIObj;
                            frmPdcReceivableObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                    if (strTypeOfVoucher == "Payment Voucher")
                    {
                        PaymentMasterSP sp = new PaymentMasterSP();
                        decimal decMasterId = sp.paymentMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmPaymentVoucher frmPaymentVoucherObj = new frmPaymentVoucher();
                        frmPaymentVoucherObj = Application.OpenForms["frmPaymentVoucher"] as frmPaymentVoucher;
                        if (frmPaymentVoucherObj == null)
                        {
                            frmPaymentVoucherObj = new frmPaymentVoucher();
                            frmPaymentVoucherObj.MdiParent = formMDI.MDIObj;

                            frmPaymentVoucherObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                    if (strTypeOfVoucher == "Receipt Voucher")
                    {

                        ReceiptMasterSP sp = new ReceiptMasterSP();
                        decimal decMasterId = sp.ReceiptMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmReceiptVoucher frmReceiptVoucherObj = new frmReceiptVoucher();
                        frmReceiptVoucherObj = Application.OpenForms["frmReceiptVoucher"] as frmReceiptVoucher;
                        if (frmReceiptVoucherObj == null)
                        {
                            frmReceiptVoucherObj = new frmReceiptVoucher();
                            frmReceiptVoucherObj.MdiParent = formMDI.MDIObj;

                            frmReceiptVoucherObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                    if (strTypeOfVoucher == "Journal Voucher")
                    {
                        JournalMasterSP sp = new JournalMasterSP();
                        decimal decMasterId = sp.JournalMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmJournalVoucher frmJournalVoucherObj = new frmJournalVoucher();
                        frmJournalVoucherObj = Application.OpenForms["frmJournalVoucher"] as frmJournalVoucher;
                        if (frmJournalVoucherObj == null)
                        {
                            frmJournalVoucherObj = new frmJournalVoucher();
                            frmJournalVoucherObj.MdiParent = formMDI.MDIObj;

                            frmJournalVoucherObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                    if (strTypeOfVoucher == "Credit Note")
                    {
                        CreditNoteMasterSP sp = new CreditNoteMasterSP();
                        decimal decMasterId = sp.CreditNoteMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmCreditNote frmCreditNoteObj = new frmCreditNote();
                        frmCreditNoteObj = Application.OpenForms["frmCreditNote"] as frmCreditNote;
                        if (frmCreditNoteObj == null)
                        {
                            frmCreditNoteObj = new frmCreditNote();
                            frmCreditNoteObj.MdiParent = formMDI.MDIObj;

                            frmCreditNoteObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                    if (strTypeOfVoucher == "Debit Note")
                    {
                        DebitNoteMasterSP sp = new DebitNoteMasterSP();
                        decimal decMasterId = sp.DebitNoteMasterIdView(decVoucherTypeId, strVoucherNo);

                        frmDebitNote frmDebitNoteObj = new frmDebitNote();
                        frmDebitNoteObj = Application.OpenForms["frmDebitNote"] as frmDebitNote;
                        if (frmDebitNoteObj == null)
                        {
                            frmDebitNoteObj = new frmDebitNote();
                            frmDebitNoteObj.MdiParent = formMDI.MDIObj;

                            frmDebitNoteObj.CallFromBillAllocation(this, decMasterId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA15:" + ex.Message;
            }
        }

        /// <summary>
        /// On reset button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA16:" + ex.Message;
            }

        }
        #endregion

        #region Navigation

        /// <summary>
        /// Enter key navigation for txtfromdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtfromdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txttodate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA17:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for txttodate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txttodate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountGroup.Focus();
                }
                else  if (e.KeyCode == Keys.Back)
                {
                    if (txttodate.Text == string.Empty || txttodate.SelectionStart == 0)
                    {
                        txtfromdate.Focus();
                        txtfromdate.SelectionStart = 0;
                        txtfromdate.SelectionLength = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA18:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation of accountgroup combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountLedger.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txttodate.SelectionStart == 0)
                    {
                        txttodate.Focus();
                        txttodate.SelectionStart = 0;
                        txttodate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA19:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation of accountledger combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbAccountGroup.SelectionStart == 0)
                    {
                        cmbAccountGroup.Focus();
                        cmbAccountLedger.SelectionStart = 0;
                        cmbAccountGroup.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA20:" + ex.Message;
            }
        }

        /// <summary>
        /// backspace navigation of btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbAccountLedger.SelectionStart == 0)
                    {
                        cmbAccountLedger.Focus();
                        cmbAccountLedger.SelectionStart = 0;
                        cmbAccountLedger.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BA21:" + ex.Message;
            }
        }

        /// <summary>
        /// Backspace navigation of btnReset
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
                formMDI.infoError.ErrorString = "BA22:" + ex.Message;
            }
        }

        /// <summary>
        /// For shortcut key
        /// Escape for formclosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBillallocation_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "BA23:" + ex.Message;
            }
        }
        #endregion
}
}





























