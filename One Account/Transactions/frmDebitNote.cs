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
    public partial class frmDebitNote : Form
    {
        

        #region Public variables

        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decDebitNoteVoucherTypeId = 0;
        string strVoucherNo = string.Empty;
        bool isAutomatic = false;
        decimal decDebitNoteSuffixPrefixId = 0;
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        string tableName = "DebitNoteMaster";
        string strInvoiceNo = string.Empty;
        bool isEditMode = false;
        int inNarrationCount = 0;
        DataTable dtblPartyBalance = new DataTable();
        ArrayList arrlstOfDeletedPartyBalanceRow;
        frmCurrencyDetails frmCurrencyObj = null;
        frmDebitNoteRegister frmDebitNoteRegisterObj = null;
        frmPartyBalance frmPartyBalanceObj = null;
        frmDebitNoteReport frmDebitNoteReportObj = null;
        ArrayList arrlstOfRemove = new ArrayList();
        ArrayList arrlstOfRemovedLedgerPostingId = new ArrayList();
        frmBillallocation frmBillallocationObj = null;
        frmAgeingReport frmAgeingObj = null;//To use in call from frmAgeing
        public string strVocherNo;
        decimal decSelectedCurrencyRate = 0;
        decimal decConvertRate = 0;
        decimal decAmount = 0;
        bool isValueChanged = false;
        decimal decDebitNoteMasterIdForEdit = 0;

        decimal decLedgerIdForPopUp = 0;

        DataTable dtblTemporaryPartyBalance = new DataTable();//to store PartyBalance entries in update mode while closing the form
        frmDayBook frmDayBookObj = null;//To use in call from frmdaybook
        int inUpdatingRowIndexForPartyRemove = -1;
        decimal decUpdatingLedgerForPartyremove = 0;
        frmVoucherSearch objVoucherSearch = null;
        frmLedgerDetails frmLedgerDetailsObj;
        #endregion

        #region Functions

        /// <summary>
        /// Create instance of frmDebitNote
        /// </summary>
        public frmDebitNote()
        {
            InitializeComponent();
        }

        /// <summary>
        /// It is a function for vouchertypeselection form to select perticular voucher and open the form under the vouchertype
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strDebitNoteVoucherTypeName"></param>
        public void CallFromVoucherTypeSelection(decimal decVoucherTypeId, string strDebitNoteVoucherTypeName)
        {
            try
            {
                decDebitNoteVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decDebitNoteVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decDebitNoteVoucherTypeId, dtpVoucherDate.Value);
                decDebitNoteSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                strPrefix = infoSuffixPrefix.Prefix;
                strSuffix = infoSuffixPrefix.Suffix;
                this.Text = strDebitNoteVoucherTypeName;
                base.Show();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT1:" + ex.Message;
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
                decDebitNoteMasterIdForEdit = decId;
                btnSave.Text = "Update";
                isEditMode = true;
                btnDelete.Enabled = true;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                DebitNoteMasterSP spDebitNoteMaster = new DebitNoteMasterSP();

                //-----------------------------------VoucherNo automatic generation-------------------------------------------//

                if (strVoucherNo == string.Empty)
                {
                  
                    strVoucherNo = "0"; //strMax;
                }
                strVoucherNo = obj.VoucherNumberAutomaicGeneration(decDebitNoteVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);

                if (Convert.ToDecimal(strVoucherNo) != spDebitNoteMaster.DebitNoteMasterGetMaxPlusOne(decDebitNoteVoucherTypeId))
                {
                    strVoucherNo = spDebitNoteMaster.DebitNoteMasterGetMax(decDebitNoteVoucherTypeId).ToString();
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decDebitNoteVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                    if (spDebitNoteMaster.DebitNoteMasterGetMax(decDebitNoteVoucherTypeId).ToString() == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decDebitNoteVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, tableName);
                    }
                }

                //===================================================================================================================//
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();

                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decDebitNoteVoucherTypeId, dtpVoucherDate.Value);
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
                    strInvoiceNo = txtVoucherNo.Text;
                }

                dgvDebitNote.Rows.Clear();
                VoucherDate();
                dtpVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtDebitTotal.Text = string.Empty;
                txtCreditTotal.Text = string.Empty;
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                isEditMode = false;
                dtblPartyBalance.Clear();//To clear party balance entries to clear the dgvpartybalance
                PrintCheck();
                if (!txtVoucherNo.ReadOnly)
                {
                    txtVoucherNo.Focus();
                }
                else
                {
                    txtDate.Select();
                }
                txtNarration.Text = string.Empty;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT3:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT4:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT5:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT6:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT7:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT8:" + ex.Message;
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
                foreach (DataGridViewRow dr in dgvDebitNote.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowNo;
                    inRowNo++;
                    if (dr.Index == dgvDebitNote.Rows.Count - 1)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT9:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to calculate total debit and credit
        /// </summary>
        public void DebitAndCreditTotal()
        {
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            int inRowCount = dgvDebitNote.RowCount;
            decimal decTxtTotalDebit = 0;
            decimal decTxtTotalCredit = 0;
            try
            {
                for (int inI = 0; inI < inRowCount; inI++)
                {
                    if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                    {
                        if (dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            if (dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString() != string.Empty)
                            {
                                if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                                {
                                    //--------Currency conversion--------------//
                                    decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));
                                    decAmount = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                    decConvertRate = decAmount * decSelectedCurrencyRate;
                                    //===========================================//
                                    decTxtTotalDebit = decTxtTotalDebit + decConvertRate;

                                }
                                else
                                {
                                    //--------Currency conversion--------------//
                                    decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));
                                    decAmount = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                    decConvertRate = decAmount * decSelectedCurrencyRate;
                                    //===========================================//
                                    decTxtTotalCredit = decTxtTotalCredit + decConvertRate;
                                }
                            }
                        }
                    }
                    txtDebitTotal.Text = Math.Round(decTxtTotalDebit, PublicVariables._inNoOfDecimalPlaces).ToString();
                    txtCreditTotal.Text = Math.Round(decTxtTotalCredit, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT10:" + ex.Message;
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
                if (dgvDebitNote.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value == null || dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvDebitNote.CurrentRow.HeaderCell.Value = "X";
                            dgvDebitNote.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvDebitNote.CurrentRow.Cells["dgvcmbDrOrCr"].Value == null || dgvDebitNote.CurrentRow.Cells["dgvcmbDrOrCr"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvDebitNote.CurrentRow.HeaderCell.Value = "X";
                            dgvDebitNote.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvDebitNote.CurrentRow.Cells["dgvtxtAmount"].Value == null || dgvDebitNote.CurrentRow.Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvDebitNote.CurrentRow.HeaderCell.Value = "X";
                            dgvDebitNote.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;

                        }
                        else if (dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].Value == null || dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].FormattedValue.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvDebitNote.CurrentRow.HeaderCell.Value = "X";
                            dgvDebitNote.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;

                        }

                        else
                        {
                            isValueChanged = true;
                            dgvDebitNote.CurrentRow.HeaderCell.Value = string.Empty;
                        }

                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT11:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT12:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT13:" + ex.Message;
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

                dgvDebitNote.CurrentRow.Cells["dgvtxtAmount"].Value = decAmount.ToString();
                dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1);
                dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].ReadOnly = true;
                this.frmPartyBalanceObj = frmPartyBalance;
                frmPartyBalance.Close();
                frmPartyBalanceObj = null;
                dtblPartyBalance = dtbl;
                arrlstOfDeletedPartyBalanceRow = arrlstOfRemovedRow;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT14:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to remove row
        /// </summary>
        public void RemoveRow()
        {
            try
            {
                int inRowCount = dgvDebitNote.RowCount;
                if (inRowCount > 1)
                {
                    if (int.Parse(dgvDebitNote.CurrentRow.Cells["dgvtxtSlNo"].Value.ToString()) < inRowCount)
                    {
                        if (dgvDebitNote.CurrentRow.Cells["dgvtxtDetailsId"].Value != null && dgvDebitNote.CurrentRow.Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                        {
                            arrlstOfRemove.Add(dgvDebitNote.CurrentRow.Cells["dgvtxtDetailsId"].Value.ToString());
                            arrlstOfRemovedLedgerPostingId.Add(dgvDebitNote.CurrentRow.Cells["dgvtxtLedgerPostingId"].Value.ToString());
                        }


                        int inTableRowCount = dtblPartyBalance.Rows.Count;
                        for (int inI = 0; inI < inTableRowCount; inI++)
                        {
                            if (dtblPartyBalance.Rows.Count == inI)
                            {
                                break;
                            }
                            if (dtblPartyBalance.Rows[inI]["LedgerId"].ToString() == dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString())
                            {
                                if (dtblPartyBalance.Rows[inI]["PartyBalanceId"].ToString() != "0")
                                {
                                    arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inI]["PartyBalanceId"]);
                                }
                                dtblPartyBalance.Rows.RemoveAt(inI);
                                inI--;
                            }
                        }
                        if (inUpdatingRowIndexForPartyRemove == dgvDebitNote.CurrentRow.Index)
                        {
                            inUpdatingRowIndexForPartyRemove = -1;
                            decUpdatingLedgerForPartyremove = 0;
                        }
                        dgvDebitNote.Rows.RemoveAt(dgvDebitNote.CurrentRow.Index);
                        SlNo();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT15:" + ex.Message;
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
                dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].Value = decId;
                dgvDebitNote.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT16:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the voucher to edit or delete while calling from the creditnote register
        /// </summary>
        /// <param name="frmDebitNoteObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromDebitNoteRegister(frmDebitNoteRegister frmDebitNoteObj, decimal decMasterId)
        {
            try
            {
                base.Show();
                isEditMode = true;
                btnDelete.Enabled = true;
                frmDebitNoteRegisterObj = frmDebitNoteObj;
                frmDebitNoteRegisterObj.Enabled = false;
                decDebitNoteMasterIdForEdit = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT17:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call the Save after checking invalid entries
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
                    if (isEditMode == false)
                    {
                        decimal decTotalDebit = 0;
                        decimal decTotalCredit = 0;

                        int inRowCount = dgvDebitNote.RowCount;
                        for (int inI = 0; inI < inRowCount - 1; inI++)
                        {
                            if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }
                            else if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }

                            else if (dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                                inIfGridColumnMissing = 1;
                                continue;
                            }
                            else if (dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].FormattedValue.ToString().Trim() == string.Empty)
                            {
                                arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
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
                                int inDgvDebitNoteRowCount = dgvDebitNote.RowCount;
                                int inK = 0;
                                for (int inI = 0; inI < inDgvDebitNoteRowCount; inI++)
                                {
                                    if (inK == arrlstOfRowToRemove.Count)
                                    {
                                        break;
                                    }
                                    if (inDgvDebitNoteRowCount > 0)
                                    {

                                        if (Convert.ToInt32(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString()) == Convert.ToInt32(arrlstOfRowToRemove[inK]))
                                        {
                                            inK++;
                                            if (dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                                            {
                                                arrlstOfRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString());
                                                arrlstOfRemovedLedgerPostingId.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                            }

                                            inTableRowCount = dtblPartyBalance.Rows.Count;
                                            for (int inJ = 0; inJ < inTableRowCount; inJ++)
                                            {
                                                if (dtblPartyBalance.Rows.Count == inJ)
                                                {
                                                    break;
                                                }
                                                if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                                {
                                                    if (Convert.ToInt32(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString()) == Convert.ToInt32(dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString()))
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
                                            dgvDebitNote.Rows.Remove(dgvDebitNote.Rows[inI]);
                                            inDgvDebitNoteRowCount = dgvDebitNote.RowCount;
                                            inI--;

                                        }
                                    }
                                }
                                SlNo();

                            }
                            //============================================================//
                            int RowCount = dgvDebitNote.RowCount;
                            if (RowCount > 1)
                            {
                                decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                                decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());
                                if (decTotalCredit != 0 && decTotalDebit != 0)
                                {
                                    if (decTotalDebit == decTotalCredit)
                                    {
                                        if (PublicVariables.isMessageAdd)
                                        {
                                            if (Messages.SaveMessage())
                                            {

                                                Save();
                                                Clear();
                                            }
                                            else
                                            {
                                                dgvDebitNote.Focus();
                                            }
                                        }
                                        else
                                        {
                                            Save();
                                            Clear();

                                        }
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Total debit and total credit should be equal");
                                        dgvDebitNote.Focus();
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
                formMDI.infoError.ErrorString = "DRNT18:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to Call the Edit after checking invalid entries
        /// </summary>
        /// <param name="decDebitNoteMasterId"></param>
        public void EditFunction(decimal decDebitNoteMasterId)
        {
            try
            {
                ArrayList arrlstOfRowToRemove = new ArrayList();
                int inReadyForSave = 0;
                int inIsRowToRemove = 0;
                int inIfGridColumnMissing = 0;


                decimal decTotalDebit = 0;
                decimal decTotalCredit = 0;

                int inRowCount = dgvDebitNote.RowCount;
                for (int inI = 0; inI < inRowCount - 1; inI++)
                {
                    if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }
                    else if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }

                    else if (dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        inIfGridColumnMissing = 1;
                        continue;
                    }
                    else if (dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value == null || dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].FormattedValue.ToString().Trim() == string.Empty)
                    {
                        arrlstOfRowToRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
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
                        int inDgvDebitNoteRowCount = dgvDebitNote.RowCount;
                        int inK = 0;
                        for (int inI = 0; inI < inDgvDebitNoteRowCount; inI++)
                        {
                            if (inK == arrlstOfRowToRemove.Count)
                            {
                                break;
                            }
                            if (inDgvDebitNoteRowCount > 0)
                            {

                                if (Convert.ToInt32(dgvDebitNote.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString()) == Convert.ToInt32(arrlstOfRowToRemove[inK]))
                                {
                                    inK++;
                                    if (dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                                    {
                                        arrlstOfRemove.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString());
                                        arrlstOfRemovedLedgerPostingId.Add(dgvDebitNote.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                    }

                                    inTableRowCount = dtblPartyBalance.Rows.Count;
                                    for (int inJ = 0; inJ < inTableRowCount; inJ++)
                                    {
                                        if (dtblPartyBalance.Rows.Count == inJ)
                                        {
                                            break;
                                        }
                                        if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                        {
                                            if (dtblPartyBalance.Rows[inJ]["LedgerId"].ToString() == dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString())
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
                                    dgvDebitNote.Rows.RemoveAt(dgvDebitNote.Rows[inI].Index);
                                    inDgvDebitNoteRowCount = dgvDebitNote.RowCount;
                                    inI--;
                                }
                            }
                        }
                        SlNo();
                    }
                    //============================================================//
                    inRowCount = dgvDebitNote.RowCount;
                    if (inRowCount > 1)
                    {
                        decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                        decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());
                        if (decTotalDebit == decTotalCredit)
                        {
                            if (PublicVariables.isMessageEdit)
                            {
                                if (Messages.UpdateMessage())
                                {
                                    DeletePartyBalanceOfRemovedRow();
                                    Edit(decDebitNoteMasterId);

                                }
                                else
                                {
                                    dgvDebitNote.Focus();
                                }
                            }
                            else
                            {
                                DeletePartyBalanceOfRemovedRow();
                                Edit(decDebitNoteMasterId);
                            }
                        }
                        else
                        {
                            Messages.InformationMessage("Total debit and total credit should be equal");
                            dgvDebitNote.Focus();
                            return;
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Can't save without atleat two complete details");
                    }
                }
               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT19:" + ex.Message;
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
                    int inRowCount = dgvDebitNote.RowCount;
                    for (int i = 0; i < inRowCount - 1; i++)
                    {
                        decimal decledgerId = 0;
                        if (dgvDebitNote.Rows[i].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[i].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            decledgerId = Convert.ToDecimal(dgvDebitNote.Rows[i].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                        decBalance = spAccountLedger.CheckLedgerBalance(decledgerId);
                        if (dgvDebitNote.Rows[i].Cells["dgvtxtAmount"].Value != null && dgvDebitNote.Rows[i].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            decCalcAmount = decBalance - Convert.ToDecimal(dgvDebitNote.Rows[i].Cells["dgvtxtAmount"].Value.ToString());
                        }
                        if (decCalcAmount < 0)
                        {
                            isNegativeLedger = true;
                            break;
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
                formMDI.infoError.ErrorString = "DRNT20:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to determine whether to call SaveFunction or EditFunction function 
        /// </summary>
        public void SaveOrEditFunction()
        {
            try
            {
                DebitNoteMasterSP spDebitNoteMaster = new DebitNoteMasterSP();
                
                    if (!isEditMode)
                    {
                        if (txtVoucherNo.Text.Trim() != string.Empty)
                        {

                            if (!isAutomatic)
                            {
                                strInvoiceNo = txtVoucherNo.Text.Trim();
                                if (!spDebitNoteMaster.DebitNoteVoucherCheckExistance(strInvoiceNo, decDebitNoteVoucherTypeId, 0))
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
                        if (txtVoucherNo.Text.Trim() != string.Empty)
                        {
                            if (!isAutomatic)
                            {
                                strInvoiceNo = txtVoucherNo.Text.Trim();
                                if (!spDebitNoteMaster.DebitNoteVoucherCheckExistance(strInvoiceNo, decDebitNoteVoucherTypeId, decDebitNoteMasterIdForEdit))
                                {
                                    EditFunction(decDebitNoteMasterIdForEdit);
                                }
                                else
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                            }
                            else
                            {
                                EditFunction(decDebitNoteMasterIdForEdit);
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
                formMDI.infoError.ErrorString = "DRNT21:" + ex.Message;
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
                if (!dgvDebitNote.Rows[inA].Cells["dgvtxtAmount"].ReadOnly)
                {
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.VoucherTypeId = decDebitNoteVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            infoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }


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
                    infoLedgerPosting.VoucherTypeId = decDebitNoteVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
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
                        if (Convert.ToDecimal(dgvDebitNote.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
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
                formMDI.infoError.ErrorString = "DRNT22:" + ex.Message;
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

                if (!dgvDebitNote.Rows[inA].Cells["dgvtxtAmount"].ReadOnly)
                {
                    infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.VoucherTypeId = decDebitNoteVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            infoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        infoLedgerPosting.ChequeNo = string.Empty;
                        infoLedgerPosting.ChequeDate = DateTime.Now;
                    }


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
                    infoLedgerPosting.VoucherTypeId = decDebitNoteVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.DetailsId = decDetailsId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();

                    if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        infoLedgerPosting.ChequeNo = dgvDebitNote.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvDebitNote.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
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
                        if (Convert.ToDecimal(dgvDebitNote.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
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
                formMDI.infoError.ErrorString = "DRNT23:" + ex.Message;
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
                InfopartyBalance.CreditPeriod = 0;
                InfopartyBalance.Date = dtpVoucherDate.Value;
                InfopartyBalance.LedgerId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString());
                InfopartyBalance.ReferenceType = dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString();
                if (dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "New" || dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "OnAccount")
                {
                    InfopartyBalance.AgainstInvoiceNo = "0";// dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0";//dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0;// Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decDebitNoteVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decDebitNoteVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                if (dgvDebitNote.Rows[inRowIndex].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
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
                formMDI.infoError.ErrorString = "DRNT24:" + ex.Message;
            }
        }

        /// <summary>
        /// function to delete voucher
        /// </summary>
        /// <param name="decDebitNoteMasterId"></param>
        public void DeleteFunction(decimal decDebitNoteMasterId)
        {
            try
            {
                DebitNoteMasterSP spDebitNoteMaster = new DebitNoteMasterSP();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();

                if (!spPartyBalance.PartyBalanceCheckReference(decDebitNoteVoucherTypeId, strVoucherNo))
                {
                    spDebitNoteMaster.DebitNoteVoucherDelete(decDebitNoteMasterId, decDebitNoteVoucherTypeId, strVoucherNo);

                    Messages.DeletedMessage();
                    if (frmDebitNoteRegisterObj != null)
                    {
                        this.Close();
                        frmDebitNoteRegisterObj.Enabled = true;
                    }
                    else if (frmDebitNoteReportObj != null)
                    {
                        this.Close();
                        frmDebitNoteReportObj.Enabled = true;
                    }
                    else if (objVoucherSearch != null)
                    {
                        this.Close();
                        objVoucherSearch.GridFill();
                    }
                    else if (frmLedgerDetailsObj != null)
                    {
                        this.Close();
                    }
                    else
                    {
                        Clear();
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
                formMDI.infoError.ErrorString = "DRNT25:" + ex.Message;
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
                DebitNoteMasterSP SpDebitNoteMaster = new DebitNoteMasterSP();
                DataSet dsDebitNoteVoucher = SpDebitNoteMaster.DebitNotePrinting(decMasterId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.DebitNotePrinting(dsDebitNoteVoucher);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT26:" + ex.Message;
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
                foreach (DataGridViewRow dRow in dgvDebitNote.Rows)
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
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decDebitNoteVoucherTypeId);
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
                formMDI.infoError.ErrorString = "DRNT27:" + ex.Message;
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
                    DataGridViewComboBoxCell dgvccAccountLedger = (DataGridViewComboBoxCell)dgvDebitNote[dgvDebitNote.Columns["dgvcmbAccountLedger"].Index, dgvDebitNote.CurrentRow.Index];
                    dgvccAccountLedger.DataSource = dtbl;
                    dgvccAccountLedger.ValueMember = "ledgerId";
                    dgvccAccountLedger.DisplayMember = "ledgerName";
                    dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value = decLedgerId;
                }
                this.Enabled = true;
                this.BringToFront();
                dgvDebitNote.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT28:" + ex.Message;
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
                dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value = decId;
                dgvDebitNote.Focus();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT29:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the voucher to edit or delete while calling from the DebitNote report
        /// </summary>
        /// <param name="frmDebitNoteReport"></param>
        /// <param name="decDebitNoteMasterId"></param>
        public void CallFromDebitNoteReport(frmDebitNoteReport frmDebitNoteReport, decimal decDebitNoteMasterId)
        {
            try
            {
                base.Show();
                isEditMode = true;
                btnDelete.Enabled = true;
                frmDebitNoteReportObj = frmDebitNoteReport;
                frmDebitNoteReportObj.Enabled = false;
                decDebitNoteMasterIdForEdit = decDebitNoteMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT30:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to save the debitnote voucher
        /// </summary>
        public void Save()
        {
            try
            {
                decimal decTotalDebit = 0;
                decimal decTotalCredit = 0;

                decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());

                DebitNoteMasterSP spDebitNoteMaster = new DebitNoteMasterSP();
                DebitNoteDetailsSP spDebitNoteDetails = new DebitNoteDetailsSP();
                DebitNoteMasterInfo infoDebitNoteMaster = new DebitNoteMasterInfo();
                DebitNoteDetailsInfo infoDebitNoteDetails = new DebitNoteDetailsInfo();
                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();

                infoDebitNoteMaster.VoucherNo = strVoucherNo;
                infoDebitNoteMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                infoDebitNoteMaster.SuffixPrefixId = decDebitNoteSuffixPrefixId;
                infoDebitNoteMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoDebitNoteMaster.Narration = txtNarration.Text;
                infoDebitNoteMaster.UserId = PublicVariables._decCurrentUserId;
                infoDebitNoteMaster.VoucherTypeId = decDebitNoteVoucherTypeId;
                infoDebitNoteMaster.FinancialYearId = Convert.ToDecimal(PublicVariables._decCurrentFinancialYearId.ToString());
                infoDebitNoteMaster.Extra1 = string.Empty;
                infoDebitNoteMaster.Extra2 = string.Empty;


                infoDebitNoteMaster.TotalAmount = decTotalDebit;
                decimal decJDebitNoteMasterId = spDebitNoteMaster.DebitNoteMasterAdd(infoDebitNoteMaster);

                /*******************DebitNoteDetailsAdd and LedgerPosting*************************/
                infoDebitNoteDetails.DebitNoteMasterId = decJDebitNoteMasterId;
                infoDebitNoteDetails.ExtraDate = DateTime.Now;
                infoDebitNoteDetails.Extra1 = string.Empty;
                infoDebitNoteDetails.Extra2 = string.Empty;


                decimal decLedgerId = 0;
                decimal decDebit = 0;
                decimal decCredit = 0;
                int inRowCount = dgvDebitNote.RowCount;
                for (int inI = 0; inI < inRowCount - 1; inI++)
                {
                    if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        infoDebitNoteDetails.LedgerId = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                        decLedgerId = infoDebitNoteDetails.LedgerId;
                    }
                    if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                    {
                        if (dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            //--------Currency conversion--------------//
                            decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));
                            decAmount = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                            decConvertRate = decAmount * decSelectedCurrencyRate;
                            //===========================================//
                            if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                            {
                                infoDebitNoteDetails.Debit = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoDebitNoteDetails.Credit = 0;
                                decDebit = decConvertRate;
                                decCredit = infoDebitNoteDetails.Credit;
                            }
                            else
                            {
                                infoDebitNoteDetails.Credit = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoDebitNoteDetails.Debit = 0;
                                decDebit = infoDebitNoteDetails.Debit;
                                decCredit = decConvertRate;
                            }
                        }
                        infoDebitNoteDetails.ExchangeRateId = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString());
                        if (dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                        {
                            infoDebitNoteDetails.ChequeNo = dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                        }
                        else
                        {
                            infoDebitNoteDetails.ChequeNo = string.Empty;
                        }
                        if (dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            infoDebitNoteDetails.ChequeDate = Convert.ToDateTime(dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value);
                        }
                        else
                        {
                            infoDebitNoteDetails.ChequeDate = DateTime.Now;
                        }
                        decimal decDebitNoteDetailsId = spDebitNoteDetails.DebitNoteDetailsAdd(infoDebitNoteDetails);

                        if (decDebitNoteDetailsId != 0)
                        {
                            PartyBalanceAddOrEdit(inI);
                            LedgerPosting(decLedgerId, decCredit, decDebit, decDebitNoteDetailsId, inI);
                        }
                    }

                }

                Messages.SavedMessage();


                //----------------If print after save is enable-----------------------//
                SettingsSP spSettings = new SettingsSP();
                if (cbxPrintAfterSave.Checked == true)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(decJDebitNoteMasterId);
                    }
                    else
                    {
                        Print(decJDebitNoteMasterId);
                    }
                }

                //===================================================================//
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT31:" + ex.Message;
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
                    InfopartyBalance.AgainstInvoiceNo = "0";//dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0";// dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0;// Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decDebitNoteVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decDebitNoteVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                if (dgvDebitNote.Rows[inRowIndex].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
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
                formMDI.infoError.ErrorString = "DRNT32:" + ex.Message;
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
                    if (dgvDebitNote.Rows[inRowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() == dtblPartyBalance.Rows[inJ]["LedgerId"].ToString())
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
                formMDI.infoError.ErrorString = "DRNT33:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to edit the debitnote voucher
        /// </summary>
        /// <param name="decDebitNoteMasterId"></param>
        public void Edit(decimal decDebitNoteMasterId)
        {
            try
            {

                DebitNoteMasterSP spDebitNoteMaster = new DebitNoteMasterSP();
                DebitNoteDetailsSP spDebitNoteDetails = new DebitNoteDetailsSP();
                DebitNoteMasterInfo infoDebitNoteMaster = new DebitNoteMasterInfo();
                DebitNoteDetailsInfo infoDebitNoteDetails = new DebitNoteDetailsInfo();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();

                /*****************Update in DebitNoteMaster table *************/

                decimal decTotalDebit = 0;
                decimal decTotalCredit = 0;

                infoDebitNoteMaster.DebitNoteMasterId = decDebitNoteMasterId;
                infoDebitNoteMaster.VoucherNo = strVoucherNo;
                infoDebitNoteMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                infoDebitNoteMaster.SuffixPrefixId = decDebitNoteSuffixPrefixId;
                infoDebitNoteMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoDebitNoteMaster.Narration = txtNarration.Text;
                infoDebitNoteMaster.UserId = PublicVariables._decCurrentUserId;
                infoDebitNoteMaster.VoucherTypeId = decDebitNoteVoucherTypeId;
                infoDebitNoteMaster.FinancialYearId = Convert.ToDecimal(PublicVariables._decCurrentFinancialYearId.ToString());
                infoDebitNoteMaster.ExtraDate = DateTime.Now;
                infoDebitNoteMaster.Extra1 = string.Empty;
                infoDebitNoteMaster.Extra2 = string.Empty;

                decTotalDebit = Convert.ToDecimal(txtDebitTotal.Text.Trim());
                decTotalCredit = Convert.ToDecimal(txtCreditTotal.Text.Trim());
                infoDebitNoteMaster.TotalAmount = decTotalDebit;
                decimal decEffectRow = spDebitNoteMaster.DebitNoteMasterEdit(infoDebitNoteMaster);

                /**********************DebitNoteDetails Edit********************/
                if (decEffectRow > 0)
                {
                    infoDebitNoteDetails.DebitNoteMasterId = decDebitNoteMasterId;
                    infoDebitNoteDetails.ExtraDate = DateTime.Now;
                    infoDebitNoteDetails.Extra1 = string.Empty;
                    infoDebitNoteDetails.Extra2 = string.Empty;

                    //-----------to delete details, LedgerPosting and bankReconciliation of removed rows--------------// 
                    LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                    BankReconciliationSP spBankReconciliation = new BankReconciliationSP();

                    foreach (object obj in arrlstOfRemove)
                    {
                        string str = Convert.ToString(obj);
                        spDebitNoteDetails.DebitNoteDetailsDelete(Convert.ToDecimal(str));
                        spLedgerPosting.LedgerPostDeleteByDetailsId(Convert.ToDecimal(str), strVoucherNo, decDebitNoteVoucherTypeId);
                    }
                    spLedgerPosting.LedgerPostingDeleteByVoucherNoVoucherTypeIdAndLedgerId(strVoucherNo, decDebitNoteVoucherTypeId, 12);
                    //=============================================================================================//

                    decimal decLedgerId = 0;
                    decimal decDebit = 0;
                    decimal decCredit = 0;
                    decimal decDebitNoteDetailsId = 0;
                    int inRowCount = dgvDebitNote.RowCount;
                    for (int inI = 0; inI < inRowCount; inI++)
                    {
                        if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            infoDebitNoteDetails.LedgerId = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                            decLedgerId = infoDebitNoteDetails.LedgerId;
                        }
                        if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                        {
                            //------------------Currency conversion------------------//
                            decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value));
                            decAmount = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                            decConvertRate = decAmount * decSelectedCurrencyRate;
                            //======================================================//

                            if (dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value.ToString() == "Dr")
                            {
                                infoDebitNoteDetails.Debit = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoDebitNoteDetails.Credit = 0;

                                decDebit = decConvertRate;
                                decCredit = infoDebitNoteDetails.Credit;
                            }
                            else
                            {
                                infoDebitNoteDetails.Credit = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                                infoDebitNoteDetails.Debit = 0;
                                decDebit = infoDebitNoteDetails.Debit;
                                decCredit = decConvertRate;
                            }
                            infoDebitNoteDetails.ExchangeRateId = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString());
                            if (dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                            {
                                infoDebitNoteDetails.ChequeNo = dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                            }
                            else
                            {
                                infoDebitNoteDetails.ChequeNo = string.Empty;
                            }
                            if (dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                            {
                                infoDebitNoteDetails.ChequeDate = Convert.ToDateTime(dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value);
                            }
                            else
                            {
                                infoDebitNoteDetails.ChequeDate = DateTime.Now;
                            }
                            if (dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString() != string.Empty)
                            {
                                infoDebitNoteDetails.DebitNoteDetailsId = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value.ToString());
                                spDebitNoteDetails.DebitNoteDetailsEdit(infoDebitNoteDetails);
                                PartyBalanceAddOrEdit(inI);
                                decDebitNoteDetailsId = infoDebitNoteDetails.DebitNoteDetailsId;
                                decimal decLedgerPostId = Convert.ToDecimal(dgvDebitNote.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                LedgerPostingEdit(decLedgerPostId, decLedgerId, decCredit, decDebit, decDebitNoteDetailsId, inI);

                            }
                            else
                            {
                                decDebitNoteDetailsId = spDebitNoteDetails.DebitNoteDetailsAdd(infoDebitNoteDetails);
                                PartyBalanceAddOrEdit(inI);
                                LedgerPosting(decLedgerId, decCredit, decDebit, decDebitNoteDetailsId, inI);
                            }

                        }

                    }
                    Messages.UpdatedMessage();
                    

                }
                //----------------If print after save is enable-----------------------//
                SettingsSP spSettings = new SettingsSP();
                if (cbxPrintAfterSave.Checked == true)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(infoDebitNoteMaster.DebitNoteMasterId);
                    }
                    else
                    {
                        Print(infoDebitNoteMaster.DebitNoteMasterId);
                    }
                }

                //===================================================================//

                if (frmDebitNoteRegisterObj != null)
                {
                    this.Close();
                    frmDebitNoteRegisterObj.Enabled = true;
                }
                else if (frmDebitNoteReportObj != null)
                {
                    this.Close();
                    frmDebitNoteReportObj.Enabled = true;
                }
                else
                {
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT34:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT35:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from BillAllocation form
        /// </summary>
        /// <param name="frmBillallocation"></param>
        /// <param name="decdebitMasterId"></param>
        public void CallFromBillAllocation(frmBillallocation frmBillallocation, decimal decdebitMasterId)
        {
            try
            {
                frmBillallocation.Enabled = false;
                base.Show();
                isEditMode = true;
                btnDelete.Enabled = true;
                frmBillallocationObj = frmBillallocation;
                decDebitNoteMasterIdForEdit = decdebitMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT36:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to fill the fields for edit or delete
        /// </summary>
        public void FillFunction()
        {
            try
            {
                DebitNoteMasterInfo infoDebitNoteMaster = new DebitNoteMasterInfo();
                DebitNoteMasterSP spDebitNoteMaster = new DebitNoteMasterSP();
                infoDebitNoteMaster = spDebitNoteMaster.DebitNoteMasterView(decDebitNoteMasterIdForEdit);

                txtVoucherNo.ReadOnly = false;
                strVoucherNo = infoDebitNoteMaster.VoucherNo;

                strInvoiceNo = infoDebitNoteMaster.InvoiceNo;
                txtVoucherNo.Text = strInvoiceNo;
                decDebitNoteSuffixPrefixId = infoDebitNoteMaster.SuffixPrefixId;
                decDebitNoteVoucherTypeId = infoDebitNoteMaster.VoucherTypeId;
                dtpVoucherDate.Value = infoDebitNoteMaster.Date;
                txtNarration.Text = infoDebitNoteMaster.Narration;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decDebitNoteVoucherTypeId);
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
                DebitNoteDetailsSP spDebitNoteDetailsSp = new DebitNoteDetailsSP();
                dtbl = spDebitNoteDetailsSp.DebitNoteDetailsViewByMasterId(decDebitNoteMasterIdForEdit);

                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                for (int inI = 0; inI < dtbl.Rows.Count; inI++)
                {
                    dgvDebitNote.Rows.Add();
                    dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value = Convert.ToDecimal(dtbl.Rows[inI]["ledgerId"].ToString());

                    if (Convert.ToDecimal(dtbl.Rows[inI]["debit"].ToString()) == 0)
                    {
                        dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value = "Cr";
                        dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value = Convert.ToDecimal(dtbl.Rows[inI]["credit"].ToString());
                    }
                    else
                    {
                        dgvDebitNote.Rows[inI].Cells["dgvcmbDrOrCr"].Value = "Dr";
                        dgvDebitNote.Rows[inI].Cells["dgvtxtAmount"].Value = Convert.ToDecimal(dtbl.Rows[inI]["debit"].ToString());
                    }
                    dgvDebitNote.Rows[inI].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(dtbl.Rows[inI]["exchangeRateId"].ToString());
                    if (dtbl.Rows[inI]["chequeNo"].ToString() != string.Empty)
                    {
                        dgvDebitNote.Rows[inI].Cells["dgvtxtChequeNo"].Value = dtbl.Rows[inI]["chequeNo"].ToString();
                        dgvDebitNote.Rows[inI].Cells["dgvtxtChequeDate"].Value = (Convert.ToDateTime(dtbl.Rows[inI]["chequeDate"].ToString())).ToString();
                    }
                    dgvDebitNote.Rows[inI].Cells["dgvtxtDetailsId"].Value = dtbl.Rows[inI]["DebitNoteDetailsId"].ToString();

                    decimal decDetailsId1 = Convert.ToDecimal(dtbl.Rows[inI]["DebitNoteDetailsId"].ToString());
                    decimal decLedgerPostingId = spLedgerPosting.LedgerPostingIdFromDetailsId(decDetailsId1, strVoucherNo, decDebitNoteVoucherTypeId);
                    dgvDebitNote.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value = decLedgerPostingId.ToString();
                    btnSave.Text = "Update";

                }

                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                DataTable dtbl1 = new DataTable();
                dtbl1 = SpPartyBalance.PartyBalanceViewByVoucherNoAndVoucherType(decDebitNoteVoucherTypeId, strVoucherNo, infoDebitNoteMaster.Date);

                dtblPartyBalance = dtbl1;

                dgvDebitNote.ClearSelection();
                txtDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT37:" + ex.Message;
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
                base.Show();
                frmDayBook.Enabled = false;
                isEditMode = true;
                btnDelete.Enabled = true;
                this.frmDayBookObj = frmDayBook;
                decDebitNoteMasterIdForEdit = decMasterId;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT38:" + ex.Message;
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
                base.Show();
                frmAgeing.Enabled = false;
                isEditMode = true;
                btnDelete.Enabled = true;
                this.frmAgeingObj = frmAgeing;
                decDebitNoteMasterIdForEdit = decMasterId;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT39:" + ex.Message;
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
                isEditMode = true;
                btnDelete.Enabled = true;
                frmLedgerDetailsObj = frmLedgerDetails;
                frmLedgerDetailsObj.Enabled = false;
                decDebitNoteMasterIdForEdit = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT40:" + ex.Message;
            }

        }
        #endregion

        #region Events

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDebitNote_Load(object sender, EventArgs e)
        {
            try
            {

                AccountLedgerComboFill();
                DrOrCrComboFill();
                Clear();
                CurrencyComboFill();

               
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT41:" + ex.Message;
            }
        }
        /// <summary>
        /// On clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT42:" + ex.Message;
            }
        }

        /// <summary>
        /// On close button click
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
                formMDI.infoError.ErrorString = "DRNT43:" + ex.Message;
            }
        }

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
                formMDI.infoError.ErrorString = "DRNT44:" + ex.Message;
            }
        }

        /// <summary>
        ///  Date validation while leaving txtDate
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
                formMDI.infoError.ErrorString = "DRNT45:" + ex.Message;
            }
        }

        /// <summary>
        /// On CellEnter of dgvCreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvDebitNote.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvDebitNote.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvDebitNote.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT46:" + ex.Message;
            }
        }

        /// <summary>
        /// On cellvalueChanged of dgvCreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    DebitAndCreditTotal();

                    if (dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        if (dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value == null || dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value.ToString() == string.Empty)
                        {
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1);//decExchangeRateId;
                        }
                    }


                    
                    //==========================================================================================//

                    //-----------To make amount readonly when party is selected as ledger------------------------------//
                    if (dgvDebitNote.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbAccountLedger")
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

                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;
                            //reset
                            decUpdatingLedgerForPartyremove = 0;
                            inUpdatingRowIndexForPartyRemove = -1;
                        }
                        /*************************************************************************/
                        //-----------To make amount readonly when party is selected as ledger------------------------------//
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        if (spAccountLedger.AccountGroupIdCheck(dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtAmount"].ReadOnly = true;

                        }
                        else
                        {
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtAmount"].ReadOnly = false;
                        }
                    }

                    //========================================================================================//

                    if (dgvDebitNote.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbDrOrCr")
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
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;

                            //reset
                            decUpdatingLedgerForPartyremove = 0;
                            inUpdatingRowIndexForPartyRemove = -1;
                        }
                        /*************************************************************************/
                    }

                    //-----------------------------------Chequedate validation----------------------------------//
                    DateValidation obj = new DateValidation();
                    TextBox txtDate1 = new TextBox();
                    if (dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value != null && dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                    {
                        txtDate1.Text = dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value.ToString();
                        bool isInvalid = obj.DateValidationFunction(txtDate1);
                        if (!isInvalid)
                        {
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            dgvDebitNote.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = txtDate1.Text;
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
                formMDI.infoError.ErrorString = "DRNT47:" + ex.Message;
            }
        }

        /// <summary>
        /// On cellBeginEdit of dgvCreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                inUpdatingRowIndexForPartyRemove = -1;
                decUpdatingLedgerForPartyremove = 0;
                DataTable dtbl = new DataTable();
                AccountLedgerSP SpAccountLedger = new AccountLedgerSP();

                if (dgvDebitNote.CurrentCell.ColumnIndex == dgvDebitNote.Columns["dgvcmbAccountLedger"].Index)
                {
                    dtbl = SpAccountLedger.AccountLedgerViewAll();
                    DataRow dr = dtbl.NewRow();
                    dr[0] = 0;
                    dr[2] = string.Empty;
                    dtbl.Rows.InsertAt(dr, 0);
                    if (dtbl.Rows.Count > 0)
                    {

                        if (dgvDebitNote.RowCount > 1)
                        {
                            int inGridRowCount = dgvDebitNote.RowCount;
                            for (int inI = 0; inI < inGridRowCount - 1; inI++)
                            {
                                if (inI != e.RowIndex)
                                {
                                    int inTableRowcount = dtbl.Rows.Count;
                                    for (int inJ = 0; inJ < inTableRowcount; inJ++)
                                    {
                                        if (dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                        {
                                            if (dtbl.Rows[inJ]["ledgerId"].ToString() == dgvDebitNote.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString())
                                            {
                                                dtbl.Rows.RemoveAt(inJ);
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        DataGridViewComboBoxCell dgvccVoucherType = (DataGridViewComboBoxCell)dgvDebitNote[dgvDebitNote.Columns["dgvcmbAccountLedger"].Index, e.RowIndex];
                        dgvccVoucherType.DataSource = dtbl;
                        dgvccVoucherType.ValueMember = "ledgerId";
                        dgvccVoucherType.DisplayMember = "ledgerName";
                    }
                }

                if (dgvDebitNote.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbAccountLedger")
                {
                    if (dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        if (spAccountLedger.AccountGroupIdCheck(dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            inUpdatingRowIndexForPartyRemove = e.RowIndex;
                            decUpdatingLedgerForPartyremove = Convert.ToDecimal(dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                    }
                }
                if (dgvDebitNote.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbDrOrCr")
                {
                    if (dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        if (spAccountLedger.AccountGroupIdCheck(dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            inUpdatingRowIndexForPartyRemove = e.RowIndex;
                            decUpdatingLedgerForPartyremove = Convert.ToDecimal(dgvDebitNote.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT48:" + ex.Message;
            }
        }

        /// <summary>
        /// For committing edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvDebitNote.IsCurrentCellDirty)
                {
                    dgvDebitNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT49:" + ex.Message;
            }
        }

        /// <summary>
        /// for handling dataerror 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
                {
                    object value = dgvDebitNote.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (!((DataGridViewComboBoxColumn)dgvDebitNote.Columns[e.ColumnIndex]).Items.Contains(value))
                    {
                        e.ThrowException = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT50:" + ex.Message;
            }
        }

        /// <summary>
        /// Calling the keypress event for validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridViewTextBoxEditingControl txt = e.Control as DataGridViewTextBoxEditingControl;
                if (dgvDebitNote.CurrentCell.ColumnIndex == dgvDebitNote.Columns["dgvtxtAmount"].Index)
                {
                    txt.KeyPress += keypressevent;
                }
                else if (dgvDebitNote.CurrentCell.ColumnIndex == dgvDebitNote.Columns["dgvtxtChequeNo"].Index)
                {
                    txt.KeyPress += keypresseventEnable;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT51:" + ex.Message;
            }
        }

        /// <summary>
        /// While adding new row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dgvDebitNote_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SlNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT52:" + ex.Message;
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
                if (dgvDebitNote.RowCount > 1)
                {
                    if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        RemoveRow();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT53:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT54:" + ex.Message;
            }
        }

        /// <summary>
        /// On Delete button click
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
                            DeleteFunction(decDebitNoteMasterIdForEdit);
                        }
                        dgvDebitNote.Focus();
                    }
                    else
                    {
                        DeleteFunction(decDebitNoteMasterIdForEdit);
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT55:" + ex.Message;
            }
        }

        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDebitNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmDebitNoteRegisterObj != null)
                {
                    frmDebitNoteRegisterObj.Enabled = true;
                    frmDebitNoteRegisterObj.SearchRegister();
                }
                else if (frmDebitNoteReportObj != null)
                {
                    frmDebitNoteReportObj.Enabled = true;
                    frmDebitNoteReportObj.Search();
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
                formMDI.infoError.ErrorString = "DRNT56:" + ex.Message;
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
        private void frmDebitNote_KeyDown(object sender, KeyEventArgs e)
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
                if (dgvDebitNote.RowCount > 1)
                {
                    if (dgvDebitNote.CurrentCell == dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"])
                    {
                        //-----------------------for ledger creation----------------------------------//
                        if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)//Ledger creation
                        {
                            frmAccountLedger accounLedgerObj = new frmAccountLedger();
                            accounLedgerObj.MdiParent = formMDI.MDIObj;
                            if (dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                            {
                                string strLedgerName = dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString();
                                accounLedgerObj.CallFromDebitNoteVoucher(this, strLedgerName);
                            }
                            else
                            {
                                string strLedgerName = string.Empty;
                                accounLedgerObj.CallFromDebitNoteVoucher(this, strLedgerName);
                            }

                        }
                        //========================================================================//

                        //--------------------For ledger Popup------------------------------------//

                        if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)//Ledger popup
                        {
                            frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                            frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                            btnSave.Focus();
                            dgvDebitNote.Focus();
                            if (dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                            {
                                btnSave.Focus();
                                dgvDebitNote.Focus();
                                decLedgerIdForPopUp = Convert.ToDecimal(dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString());
                                frmLedgerPopupObj.CallFromDebitNoteVoucher(this, decLedgerIdForPopUp, string.Empty);

                            }

                        }
                        //========================================================================// 
                    }
                    if (dgvDebitNote.CurrentCell == dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"])
                    {
                        if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                        {
                            frmCurrencyObj = new frmCurrencyDetails();
                            frmCurrencyObj.MdiParent = formMDI.MDIObj;
                            if (dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].Value != null && dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString() != string.Empty)
                            {
                                frmCurrencyObj.CallFromDebitNoteVoucher(this, Convert.ToDecimal(dgvDebitNote.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString()));
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
                formMDI.infoError.ErrorString = "DRNT57:" + ex.Message;
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
                    txtDate.SelectionStart = txtDate.TextLength;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT58:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key and backspace navigation of txtDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvDebitNote.Focus();
                }
                if (txtDate.Text == string.Empty || txtDate.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        if (txtVoucherNo.ReadOnly == true)
                        {

                        }
                        else
                        {
                            txtVoucherNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT59:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key and backspace navigation of dgvDebitNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDebitNote_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int inDgvDebitNoteRowCount = dgvDebitNote.Rows.Count;
                if (e.KeyCode == Keys.Enter)
                {

                    if (dgvDebitNote.CurrentCell == dgvDebitNote.Rows[inDgvDebitNoteRowCount - 1].Cells["dgvtxtChequeDate"])
                    {
                        txtNarration.Focus();
                        txtNarration.SelectionStart = txtNarration.TextLength;
                        dgvDebitNote.ClearSelection();
                    }

                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvDebitNote.CurrentCell == dgvDebitNote.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = txtDate.TextLength;
                        dgvDebitNote.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DRNT60:" + ex.Message;
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
                        if (dgvDebitNote.RowCount > 0)
                        {
                            dgvDebitNote.Focus();
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
                formMDI.infoError.ErrorString = "DRNT61:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key navigation of txtNarration
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
                formMDI.infoError.ErrorString = "DRNT62:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT63:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT64:" + ex.Message;
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
                formMDI.infoError.ErrorString = "DRNT65:" + ex.Message;
            }
        }

        #endregion 

        private void dgvDebitNote_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvDebitNote.CurrentCell.ColumnIndex == dgvDebitNote.Columns["dgvbtnAgainst"].Index)
                    {
                        AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
                        if (dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            if (dgvDebitNote.CurrentRow.Cells["dgvcmbDrOrCr"].Value != null && dgvDebitNote.CurrentRow.Cells["dgvcmbDrOrCr"].Value.ToString() != string.Empty)
                            {

                                if (SpAccountLedger.AccountGroupIdCheck(dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                                {
                                    frmPartyBalanceObj = new frmPartyBalance();
                                    frmPartyBalanceObj.MdiParent = formMDI.MDIObj;
                                    decimal decLedgerId = Convert.ToDecimal(dgvDebitNote.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString());

                                    string strDebitOrCredit = dgvDebitNote.CurrentRow.Cells["dgvcmbDrOrCr"].Value.ToString();
                                    if (!isAutomatic)
                                    {
                                        frmPartyBalanceObj.CallFromDebitNote(this, decLedgerId, dtblPartyBalance, strDebitOrCredit, decDebitNoteVoucherTypeId, txtVoucherNo.Text, Convert.ToDateTime(txtDate.Text), arrlstOfDeletedPartyBalanceRow);
                                    }
                                    else
                                    {
                                        frmPartyBalanceObj.CallFromDebitNote(this, decLedgerId, dtblPartyBalance, strDebitOrCredit, decDebitNoteVoucherTypeId, strVoucherNo, Convert.ToDateTime(txtDate.Text), arrlstOfDeletedPartyBalanceRow);
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
                formMDI.infoError.ErrorString = "DRNT66:" + ex.Message;
            }
        }

     

    }
}
