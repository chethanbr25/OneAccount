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
    public partial class frmPdcReceivable : Form
    {
        #region Public Variables
        bool isAutomatic = false;       //To checking vocher no generation auto or not
        decimal decPDCReceivableVoucherTypeId = 0;
        string strAccountLedger;
        SettingsSP spSettings = new SettingsSP();
        string strAccNameText = string.Empty;//To keep the selected Account Name
        bool isWorkLedgerIndexChange = true;
        string strVoucherNo = string.Empty;
        decimal decLedgerIdForPartyBalance; // To keep ledger Id
        decimal decSufixprefixPdcReceivableID = 0;
        string strPrefix = string.Empty;
        string strOldLedgerId = string.Empty;
        string strSuffix = string.Empty;
        string strtableName = "PdcReceivableMaster";
        string strInvoiceNo = string.Empty;
        string strBankNameText = string.Empty;
        ArrayList arrlstOfDeletedPartyBalanceRow;
        decimal decPDCReceivableEditId = 0;
        frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();//to use in call from ledger popup function
        DataTable dtblPartyBalance = new DataTable(); // To pass values to party balance
        bool isInEditMode = false; // Tp decide whether is edit mode of not
        string strBankID = string.Empty;
        int inKeyPrsCount = 0;
        frmPartyBalance frmPartyBalanceObj = new frmPartyBalance();//To use in call from PartyBalance class
        frmVoucherSearch objVoucherSearch = null;
        frmDayBook frmDayBookObj = null;//to use in call from frmDayBook
        frmAgeingReport frmAgeingObj = null;//to use in call from frmDayBook
        frmChequeReport frmChequeReportObj = null; // Used to call from frmChequeReport
        string strOldBankLedgerId = string.Empty;//To keep the  bank ledgerId 
        frmPDCReceivableRegister PDCReceivableRegisterObj = null;
        frmPDCRecievableReport PDCReceivableReportObj = null;
        frmBillallocation frmBillallocationObj = null;
        frmLedgerDetails frmLedgerDetailsObj;
        #endregion

        #region Functions
        /// <summary>
        /// Create an Instance for frmPdcReceivable Class
        /// </summary>
        public frmPdcReceivable()
        {
            InitializeComponent();
        }
        /// <summary>
        /// CashorBank Account ComboFill
        /// </summary>
        public void cmbBankAccountFill()
        {
            try
            {
                PDCPayableMasterSP sppdcpayble = new PDCPayableMasterSP();
                cmbBank.DataSource = null;
                DataTable dtblBank = sppdcpayble.BankAccountComboFill();
                cmbBank.DataSource = dtblBank;
                cmbBank.DisplayMember = "ledgerName";
                cmbBank.ValueMember = "ledgerId";
                cmbBank.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR1:" + ex.Message;
            }
        }
        /// <summary>
        /// Account Ledger Combofill function
        /// </summary>
        public void cmbAccountNameFill()
        {
            try
            {
                isWorkLedgerIndexChange = false;
                AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
                cmbAccountLedger.DataSource = null;
                DataTable dtblParty = SpAccountLedger.AccountLedgerViewAll();
                cmbAccountLedger.DataSource = dtblParty;
                cmbAccountLedger.DisplayMember = "ledgerName";
                cmbAccountLedger.ValueMember = "ledgerId";
                isWorkLedgerIndexChange = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVoucherSearch to view details and for updation 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallThisFormFromVoucherSearch(frmVoucherSearch frm, decimal decId)
        {
            try
            {
                this.objVoucherSearch = frm;
                decPDCReceivableEditId = decId;
                btnClear.Text = "New";
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                FillFunction();
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR3:" + ex.Message;
            }
        }
        /// <summary>
        /// Cleare Function, The form will be reset here
        /// </summary>
        public void ClearFunction()
        {
            try
            {
                CurrencySP SpCurrency = new CurrencySP();
                CurrencyInfo InfoCurrency = new CurrencyInfo();
                isInEditMode = false;
                VoucherNumberGeneration();
                FinancialYearDate();
                dtpVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpcheckdate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                btnDelete.Enabled = false;
                btnAgainRef.Enabled = false;
                cmbBankAccountFill();
                cmbAccountNameFill();
                txtAmount.Clear();
                txtAmount.Enabled = true;
                txtcheckNo.Clear();
                PrintCheck();
                txtNarration.Clear();
                dtblPartyBalance.Clear();
                btnSave.Text = "Save";
                if (!txtVoucherNo.ReadOnly)
                {
                    txtVoucherNo.Focus();
                }
                else
                {
                    txtVoucherDate.Select();
                }
                if (!btnAgainRef.Enabled)
                {
                    txtAmount.ReadOnly = false;
                }
                else
                {
                    txtAmount.ReadOnly = true;
                }
                if (PDCReceivableReportObj != null)
                {
                    PDCReceivableReportObj.Close();
                }
                if (PDCReceivableRegisterObj != null)
                {
                    PDCReceivableRegisterObj.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Setting the financial year and date
        /// </summary>
        public void FinancialYearDate()
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
                txtVoucherDate.Text = dtVoucherDate.ToString("dd-MMM-yyyy");
                dtpVoucherDate.Value = Convert.ToDateTime(txtVoucherDate.Text);
                txtVoucherDate.Focus();
                txtVoucherDate.SelectAll();
                infoComapany = spCompany.CompanyView(1);
                DateTime dtcheckdate = infoComapany.CurrentDate;
                dtpcheckdate.Value = dtcheckdate;
                txtCheckDate.Text = dtcheckdate.ToString("dd-MMM-yyyy");
                dtpcheckdate.Value = Convert.ToDateTime(txtCheckDate.Text);
                txtCheckDate.Focus();
                txtCheckDate.SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR5:" + ex.Message;
            }
        }
        /// <summary>
        /// Settings to set the print after save checkbox as per settings
        /// </summary>
        public void PrintCheck()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("TickPrintAfterSave") == "Yes")
                {
                    cbxPrint.Checked = true;
                }
                else
                {
                    cbxPrint.Checked = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmLedgerPopup form to select and view Ledger
        /// </summary>
        /// <param name="frmLedgerPopup"></param>
        /// <param name="decId"></param>
        /// <param name="strComboType"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId, string strComboType) //   Ledger pop up
        {
            try
            {
                base.Show();
                this.frmLedgerPopupObj = frmLedgerPopup;
                if (strComboType == "Account Ledger")
                {
                    cmbAccountNameFill();
                    cmbAccountLedger.SelectedValue = decId;
                }
                else if (strComboType == "Bank")
                {
                    cmbBankAccountFill();
                    cmbBank.SelectedValue = decId;
                }
                frmLedgerPopupObj.Close();
                frmLedgerPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Save or edit function, Checking all invalid entrys to save or edit
        /// </summary>
        public void SaveOrEditFunction()
        {
            try
            {
                txtVoucherDate.Text = txtVoucherDate.Text.Trim();
                cmbAccountLedger.Text = cmbAccountLedger.Text.Trim();
                txtAmount.Text = txtAmount.Text.Trim();
                cmbBank.Text = cmbBank.Text.Trim();
                txtCheckDate.Text = txtCheckDate.Text.Trim();
                txtcheckNo.Text = txtcheckNo.Text.Trim();
                txtNarration.Text = txtNarration.Text.Trim();
                bool isOk = true;
                bool isAmountOk = false;
                try
                {
                    if (Convert.ToDecimal(txtAmount.Text) > 0)
                        isAmountOk = true;
                }
                catch
                {
                    txtAmount.Text = string.Empty;
                }
                if (isOk)
                {
                    if (txtVoucherNo.Text == string.Empty)
                    {
                        Messages.InformationMessage("Enter voucher no");
                        txtVoucherNo.Focus();
                    }
                    else if (txtVoucherDate.Text == string.Empty)
                    {
                        Messages.InformationMessage("Select date");
                        txtVoucherDate.Focus();
                    }
                    else if (cmbAccountLedger.SelectedValue == null)
                    {
                        Messages.InformationMessage("Select account ledger");
                        cmbAccountLedger.Focus();
                    }
                    else if (!isAmountOk)
                    {
                        Messages.InformationMessage("Select amount");
                        txtAmount.Focus();
                    }
                    else if (cmbBank.SelectedValue == null)
                    {
                        Messages.InformationMessage("Select bank ");
                        cmbBank.Focus();
                    }
                    else if (txtcheckNo.Text == string.Empty)
                    {
                        Messages.InformationMessage("Enter cheque no");
                        txtcheckNo.Focus();
                    }
                    else if (txtCheckDate.Text == string.Empty)
                    {
                        Messages.InformationMessage("Select cheque date");
                        txtCheckDate.Focus();
                    }
                    else
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            isOk = false;
                            PDCReceivableMasterSP spPdcreceivable = new PDCReceivableMasterSP();
                            if (!isInEditMode)
                            {
                                if (Messages.SaveMessage())
                                    if (!spPdcreceivable.PDCReceivableCheckExistence(txtVoucherNo.Text.Trim(), decPDCReceivableVoucherTypeId, 0))
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
                                if (Messages.UpdateMessage())
                                    if (isInEditMode && spPdcreceivable.PDCReceivableVoucherCheckRreferenceUpdating(decPDCReceivableEditId, decPDCReceivableVoucherTypeId))
                                    {
                                        MessageBox.Show("Can't update,reference exist", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        SaveFunction();
                                    }
                            }
                        }
                        if (isOk)
                        {
                            if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                            {
                                PDCReceivableMasterSP spPdcreceivable = new PDCReceivableMasterSP();
                                if (isInEditMode && spPdcreceivable.PDCReceivableVoucherCheckRreferenceUpdating(decPDCReceivableEditId, decPDCReceivableVoucherTypeId))
                                {
                                    MessageBox.Show("Can't update,reference exist", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    SaveFunction();
                                }
                            }
                            else
                            {
                                Messages.NoPrivillageMessage();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR8:" + ex.Message;
            }
        }
        /// <summary>
        /// Save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                DateValidation Objdatevalidation = new DateValidation();
                OtherDateValidationFunction ObjotherdateValidation = new OtherDateValidationFunction();
                Objdatevalidation.DateValidationFunction(txtVoucherDate);
                ObjotherdateValidation.DateValidationFunction(txtCheckDate, false);
                DataTable dtblMaster = new DataTable();
                PDCReceivableMasterSP spPdcReceivable = new PDCReceivableMasterSP();
                PDCReceivableMasterInfo InfopdcRecivable = new PDCReceivableMasterInfo();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                InfopdcRecivable.VoucherNo = strVoucherNo;
                InfopdcRecivable.InvoiceNo = txtVoucherNo.Text.Trim();
                InfopdcRecivable.Date = DateTime.Parse(txtVoucherDate.Text);
                InfopdcRecivable.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                InfopdcRecivable.Amount = decimal.Parse(txtAmount.Text);
                InfopdcRecivable.Narration = txtNarration.Text;
                if (txtcheckNo.Text != string.Empty)
                    InfopdcRecivable.ChequeNo = txtcheckNo.Text;
                else
                    InfopdcRecivable.ChequeNo = string.Empty;
                if (txtCheckDate.Text != string.Empty)
                    InfopdcRecivable.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                else
                    InfopdcRecivable.ChequeDate = DateTime.Now;
                InfopdcRecivable.UserId = PublicVariables._decCurrentUserId;
                InfopdcRecivable.VoucherTypeId = decPDCReceivableVoucherTypeId;
                if (cmbBank.SelectedValue != null && cmbBank.SelectedValue.ToString() != string.Empty)
                {
                    InfopdcRecivable.BankId = Convert.ToDecimal(cmbBank.SelectedValue.ToString());
                }
                else
                    InfopdcRecivable.BankId = 0;
                InfopdcRecivable.ExtraDate = DateTime.Now;
                InfopdcRecivable.Extra1 = string.Empty;
                InfopdcRecivable.Extra2 = string.Empty;
                if (!isInEditMode)
                {
                    decimal decIdentity = spPdcReceivable.PDCReceivableMasterAdd(InfopdcRecivable);
                    LedgerPosting();
                    PartyBalanceAddOrEdit();
                    Messages.SavedMessage();
                    if (cbxPrint.Checked)
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
                    ClearFunction();
                }
                else
                {
                    
                    decimal decIdentity = decPDCReceivableEditId;
                    InfopdcRecivable.PdcReceivableMasterId = decPDCReceivableEditId;
                    spPdcReceivable.PDCReceivableMasterEdit(InfopdcRecivable);
                    LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                    spLedgerPosting.LedgerPostingDeleteByVoucherNoVoucherTypeIdAndLedgerId(strVoucherNo, decPDCReceivableVoucherTypeId, 12);
                    spPartyBalance.PartyBalanceDeleteByVoucherTypeAndVoucherNo(decPDCReceivableVoucherTypeId, strVoucherNo);
                    PartyBalanceAddOrEdit();
                    LedgerPostingEdit(decPDCReceivableEditId);
                    Messages.UpdatedMessage();
                    if (cbxPrint.Checked)
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
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR9:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger Posting edit function
        /// </summary>
        /// <param name="decpdcMasterId"></param>
        public void LedgerPostingEdit(decimal decpdcMasterId)
        {
            PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
            DataTable dtblLedgerPostingId = new DataTable();
            LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
            LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            decimal decOldExchange = 0;
            decimal decNewExchangeRate = 0;
            decimal decNewExchangeRateId = 0;
            decimal decSelectedCurrencyRate = 0;
            decimal decAmount = 0;
            decimal decConvertRate = 0;
            string strReferenceType = string.Empty;
            decimal decOldExchangeId = 0;
            try
            {
                
                dtblLedgerPostingId = sppdcreceivable.LedgerPostingIdByPDCReceivableId(decpdcMasterId);
                decimal decledgerpostingId1 = Convert.ToDecimal(dtblLedgerPostingId.Rows[0]["ledgerPostingId"].ToString());
                decimal decLedgerPostingId2 = Convert.ToDecimal(dtblLedgerPostingId.Rows[1]["ledgerPostingId"].ToString());
                if (!btnAgainRef.Enabled)
                {
                    infoLedgerPosting.LedgerPostingId = decledgerpostingId1;
                    infoLedgerPosting.VoucherTypeId = decPDCReceivableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Debit = 0;
                    infoLedgerPosting.Credit = Convert.ToDecimal(txtAmount.Text);
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                    infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                    infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    SpLedgerPosting.LedgerPostingEdit(infoLedgerPosting);

                }
                else
                {
                    infoLedgerPosting.LedgerPostingId = decledgerpostingId1;
                    infoLedgerPosting.VoucherTypeId = decPDCReceivableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Debit = 0;
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
                    infoLedgerPosting.Credit = decConvertRate;
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                    infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                    infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    SpLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
                    
                    infoLedgerPosting.LedgerId = 12;
                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            if (dr["ReferenceType"].ToString() == "Against")
                            {
                                decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                                decNewExchangeRate = SpExchangRate.GetExchangeRateByExchangeRateId(decNewExchangeRateId);
                                decOldExchangeId = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                                decOldExchange = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchangeId);
                                decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                                decimal decForexAmount = (decAmount * decNewExchangeRate) - (decAmount * decOldExchange);
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
                                SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }

                    }
                }

                infoLedgerPosting.LedgerPostingId = decLedgerPostingId2;
                infoLedgerPosting.VoucherNo = strVoucherNo;
                infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.LedgerId = 7;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Debit = Convert.ToDecimal(txtAmount.Text);
                infoLedgerPosting.Credit = 0;
                infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                SpLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR10:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger posting Save function
        /// </summary>
        public void LedgerPosting()
        {
            LedgerPostingInfo InfoPosting = new LedgerPostingInfo();
            LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
            AccountLedgerSP SpLedger = new AccountLedgerSP();
            LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            decimal decOldExchange = 0;
            decimal decNewExchangeRate = 0;
            decimal decNewExchangeRateId = 0;
            decimal decSelectedCurrencyRate = 0;
            decimal decAmount = 0;
            decimal decConvertRate = 0;
            string strReferenceType = string.Empty;
            decimal decOldExchangeId = 0;
            try
            {
                if (!btnAgainRef.Enabled)
                {
                    infoLedgerPosting.VoucherTypeId = decPDCReceivableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Debit = 0;
                    infoLedgerPosting.Credit = Convert.ToDecimal(txtAmount.Text);
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                    infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                    infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                }
                else
                {
                    infoLedgerPosting.VoucherTypeId = decPDCReceivableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Debit = 0;
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
                    infoLedgerPosting.Credit = decConvertRate;

                    
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                    infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                    infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

                    infoLedgerPosting.LedgerId = 12;
                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            if (dr["ReferenceType"].ToString() == "Against")
                            {
                                decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                                decNewExchangeRate = SpExchangRate.GetExchangeRateByExchangeRateId(decNewExchangeRateId);
                                decOldExchangeId = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                                decOldExchange = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchangeId);
                                decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                                decimal decForexAmount = (decAmount * decNewExchangeRate) - (decAmount * decOldExchange);
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
                                SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }

                    }
                }
                infoLedgerPosting.LedgerId = 7;
                infoLedgerPosting.VoucherNo = strVoucherNo;
                infoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Debit = Convert.ToDecimal(txtAmount.Text);
                infoLedgerPosting.Credit = 0;
                infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtCheckDate.Text);
                infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR11:" + ex.Message;
            }
        }
        /// <summary>
        /// Check the status of ledger and call the Party Balance Save Function
        /// </summary>
        public void PartyBalanceAddOrEdit()
        {
            int inTableRowCount = dtblPartyBalance.Rows.Count;
            try
            {
                for (int inJ = 0; inJ < inTableRowCount; inJ++)
                {
                    if (cmbAccountLedger.SelectedValue.ToString() == dtblPartyBalance.Rows[inJ]["LedgerId"].ToString())
                    {
                        PartyBalanceAdd(inJ);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR12:" + ex.Message;
            }
        }
        /// <summary>
        /// Party balance Save function
        /// </summary>
        /// <param name="inJ"></param>
        public void PartyBalanceAdd(int inJ)
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
                    InfopartyBalance.AgainstInvoiceNo = "0"; // dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0"; // dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0; // Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decPDCReceivableVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decPDCReceivableVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                InfopartyBalance.Credit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                InfopartyBalance.Debit = 0;
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spPartyBalance.PartyBalanceAdd(InfopartyBalance);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR13:" + ex.Message;
            }
        }
        /// <summary>
        /// Party balance Update function
        /// </summary>
        /// <param name="decPartyBalanceId"></param>
        /// <param name="inJ"></param>
        public void PartyBalanceEdit(decimal decPartyBalanceId, int inJ)
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
                    InfopartyBalance.AgainstInvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decPDCReceivableVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decPDCReceivableVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                InfopartyBalance.Credit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                InfopartyBalance.Debit = 0;
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spPartyBalance.PartyBalanceEdit(InfopartyBalance);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR14:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from VoucherType Selection form
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strVoucherTypeName"></param>
        public void CallFromVoucherTypeSelection(decimal decVoucherTypeId, string strVoucherTypeName)
        {
            try
            {
                decPDCReceivableVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPDCReceivableVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPDCReceivableVoucherTypeId, dtpVoucherDate.Value);
                decSufixprefixPdcReceivableID = infoSuffixPrefix.SuffixprefixId;
                this.Text = strVoucherTypeName;
                base.Show();
                if (isAutomatic)
                {
                    txtVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
                ClearFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR15:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPartyBalance to view details and for updation
        /// </summary>
        /// <param name="frmPartyBalance"></param>
        /// <param name="decAmount"></param>
        /// <param name="dtbl"></param>
        /// <param name="arrlstOfRemovedRow"></param>
        public void CallFromPartyBalance(frmPartyBalance frmPartyBalance, decimal decAmount, DataTable dtbl, ArrayList arrlstOfRemovedRow)
        {
            try
            {
                base.Show();
                txtAmount.Text = decAmount.ToString();
                this.frmPartyBalanceObj = frmPartyBalance;
                frmPartyBalance.Close();
                frmPartyBalanceObj = null;
                dtblPartyBalance = dtbl;
                arrlstOfDeletedPartyBalanceRow = arrlstOfRemovedRow;
                txtAmount.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR16:" + ex.Message;
            }
        }
        /// <summary>
        /// Voucher no generation function based on the settings
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
                if (strVoucherNo == string.Empty)
                {
                    strVoucherNo = "0";
                }
                strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPDCReceivableVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strtableName);
                if (Convert.ToDecimal(strVoucherNo) != sppdcreceivable.PDCReceivableMaxUnderVoucherTypePlusOne(decPDCReceivableVoucherTypeId))
                {
                    strVoucherNo = sppdcreceivable.PDCReceivableMaxUnderVoucherType(decPDCReceivableVoucherTypeId).ToString();
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPDCReceivableVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strtableName);
                    if (sppdcreceivable.PDCReceivableMaxUnderVoucherType(decPDCReceivableVoucherTypeId).ToString() == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPDCReceivableVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strtableName);
                    }
                }
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPDCReceivableVoucherTypeId, dtpVoucherDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    decSufixprefixPdcReceivableID = infoSuffixPrefix.SuffixprefixId;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.Text = strInvoiceNo;
                    txtVoucherNo.ReadOnly = true;
                    txtVoucherNo.Enabled = false;
                    lblVoucherNoManualValidator.Visible = false;
                }
                else
                {
                    txtVoucherNo.ReadOnly = false;
                    txtVoucherNo.Text = string.Empty;
                    lblVoucherNoManualValidator.Visible = true;
                    strInvoiceNo = txtVoucherNo.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR17:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Account ledger combobox while return from Account ledger creation when creating new ledger 
        /// </summary>
        /// <param name="decBankaccountledgerid"></param>
        public void ReturnFromAccountLedger2(decimal decBankaccountledgerid)
        {
            try
            {
                this.Enabled = true;
                this.Activate();
                cmbBankAccountFill();
                if (decBankaccountledgerid != 0)
                {
                    cmbBank.SelectedValue = decBankaccountledgerid;
                }
                if (cmbBank.Text == string.Empty)
                {
                    cmbBank.SelectedValue = Convert.ToDecimal(strBankID);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR18:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Account ledger combobox while return from Account ledger creation when creating new ledger 
        /// </summary>
        /// <param name="decaccountledgerid"></param>
        public void ReturnFromAccountLedger(decimal decaccountledgerid)
        {
            try
            {
                this.Enabled = true;
                this.Activate();
                cmbAccountNameFill();
                if (decaccountledgerid != 0)
                {
                    cmbAccountLedger.SelectedValue = decaccountledgerid;
                }
                if (cmbAccountLedger.Text == string.Empty)
                {
                    cmbAccountLedger.SelectedValue = Convert.ToDecimal(strAccountLedger);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR19:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete Function, Checking the References and Check other forms are opend or not
        /// </summary>
        /// <param name="decPDCReceivableMasterId"></param>
        public void DeleteFunction(decimal decPDCReceivableMasterId)
        {
            try
            {
                PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                if (!spPartyBalance.PartyBalanceCheckReference(decPDCReceivableVoucherTypeId, strVoucherNo))
                {
                    sppdcreceivable.PDCReceivableDeleteMaster(decPDCReceivableMasterId, decPDCReceivableVoucherTypeId, strVoucherNo);
                    Messages.DeletedMessage();
                }
                else
                {
                    Messages.InformationMessage("Reference exist. Cannot delete");
                    txtVoucherDate.Focus();
                }
                if (PDCReceivableRegisterObj != null)
                {
                    this.Close();
                    PDCReceivableRegisterObj.Show();
                }
                else if (PDCReceivableReportObj != null)
                {
                    this.Close();
                    PDCReceivableReportObj.Show();
                }
                else if (frmLedgerDetailsObj != null)
                {
                    this.Close();
                    frmLedgerDetailsObj.Show();
                }
                else if (objVoucherSearch != null)
                {
                    this.Close();
                    objVoucherSearch.GridFill();
                }
                else
                {
                    ClearFunction();
                }
                if (frmDayBookObj != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR20:" + ex.Message;
            }
        }
        /// <summary>
        /// Print function
        /// </summary>
        /// <param name="decMasterId"></param>
        public void Print(decimal decMasterId)
        {
            try
            {
                DataSet dsPdcReceivable = new DataSet();
                PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
                dsPdcReceivable = sppdcreceivable.PDCReceivableVoucherPrinting(decMasterId, 1);
                frmReport frmreport = new frmReport();
                frmreport.MdiParent = formMDI.MDIObj;
                frmreport.PDCreceivableVoucherPrinting(dsPdcReceivable);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR21:" + ex.Message;
            }
        }
        /// <summary>
        /// Print function for Dotmatrix Printer
        /// </summary>
        /// <param name="decMasterId"></param>
        public void PrintForDotMatrix(decimal decMasterId)
        {
            try
            {
                DataTable dtblOtherDetails = new DataTable();
                CompanySP spComapany = new CompanySP();
                dtblOtherDetails = spComapany.CompanyViewForDotMatrix();
                DataTable dtblGridDetails = new DataTable();
                dtblGridDetails.Columns.Add("SlNo");
                dtblGridDetails.Columns.Add("BankAccount");
                dtblGridDetails.Columns.Add("ChequeNo");
                dtblGridDetails.Columns.Add("ChequeDate");
                dtblGridDetails.Columns.Add("Amount");
                DataRow dr = dtblGridDetails.NewRow();
                dr["SlNo"] = 1;
                dr["ChequeNo"] = txtcheckNo.Text;
                dr["BankAccount"] = cmbBank.Text;
                dr["ChequeDate"] = txtCheckDate.Text;
                dr["Amount"] = txtAmount.Text;
                dtblGridDetails.Rows.Add(dr);
                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("ledgerName");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("TotalAmount");
                dtblOtherDetails.Columns.Add("AmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                dtblOtherDetails.Columns.Add("CustomerAddress");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtVoucherNo.Text;
                dRowOther["date"] = txtVoucherDate.Text;
                dRowOther["ledgerName"] = cmbAccountLedger.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["TotalAmount"] = txtAmount.Text;
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                infoAccountLedger = spAccountLedger.AccountLedgerView(Convert.ToDecimal(cmbAccountLedger.SelectedValue));
                dRowOther["CustomerAddress"] = (infoAccountLedger.Address.ToString().Replace("\n", ", ")).Replace("\r", "");
                string s = new NumToText().AmountWords(Convert.ToDecimal(txtAmount.Text), PublicVariables._decCurrencyId);
                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtAmount.Text), PublicVariables._decCurrencyId);
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decPDCReceivableVoucherTypeId);
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
                formMDI.infoError.ErrorString = "PR22:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPDCReceivableRegister to view details and for updation 
        /// </summary>
        /// <param name="pdcreceivableRegObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromPDCReceivableRegister(frmPDCReceivableRegister pdcreceivableRegObj, decimal decMasterId)
        {
            try
            {
                pdcreceivableRegObj.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                PDCReceivableRegisterObj = pdcreceivableRegObj;
                decPDCReceivableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR23:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill function to updation
        /// </summary>
        public void FillFunction()
        {
            try
            {
                PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
                PDCReceivableMasterInfo infopdcrecivable = new PDCReceivableMasterInfo();
                infopdcrecivable = sppdcreceivable.PDCReceivableMasterView(decPDCReceivableEditId);
                txtVoucherNo.ReadOnly = false;
                strVoucherNo = infopdcrecivable.VoucherNo;
                strInvoiceNo = infopdcrecivable.InvoiceNo;
                txtVoucherNo.Text = strInvoiceNo;
                decSufixprefixPdcReceivableID = infopdcrecivable.SuffixPrefixId;
                decPDCReceivableVoucherTypeId = infopdcrecivable.VoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPDCReceivableVoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.ReadOnly = true;
                }
                else
                {
                    txtVoucherNo.ReadOnly = false;
                    lblVoucherNoManualValidator.Visible = false;
                }
                if (infopdcrecivable.PdcReceivableMasterId != 0)
                {
                    txtVoucherNo.Text = infopdcrecivable.InvoiceNo;
                    dtpVoucherDate.Value = infopdcrecivable.Date;
                    txtVoucherDate.Text = dtpVoucherDate.Value.ToString("dd-MMM-yyyy");
                    txtNarration.Text = infopdcrecivable.Narration;
                    cmbAccountLedger.SelectedValue = infopdcrecivable.LedgerId;
                    txtAmount.Text = infopdcrecivable.Amount.ToString();
                    if (infopdcrecivable.BankId != 0)
                        cmbBank.SelectedValue = infopdcrecivable.BankId;
                    else
                        cmbBank.SelectedValue = string.Empty;
                    txtcheckNo.Text = infopdcrecivable.ChequeNo;
                    txtCheckDate.Text = infopdcrecivable.ChequeDate.ToString("dd-MMM-yyyy");
                    btnSave.Text = "Update";
                    PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                    DataTable dtbl1 = new DataTable();
                    dtbl1 = SpPartyBalance.PartyBalanceViewByVoucherNoAndVoucherType(decPDCReceivableVoucherTypeId, strVoucherNo, infopdcrecivable.Date);
                    dtblPartyBalance = dtbl1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR24:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPDCRecievableReport to view details and for updation
        /// </summary>
        /// <param name="pdcReceivableReport"></param>
        /// <param name="decMasterId"></param>
        public void CallFromPdcReceivableReport(frmPDCRecievableReport pdcReceivableReport, decimal decMasterId)
        {
            try
            {
                pdcReceivableReport.Enabled = false;
                base.Show();
                this.PDCReceivableReportObj = pdcReceivableReport;
                isInEditMode = true;
                btnDelete.Enabled = true;
                PDCReceivableReportObj = pdcReceivableReport;
                decPDCReceivableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR25:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmBillallocation to view details and for updation
        /// </summary>
        /// <param name="frmBillallocation"></param>
        /// <param name="decPdcReceivableMasterId"></param>
        public void CallFromBillAllocation(frmBillallocation frmBillallocation, decimal decPdcReceivableMasterId)
        {
            try
            {
                frmBillallocation.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmBillallocationObj = frmBillallocation;
                decPDCReceivableEditId = decPdcReceivableMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR26:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDayBook to view details and for updation
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decPDCMasterId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decPDCMasterId)
        {
            try
            {
                frmDayBook.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmDayBookObj = frmDayBook;
                decPDCReceivableEditId = decPDCMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR27:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmAgeingReport to view details and for updation
        /// </summary>
        /// <param name="frmAgeing"></param>
        /// <param name="decPDCMasterId"></param>
        public void callFromAgeing(frmAgeingReport frmAgeing, decimal decPDCMasterId)
        {
            try
            {
                frmAgeing.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmAgeingObj = frmAgeing;
                decPDCReceivableEditId = decPDCMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR28:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmChequeReport to view details and for updation
        /// </summary>
        /// <param name="frmChequeReport"></param>
        /// <param name="decPDCMasterId"></param>
        public void CallFromChequeReport(frmChequeReport frmChequeReport, decimal decPDCMasterId)
        {
            try
            {
                frmChequeReport.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmChequeReportObj = frmChequeReport;
                decPDCReceivableEditId = decPDCMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR29:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmLedgerDetails to view details and for updation
        /// </summary>
        /// <param name="frmledgerdetails"></param>
        /// <param name="decMasterId"></param>
        public void CallFromLedgerDetails(frmLedgerDetails frmledgerdetails, decimal decMasterId)
        {
            try
            {
                frmLedgerDetailsObj = frmledgerdetails;
                frmLedgerDetailsObj.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                decPDCReceivableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR30:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for adding datatable column to keep party balance entry's
        /// </summary>
        public void DataTableForPartyBalance()
        {
            try
            {
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
                arrlstOfDeletedPartyBalanceRow = new ArrayList();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR31:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// When form loads call the clear function and cleare the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPdcReceivable_Load(object sender, EventArgs e)
        {
            try
            {
                ClearFunction();
                DataTableForPartyBalance();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR32:" + ex.Message;
            }
        }
        /// <summary>
        /// Account ledger combobox index changed, get the id for party balance and chwcking the ledger comes under sundry dr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountLedgerSP spaccountledger = new AccountLedgerSP();
            try
            {
                strAccNameText = cmbAccountLedger.Text;
                if (isWorkLedgerIndexChange)
                {
                    if (cmbAccountLedger.SelectedValue != null && cmbAccountLedger.SelectedValue.ToString() != string.Empty)
                    {
                        if (spaccountledger.AccountGroupIdCheckSundryDeptor(cmbAccountLedger.Text))
                        {
                            btnAgainRef.Enabled = true;
                            txtAmount.ReadOnly = true;
                            decLedgerIdForPartyBalance = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                        }
                        else
                        {
                            btnAgainRef.Enabled = false;
                            txtAmount.ReadOnly = false;
                        }
                    }
                    strOldLedgerId = (cmbAccountLedger.SelectedValue.ToString());
                    txtAmount.Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR33:" + ex.Message;
            }
        }
        /// <summary>
        /// AgainRef click, call the Party balance form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgainRef_Click(object sender, EventArgs e)
        {
            frmPartyBalance frmpartybalance = new frmPartyBalance();
            frmpartybalance.MdiParent = formMDI.MDIObj;
            try
            {
                if (!isAutomatic)
                {
                    if (txtVoucherNo.Text == string.Empty)
                    {
                        MessageBox.Show("Voucher Number Empty", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtVoucherNo.Focus();
                    }
                    else
                    {
                        frmpartybalance.CallThisFormFromPDCReceivable(this, decLedgerIdForPartyBalance, dtblPartyBalance, decPDCReceivableVoucherTypeId, txtVoucherNo.Text, DateTime.Parse(txtVoucherDate.Text));
                    }
                }
                else
                {
                    frmpartybalance.CallThisFormFromPDCReceivable(this, decLedgerIdForPartyBalance, dtblPartyBalance, decPDCReceivableVoucherTypeId, strVoucherNo, DateTime.Parse(txtVoucherDate.Text));
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR34:" + ex.Message;
            }
        }
        /// <summary>
        ///  Save button click, checking the user privilage and call the saveoredit function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEditFunction();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR35:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click, call the cleare Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR36:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click, Checking the user privilage and checking the references
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                PDCReceivableMasterSP sppdcreceivable = new PDCReceivableMasterSP();
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (PublicVariables.isMessageDelete)
                    {
                        if (Messages.DeleteMessage())
                        {
                            if (isInEditMode && sppdcreceivable.PDCReceivableReferenceCheck(decPDCReceivableEditId))
                            {
                                Messages.ReferenceExistsMessage();
                            }
                            else
                            {
                                DeleteFunction(decPDCReceivableEditId);
                                txtVoucherNo.Focus();
                            }
                        }
                    }
                    else
                    {
                        if (isInEditMode && sppdcreceivable.PDCReceivableReferenceCheck(decPDCReceivableEditId))
                        {
                            Messages.ReferenceExistsMessage();
                        }
                        DeleteFunction(decPDCReceivableEditId);
                        txtVoucherNo.Focus();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR37:" + ex.Message;
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
                if (Messages.CloseConfirmation())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR38:" + ex.Message;
            }
        }
        /// <summary>
        /// Create a new account ledger using these button form these form for party
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccountLedger_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    if (cmbAccountLedger.SelectedValue != null)
                    {
                        strAccountLedger = cmbAccountLedger.SelectedValue.ToString();
                    }
                    else
                    {
                        strAccountLedger = "0";
                    }
                    frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();
                    frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                    frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                    if (open == null)
                    {
                        frmAccountLedgerObj.WindowState = FormWindowState.Normal;
                        frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                        frmAccountLedgerObj.CallThisFormFromPDCreceivable(this);//Edited by Najma
                    }
                    else
                    {
                        open.MdiParent = formMDI.MDIObj;
                        open.BringToFront();
                        open.CallThisFormFromPDCreceivable(this);
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                    this.Enabled = false;
                }
                else
                {
                    MessageBox.Show("You don’t have privilege", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR39:" + ex.Message;
            }
        }
        /// <summary>
        /// Create a new account ledger using these button form these form for bank
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccountLedger2_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    if (cmbBank.SelectedValue != null)
                    {
                        strBankID = cmbBank.SelectedValue.ToString();
                    }
                    else
                    {
                        strBankID = "0";
                    }
                    frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();
                    frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                    frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                    if (open == null)
                    {
                        frmAccountLedgerObj.WindowState = FormWindowState.Normal;
                        frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                        frmAccountLedgerObj.CallThisFormFromPDCreceivable2(this);//Edited by Najma
                    }
                    else
                    {
                        open.MdiParent = formMDI.MDIObj;
                        open.BringToFront();
                        open.CallThisFormFromPDCreceivable2(this);
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                    this.Enabled = false;
                }
                else
                {
                    MessageBox.Show("You don’t have privilege", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR40:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the datetime picker current value in text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpVoucherDate.Value;
                this.txtVoucherDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR41:" + ex.Message;
            }
        }
        /// <summary>
        ///  Set the text box current value in datetime picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpcheckdate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpcheckdate.Value;
                this.txtCheckDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR42:" + ex.Message;
            }
        }
        /// <summary>
        /// Form closing event, checking the other form status, its opend or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPdcReceivable_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (PDCReceivableRegisterObj != null)
                {
                    PDCReceivableRegisterObj.Enabled = true;
                    PDCReceivableRegisterObj.GridSearchRegister();
                }
                if (PDCReceivableReportObj != null)
                {
                    PDCReceivableReportObj.Enabled = true;
                    PDCReceivableReportObj.Search();
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
                if (frmChequeReportObj != null)
                {
                    frmChequeReportObj.Enabled = true;
                    frmChequeReportObj.ChequeReportFillGrid();
                }
                if (frmBillallocationObj != null)
                {
                    frmBillallocationObj.Enabled = true;
                    frmBillallocationObj.BillAllocationGridFill();
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.LedgerDetailsView();
                }
                if (frmAgeingObj != null)
                {
                    frmAgeingObj.Enabled = true;
                    frmAgeingObj.FillGrid();
                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Enabled = true;
                    objVoucherSearch.GridFill();
                }
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR43:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherDate.Focus();
                    txtVoucherDate.SelectionStart = 0;
                    txtVoucherDate.SelectionLength = 0;
                }
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR44:" + ex.Message;
            }
        }
        /// <summary>
        /// From keydown for Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPdcReceivable_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    btnSave_Click(sender, e);
                }
                else if (e.Control && e.KeyCode == Keys.D)
                {
                    if (btnDelete.Enabled)
                        btnDelete_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR45:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountLedger.Focus();
                }
                if (txtVoucherDate.Text == string.Empty || txtVoucherDate.SelectionStart == 0)
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
                formMDI.infoError.ErrorString = "PR46:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAccountLedger.Text = strAccNameText;
                    if (btnAgainRef.Enabled == true)
                    {
                        btnAgainRef.Focus();
                    }
                    else
                    {
                        txtAmount.Focus();
                        txtAmount.SelectionStart = 0;
                        txtAmount.SelectionLength = 0;
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbAccountLedger.Text == string.Empty || cmbAccountLedger.SelectionStart == 0)
                    {

                        txtVoucherDate.Focus();
                        txtVoucherDate.SelectionStart = 0;
                        txtVoucherDate.SelectionLength = 0;
                    }
                }
                else if (e.Alt && e.KeyCode == Keys.C)
                {
                    SendKeys.Send("{F10}");
                    btnNewAccountLedger_Click(sender, e);
                }
                else if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
                {
                    if (cmbAccountLedger.Focused)
                    {
                        cmbAccountLedger.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                    else
                    {
                        cmbAccountLedger.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    if (cmbAccountLedger.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromPdcReceivableVoucher(this, Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString()), "Account Ledger");
                        this.Enabled = false;
                    }
                    else
                    {
                        Messages.InformationMessage("Select any Account Ledger");
                        cmbAccountLedger.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR47:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBank.Focus();
                    cmbBank.SelectionStart = 0;
                    cmbBank.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtAmount.Text == string.Empty || txtAmount.SelectionStart == 0)
                    {
                        if (btnAgainRef.Enabled == true)
                            btnAgainRef.Focus();
                        else
                        {
                            cmbAccountLedger.Focus();
                            cmbAccountLedger.SelectionStart = 0;
                            cmbAccountLedger.SelectionLength = 0;
                        }
                    }
                }
            }
         catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR48:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBank_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBank.Text = strBankNameText;
                    txtcheckNo.Focus();
                    txtcheckNo.SelectionStart = 0;
                    txtcheckNo.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (cmbBank.Text == string.Empty || cmbBank.SelectionStart == 0)
                    {
                        txtAmount.Focus();
                        txtAmount.SelectionStart = 0;
                        txtAmount.SelectionLength = 0;
                    }
                }
                else if (e.Alt && e.KeyCode == Keys.C)
                {
                    SendKeys.Send("{F10}");
                    btnNewAccountLedger2_Click(sender, e);
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (cmbBank.Focused)
                    {
                        cmbBank.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                    else
                    {
                        cmbBank.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    if (cmbBank.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromPdcReceivableVoucher(this, Convert.ToDecimal(cmbBank.SelectedValue.ToString()), "Bank");
                        this.Enabled = false;
                    }
                    else
                    {
                        Messages.InformationMessage("Select any Bank account");
                        cmbBank.Text = string.Empty;
                    }
                }
            }
          catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR49:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcheckNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCheckDate.Focus();
                    txtCheckDate.SelectionStart = 0;
                    txtCheckDate.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtcheckNo.Text == string.Empty || txtcheckNo.SelectionStart == 0)
                    {
                        cmbBank.Focus();
                        cmbBank.SelectionStart = 0;
                        cmbBank.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR50:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCheckDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtCheckDate.Text == string.Empty || txtCheckDate.SelectionStart == 0)
                    {
                        txtcheckNo.Focus();
                        txtcheckNo.SelectionStart = 0;
                        txtcheckNo.SelectionLength = 0;
                    }
                }
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR51:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        txtCheckDate.Focus();
                        txtCheckDate.SelectionStart = 0;
                        txtCheckDate.SelectionLength = 0;
                    }
                }
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR52:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inKeyPrsCount++;
                    if (inKeyPrsCount == 2)
                    {
                        inKeyPrsCount = 0;
                        btnSave.Focus();
                    }
                }
                else
                {
                    inKeyPrsCount = 0;
                }
            }
         catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR53:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set the date format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtVoucherDate);
                if (!isInvalid)
                {
                    txtVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtVoucherDate.Text;
                dtpVoucherDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR54:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation and set the date format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCheckDate_Leave(object sender, EventArgs e)
        {
            try
            {
                OtherDateValidationFunction obj = new OtherDateValidationFunction();
                bool isInvalid = obj.DateValidationFunction(txtCheckDate, false);
                if (!isInvalid)
                {
                    txtCheckDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtCheckDate.Text;
                dtpcheckdate.Value = Convert.ToDateTime(date);
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR55:" + ex.Message;
            }
        }
        /// <summary>
        /// calling the Click function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgainRef_Enter(object sender, EventArgs e)
        {
            try
            {
                btnAgainRef_Click(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR56:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgainRef_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbAccountLedger.Focus();
                    cmbAccountLedger.SelectionStart = 0;
                    cmbAccountLedger.SelectionLength = 0;
                }
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR57:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the amount column for decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
           catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR58:" + ex.Message;
            }
        }
        /// <summary>
        /// For backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PR59:" + ex.Message;
            }
        }
        #endregion
    }
}
