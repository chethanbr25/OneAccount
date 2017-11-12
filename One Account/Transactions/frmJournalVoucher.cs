﻿//This is a source code or part of Oneaccount project
//Copyright (C) 2013  C-Macys Technosolutions Pvt.Ltd

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
using System.IO;
using System.Collections;

namespace One_Account
{

    public partial class frmJournalVoucher : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decJournalVoucherTypeId = 0;
        string strVoucherNo = string.Empty;
        string tableName = "JournalMaster";
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        string strInvoiceNo = string.Empty;
        frmCurrencyDetails frmCurrencyObj;//to use in call from currency class
        int inNarrationCount = 0;
        decimal decJournalSuffixPrefixId = 0;
        bool isAutomatic = false;
        bool isEditMode = false;
        ArrayList arrlstOfRemovedLedgerPostingId = new ArrayList();
        ArrayList arrlstOfRemove = new ArrayList();
        int inArrOfRemove = 0;
        decimal decJournalMasterIdForEdit = 0;
        frmJournalRegister journalRegisterObj = null;//To use in call from JournalRegister class
        frmJournalReport frmJournalReportObj = null;//To use in call from JournalReport class
        frmPartyBalance frmPartyBalanceObj = new frmPartyBalance();//To use in call from PartyBalance class
        frmVoucherSearch objVoucherSearch = null;
        DataTable dtblPartyBalance = new DataTable();//To store PartyBalance entries while clicking btn_Save in JournalVoucher
        ArrayList arrlstOfDeletedPartyBalanceRow;
        DataTable dtblTemporaryPartyBalance = new DataTable();//to store PartyBalance entries in update mode while closing the form before updating
        SettingsSP spSettings = new SettingsSP();//to select data from settings table

        decimal decSelectedCurrencyRate = 0;
        decimal decConvertRate = 0;
        decimal decAmount = 0;

        decimal decLedgerIdForPopUp = 0;
        bool isValueChanged = false;
        frmBillallocation frmBillallocationObj = null;
        frmDayBook frmDayBookObj = null; //to use in call from frmDayBook
        frmAgeingReport frmAgeingObj = null; //to use in call from frmAgeing
        frmLedgerDetails frmLedgerDetailsObj = null;//to use in call from frmLedgerDetails
        public string strVocherNo;

        int inUpdatingRowIndexForPartyRemove = -1;
        decimal decUpdatingLedgerForPartyremove = 0;

        #endregion

        #region Functions
        /// <summary>
        /// Create instance of frmJournalVoucher
        /// </summary>
        public frmJournalVoucher()
        {
            InitializeComponent();
        }

        /// <summary>
        /// It is a function for vouchertypeselection form to select perticular voucher and open the form under the vouchertype
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strVoucherTypeName"></param>
        public void CallFromVoucherTypeSelection(decimal decVoucherTypeId, string strVoucherTypeName)
        {
            try
            {
                decJournalVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decJournalVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();

                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decJournalVoucherTypeId, dtpVoucherDate.Value);
                decJournalSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                strPrefix = infoSuffixPrefix.Prefix;
                strSuffix = infoSuffixPrefix.Suffix;
                this.Text = strVoucherTypeName;
                base.Show();
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV1:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void clear()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                JournalMasterSP spMaster = new JournalMasterSP();


                //-----------------------------------VoucherNo automatic generation-------------------------------------------//

                if (strVoucherNo == string.Empty)
                {

                    strVoucherNo = "0"; //strMax;
                }
                strVoucherNo = obj.VoucherNumberAutomaicGeneration(decJournalVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);

                if (Convert.ToDecimal(strVoucherNo) != spMaster.JournalMasterGetMaxPlusOne(decJournalVoucherTypeId))
                {
                    strVoucherNo = spMaster.JournalMasterGetMax(decJournalVoucherTypeId).ToString();
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decJournalVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                    if (spMaster.JournalMasterGetMax(decJournalVoucherTypeId).ToString() == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decJournalVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                    }
                }

                //===================================================================================================================//
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();

                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decJournalVoucherTypeId, dtpVoucherDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.Text = strInvoiceNo;
                    txtVoucherNo.ReadOnly = true;
                }
                else
                {
                    txtVoucherNo.ReadOnly = false;
                    txtVoucherNo.Text = string.Empty;
                    strInvoiceNo = txtVoucherNo.Text.Trim();
                }


