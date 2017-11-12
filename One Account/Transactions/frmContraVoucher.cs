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
    public partial class frmContraVoucher : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decContraSuffixPrefixId = 0;//to store the selected voucher type's suffixpreffixid from frmVoucherTypeSelection
        decimal DecContraVoucherTypeId = 0;//to get the selected voucher type id from frmVoucherTypeSelection
        decimal decMasterId = 0;//ContramasterId to edit delete and print
        decimal decSelectedCurrencyRate = 0;//To select current currency rate
        decimal decAmount = 0;//Amount to be convert 
        decimal decConvertRate = 0;//Converted amount
        string strBankOrCashAccount;//to get the selected value in cmbBankOrCash at teh time of ledger popup
        string strVoucherNo = string.Empty;//to save voucher no into tbl_Contra master
        string strInvoiceNo = string.Empty;//to save invoice no into tbl_Contra master
        string strPrefix = string.Empty;//to get the prefix string from frmvouchertypeselection
        string strSuffix = string.Empty;//to get the suffix string from frmvouchertypeselection
        string tableName = "ContraMaster";//to get the table name in voucher type selection
        int inNarrationCount = 0;//To check no. of lines in txtNarration
        int inArrOfRemove = 0;//number of rows removed by clicking remove button
        bool isAutomatic = false;//To checking vocher no generation auto or not
        bool isEditMode = false;//To indicate whether the form is editmode or not
        bool isValueChanged = false;//To check grid details values incomplete or not
        bool isBankAcocunt = false;//To checking whether selected gridcombo box bank or not
        ArrayList arrlstOfRemove = new ArrayList();//to get the removed rows contraDetailsId
        frmCurrencyDetails frmCurrencyObj = new frmCurrencyDetails();//to use in call from currency function
        frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();//to use in call from account ledger functio
        frmLedgerPopup frmLedgerPopupObj;//to use in call from ledger popup function
        frmContraRegister frmContraRegisterObj = null;//to use in call from contra register function
        frmContraReport frmContraReportObj = null;//to use in call from contra report function
        frmDayBook frmDayBookObj = null;//to use in call from dayBook Report
        frmAgeingReport frmAgeingObj = null;//to use in call from Ageing Report
        frmChequeReport frmChequeReportObj = null; //to use in call from Cheque Report
        SettingsSP spSettings = new SettingsSP();//to select data from settings table
        frmVoucherSearch frmsearch = null;
        frmLedgerDetails frmLedgerDetailsObj = null;//to use in Call From LedgerDetails
        #endregion
        #region Functions
        /// <summary>
        /// Create an Instance of a frmContraVoucher class
        /// </summary>
        public frmContraVoucher()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Combofill function of Cash or Bank
        /// </summary>
        /// <param name="cmbBankOrcash"></param>
        public void BankOrCashComboFill(ComboBox cmbBankOrcash)
        {
            try
            {
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                Obj.CashOrBankComboFill(cmbBankOrcash, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV1:" + ex.Message;
            }
        }
        /// <summary>
        /// Combo fill function of CashorBank in Grid
        /// </summary>
        public void GridBankOrCashComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ContraDetailsSP spContraDetails = new ContraDetailsSP();
                dtbl = spContraDetails.CashOrBankComboFill();
                DataRow drow = dtbl.NewRow();
                drow["ledgerName"] = string.Empty;
                drow["ledgerId"] = 0;
                dtbl.Rows.InsertAt(drow, 0);
                dgvcmbBankorCashAccount.DataSource = dtbl;
                dgvcmbBankorCashAccount.DisplayMember = "ledgerName";
                dgvcmbBankorCashAccount.ValueMember = "ledgerId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV2:" + ex.Message;
            }
        }
        /// <summary>
        /// Here fill the currency combobox in the grid
        /// </summary>
        public void GridCurrencyComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
                dtbl = TransactionGeneralFillObj.CurrencyComboByDate(Convert.ToDateTime(dtpContraVoucherDate.Value));
                dgvcmbCurrency.DataSource = dtbl;
                DataRow drow = dtbl.NewRow();
                drow["currencyName"] = string.Empty;
                drow["exchangeRateId"] = 0;
                dtbl.Rows.InsertAt(drow, 0);
                dgvcmbCurrency.DisplayMember = "currencyName";
                dgvcmbCurrency.ValueMember = "exchangeRateId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV3:" + ex.Message;
            }
        }
        /// <summary>
        /// generate the serial no for the grid here
        /// </summary>
        public void SlNo()
        {
            try
            {
                int inRowNo = 1;
                foreach (DataGridViewRow dr in dgvContraVoucher.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowNo;
                    inRowNo++;
                    if (dr.Index == dgvContraVoucher.Rows.Count - 1)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV4:" + ex.Message;
            }
        }
        /// <summary>
        /// Total amount calculation of Grid rows and the calculate the exchange rate if the currency conversion is available
        /// </summary>
        public void TotalAmount()
        {
            try
            {
                decimal decTotalAmount = 0;
                decimal decSelectedCurrencyRate = 0;
                ExchangeRateSP SpExchangRate = new ExchangeRateSP();
                foreach (DataGridViewRow dr in dgvContraVoucher.Rows)
                {
                    if (dr.Cells["dgvtxtAmount"].Value != null && dr.Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                    {
                        if (dr.Cells["dgvcmbCurrency"].Value != null)
                        {
                            decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dr.Cells["dgvcmbCurrency"].Value.ToString()));//Exchange rate of grid's row
                            decTotalAmount = decTotalAmount + (Convert.ToDecimal(dr.Cells["dgvtxtAmount"].Value.ToString()) * decSelectedCurrencyRate);
                        }
                        else
                        {
                            decTotalAmount = decTotalAmount + Convert.ToDecimal(dr.Cells["dgvtxtAmount"].Value.ToString());
                        }
                    }
                }
                txtTotal.Text = Math.Round(decTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV5:" + ex.Message;
            }
        }
        /// <summary>
        /// Its for Search and fill the Currency's are available in current date 
        /// </summary>
        /// <param name="frmCurrencyDetails"></param>
        /// <param name="decId"></param>
        public void CallFromCurrenCyDetails(frmCurrencyDetails frmCurrencyDetails, decimal decId)
        {
            try
            {
                decimal decExchangeRateId = 0;
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                decExchangeRateId = spExchangeRate.GetExchangeRateId(decId, Convert.ToDateTime(txtContraVoucherDate.Text));
                dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value = decExchangeRateId;
                base.Show();
                this.frmCurrencyObj = frmCurrencyDetails;
                frmCurrencyObj.Close();
                frmCurrencyObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV6:" + ex.Message;
            }
        }
        /// <summary>
        /// Its for validate the amount field (Invalid entries) in grid keypress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvtxtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvContraVoucher.CurrentCell != null)
                {
                    if (dgvContraVoucher.Columns[dgvContraVoucher.CurrentCell.ColumnIndex].Name == "dgvtxtAmount")
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV7:" + ex.Message;
            }
        }
        /// <summary>
        /// These function for generating the voucher no automatically if its automatic with preffix and sufix
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                ContraMasterSP spContraMaster = new ContraMasterSP();
                if (strVoucherNo == string.Empty)
                {
                    strVoucherNo = "0";
                }
                strVoucherNo = obj.VoucherNumberAutomaicGeneration(DecContraVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpContraVoucherDate.Value, tableName);
                if (Convert.ToDecimal(strVoucherNo) != spContraMaster.ContraVoucherMasterGetMaxPlusOne(DecContraVoucherTypeId))
                {
                    strVoucherNo = spContraMaster.ContraVoucherMasterGetMax(DecContraVoucherTypeId).ToString();
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(DecContraVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpContraVoucherDate.Value, tableName);
                    if (spContraMaster.ContraVoucherMasterGetMax(DecContraVoucherTypeId) == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(DecContraVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpContraVoucherDate.Value, tableName);
                    }
                }
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(DecContraVoucherTypeId, dtpContraVoucherDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    decContraSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.Text = strInvoiceNo;
                    txtVoucherNo.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV8:" + ex.Message;
            }
        }
        /// <summary>
        /// These function is used to reset the form and clear its controlls
        /// </summary>
        public void Clear()
        {
            try
            {
                if (isAutomatic)
                {
                    VoucherNumberGeneration();
                    txtContraVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Text = string.Empty;
                    txtVoucherNo.ReadOnly = false;
                }
                BankOrCashComboFill(cmbBankAccount);
                GridBankOrCashComboFill();
                isEditMode = false;
                dtpContraVoucherDate.MinDate = PublicVariables._dtFromDate;
                dtpContraVoucherDate.MaxDate = PublicVariables._dtToDate;
                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();
                infoComapany = spCompany.CompanyView(1);
                DateTime dtVoucherDate = infoComapany.CurrentDate;
                dtpContraVoucherDate.Value = dtVoucherDate;
                txtContraVoucherDate.Text = dtVoucherDate.ToString("dd-MMM-yyyy");
                dtpContraVoucherDate.Value = Convert.ToDateTime(txtContraVoucherDate.Text);
                txtContraVoucherDate.Focus();
                txtContraVoucherDate.SelectAll();
                if (txtVoucherNo.ReadOnly)
                {
                    txtContraVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
                cmbBankAccount.SelectedIndex = -1;
                txtNarration.Text = string.Empty;
                txtTotal.Text = string.Empty;
                dgvContraVoucher.ClearSelection();
                rbtnDeposit.Checked = true;
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                dgvContraVoucher.Rows.Clear();
                if (frmContraRegisterObj != null)
                {
                    frmContraRegisterObj.Close();
                }
                if (frmContraReportObj != null)
                {
                    frmContraReportObj.Close();
                }
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("TickPrintAfterSave") == "Yes")
                {
                    cbxPrintafterSave.Checked = true;
                }
                else
                {
                    cbxPrintafterSave.Checked = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV9:" + ex.Message;
            }
        }
        /// <summary>
        /// its a function for vouchertype selection form to select purticular voucher and open the form under the vouchertype
        /// </summary>
        /// <param name="decDailyContraVoucherTypeId"></param>
        /// <param name="strVoucherTypeNames1"></param>
        public void CallFromVoucherTypeSelection(decimal decDailyContraVoucherTypeId, string strVoucherTypeNames1)
        {
            try
            {
                DecContraVoucherTypeId = decDailyContraVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(DecContraVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(DecContraVoucherTypeId, dtpContraVoucherDate.Value);
                decContraSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                strPrefix = infoSuffixPrefix.Prefix;
                strSuffix = infoSuffixPrefix.Suffix;
                this.Text = strVoucherTypeNames1;
                base.Show();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV10:" + ex.Message;
            }
        }
        /// <summary>
        /// Its the function for find and select purticular cash or bank ledger in a popup window
        /// </summary>
        /// <param name="frmLedgerPopup"></param>
        /// <param name="decId"></param>
        /// <param name="strComboTypes"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId, string strComboTypes) //PopUp
        {
            try
            {
                base.Show();
                this.frmLedgerPopupObj = frmLedgerPopup;
                if (strComboTypes == "CashOrBank")
                {
                    TransactionsGeneralFill obj = new TransactionsGeneralFill();
                    obj.CashOrBankComboFill(cmbBankAccount, false);
                    cmbBankAccount.SelectedValue = decId;
                }
                else
                {
                    dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value = decId;
                }
                frmLedgerPopupObj.Close();
                frmLedgerPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV11:" + ex.Message;
            }
        }
        /// <summary>
        /// Its the function for find and search new account ledger under bank or cash and add these combo fill
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAccountLedgerForm(decimal decId)
        {
            try
            {
                BankOrCashComboFill(cmbBankAccount);
                if (decId != 0)
                {
                    cmbBankAccount.SelectedValue = decId.ToString();
                }
                else if (strBankOrCashAccount != string.Empty)
                {
                    cmbBankAccount.SelectedValue = strBankOrCashAccount;
                }
                else
                {
                    cmbBankAccount.SelectedIndex = -1;
                }
                this.Enabled = true;
                this.BringToFront();
                GridBankOrCashComboFill();
                cmbBankAccount.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV12:" + ex.Message;
            }
        }
        /// <summary>
        /// Used to find and add the ledger under cash or bank in grid combobos here
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAccountLedgerFormForGridCombo(decimal decId)
        {
            try
            {
                GridBankOrCashComboFill();
                if (decId != 0)
                {
                    dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value = decId;
                }
                else if (strBankOrCashAccount != string.Empty)
                {
                    dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value = strBankOrCashAccount;
                }
                this.Enabled = true;
                GridBankOrCashComboFill();
                dgvContraVoucher.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV13:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking VoucherNo existance for save or update and call its curresponding function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                ContraMasterSP spContraMaster = new ContraMasterSP();
                if (!isEditMode)
                {
                    if (PublicVariables.isMessageAdd)
                    {
                        if (Messages.SaveMessage())
                        {
                            if (!isAutomatic)
                            {
                                if (spContraMaster.ContraVoucherCheckExistence(txtVoucherNo.Text.Trim(), DecContraVoucherTypeId, 0) == false)
                                {
                                    SaveFuction();
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                            }
                            else
                            {
                                if (spContraMaster.ContraVoucherCheckExistence(txtVoucherNo.Text.Trim(), DecContraVoucherTypeId, 0) == false)
                                {
                                    SaveFuction();
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!isAutomatic)
                        {
                            if (spContraMaster.ContraVoucherCheckExistence(txtVoucherNo.Text.Trim(), DecContraVoucherTypeId, 0) == false)
                            {
                                SaveFuction();
                            }
                            else
                            {
                                Messages.InformationMessage("Voucher number already exist");
                            }
                        }
                        else
                        {
                            SaveFuction();
                        }
                    }
                }
                else if (isEditMode)
                {
                    if (PublicVariables.isMessageEdit)
                    {
                        if (Messages.UpdateMessage())
                        {
                            if (!isAutomatic)
                            {
                                if (spContraMaster.ContraVoucherCheckExistence(txtVoucherNo.Text.Trim(), DecContraVoucherTypeId, decMasterId) == false)
                                {
                                    EditFunction(decMasterId);
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                            }
                            else
                            {
                                EditFunction(decMasterId);
                            }
                        }
                    }
                    else
                    {
                        if (!isAutomatic)
                        {
                            if (spContraMaster.ContraVoucherCheckExistence(txtVoucherNo.Text.Trim(), DecContraVoucherTypeId, decMasterId) == false)
                            {
                                EditFunction(decMasterId);
                            }
                            else
                            {
                                Messages.InformationMessage("Voucher number already exist");
                            }
                        }
                        else
                        {
                            EditFunction(decMasterId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV14:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking invalid entries for save or update and call its curresponding function
        /// </summary>
        public void SaveOrEditFuction()
        {
            try
            {
                decimal decCalcAmount = 0;
                decimal decBalance = 0;
                string strStatus = string.Empty;
                ContraMasterSP spContraMaster = new ContraMasterSP();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                SettingsSP spSettings = new SettingsSP();
                if (txtVoucherNo.Text == string.Empty)
                {
                    Messages.InformationMessage("Enter voucher number");
                    txtVoucherNo.Focus();
                }
                else if (cmbBankAccount.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select cash or bank account");
                    cmbBankAccount.Focus();
                }
                else
                {
                    if (RemoveIncompleteRowsFromGrid())
                    {
                        if (dtpContraVoucherDate.Value.ToString() != string.Empty)
                        {
                            strStatus = spSettings.SettingsStatusCheck("NegativeCashTransaction");
                            if (rbtnWithdrawal.Checked)
                            {
                                decBalance = spAccountLedger.CheckLedgerBalance(Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString()));
                                decCalcAmount = decBalance - Convert.ToDecimal(txtTotal.Text);
                                if (decCalcAmount < 0)
                                {
                                    if (strStatus == "Warn")
                                    {
                                        if (MessageBox.Show("Negative balance exists,Do you want to Continue", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                        {
                                            SaveOrEdit();
                                        }
                                    }
                                    else if (strStatus == "Block")
                                    {
                                        MessageBox.Show("Cannot continue ,due to negative balance", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    }
                                    else
                                    {
                                        SaveOrEdit();
                                    }
                                }
                                else
                                {
                                    SaveOrEdit();
                                }
                            }
                            else
                            {
                                bool isNegativeLedger = false;
                                int inRowCount = dgvContraVoucher.RowCount;
                                for (int i = 0; i < inRowCount - 1; i++)
                                {
                                    decCalcAmount = 0;
                                    decimal decledgerId = 0;
                                    if (dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString() != "")
                                    {
                                        decledgerId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString());
                                    }
                                    decBalance = spAccountLedger.CheckLedgerBalance(decledgerId);
                                    decCalcAmount = decBalance - Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                                    if (decCalcAmount < 0)
                                    {
                                        isNegativeLedger = true;
                                        break;
                                    }
                                }
                                if (isNegativeLedger)
                                {
                                    if (strStatus == "Warn")
                                    {
                                        if (MessageBox.Show("Negative balance exists,Do you want to Continue", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                        {
                                            SaveOrEdit();
                                        }
                                    }
                                    else if (strStatus == "Block")
                                    {
                                        MessageBox.Show("Cannot continue ,due to negative balance", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    }
                                    else
                                    {
                                        SaveOrEdit();
                                    }
                                }
                                else
                                {
                                    SaveOrEdit();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV15:" + ex.Message;
            }
        }
        /// <summary>
        /// Save function Call the ledger posting function also
        /// </summary>
        public void SaveFuction()
        {
            try
            {
                decimal decContraDetailsId = 0;
                ContraMasterSP spContraMaster = new ContraMasterSP();
                ContraMasterInfo infoContraMaster = new ContraMasterInfo();
                ContraDetailsSP spContraDetails = new ContraDetailsSP();
                ContraDetailsInfo infoCOntraDetails = new ContraDetailsInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                decimal decIdentity = 0;
                decimal decLedgerId = 0;
                decimal decDebit = 0;
                decimal decCredit = 0;
                int inCount = dgvContraVoucher.RowCount;
                int inValue = 0;
                for (int i = 0; i < inCount - 1; i++)
                {
                    if (dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value != null &&
                        dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString() != null)
                    {
                        inValue++;
                    }
                }
                if (inValue > 0)
                {
                    if (Convert.ToDecimal(txtTotal.Text) != 0)
                    {
                        infoContraMaster.LedgerId = Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString());
                        if (isAutomatic)
                        {
                            infoContraMaster.VoucherNo = strVoucherNo;
                        }
                        else
                        {
                            decimal decVoucherNo = spContraMaster.ContraVoucherMasterGetMaxPlusOne(DecContraVoucherTypeId) + 1;
                            infoContraMaster.VoucherNo = Convert.ToString(decVoucherNo);
                        }
                        infoContraMaster.Date = Convert.ToDateTime(txtContraVoucherDate.Text.ToString());
                        infoContraMaster.Narration = txtNarration.Text.Trim();
                        infoContraMaster.TotalAmount = Convert.ToDecimal(txtTotal.Text);
                        infoContraMaster.Extra1 = string.Empty;
                        infoContraMaster.Extra2 = string.Empty;
                        if (rbtnDeposit.Checked)
                        {
                            infoContraMaster.Type = "Deposit";
                        }
                        else
                        {
                            infoContraMaster.Type = "Withdraw";
                        }
                        infoContraMaster.SuffixPrefixId = decContraSuffixPrefixId;
                        infoContraMaster.VoucherTypeId = DecContraVoucherTypeId;
                        infoContraMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                        infoContraMaster.UserId = PublicVariables._decCurrentUserId;
                        if (isAutomatic)
                        {
                            infoContraMaster.InvoiceNo = strInvoiceNo;
                        }
                        else
                        {
                            infoContraMaster.InvoiceNo = txtVoucherNo.Text;
                        }
                        decIdentity = spContraMaster.ContraMasterAdd(infoContraMaster);
                        infoCOntraDetails.ContraMasterId = decIdentity;
                        infoCOntraDetails.Extra1 = string.Empty;
                        infoCOntraDetails.Extra2 = string.Empty;
                        int inRowCount = dgvContraVoucher.RowCount;
                        //-------------------------------Saving grid details--------------------------------------------------------------------
                        for (int i = 0; i < inRowCount - 1; i++)
                        {
                            if (dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString() != string.Empty)
                            {
                                infoCOntraDetails.LedgerId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString());
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                            {
                                infoCOntraDetails.Amount = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                            {
                                infoCOntraDetails.ChequeNo = dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value.ToString();
                            }
                            else
                            {
                                infoCOntraDetails.ChequeNo = string.Empty;
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                            {
                                infoCOntraDetails.ChequeDate = Convert.ToDateTime(dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value.ToString());
                            }
                            else
                            {
                                infoCOntraDetails.ChequeDate = Convert.ToDateTime("1/1/1753");
                            }
                            infoCOntraDetails.ExchangeRateId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value.ToString());
                            decContraDetailsId = spContraDetails.ContraDetailsAddReturnWithhIdentity(infoCOntraDetails);
                            //---------------------------------------------------------Ledger Posting-----------------------------------------/---------------------------------------------------/
                            if (dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString() != string.Empty)
                            {
                                decLedgerId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString());
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                            {
                                decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value));
                                decAmount = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                                decConvertRate = decAmount * decSelectedCurrencyRate;
                                if (rbtnDeposit.Checked)
                                {
                                    decCredit = decConvertRate;
                                    decDebit = 0;
                                    LedgerPosting(decLedgerId, decCredit, decDebit, decContraDetailsId, i);
                                }
                                else
                                {
                                    decDebit = decConvertRate;
                                    decCredit = 0;
                                    LedgerPosting(decLedgerId, decCredit, decDebit, decContraDetailsId, i);
                                }
                            }
                        }
                        decAmount = Convert.ToDecimal(txtTotal.Text);
                        decContraDetailsId = 0;
                        if (rbtnDeposit.Checked)
                        {
                            decDebit = decAmount;
                            decCredit = 0;
                            LedgerPosting(infoContraMaster.LedgerId, decCredit, decDebit, decContraDetailsId, -1);
                        }
                        else
                        {
                            decCredit = decAmount;
                            decDebit = 0;
                            LedgerPosting(infoContraMaster.LedgerId, decCredit, decDebit, decContraDetailsId, -1);
                        }
                        //------------------------------------------------------------------Ledger Posting---end---------------------------------------------------------------------//
                        Messages.SavedMessage();
                        if (cbxPrintafterSave.Checked)
                        {
                            if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                            {
                                PrintForDotMatrix(decIdentity);
                            }
                            else
                            {
                                Print(decIdentity);
                            }
                        }
                        Clear();
                    }
                    else
                    {
                        Messages.InformationMessage("Cannot save total debit and credit as 0");
                        dgvContraVoucher.Focus();
                    }
                }
                else
                {
                    Messages.InformationMessage("Cant save contra voucher without atleast one ledger with complete details");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV16:" + ex.Message;
            }
        }
        /// <summary>
        /// Edit function Call the ledger posting Edit function also
        /// </summary>
        /// <param name="decMasterId"></param>
        public void EditFunction(decimal decMasterId)
        {
            try
            {
                ContraMasterSP spContraMaster = new ContraMasterSP();
                ContraMasterInfo infoContraMaster = new ContraMasterInfo();
                ContraDetailsSP spContraDetails = new ContraDetailsSP();
                ContraDetailsInfo infoCOntraDetails = new ContraDetailsInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                decimal decContraDetailsId = 0;
                decimal decLedgerPostId = 0;
                infoContraMaster.ContraMasterId = decMasterId;
                infoContraMaster.LedgerId = Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString());
                if (isAutomatic)
                {
                    infoContraMaster.VoucherNo = strVoucherNo;
                }
                if (isAutomatic)
                {
                    infoContraMaster.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoContraMaster.InvoiceNo = txtVoucherNo.Text;
                }
                infoContraMaster.Date = Convert.ToDateTime(dtpContraVoucherDate.Text.ToString());
                infoContraMaster.Narration = txtNarration.Text.Trim();
                infoContraMaster.TotalAmount = Convert.ToDecimal(txtTotal.Text);
                if (rbtnDeposit.Checked)
                    infoContraMaster.Type = "Deposit";
                else
                    infoContraMaster.Type = "Withdraw";
                infoContraMaster.SuffixPrefixId = decContraSuffixPrefixId;
                infoContraMaster.VoucherTypeId = DecContraVoucherTypeId;
                infoContraMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoContraMaster.UserId = PublicVariables._decCurrentUserId;
                infoContraMaster.Extra1 = string.Empty;
                infoContraMaster.Extra2 = string.Empty;
                infoContraMaster.Date = Convert.ToDateTime(txtContraVoucherDate.Text);
                int inCount = dgvContraVoucher.RowCount;
                int inValue = 0;
                for (int i = 0; i < inCount - 1; i++)
                {
                    if (dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value != null &&
                        dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString() != null)
                    {
                        inValue++;
                    }
                }
                if (inValue > 0)
                {
                    if (Convert.ToDecimal(txtTotal.Text) != 0)
                    {
                        infoCOntraDetails.ContraMasterId = decMasterId;
                        infoCOntraDetails.Extra1 = string.Empty;
                        infoCOntraDetails.Extra2 = string.Empty;
                        //------------------deleting removed rows----------------------------------------//
                        foreach (var item in arrlstOfRemove)
                        {
                            decimal decId = Convert.ToDecimal(item);
                            spContraDetails.ContraDetailsDelete(Convert.ToDecimal(decId));
                            spLedgerPosting.LedgerPostDeleteByDetailsId(Convert.ToDecimal(decId), strVoucherNo, DecContraVoucherTypeId);
                        }
                        //--------------------------End---------------------------------------------------//
                        int inRowCount = dgvContraVoucher.RowCount;
                        decimal decLedgerId = 0;
                        decimal decLedger1Id = 0;
                        decimal decDebit = 0;
                        decimal decCredit = 0;
                        decLedger1Id = Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString());
                        for (int i = 0; i < inRowCount; i++)
                        {
                            if (dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString() != "")
                            {
                                decLedgerId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString());
                                infoCOntraDetails.LedgerId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value.ToString());
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value.ToString() != "")
                            {
                                infoCOntraDetails.ChequeNo = dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value.ToString();
                            }
                            else
                            {
                                infoCOntraDetails.ChequeNo = string.Empty;
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value.ToString() != "")
                            {
                                infoCOntraDetails.ChequeDate = Convert.ToDateTime(dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value.ToString());
                            }
                            else
                            {
                                infoCOntraDetails.ChequeDate = Convert.ToDateTime("1/1/1753");
                            }
                            if (dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString() != "")
                            {
                                infoCOntraDetails.Amount = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                                if (dgvContraVoucher.Rows[i].Cells["dgvtxtDetailsId"].Value != null && dgvContraVoucher.Rows[i].Cells["dgvtxtDetailsId"].Value.ToString() != "")
                                {
                                    infoCOntraDetails.ContraDetailsId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtDetailsId"].Value.ToString());
                                    decContraDetailsId = infoCOntraDetails.ContraDetailsId;
                                    decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value.ToString()));
                                    decAmount = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                                    decConvertRate = decAmount * decSelectedCurrencyRate;
                                    if (rbtnDeposit.Checked)
                                    {
                                        decCredit = decConvertRate;
                                        decDebit = 0;
                                    }
                                    else
                                    {
                                        decDebit = decConvertRate;
                                        decCredit = 0;
                                    }
                                    infoCOntraDetails.ContraMasterId = decMasterId;
                                    infoCOntraDetails.ExchangeRateId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value.ToString());
                                    spContraDetails.ContraDetailsEdit(infoCOntraDetails);
                                    decLedgerPostId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                    LedgerPostingEdit(decLedgerPostId, decLedgerId, decCredit, decDebit, decContraDetailsId, i);
                                }
                                else
                                {
                                    infoCOntraDetails.ExchangeRateId = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value.ToString());
                                    decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value.ToString()));
                                    decAmount = Convert.ToDecimal(dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                                    decConvertRate = decAmount * decSelectedCurrencyRate;
                                    if (rbtnDeposit.Checked)
                                    {
                                        decCredit = decConvertRate;
                                        decDebit = 0;
                                    }
                                    else
                                    {
                                        decDebit = decConvertRate;
                                        decCredit = 0;
                                    }
                                    decContraDetailsId = spContraDetails.ContraDetailsAddReturnWithhIdentity(infoCOntraDetails);
                                    LedgerPosting(decLedgerId, decCredit, decDebit, decContraDetailsId, i);
                                }
                            }
                        }
                        spContraMaster.ContraMasterEdit(infoContraMaster);
                        decLedgerPostId = spLedgerPosting.LedgerPostingIdForTotalAmount(strVoucherNo, DecContraVoucherTypeId);
                        decAmount = Convert.ToDecimal(txtTotal.Text);
                        decContraDetailsId = 0;
                        if (rbtnDeposit.Checked)
                        {
                            decDebit = decAmount;
                            decCredit = 0;
                            LedgerPostingEdit(decLedgerPostId, decLedger1Id, decCredit, decDebit, decContraDetailsId, -1);
                        }
                        else
                        {
                            decCredit = decAmount;
                            decDebit = 0;
                            LedgerPostingEdit(decLedgerPostId, decLedger1Id, decCredit, decDebit, decContraDetailsId, -1);
                        }
                        Messages.UpdatedMessage();
                        if (cbxPrintafterSave.Checked)
                        {
                            if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                            {
                                PrintForDotMatrix(decMasterId);
                            }
                            else
                            {
                                Print(decMasterId);
                            }
                        }
                        this.Close();
                    }
                    else
                    {
                        Messages.InformationMessage("Cannot save total amount as 0");
                        dgvContraVoucher.Focus();
                    }
                }
                else
                {
                    Messages.InformationMessage("Cant update contra voucher without atleast one ledger with complete details");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV17:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete Function
        /// </summary>
        /// <param name="decContraMasterId"></param>
        public void DeleteFunction(decimal decContraMasterId)
        {
            try
            {
                ContraDetailsSP spContraDetails = new ContraDetailsSP();
                spContraDetails.ContraVoucherDelete(decContraMasterId, DecContraVoucherTypeId, strVoucherNo);
                Messages.DeletedMessage();
                this.Close();
                if (frmsearch != null)
                {
                    this.Close();
                    frmsearch.GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV18:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger Posting Save Function
        /// </summary>
        /// <param name="decid"></param>
        /// <param name="decCredit"></param>
        /// <param name="decDebit"></param>
        /// <param name="decDetailsId"></param>
        /// <param name="inI"></param>
        public void LedgerPosting(decimal decid, decimal decCredit, decimal decDebit, decimal decDetailsId, int inI)
        {
            try
            {
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                infoLedgerPosting.VoucherTypeId = DecContraVoucherTypeId;
                if (isAutomatic)
                {
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoLedgerPosting.VoucherNo = txtVoucherNo.Text;
                }
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.LedgerId = decid;
                infoLedgerPosting.DetailsId = decDetailsId;
                infoLedgerPosting.Debit = decDebit;
                infoLedgerPosting.Credit = decCredit;
                if (isAutomatic)
                {
                    infoLedgerPosting.InvoiceNo = strInvoiceNo;
                }
                else
                {
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text;
                }
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                if (inI > -1)
                {
                    if (dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null)
                    {
                        infoLedgerPosting.ChequeNo = dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                    }
                    if (dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value);
                    }
                    else
                    {
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }
                }
                else
                {
                    infoLedgerPosting.ChequeNo = string.Empty;
                    infoLedgerPosting.ChequeDate = DateTime.Now;
                }
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV19:" + ex.Message;
            }
        }
        /// <summary>
        /// LedgerPosting Update Function
        /// </summary>
        /// <param name="decLedgerPostingId"></param>
        /// <param name="decLedgerId"></param>
        /// <param name="decCredit"></param>
        /// <param name="decDebit"></param>
        /// <param name="decDetailsId"></param>
        /// <param name="inI"></param>
        public void LedgerPostingEdit(decimal decLedgerPostingId, decimal decLedgerId, decimal decCredit, decimal decDebit, decimal decDetailsId, int inI)
        {
            try
            {
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.VoucherTypeId = DecContraVoucherTypeId;
                infoLedgerPosting.VoucherNo = strVoucherNo;
                infoLedgerPosting.DetailsId = decDetailsId;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.InvoiceNo = txtVoucherNo.Text;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                infoLedgerPosting.LedgerId = decLedgerId;
                infoLedgerPosting.Credit = decCredit;
                infoLedgerPosting.Debit = decDebit;
                if (inI > -1)
                {
                    if (dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                    }
                    if (dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvContraVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value);
                    }
                    else
                    {
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }
                }
                else
                {
                    infoLedgerPosting.ChequeNo = string.Empty;
                    infoLedgerPosting.ChequeDate = DateTime.Now;
                }
                spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV20:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form VoucherSearch For update or delete
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallThisFormFromVoucherSearch(frmVoucherSearch frm, decimal decId)
        {
            try
            {
                base.Show();
                this.frmsearch = frm;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                isEditMode = true;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV21:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form Contra Register For update or delete
        /// </summary>
        /// <param name="frmContraRegister"></param>
        /// <param name="decId"></param>
        public void CallFromContraRegister(frmContraRegister frmContraRegister, decimal decId)
        {
            try
            {
                base.Show();
                this.frmContraRegisterObj = frmContraRegister;
                frmContraRegisterObj.Enabled = false;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                isEditMode = true;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV22:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form Contra Report For update or delete
        /// </summary>
        /// <param name="frmContraReport"></param>
        /// <param name="decId"></param>
        public void CallFromContraReport(frmContraReport frmContraReport, decimal decId)
        {
            try
            {
                base.Show();
                this.frmContraReportObj = frmContraReport;
                frmContraReportObj.Enabled = false;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                isEditMode = true;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV23:" + ex.Message;
            }
        }
        /// <summary>
        /// CheckInvalidEntries in Grid
        /// </summary>
        /// <param name="e"></param>
        public void CheckColumnMissing(DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvContraVoucher.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].FormattedValue == null || dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].FormattedValue.ToString().Trim() == "")
                        {
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvContraVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.Cells["dgvtxtCheck"].Value = "x";
                            dgvContraVoucher["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                        }
                        else if (dgvContraVoucher.CurrentRow.Cells["dgvtxtAmount"].Value == null || dgvContraVoucher.CurrentRow.Cells["dgvtxtAmount"].Value.ToString().Trim() == "")
                        {
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvContraVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.Cells["dgvtxtCheck"].Value = "x";
                            dgvContraVoucher["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                        }
                        else if (dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"].FormattedValue == null || dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"].FormattedValue.ToString().Trim() == "")
                        {
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvContraVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.Cells["dgvtxtCheck"].Value = "x";
                            dgvContraVoucher["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.HeaderCell.Value = "";
                            isValueChanged = true;
                            dgvContraVoucher.CurrentRow.Cells["dgvtxtCheck"].Value = "";
                        }
                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV24:" + ex.Message;
            }
        }
        /// <summary>
        /// In save or update time remove the incomplete Rows from the grid
        /// </summary>
        /// <returns></returns>
        public bool RemoveIncompleteRowsFromGrid()
        {
            bool isOk = true;
            try
            {
                
                string strMessage = "Rows";
                int inC = 0, inForFirst = 0;
                int inRowcount = dgvContraVoucher.RowCount;
                int inLastRow = 1;
                foreach (DataGridViewRow dgvrowCur in dgvContraVoucher.Rows)
                {
                    if (inLastRow < inRowcount)
                    {
                        if ((dgvrowCur.Cells["dgvtxtCheck"].Value != null && dgvrowCur.Cells["dgvtxtCheck"].Value.ToString() == "x") || dgvrowCur.Cells["dgvcmbBankorCashAccount"].Value == null && dgvrowCur.Cells["dgvcmbCurrency"].Value == null)
                        {
                            isOk = false;
                            if (inC == 0)
                            {
                                strMessage = strMessage + Convert.ToString(dgvrowCur.Index + 1);
                                inForFirst = dgvrowCur.Index;
                                inC++;
                            }
                            else
                            {
                                strMessage = strMessage + ", " + Convert.ToString(dgvrowCur.Index + 1);
                            }
                        }
                    }
                    inLastRow++;
                }
                inLastRow = 1;
                if (!isOk)
                {
                    strMessage = strMessage + " contains invalid entries. Do you want to continue?";
                    if (MessageBox.Show(strMessage, "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        isOk = true;
                        for (int inK = 0; inK < dgvContraVoucher.Rows.Count; inK++)
                        {
                            if (dgvContraVoucher.Rows[inK].Cells["dgvtxtCheck"].Value != null && dgvContraVoucher.Rows[inK].Cells["dgvtxtCheck"].Value.ToString() == "x")
                            {
                                if (!dgvContraVoucher.Rows[inK].IsNewRow)
                                {
                                    dgvContraVoucher.Rows.RemoveAt(inK);
                                    inK--;
                                }
                            }
                            TotalAmount();
                            SlNo();
                        }
                    }
                    else
                    {
                        dgvContraVoucher.Rows[inForFirst].Cells["dgvcmbBankorCashAccount"].Selected = true;
                        dgvContraVoucher.CurrentCell = dgvContraVoucher.Rows[inForFirst].Cells["dgvcmbBankorCashAccount"];
                        dgvContraVoucher.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV25:" + ex.Message;
            }
            return isOk;
        }
        /// <summary>
        /// Its for using Crystal report printing
        /// </summary>
        /// <param name="decContraMasterId"></param>
        public void Print(decimal decContraMasterId)
        {
            try
            {
                ContraMasterSP spContraMaster = new ContraMasterSP();
                DataSet dsContraVoucher = spContraMaster.ContraVoucherPrinting(decContraMasterId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.ContraVoucherPrinting(dsContraVoucher);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV26:" + ex.Message;
            }
        }
        /// <summary>
        /// Print function for dotmatrix printer
        /// </summary>
        /// <param name="decContraMasterId"></param>
        public void PrintForDotMatrix(decimal decContraMasterId)
        {
            try
            {
                DataTable dtblOtherDetails = new DataTable();
                CompanySP spComapany = new CompanySP();
                dtblOtherDetails = spComapany.CompanyViewForDotMatrix();
                //-------------Grid Details-------------------\\
                DataTable dtblGridDetails = new DataTable();
                dtblGridDetails.Columns.Add("SlNo");
                dtblGridDetails.Columns.Add("CashOrBankGrid");
                dtblGridDetails.Columns.Add("Amount");
                dtblGridDetails.Columns.Add("Currency");
                dtblGridDetails.Columns.Add("Cheque No");
                dtblGridDetails.Columns.Add("Cheque Date");
                int inRowCount = 0;
                foreach (DataGridViewRow dRow in dgvContraVoucher.Rows)
                {
                    if (!dRow.IsNewRow)
                    {
                        DataRow dr = dtblGridDetails.NewRow();
                        dr["SlNo"] = ++inRowCount;
                        dr["CashOrBankGrid"] = dRow.Cells["dgvcmbBankorCashAccount"].FormattedValue.ToString();
                        dr["Amount"] = dRow.Cells["dgvtxtAmount"].Value.ToString();
                        dr["Currency"] = dRow.Cells["dgvcmbCurrency"].FormattedValue.ToString();
                        dr["Cheque No"] = (dRow.Cells["dgvtxtChequeNo"].Value == null ? "" : dRow.Cells["dgvtxtChequeNo"].Value.ToString());
                        dr["Cheque Date"] = (dRow.Cells["dgvtxtChequeDate"].Value == null ? "" : dRow.Cells["dgvtxtChequeDate"].Value.ToString());
                        dtblGridDetails.Rows.Add(dr);
                    }
                }
                //-------------Other Details-------------------\\
                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("totalAmount");
                dtblOtherDetails.Columns.Add("CashOrBank");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("Type");
                dtblOtherDetails.Columns.Add("AmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtVoucherNo.Text;
                dRowOther["date"] = txtContraVoucherDate.Text;
                dRowOther["totalAmount"] = txtTotal.Text;
                dRowOther["CashOrBank"] = cmbBankAccount.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                if (rbtnDeposit.Checked)
                {
                    dRowOther["Type"] = "Deposit";
                }
                else
                {
                    dRowOther["Type"] = "Withdraw";
                }
                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtTotal.Text), PublicVariables._decCurrencyId);
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(DecContraVoucherTypeId);
                dRowOther["Declaration"] = dtblDeclaration.Rows[0]["Declaration"].ToString();
                dRowOther["Heading1"] = dtblDeclaration.Rows[0]["Heading1"].ToString();
                dRowOther["Heading2"] = dtblDeclaration.Rows[0]["Heading2"].ToString();
                dRowOther["Heading3"] = dtblDeclaration.Rows[0]["Heading3"].ToString();
                dRowOther["Heading4"] = dtblDeclaration.Rows[0]["Heading4"].ToString();
                int inFormId = spVoucherType.FormIdGetForPrinterSettings(Convert.ToInt32(dtblDeclaration.Rows[0]["masterId"].ToString()));
                DotMatrixPrint.PrintDesign(inFormId, dtblOtherDetails, dtblGridDetails, dtblOtherDetails);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV27:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form DayBook For update or delete
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decContraMasterId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decContraMasterId)
        {
            try
            {
                frmDayBook.Enabled = false;
                base.Show();
                btnSave.Text = "Update";
                isEditMode = true;
                btnDelete.Enabled = true;
                frmDayBookObj = frmDayBook;
                decMasterId = decContraMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV:28" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form Agieng Report For update or delete
        /// </summary>
        /// <param name="frmAgeing"></param>
        /// <param name="decContraMasterId"></param>
        public void callFromAgeing(frmAgeingReport frmAgeing, decimal decContraMasterId)
        {
            try
            {
                frmAgeing.Enabled = false;
                base.Show();
                btnDelete.Enabled = true;
                frmAgeingObj = frmAgeing;
                decMasterId = decContraMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV29:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form Cheque Report For update or delete
        /// </summary>
        /// <param name="frmChequeReport"></param>
        /// <param name="decContraMasterId"></param>
        public void CallFromChequeReport(frmChequeReport frmChequeReport, decimal decContraMasterId)
        {
            try
            {
                frmChequeReport.Enabled = false;
                base.Show();
                btnDelete.Enabled = true;
                this.frmChequeReportObj = frmChequeReport;
                btnSave.Text = "Update";
                decMasterId = decContraMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV30:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill all the controlls while coming from other forms for update or delete
        /// </summary>
        public void FillFunction()
        {
            try
            {
                ContraDetailsSP spContraDetails = new ContraDetailsSP();
                ContraMasterSP spContraMaster = new ContraMasterSP();
                ContraMasterInfo infoContraMaster = new ContraMasterInfo();
                LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                infoContraMaster = spContraMaster.ContraMasterView(decMasterId);
                txtVoucherNo.ReadOnly = false;
                strVoucherNo = infoContraMaster.VoucherNo;
                txtVoucherNo.Text = infoContraMaster.InvoiceNo;
                strInvoiceNo = infoContraMaster.InvoiceNo;
                strInvoiceNo = infoContraMaster.InvoiceNo;
                decContraSuffixPrefixId = Convert.ToDecimal(infoContraMaster.SuffixPrefixId.ToString());
                DecContraVoucherTypeId = Convert.ToDecimal(infoContraMaster.VoucherTypeId.ToString());
                int inDecimalPlace = PublicVariables._inNoOfDecimalPlaces;
                txtNarration.Text = infoContraMaster.Narration;
                txtContraVoucherDate.Text = Convert.ToString(infoContraMaster.date);
                cmbBankAccount.SelectedValue = infoContraMaster.LedgerId;
                strVoucherNo = infoContraMaster.VoucherNo;
                if (infoContraMaster.Type == "Deposit")
                {
                    rbtnDeposit.Checked = true;
                }
                else
                {
                    rbtnWithdrawal.Checked = true;
                }
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(DecContraVoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.Enabled = false;
                }
                else
                {
                    txtVoucherNo.Enabled = true;
                }
                DataTable dtbl = new DataTable();
                dtbl = spContraDetails.ContraDetailsViewWithMasterId(decMasterId);
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    dgvContraVoucher.Rows.Add();
                    dgvContraVoucher.Rows[i].Cells["dgvcmbBankorCashAccount"].Value = Convert.ToDecimal(dtbl.Rows[i]["ledgerId"].ToString());
                    dgvContraVoucher.Rows[i].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(dtbl.Rows[i]["exchangeRateId"].ToString());
                    dgvContraVoucher.Rows[i].Cells["dgvtxtAmount"].Value = Convert.ToDecimal(dtbl.Rows[i]["amount"].ToString());
                    if (dtbl.Rows[i]["chequeNo"].ToString() != string.Empty)
                    {
                        dgvContraVoucher.Rows[i].Cells["dgvtxtChequeNo"].Value = dtbl.Rows[i]["chequeNo"].ToString();
                        if (dtbl.Rows[i]["chequeDate"].ToString() == "01 Jan 1753")
                        {
                            dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value = null;
                        }
                        else
                        {
                            dgvContraVoucher.Rows[i].Cells["dgvtxtChequeDate"].Value = dtbl.Rows[i]["chequeDate"].ToString();
                        }
                    }
                    dgvContraVoucher.Rows[i].Cells["dgvtxtDetailsId"].Value = dtbl.Rows[i]["contraDetailsId"].ToString();
                    decimal decDetailsId1 = Convert.ToDecimal(dtbl.Rows[i]["contraDetailsId"].ToString());
                    decimal decLedgerPostingId = SpLedgerPosting.LedgerPostingIdFromDetailsId(decDetailsId1, strVoucherNo, DecContraVoucherTypeId);
                    dgvContraVoucher.Rows[i].Cells["dgvtxtLedgerPostingId"].Value = decLedgerPostingId.ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV31:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call form Ledger Details For update or delete
        /// </summary>
        /// <param name="frmLedgerDetails"></param>
        /// <param name="decmasterid"></param>
        public void CallFromLedgerDetails(frmLedgerDetails frmLedgerDetails, decimal decmasterid)
        {
            try
            {
                base.Show();
                this.frmLedgerDetailsObj = frmLedgerDetails;
                frmLedgerDetailsObj.Enabled = false;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                isEditMode = true;
                decMasterId = decmasterid;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV32:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Save button click. checking the user privilege here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEditFuction();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV33:" + ex.Message;
            }
        }
        /// <summary>
        /// To set gridview as edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraVoucher_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvContraVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvContraVoucher.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvContraVoucher.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV34:" + ex.Message;
            }
        }
        /// <summary>
        /// Close button click, checking the confirmation
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
                formMDI.infoError.ErrorString = "CV35:" + ex.Message;
            }
        }
        /// <summary>
        /// Add button click,its for add a new bank account ledger from these form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBankAccountAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBankAccount.SelectedValue != null)
                {
                    strBankOrCashAccount = cmbBankAccount.SelectedValue.ToString();
                }
                else
                {
                    strBankOrCashAccount = string.Empty;
                }
                frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();
                frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                if (open == null)
                {
                    frmAccountLedgerObj.WindowState = FormWindowState.Normal;
                    frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                    frmAccountLedgerObj.CallFromContraVoucher(this, false);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallFromContraVoucher(this, false);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV36:" + ex.Message;
            }
        }
        /// <summary>
        /// for change date in Date time picker and text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContraVoucherDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtContraVoucherDate);
                if (txtContraVoucherDate.Text == string.Empty)
                {
                    txtContraVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string strdate = txtContraVoucherDate.Text;
                dtpContraVoucherDate.Value = Convert.ToDateTime(strdate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV37:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the multicurrency and set the read only mode of currency combo as per the basis of settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpContraVoucherDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                GridCurrencyComboFill();
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("MultiCurrency") == "Yes")
                {
                    dgvcmbCurrency.ReadOnly = false;
                }
                else
                {
                    dgvcmbCurrency.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV38:" + ex.Message;
            }
        }
        /// <summary>
        /// to set the date time format as standard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpContraVoucherDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpContraVoucherDate.Value;
                this.txtContraVoucherDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV39:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click and checking the other forms is opend or not and close it if its opend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                if (frmAgeingObj != null)
                {
                    frmAgeingObj.Close();
                    frmAgeingObj = null;
                }
                if (frmChequeReportObj != null)
                {
                    frmChequeReportObj.Close();
                    frmChequeReportObj = null;
                }
                if (frmContraRegisterObj != null)
                {
                    frmContraRegisterObj.Close();
                    frmContraRegisterObj = null;
                }
                if (frmContraReportObj != null)
                {
                    frmContraReportObj.Close();
                    frmContraReportObj = null;
                }
                if (frmCurrencyObj != null)
                {
                    frmCurrencyObj.Close();
                    frmCurrencyObj = null;
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Close();
                    frmDayBookObj = null;
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Close();
                    frmLedgerDetailsObj = null;
                }
                if (frmLedgerPopupObj != null)
                {
                    frmLedgerPopupObj.Close();
                    frmLedgerPopupObj = null;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV40:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the serial no generation function In the datagridview Rows added event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraVoucher_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SlNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV41:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the totalamount,SlNo,InvalidEntry functions to do the all are the process even if change one value of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraVoucher_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                AccountGroupSP spAccountGroup = new AccountGroupSP();
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    if (dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbBankorCashAccount"].Value != null && dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbBankorCashAccount"].Value.ToString() != "")
                    {
                        if (dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value == null || dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value.ToString() == "")
                        {
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1);//PublicVariables._decCurrencyId;
                        }
                    }
                    TotalAmount();
                    SlNo();
                    if (dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbBankorCashAccount"].Value != null)
                    {
                        decimal decLedger = Convert.ToDecimal(dgvContraVoucher.Rows[e.RowIndex].Cells["dgvcmbBankorCashAccount"].Value.ToString());
                        isBankAcocunt = spAccountGroup.AccountGroupwithLedgerId(decLedger);
                        if (isBankAcocunt)
                        {
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].ReadOnly = false;
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeNo"].ReadOnly = false;
                        }
                        else
                        {
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].ReadOnly = true;
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeNo"].ReadOnly = true;
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = string.Empty;
                            dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeNo"].Value = string.Empty;
                        }
                    }
                    CheckColumnMissing(e);
                    if (!dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].ReadOnly)
                    {
                        DateValidation objVal = new DateValidation();
                        TextBox txtDate = new TextBox();
                        if (dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value != null)
                        {
                            txtDate.Text = dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value.ToString();
                            bool isDate = objVal.DateValidationFunction(txtDate);
                            if (isDate)
                            {
                                dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = txtDate.Text;
                            }
                            else
                            {
                                dgvContraVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV42:" + ex.Message;
            }
        }
        /// <summary>
        /// On Remove Link button click.store the detailsId in List Array to delete the curresponding items from table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dgvContraVoucher.Rows.Count == 1)
                {
                    MessageBox.Show("No row to remove!", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int inRowCount = dgvContraVoucher.RowCount;
                    if (inRowCount > 1 && !dgvContraVoucher.CurrentRow.IsNewRow)
                    {
                        if (dgvContraVoucher.CurrentRow.Cells["dgvtxtDetailsId"].Value != null && dgvContraVoucher.CurrentRow.Cells["dgvtxtDetailsId"].Value.ToString() != "")
                        {
                            arrlstOfRemove.Add(dgvContraVoucher.CurrentRow.Cells["dgvtxtDetailsId"].Value.ToString());
                            inArrOfRemove++;
                        }
                        dgvContraVoucher.Rows.RemoveAt(dgvContraVoucher.CurrentRow.Index);
                        SlNo();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV43:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click , checking the conformation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (isEditMode)
                    {
                        if (PublicVariables.isMessageDelete)
                        {
                            if (Messages.DeleteMessage())
                            {
                                DeleteFunction(decMasterId);
                            }
                        }
                        else
                        {
                            DeleteFunction(decMasterId);
                        }
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV44:" + ex.Message;
            }
        }
        /// <summary>
        /// When Form load call the clear function and reset all are the controlls  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraVoucher_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV45:" + ex.Message;
            }
        }
        /// <summary>
        /// calling the keypress event for Validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraVoucher_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridViewTextBoxEditingControl dgvtxtAmount = e.Control as DataGridViewTextBoxEditingControl;
                if (dgvtxtAmount != null)
                {
                    dgvtxtAmount.KeyPress += dgvtxtAmount_KeyPress;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV46:" + ex.Message;
            }
        }
        /// <summary>
        /// Handling Exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraVoucher_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
                {
                    object value = dgvContraVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (!((DataGridViewComboBoxColumn)dgvContraVoucher.Columns[e.ColumnIndex]).Items.Contains(value))
                    {
                        e.ThrowException = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV47:" + ex.Message;
            }
        }
        /// <summary>
        /// Form Closing event, checking other forms are opend or not, if its opend Activate the forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraVoucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmContraRegisterObj != null)
                {
                    frmContraRegisterObj.Enabled = true;
                    frmContraRegisterObj.Gridfill();
                }
                if (frmContraReportObj != null)
                {
                    frmContraReportObj.Enabled = true;
                    frmContraReportObj.GridFill();
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();
                    frmDayBookObj = null;
                }
                if (frmChequeReportObj != null)
                {
                    frmChequeReportObj.Enabled = true;
                    frmChequeReportObj.ChequeReportFillGrid();
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.LedgerDetailsView();
                }
                if (frmsearch != null)
                {
                    frmsearch.Enabled = true;
                    frmsearch.GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV48:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// From keydown for Showing curresponding popup and create Quick access in Keybord entrys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmContraVoucher_KeyDown(object sender, KeyEventArgs e)
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
                if (dgvContraVoucher.CurrentRow != null)
                {
                    if (dgvContraVoucher.CurrentCell.ColumnIndex == dgvContraVoucher.Columns["dgvcmbBankorCashAccount"].Index)
                    {
                        if (dgvContraVoucher.CurrentCell == dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"])
                        {
                            //--------------------For ledger Popup------------------------------------//
                            if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)//Ledger popup
                            {
                                frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                                frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                                btnSave.Focus();
                                dgvContraVoucher.Focus();
                                if (dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value != null && dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value.ToString() != "")
                                {
                                    decimal decLedgerIdForPopUp = Convert.ToDecimal(dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value.ToString());
                                    btnSave.Focus();
                                    dgvContraVoucher.Focus();
                                    frmLedgerPopupObj.CallFromContraVoucher(this, decLedgerIdForPopUp, "");
                                }
                            }
                        }
                    }
                    if (dgvContraVoucher.CurrentCell == dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"])
                    {
                        if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)//Ledger creation
                        {
                            SendKeys.Send("{F10}");
                            if (dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value != null)
                            {
                                strBankOrCashAccount = dgvContraVoucher.CurrentRow.Cells["dgvcmbBankorCashAccount"].Value.ToString();
                            }
                            else
                            {
                                strBankOrCashAccount = string.Empty;
                            }
                            frmAccountLedgerObj = new frmAccountLedger();
                            frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                            frmAccountLedgerObj.CallFromContraVoucher(this, true);
                        }
                    }
                    if (dgvContraVoucher.CurrentCell == dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"])
                    {
                        if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                        {
                            frmCurrencyObj = new frmCurrencyDetails();
                            frmCurrencyObj.MdiParent = formMDI.MDIObj;
                            btnSave.Focus();
                            dgvContraVoucher.Focus();
                            if (dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value != null && dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString() != "")
                            {
                                btnSave.Focus();
                                dgvContraVoucher.Focus();
                                frmCurrencyObj.CallFromContraVoucher(this, Convert.ToDecimal(dgvContraVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString()));
                            }
                        }
                    }
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    btnSave.Focus();
                    if (dgvContraVoucher.Columns["dgvcmbBankorCashAccount"].Selected)
                    {
                        btnSave.Select();
                    }
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control) //Delete
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV49:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtContraVoucherDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV50:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContraVoucherDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBankAccount.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (!isAutomatic)
                    {
                        txtVoucherNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV51:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation and popup open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvContraVoucher.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtContraVoucherDate.Focus();
                    txtContraVoucherDate.SelectionStart = 0;
                    txtContraVoucherDate.SelectionLength = 0;
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
                {
                    if (cmbBankAccount.Focused)
                    {
                        cmbBankAccount.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                    else
                    {
                        cmbBankAccount.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    if (cmbBankAccount.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromContraVoucher(this, Convert.ToDecimal(cmbBankAccount.SelectedValue.ToString()), "CashOrBank");
                    }
                    else
                    {
                        Messages.InformationMessage("Select any cash or bank account ");
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnBankAccountAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV52:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvContraVoucher.RowCount > 0)
                    {
                        dgvContraVoucher.Focus();
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbBankAccount.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV53:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    inNarrationCount++;
                    if (inNarrationCount == 2)
                    {
                        inNarrationCount = 0;
                        if (cbxPrintafterSave.Enabled)
                        {
                            cbxPrintafterSave.Focus();
                        }
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text.Trim() == "" || txtNarration.SelectionStart == 0)
                    {
                        if (dgvContraVoucher.RowCount > 0)
                        {
                            dgvContraVoucher.Focus();
                            dgvContraVoucher.Rows[dgvContraVoucher.RowCount - 1].Cells["dgvtxtChequeDate"].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV54:" + ex.Message;
            }
        }
        /// <summary>
        /// Get narration textbox row count for For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text.Trim() == "")
                {
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                    txtNarration.Focus();
                }
                else
                {
                    txtNarration.SelectionStart = txtNarration.Text.Trim().Length;
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV55:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvContraVoucher_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvContraVoucher.CurrentCell == dgvContraVoucher.Rows[dgvContraVoucher.Rows.Count - 1].Cells["dgvtxtChequeDate"])
                    {
                        txtNarration.Focus();
                        dgvContraVoucher.ClearSelection();
                        e.Handled = true;
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvContraVoucher.CurrentCell == dgvContraVoucher.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        cmbBankAccount.Focus();
                        dgvContraVoucher.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV56:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnWithdrawal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (isAutomatic)
                {
                    txtContraVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV57:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDeposit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                rbtnWithdrawal.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV58:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPrintafterSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV59:" + ex.Message;
            }
        }
        /// <summary>
        /// For EnterKey and backSpace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cbxPrintafterSave.Enabled)
                    {
                        cbxPrintafterSave.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CV60:" + ex.Message;
            }
        }
       
        private void dgvContraVoucher_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
             try
            {
              CheckColumnMissing(e);
            }
             catch (Exception ex)
             {
                 formMDI.infoError.ErrorString = "CV61:" + ex.Message;
             }
        }
        #endregion
    }
}
