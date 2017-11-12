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
//along with this program.  If not, see <http://www.gnu.org/licenses/>

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
    public partial class frmPaymentVoucher : Form
    {

        #region Public variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        frmCurrencyDetails frmCurrencyObj = new frmCurrencyDetails();//to use in call from currency function
        frmPartyBalance frmPartyBalanceObj = new frmPartyBalance();//to use in call from perty balance function
        frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();//to use in call from ledger popup function
        frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();//to use in call from account ledger function
        frmPaymentRegister frmPaymentRegisterObj = null;//to use in call from payment register function
        frmPaymentReport frmPaymentReportObj = null;//to use in call from payment report function
        frmDayBook frmDayBookObj = null;//to use in call from daybook function
        frmAgeingReport frmAgeingObj = null;//to use in call from Ageing function
        frmChequeReport frmChequeReportObj = null;// To use in call fromCheque Report function
        DataTable dtblPartyBalance = new DataTable();//to store party balance entries while clicking btn_Save in payment voucher
        SettingsSP spSettings = new SettingsSP();//to select data from settings table
        string strVoucherNo = string.Empty;//to save voucher no into tbl_payment master
        string strInvoiceNo = string.Empty;//to save invoice no into tbl_payment master 
        string tableName = "PaymentMaster";//to get the table name in voucher type selection
        string strCashOrBank;//to get the selected value in cmbBankOrCash at teh time of ledger popup
        string strPrefix = string.Empty;//to get the prefix string from frmvouchertypeselection
        string strSuffix = string.Empty;//to get the suffix string from frmvouchertypeselection
        decimal decPaymentVoucherTypeId = 0;//to get the selected voucher type id from frmVoucherTypeSelection
        decimal decDailySuffixPrefixId = 0;//to store the selected voucher type's suffixpreffixid from frmVoucherTypeSelection
        decimal decSelectedCurrencyRate = 0;//to store the selected currency rate
        decimal decAmount = 0;//to find the total amount 
        decimal decConvertRate = 0;//to find the amont after converted into converted rate by multiplying with exchange rate
        decimal decPaymentmasterId = 0;//to get the payment master id from payment register
        public string strVocherNo;
        bool isAutomatic = false;//to check whether the voucher number is automatically generated or not
        bool isUpdated = false;//to check wheteher the using mode is save or update
        bool isValueChanged = false;//to check column missing
        int inArrOfRemoveIndex = 0;//number of rows removed by clicking remove button
        int inNarrationCount = 0;
        ArrayList arrlstOfDeletedPartyBalanceRow;
        ArrayList arrlstOfRemovedLedgerPostingId = new ArrayList();
        ArrayList arrlstOfRemove = new ArrayList();
        frmBillallocation frmBillallocationObj = null;
        frmVoucherSearch _frmVoucherSearch = null;
        int inUpdatingRowIndexForPartyRemove = -1;
        decimal decUpdatingLedgerForPartyremove = 0;
        frmLedgerDetails frmLedgerDetailsObj = null;//To Use in CallFromLedgerDetails
        #endregion

        #region Function
        /// <summary>
        /// Create an instance for frmPaymentVoucher Class
        /// </summary>
        public frmPaymentVoucher()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV1:" + ex.Message;
            }
        }
        /// <summary>
        /// Printing checkbox status checking
        /// </summary>
        public void PrintCheck()
        {
            try
            {
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
                formMDI.infoError.ErrorString = "PV2:" + ex.Message;
            }
        }
        /// <summary>
        /// Serial no generation function for grid
        /// </summary>
        public void SerialNumberGeneration()
        {
            try
            {
                int inRowSlNo = 1;
                foreach (DataGridViewRow dr in dgvPaymentVoucher.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowSlNo;
                    inRowSlNo++;
                    if (dr.Index == dgvPaymentVoucher.Rows.Count - 2)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmCurrencyDetails form
        /// </summary>
        /// <param name="frmCurrencyDetails"></param>
        /// <param name="decId"></param>
        public void CallFromCurrenCyDetails(frmCurrencyDetails frmCurrencyDetails, decimal decId) //PopUp
        {
            try
            {
                base.Show();
                this.frmCurrencyObj = frmCurrencyDetails;
                GridCurrencyComboFill();
                dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value = decId;
                frmCurrencyObj.Close();
                frmCurrencyObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV4:" + ex.Message;
            }
        }
        /// <summary>
        /// Currency Combofill function
        /// </summary>
        public void GridCurrencyComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                dtbl = Obj.CurrencyComboByDate(Convert.ToDateTime(txtDate.Text));
                DataRow dr = dtbl.NewRow();
                dr["exchangeRateId"] = "0";
                dr["currencyName"] = string.Empty;
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
                formMDI.infoError.ErrorString = "PV5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmLedgerPopup form to select and view Ledger
        /// </summary>
        /// <param name="frmLedgerPopup"></param>
        /// <param name="decId"></param>
        /// <param name="str"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId, string str)
        {
            try
            {
                this.Enabled = true;
                if (str == "CashOrBank")
                {
                    TransactionsGeneralFill obj = new TransactionsGeneralFill();
                    obj.CashOrBankComboFill(cmbBankorCash, false);
                    cmbBankorCash.SelectedValue = decId;
                    cmbBankorCash.Focus();
                }
                else
                {
                    dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value = decId;
                    dgvPaymentVoucher.Focus();
                }
                frmLedgerPopupObj.Close();
                frmLedgerPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV6:" + ex.Message;
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
                this._frmVoucherSearch = frm;
                decPaymentmasterId = decId;
                btnClear.Text = "New";
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                FillFunction();
                this.Activate();
                this.BringToFront();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPartyBalance to view details and for updation 
        /// </summary>
        /// <param name="frmPartyBalance"></param>
        /// <param name="decId"></param>
        /// <param name="dtbl"></param>
        /// <param name="arrlstOfRemovedRow"></param>
        public void CallFromPartyBalance(frmPartyBalance frmPartyBalance, decimal decId, DataTable dtbl, ArrayList arrlstOfRemovedRow)
        {
            try
            {
                this.frmPartyBalanceObj = frmPartyBalance;
                dgvPaymentVoucher.CurrentRow.Cells["dgvtxtAmount"].Value = decId.ToString();
                frmPartyBalanceObj.Close();
                frmPartyBalanceObj = null;
                dtblPartyBalance = dtbl;
                arrlstOfDeletedPartyBalanceRow = arrlstOfRemovedRow;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV8:" + ex.Message;
            }
        }
        /// <summary>
        /// To reset the form here and Generate the voucher no generation
        /// </summary>
        public void Clear()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                if (btnSave.Text == "Update")
                {
                    if (frmPaymentRegisterObj != null)
                    {
                        frmPaymentRegisterObj.Close();
                    }
                }
                if (isAutomatic)
                {

                    SalaryVoucherMasterSP spMaster = new SalaryVoucherMasterSP();
                    PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                    if (strVoucherNo == string.Empty)
                    {
                        strVoucherNo = "0";
                    }
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPaymentVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, tableName);
                    if (Convert.ToDecimal(strVoucherNo) != SpPaymentMaster.PaymentMasterMax(decPaymentVoucherTypeId) + 1)
                    {
                        strVoucherNo = SpPaymentMaster.PaymentMasterMax(decPaymentVoucherTypeId).ToString();
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPaymentVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, tableName);
                        if (SpPaymentMaster.PaymentMasterMax(decPaymentVoucherTypeId) == 0)
                        {
                            strVoucherNo = "0";
                            strVoucherNo = obj.VoucherNumberAutomaicGeneration(decPaymentVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, tableName);
                        }
                    }
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPaymentVoucherTypeId, dtpDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    strInvoiceNo = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.Text = strInvoiceNo;
                    txtVoucherNo.ReadOnly = true;
                }
                else
                {
                    txtVoucherNo.Text = string.Empty;
                    txtVoucherNo.ReadOnly = false;
                }
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                dtpDate.Value = PublicVariables._dtCurrentDate;
                cmbBankorCash.SelectedIndex = -1;
                txtNarration.Text = string.Empty;
                txtTotal.Text = string.Empty;
                dgvPaymentVoucher.ClearSelection();
                dgvPaymentVoucher.Rows.Clear();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                cbxPrintafterSave.Checked = false;
                dtblPartyBalance.Clear();
                if (isAutomatic)
                {
                    txtDate.Select();
                }
                else
                {
                    txtVoucherNo.Select();
                }
                PrintCheck();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV9:" + ex.Message;
            }
        }
        /// <summary>
        /// Print function
        /// </summary>
        /// <param name="decPaymentMasterId"></param>
        public void Print(decimal decPaymentMasterId)
        {
            try
            {
                PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                DataSet dsPaymentVoucher = SpPaymentMaster.PaymentVoucherPrinting(decPaymentMasterId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.PaymentVoucherPrinting(dsPaymentVoucher);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV10:" + ex.Message;
            }
        }
        /// <summary>
        /// Dotmatrix print function add the details to fill here
        /// </summary>
        /// <param name="decPaymentMasterId"></param>
        public void PrintForDotMatrix(decimal decPaymentMasterId)
        {
            try
            {

                DataTable dtblOtherDetails = new DataTable();
                CompanySP spComapany = new CompanySP();
                dtblOtherDetails = spComapany.CompanyViewForDotMatrix();
                DataTable dtblGridDetails = new DataTable();
                dtblGridDetails.Columns.Add("SlNo");
                dtblGridDetails.Columns.Add("Account Ledger");
                dtblGridDetails.Columns.Add("Amount");
                dtblGridDetails.Columns.Add("Currency");
                dtblGridDetails.Columns.Add("Cheque No");
                dtblGridDetails.Columns.Add("Cheque Date");
                int inRowCount = 0;
                foreach (DataGridViewRow dRow in dgvPaymentVoucher.Rows)
                {
                    if (dRow.HeaderCell.Value != null && dRow.HeaderCell.Value.ToString() != "X")
                    {
                        if (!dRow.IsNewRow)
                        {
                            DataRow dr = dtblGridDetails.NewRow();
                            dr["SlNo"] = ++inRowCount;
                            dr["Account Ledger"] = dRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString();
                            dr["Amount"] = dRow.Cells["dgvtxtAmount"].Value.ToString();
                            dr["Currency"] = dRow.Cells["dgvcmbCurrency"].FormattedValue.ToString();
                            dr["Cheque No"] = (dRow.Cells["dgvtxtChequeNo"].Value == null ? "" : dRow.Cells["dgvtxtChequeNo"].Value.ToString());
                            dr["Cheque Date"] = (dRow.Cells["dgvtxtChequeDate"].Value == null ? "" : dRow.Cells["dgvtxtChequeDate"].Value.ToString());
                            dtblGridDetails.Rows.Add(dr);
                        }
                    }
                }
                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("totalAmount");
                dtblOtherDetails.Columns.Add("ledgerName");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("AmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtVoucherNo.Text;
                dRowOther["date"] = txtDate.Text;
                dRowOther["totalAmount"] = txtTotal.Text;
                dRowOther["ledgerName"] = cmbBankorCash.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtTotal.Text), PublicVariables._decCurrencyId);
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decPaymentVoucherTypeId);
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
                formMDI.infoError.ErrorString = "PV11:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete party balance entry while click remove button
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
                formMDI.infoError.ErrorString = "PV12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from VoucherType Selection form
        /// </summary>
        /// <param name="decPaymentVoucherTypeId1"></param>
        /// <param name="strVoucherTypeNames1"></param>
        public void CallFromVoucherTypeSelection(decimal decPaymentVoucherTypeId1, string strVoucherTypeNames1)
        {
            try
            {
                decPaymentVoucherTypeId = decPaymentVoucherTypeId1;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPaymentVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPaymentVoucherTypeId, dtpDate.Value);
                decDailySuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                strPrefix = infoSuffixPrefix.Prefix;
                strSuffix = infoSuffixPrefix.Suffix;
                base.Show();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV13:" + ex.Message;
            }
        }
        /// <summary>
        /// Save or edit function, checking invalid entries to save or update
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                int inIfGridColumnMissing = 0;
                int inRowCount = dgvPaymentVoucher.RowCount;
                ArrayList arrLst = new ArrayList();
                string output = string.Empty;
                if (txtVoucherNo.Text == string.Empty)
                {
                    Messages.InformationMessage("Enter voucher number.");
                    txtVoucherNo.Focus();
                    inIfGridColumnMissing = 1;
                }
                else if (cmbBankorCash.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select any bank or cash");
                    cmbBankorCash.Focus();
                    inIfGridColumnMissing = 1;
                }
                else if (inRowCount == 1)
                {
                    Messages.InformationMessage("Can't save without atleast one complete details");
                    dgvPaymentVoucher.Focus();
                    inIfGridColumnMissing = 1;
                }
                else if (Convert.ToDecimal(txtTotal.Text) == 0)
                {
                    Messages.InformationMessage("Can't save the total amount as Zero");
                    dgvPaymentVoucher.Focus();
                }
                else
                {
                    int inJ = 0;
                    for (int inI = 0; inI < inRowCount - 1; inI++)
                    {

                        if (dgvPaymentVoucher.Rows[inI].HeaderCell.Value.ToString() == "X")
                        {
                            arrLst.Add(Convert.ToString(inI + 1));
                            inIfGridColumnMissing = 1;
                            inJ++;
                        }
                    }
                    if (inJ != 0)
                    {
                        if (inJ == inRowCount - 1)
                        {
                            Messages.InformationMessage("Can't save without atleat one complete details");
                            inIfGridColumnMissing = 1;
                        }
                        else
                        {
                            foreach (object obj in arrLst)
                            {
                                string str = Convert.ToString(obj);
                                if (str != null)
                                {
                                    output += str + ",";
                                }
                                else
                                {
                                    break;
                                }
                            }
                            bool isOk = Messages.UpdateMessageCustom("Row No " + output + " not completed.Do you want to continue?");
                            if (isOk)
                            {
                                inIfGridColumnMissing = 0;
                            }
                            else
                            {
                                inIfGridColumnMissing = 1;
                            }
                        }
                    }

                    if (inIfGridColumnMissing == 0)
                    {
                        if (btnSave.Text == "Save")
                        {
                            if (!isAutomatic)
                            {
                                if (SpPaymentMaster.PaymentVoucherCheckExistence(txtVoucherNo.Text.Trim(), decPaymentVoucherTypeId, 0))
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                                else
                                {
                                    if (PublicVariables.isMessageAdd)
                                    {
                                        if (Messages.SaveMessage())
                                        {
                                            Save();
                                        }
                                    }
                                    else
                                    {
                                        Save();
                                    }
                                }
                            }
                            else
                            {
                                if (PublicVariables.isMessageAdd)
                                {
                                    if (Messages.SaveMessage())
                                    {
                                        Save();
                                    }
                                }
                                else
                                {
                                    Save();
                                }
                            }
                        }
                        else if (btnSave.Text == "Update")
                        {
                            if (!isAutomatic)
                            {
                                if (SpPaymentMaster.PaymentVoucherCheckExistence(txtVoucherNo.Text.Trim(), decPaymentVoucherTypeId, decPaymentmasterId))
                                {
                                    Messages.InformationMessage("Voucher number already exist");
                                }
                                else
                                {
                                    if (PublicVariables.isMessageEdit)
                                    {
                                        if (Messages.UpdateMessage())
                                        {
                                            Edit(decPaymentmasterId);
                                        }
                                    }
                                    else
                                    {
                                        Edit(decPaymentmasterId);
                                    }
                                }
                            }
                            else
                            {
                                if (PublicVariables.isMessageEdit)
                                {
                                    if (Messages.UpdateMessage())
                                    {
                                        Edit(decPaymentmasterId);
                                    }
                                }
                                else
                                {
                                    Edit(decPaymentmasterId);
                                }
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV14:" + ex.Message;
            }
        }
        /// <summary>
        /// Party balance Save Or Edit
        /// </summary>
        /// <param name="inJ"></param>
        public void PartyBalanceAddOrEdit(int inJ)
        {
            try
            {


                int inTableRowCount = dtblPartyBalance.Rows.Count;
                PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                InfopartyBalance.Credit = 0;
                InfopartyBalance.CreditPeriod = 0;
                InfopartyBalance.Date = dtpDate.Value;
                InfopartyBalance.Debit = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["Amount"].ToString());
                InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["CurrencyId"].ToString());
                InfopartyBalance.Extra1 = string.Empty;
                InfopartyBalance.Extra2 = string.Empty;
                InfopartyBalance.ExtraDate = DateTime.Now;
                InfopartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                InfopartyBalance.LedgerId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["LedgerId"].ToString());
                InfopartyBalance.ReferenceType = dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString();
                if (dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "New" || dtblPartyBalance.Rows[inJ]["ReferenceType"].ToString() == "OnAccount")
                {
                    InfopartyBalance.AgainstInvoiceNo = "0";
                    InfopartyBalance.AgainstVoucherNo ="0";
                    InfopartyBalance.AgainstVoucherTypeId = 0;
                    InfopartyBalance.VoucherTypeId = decPaymentVoucherTypeId;
                    InfopartyBalance.InvoiceNo = strInvoiceNo;
                    if (!isAutomatic)
                    {
                        InfopartyBalance.VoucherNo = txtVoucherNo.Text.Trim();
                    }
                    else
                    {
                        InfopartyBalance.VoucherNo = strVoucherNo;
                    }
                }
                else
                {
                    InfopartyBalance.ExchangeRateId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["OldExchangeRate"].ToString());
                    InfopartyBalance.AgainstInvoiceNo = strInvoiceNo;
                    if (!isAutomatic)
                    {
                        InfopartyBalance.AgainstVoucherNo = txtVoucherNo.Text.Trim();
                    }
                    else
                    {
                        InfopartyBalance.AgainstVoucherNo = strVoucherNo;
                    }
                    InfopartyBalance.AgainstVoucherTypeId = decPaymentVoucherTypeId;
                    InfopartyBalance.VoucherTypeId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["AgainstVoucherTypeId"].ToString());
                    InfopartyBalance.VoucherNo = dtblPartyBalance.Rows[inJ]["AgainstVoucherNo"].ToString();
                    InfopartyBalance.InvoiceNo = dtblPartyBalance.Rows[inJ]["AgainstInvoiceNo"].ToString();
                }
                if (dtblPartyBalance.Rows[inJ]["PartyBalanceId"].ToString() == "0")
                {
                    spPartyBalance.PartyBalanceAdd(InfopartyBalance);
                }
                else
                {
                    InfopartyBalance.PartyBalanceId = Convert.ToDecimal(dtblPartyBalance.Rows[inJ]["partyBalanceId"]);
                    spPartyBalance.PartyBalanceEdit(InfopartyBalance);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV15:" + ex.Message;
            }
        }
        /// <summary>
        /// TotalAmountCalculation
        /// </summary>
        /// <returns></returns>
        public decimal TotalAmountCalculation()
        {
            decimal decTotal = 0;
            decimal decSelectedCurrencyRate = 0;
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            try
            {
                for (int inI = 0; inI < dgvPaymentVoucher.RowCount - 1; inI++)
                {

                    if (dgvPaymentVoucher.Rows[inI].HeaderCell.Value.ToString() != "X")
                    {
                        decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString()));//Exchange rate of grid's row
                        decTotal = decTotal + (Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString()) * decSelectedCurrencyRate);
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV16:" + ex.Message;
            }
            return decTotal;
        }
        /// <summary>
        /// Save Function
        /// </summary>
        public void Save()
        {
            try
            {
                int inGridRowCount = dgvPaymentVoucher.RowCount;
                int inTableRowCount = dtblPartyBalance.Rows.Count;
                int inB = 0;
                PaymentMasterInfo InfoPaymentMaster = new PaymentMasterInfo();
                PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                PaymentDetailsInfo InfoPaymentDetails = new PaymentDetailsInfo();
                PaymentDetailsSP SpPaymentDetails = new PaymentDetailsSP();
                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
                DateValidation objVal = new DateValidation();
                TextBox txtDate1 = new TextBox();
                InfoPaymentMaster.Date = dtpDate.Value;
                InfoPaymentMaster.Extra1 = string.Empty;
                InfoPaymentMaster.Extra2 = string.Empty;
                InfoPaymentMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                InfoPaymentMaster.InvoiceNo = txtVoucherNo.Text.Trim(); ;
                InfoPaymentMaster.LedgerId = Convert.ToDecimal(cmbBankorCash.SelectedValue.ToString());
                InfoPaymentMaster.Narration = txtNarration.Text.Trim();
                InfoPaymentMaster.SuffixPrefixId = decDailySuffixPrefixId;
                decimal decTotalAmount = TotalAmountCalculation();
                InfoPaymentMaster.TotalAmount = decTotalAmount;
                InfoPaymentMaster.UserId = PublicVariables._decCurrentUserId;
                InfoPaymentMaster.VoucherNo = strVoucherNo;
                InfoPaymentMaster.VoucherTypeId = decPaymentVoucherTypeId;
                decimal decPaymentMasterId = SpPaymentMaster.PaymentMasterAdd(InfoPaymentMaster);
                if (decPaymentMasterId != 0)
                {
                    MasterLedgerPosting();
                }
                for (int inI = 0; inI < inGridRowCount - 1; inI++)
                {
                    if (dgvPaymentVoucher.Rows[inI].HeaderCell.Value.ToString() != "X")
                    {
                        InfoPaymentDetails.Amount = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                        InfoPaymentDetails.ExchangeRateId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString());
                        InfoPaymentDetails.Extra1 = string.Empty;
                        InfoPaymentDetails.Extra2 = string.Empty;
                        InfoPaymentDetails.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                        InfoPaymentDetails.PaymentMasterId = decPaymentMasterId;
                        if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                        {
                            InfoPaymentDetails.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                        if (dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                        {
                            InfoPaymentDetails.ChequeNo = dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();

                            if (dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                            {
                                InfoPaymentDetails.ChequeDate = Convert.ToDateTime(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value);
                            }
                            else
                            {
                                InfoPaymentDetails.ChequeDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            InfoPaymentDetails.ChequeNo = string.Empty;
                            InfoPaymentDetails.ChequeDate = DateTime.Now;
                        }
                        decimal decPaymentDetailsId = SpPaymentDetails.PaymentDetailsAdd(InfoPaymentDetails);
                        if (decPaymentDetailsId != 0)
                        {
                            for (int inJ = 0; inJ < inTableRowCount; inJ++)
                            {
                                if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() == dtblPartyBalance.Rows[inJ]["LedgerId"].ToString())
                                {
                                    PartyBalanceAddOrEdit(inJ);
                                }
                            }
                            inB++;
                            DetailsLedgerPosting(inI, decPaymentDetailsId);
                        }
                    }
                }
                Messages.SavedMessage();
                if (cbxPrintafterSave.Checked)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(decPaymentmasterId);
                    }
                    else
                    {
                        Print(decPaymentMasterId);
                    }
                }
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV17:" + ex.Message;
            }
        }
        /// <summary>
        /// Edit Function
        /// </summary>
        /// <param name="decMasterId"></param>
        public void Edit(decimal decMasterId)
        {
            try
            {
                int inRowCount = dgvPaymentVoucher.RowCount;
                int inTableRowCount = dtblPartyBalance.Rows.Count;
                int inB = 0;
                PaymentMasterInfo InfoPaymentMaster = new PaymentMasterInfo();
                PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                PaymentDetailsInfo InfoPaymentDetails = new PaymentDetailsInfo();
                PaymentDetailsSP SpPaymentDetails = new PaymentDetailsSP();
                LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo InfoLegerPosting = new LedgerPostingInfo();
                PartyBalanceInfo InfopartyBalance = new PartyBalanceInfo();
                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                BankReconciliationSP SpBankReconcilation = new BankReconciliationSP();
                InfoPaymentMaster.Date = dtpDate.Value;
                InfoPaymentMaster.PaymentMasterId = decMasterId;
                InfoPaymentMaster.Extra1 = string.Empty;
                InfoPaymentMaster.Extra2 = string.Empty;
                InfoPaymentMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                InfoPaymentMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                InfoPaymentMaster.LedgerId = Convert.ToDecimal(cmbBankorCash.SelectedValue.ToString());
                InfoPaymentMaster.Narration = txtNarration.Text.Trim();
                InfoPaymentMaster.SuffixPrefixId = decDailySuffixPrefixId;
                decimal decTotalAmount = TotalAmountCalculation();
                InfoPaymentMaster.TotalAmount = decTotalAmount;
                InfoPaymentMaster.UserId = PublicVariables._decCurrentUserId;

                InfoPaymentMaster.VoucherNo = strVoucherNo;

                InfoPaymentMaster.VoucherTypeId = decPaymentVoucherTypeId;
                decimal decPaymentMasterId = SpPaymentMaster.PaymentMasterEdit(InfoPaymentMaster);
                if (decPaymentmasterId != 0)
                {
                    MasterLedgerPostingEdit();
                }
                foreach (object obj in arrlstOfRemove)
                {
                    string str = Convert.ToString(obj);
                    SpPaymentDetails.PaymentDetailsDelete(Convert.ToDecimal(str));
                    SpLedgerPosting.LedgerPostDeleteByDetailsId(Convert.ToDecimal(str), strVoucherNo, decPaymentVoucherTypeId);
                }
                SpLedgerPosting.LedgerPostingDeleteByVoucherNoVoucherTypeIdAndLedgerId(strVoucherNo, decPaymentVoucherTypeId, 12);
                decimal decPaymentDetailsId1 = 0;
                for (int inI = 0; inI < inRowCount - 1; inI++)
                {
                    InfoPaymentDetails.Amount = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                    InfoPaymentDetails.ExchangeRateId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value.ToString());
                    InfoPaymentDetails.Extra1 = string.Empty;
                    InfoPaymentDetails.Extra2 = string.Empty;
                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        InfoPaymentDetails.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString());
                    }
                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        InfoPaymentDetails.ChequeNo = dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            InfoPaymentDetails.ChequeDate = Convert.ToDateTime(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value);
                        }
                        else
                        {
                            InfoPaymentDetails.ChequeDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        InfoPaymentDetails.ChequeNo = string.Empty;
                        InfoPaymentDetails.ChequeDate = DateTime.Now;
                    }
                    InfoPaymentDetails.PaymentMasterId = decPaymentMasterId;
                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvtxtpaymentDetailsId"].FormattedValue.ToString() == "0")//if new rows are added
                    {
                        if (dgvPaymentVoucher.Rows[inI].HeaderCell.Value != null && dgvPaymentVoucher.Rows[inI].HeaderCell.Value.ToString() != "X")//add new rows added which are completed
                        {
                            decimal decPaymentDetailsId = SpPaymentDetails.PaymentDetailsAdd(InfoPaymentDetails);//to add new rows in payment details
                            if (decPaymentDetailsId != 0)
                            {
                                for (int inJ = 0; inJ < inTableRowCount; inJ++)
                                {
                                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() == dtblPartyBalance.Rows[inJ]["LedgerId"].ToString())
                                    {
                                        PartyBalanceAddOrEdit(inJ);
                                    }
                                }
                                inB++;
                                DetailsLedgerPosting(inI, decPaymentDetailsId);//to add new ledger posting
                            }
                        }
                    }
                    else
                    {
                        if (dgvPaymentVoucher.Rows[inI].HeaderCell.Value.ToString() != "X")//add new rows updated which are completed
                        {
                            InfoPaymentDetails.PaymentDetailsId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtpaymentDetailsId"].Value.ToString());
                            decimal decPaymentDetailsId = SpPaymentDetails.PaymentDetailsEdit(InfoPaymentDetails);//to edit rows
                            if (decPaymentDetailsId != 0)
                            {
                                for (int inJ = 0; inJ < inTableRowCount; inJ++)
                                {
                                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() == dtblPartyBalance.Rows[inJ]["LedgerId"].ToString())
                                    {
                                        PartyBalanceAddOrEdit(inJ);
                                    }
                                }
                                inB++;
                                decPaymentDetailsId1 = InfoPaymentDetails.PaymentDetailsId;
                                decimal decLedgerPostId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                DetailsLedgerPostingEdit(inI, decLedgerPostId, decPaymentDetailsId1);
                            }
                        }
                        else
                        {
                            decimal decDetailsId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inI].Cells["dgvtxtpaymentDetailsId"].Value.ToString());
                            SpPaymentDetails.PaymentDetailsDelete(decDetailsId);
                            SpLedgerPosting.LedgerPostDeleteByDetailsId(decDetailsId, strVoucherNo, decPaymentVoucherTypeId);
                            for (int inJ = 0; inJ < dtblPartyBalance.Rows.Count; inJ++)
                            {
                                if (dtblPartyBalance.Rows.Count == inJ)
                                {
                                    break;
                                }
                                if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                {
                                    if (dtblPartyBalance.Rows[inJ]["LedgerId"].ToString() == dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString())
                                    {
                                        if (dtblPartyBalance.Rows[inJ]["PartyBalanceId"].ToString() != "0")
                                        {
                                            arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inJ]["PartyBalanceId"]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                DeletePartyBalanceOfRemovedRow();
                isUpdated = true;
                Messages.UpdatedMessage();
                if (cbxPrintafterSave.Checked)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(decPaymentmasterId);
                    }
                    else
                    {
                        Print(decPaymentMasterId);
                    }
                }
                if (frmPaymentRegisterObj != null)
                {
                    this.Close();
                    frmPaymentRegisterObj.CallFromPaymentVoucher(this);
                }
                if (frmPaymentReportObj != null)
                {
                    this.Close();
                    frmPaymentReportObj.CallFromPaymentVoucher(this);
                }
                if (frmDayBookObj != null)
                {
                    this.Close();
                }
                if (frmBillallocationObj != null)
                {
                    this.Close();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV18:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete Function
        /// </summary>
        /// <param name="decMasterId"></param>
        public void Delete(decimal decMasterId)
        {
            try
            {
                PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();

                if (!SpPartyBalance.PartyBalanceCheckReference(decPaymentVoucherTypeId, strVoucherNo))
                {
                    SpPaymentMaster.PaymentVoucherDelete(decPaymentmasterId, decPaymentVoucherTypeId, strVoucherNo);
                    Messages.DeletedMessage();
                    if (frmPaymentRegisterObj != null)
                    {
                        this.Close();
                        frmPaymentRegisterObj.CallFromPaymentVoucher(this);
                    }
                    else if (frmPaymentReportObj != null)
                    {
                        this.Close();
                        frmPaymentReportObj.CallFromPaymentVoucher(this);
                    }
                    else if (frmLedgerDetailsObj != null)
                    {
                        this.Close();
                    }
                    if (_frmVoucherSearch != null)
                    {
                        this.Close();
                        _frmVoucherSearch.GridFill();
                    }
                    if (frmDayBookObj != null)
                    {
                        this.Close();
                    }
                    if (frmBillallocationObj != null)
                    {
                        this.Close();
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
                formMDI.infoError.ErrorString = "PV19:" + ex.Message;
            }
        }
        /// <summary>
        /// Total Amount Calculation
        /// </summary>
        public void TotalAmount()
        {
            try
            {
                decimal decTotalAmount = 0;
                decimal decSelectedCurrencyRate = 0;
                ExchangeRateSP SpExchangRate = new ExchangeRateSP();
                foreach (DataGridViewRow dr in dgvPaymentVoucher.Rows)
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
                txtTotal.Text = Math.Round(decTotalAmount, Convert.ToInt32(PublicVariables._inNoOfDecimalPlaces.ToString())).ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV20:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger Posting Add
        /// </summary>
        public void MasterLedgerPosting()
        {
            try
            {
                LedgerPostingInfo InfoLedgerPosting = new LedgerPostingInfo();
                LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
                ExchangeRateSP SpExchangRate = new ExchangeRateSP();
                InfoLedgerPosting.Credit = Convert.ToDecimal(txtTotal.Text);
                InfoLedgerPosting.Date = dtpDate.Value;
                InfoLedgerPosting.Debit = 0;
                InfoLedgerPosting.DetailsId = 0;
                InfoLedgerPosting.Extra1 = string.Empty;
                InfoLedgerPosting.Extra2 = string.Empty;
                InfoLedgerPosting.InvoiceNo = strInvoiceNo;
                InfoLedgerPosting.ChequeNo = string.Empty;
                InfoLedgerPosting.ChequeDate = DateTime.Now;
                InfoLedgerPosting.LedgerId = Convert.ToDecimal(cmbBankorCash.SelectedValue.ToString());
                if (!isAutomatic)
                {
                    InfoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                }
                else
                {
                    InfoLedgerPosting.VoucherNo = strVoucherNo;
                }
                InfoLedgerPosting.VoucherTypeId = decPaymentVoucherTypeId;
                InfoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                SpLedgerPosting.LedgerPostingAdd(InfoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV21:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger Posting Edit
        /// </summary>
        public void MasterLedgerPostingEdit()
        {
            try
            {
                LedgerPostingInfo InfoLedgerPosting = new LedgerPostingInfo();
                LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
                ExchangeRateSP SpExchangRate = new ExchangeRateSP();
                InfoLedgerPosting.Credit = Convert.ToDecimal(txtTotal.Text);
                InfoLedgerPosting.Date = dtpDate.Value;
                InfoLedgerPosting.Debit = 0;
                InfoLedgerPosting.DetailsId = 0;
                InfoLedgerPosting.Extra1 = string.Empty;
                InfoLedgerPosting.Extra2 = string.Empty;
                InfoLedgerPosting.InvoiceNo = strInvoiceNo;
                InfoLedgerPosting.ChequeNo = string.Empty;
                InfoLedgerPosting.ChequeDate = DateTime.Now;
                InfoLedgerPosting.LedgerId = Convert.ToDecimal(cmbBankorCash.SelectedValue.ToString());
                if (!isAutomatic)
                {
                    InfoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                }
                else
                {
                    InfoLedgerPosting.VoucherNo = strVoucherNo;
                }
                InfoLedgerPosting.VoucherTypeId = decPaymentVoucherTypeId;
                InfoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                SpLedgerPosting.LedgerPostingEditByVoucherTypeAndVoucherNo(InfoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV22:" + ex.Message;
            }
        }
        /// <summary>
        /// Ledger posting details add
        /// </summary>
        /// <param name="inA"></param>
        /// <param name="decPaymentDetailsId"></param>
        public void DetailsLedgerPosting(int inA, decimal decPaymentDetailsId)
        {
            LedgerPostingInfo InfoLedgerPosting = new LedgerPostingInfo();
            LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            decimal decOldExchange = 0;
            decimal decNewExchangeRate = 0;
            decimal decNewExchangeRateId = 0;
            decimal decOldExchangeId = 0;
            try
            {


                if (!dgvPaymentVoucher.Rows[inA].Cells["dgvtxtAmount"].ReadOnly)
                {
                    decimal d = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbCurrency"].Value.ToString());
                    decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbCurrency"].Value.ToString()));
                    decAmount = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvtxtAmount"].Value.ToString());
                    decConvertRate = decAmount * decSelectedCurrencyRate;
                    InfoLedgerPosting.Credit = 0;
                    InfoLedgerPosting.Date = dtpDate.Value;
                    InfoLedgerPosting.Debit = decConvertRate;
                    InfoLedgerPosting.DetailsId = decPaymentDetailsId;
                    InfoLedgerPosting.Extra1 = string.Empty;
                    InfoLedgerPosting.Extra2 = string.Empty;
                    InfoLedgerPosting.InvoiceNo = strInvoiceNo;

                    if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        InfoLedgerPosting.ChequeNo = dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            InfoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            InfoLedgerPosting.ChequeDate = DateTime.Now;

                    }
                    else
                    {
                        InfoLedgerPosting.ChequeNo = string.Empty;
                        InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }
                    InfoLedgerPosting.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString());
                    if (!isAutomatic)
                    {
                        InfoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                    }
                    else
                    {
                        InfoLedgerPosting.VoucherNo = strVoucherNo;
                    }
                    InfoLedgerPosting.VoucherTypeId = decPaymentVoucherTypeId;
                    InfoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    SpLedgerPosting.LedgerPostingAdd(InfoLedgerPosting);
                }
                else
                {
                    InfoLedgerPosting.Date = dtpDate.Value;

                    InfoLedgerPosting.Extra1 = string.Empty;
                    InfoLedgerPosting.Extra2 = string.Empty;
                    InfoLedgerPosting.InvoiceNo = strInvoiceNo;
                    InfoLedgerPosting.VoucherTypeId = decPaymentVoucherTypeId;
                    InfoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    InfoLedgerPosting.Credit = 0;
                    InfoLedgerPosting.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString());
                    InfoLedgerPosting.VoucherNo = strVoucherNo;
                    InfoLedgerPosting.DetailsId = decPaymentDetailsId;
                    InfoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                    if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        InfoLedgerPosting.ChequeNo = dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            InfoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }
                    else
                    {
                        InfoLedgerPosting.ChequeNo = string.Empty;
                        InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }

                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (InfoLedgerPosting.LedgerId == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            decOldExchange = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                            decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                            decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchange);
                            decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            decConvertRate = decConvertRate + (decAmount * decSelectedCurrencyRate);

                        }
                    }
                    InfoLedgerPosting.Debit = decConvertRate;
                    SpLedgerPosting.LedgerPostingAdd(InfoLedgerPosting);

                    InfoLedgerPosting.LedgerId = 12;
                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
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

                                    InfoLedgerPosting.Debit = decForexAmount;
                                    InfoLedgerPosting.Credit = 0;
                                }
                                else
                                {
                                    InfoLedgerPosting.Credit = -1 * decForexAmount;
                                    InfoLedgerPosting.Debit = 0;
                                }
                                SpLedgerPosting.LedgerPostingAdd(InfoLedgerPosting);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV23:" + ex.Message;
            }
        }
        /// <summary>
        /// Details ledger posting edit
        /// </summary>
        /// <param name="inA"></param>
        /// <param name="decLedgerPostingId"></param>
        /// <param name="decPaymentDetailsId"></param>
        public void DetailsLedgerPostingEdit(int inA, decimal decLedgerPostingId, decimal decPaymentDetailsId)
        {
            LedgerPostingInfo InfoLedgerPosting = new LedgerPostingInfo();
            LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
            ExchangeRateSP SpExchangRate = new ExchangeRateSP();
            decimal decOldExchange = 0;
            decimal decNewExchangeRate = 0;
            decimal decNewExchangeRateId = 0;
            decimal decOldExchangeId = 0;
            try
            {
                if (!dgvPaymentVoucher.Rows[inA].Cells["dgvtxtAmount"].ReadOnly)
                {
                    decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbCurrency"].Value.ToString()));
                    decAmount = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvtxtAmount"].Value.ToString());
                    decConvertRate = decAmount * decSelectedCurrencyRate;
                    InfoLedgerPosting.Credit = 0;
                    InfoLedgerPosting.Date = dtpDate.Value;
                    InfoLedgerPosting.Debit = decConvertRate;
                    InfoLedgerPosting.DetailsId = decPaymentDetailsId;
                    InfoLedgerPosting.Extra1 = string.Empty;
                    InfoLedgerPosting.Extra2 = string.Empty;
                    InfoLedgerPosting.InvoiceNo = strInvoiceNo;
                    InfoLedgerPosting.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString());
                    if (!isAutomatic)
                    {
                        InfoLedgerPosting.VoucherNo = txtVoucherNo.Text.Trim();
                    }
                    else
                    {
                        InfoLedgerPosting.VoucherNo = strVoucherNo;
                    }
                    if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        InfoLedgerPosting.ChequeNo = dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            InfoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }
                    else
                    {
                        InfoLedgerPosting.ChequeNo = string.Empty;
                        InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }

                    InfoLedgerPosting.VoucherTypeId = decPaymentVoucherTypeId;
                    InfoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    InfoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                    SpLedgerPosting.LedgerPostingEdit(InfoLedgerPosting);
                }
                else
                {
                    InfoLedgerPosting.Date = dtpDate.Value;

                    InfoLedgerPosting.Extra1 = string.Empty;
                    InfoLedgerPosting.Extra2 = string.Empty;
                    InfoLedgerPosting.InvoiceNo = strInvoiceNo;
                    InfoLedgerPosting.VoucherTypeId = decPaymentVoucherTypeId;
                    InfoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    InfoLedgerPosting.Credit = 0;
                    InfoLedgerPosting.LedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString());
                    InfoLedgerPosting.VoucherNo = strVoucherNo;
                    InfoLedgerPosting.DetailsId = decPaymentDetailsId;
                    InfoLedgerPosting.InvoiceNo = txtVoucherNo.Text.Trim();
                    if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        InfoLedgerPosting.ChequeNo = dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeNo"].Value.ToString();
                        if (dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value != null && dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString() != string.Empty)
                        {
                            InfoLedgerPosting.ChequeDate = Convert.ToDateTime(dgvPaymentVoucher.Rows[inA].Cells["dgvtxtChequeDate"].Value.ToString());
                        }
                        else
                            InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }
                    else
                    {
                        InfoLedgerPosting.ChequeNo = string.Empty;
                        InfoLedgerPosting.ChequeDate = DateTime.Now;
                    }

                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (InfoLedgerPosting.LedgerId == Convert.ToDecimal(dr["LedgerId"].ToString()))
                        {
                            decOldExchange = Convert.ToDecimal(dr["OldExchangeRate"].ToString());
                            decNewExchangeRateId = Convert.ToDecimal(dr["CurrencyId"].ToString());
                            decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(decOldExchange);
                            decAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            decConvertRate = decConvertRate + (decAmount * decSelectedCurrencyRate);

                        }
                    }
                    InfoLedgerPosting.Debit = decConvertRate;
                    InfoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                    SpLedgerPosting.LedgerPostingEdit(InfoLedgerPosting);
                    InfoLedgerPosting.LedgerId = 12;
                    foreach (DataRow dr in dtblPartyBalance.Rows)
                    {
                        if (Convert.ToDecimal(dgvPaymentVoucher.Rows[inA].Cells["dgvcmbAccountLedger"].Value.ToString()) == Convert.ToDecimal(dr["LedgerId"].ToString()))
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

                                    InfoLedgerPosting.Debit = decForexAmount;
                                    InfoLedgerPosting.Credit = 0;
                                }
                                else
                                {
                                    InfoLedgerPosting.Credit = -1 * decForexAmount;
                                    InfoLedgerPosting.Debit = 0;
                                }
                                SpLedgerPosting.LedgerPostingAdd(InfoLedgerPosting);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV24:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPaymentRegister to view details and for updation 
        /// </summary>
        /// <param name="frmPaymentRegister"></param>
        /// <param name="decmasterId"></param>
        public void CallFromPaymentRegister(frmPaymentRegister frmPaymentRegister, decimal decmasterId)
        {
            try
            {
                base.Show();
                this.frmPaymentRegisterObj = frmPaymentRegister;
                frmPaymentRegisterObj.Enabled = false;
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                decPaymentmasterId = decmasterId;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV25:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPaymentReport to view details and for updation 
        /// </summary>
        /// <param name="frmPaymentReport"></param>
        /// <param name="decmasterId"></param>
        public void CallFromPaymentReport(frmPaymentReport frmPaymentReport, decimal decmasterId)
        {
            try
            {
                base.Show();
                this.frmPaymentReportObj = frmPaymentReport;
                frmPaymentReportObj.Enabled = false;
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                decPaymentmasterId = decmasterId;

                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV26:" + ex.Message;
            }
        }
        /// <summary>
        /// Remove function when click Link button
        /// </summary>
        public void Remove()
        {
            try
            {
                if (dgvPaymentVoucher.CurrentRow != null)
                {
                    if (dgvPaymentVoucher.CurrentRow.Index != dgvPaymentVoucher.NewRowIndex)
                    {
                        if (Convert.ToInt32(dgvPaymentVoucher.CurrentRow.Cells["dgvtxtSlNo"].Value.ToString()) < dgvPaymentVoucher.RowCount)
                        {
                            if (btnSave.Text == "Update")
                            {
                                if (dgvPaymentVoucher.CurrentRow.Cells["dgvtxtpaymentDetailsId"].Value != null && dgvPaymentVoucher.CurrentRow.Cells["dgvtxtpaymentDetailsId"].Value.ToString() != string.Empty)
                                {
                                    arrlstOfRemove.Add(dgvPaymentVoucher.CurrentRow.Cells["dgvtxtpaymentDetailsId"].Value.ToString());
                                    arrlstOfRemovedLedgerPostingId.Add(dgvPaymentVoucher.CurrentRow.Cells["dgvtxtLedgerPostingId"].Value.ToString());
                                    inArrOfRemoveIndex++;
                                }
                            }
                            int inTableRowCount = dtblPartyBalance.Rows.Count;
                            for (int inI = 0; inI < inTableRowCount; inI++)
                            {
                                if (dtblPartyBalance.Rows.Count == inI)
                                {
                                    break;
                                }
                                if (dtblPartyBalance.Rows[inI]["LedgerId"].ToString() == dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString())
                                {
                                    if (dtblPartyBalance.Rows[inI]["PartyBalanceId"].ToString() != "0")
                                    {
                                        arrlstOfDeletedPartyBalanceRow.Add(dtblPartyBalance.Rows[inI]["PartyBalanceId"]);
                                    }
                                    dtblPartyBalance.Rows.RemoveAt(inI);
                                    inI--;
                                }
                            }
                            if (inUpdatingRowIndexForPartyRemove == dgvPaymentVoucher.CurrentRow.Index)
                            {
                                inUpdatingRowIndexForPartyRemove = -1;
                                decUpdatingLedgerForPartyremove = 0;
                            }
                            dgvPaymentVoucher.Rows.RemoveAt(this.dgvPaymentVoucher.CurrentRow.Index);
                            SerialNumberGeneration();
                            TotalAmount();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV27:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking Invalid entries
        /// </summary>
        public void CheckColumnMissing()
        {
            try
            {
                if (dgvPaymentVoucher.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue == null || dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvPaymentVoucher.CurrentRow.Cells["dgvtxtAmount"].Value == null || dgvPaymentVoucher.CurrentRow.Cells["dgvtxtAmount"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].FormattedValue == null || dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].FormattedValue.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Value = "X";
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            isValueChanged = true;
                            dgvPaymentVoucher.CurrentRow.HeaderCell.Value = string.Empty;
                        }
                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV28:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill BankorCash combobox while return from Ledger creation when creating new ledger 
        /// </summary>
        /// <param name="decId"></param>
        /// <param name="str"></param>
        public void ReturnFromAccountLedgerForm(decimal decId, string str)
        {
            try
            {
                if (str == "CashOrBank")
                {
                    if (decId != 0)
                    {
                        TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                        Obj.CashOrBankComboFill(cmbBankorCash, false);
                        cmbBankorCash.SelectedValue = decId.ToString();
                    }
                    cmbBankorCash.Focus();
                }
                else
                {
                    if (decId != 0)
                    {
                        int inCurrentRowIndex = dgvPaymentVoucher.CurrentRow.Index;
                        if (inCurrentRowIndex == dgvPaymentVoucher.Rows.Count - 1)
                        {
                            dgvPaymentVoucher.Rows.Add();
                        }
                        dgvPaymentVoucher.CurrentRow.HeaderCell.Value = "X";
                        dgvPaymentVoucher.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        DataTable dtbl = new DataTable();
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        dtbl = spAccountLedger.AccountLedgerViewAll();
                        DataGridViewComboBoxCell dgvccCashOrBank = (DataGridViewComboBoxCell)dgvPaymentVoucher[dgvPaymentVoucher.Columns["dgvcmbAccountLedger"].Index, dgvPaymentVoucher.Rows[inCurrentRowIndex].Index];
                        DataRow dr = dtbl.NewRow();
                        dr["ledgerId"] = "0";
                        dr["ledgerName"] = string.Empty;
                        dtbl.Rows.InsertAt(dr, 0);
                        dgvccCashOrBank.DataSource = dtbl;
                        dgvccCashOrBank.ValueMember = "ledgerId";
                        dgvccCashOrBank.DisplayMember = "ledgerName";
                        dgvPaymentVoucher.Rows[inCurrentRowIndex].Cells["dgvcmbAccountLedger"].Value = decId;
                        dgvPaymentVoucher.Rows[inCurrentRowIndex].Cells["dgvcmbAccountLedger"].Selected = true;


                    }
                }

                this.Enabled = true;
                this.BringToFront();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV29:" + ex.Message;
            }
        }
        /// <summary>
        /// Cash or bank combofill
        /// </summary>
        public void CashOrBankComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TransactionsGeneralFill Obj = new TransactionsGeneralFill();
                dtbl = Obj.BankOrCashComboFill(false);
                cmbBankorCash.DataSource = dtbl;
                cmbBankorCash.ValueMember = "ledgerId";
                cmbBankorCash.DisplayMember = "ledgerName";
                cmbBankorCash.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV30:" + ex.Message;
            }
        }
        /// <summary>
        /// Account ledger combofill
        /// </summary>
        public void AccountLedgerComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                dtbl = obj.AccountLedgerComboFill();
                DataRow dr = dtbl.NewRow();
                dr["ledgerId"] = "0";
                dr["ledgerName"] = string.Empty;
                dtbl.Rows.InsertAt(dr, 0);
                dgvcmbAccountLedger.DataSource = dtbl;
                dgvcmbAccountLedger.ValueMember = "ledgerId";
                dgvcmbAccountLedger.DisplayMember = "ledgerName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV31:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmBillallocation to view details and for updation 
        /// </summary>
        /// <param name="frmBillallocation"></param>
        /// <param name="decPayementId"></param>
        public void CallFromBillAllocation(frmBillallocation frmBillallocation, decimal decPayementId)
        {
            try
            {
                frmBillallocation.Enabled = false;
                base.Show();
                isUpdated = true;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                frmBillallocationObj = frmBillallocation;
                decPaymentmasterId = decPayementId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV32:" + ex.Message;
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
                base.Show();
                frmDayBook.Enabled = false;
                this.frmDayBookObj = frmDayBook;
                decPaymentmasterId = decMasterId;
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV33:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmChequeReport to view details and for updation 
        /// </summary>
        /// <param name="frmChequeReport"></param>
        /// <param name="decMasterId"></param>
        public void CallFromChequeReport(frmChequeReport frmChequeReport, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmChequeReport.Enabled = false;
                this.frmChequeReportObj = frmChequeReport;
                decPaymentmasterId = decMasterId;
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV34:" + ex.Message;
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
                base.Show();
                frmAgeing.Enabled = false;
                this.frmAgeingObj = frmAgeing;
                decPaymentmasterId = decMasterId;
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV35:" + ex.Message;
            }
        }
        /// <summary>
        /// Grid Fill function while coming from other form to update or delete
        /// </summary>
        public void FillFunction()
        {
            try
            {
                PaymentMasterSP SpPaymentMaster = new PaymentMasterSP();
                PaymentMasterInfo InfoPaymentMaster = new PaymentMasterInfo();
                PaymentDetailsSP SpPaymentDetails = new PaymentDetailsSP();
                PartyBalanceSP SpPartyBalance = new PartyBalanceSP();
                LedgerPostingSP SpLedgerPosting = new LedgerPostingSP();
                VoucherTypeSP SpVoucherType = new VoucherTypeSP();
                AccountGroupSP spAccountGroup = new AccountGroupSP();
                AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
                InfoPaymentMaster = SpPaymentMaster.PaymentMasterViewByMasterId(decPaymentmasterId);//view master details                    
                isAutomatic = SpVoucherType.CheckMethodOfVoucherNumbering(InfoPaymentMaster.VoucherTypeId);
                if (isAutomatic)
                {
                    txtVoucherNo.ReadOnly = true;
                    txtVoucherNo.Text = InfoPaymentMaster.InvoiceNo;
                    txtDate.Focus();
                }
                else
                {
                    txtVoucherNo.ReadOnly = false;
                    txtVoucherNo.Text = InfoPaymentMaster.InvoiceNo;
                    txtVoucherNo.Focus();
                }
                dtpDate.Text = InfoPaymentMaster.Date.ToString();
                cmbBankorCash.SelectedValue = InfoPaymentMaster.LedgerId;
                txtNarration.Text = InfoPaymentMaster.Narration;
                txtTotal.Text = InfoPaymentMaster.TotalAmount.ToString();
                decDailySuffixPrefixId = InfoPaymentMaster.SuffixPrefixId;
                decPaymentVoucherTypeId = InfoPaymentMaster.VoucherTypeId;
                strVoucherNo = InfoPaymentMaster.VoucherNo;
                strInvoiceNo = InfoPaymentMaster.InvoiceNo;
                DataTable dtbl = new DataTable();
                dtbl = SpPaymentDetails.PaymentDetailsViewByMasterId(decPaymentmasterId);//view details details
                for (int inI = 0; inI < dtbl.Rows.Count; inI++)
                {
                    dgvPaymentVoucher.Rows.Add();
                    dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value = Convert.ToDecimal(dtbl.Rows[inI]["ledgerId"].ToString());
                    dgvPaymentVoucher.Rows[inI].Cells["dgvtxtpaymentMasterId"].Value = dtbl.Rows[inI]["paymentMasterId"].ToString();
                    dgvPaymentVoucher.Rows[inI].Cells["dgvtxtpaymentDetailsId"].Value = dtbl.Rows[inI]["paymentDetailsId"].ToString();
                    dgvPaymentVoucher.Rows[inI].Cells["dgvtxtAmount"].Value = dtbl.Rows[inI]["amount"].ToString();
                    dgvPaymentVoucher.Rows[inI].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(dtbl.Rows[inI]["exchangeRateId"].ToString());
                    decimal decDetailsId1 = Convert.ToDecimal(dtbl.Rows[inI]["paymentDetailsId"].ToString());
                    decimal decLedgerPostingId = SpLedgerPosting.LedgerPostingIdFromDetailsId(decDetailsId1, strVoucherNo, decPaymentVoucherTypeId);
                    dgvPaymentVoucher.Rows[inI].Cells["dgvtxtLedgerPostingId"].Value = decLedgerPostingId.ToString();
                    decimal decLedgerId = Convert.ToDecimal(dtbl.Rows[inI]["ledgerId"].ToString());
                    bool IsBankAccount = spAccountGroup.AccountGroupwithLedgerId(decLedgerId);
                    decimal decI = Convert.ToDecimal(SpAccountLedger.AccountGroupIdCheck(dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()));
                    if (decI > 0)//to make amount and currency read only when party is choosen as ledger
                    {
                        dgvPaymentVoucher.Rows[inI].Cells["dgvtxtAmount"].ReadOnly = true;
                        dgvPaymentVoucher.Rows[inI].Cells["dgvcmbCurrency"].ReadOnly = true;
                    }
                    else
                    {
                        dgvPaymentVoucher.Rows[inI].Cells["dgvtxtAmount"].ReadOnly = false;
                        dgvPaymentVoucher.Rows[inI].Cells["dgvcmbCurrency"].ReadOnly = false;
                    }
                    dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value = dtbl.Rows[inI]["ChequeNo"].ToString();
                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeNo"].Value.ToString() != string.Empty)
                    {
                        dgvPaymentVoucher.Rows[inI].Cells["dgvtxtChequeDate"].Value = Convert.ToDateTime(dtbl.Rows[inI]["ChequeDate"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    dgvPaymentVoucher.Rows[inI].HeaderCell.Value = string.Empty;
                }
                DataTable dtbl1 = new DataTable();
                dtbl1 = SpPartyBalance.PartyBalanceViewByVoucherNoAndVoucherType(decPaymentVoucherTypeId, strVoucherNo, InfoPaymentMaster.Date);

                dtblPartyBalance = dtbl1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV36:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmLedgerDetails to view details and for updation 
        /// </summary>
        /// <param name="frmLedgerDetails"></param>
        /// <param name="decMasterId"></param>
        public void CallFromLedgerDetails(frmLedgerDetails frmLedgerDetails, decimal decMasterId)
        {
            try
            {
                base.Show();
                this.frmLedgerDetailsObj = frmLedgerDetails;
                frmLedgerDetailsObj.Enabled = false;
                btnDelete.Enabled = true;
                btnSave.Text = "Update";
                decPaymentmasterId = decMasterId;
                FillFunction();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV37:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        ///  Close button click 
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
                formMDI.infoError.ErrorString = "PV38:" + ex.Message;
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
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV39:" + ex.Message;
            }
        }
        /// <summary>
        /// When form load call the clear function to reset the form controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaymentVoucher_Load(object sender, EventArgs e)
        {
            try
            {

                dtpDate.Value = PublicVariables._dtCurrentDate;
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                dtpDate.CustomFormat = "dd-MMMM-yyyy";
                Clear();
                CashOrBankComboFill();
                AccountLedgerComboFill();
                txtTotal.Text = "0";
                dtblPartyBalance.Columns.Add("LedgerId", typeof(decimal));
                dtblPartyBalance.Columns.Add("AgainstVoucherTypeId", typeof(decimal));
                dtblPartyBalance.Columns.Add("AgainstVoucherNo", typeof(string));
                dtblPartyBalance.Columns.Add("ReferenceType", typeof(string));
                dtblPartyBalance.Columns.Add("Amount", typeof(decimal));
                dtblPartyBalance.Columns.Add("AgainstInvoiceNo", typeof(string));
                dtblPartyBalance.Columns.Add("DebitOrCredit", typeof(string));
                dtblPartyBalance.Columns.Add("CurrencyId", typeof(decimal));
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
                formMDI.infoError.ErrorString = "PV40:" + ex.Message;
            }
        }
        /// <summary>
        /// Button LedgerAdd click to create a new ledger from this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLedgerAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBankorCash.SelectedValue != null)
                {
                    strCashOrBank = cmbBankorCash.SelectedValue.ToString();
                }
                else
                {
                    strCashOrBank = string.Empty;
                }
                frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();
                frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                if (open == null)
                {
                    frmAccountLedgerObj.WindowState = FormWindowState.Normal;
                    frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                    frmAccountLedgerObj.CallFromPaymentVoucher(this, "CashOrBank");
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallFromPaymentVoucher(this, "CashOrBank");
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV41:" + ex.Message;
            }
        }
        /// <summary>
        /// xhange the text box value when change the datetimepicker value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpDate.Value;
                this.txtDate.Text = date.ToString("dd-MMM-yyyy");
                GridCurrencyComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV42:" + ex.Message;
            }
        }
        /// <summary>
        /// Set the current date in text box when leave the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtDate);
                if (txtDate.Text == string.Empty)
                {
                    txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpDate.Value = Convert.ToDateTime(txtDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV43:" + ex.Message;
            }
        }
        /// <summary>
        /// Remove link button click. call the remove function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dgvPaymentVoucher.SelectedCells.Count > 0 && dgvPaymentVoucher.CurrentRow != null)
                {
                    if (!dgvPaymentVoucher.Rows[dgvPaymentVoucher.CurrentRow.Index].IsNewRow)
                    {
                        if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Remove();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV44:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the serial no function to generate serial no automatically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (dgvPaymentVoucher.Rows.Count != 1)
                    SerialNumberGeneration();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV45:" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click, check the privillage and call the save or edit function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEdit();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV46:" + ex.Message;
            }

        }
        /// <summary>
        /// Delete button click, call the Delete function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (btnSave.Text == "Update")
                    {
                        if (PublicVariables.isMessageDelete)
                        {
                            if (Messages.DeleteMessage())
                            {
                                Delete(decPaymentmasterId);
                            }
                        }
                        else
                        {
                            Delete(decPaymentmasterId);
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
                formMDI.infoError.ErrorString = "PV47:" + ex.Message;
            }
        }
        /// <summary>
        /// Gridview cell value changed , doing basic calculations and checking invalid entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    TotalAmount();

                    if (dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {

                        if (dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value == null || dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value.ToString() == string.Empty)
                        {
                            dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1);
                        }

                    }
                    AccountGroupSP spAccountGroup = new AccountGroupSP();
                    AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
                    if (dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString().Trim() != string.Empty)
                    {
                        if (dgvPaymentVoucher.CurrentCell.ColumnIndex == dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].ColumnIndex)
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
                                dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = string.Empty;
                                decUpdatingLedgerForPartyremove = 0;
                                inUpdatingRowIndexForPartyRemove = -1;
                            }
                            /*************************************************************************/
                            decimal decLedgerId = Convert.ToDecimal(dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                            bool IsBankAccount = spAccountGroup.AccountGroupwithLedgerId(decLedgerId);
                            decimal decI = Convert.ToDecimal(SpAccountLedger.AccountGroupIdCheck(dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString()));
                            if (decI > 0)//to make amount and currency read only when party is choosen as ledger
                            {
                                dgvPaymentVoucher.CurrentRow.Cells["dgvtxtAmount"].ReadOnly = true;
                                dgvPaymentVoucher.CurrentRow.Cells["dgvtxtAmount"].Value = string.Empty;
                                dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbCurrency"].Value = Convert.ToDecimal(1);
                                dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].ReadOnly = true;
                            }
                            else
                            {
                                dgvPaymentVoucher.CurrentRow.Cells["dgvtxtAmount"].ReadOnly = false;
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
                        }
                    }
                    CheckColumnMissing();
                    DateValidation objVal = new DateValidation();
                    TextBox txtDate1 = new TextBox();
                    if (dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value != null)
                    {

                        txtDate1.Text = dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value.ToString();
                        bool isDate = objVal.DateValidationFunction(txtDate1);
                        if (isDate)
                        {
                            dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = txtDate1.Text;
                        }
                        else
                        {
                            dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvtxtChequeDate"].Value = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV48:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking and blocking in invalid entries in Decimal fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridViewTextBoxEditingControl txt = e.Control as DataGridViewTextBoxEditingControl;
                if (dgvPaymentVoucher.CurrentCell.ColumnIndex == dgvPaymentVoucher.Columns["dgvtxtAmount"].Index)
                {
                    if (txt != null)
                    {
                        txt.KeyPress += dgvtxtAmount_KeyPress;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV49:" + ex.Message;
            }
        }
        /// <summary>
        /// It handle the unexpected DataErrors and throw it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
                {
                    object value = dgvPaymentVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (!((DataGridViewComboBoxColumn)dgvPaymentVoucher.Columns[e.ColumnIndex]).Items.Contains(value))
                    {
                        e.ThrowException = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV50:" + ex.Message;
            }
        }
        /// <summary>
        /// Commit the each and every changes of grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvPaymentVoucher.IsCurrentCellDirty)
                {
                    dgvPaymentVoucher.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV51:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell click event for open Party balance form and calculate the settelment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    AccountLedgerSP SpAccountLedger = new AccountLedgerSP();
                    if (dgvPaymentVoucher.CurrentCell.ColumnIndex == dgvPaymentVoucher.Columns["dgvbtnAgainst"].Index)
                    {
                        decimal decI = Convert.ToDecimal(SpAccountLedger.AccountGroupIdCheck(dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].FormattedValue.ToString()));
                        if (decI > 0)
                        {
                            frmPartyBalanceObj = new frmPartyBalance();
                            frmPartyBalanceObj.MdiParent = formMDI.MDIObj;
                            decimal decLedgerId = Convert.ToDecimal(dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString());
                            if (!isAutomatic)
                            {
                                frmPartyBalanceObj.CallFromPaymentVoucher(this, decLedgerId, dtblPartyBalance, decPaymentVoucherTypeId, txtVoucherNo.Text, Convert.ToDateTime(txtDate.Text), arrlstOfDeletedPartyBalanceRow);
                            }
                            else
                            {
                                frmPartyBalanceObj.CallFromPaymentVoucher(this, decLedgerId, dtblPartyBalance, decPaymentVoucherTypeId, strVoucherNo, Convert.ToDateTime(txtDate.Text), arrlstOfDeletedPartyBalanceRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV52:" + ex.Message;
            }
        }
        /// <summary>
        /// Form Closing event , checking any other forms is opend then activate that form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaymentVoucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (btnSave.Text == "Update")
                {
                    if (frmPaymentRegisterObj != null)
                    {
                        frmPaymentRegisterObj.Enabled = true;
                        frmPaymentRegisterObj.gridfill();
                    }

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

                if (frmChequeReportObj != null)
                {
                    frmChequeReportObj.Enabled = true;
                    frmChequeReportObj.ChequeReportFillGrid();
                }

                if (frmPaymentReportObj != null)
                {
                    frmPaymentReportObj.Enabled = true;
                    frmPaymentReportObj.GridFill();
                }
                if (frmBillallocationObj != null)
                {
                    frmBillallocationObj.Enabled = true;
                    frmBillallocationObj.BillAllocationGridFill();
                }
                if (_frmVoucherSearch != null)
                {
                    _frmVoucherSearch.Enabled = true;
                    _frmVoucherSearch.GridFill();
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.LedgerDetailsView();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV53:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell begin edit event of grid, its for removing the item from the list once its added or Selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                inUpdatingRowIndexForPartyRemove = -1;
                decUpdatingLedgerForPartyremove = 0;
                DataTable dtbl = new DataTable();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                if (dgvPaymentVoucher.CurrentCell.ColumnIndex == dgvPaymentVoucher.Columns["dgvcmbAccountLedger"].Index)
                {
                    dtbl = spAccountLedger.AccountLedgerViewAll();
                    if (dtbl.Rows.Count < 2)
                    {
                        DataRow dr = dtbl.NewRow();
                        dr[0] = string.Empty;
                        dr[1] = string.Empty;
                        dtbl.Rows.InsertAt(dr, 0);
                    }
                    if (dgvPaymentVoucher.RowCount > 1)
                    {
                        int inGridRowCount = dgvPaymentVoucher.RowCount;
                        for (int inI = 0; inI < inGridRowCount - 1; inI++)
                        {
                            if (inI != e.RowIndex)
                            {
                                int inTableRowcount = dtbl.Rows.Count;
                                for (int inJ = 0; inJ < inTableRowcount; inJ++)
                                {
                                    if (dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                    {
                                        if (dtbl.Rows[inJ]["ledgerId"].ToString() == dgvPaymentVoucher.Rows[inI].Cells["dgvcmbAccountLedger"].Value.ToString())
                                        {
                                            dtbl.Rows.RemoveAt(inJ);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    DataGridViewComboBoxCell dgvccCashOrBank = (DataGridViewComboBoxCell)dgvPaymentVoucher[dgvPaymentVoucher.Columns["dgvcmbAccountLedger"].Index, e.RowIndex];
                    DataRow drow = dtbl.NewRow();
                    drow["ledgerId"] = "0";
                    drow["ledgerName"] = string.Empty;
                    dtbl.Rows.InsertAt(drow, 0);
                    dgvccCashOrBank.DataSource = dtbl;
                    dgvccCashOrBank.ValueMember = "ledgerId";
                    dgvccCashOrBank.DisplayMember = "ledgerName";
                }
                if (dgvPaymentVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbAccountLedger")
                {
                    if (dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        if (spAccountLedger.AccountGroupIdCheck(dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            inUpdatingRowIndexForPartyRemove = e.RowIndex;
                            decUpdatingLedgerForPartyremove = Convert.ToDecimal(dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                    }
                }
                if (dgvPaymentVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvcmbDrOrCr")
                {
                    if (dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                    {
                        if (spAccountLedger.AccountGroupIdCheck(dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].FormattedValue.ToString()))
                        {
                            inUpdatingRowIndexForPartyRemove = e.RowIndex;
                            decUpdatingLedgerForPartyremove = Convert.ToDecimal(dgvPaymentVoucher.Rows[e.RowIndex].Cells["dgvcmbAccountLedger"].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV54:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell enter event of grid,here makes the grid column to edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvPaymentVoucher.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvPaymentVoucher.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvPaymentVoucher.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV55:" + ex.Message;
            }

        }

        #endregion

        #region Navigation
        /// <summary>
        /// For Enter key navigation
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
                formMDI.infoError.ErrorString = "PV56:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBankorCash.Focus();
                }
                if (txtVoucherNo.ReadOnly == false)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtVoucherNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV57:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBankorCash_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvPaymentVoucher.Focus();
                    dgvPaymentVoucher.ClearSelection();
                    if (dgvPaymentVoucher.Rows.Count > 0)
                    {
                        dgvPaymentVoucher.CurrentCell = dgvPaymentVoucher.Rows[0].Cells[0];
                        dgvPaymentVoucher.Rows[0].Cells[0].Selected = true;
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbBankorCash.SelectionStart == 0 || cmbBankorCash.Text == string.Empty)
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnLedgerAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV58:" + ex.Message;
            }
        }
        /// <summary>
        /// get narration count For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "PV59:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == String.Empty)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;

                }
                else
                {
                    txtNarration.Text = txtNarration.Text.Trim();
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV60:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPaymentVoucher_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvPaymentVoucher.CurrentCell == dgvPaymentVoucher.Rows[dgvPaymentVoucher.Rows.Count - 1].Cells["dgvtxtChequeDate"])
                    {
                        txtNarration.Focus();
                        dgvPaymentVoucher.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvPaymentVoucher.CurrentCell == dgvPaymentVoucher.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        cmbBankorCash.Focus();

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV61:" + ex.Message;
            }
        }
        /// <summary>
        /// For backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {

                    if (txtNarration.SelectionStart == 0 || txtNarration.Text == String.Empty)
                    {
                        dgvPaymentVoucher.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV62:" + ex.Message;
            }
        }
        /// <summary>
        /// For  backspace navigation
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
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV63:" + ex.Message;
            }
        }
        /// <summary>
        /// From keydown for Quick access like Save and Delete functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaymentVoucher_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (cmbBankorCash.Focused)
                {
                    if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
                    {
                        if (cmbBankorCash.SelectedIndex != -1)
                        {
                            frmLedgerPopupObj = new frmLedgerPopup();
                            frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                            frmLedgerPopupObj.CallFromPaymentVoucher(this, Convert.ToDecimal(cmbBankorCash.SelectedValue.ToString()), "CashOrBank");
                        }
                        else
                        {
                            Messages.InformationMessage("Select any cash or bank account");
                            cmbBankorCash.Text = string.Empty;
                        }
                    }
                }
                else
                {
                    if (dgvPaymentVoucher.CurrentRow != null)
                    {
                        if (dgvPaymentVoucher.CurrentCell.ColumnIndex == dgvPaymentVoucher.Columns["dgvcmbAccountLedger"].Index)
                        {
                            if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                            {
                                btnSave.Focus();
                                dgvPaymentVoucher.Focus();
                                if (dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value != null && dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString() != string.Empty)
                                {
                                    frmLedgerPopupObj = new frmLedgerPopup();
                                    frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                                    frmLedgerPopupObj.CallFromPaymentVoucher(this, Convert.ToDecimal(dgvPaymentVoucher.CurrentRow.Cells["dgvcmbAccountLedger"].Value.ToString()), string.Empty);
                                }
                                else
                                {
                                    Messages.InformationMessage("Select any ledger");
                                }
                            }
                            if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                            {
                                //SendKeys.Send("{F10}");
                                frmAccountLedgerObj = new frmAccountLedger();
                                frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                                frmAccountLedgerObj.CallFromPaymentVoucher(this, string.Empty);
                            }
                        }
                        else if (dgvPaymentVoucher.CurrentCell.ColumnIndex == dgvPaymentVoucher.Columns["dgvcmbCurrency"].Index)
                        {
                            if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                            {
                                if (dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value != null && dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString() != string.Empty)
                                {
                                    frmCurrencyObj = new frmCurrencyDetails();
                                    frmCurrencyObj.MdiParent = formMDI.MDIObj;
                                    frmCurrencyObj.CallFromPaymentVoucher(this, Convert.ToDecimal(dgvPaymentVoucher.CurrentRow.Cells["dgvcmbCurrency"].Value.ToString()));
                                }
                                else
                                {
                                    Messages.InformationMessage("Select any currency ");
                                }
                            }
                        }
                    }
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    btnDelete_Click(sender, e);
                }
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
                formMDI.infoError.ErrorString = "PV64:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation in the Amount field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvtxtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvPaymentVoucher.CurrentCell != null)
                {
                    if (dgvPaymentVoucher.Columns[dgvPaymentVoucher.CurrentCell.ColumnIndex].Name == "dgvtxtAmount")
                    {
                        Common.DecimalValidation(sender, e, false);

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PV65:" + ex.Message;
            }
        }
        #endregion
    }
}