                dgvJournalVoucher.Rows.Clear();
                VoucherDate();
                dtpVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtDebitTotal.Text = string.Empty;
                txtCreditTotal.Text = string.Empty;
                txtNarration.Text = string.Empty;
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                isEditMode = false;
                dtblPartyBalance.Clear();//to clear party balance entries to clear the dgvpatybalance
                PrintCheck();
                if (!txtVoucherNo.ReadOnly)
                {
                    txtVoucherNo.Focus();
                }
                else
                {
                    txtDate.Select();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV2:" + ex.Message;
            }

        }

        /// <summary>
        /// To change the currency from currency popup
        /// </summary>
        /// <param name="frmCurrencyDetails"></param>
        /// <param name="decId"></param>
        public void CallFromCurrenCyDetails(frmCurrencyDetails frmCurrencyDetails, decimal decId) //PopUp
        {
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            try
            {
                frmCurrencyObj.Close();
                CurrencyComboFill();
                decId = spExchangeRate.GetExchangeRateId(decId, Convert.ToDateTime(txtDate.Text));
                dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value = decId;
                dgvJournalVoucher.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV3:" + ex.Message;
            }
        }

        /// <summary>
        /// To select the ledger from ledger popup
        /// </summary>
        /// <param name="decId"></param>
        /// <param name="frmLedgerPopUpObj"></param>
        public void CallFromLedgerPopup(decimal decId, frmLedgerPopup frmLedgerPopUpObj) //PopUp
        {
            try
            {
                frmLedgerPopUpObj.Close();
                dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value = decId;
                dgvJournalVoucher.Focus();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV4:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from the vouchersearch
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallThisFormFromVoucherSearch(frmVoucherSearch frm, decimal decId)
        {
            // Function to call form voucher Search
            try
            {
                this.objVoucherSearch = frm;
                decJournalMasterIdForEdit = decId;
                isEditMode = true;
                btnDelete.Enabled = true;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV5:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the currency combo box
        /// </summary>
        public void CurrencyComboFill()
        {
            try
            {

                DataTable dtbl = new DataTable();
                TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
                dtbl = TransactionGeneralFillObj.CurrencyComboByDate(Convert.ToDateTime(txtDate.Text));
                DataRow dr = dtbl.NewRow();
                dr[0] = string.Empty;
                dr[1] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                dgvcmbCurrency.DataSource = dtbl;
                dgvcmbCurrency.DisplayMember = "currencyName";
                dgvcmbCurrency.ValueMember = "exchangeRateId";


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
                formMDI.infoError.ErrorString = "JV6:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the account ledger
        /// </summary>
        public void AccountLedgerComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                AccountLedgerSP spaccountledger = new AccountLedgerSP();
                dtbl = spaccountledger.AccountLedgerViewAll();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[2] = string.Empty;
                dtbl.Rows.InsertAt(dr, 0);
                dgvcmbAccountLedger.DataSource = dtbl;
                dgvcmbAccountLedger.ValueMember = "ledgerId";
                dgvcmbAccountLedger.DisplayMember = "ledgerName";

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV7:" + ex.Message;
            }
        }

        /// <summary>
        /// function to fill the debit/credit combobox
        /// </summary>
        public void DrOrCrComboFill()
        {
            try
            {
                dgvcmbDrOrCr.Items.AddRange("Dr", "Cr");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV8:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to keep the datatable containing party balance details while calling from the partybalance form
        /// </summary>
        /// <param name="frmPartyBalance"></param>
        /// <param name="decAmount"></param>
        /// <param name="dtbl"></param>
        /// <param name="arrlstOfRemovedRow"></param>
        public void CallFromPartyBalance(frmPartyBalance frmPartyBalance, decimal decAmount, DataTable dtbl, ArrayList arrlstOfRemovedRow)
        {
            try
            {
                dgvJournalVoucher.CurrentRow.Cells["dgvtxtAmount"].Value = decAmount.ToString();
                dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1);
                dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].ReadOnly = true;
                this.frmPartyBalanceObj = frmPartyBalance;
                frmPartyBalance.Close();
                frmPartyBalanceObj = null;
                dtblPartyBalance = dtbl;
                arrlstOfDeletedPartyBalanceRow = arrlstOfRemovedRow;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV9:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the voucher to edit or delete while calling from the Journal register
        /// </summary>
        /// <param name="frmJournalObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromJournalRegister(frmJournalRegister frmJournalObj, decimal decMasterId)
        {
            try
            {
                base.Show();
                journalRegisterObj = frmJournalObj;
                journalRegisterObj.Enabled = false;
                isEditMode = true;
                btnDelete.Enabled = true;
                decJournalMasterIdForEdit = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV10:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to generate serial number
        /// </summary>
        public void SlNo()
        {
            int inRowNo = 1;
            try
            {
                foreach (DataGridViewRow dr in dgvJournalVoucher.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowNo;
                    inRowNo++;
                    if (dr.Index == dgvJournalVoucher.Rows.Count - 1)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV11:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to calculate total debit and credit
        /// </summary>
        public void DebitAndCreditTotal()
        {
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            int inRowCount = dgvJournalVoucher.RowCount;
            decimal decTxtTotalDebit = 0;
            decimal decTxtTotalCredit = 0;
            try
            {
                for (int inI = 0; inI < inRowCount; inI++)
                {
                    if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                    {
                        if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString() != string.Empty)
                            {
                                if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString() != ".")
                                {
                                    if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                                    {

                                        //--------Currency conversion--------------//
                                        decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));
                                        decAmount = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                        decConvertRate = decAmount * decSelectedCurrencyRate;
                                        //===========================================//
                                        decTxtTotalDebit = decTxtTotalDebit + decConvertRate;


                                    }
                                    else
                                    {
                                        //--------Currency conversion--------------//
                                        decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));
                                        decAmount = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                        decConvertRate = decAmount * decSelectedCurrencyRate;
                                        //===========================================//
                                        decTxtTotalCredit = decTxtTotalCredit + decConvertRate;
                                    }
                                }
                            }
                        }
                    }

                    txtDebitTotal.Text = Math.Round(decTxtTotalDebit, PublicVariables._inNoOfDecimalPlaces).ToString();
                    txtCreditTotal.Text = Math.Round(decTxtTotalCredit, Convert.ToInt16(PublicVariables._inNoOfDecimalPlaces)).ToString();

                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV12:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to call the save after checking invalid entries
        /// </summary>
        public void SaveFunction()
        {
            ArrayList arrlstOfRowToRemove = new ArrayList();
            int inReadyForSave = 0;
            int inIsRowToRemove = 0;
            int inIfGridColumnMissing = 0;

            try
            {
                if (txtVoucherNo.Text.Trim() != string.Empty)
                {
                    if (!isEditMode)
                    {
                        decimal decTotalDebit = 0;
                        decimal decTotalCredit = 0;

                        int inRowCount = dgvJournalVoucher.RowCount;
                        for (int inI = 0; inI < inRowCount - 1; inI++)
                        {
                            if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }
                            else if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }

                            else if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }
                            else if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].FormattedValue.ToString() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }

                        }

                        //----------------------------------------------------------------------//
                        if (inIfGridColumnMissing == 0)
                        {
                            inReadyForSave = 1;
                            inIsRowToRemove = 0;

                        }
                        else
                        {
                            string strMsg = string.Empty;
                            int inK = 0;
                            foreach (object obj in arrlstOfRowToRemove)
                            {
                                if (inK != 0)
                                {
                                    strMsg = strMsg + ", ";
                                }
                                string str = Convert.ToString(obj);
                                strMsg = strMsg + str;
                                inK++;
                            }
                            if (MessageBox.Show("Row " + strMsg + " Contains invalid entries.\n Do you want to continue ? ", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                inReadyForSave = 1;
                                inIsRowToRemove = 1;
                            }
                            else
                                return;
                        }
                        //=====================================================================//

                        //-------------------------------------------------------------------//
                        if (inReadyForSave == 1)
                        {

                            int inTableRowCount = dtblPartyBalance.Rows.Count;
                            //-----------------If there are rows to remove---------------//
                            if (inIsRowToRemove == 1)
                            {
                                int inDgvJournalRowCount = dgvJournalVoucher.RowCount;
                                int inK = 0;
                                for (int inI = 0; inI < inDgvJournalRowCount; inI++)
                                {
                                    if (inK == arrlstOfRowToRemove.Count)
                                    {
                                        break;
                                    }
                                    if (inDgvJournalRowCount > 0)
                                    {

                                        if (Convert.ToInt32(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString()) == Convert.ToInt32(arrlstOfRowToRemove[inK]))
                                        {
                                            inK++;
                                            if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                                            {
                                                arrlstOfRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString());
                                                arrlstOfRemovedLedgerPostingId.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                            }

                                            inTableRowCount = dtblPartyBalance.Rows.Count;
                                            for (int inJ = 0; inJ < inTableRowCount; inJ++)
                                            {
                                                if (dtblPartyBalance.Rows.Count == inJ)
                                                {
                                                    break;
                                                }
                                                if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                                {
                                                    if (Convert.ToInt32(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString()) == Convert.ToInt32(dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString()))
                                                    {
                                                        if (dtblPartyBalance.Rows[inJ]["PartyBalanceId"].ToString() != "0")
                                                        {
                                                            arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inI]["PartyBalanceId"]);
                                                        }
                                                        dtblPartyBalance.Rows.RemoveAt(inJ);
                                                        inJ--;
                                                    }
                                                }
                                            }
                                            if (inUpdatingRowIndexForPartyRemove == inI)
                                            {
                                                inUpdatingRowIndexForPartyRemove = -1;
                                                decUpdatingLedgerForPartyremove = 0;
                                            }
                                            dgvJournalVoucher.Rows.Remove(dgvJournalVoucher.Rows[inI]);
                                            inDgvJournalRowCount = dgvJournalVoucher.RowCount;
                                            inI--;

                                        }
                                    }
                                }
                                SlNo();

                            }

                            //============================================================//
                            int RowCount = dgvJournalVoucher.RowCount;
                            if (RowCount > 1)
                            {
                                decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                                decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());
                                if (decTotalCredit != 0 || decTotalDebit != 0)
                                {
                                    if (decTotalDebit == decTotalCredit)
                                    {
                                        if (PublicVariables.isMessageAdd)
                                        {
                                            if (Messages.SaveMessage())
                                            {

                                                Save();
                                                clear();
                                            }
                                            else
                                            {
                                                dgvJournalVoucher.Focus();
                                            }
                                        }
                                        else
                                        {

                                            Save();
                                            clear();

                                        }
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Total debit and total credit should be equal");
                                        dgvJournalVoucher.Focus();
                                    }
                                }
                                else
                                {
                                    Messages.InformationMessage("Cannot save total debit and credit as 0");

                                }
                            }
                            else
                            {
                                Messages.InformationMessage("There is no row to save");
                            }

                        }

                    }

                }
                else
                {
                    Messages.InformationMessage("Enter voucherno.");
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV13:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to Save the voucher
        /// </summary>
        public void Save()
        {
            try
            {
                decimal decTotalDebit = 0;
                decimal decTotalCredit = 0;

                decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());

                JournalMasterSP spJournalMaster = new JournalMasterSP();
                JournalDetailsSP spJournalDetails = new JournalDetailsSP();
                JournalMasterInfo infoJournalMaster = new JournalMasterInfo();
                JournalDetailsInfo infoJournalDetails = new JournalDetailsInfo();
                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();

                infoJournalMaster.VoucherNo = strVoucherNo;
                infoJournalMaster.InvoiceNo = txtVoucherNo.Text;
                infoJournalMaster.SuffixPrefixId = decJournalSuffixPrefixId;
                infoJournalMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoJournalMaster.Narration = txtNarration.Text.Trim();
                infoJournalMaster.UserId = PublicVariables._decCurrentUserId;
                infoJournalMaster.VoucherTypeId = decJournalVoucherTypeId;
                infoJournalMaster.FinancialYearId = Convert.ToDecimal(PublicVariables._decCurrentFinancialYearId.ToString());

                infoJournalMaster.Extra1 = string.Empty;
                infoJournalMaster.Extra2 = string.Empty;
                infoJournalMaster.ExtraDate = DateTime.Now;


                infoJournalMaster.TotalAmount = decTotalDebit;
                decimal decJournalMasterId = spJournalMaster.JournalMasterAdd(infoJournalMaster);

                /*******************JournalDetailsAdd and LedgerPosting*************************/
                infoJournalDetails.JournalMasterId = decJournalMasterId;
                infoJournalDetails.ExtraDate = DateTime.Now;
                infoJournalDetails.Extra1 = string.Empty;
                infoJournalDetails.Extra2 = string.Empty;


                decimal decLedgerId = 0;
                decimal decDebit = 0;
                decimal decCredit = 0;
                int inRowCount = dgvJournalVoucher.RowCount;
                for (int inI = 0; inI < inRowCount - 1; inI++)
                {
                    if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        infoJournalDetails.LedgerId = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                        decLedgerId = infoJournalDetails.LedgerId;
                    }
                    if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                    {
                        if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            //--------Currency conversion--------------//
                            decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));
                            decAmount = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                            decConvertRate = decAmount * decSelectedCurrencyRate;
                            //===========================================//
                            if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                            {
                                infoJournalDetails.Debit = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoJournalDetails.Credit = 0;
                                decDebit = decConvertRate;
                                decCredit = infoJournalDetails.Credit;
                            }
                            else
                            {
                                infoJournalDetails.Credit = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoJournalDetails.Debit = 0;
                                decDebit = infoJournalDetails.Debit;
                                decCredit = decConvertRate;
                            }
                        }
                        infoJournalDetails.ExchangeRateId = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString());
                        if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                        {
                            infoJournalDetails.ChequeNo = dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                        }
                        else
                        {
                            infoJournalDetails.ChequeNo = string.Empty;
                        }
                        if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoJournalDetails.ChequeDate = Convert.ToDateTime(dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                        {
                            infoJournalDetails.ChequeDate = DateTime.Now;
                        }
                        decimal decJournalDetailsId = spJournalDetails.JournalDetailsAdd(infoJournalDetails);
                        if (decJournalDetailsId != 0)
                        {
                            PartyBalanceAddOrEdit(inI);
                            LedgerPosting(decLedgerId, decCredit, decDebit, decJournalDetailsId, inI);
                        }
                    }

                }

                Messages.SavedMessage();

                //----------------If print after save is enable-----------------------//
                if (cbxPrintAfterSave.Checked)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(decJournalMasterId);
                    }
                    else
                    {
                        Print(decJournalMasterId);
                    }
                }

                //===================================================================//
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV14:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call the Edit after checking invalid entries
        /// </summary>
        /// <param name="decJournalMasterId"></param>
        public void EditFunction(decimal decJournalMasterId)
        {
            try
            {
                ArrayList arrlstOfRowToRemove = new ArrayList();
                int inReadyForSave = 0;
                int inIsRowToRemove = 0;
                int inIfGridColumnMissing = 0;

                decimal decTotalDebit = 0;
                decimal decTotalCredit = 0;

                int inRowCount = dgvJournalVoucher.RowCount;
                for (int inI = 0; inI < inRowCount - 1; inI++)
                {
                    if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }
                    else if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }

                    else if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }
                    else if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value == null || dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }

                }
                //----------------------------------------------------------------------//
                if (inIfGridColumnMissing == 0)
                {
                    inReadyForSave = 1;
                    inIsRowToRemove = 0;

                }
                else
                {
                    string strMsg = string.Empty;
                    int inK = 0;

                    foreach (object obj in arrlstOfRowToRemove)
                    {
                        if (inK != 0)
                        {
                            strMsg = strMsg + ", ";
                        }
                        string str = Convert.ToString(obj);
                        strMsg = strMsg + str;
                        inK++;
                    }
                    if (MessageBox.Show("Row " + strMsg + " Contains invalid entries.\n Do you want to continue ? ", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        inReadyForSave = 1;
                        inIsRowToRemove = 1;
                    }
                    else
                        inReadyForSave = 0;
                }
                //=====================================================================//

                if (inReadyForSave == 1)
                {
                    int inTableRowCount = dtblPartyBalance.Rows.Count;
                    //-----------------If there are rows to remove---------------//
                    if (inIsRowToRemove == 1)
                    {
                        int inDgvJournalRowCount = dgvJournalVoucher.RowCount;
                        int inK = 0;
                        for (int inI = 0; inI < inDgvJournalRowCount; inI++)
                        {
                            if (inK == arrlstOfRowToRemove.Count)
                            {
                                break;
                            }
                            if (inDgvJournalRowCount > 0)
                            {

                                if (Convert.ToInt32(dgvJournalVoucher.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString()) == Convert.ToInt32(arrlstOfRowToRemove[inK]))
                                {
                                    inK++;
                                    if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                                    {
                                        arrlstOfRemove.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString());
                                        arrlstOfRemovedLedgerPostingId.Add(dgvJournalVoucher.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                    }

                                    inTableRowCount = dtblPartyBalance.Rows.Count;
                                    for (int inJ = 0; inJ < inTableRowCount; inJ++)
                                    {
                                        if (dtblPartyBalance.Rows.Count == inJ)
                                        {
                                            break;
                                        }
                                        if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                        {
                                            if (dtblPartyBalance.Rows[inJ]["LedgerId"].ToString() == dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString())
                                            {
                                                if (dtblPartyBalance.Rows[inI]["PartyBalanceId"].ToString() != "0")
                                                {
                                                    arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inI]["PartyBalanceId"]);
                                                }
                                                dtblPartyBalance.Rows.RemoveAt(inJ);
                                                inJ--;
                                            }
                                        }
                                    }
                                    if (inUpdatingRowIndexForPartyRemove == inI)
                                    {
                                        inUpdatingRowIndexForPartyRemove = -1;
                                        decUpdatingLedgerForPartyremove = 0;
                                    }
                                    dgvJournalVoucher.Rows.RemoveAt(dgvJournalVoucher.Rows[inI].Index);
                                    inDgvJournalRowCount = dgvJournalVoucher.RowCount;
                                    inI--;
                                }
                            }
                        }
                        SlNo();
                    }
                    //============================================================//
                    inRowCount = dgvJournalVoucher.RowCount;
                    if (inRowCount > 1)
                    {
                        decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                        decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());
                        if (decTotalDebit != 0 && decTotalCredit != 0)
                        {
                            if (decTotalDebit == decTotalCredit)
                            {
                                if (PublicVariables.isMessageEdit)
                                {
                                    if (Messages.UpdateMessage())
                                    {
                                        DeletePartyBalanceOfRemovedRow();
                                        Edit(decJournalMasterId);
                                    }
                                    else
                                    {
                                        dgvJournalVoucher.Focus();
                                    }
                                }
                                else
                                {
                                    DeletePartyBalanceOfRemovedRow();
                                    Edit(decJournalMasterId);
                                }
                            }
                            else
                            {
                                Messages.InformationMessage("Total debit and total credit should be equal");
                                dgvJournalVoucher.Focus();
                                return;
                            }
                        }
                        else
                        {
                            Messages.InformationMessage("Cannot save total debit and credit as 0");
                            dgvJournalVoucher.Focus();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("There is no row to save");
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV15:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to edit the debitnote voucher
        /// </summary>
        /// <param name="decJournalMasterId"></param>
        public void Edit(decimal decJournalMasterId)
        {
            try
            {
                JournalMasterSP spJournalMaster = new JournalMasterSP();
                JournalMasterInfo infoJournalMaster = new JournalMasterInfo();
                JournalDetailsSP spJournalDetails = new JournalDetailsSP();
                JournalDetailsInfo infoJournalDetails = new JournalDetailsInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();

                /*****************Update in JournalMaster table *************/

                decimal decTotalDebit = 0;
                decimal decTotalCredit = 0;

                infoJournalMaster.JournalMasterId = decJournalMasterId;
                infoJournalMaster.VoucherNo = strVoucherNo;
                infoJournalMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                infoJournalMaster.SuffixPrefixId = decJournalSuffixPrefixId;
                infoJournalMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoJournalMaster.Narration = txtNarration.Text.Trim();
                infoJournalMaster.UserId = PublicVariables._decCurrentUserId;
                infoJournalMaster.VoucherTypeId = decJournalVoucherTypeId;
                infoJournalMaster.FinancialYearId = Convert.ToDecimal(PublicVariables._decCurrentFinancialYearId.ToString());
                infoJournalMaster.ExtraDate = DateTime.Now;
                infoJournalMaster.Extra1 = string.Empty;
                infoJournalMaster.Extra2 = string.Empty;


                decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());

                infoJournalMaster.TotalAmount = decTotalDebit;
                decimal decEffectRow = spJournalMaster.JournalMasterEdit(infoJournalMaster);

                /**********************JournalDetails Edit********************/
                if (decEffectRow > 0)
                {
                    infoJournalDetails.JournalMasterId = decJournalMasterId;
                    infoJournalDetails.ExtraDate = DateTime.Now;
                    infoJournalDetails.Extra1 = string.Empty;
                    infoJournalDetails.Extra2 = string.Empty;

                    //-----------to delete details, LedgerPosting and bankReconciliation of removed rows--------------// 
                    LedgerPostingSP spLedgerPosting = new LedgerPostingSP();

                    foreach (object obj in arrlstOfRemove)
                    {
                        string str = Convert.ToString(obj);
                        spJournalDetails.JournalDetailsDelete(Convert.ToDecimal(str));
                        spLedgerPosting.LedgerPostDeleteByDetailsId(Convert.ToDecimal(str), strVoucherNo, decJournalVoucherTypeId);
                    }
                    spLedgerPosting.LedgerPostingDeleteByVoucherNoVoucherTypeIdAndLedgerId(strVoucherNo, decJournalVoucherTypeId, 12);
                    //=============================================================================================//

                    decimal decLedgerId = 0;
                    decimal decDebit = 0;
                    decimal decCredit = 0;
                    decimal decJournalDetailsId = 0;
                    int inRowCount = dgvJournalVoucher.RowCount;
                    for (int inI = 0; inI < inRowCount; inI++)
                    {
                        if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            infoJournalDetails.LedgerId = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                            decLedgerId = infoJournalDetails.LedgerId;
                        }
                        if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                        {
                            //------------------Currency conversion------------------//
                            decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value));
                            decAmount = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                            decConvertRate = decAmount * decSelectedCurrencyRate;
                            //======================================================//

                            if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                            {
                                infoJournalDetails.Debit = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoJournalDetails.Credit = 0;

                                decDebit = decConvertRate;
                                decCredit = infoJournalDetails.Credit;
                            }
                            else
                            {
                                infoJournalDetails.Credit = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoJournalDetails.Debit = 0;
                                decDebit = infoJournalDetails.Debit;
                                decCredit = decConvertRate;
                            }
                            infoJournalDetails.ExchangeRateId = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString());
                            if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                            {
                                infoJournalDetails.ChequeNo = dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                            }
                            else
                            {
                                infoJournalDetails.ChequeNo = string.Empty;
                            }
                            if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                            {
                                infoJournalDetails.ChequeDate = Convert.ToDateTime(dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString());
                            }
                            else
                            {
                                infoJournalDetails.ChequeDate = DateTime.Now;
                            }
                            if (dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                            {
                                infoJournalDetails.JournalDetailsId = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString());
                                spJournalDetails.JournalDetailsEdit(infoJournalDetails);
                                PartyBalanceAddOrEdit(inI);
                                decJournalDetailsId = infoJournalDetails.JournalDetailsId;
                                decimal decLedgerPostId = Convert.ToDecimal(dgvJournalVoucher.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                LedgerPostingEdit(decLedgerPostId, decLedgerId, decCredit, decDebit, decJournalDetailsId, inI);

                            }
                            else
                            {
                                decJournalDetailsId = spJournalDetails.JournalDetailsAdd(infoJournalDetails);
                                PartyBalanceAddOrEdit(inI);
                                LedgerPosting(decLedgerId, decCredit, decDebit, decJournalDetailsId, inI);
                            }

                        }

                    }
                    DeletePartyBalanceOfRemovedRow();
                    Messages.UpdatedMessage();
                    if (cbxPrintAfterSave.Checked)
                    {
                        if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(infoJournalMaster.JournalMasterId);
                        }
                        else
                        {
                            Print(infoJournalMaster.JournalMasterId);
                        }
                    }
                    if (journalRegisterObj != null)
                    {
                        this.Close();
                        journalRegisterObj.Enabled = true;
                    }
                    else if (frmJournalReportObj != null)
                    {
                        this.Close();
                        frmJournalReportObj.Enabled = true;
                    }
                    else if (frmDayBookObj != null)
                    {
                        this.Close();

                    }
                    else if (frmBillallocationObj != null)
                    {
                        this.Close();

                    }
                }
                //----------------If print after save is enable-----------------------//


                //===================================================================//
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV16:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to determine whether to call SaveFunction or EditFunction function 
        /// </summary>
        public void SaveOrEditFunction()
        {
            JournalMasterSP spJournalMaster = new JournalMasterSP();
            try
            {

                if (!isEditMode)
                {

                    if (txtVoucherNo.Text != string.Empty)
                    {

                        if (!isAutomatic)
                        {
                            strInvoiceNo = txtVoucherNo.Text.Trim();
                            if (!spJournalMaster.JournalVoucherCheckExistance(strInvoiceNo, decJournalVoucherTypeId, 0))
                            {
                                SaveFunction();
                            }
                            else
                            {
                                Messages.InformationMessage("Voucher number already exist");
                            }
                        }
                        else
                        {
                            SaveFunction();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Enter voucherNo");
                        txtVoucherNo.Focus();
                    }
                }
                else
                {
                    if (txtVoucherNo.Text != string.Empty)
                    {
                        if (!isAutomatic)
                        {
                            strInvoiceNo = txtVoucherNo.Text.Trim();
                            if (!spJournalMaster.JournalVoucherCheckExistance(strInvoiceNo, decJournalVoucherTypeId, decJournalMasterIdForEdit))
                            {
                                EditFunction(decJournalMasterIdForEdit);
                            }
                            else
                            {
                                Messages.InformationMessage("Voucher number already exist");
                            }
                        }
                        else
                        {
                            EditFunction(decJournalMasterIdForEdit);
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Enter voucherNo");
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV17:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call the SaveOrEditFunction after checking negative balance 
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    JournalMasterSP spJournalMaster = new JournalMasterSP();
                    //=====================================================
                    SettingsSP spSettings = new SettingsSP();
                    string strStatus = spSettings.SettingsStatusCheck("NegativeCashTransaction");
                    decimal decBalance = 0;
                    decimal decCalcAmount = 0;
                    AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                    bool isNegativeLedger = false;
                    int inRowCount = dgvJournalVoucher.RowCount;
                    for (int i = 0; i < inRowCount - 1; i++)
                    {
                        decimal decledgerId = 0;
                        if (dgvJournalVoucher.Rows[i].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[i].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            decledgerId = Convert.ToDecimal(dgvJournalVoucher.Rows[i].Cells["dgvcmbAccountLedger"].Value.ToString());

                            decBalance = spAccountLedger.CheckLedgerBalance(decledgerId);
                            if (dgvJournalVoucher.Rows[i].Cells["dgvtxtAmount"].Value != null && dgvJournalVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                            {
                                decCalcAmount = decBalance - Convert.ToDecimal(dgvJournalVoucher.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                            }
                            if (decCalcAmount < 0)
                            {
                                isNegativeLedger = true;
                                break;
                            }
                        }
                    }
                    //=========================================
                    if (isNegativeLedger)
                    {

                        if (strStatus == "Warn")
                        {
                            if (MessageBox.Show("Negative balance exists,Do you want to Continue", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                SaveOrEditFunction();
                            }
                        }
                        else if (strStatus == "Block")
                        {
                            MessageBox.Show("Cannot continue ,due to negative balance", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            SaveOrEditFunction();
                        }
                    }
                    else
                    {
                        SaveOrEditFunction();
                    }

                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV18:" + ex.Message;
            }
        }

        /// <summary>
        /// function to delete voucher
        /// </summary>
        /// <param name="decJournalMasterId"></param>
        public void DeleteFunction(decimal decJournalMasterId)
        {
            try
            {
                JournalMasterSP spJournalMaster = new JournalMasterSP();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();

                if (!spPartyBalance.PartyBalanceCheckReference(decJournalVoucherTypeId, strVoucherNo))
                {
                    spJournalMaster.JournalVoucherDelete(decJournalMasterId, decJournalVoucherTypeId, strVoucherNo);

                    Messages.DeletedMessage();
                    if (journalRegisterObj != null)
                    {
                        this.Close();
                        journalRegisterObj.Enabled = true;
                    }
                    else if (frmJournalReportObj != null)
                    {
                        this.Close();
                        frmJournalReportObj.Enabled = true;
                    }
                    else if (frmLedgerDetailsObj != null)
                    {
                        this.Close();
                    }
                    else if (objVoucherSearch != null)
                    {
                        this.Close();
                        objVoucherSearch.GridFill();
                    }
                    else if (frmDayBookObj != null)
                    {
                        this.Close();
                    }
                    else if (frmBillallocationObj != null)
                    {
                        this.Close();
                    }
                    else
                    {
                        clear();
                    }
                }
                else
                {
                    Messages.InformationMessage("Reference exist. Cannot delete");
                    txtDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV19:" + ex.Message;
            }

        }

        /// <summary>
        /// Function for ledger posting
        /// </summary>
        /// <param name="decId"></param>
        /// <param name="decCredit"></param>
        /// <param name="decDebit"></param>
        /// <param name="decDetailsId"></param>
        /// <param name="inA"></param>
        public void LedgerPosting(decimal decId, decimal decCredit, decimal decDebit, decimal decDetailsId, int inA)
        {
            LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
            LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            decimal decOldExchange = 0;
            decimal decNewExchangeRate = 0;
            decimal decNewExchangeRateId = 0;
            decimal decOldExchangeId = 0;
            try
            {

                if (!dgvJournalVoucher.Rows[inA].Cells["dgvtxtAmount"].ReadOnly)
                {

                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.VoucherTypeId = decJournalVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            infoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }


                    infoLedgerPosting.ExtraDate = DateTime.Now;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;

                    infoLedgerPosting.LedgerId = decId;
                    infoLedgerPosting.Credit = decCredit;
                    infoLedgerPosting.Debit = decDebit;

                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                }
                else
                {
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.VoucherTypeId = decJournalVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            infoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }


                    infoLedgerPosting.ExtraDate = DateTime.Now;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    infoLedgerPosting.LedgerId = decId;

                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (infoLedgerPosting.LedgerId == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            decOldExchange = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                            decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                            decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchange);
                            decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            decConvertRate = decConvertRate + (decAmount * decSelectedCurrencyRate);

                        }
                    }

                    if (decCredit == 0)
                    {
                        infoLedgerPosting.Credit = 0;
                        infoLedgerPosting.Debit = decConvertRate;
                    }
                    else
                    {
                        infoLedgerPosting.Debit = 0;
                        infoLedgerPosting.Credit = decConvertRate;
                    }
                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

                    infoLedgerPosting.LedgerId = 12;
                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (Convert.ToDecimal(dgvJournalVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            if (dr["ReferenceType"].ToString() == "Against")
                            {
                                decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                                decNewExchangeRate = SpExchangRate.GetExchangeRateByExchangeRateId(decNewExchangeRateId);
                                decOldExchangeId = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                                decOldExchange = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchangeId);
                                decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                                decimal decForexAmount = (decAmount * decNewExchangeRate) - (decAmount * decOldExchange);
                                if (dr["DebitOrCredit"].ToString() == "Dr")
                                {
                                    if (decForexAmount >= 0)
                                    {

                                        infoLedgerPosting.Debit = decForexAmount;
                                        infoLedgerPosting.Credit = 0;
                                    }
                                    else
                                    {
                                        infoLedgerPosting.Credit = -1 * decForexAmount;
                                        infoLedgerPosting.Debit = 0;
                                    }
                                }
                                else
                                {
                                    if (decForexAmount >= 0)
                                    {

                                        infoLedgerPosting.Credit = decForexAmount;
                                        infoLedgerPosting.Debit = 0;
                                    }
                                    else
                                    {
                                        infoLedgerPosting.Debit = -1 * decForexAmount;
                                        infoLedgerPosting.Credit = 0;
                                    }
                                }
                                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV20:" + ex.Message;
            }


        }

        /// <summary>
        /// Function For ledger posting edit
        /// </summary>
        /// <param name="decLedgerPostingId"></param>
        /// <param name="decLedgerId"></param>
        /// <param name="decCredit"></param>
        /// <param name="decDebit"></param>
        /// <param name="decDetailsId"></param>
        /// <param name="inA"></param>
        public void LedgerPostingEdit(decimal decLedgerPostingId, decimal decLedgerId, decimal decCredit, decimal decDebit, decimal decDetailsId, int inA)
        {
            LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
            LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            decimal decOldExchange = 0;
            decimal decNewExchangeRate = 0;
            decimal decNewExchangeRateId = 0;
            decimal decOldExchangeId = 0;
            try
            {
                if (!dgvJournalVoucher.Rows[inA].Cells["dgvtxtAmount"].ReadOnly)
                {
                    infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.VoucherTypeId = decJournalVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            infoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }


                    infoLedgerPosting.ExtraDate = DateTime.Now;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;

                    infoLedgerPosting.LedgerId = decLedgerId;
                    infoLedgerPosting.Credit = decCredit;
                    infoLedgerPosting.Debit = decDebit;


                    spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
                }
                else
                {
                    infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.VoucherTypeId = decJournalVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvJournalVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            infoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }


                    infoLedgerPosting.ExtraDate = DateTime.Now;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;

                    infoLedgerPosting.LedgerId = decLedgerId;


                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (infoLedgerPosting.LedgerId == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            decOldExchange = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                            decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                            decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchange);
                            decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            decConvertRate = decConvertRate + (decAmount * decSelectedCurrencyRate);

                        }
                    }

                    if (decCredit == 0)
                    {
                        infoLedgerPosting.Credit = 0;
                        infoLedgerPosting.Debit = decConvertRate;
                    }
                    else
                    {
                        infoLedgerPosting.Debit = 0;
                        infoLedgerPosting.Credit = decConvertRate;
                    }

                    spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);

                    infoLedgerPosting.LedgerId = 12;
                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (Convert.ToDecimal(dgvJournalVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            if (dr["ReferenceType"].ToString() == "Against")
                            {
                                decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                                decNewExchangeRate = SpExchangRate.GetExchangeRateByExchangeRateId(decNewExchangeRateId);
                                decOldExchangeId = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                                decOldExchange = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchangeId);
                                decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                                decimal decForexAmount = (decAmount * decNewExchangeRate) - (decAmount * decOldExchange);
                                if (dr["DebitOrCredit"].ToString() == "Dr")
                                {
                                    if (decForexAmount >= 0)
                                    {

                                        infoLedgerPosting.Debit = decForexAmount;
                                        infoLedgerPosting.Credit = 0;
                                    }
                                    else
                                    {
                                        infoLedgerPosting.Credit = -1 * decForexAmount;
                                        infoLedgerPosting.Debit = 0;
                                    }
                                }
                                else
                                {
                                    if (decForexAmount >= 0)
                                    {

                                        infoLedgerPosting.Credit = decForexAmount;
                                        infoLedgerPosting.Debit = 0;
                                    }
                                    else
                                    {
                                        infoLedgerPosting.Debit = -1 * decForexAmount;
                                        infoLedgerPosting.Credit = 0;
                                    }
                                }
                                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV21:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to save partybalance
        /// </summary>
        /// <param name="inRowIndex"></param>
        /// <param name="inJ"></param>
        public void PartyBalanceAdd(int inRowIndex, int inJ)
        {
            int inTableRowCount = dtblPartyBalance.Rows.Count;
            PartyBalanceSP spPartyBalance = new PartyBalanceSP();
            PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
            try
            {
                InfopartyBalance.CreditPeriod = 0;//
                InfopartyBalance.Date = dtpVoucherDate.Value;
                InfopartyBalance.LedgerId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString());
                InfopartyBalance.ReferenceType = dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString();
                if (dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "New" || dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "OnAccount")
                {
                    InfopartyBalance.AgainstInvoiceNo = "0";//dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0"; dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0; Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decJournalVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decJournalVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                if (dgvJournalVoucher.Rows[inRowIndex].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                {
                    InfopartyBalance.Debit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                    InfopartyBalance.Credit = 0;
                }
                else
                {
                    InfopartyBalance.Credit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                    InfopartyBalance.Debit = 0;
                }
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spPartyBalance.PartyBalanceAdd(InfopartyBalance);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV22:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to edit the partybalance
        /// </summary>
        /// <param name="decPartyBalanceId"></param>
        /// <param name="inRowIndex"></param>
        /// <param name="inJ"></param>
        public void PartyBalanceEdit(decimal decPartyBalanceId, int inRowIndex, int inJ)
        {

            PartyBalanceSP spPartyBalance = new PartyBalanceSP();
            PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
            try
            {

                InfopartyBalance.PartyBalanceId = decPartyBalanceId;
                InfopartyBalance.CreditPeriod = 0;//
                InfopartyBalance.Date = dtpVoucherDate.Value;
                InfopartyBalance.LedgerId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString());
                InfopartyBalance.ReferenceType = dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString();
                if (dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "New" || dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "OnAccount")
                {
                    InfopartyBalance.AgainstInvoiceNo = "0"; //dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0";// dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0;// Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decJournalVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decJournalVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                if (dgvJournalVoucher.Rows[inRowIndex].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                {
                    InfopartyBalance.Debit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                    InfopartyBalance.Credit = 0;
                }
                else
                {
                    InfopartyBalance.Credit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                    InfopartyBalance.Debit = 0;
                }
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spPartyBalance.PartyBalanceEdit(InfopartyBalance);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV23:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call partybalance add or edit
        /// </summary>
        /// <param name="inRowIndex"></param>
        public void PartyBalanceAddOrEdit(int inRowIndex)
        {
            int inTableRowCount = dtblPartyBalance.Rows.Count;

            try
            {
                for (int inJ = 0; inJ < inTableRowCount; inJ++)
                {
                    if (dgvJournalVoucher.Rows[inRowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() == dtblPartyBalance.Rows[inJ]["LedgerId"].ToString())
                    {
                        if (dtblPartyBalance.Rows[inJ]["PartyBalanceId"].ToString() == "0")
                        {
                            PartyBalanceAdd(inRowIndex, inJ);
                        }
                        else
                        {
                            decimal decPartyBalanceId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["partyBalanceId"]);
                            PartyBalanceEdit(decPartyBalanceId, inRowIndex, inJ);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV24:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to check invalid entries
        /// </summary>
        /// <param name="e"></param>
        public void CheckColumnMissing(DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvJournalVoucher.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value == null || dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvJournalVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvJournalVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbDrOrCr"].Value == null || dgvJournalVoucher.CurrentRow.Cells["dgvcmbDrOrCr"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvJournalVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvJournalVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvJournalVoucher.CurrentRow.Cells["dgvtxtAmount"].Value == null || dgvJournalVoucher.CurrentRow.Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvJournalVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvJournalVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;

                        }
                        else if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value == null || dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].FormattedValue.ToString() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvJournalVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvJournalVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;

                        }

                        else
                        {
                            isValueChanged = true;
                            dgvJournalVoucher.CurrentRow.HeaderCell.Value = string.Empty;
                        }

                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV25:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to remove row
        /// </summary>
        public void RemoveRow()
        {
            try
            {
                int inRowCount = dgvJournalVoucher.RowCount;
                if (inRowCount > 1)
                {
                    if (int.Parse(dgvJournalVoucher.CurrentRow.Cells["dgvtxtSlNo"].Value.ToString()) < inRowCount)
                    {
                        if (dgvJournalVoucher.CurrentRow.Cells["dgvtxtDetailsId"].Value != null && dgvJournalVoucher.CurrentRow.Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                        {
                            arrlstOfRemove.Add(dgvJournalVoucher.CurrentRow.Cells["dgvtxtDetailsId"].Value.ToString());
                            arrlstOfRemovedLedgerPostingId.Add(dgvJournalVoucher.CurrentRow.Cells["dgvtxtLedgerPostingId"].Value.ToString());
                            inArrOfRemove++;
                        }


                        int inTableRowCount = dtblPartyBalance.Rows.Count;
                        for (int inI = 0; inI < inTableRowCount; inI++)
                        {
                            if (dtblPartyBalance.Rows.Count == inI)
                            {
                                break;
                            }
                            if (dtblPartyBalance.Rows[inI]["LedgerId"].ToString() == dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString())
                            {
                                if (dtblPartyBalance.Rows[inI]["PartyBalanceId"].ToString() != "0")
                                {
                                    arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inI]["PartyBalanceId"]);
                                }
                                dtblPartyBalance.Rows.RemoveAt(inI);
                                inI--;
                            }
                        }
                        if (inUpdatingRowIndexForPartyRemove == dgvJournalVoucher.CurrentRow.Index)
                        {
                            inUpdatingRowIndexForPartyRemove = -1;
                            decUpdatingLedgerForPartyremove = 0;
                        }
                        dgvJournalVoucher.Rows.RemoveAt(dgvJournalVoucher.CurrentRow.Index);
                        SlNo();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV26:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to print the voucher
        /// </summary>
        /// <param name="decMasterId"></param>
        public void Print(decimal decMasterId)
        {
            try
            {
                JournalMasterSP SpJournalMaster = new JournalMasterSP();
                DataSet dsJournalVoucher = SpJournalMaster.JournalVoucherPrinting(decMasterId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.JournalVoucherPrinting(dsJournalVoucher);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV27:" + ex.Message;
            }


        }

        /// <summary>
        /// Function to print the voucher in dotmatrix
        /// </summary>
        /// <param name="decMasterId"></param>
        public void PrintForDotMatrix(decimal decMasterId)
        {
            try
            {

                DataTable dtblOtherDetails = new DataTable();
                CompanySP spComapany = new CompanySP();
                dtblOtherDetails = spComapany.CompanyViewForDotMatrix();
                //-------------Grid Details-------------------\\
                DataTable dtblGridDetails = new DataTable();
                dtblGridDetails.Columns.Add("SlNo");
                dtblGridDetails.Columns.Add("Account Ledger");
                dtblGridDetails.Columns.Add("CrOrDr");
                dtblGridDetails.Columns.Add("Amount");
                dtblGridDetails.Columns.Add("Currency");
                dtblGridDetails.Columns.Add("Cheque No");
                dtblGridDetails.Columns.Add("Cheque Date");
                int inRowCount = 0;
                foreach (DataGridViewRow dRow in dgvJournalVoucher.Rows)
                {
                    if (dRow.HeaderCell.Value != null && dRow.HeaderCell.Value.ToString() != "X")
                    {
                        if (!dRow.IsNewRow)
                        {
                            DataRow dr = dtblGridDetails.NewRow();
                            dr["SlNo"] = ++inRowCount;
                            dr["Account Ledger"] = dRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString();
                            dr["CrOrDr"] = dRow.Cells["dgvcmbDrOrCr"].Value.ToString();
                            dr["Amount"] = dRow.Cells["dgvtxtAmount"].Value.ToString();
                            dr["Currency"] = dRow.Cells["dgvcmbCurrency"].FormattedValue.ToString();
                            dr["Cheque No"] = (dRow.Cells["dgvtxtChequeNo"].Value == null ? "" : dRow.Cells["dgvtxtChequeNo"].Value.ToString());
                            dr["Cheque Date"] = (dRow.Cells["dgvtxtChequeDate"].Value == null ? "" : dRow.Cells["dgvtxtChequeDate"].Value.ToString());
                            dtblGridDetails.Rows.Add(dr);
                        }
                    }
                }


                //-------------Other Details-------------------\\

                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("DebitTotal");
                dtblOtherDetails.Columns.Add("CreditTotal");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("DebitAmountInWords");
                dtblOtherDetails.Columns.Add("CreditAmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtVoucherNo.Text;
                dRowOther["date"] = txtDate.Text;
                dRowOther["DebitTotal"] = txtDebitTotal.Text;
                dRowOther["CreditTotal"] = txtCreditTotal.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["DebitAmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtDebitTotal.Text), PublicVariables._decCurrencyId);
                dRowOther["CreditAmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtCreditTotal.Text), PublicVariables._decCurrencyId);
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");


                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decJournalVoucherTypeId);
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
                formMDI.infoError.ErrorString = "JV28:" + ex.Message;
            }
        }

        /// <summary>
        /// For validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keypressevent(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV29:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to add the new accountledger from accountLedger form
        /// </summary>
        /// <param name="decLedgerId"></param>
        public void CallFromAccountLedger(decimal decLedgerId)
        {
            try
            {
                if (decLedgerId != 0)
                {
                    AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                    DataTable dtbl = new DataTable();
                    dtbl = spAccountLedger.AccountLedgerViewAll();
                    DataGridViewComboBoxCell dgvccAccountLedger = (DataGridViewComboBoxCell)dgvJournalVoucher[dgvJournalVoucher.Columns["dgvcmbAccountLedger"].Index, dgvJournalVoucher.CurrentRow.Index];
                    dgvccAccountLedger.DataSource = dtbl;
                    dgvccAccountLedger.ValueMember = "ledgerId";
                    dgvccAccountLedger.DisplayMember = "ledgerName";
                    dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value = decLedgerId;
                }
                dgvJournalVoucher.Focus();
                this.Enabled = true;
                this.BringToFront();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV30:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to set the date
        /// </summary>
        public void VoucherDate()
        {
            try
            {
                dtpVoucherDate.MinDate = PublicVariables._dtFromDate;
                dtpVoucherDate.MaxDate = PublicVariables._dtToDate;

                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();

                infoComapany = spCompany.CompanyView(1);
                DateTime dtVoucherDate = infoComapany.CurrentDate;
                dtpVoucherDate.Value = dtVoucherDate;
                txtDate.Text = dtVoucherDate.ToString("dd-MMM-yyyy");
                dtpVoucherDate.Value = Convert.ToDateTime(txtDate.Text);
                txtDate.Focus();
                txtDate.SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV31:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to check the settings for printaftersave
        /// </summary>
        public void PrintCheck()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("TickPrintAfterSave") == "Yes")
                {
                    cbxPrintAfterSave.Checked = true;
                }
                else
                {
                    cbxPrintAfterSave.Checked = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV32:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the voucher to edit or delete while calling from the Journal report
        /// </summary>
        /// <param name="frmJournalReport"></param>
        /// <param name="decJournalMasterId"></param>
        public void CallFromJournalReport(frmJournalReport frmJournalReport, decimal decJournalMasterId)
        {
            try
            {
                frmJournalReport.Enabled = false;
                base.Show();
                isEditMode = true;
                btnDelete.Enabled = true;
                frmJournalReportObj = frmJournalReport;
                decJournalMasterIdForEdit = decJournalMasterId;
                FillFunction();


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV33:" + ex.Message;
            }
        }

        /// <summary>
        /// For enabling keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keypresseventEnable(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = false;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV34:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to delete partybalance of removed row
        /// </summary>
        public void DeletePartyBalanceOfRemovedRow()
        {
            PartyBalanceSP spPartyBalance = new PartyBalanceSP();
            try
            {
                foreach (object obj in arrlstOfDeletedPartyBalanceRow)
                {
                    string str = Convert.ToString(obj);
                    spPartyBalance.PartyBalanceDelete(Convert.ToDecimal(str));
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV35:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to load the form while calling from BillAllocation form
        /// </summary>
        /// <param name="frmBillallocation"></param>
        /// <param name="decJournalMasterId"></param>
        public void CallFromBillAllocation(frmBillallocation frmBillallocation, decimal decJournalMasterId)
        {
            try
            {
                frmBillallocation.Enabled = false;
                base.Show();
                isEditMode = true;
                btnDelete.Enabled = true;
                frmBillallocationObj = frmBillallocation;
                decJournalMasterIdForEdit = decJournalMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV36:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the fields for edit or delete
        /// </summary>
        public void FillFunction()
        {
            try
            {
                JournalMasterInfo infoJournalMaster = new JournalMasterInfo();
                JournalMasterSP spJournalMaster = new JournalMasterSP();
                infoJournalMaster = spJournalMaster.JournalMasterView(decJournalMasterIdForEdit);

                VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                infoVoucherType = spVoucherType.VoucherTypeView(infoJournalMaster.VoucherTypeId);
                this.Text = infoVoucherType.VoucherTypeName;

                txtVoucherNo.ReadOnly = false;
                strVoucherNo = infoJournalMaster.VoucherNo;
                strInvoiceNo = infoJournalMaster.InvoiceNo;
                txtVoucherNo.Text = strInvoiceNo;
                decJournalSuffixPrefixId = infoJournalMaster.SuffixPrefixId;
                decJournalVoucherTypeId = infoJournalMaster.VoucherTypeId;
                dtpVoucherDate.Value = infoJournalMaster.Date;
                txtNarration.Text = infoJournalMaster.Narration;

                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decJournalVoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.ReadOnly = true;
                }
                else
                {
                    txtVoucherNo.ReadOnly = false;
                }

                //GridFill
                DataTable dtbl = new DataTable();
                JournalDetailsSP spJournalDetailsSp = new JournalDetailsSP();
                dtbl = spJournalDetailsSp.JournalDetailsViewByMasterId(decJournalMasterIdForEdit);

                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                for (int inI = 0; inI < dtbl.Rows.Count; inI++)
                {
                    dgvJournalVoucher.Rows.Add();
                    dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value = Convert.ToDecimal(dtbl.Rows[inI]["ledgerId"].ToString());

                    if (Convert.ToDecimal(dtbl.Rows[inI]["debit"].ToString()) == 0)
                    {
                        dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value = "Cr";
                        dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value = Convert.ToDecimal(dtbl.Rows[inI]["credit"].ToString());
                    }
                    else
                    {
                        dgvJournalVoucher.Rows[inI].Cells["dgvcmbDrOrCr"].Value = "Dr";
                        dgvJournalVoucher.Rows[inI].Cells["dgvtxtAmount"].Value = Convert.ToDecimal(dtbl.Rows[inI]["debit"].ToString());
                    }
                    dgvJournalVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(dtbl.Rows[inI]["exchangeRateId"].ToString());
                    if (dtbl.Rows[inI]["chequeNo"].ToString() != string.Empty)
                    {
                        dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value = dtbl.Rows[inI]["chequeNo"].ToString();
                        dgvJournalVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value = (Convert.ToDateTime(dtbl.Rows[inI]["chequeDate"].ToString())).ToString("dd-MMM-yyyy");
                    }
                    dgvJournalVoucher.Rows[inI].Cells["dgvtxtDetailsId"].Value = dtbl.Rows[inI]["journalDetailsId"].ToString();

                    decimal decDetailsId1 = Convert.ToDecimal(dtbl.Rows[inI]["journalDetailsId"].ToString());
                    decimal decLedgerPostingId = spLedgerPosting.LedgerPostingIdFromDetailsId(decDetailsId1, strVoucherNo, decJournalVoucherTypeId);
                    dgvJournalVoucher.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value = decLedgerPostingId.ToString();
                    btnSave.Text = "Update";

                }

                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                DataTable dtbl1 = new DataTable();
                dtbl1 = SpPartyBalance.PartyBalanceViewByVoucherNoAndVoucherType(decJournalVoucherTypeId, strVoucherNo, infoJournalMaster.Date);


                dtblPartyBalance = dtbl1;
                dgvJournalVoucher.ClearSelection();
                txtDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV37:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from LedgerDetails form
        /// </summary>
        /// <param name="frmLedgerDetails"></param>
        /// <param name="decMasterId"></param>
        public void CallFromLedgerDetails(frmLedgerDetails frmLedgerDetails, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmLedgerDetailsObj = frmLedgerDetails;
                frmLedgerDetailsObj.Enabled = false;
                isEditMode = true;
                btnDelete.Enabled = true;
                decJournalMasterIdForEdit = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV38:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to load the form while calling from DayBook form
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decMasterId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decMasterId)
        {
            try
            {
                frmDayBook.Enabled = false;
                base.Show();
                isEditMode = true;
                btnDelete.Enabled = true;
                frmDayBookObj = frmDayBook;
                decJournalMasterIdForEdit = decMasterId;
                FillFunction();
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV39:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from Ageingreport form
        /// </summary>
        /// <param name="frmAgeing"></param>
        /// <param name="decMasterId"></param>
        public void callFromAgeing(frmAgeingReport frmAgeing, decimal decMasterId)
        {
            try
            {
                frmAgeing.Enabled = false;
                base.Show();

                btnDelete.Enabled = true;
                frmAgeingObj = frmAgeing;
                decJournalMasterIdForEdit = decMasterId;
                FillFunction();

            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV40:" + ex.Message;
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// On changing the date from dtpVoucherDate, sets the txtDate with the new date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpVoucherDate.Value;
                this.txtDate.Text = date.ToString("dd-MMM-yyyy");
                CurrencyComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV41:" + ex.Message;
            }
        }

        /// <summary>
        /// Date validation while leaving txtDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtDate);
                if (!isInvalid)
                {
                    txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtDate.Text;
                dtpVoucherDate.Value = Convert.ToDateTime(date);
                CurrencyComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV42:" + ex.Message;
            }
        }

        /// <summary>
        /// On Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveOrEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV43:" + ex.Message;
            }
        }

        /// <summary>
        /// On CellEnter of dgvCreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvJournalVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvJournalVoucher.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvJournalVoucher.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV44:" + ex.Message;
            }
        }

        /// <summary>
        /// On Close button click
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
                formMDI.infoError.ErrorString = "JV45:" + ex.Message;
            }
        }

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmJournalVoucher_Load(object sender, EventArgs e)
        {
            try
            {

                AccountLedgerComboFill();
                DrOrCrComboFill();
                clear();
                CurrencyComboFill();
                DebitAndCreditTotal();

                /************For PartyBalance***********************/
                // dtblPartyBalance.Columns.Add("RowIndex", typeof(int));
                dtblPartyBalance.Columns.Add("LedgerId", typeof(decimal));
                dtblPartyBalance.Columns.Add("AgainstVoucherTypeId", typeof(decimal));
                dtblPartyBalance.Columns.Add("AgainstVoucherNo", typeof(string));
                dtblPartyBalance.Columns.Add("ReferenceType", typeof(string));
                dtblPartyBalance.Columns.Add("Amount", typeof(decimal));
                dtblPartyBalance.Columns.Add("AgainstInvoiceNo", typeof(string));
                dtblPartyBalance.Columns.Add("CurrencyId", typeof(decimal));
                dtblPartyBalance.Columns.Add("DebitOrCredit", typeof(string));
                dtblPartyBalance.Columns.Add("PendingAmount", typeof(decimal));
                dtblPartyBalance.Columns.Add("PartyBalanceId", typeof(decimal));
                dtblPartyBalance.Columns.Add("VoucherTypeId", typeof(decimal));
                dtblPartyBalance.Columns.Add("VoucherNo", typeof(string));
                dtblPartyBalance.Columns.Add("InvoiceNo", typeof(string));
                dtblPartyBalance.Columns.Add("OldExchangeRate", typeof(decimal));
                //ArrayList of deleted partybalance row in update mode
                arrlstOfDeletedPartyBalanceRow = new ArrayList();

                /***********************************************************/
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV46:" + ex.Message;
            }

        }

        /// <summary>
        /// On Clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                if (frmJournalReportObj != null)
                {
                    frmJournalReportObj.Close();
                    frmJournalReportObj = null;
                }
                if (journalRegisterObj != null)
                {
                    journalRegisterObj.Close();
                    journalRegisterObj = null;
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Close();
                    frmDayBookObj = null;
                }
                if (frmAgeingObj != null)
                {
                    frmAgeingObj.Close();
                    frmAgeingObj = null;
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj = null;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV47:" + ex.Message;
            }
        }


        /// <summary>
        /// On remove linkbutton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dgvJournalVoucher.RowCount > 1)
                {
                    if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        RemoveRow();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV48:" + ex.Message;
            }
        }

        /// <summary>
        /// Calling the keypress event for validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridViewTextBoxEditingControl txt = e.Control as DataGridViewTextBoxEditingControl;
                if (dgvJournalVoucher.CurrentCell.ColumnIndex == dgvJournalVoucher.Columns["dgvtxtAmount"].Index)
                {
                    txt.KeyPress += keypressevent;
                }
                else if (dgvJournalVoucher.CurrentCell.ColumnIndex == dgvJournalVoucher.Columns["dgvtxtChequeNo"].Index)
                {
                    txt.KeyPress += keypresseventEnable;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV49:" + ex.Message;
            }
        }

        /// <summary>
        /// On cellvalueChanged of dgvCreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    DebitAndCreditTotal();

                    if (dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {

                        if (dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value == null || dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value.ToString() == string.Empty)
                        {
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1); //decExchangeRateId;
                        }

                    }

                    if (dgvJournalVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbAccountLedger")
                    {
                        /*************Remove partybalance while changing the ledger ************/
                        if (inUpdatingRowIndexForPartyRemove != -1)
                        {
                            int inTableRowCount = dtblPartyBalance.Rows.Count;
                            for (int inJ = 0; inJ < inTableRowCount; inJ++)
                            {
                                if (dtblPartyBalance.Rows.Count == inJ)
                                {
                                    break;
                                }

                                if (Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["ledgerId"].ToString()) == decUpdatingLedgerForPartyremove)
                                {
                                    if (dtblPartyBalance.Rows[inJ]["PartyBalanceId"].ToString() != "0")
                                    {
                                        arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inJ]["PartyBalanceId"]);
                                    }
                                    dtblPartyBalance.Rows.RemoveAt(inJ);
                                    inJ--;
                                }
                            }

                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;

                            //reset
                            decUpdatingLedgerForPartyremove = 0;
                            inUpdatingRowIndexForPartyRemove = -1;
                        }
                        /*************************************************************************/

                        //-----------To make amount readonly when party is selected as ledger------------------------------//
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        if (spAccountLedger.AccountGroupIdCheck(dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].ReadOnly = true;

                        }
                        else
                        {
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].ReadOnly = false;
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

                        //========================================================================================//
                    }

                    if (dgvJournalVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbDrOrCr")
                    {
                        /*************Remove partybalance while changing the Dr/Cr ************/
                        if (inUpdatingRowIndexForPartyRemove != -1)
                        {
                            int inTableRowCount = dtblPartyBalance.Rows.Count;
                            for (int inJ = 0; inJ < inTableRowCount; inJ++)
                            {
                                if (dtblPartyBalance.Rows.Count == inJ)
                                {
                                    break;
                                }

                                if (Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["ledgerId"].ToString()) == decUpdatingLedgerForPartyremove)
                                {
                                    if (dtblPartyBalance.Rows[inJ]["PartyBalanceId"].ToString() != "0")
                                    {
                                        arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inJ]["PartyBalanceId"]);
                                    }
                                    dtblPartyBalance.Rows.RemoveAt(inJ);
                                    inJ--;
                                }
                            }
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;

                            //reset
                            decUpdatingLedgerForPartyremove = 0;
                            inUpdatingRowIndexForPartyRemove = -1;
                        }
                        /*************************************************************************/
                    }

                    //-----------------------------------Chequedate validation----------------------------------//
                    DateValidation obj = new DateValidation();
                    TextBox txtDate1 = new TextBox();
                    if (dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value != null && dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                    {
                        txtDate1.Text = dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value.ToString();
                        bool isInvalid = obj.DateValidationFunction(txtDate1);
                        if (!isInvalid)
                        {
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = txtDate1.Text;
                        }
                    }
                    //=========================================================================================//
                    //---------------------check column missing---------------------------------//

                    CheckColumnMissing(e);

                    //==========================================================================//

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV50:" + ex.Message;
            }
        }

        /// <summary>
        /// For the shortcut keys
        /// Esc for form closing
        /// ctrl+s for save
        /// ctrl+d for delete
        /// alt+c for ledger creation
        /// ctrl+f for ledger popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmJournalVoucher_KeyDown(object sender, KeyEventArgs e)
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

                if (dgvJournalVoucher.RowCount > 0)
                {


                    //-----------------------for ledger creation----------------------------------//
                    if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)//Ledger creation
                    {
                        if (dgvJournalVoucher.CurrentCell == dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"])
                        {
                           // SendKeys.Send("{F10}");
                            frmAccountLedger accounLedgerObj = new frmAccountLedger();
                            accounLedgerObj.MdiParent = formMDI.MDIObj;
                            if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                            {
                                string strLedgerName = dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString();
                                accounLedgerObj.CallFromJournalVoucher(this, strLedgerName);
                            }
                            else
                            {
                                string strLedgerName = string.Empty;
                                accounLedgerObj.CallFromJournalVoucher(this, strLedgerName);
                            }
                        }

                    }
                    //========================================================================//

                    //--------------------For ledger Popup------------------------------------//

                    if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)//Ledger popup
                    {
                        if (dgvJournalVoucher.CurrentCell == dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"])
                        {
                            frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                            frmLedgerPopupObj.MdiParent = formMDI.MDIObj;

                            if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                            {
                                decLedgerIdForPopUp = Convert.ToDecimal(dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString());
                                frmLedgerPopupObj.CallFromJournalVoucher(this, decLedgerIdForPopUp, string.Empty);

                            }
                        }

                    }
                    //========================================================================// 


                    if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                    {
                        if (dgvJournalVoucher.CurrentCell == dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"])
                        {
                            frmCurrencyObj = new frmCurrencyDetails();
                            frmCurrencyObj.MdiParent = formMDI.MDIObj;
                            if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value != null && dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString() != string.Empty)
                            {
                                frmCurrencyObj.CallFromJournalVoucher(this, Convert.ToDecimal(dgvJournalVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString()));
                            }
                        }
                    }

                }


                //-----------------------CTRL+S Save-----------------------------//
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    btnSave_Click(sender, e);
                }
                //===============================================================//

                //-----------------------CTRL+D Delete-----------------------------//
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control) //Delete
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
                //=====================================================================//
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV51:" + ex.Message;
            }

        }

        /// <summary>
        /// On delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (PublicVariables.isMessageDelete)
                    {
                        if (Messages.DeleteMessage())
                        {
                            DeleteFunction(decJournalMasterIdForEdit);
                        }
                        dgvJournalVoucher.Focus();
                    }
                    else
                    {
                        DeleteFunction(decJournalMasterIdForEdit);
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV52:" + ex.Message;
            }
        }

        /// <summary>
        /// While adding new row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SlNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV53:" + ex.Message;
            }
        }

        /// <summary>
        /// For committing edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvJournalVoucher.IsCurrentCellDirty)
                {
                    dgvJournalVoucher.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV54:" + ex.Message;
            }
        }


        /// <summary>
        /// For against button click in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvJournalVoucher.CurrentCell.ColumnIndex == dgvJournalVoucher.Columns["dgvbtnAgainst"].Index)
                    {
                        AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
                        if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            if (dgvJournalVoucher.CurrentRow.Cells["dgvcmbDrOrCr"].Value != null && dgvJournalVoucher.CurrentRow.Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                            {
                                if (SpAccountLedger.AccountGroupIdCheck(dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                                {
                                    frmPartyBalanceObj = new frmPartyBalance();
                                    frmPartyBalanceObj.MdiParent = formMDI.MDIObj;
                                    decimal decLedgerId = Convert.ToDecimal(dgvJournalVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString());

                                    string strDebitOrCredit = dgvJournalVoucher.CurrentRow.Cells["dgvcmbDrOrCr"].Value.ToString();
                                    if (!isAutomatic)
                                    {
                                        frmPartyBalanceObj.CallFromJournalVoucher(this, decLedgerId, dtblPartyBalance, strDebitOrCredit, decJournalVoucherTypeId, txtVoucherNo.Text, Convert.ToDateTime(txtDate.Text), arrlstOfDeletedPartyBalanceRow);
                                    }
                                    else
                                    {
                                        frmPartyBalanceObj.CallFromJournalVoucher(this, decLedgerId, dtblPartyBalance, strDebitOrCredit, decJournalVoucherTypeId, strVoucherNo, Convert.ToDateTime(txtDate.Text), arrlstOfDeletedPartyBalanceRow);
                                    }

                                }

                            }
                            else
                            {
                                Messages.InformationMessage("Select debit or credit");
                            }
                        }
                        else
                        {
                            Messages.InformationMessage("Select any ledger");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV55:" + ex.Message;
            }
        }

        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmJournalVoucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (journalRegisterObj != null)
                {
                    journalRegisterObj.Enabled = true;
                    journalRegisterObj.SearchRegister();
                }
                else if (frmJournalReportObj != null)
                {
                    frmJournalReportObj.Enabled = true;
                    frmJournalReportObj.Search();
                }
                if (frmBillallocationObj != null)
                {
                    frmBillallocationObj.Enabled = true;
                    frmBillallocationObj.BillAllocationGridFill();
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();
                    frmDayBookObj = null;
                }
                if (frmAgeingObj != null)
                {
                    frmAgeingObj.Enabled = true;
                    frmAgeingObj.FillGrid();
                    frmAgeingObj = null;
                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Enabled = true;
                    objVoucherSearch.GridFill();
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.LedgerDetailsView();
                    frmLedgerDetailsObj = null;
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV56:" + ex.Message;
            }
        }

        /// <summary>
        /// On cellBeginEdit of dgvJournalVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                inUpdatingRowIndexForPartyRemove = -1;
                decUpdatingLedgerForPartyremove = 0;
                DataTable dtbl = new DataTable();
                AccountLedgerSP SpAccountLedger = new AccountLedgerSP();

                if (dgvJournalVoucher.CurrentCell.ColumnIndex == dgvJournalVoucher.Columns["dgvcmbAccountLedger"].Index)
                {
                    dtbl = SpAccountLedger.AccountLedgerViewAll();
                    DataRow dr = dtbl.NewRow();
                    dr[0] = 0;
                    dr[2] = string.Empty;
                    dtbl.Rows.InsertAt(dr, 0);

                    if (dtbl.Rows.Count > 0)
                    {
                        if (dgvJournalVoucher.RowCount > 1)
                        {
                            int inGridRowCount = dgvJournalVoucher.RowCount;
                            for (int inI = 0; inI < inGridRowCount - 1; inI++)
                            {
                                if (inI != e.RowIndex)
                                {
                                    int inTableRowcount = dtbl.Rows.Count;
                                    for (int inJ = 0; inJ < inTableRowcount; inJ++)
                                    {
                                        if (dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                        {
                                            if (dtbl.Rows[inJ]["ledgerId"].ToString() == dgvJournalVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString())
                                            {
                                                dtbl.Rows.RemoveAt(inJ);
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        DataGridViewComboBoxCell dgvccVoucherType = (DataGridViewComboBoxCell)dgvJournalVoucher[dgvJournalVoucher.Columns["dgvcmbAccountLedger"].Index, e.RowIndex];
                        dgvccVoucherType.DataSource = dtbl;
                        dgvccVoucherType.ValueMember = "ledgerId";
                        dgvccVoucherType.DisplayMember = "ledgerName";
                    }
                }

                if (dgvJournalVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbAccountLedger")
                {
                    if (dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        if (spAccountLedger.AccountGroupIdCheck(dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            inUpdatingRowIndexForPartyRemove = e.RowIndex;
                            decUpdatingLedgerForPartyremove = Convert.ToDecimal(dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                    }
                }
                if (dgvJournalVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbDrOrCr")
                {
                    if (dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        if (spAccountLedger.AccountGroupIdCheck(dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            inUpdatingRowIndexForPartyRemove = e.RowIndex;
                            decUpdatingLedgerForPartyremove = Convert.ToDecimal(dgvJournalVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV57:" + ex.Message;
            }
        }

        /// <summary>
        /// For handling dataerror
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
                {
                    object value = dgvJournalVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (!((DataGridViewComboBoxColumn)dgvJournalVoucher.Columns[e.ColumnIndex]).Items.Contains(value))
                    {
                        e.ThrowException = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV58:" + ex.Message;
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// For enter key navigation txtVoucherNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV59:" + ex.Message;
            }
        }

        /// <summary>
        /// For enter key and backspace navigation of txtDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvJournalVoucher.Focus();

                }
                if (txtDate.Text == string.Empty || txtDate.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        if (!txtVoucherNo.ReadOnly)
                        {
                            txtVoucherNo.Focus();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV60:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of txtNarration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text.Trim() == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        if (dgvJournalVoucher.RowCount > 0)
                        {
                            dgvJournalVoucher.Focus();
                        }
                        else
                        {
                            txtDate.Focus();
                            txtDate.SelectionStart = txtDate.TextLength;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV61:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key navigation of txtNarration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inNarrationCount++;
                    if (inNarrationCount == 2)
                    {
                        inNarrationCount = 0;
                        btnSave.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV62:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of btnSave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV63:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of btnClear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV64:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of btnDelete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete.Focus();
                    }
                    else
                    {
                        btnClear.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV65:" + ex.Message;
            }
        }

        /// <summary>
        /// For enter key and backspace navigation of dgvJournalVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJournalVoucher_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int inDgvJournalRowCount = dgvJournalVoucher.Rows.Count;
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvJournalVoucher.CurrentCell == dgvJournalVoucher.Rows[inDgvJournalRowCount - 1].Cells["dgvtxtChequeDate"])
                    {
                        txtNarration.Focus();
                        txtNarration.SelectionStart = txtNarration.TextLength;
                        dgvJournalVoucher.ClearSelection();
                    }

                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvJournalVoucher.CurrentCell == dgvJournalVoucher.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = txtDate.TextLength;
                        dgvJournalVoucher.ClearSelection();
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "JV66:" + ex.Message;
            }
        }

        #endregion



    }
}
