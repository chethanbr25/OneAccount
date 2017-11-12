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

    public partial class frmPdcPayable : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strAccountLedger;
        string strAccNameText = string.Empty;//To keep the selected Account Name
        int inKeyPrsCount = 0;
        string strInvoiceNo = string.Empty;
        decimal decPDCpayableEditId = 0;
        string strBankID;               //To keep the selectd  BankId
        string strLedgerID = string.Empty;      //To keep the selectd ledgerid 
        decimal decLedgerIdForPartyBalance; // To keep ledger Id
        decimal decPDCpayableVoucherTypeId = 0;
        bool isAutomatic = false;       //To checking vocher no generation auto or not
        decimal decSufixprefixPdcpayableID = 0;
        string strVoucherNo = string.Empty;
        string strReturnNo = string.Empty;
        string strtableName = "PdcPayableMaster";
        string strOldLedgerId = string.Empty;
        string strOldBankLedgerId = string.Empty;
        ArrayList arrlstOfDeletedPartyBalanceRow;
        bool isWorkLedgerIndexChange = true;
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();//to use in call from ledger popup function
        DataTable dtblPartyBalance = new DataTable(); // To pass values to party balance
        bool isInEditMode = false; // Tp decide whether is edit mode of not
        frmPartyBalance frmPartyBalanceObj = new frmPartyBalance();//To use in call from PartyBalance class
        frmPDCPayableRegister PDCPayableRegisterObj = null;//To use in call from PdcPayableRegister class
        frmPDCPayableReport PDCpayableReportObj = null;//To use in call from pdcpayableReport
        frmBillallocation BillallocationObj = null;//To use in call from Billallocation
        frmVoucherSearch frmVoucherSearch = null;
        frmDayBook frmDayBookObj = null;//To use in call from frmDayBook
        frmAgeingReport frmAgeingObj = null;//To use in call from frmDayBook
        frmLedgerDetails frmLedgerDetailsObj;
        frmChequeReport frmChequeReportObj = null;
        #endregion
        #region Functions
        /// <summary>
        /// Create an instance for frmPdcPayable class
        /// </summary>
        public frmPdcPayable()
        {
            InitializeComponent();
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
                decPDCpayableVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPDCpayableVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPDCpayableVoucherTypeId, dtpVoucherDate.Value);
                decSufixprefixPdcpayableID = infoSuffixPrefix.SuffixprefixId;
                this.Text = strVoucherTypeName;
                base.Show();
                if (isAutomatic)
                {
                    txtVoucherDate.Focus();
                }
                else
                {
                    txtvoucherNo.Focus();
                }
                ClearFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP1:" + ex.Message;
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
                base.Show();
                this.frmVoucherSearch = frm;
                decPDCpayableEditId = decId;
                btnClear.Text = "New";
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                FillFunction();
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP2:" + ex.Message;
            }
        }
        /// <summary>
        /// Voucher no generation function based on the settings
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                if (isAutomatic == true)
                {
                    TransactionsGeneralFill obj = new TransactionsGeneralFill();
                    PDCPayableMasterSP spPdcpayable = new PDCPayableMasterSP();
                    if (strVoucherNo == string.Empty)
                    {
                        strVoucherNo = "0";
                    }
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPDCpayableVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strtableName);
                    if (Convert.ToDecimal(strVoucherNo) != spPdcpayable.PDCPayableMaxUnderVoucherTypePlusOne(decPDCpayableVoucherTypeId) + 1)
                    {
                        strVoucherNo = spPdcpayable.PDCPayableMaxUnderVoucherTypePlusOne(decPDCpayableVoucherTypeId).ToString();
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPDCpayableVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strtableName);
                        if (spPdcpayable.PDCPayableMaxUnderVoucherTypePlusOne(decPDCpayableVoucherTypeId) == 0)
                        {
                            strVoucherNo = "0";
                            strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPDCpayableVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strtableName);
                        }
                    }
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPDCpayableVoucherTypeId, dtpVoucherDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    decSufixprefixPdcpayableID = infoSuffixPrefix.SuffixprefixId;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtvoucherNo.Text = strInvoiceNo;
                    txtvoucherNo.ReadOnly = true;
                }
                else
                {
                    txtvoucherNo.ReadOnly = false;
                    txtvoucherNo.Text = string.Empty;
                    lblVoucherNoManualValidator.Visible = true;
                    strInvoiceNo = txtvoucherNo.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP3:" + ex.Message;
            }
        }
        /// <summary>
        /// Account name combofill function
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
                cmbAccountLedger.SelectedIndex = 0;
                isWorkLedgerIndexChange = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP4:" + ex.Message;
            }
        }
        /// <summary>
        /// Cash or bank combofill function
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
                formMDI.infoError.ErrorString = "PP5:" + ex.Message;
            }
        }
        /// <summary>
        /// The form will reset here, clearing the controlls
        /// </summary>
        public void ClearFunction()
        {
            try
            {
                VoucherNumberGeneration();
                decPDCpayableEditId = 0;
                CurrencySP SpCurrency = new CurrencySP();
                CurrencyInfo InfoCurrency = new CurrencyInfo();
                btnAgainstRef.Enabled = false;
                btnDelete.Enabled = false;
                isInEditMode = false;
                PrintCheck();
                cmbAccountNameFill();
                cmbBankAccountFill();
                txtAmount.Clear();
                txtcheckNo.Clear();
                FinancialYearDate();
                txtVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtChequeDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtNarration.Clear();
                dtblPartyBalance.Clear();
                btnSave.Text = "Save";
                cmbAccountLedger.Focus();
                if (PDCPayableRegisterObj != null)
                {
                    PDCPayableRegisterObj.Close();
                }
                if (PDCpayableReportObj != null)
                {
                    PDCpayableReportObj.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP6:" + ex.Message;
            }
        }
        /// <summary>
        /// Save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();
                DateValidation Objdatevalidation = new DateValidation();
                OtherDateValidationFunction ObjotherdateValidation = new OtherDateValidationFunction();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                Objdatevalidation.DateValidationFunction(txtVoucherDate);
                ObjotherdateValidation.DateValidationFunction(txtChequeDate, false);
                DataTable dtblMaster = new DataTable();
                PDCPayableMasterInfo InfoPayable = new PDCPayableMasterInfo();
                PDCPayableMasterSP SpPayable = new PDCPayableMasterSP();
                InfoPayable.VoucherNo = strVoucherNo;
                InfoPayable.InvoiceNo = txtvoucherNo.Text.Trim();
                InfoPayable.Date = DateTime.Parse(txtVoucherDate.Text);
                InfoPayable.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                InfoPayable.Amount = decimal.Parse(txtAmount.Text);
                InfoPayable.Narration = txtNarration.Text;
                InfoPayable.ChequeNo = txtcheckNo.Text;
                if (txtChequeDate.Text != string.Empty)
                    InfoPayable.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
                else
                    InfoPayable.ChequeDate = DateTime.Now;
                InfoPayable.UserId = PublicVariables._decCurrentUserId;
                InfoPayable.VoucherTypeId = decPDCpayableVoucherTypeId;
                if (cmbBank.SelectedValue != null && cmbBank.SelectedValue.ToString() != string.Empty)
                {
                    InfoPayable.BankId = Convert.ToDecimal(cmbBank.SelectedValue.ToString());
                }
                else
                    InfoPayable.ExtraDate = DateTime.Now;
                InfoPayable.Extra1 = string.Empty;
                InfoPayable.Extra2 = string.Empty;
                if (!isInEditMode)
                {
                    decimal decIdentity = SpPayable.PDCPayableMasterAdd(InfoPayable);
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
                    decimal decIdentity = decPDCpayableEditId;
                    InfoPayable.PdcPayableMasterId = decPDCpayableEditId;
                    SpPayable.PDCPayableMasterEdit(InfoPayable);
                    LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                    spLedgerPosting.LedgerPostingDeleteByVoucherNoVoucherTypeIdAndLedgerId(strVoucherNo, decPDCpayableVoucherTypeId, 12);
                    spPartyBalance.PartyBalanceDeleteByVoucherTypeAndVoucherNo(decPDCpayableVoucherTypeId, strVoucherNo);
                    PartyBalanceAddOrEdit();
                    LedgerPostingEdit(decPDCpayableEditId);
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
                formMDI.infoError.ErrorString = "PP7:" + ex.Message;
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
                DataSet dsPdcPayable = new DataSet();
                PDCPayableMasterSP Sppdcpayable = new PDCPayableMasterSP();
                dsPdcPayable = Sppdcpayable.PDCpayableVoucherPrinting(decMasterId, 1);
                frmReport frmreport = new frmReport();
                frmreport.MdiParent = formMDI.MDIObj;
                frmreport.PDCpayableReportPrinting(dsPdcPayable);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP8:" + ex.Message;
            }
        }
        /// <summary>
        /// print function for dotmatrix printer
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
                dr["ChequeDate"] = txtChequeDate.Text;
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
                dRowOther["voucherNo"] = txtvoucherNo.Text;
                dRowOther["date"] = txtVoucherDate.Text;
                dRowOther["ledgerName"] = cmbAccountLedger.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["TotalAmount"] = txtAmount.Text;
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                infoAccountLedger = spAccountLedger.AccountLedgerView(Convert.ToDecimal(cmbAccountLedger.SelectedValue));
                dRowOther["CustomerAddress"] = (infoAccountLedger.Address.ToString().Replace("\n", ", ")).Replace("\r", "");
                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtAmount.Text), PublicVariables._decCurrencyId);
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decPDCpayableVoucherTypeId);
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
                formMDI.infoError.ErrorString = "PP9:" + ex.Message;
            }
        }
        /// <summary>
        /// checking the print check status for print checkbox
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
                formMDI.infoError.ErrorString = "PP10:" + ex.Message;
            }
        }
        /// <summary>
        /// Setting the financial year date here
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
                dtpchekdate.Value = dtcheckdate;
                txtChequeDate.Text = dtcheckdate.ToString("dd-MMM-yyyy");
                dtpchekdate.Value = Convert.ToDateTime(txtChequeDate.Text);
                txtChequeDate.Focus();
                txtChequeDate.SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP11:" + ex.Message;
            }
        }
        /// <summary>
        /// Save or edit function, its for checking the invalid entries
        /// </summary>
        public void SaveOrEditFunction()
        {
            try
            {
                txtVoucherDate.Text = txtVoucherDate.Text.Trim();
                cmbAccountLedger.Text = cmbAccountLedger.Text.Trim();
                txtAmount.Text = txtAmount.Text.Trim();
                cmbBank.Text = cmbBank.Text.Trim();
                txtChequeDate.Text = txtChequeDate.Text.Trim();
                txtcheckNo.Text = txtcheckNo.Text.Trim();
                txtNarration.Text = txtNarration.Text.Trim();
                bool isOk = true;
                bool isAmountOk = false;
                try
                {
                    if (decimal.Parse(txtAmount.Text) > 0)
                        isAmountOk = true;
                }
                catch
                {
                    txtAmount.Text = string.Empty;
                }
                if (isOk)
                {
                    if (txtvoucherNo.Text == string.Empty)
                    {
                        Messages.InformationMessage("Enter voucher no");
                        txtvoucherNo.Focus();
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
                    else if (txtChequeDate.Text == string.Empty)
                    {
                        Messages.InformationMessage("Select cheque date");
                        txtChequeDate.Focus();
                    }
                    else
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            isOk = false;
                            PDCPayableMasterSP SpPDCpayable = new PDCPayableMasterSP();
                            if (!isInEditMode)
                            {
                                if (Messages.SaveMessage())
                                    if (!SpPDCpayable.PDCpayableCheckExistence(txtvoucherNo.Text.Trim(), decPDCpayableVoucherTypeId, 0))
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
                                    if (isInEditMode && SpPDCpayable.PDCPayableVoucherCheckRreference(decPDCpayableEditId, decPDCpayableVoucherTypeId))
                                    {
                                        MessageBox.Show("Can't update,reference exist", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        SaveFunction();
                                    }
                                if (BillallocationObj != null)
                                {
                                    this.Close();
                                    BillallocationObj.Enabled = true;
                                    BillallocationObj.BillAllocationGridFill();
                                }
                            }
                        }
                        if (isOk)
                        {
                            if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                            {
                                PDCPayableMasterSP SpPDCpayable = new PDCPayableMasterSP();

                                if (isInEditMode && SpPDCpayable.PDCPayableVoucherCheckRreference(decPDCpayableEditId, decPDCpayableVoucherTypeId))
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
                formMDI.infoError.ErrorString = "PP12:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PP13:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Account ledger combobox while return from Account ledger creation when creating new ledger for bank
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
                formMDI.infoError.ErrorString = "PP14:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger posting edit function
        /// </summary>
        /// <param name="decpdcMasterId"></param>
        public void LedgerPostingEdit(decimal decpdcMasterId)
        {
            PDCPayableMasterSP SpPDCPayable = new PDCPayableMasterSP();
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
                
                dtblLedgerPostingId = SpPDCPayable.LedgerPostingIdByPDCpayableId(decpdcMasterId);
                decimal decledgerpostingId1 = Convert.ToDecimal(dtblLedgerPostingId.Rows[0]["ledgerPostingId"].ToString());
                decimal decLedgerPostingId2 = Convert.ToDecimal(dtblLedgerPostingId.Rows[1]["ledgerPostingId"].ToString());
                if (!btnAgainstRef.Enabled)
                {
                    infoLedgerPosting.LedgerPostingId = decledgerpostingId1;
                    infoLedgerPosting.VoucherTypeId = decPDCpayableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtvoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Debit = Convert.ToDecimal(txtAmount.Text.ToString());
                    infoLedgerPosting.Credit = 0;
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
                    infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                    infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    SpLedgerPosting.LedgerPostingEdit(infoLedgerPosting);

                }
                else
                {
                    infoLedgerPosting.LedgerPostingId = decledgerpostingId1;
                    infoLedgerPosting.VoucherTypeId = decPDCpayableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtvoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Credit = 0;
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
                    infoLedgerPosting.Debit = decConvertRate;

                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
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

                                    infoLedgerPosting.Debit = decForexAmount;
                                    infoLedgerPosting.Credit = 0;
                                }
                                else
                                {
                                    infoLedgerPosting.Credit = -1 * decForexAmount;
                                    infoLedgerPosting.Debit = 0;
                                }
                                SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }

                    }
                }

                infoLedgerPosting.LedgerPostingId = decLedgerPostingId2;
                infoLedgerPosting.VoucherNo = strVoucherNo;
                infoLedgerPosting.InvoiceNo = txtvoucherNo.Text.Trim();
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.LedgerId = 6;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Debit = 0;
                infoLedgerPosting.Credit = Convert.ToDecimal(txtAmount.Text.ToString());
                infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
                infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                SpLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP15:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger posting save function
        /// </summary>
        public void LedgerPosting()
        {
            try
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
                if (!btnAgainstRef.Enabled)
                {

                    infoLedgerPosting.VoucherTypeId = decPDCpayableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtvoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Debit = Convert.ToDecimal(txtAmount.Text.ToString());
                    infoLedgerPosting.Credit = 0;
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
                    infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                    infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

                }
                else
                {
                    infoLedgerPosting.VoucherTypeId = decPDCpayableVoucherTypeId;
                    infoLedgerPosting.VoucherNo = strVoucherNo;
                    infoLedgerPosting.InvoiceNo = txtvoucherNo.Text.Trim();
                    infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                    infoLedgerPosting.LedgerId = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.Credit = 0;
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
                    infoLedgerPosting.Debit = decConvertRate;
                    infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
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

                                    infoLedgerPosting.Debit = decForexAmount;
                                    infoLedgerPosting.Credit = 0;
                                }
                                else
                                {
                                    infoLedgerPosting.Credit = -1 * decForexAmount;
                                    infoLedgerPosting.Debit = 0;
                                }
                                SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }

                    }
                }
                infoLedgerPosting.LedgerId = 6;
                infoLedgerPosting.VoucherNo = strVoucherNo;
                infoLedgerPosting.InvoiceNo = txtvoucherNo.Text.Trim();
                infoLedgerPosting.Date = PublicVariables._dtCurrentDate;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.Debit = 0;
                infoLedgerPosting.Credit = Convert.ToDecimal(txtAmount.Text);
                infoLedgerPosting.ChequeDate = Convert.ToDateTime(txtChequeDate.Text);
                infoLedgerPosting.ChequeNo = txtcheckNo.Text.Trim();
                infoLedgerPosting.ExtraDate = PublicVariables._dtCurrentDate;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                SpLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP16:" + ex.Message;
            }
        }
        /// <summary>
        /// Party balance save or update function
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
                formMDI.infoError.ErrorString = "PP17:" + ex.Message;
            }
        }
        /// <summary>
        /// Party balance add function
        /// </summary>
        /// <param name="inJ"></param>
        public void PartyBalanceAdd(int inJ)
        {
            try
            {
                int inTableRowCount = dtblPartyBalance.Rows.Count;
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
                InfopartyBalance.CreditPeriod = 0;
                InfopartyBalance.Date = dtpVoucherDate.Value;
                InfopartyBalance.LedgerId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString());
                InfopartyBalance.ReferenceType = dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString();
                if (dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "New" || dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "OnAccount")
                {
                    InfopartyBalance.AgainstInvoiceNo = "0";//dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0";// dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0;//Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decPDCpayableVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decPDCpayableVoucherTypeId; ;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                InfopartyBalance.Debit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                InfopartyBalance.Credit = 0;
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spPartyBalance.PartyBalanceAdd(InfopartyBalance);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP18:" + ex.Message;
            }
        }
        /// <summary>
        /// Party balance edit function
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
                InfopartyBalance.CreditPeriod = 0;
                InfopartyBalance.Date = dtpVoucherDate.Value;
                InfopartyBalance.LedgerId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString());
                InfopartyBalance.ReferenceType = dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString();
                if (dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "New" || dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "OnAccount")
                {
                    InfopartyBalance.AgainstInvoiceNo = "0";// dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                    InfopartyBalance.AgainstVoucherNo = "0";// dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.AgainstVoucherTypeId = 0;// Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());//decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = decPDCpayableVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    InfopartyBalance.VoucherNo = strVoucherNo;
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    InfopartyBalance.AgainstVoucherTypeId = decPDCpayableVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                InfopartyBalance.Debit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                InfopartyBalance.Credit = 0;
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spPartyBalance.PartyBalanceEdit(InfopartyBalance);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP19:" + ex.Message;
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP20:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmPDCPayableRegister to view details and for updation
        /// </summary>
        /// <param name="pdcpayableRegObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromPDCPayableRegister(frmPDCPayableRegister pdcpayableRegObj, decimal decMasterId)
        {
            try
            {
                pdcpayableRegObj.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                PDCPayableRegisterObj = pdcpayableRegObj;
                decPDCpayableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP21:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill function for updation
        /// </summary>
        public void FillFunction()
        {
            try
            {
                PDCPayableMasterInfo infopdcpayable = new PDCPayableMasterInfo();
                PDCPayableMasterSP spPdcpayable = new PDCPayableMasterSP();
                infopdcpayable = spPdcpayable.PDCPayableMasterView(decPDCpayableEditId);
                txtvoucherNo.ReadOnly = false;
                strVoucherNo = infopdcpayable.VoucherNo;
                strInvoiceNo = infopdcpayable.InvoiceNo;
                txtvoucherNo.Text = strInvoiceNo;
                decSufixprefixPdcpayableID = infopdcpayable.SuffixPrefixId;
                decPDCpayableVoucherTypeId = infopdcpayable.VoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPDCpayableVoucherTypeId);
                if (isAutomatic)
                {
                    txtvoucherNo.ReadOnly = true;
                }
                else
                {
                    txtvoucherNo.ReadOnly = false;
                }
                if (infopdcpayable.PdcPayableMasterId != 0)
                {
                    txtvoucherNo.Text = infopdcpayable.InvoiceNo;
                    dtpVoucherDate.Value = infopdcpayable.Date;
                    txtVoucherDate.Text = dtpVoucherDate.Value.ToString("dd-MMM-yyyy");
                    txtNarration.Text = infopdcpayable.Narration;
                    cmbAccountLedger.SelectedValue = infopdcpayable.LedgerId;
                    txtAmount.Text = infopdcpayable.Amount.ToString();
                    if (infopdcpayable.BankId != 0)
                        cmbBank.SelectedValue = infopdcpayable.BankId;
                    else
                        cmbBank.SelectedValue = string.Empty;
                    txtcheckNo.Text = infopdcpayable.ChequeNo;
                    txtChequeDate.Text = infopdcpayable.ChequeDate.ToString("dd-MMM-yyyy");
                    btnSave.Text = "Update";
                    PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                    DataTable dtbl1 = new DataTable();
                    dtbl1 = SpPartyBalance.PartyBalanceViewByVoucherNoAndVoucherType(decPDCpayableVoucherTypeId, strVoucherNo, infopdcpayable.Date);
                    dtblPartyBalance = dtbl1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP22:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete function
        /// </summary>
        /// <param name="decPDCpayableMasterId"></param>
        public void DeleteFunction(decimal decPDCpayableMasterId)
        {
            try
            {
                PDCPayableMasterSP sppdcpayable = new PDCPayableMasterSP();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                if (!spPartyBalance.PartyBalanceCheckReference(decPDCpayableVoucherTypeId, strVoucherNo))
                {
                    sppdcpayable.PDCPayableMasterDelete(decPDCpayableMasterId, decPDCpayableVoucherTypeId, strVoucherNo);
                    Messages.DeletedMessage();
                }
                else
                {
                    Messages.InformationMessage("Reference exist. Cannot delete");
                    txtVoucherDate.Focus();
                }
                if (PDCPayableRegisterObj != null)
                {
                    this.Close();
                    PDCPayableRegisterObj.Enabled = true;
                }
                else if (PDCpayableReportObj != null)
                {
                    this.Close();
                    PDCpayableReportObj.Enabled = true;
                }
                else if (frmVoucherSearch != null)
                {
                    this.Close();
                    frmVoucherSearch.GridFill();
                }
                else if (frmLedgerDetailsObj != null)
                {
                    this.Close();
                }
                else if (frmDayBookObj != null)
                {
                    this.Close();
                }
                else if (BillallocationObj != null)
                {
                    this.Close();
                }
                else
                {
                    ClearFunction();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP23:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPDCPayableReport to view details and for updation
        /// </summary>
        /// <param name="pdcpayableReportObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromPdcPayableReport(frmPDCPayableReport pdcpayableReportObj, decimal decMasterId)
        {
            try
            {
                pdcpayableReportObj.Enabled = false;
                base.Show();
                this.PDCpayableReportObj = pdcpayableReportObj;
                isInEditMode = true;
                btnDelete.Enabled = true;
                PDCpayableReportObj = pdcpayableReportObj;
                decPDCpayableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP24:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PP25:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmBillallocation to view details and for updation
        /// </summary>
        /// <param name="frmBillallocation"></param>
        /// <param name="decPdcPayableMasterId"></param>
        public void CallFromBillAllocation(frmBillallocation frmBillallocation, decimal decPdcPayableMasterId)
        {
            try
            {
                frmBillallocation.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                BillallocationObj = frmBillallocation;
                decPDCpayableEditId = decPdcPayableMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP26:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDayBook to view details and for updation
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decMasterId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decMasterId)
        {
            try
            {
                frmDayBook.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmDayBookObj = frmDayBook;
                decPDCpayableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP27:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmAgeingReport to view details and for updation
        /// </summary>
        /// <param name="frmAgeing"></param>
        /// <param name="decMasterId"></param>
        public void callFromAgeing(frmAgeingReport frmAgeing, decimal decMasterId)
        {
            try
            {
                frmAgeing.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmAgeingObj = frmAgeing;
                decPDCpayableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP28:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmLedgerDetails to view details and for updation
        /// </summary>
        /// <param name="LedgerDetailsObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromLedgerDetails(frmLedgerDetails LedgerDetailsObj, decimal decMasterId)
        {
            try
            {
                LedgerDetailsObj.Enabled = false;
                base.Show();
                isInEditMode = true;
                btnDelete.Enabled = true;
                frmLedgerDetailsObj = LedgerDetailsObj;
                decPDCpayableEditId = decMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP29:" + ex.Message;
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
                decPDCpayableEditId = decPDCMasterId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP30:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for adding datatable column to keep party balance entry's
        /// </summary>
        public void PartyBalanceDataTable()
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
                formMDI.infoError.ErrorString = "PP31:" + ex.Message;
            }
        }
        #endregion
        #region  Events
        /// <summary>
        /// When form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPdcPayable_Load(object sender, EventArgs e)
        {
            try
            {
                ClearFunction();
                PartyBalanceDataTable();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP32:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the textbox text as datetime picker value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                txtVoucherDate.Text = dtpVoucherDate.Value.ToString("dd-MMM-yyyy");
                txtVoucherDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP33:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the textbox text as datetime picker value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpchekdate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                txtChequeDate.Text = dtpchekdate.Value.ToString("dd-MMM-yyyy");
                txtChequeDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP34:" + ex.Message;
            }
        }
        /// <summary>
        /// TO create a new uledger using the button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccountLedger_Click(object sender, EventArgs e)
        {
            try
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
                    frmAccountLedgerObj.CallThisFormFromPDCPayable(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallThisFormFromPDCPayable(this);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP35:" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click, check the user privilege and  call the save function
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
                formMDI.infoError.ErrorString = "PP36:" + ex.Message;
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
                ClearFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP37:" + ex.Message;
            }
        }
        /// <summary>
        /// To set the cheque as current date time and checking date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeDate_Leave(object sender, EventArgs e)
        {
            try
            {
                OtherDateValidationFunction obj = new OtherDateValidationFunction();
                bool isInvalid = obj.DateValidationFunction(txtChequeDate, false);
                if (!isInvalid)
                {
                    txtChequeDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtChequeDate.Text;
                dtpchekdate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP38:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation in Amount field
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
                formMDI.infoError.ErrorString = "PP39:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click, check the user privilage, references and call the delete function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                PDCPayableMasterSP sppdcpayable = new PDCPayableMasterSP();
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (PublicVariables.isMessageDelete)
                    {
                        if (Messages.DeleteMessage())
                        {
                            if (isInEditMode && sppdcpayable.PDCPayableReferenceCheck(decPDCpayableEditId, decPDCpayableVoucherTypeId))
                            {
                                Messages.ReferenceExistsMessage();
                            }
                            else
                            {
                                DeleteFunction(decPDCpayableEditId);
                                txtvoucherNo.Focus();
                            }
                        }
                    }
                    else
                    {
                        if (isInEditMode && sppdcpayable.PDCPayableReferenceCheck(decPDCpayableEditId, decPDCpayableVoucherTypeId))
                        {
                            Messages.ReferenceExistsMessage();
                        }
                        DeleteFunction(decPDCpayableEditId);
                        txtvoucherNo.Focus();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP40:" + ex.Message;
            }
        }
        /// <summary>
        /// close button click
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
                formMDI.infoError.ErrorString = "PP41:" + ex.Message;
            }
        }
        /// <summary>
        /// account no combobox selected index change 
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
                    if (cmbAccountLedger.SelectedValue != null && cmbAccountLedger.SelectedValue.ToString() != "")
                    {
                        if (spaccountledger.AccountGroupIdCheckSundryCreditorOnly(cmbAccountLedger.Text))
                        {
                            btnAgainstRef.Enabled = true;
                            txtAmount.ReadOnly = true;
                            decLedgerIdForPartyBalance = Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString());
                        }
                        else
                        {
                            btnAgainstRef.Enabled = false;
                            txtAmount.ReadOnly = false;
                        }
                    }
                    strOldLedgerId = (cmbAccountLedger.SelectedValue.ToString());
                    txtAmount.Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP42:" + ex.Message;
            }
        }
        /// <summary>
        /// Against references button click for party balance calculations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgainstRef_Click(object sender, EventArgs e)
        {
            try
            {
                AccountLedgerSP spaccountledger = new AccountLedgerSP();
                if (spaccountledger.AccountGroupIdCheck(cmbAccountLedger.Text))
                {
                    frmPartyBalance frmpartybalance = new frmPartyBalance();
                    frmpartybalance.MdiParent = formMDI.MDIObj;
                    if (!isAutomatic)
                    {
                        if (txtvoucherNo.Text == string.Empty)
                        {
                            MessageBox.Show("Voucher Number Empty", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtvoucherNo.Focus();
                        }
                        else
                        {
                            frmpartybalance.CallThisFormFromPDCPayable(this, decLedgerIdForPartyBalance, dtblPartyBalance, decPDCpayableVoucherTypeId, txtvoucherNo.Text, DateTime.Parse(txtVoucherDate.Text));
                        }
                    }
                    else
                    {
                        frmpartybalance.CallThisFormFromPDCPayable(this, decLedgerIdForPartyBalance, dtblPartyBalance, decPDCpayableVoucherTypeId, strVoucherNo, DateTime.Parse(txtVoucherDate.Text));
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP43:" + ex.Message;
            }
        }
        /// <summary>
        /// voucherdate textbox leave to set datetime value into textbox
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
                formMDI.infoError.ErrorString = "PP44:" + ex.Message;
            }
        }
        /// <summary>
        /// datetimepicker value change  to set datetime value into textbox for voucherDate
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
                formMDI.infoError.ErrorString = "PP45:" + ex.Message;
            }
        }
        /// <summary>
        /// datetimepicker value change  to set datetime value into textbox for ChequeDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpchekdate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpchekdate.Value;
                this.txtChequeDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP46:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledgername to keep
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLedger_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbAccountLedger.SelectedIndex == -1)
                {
                    cmbAccountLedger.Text = strAccNameText;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP47:" + ex.Message;
            }
        }
        /// <summary>
        /// Form closing, Checking the other forms is opend or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPdcPayable_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (PDCPayableRegisterObj != null)
                {
                    PDCPayableRegisterObj.Enabled = true;
                    PDCPayableRegisterObj.GridSearchRegister();
                }
                if (PDCpayableReportObj != null)
                {
                    PDCpayableReportObj.Enabled = true;
                    PDCpayableReportObj.Search();
                }
                if (BillallocationObj != null)
                {
                    BillallocationObj.Enabled = true;
                    BillallocationObj.BillAllocationGridFill();
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
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Activate();
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.LedgerDetailsView();
                    frmLedgerDetailsObj = null;
                }
                if (frmVoucherSearch != null)
                {
                    frmVoucherSearch.Enabled = true;
                    frmVoucherSearch.GridFill();
                }
                if (frmChequeReportObj != null)
                {
                    frmChequeReportObj.Enabled = true;
                    frmChequeReportObj.ChequeReportFillGrid();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP48:" + ex.Message;
            }
        }
        /// <summary>
        /// To add new ledger from these form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccountLedger2_Click(object sender, EventArgs e)
        {
            try
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
                    frmAccountLedgerObj.CallThisFormFromPDCPayable2(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallThisFormFromPDCPayable2(this);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP49:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "PP50:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter key navigation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtvoucherNo_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PP51:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPdcPayable_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PP52:" + ex.Message;
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
                        if (!txtvoucherNo.ReadOnly)
                        {
                            txtvoucherNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP53:" + ex.Message;
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
                    if (btnAgainstRef.Enabled == true)
                    {
                        btnAgainstRef.Focus();
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
                    if (cmbAccountLedger.Text == "" || cmbAccountLedger.SelectionStart == 0)
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
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (cmbAccountLedger.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromPdcpayableVoucher(this, Convert.ToDecimal(cmbAccountLedger.SelectedValue.ToString()), "Account Ledger");
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
                formMDI.infoError.ErrorString = "PP54:" + ex.Message;
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
                        if (btnAgainstRef.Enabled == true)
                            btnAgainstRef.Focus();
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
                formMDI.infoError.ErrorString = "PP55:" + ex.Message;
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
                    if (cmbBank.SelectedIndex != -1)
                    {
                        frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromPdcpayableVoucher(this, Convert.ToDecimal(cmbBank.SelectedValue.ToString()), "Bank");
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
                formMDI.infoError.ErrorString = "PP56:" + ex.Message;
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
                    txtChequeDate.Focus();
                    txtChequeDate.SelectionStart = 0;
                    txtChequeDate.SelectionLength = 0;
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
                formMDI.infoError.ErrorString = "PP57:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChequeDate_KeyDown(object sender, KeyEventArgs e)
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
                    if (txtChequeDate.Text == string.Empty || txtChequeDate.SelectionStart == 0)
                    {
                        txtcheckNo.Focus();
                        txtcheckNo.SelectionStart = 0;
                        txtcheckNo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP58:" + ex.Message;
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
                        txtChequeDate.Focus();
                        txtChequeDate.SelectionStart = 0;
                        txtChequeDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PP59:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "PP60:" + ex.Message;
            }
        }

        #endregion
    }
}
