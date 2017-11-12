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
using System.Text.RegularExpressions;
namespace One_Account
{
    public partial class frmCustomer : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strAreaId;
        string strPricingLevelId;
        string strRoutId;
        decimal decLedgerIdForEdit;
        decimal decLedger;
        decimal decledgerid;
        decimal decOpeningBlnc;
        int inNarrationCount = 0;
        int inAddressCount = 0;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmCustomer class
        /// </summary>
        public frmCustomer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Area combo box
        /// </summary>
        public void AreaComboFill()
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                DataTable dtbl = new DataTable();
                dtbl = spAccountLedger.cmbAreafillInCustomer();
                DataRow dr = dtbl.NewRow();
                dr["areaId"] = 0;
                dr["areaName"] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbArea.DataSource = dtbl;
                cmbArea.ValueMember = "areaId";
                cmbArea.DisplayMember = "areaName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus1:" + ex.Message;
               
            }
        }
        /// <summary>
        /// Function to fill Area combobox when returning from Area form
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAreaForm(decimal decId)
        {
            try
            {
                AreaComboFill();
                if (decId != 0m)
                {
                    cmbArea.SelectedValue = decId.ToString();
                }
                else if (strAreaId != string.Empty)
                {
                    cmbArea.SelectedValue = strAreaId;
                }
                else
                {
                    cmbArea.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbArea.Focus();
                AreaComboFillForSearch();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Area combo box
        /// </summary>
        public void AreaComboFillForSearch()
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                DataTable dtbl = new DataTable();
                dtbl = spAccountLedger.cmbAreafillInCustomer();
                DataRow dr = dtbl.NewRow();
                dr["areaId"] = 0;
                dr["areaName"] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbAreaSearch.DataSource = dtbl;
                cmbAreaSearch.ValueMember = "areaId";
                cmbAreaSearch.DisplayMember = "areaName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Pricinglevel combo box
        /// </summary>
        public void PricingLevelComboFill()
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                DataTable dtbl = new DataTable();
                dtbl = spAccountLedger.cmbPricingLevelInCustomer();
                cmbPricingLevel.DataSource = dtbl;
                cmbPricingLevel.ValueMember = "pricinglevelId";
                cmbPricingLevel.DisplayMember = "pricinglevelName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Pricinglevel combo box when return from Pricinglevel form
        /// </summary>
        /// <param name="decpriceId"></param>
        public void ReturnFromPricingLevelForm(decimal decpriceId)
        {
            try
            {
                PricingLevelComboFill();
                if (decpriceId != 0m)
                {
                    cmbPricingLevel.SelectedValue = decpriceId.ToString();
                }
                else if (strPricingLevelId != string.Empty)
                {
                    cmbPricingLevel.SelectedValue = strPricingLevelId;
                }
                else
                {
                    cmbPricingLevel.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbPricingLevel.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Route combobox
        /// </summary>
        /// <param name="decAreaId"></param>
        public void RouteComboFill(decimal decAreaId)
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                DataTable dtbl = new DataTable();
                dtbl = spAccountLedger.cmbRoutInCustomer(decAreaId);
                DataRow dr = dtbl.NewRow();
                dr["routeId"] = 0;
                dr["routeName"] = "";
                dtbl.Rows.InsertAt(dr, 0);
                cmbRoute.ValueMember = "routeId";
                cmbRoute.DisplayMember = "routeName";
                cmbRoute.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Route combobox when return from Route Form
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromRouteForm(decimal decId)
        {
            try
            {
                RouteComboFill(Convert.ToDecimal(cmbArea.SelectedValue.ToString()));
                if (decId != 0m)
                {
                    cmbRoute.SelectedValue = decId.ToString();
                }
                else if (strRoutId != string.Empty)
                {
                    cmbRoute.SelectedValue = strRoutId;
                }
                else
                {
                    cmbRoute.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbRoute.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Route combobox
        /// </summary>
        /// <param name="decAreaId"></param>
        public void RoutComboFillForSearch(decimal decAreaId)
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                DataTable dtbl = new DataTable();
                dtbl = spAccountLedger.cmbRoutInCustomer(decAreaId);
                DataRow dr = dtbl.NewRow();
                dr["routeName"] = "All";
                dr["routeId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbRoutSearch.DataSource = dtbl;
                cmbRoutSearch.ValueMember = "routeId";
                cmbRoutSearch.DisplayMember = "routeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                infoAccountLedger.AccountGroupId = 26;
                infoAccountLedger.LedgerName = txtCustomerName.Text.Trim();
                if (txtOpeningBalance.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.OpeningBalance = Convert.ToDecimal(txtOpeningBalance.Text.Trim());
                    decOpeningBlnc = infoAccountLedger.OpeningBalance;
                }
                else
                {
                    infoAccountLedger.OpeningBalance = 0;
                }
                infoAccountLedger.CrOrDr = cmbDrorCr.Text;
                infoAccountLedger.BankAccountNumber = txtAccountNo.Text.Trim();
                infoAccountLedger.BranchName = txtBranchName.Text.Trim();
                infoAccountLedger.BranchCode = txtBranchCode.Text.Trim();
                infoAccountLedger.Mobile = txtMobile.Text.Trim();
                infoAccountLedger.Address = txtAddress.Text.Trim();
                if (cmbBillbyBill.Text == "Yes")
                {
                    infoAccountLedger.BillByBill = true;
                }
                else
                {
                    infoAccountLedger.BillByBill = false;
                }
                if (txtCreditLimit.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.CreditLimit = Convert.ToDecimal(txtCreditLimit.Text.ToString());
                }
                else
                {
                    infoAccountLedger.CreditLimit = 0;
                }
                if (txtCreditPeriod.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.CreditPeriod = Convert.ToInt32(txtCreditPeriod.Text.ToString());
                }
                else
                {
                    infoAccountLedger.CreditPeriod = 0;
                }
                infoAccountLedger.Cst = txtCST.Text.Trim();
                if (Convert.ToDecimal(cmbArea.SelectedValue.ToString()) != 0)
                {
                    infoAccountLedger.AreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                }
                else
                {
                    infoAccountLedger.AreaId = 1m;
                }
                if (Convert.ToDecimal(cmbRoute.SelectedValue.ToString()) != 0)
                {
                    infoAccountLedger.RouteId = Convert.ToDecimal(cmbRoute.SelectedValue.ToString());
                }
                else
                {
                    infoAccountLedger.RouteId = 1m;
                }
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                infoAccountLedger.Phone = txtphone.Text.Trim();
                infoAccountLedger.Email = txtEmail.Text.Trim();
                infoAccountLedger.PricinglevelId = Convert.ToDecimal(cmbPricingLevel.SelectedValue.ToString());
                infoAccountLedger.Tin = txtTin.Text.Trim();
                infoAccountLedger.Pan = txtPan.Text.Trim();
                infoAccountLedger.Narration = txtNarration.Text.Trim();
                infoAccountLedger.IsDefault = false;
                infoAccountLedger.Extra1 = string.Empty;
                infoAccountLedger.Extra2 = string.Empty;
                infoAccountLedger.ExtraDate = PublicVariables._dtCurrentDate;
                if (spAccountLedger.AccountLedgerCheckExistenceForCustomer(txtCustomerName.Text.Trim(), 0) == false)
                {
                    decledgerid = spAccountLedger.AccountLedgerAddForCustomer(infoAccountLedger);
                    if (decOpeningBlnc > 0)
                    {
                        ledgerPosting();
                        if (cmbBillbyBill.Text == "Yes")
                        {
                            partyBalance();
                        }
                    }
                    Messages.SavedMessage();
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Ledger name already exist");
                    txtCustomerName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Edit
        /// </summary>
        public void EditFunction()
        {
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                infoAccountLedger.LedgerName = txtCustomerName.Text.Trim();
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                if (txtOpeningBalance.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.OpeningBalance = Convert.ToDecimal(txtOpeningBalance.Text.Trim());
                }
                else
                {
                    infoAccountLedger.OpeningBalance = 0;
                }
                infoAccountLedger.CrOrDr = cmbDrorCr.Text.ToString();
                infoAccountLedger.BankAccountNumber = txtAccountNo.Text.Trim();
                infoAccountLedger.BranchName = txtBranchName.Text.Trim();
                infoAccountLedger.BranchCode = txtBranchCode.Text.Trim();
                infoAccountLedger.Mobile = txtMobile.Text.Trim();
                infoAccountLedger.Address = txtAddress.Text.Trim();
                if (cmbBillbyBill.Text == "Yes")
                {
                    infoAccountLedger.BillByBill = true;
                }
                else
                {
                    infoAccountLedger.BillByBill = false;
                }
                if (txtCreditLimit.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.CreditLimit = Convert.ToDecimal(txtCreditLimit.Text.Trim());
                }
                else
                {
                    infoAccountLedger.CreditLimit = 0;
                }
                if (txtCreditPeriod.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.CreditPeriod = Convert.ToInt32(txtCreditPeriod.Text.Trim());
                }
                else
                {
                    infoAccountLedger.CreditPeriod = 0;
                }
                infoAccountLedger.Cst = txtCST.Text.Trim();
                if (cmbArea.SelectedValue != null)
                //if (Convert.ToDecimal(cmbArea.SelectedValue.ToString()) != 0)
                {
                    infoAccountLedger.AreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                }
                else
                {
                    infoAccountLedger.AreaId = 1m;
                }
                //if (Convert.ToDecimal(cmbRoute.SelectedValue.ToString()) != 0 && cmbRoute.SelectedValue != null)
                if(cmbRoute.SelectedValue != null)
                {
                    infoAccountLedger.RouteId = Convert.ToDecimal(cmbRoute.SelectedValue.ToString());
                }
                else
                {
                    infoAccountLedger.RouteId = 1m;
                }
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                infoAccountLedger.Phone = txtphone.Text.Trim();
                infoAccountLedger.Email = txtEmail.Text.Trim();
                if (cmbPricingLevel.SelectedValue != null)
                {
                    infoAccountLedger.PricinglevelId = Convert.ToDecimal(cmbPricingLevel.SelectedValue.ToString());
                }
                else
                {
                    infoAccountLedger.PricinglevelId = 0;
                }
                infoAccountLedger.Tin = txtTin.Text.Trim();
                infoAccountLedger.Pan = txtPan.Text.Trim();
                infoAccountLedger.Narration = txtNarration.Text.Trim();
                infoAccountLedger.LedgerId = decLedgerIdForEdit;
                infoAccountLedger.ExtraDate = PublicVariables._dtCurrentDate;
                decOpeningBlnc = infoAccountLedger.OpeningBalance;
                if (spAccountLedger.AccountLedgerCheckExistenceForCustomer(txtCustomerName.Text.Trim(), decLedger) == false)
                {
                    spAccountLedger.AccountLedgerEditForCustomer(infoAccountLedger);
                    ledgerUpdate();
                    if (cmbBillbyBill.Text == "Yes")
                    {
                        partyBalanceUpdate();
                    }
                    else
                    {
                        AccountLedgerSP spLedger = new AccountLedgerSP();
                        spLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(decLedgerIdForEdit.ToString(), 1);
                    }
                    Messages.UpdatedMessage();
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Customer name already exist");
                    txtCustomerName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call save or edit
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtCustomerName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter customer name");
                    txtCustomerName.Focus();
                }
                else
                {
                    if (btnSave.Text == "Save")
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            if (Messages.SaveMessage())
                            {
                                SaveFunction();
                            }
                        }
                        else
                        {
                            SaveFunction();
                        }
                    }
                    else
                    {
                        if (PublicVariables.isMessageEdit)
                        {
                            if (Messages.UpdateMessage())
                            {
                                EditFunction();
                            }
                        }
                        else
                        {
                            EditFunction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus11:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                if (spAccountLedger.AccountLedgerCheckReferences(decLedgerIdForEdit) == -1)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    spAccountLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(decLedgerIdForEdit.ToString(), 1);
                    spAccountLedger.LedgerPostingDeleteByVoucherTypeAndVoucherNo(decLedgerIdForEdit.ToString(), 1);
                    Messages.DeletedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call delete
        /// 
        /// </summary>
        public void Delete()
        {
            try
            {
                if (PublicVariables.isMessageDelete)
                {
                    if (Messages.DeleteMessage())
                    {
                        DeleteFunction();
                    }
                }
                else
                {
                    DeleteFunction();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus13:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls to update
        /// </summary>
        public void FillControls()
        {
            try
            {
                AccountLedgerInfo infoAccountledger = new AccountLedgerInfo();
                AccountLedgerSP spAccountledger = new AccountLedgerSP();
                infoAccountledger = spAccountledger.AccountLedgerViewForCustomer(decLedgerIdForEdit);
                txtCustomerName.Text = infoAccountledger.LedgerName;
                txtMailingName.Text = infoAccountledger.MailingName;
                txtOpeningBalance.Text = infoAccountledger.OpeningBalance.ToString();
                cmbDrorCr.Text = infoAccountledger.CrOrDr.ToString();
                txtAccountNo.Text = infoAccountledger.BankAccountNumber;
                txtBranchName.Text = infoAccountledger.BranchName;
                txtBranchCode.Text = infoAccountledger.BranchCode;
                txtMobile.Text = infoAccountledger.Mobile.ToString();
                txtphone.Text = infoAccountledger.Phone.ToString();
                txtAddress.Text = infoAccountledger.Address;
                txtEmail.Text = infoAccountledger.Email;
                cmbPricingLevel.SelectedValue = infoAccountledger.PricinglevelId.ToString();
                if (infoAccountledger.BillByBill)
                {
                    cmbBillbyBill.Text = "Yes";
                }
                else
                {
                    cmbBillbyBill.Text = "No";
                }
                if (spAccountledger.PartyBalanceAgainstReferenceCheck(decLedgerIdForEdit.ToString(), 1))
                {
                    txtOpeningBalance.Enabled = false;
                    cmbDrorCr.Enabled = false;
                    cmbBillbyBill.Enabled = false;
                }
                txtCreditPeriod.Text = infoAccountledger.CreditPeriod.ToString();
                txtCreditLimit.Text = infoAccountledger.CreditLimit.ToString();
                txtTin.Text = infoAccountledger.Tin;
                txtPan.Text = infoAccountledger.Pan;
                txtCST.Text = infoAccountledger.Cst;
                cmbArea.SelectedValue = infoAccountledger.AreaId.ToString();
                cmbRoute.SelectedValue = infoAccountledger.RouteId.ToString();
                txtNarration.Text = infoAccountledger.Narration;
                decLedger = infoAccountledger.LedgerId;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus14:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear
        /// </summary>
        public void Clear()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();
                txtCustomerName.Text = string.Empty;
                txtMailingName.Text = string.Empty;
                txtOpeningBalance.Text = "0.00";
                cmbDrorCr.Text = "Dr";
                txtAccountNo.Text = string.Empty;
                txtBranchName.Text = string.Empty;
                txtBranchCode.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtphone.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtEmail.Text = string.Empty;
                if (spSettings.SettingsStatusCheck("BillByBill") == "Yes")
                {
                    cmbBillbyBill.Enabled = true;
                    cmbBillbyBill.Text = "No";
                }
                else
                {
                    cmbBillbyBill.Enabled = false;
                }
                cmbBillbyBill.Text = "No";
                cmbPricingLevel.SelectedIndex = 0;
                txtCreditLimit.Text = string.Empty;
                txtCreditPeriod.Text = string.Empty;
                txtTin.Text = string.Empty;
                txtPan.Text = string.Empty;
                txtCST.Text = string.Empty;
                txtCustomerName.Focus();
                cmbArea.SelectedIndex = 0;
                txtNarration.Text = string.Empty; ;
                cmbRoute.SelectedIndex = 0;
                txtCustomerNameSearch.Text = string.Empty;
                cmbAreaSearch.SelectedIndex = 0;
                cmbRoutSearch.SelectedIndex = 0;
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus15:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                decimal decAreaId = 0;
                decimal decRouteId = 0;
                DataTable dtbl = new DataTable();
                AccountLedgerSP spAccountledger = new AccountLedgerSP();
                if (cmbAreaSearch.Text == "All")
                {
                    decAreaId = 0;
                }
                else
                {
                    decAreaId = Convert.ToDecimal(cmbAreaSearch.SelectedValue.ToString());
                }
                if (cmbRoutSearch.Text == "All")
                {
                    decRouteId = 0;
                }
                else
                {
                    decRouteId = Convert.ToDecimal(cmbRoutSearch.SelectedValue.ToString());
                }
                dtbl = spAccountledger.AccountLedgerSearchforCustomer(decAreaId, decRouteId, txtCustomerNameSearch.Text.Trim());
                dgvCustomer.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus16:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save values to ledgerposting table
        /// </summary>
        public void ledgerPosting()
        {
            try
            {
                string strfinancialId;
                decOpeningBlnc = Convert.ToDecimal(txtOpeningBalance.Text);
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                FinancialYearSP spFinancialYear = new FinancialYearSP();
                FinancialYearInfo infoFinancialYear = new FinancialYearInfo();
                infoFinancialYear = spFinancialYear.FinancialYearViewForAccountLedger(1);
                strfinancialId = infoFinancialYear.FromDate.ToString("dd-MMM-yyyy");
                if (cmbDrorCr.Text == "Dr")
                {
                    infoLedgerPosting.Debit = decOpeningBlnc;
                }
                else
                {
                    infoLedgerPosting.Credit = decOpeningBlnc;
                }
                infoLedgerPosting.VoucherTypeId = 1;
                infoLedgerPosting.VoucherNo = decledgerid.ToString();
                infoLedgerPosting.Date = Convert.ToDateTime(strfinancialId.ToString());
                infoLedgerPosting.LedgerId = decledgerid;
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = decledgerid.ToString();
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus17:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save values to partyBalance table
        /// </summary>
        public void partyBalance()
        {
            try
            {
                PartyBalanceInfo infoPatryBalance = new PartyBalanceInfo();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                AccountLedgerSP spLedger = new AccountLedgerSP();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                if (decOpeningBlnc > 0)
                {
                    if (cmbBillbyBill.Text == "Yes")
                    {
                        infoPatryBalance.Date = PublicVariables._dtFromDate;
                        infoPatryBalance.LedgerId = decledgerid;
                        infoPatryBalance.VoucherTypeId = 1;
                        infoPatryBalance.VoucherNo = decledgerid.ToString();
                        infoPatryBalance.AgainstVoucherTypeId = 0;
                        infoPatryBalance.AgainstVoucherNo = "0";
                        infoPatryBalance.ReferenceType = "New";
                        if (cmbDrorCr.Text == "Dr")
                        {
                            infoPatryBalance.Debit = decOpeningBlnc;
                            infoPatryBalance.Credit = 0;
                        }
                        else
                        {
                            infoPatryBalance.Debit = 0;
                            infoPatryBalance.Credit = decOpeningBlnc;
                        }
                        infoPatryBalance.InvoiceNo = decledgerid.ToString();
                        infoPatryBalance.AgainstInvoiceNo = "0";
                        infoPatryBalance.CreditPeriod = 0;
                        infoPatryBalance.ExchangeRateId = spExchangeRate.ExchangerateViewByCurrencyId(PublicVariables._decCurrencyId);
                        infoPatryBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                        infoPatryBalance.Extra1 = string.Empty;
                        infoPatryBalance.Extra2 = string.Empty;
                    }
                    spPartyBalance.PartyBalanceAdd(infoPatryBalance);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus18:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to update values in ledgerposting table
        /// </summary>
        public void ledgerUpdate()
        {
            try
            {
                string strfinancialId;
                FinancialYearSP spFinancialYear = new FinancialYearSP();
                FinancialYearInfo infoFinancialYear = new FinancialYearInfo();
                infoFinancialYear = spFinancialYear.FinancialYearViewForAccountLedger(1);
                strfinancialId = infoFinancialYear.FromDate.ToString("dd-MMM-yyyy");
                decimal decLedgerPostingId = 0;
                if (txtOpeningBalance.Text.Trim() != string.Empty)
                {
                    decOpeningBlnc = Convert.ToDecimal(txtOpeningBalance.Text.Trim());
                }
                else
                {
                    decOpeningBlnc = 0;
                }
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                DataTable dtbl = spLedgerPosting.GetLedgerPostingIds(decLedgerIdForEdit.ToString(), 1);
                foreach (DataRow dr in dtbl.Rows)
                {
                    decLedgerPostingId = Convert.ToDecimal(dr.ItemArray[0].ToString());
                }
                if (cmbDrorCr.Text == "Dr")
                {
                    infoLedgerPosting.Debit = decOpeningBlnc;
                }
                else
                {
                    infoLedgerPosting.Credit = decOpeningBlnc;
                }
                infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                infoLedgerPosting.VoucherTypeId = 1;
                infoLedgerPosting.VoucherNo = decLedgerIdForEdit.ToString();
                infoLedgerPosting.Date = Convert.ToDateTime(strfinancialId.ToString());
                infoLedgerPosting.LedgerId = decLedgerIdForEdit;
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = decLedgerIdForEdit.ToString();
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                if (dtbl.Rows.Count > 0)
                {
                    if (decOpeningBlnc > 0)
                    {
                        spLedgerPosting.LedgerPostingEdit(infoLedgerPosting);
                    }
                    else
                    {
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        spAccountLedger.LedgerPostingDeleteByVoucherTypeAndVoucherNo(decLedgerIdForEdit.ToString(), 1);
                    }
                }
                else
                {
                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus19:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to update values in partyBalance table
        /// </summary>
        public void partyBalanceUpdate()
        {
            try
            {
                AccountLedgerSP spLedger = new AccountLedgerSP();
                spLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(decLedgerIdForEdit.ToString(), 1);
                if (decOpeningBlnc > 0)
                {
                    partyBalance();
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus20:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On OpeningBalance textbox KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOpeningBalance_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus21:" + ex.Message;
            }
        }
        /// <summary>
        ///  On form close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
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
                formMDI.infoError.ErrorString = "Cus22:" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                AreaComboFill();
                AreaComboFillForSearch();
                PricingLevelComboFill();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus23:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save' button click
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
                  formMDI.infoError.ErrorString = "Cus24:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear' button click
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
                  formMDI.infoError.ErrorString = "Cus25:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Delete' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, "Delete"))
                {
                    Delete();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus26:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Close' button click
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
                  formMDI.infoError.ErrorString = "Cus27:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Search' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus28:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Area Add' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAreaAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbArea.SelectedValue != null)
                {
                    strAreaId = cmbArea.SelectedValue.ToString();
                }
                else
                {
                    strAreaId = string.Empty;
                }
                frmArea frmArea = new frmArea();
                frmArea.MdiParent = formMDI.MDIObj;
                frmArea open = Application.OpenForms["frmArea"] as frmArea;
                if (open == null)
                {
                    frmArea.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmArea.MdiParent = formMDI.MDIObj;
                    frmArea.CallFromCustomer(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromCustomer(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus29:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Pricing level add' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPricingLevelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPricingLevel.SelectedValue != null)
                {
                    strPricingLevelId = cmbPricingLevel.SelectedValue.ToString();
                }
                else
                {
                    strPricingLevelId = string.Empty;
                }
                frmPricingLevel frmPricingLevel = new frmPricingLevel();
                frmPricingLevel open = Application.OpenForms["frmPricingLevel"] as frmPricingLevel;
                if (open == null)
                {
                    frmPricingLevel.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmPricingLevel.MdiParent = formMDI.MDIObj;
                    frmPricingLevel.CallFromCustomer(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromCustomer(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus30:" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Route add' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRoutAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRoute.SelectedValue != null)
                {
                    strRoutId = cmbRoute.SelectedValue.ToString();
                }
                else
                {
                    strRoutId = string.Empty;
                }
                frmRoute frmroute = new frmRoute();
                frmRoute open = Application.OpenForms["frmRoute"] as frmRoute;
                if (open == null)
                {
                    frmroute.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmroute.MdiParent = formMDI.MDIObj;
                    frmroute.CallFromCustomer(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromCustomer(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus31:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'OpeningBalance' textbox keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOpeningBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus32:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Creditperiod' textbox keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreditPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.NumberOnly(sender, e);
                txtCreditPeriod.MaxLength = 3;
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus33:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'CreditLimit' textbox keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreditLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
                txtCreditLimit.MaxLength = 10;
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus34:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview cell double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvCustomer.CurrentRow.Index == e.RowIndex)
                    {
                        decLedgerIdForEdit = Convert.ToDecimal(dgvCustomer.Rows[e.RowIndex].Cells["dgvtxtledgerId"].Value.ToString());
                        FillControls();
                        btnDelete.Enabled = true;
                        btnSave.Text = "Update";
                        txtCustomerName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus35:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears datagridview selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCustomer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvCustomer.ClearSelection();
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus36:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Area' combobox selected index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbArea.SelectedIndex != -1)
                {
                    if (cmbArea.SelectedValue.ToString() != "System.Data.DataRowView" && cmbArea.Text != "System.Data.DataRowView")
                    {
                        RouteComboFill(Convert.ToDecimal(cmbArea.SelectedValue.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Cus37:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Area' combobox selected index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAreaSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAreaSearch.SelectedIndex != -1)
                {
                    if (cmbAreaSearch.SelectedValue.ToString() != "System.Data.DataRowView" && cmbAreaSearch.Text != "System.Data.DataRowView")
                    {
                        RoutComboFillForSearch(Convert.ToDecimal(cmbAreaSearch.SelectedValue.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "Cus38:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// from keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCustomer_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && e.Control)
                {
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && e.Control)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus39:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'CustomerName' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtMailingName.Focus();
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus40:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'OpeningBalance' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOpeningBalance_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDrorCr.Focus();
                }
                if (txtOpeningBalance.Text == string.Empty || txtOpeningBalance.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtMailingName.Focus();
                        txtMailingName.SelectionStart = 0;
                        txtMailingName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus41:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'DR/CR' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDrorCr_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtAccountNo.Focus();
                }
                if (cmbDrorCr.Text == string.Empty || cmbDrorCr.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtOpeningBalance.Focus();
                        txtOpeningBalance.SelectionStart = 0;
                        txtOpeningBalance.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus42:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'MailingName' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMailingName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtOpeningBalance.Focus();
                }
                if (txtMailingName.Text == string.Empty || txtMailingName.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtCustomerName.Focus();
                        txtCustomerName.SelectionStart = 0;
                        txtCustomerName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus43:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'AccountNo' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAccountNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBranchName.Focus();
                }
                if (txtAccountNo.Text == string.Empty || txtAccountNo.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbDrorCr.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus44:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Phone' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtphone_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtAddress.Focus();
                }
                if (txtphone.Text == string.Empty || txtphone.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtMobile.Focus();
                        txtMobile.SelectionStart = 0;
                        txtMobile.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus45:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Mobile' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtphone.Focus();
                }
                if (txtMobile.Text == string.Empty || txtMobile.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtBranchCode.Focus();
                        txtBranchCode.SelectionStart = 0;
                        txtBranchCode.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus46:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Email' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBillbyBill.Focus();
                }
                if (txtEmail.Text == string.Empty || txtEmail.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtAddress.Focus();
                        txtAddress.SelectionStart = 0;
                        txtAddress.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus47:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'PricingLevel' comvobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPricingLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCreditLimit.Focus();
                }
                if (cmbPricingLevel.Text == string.Empty || cmbPricingLevel.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbBillbyBill.Focus();
                    }
                }
                if (e.Alt && e.KeyCode == Keys.C)//To open PricingLevel Form
                {
                    SendKeys.Send("{F10}");
                    btnPricingLevelAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus48:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'BillByBill' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBillbyBill_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbPricingLevel.Focus();
                }
                if (cmbBillbyBill.Text == string.Empty || cmbBillbyBill.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtEmail.Focus();
                        txtEmail.SelectionStart = 0;
                        txtEmail.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus49:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Tin' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbArea.Focus();
                }
                if (txtTin.Text == string.Empty || txtTin.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtCST.Focus();
                        txtCST.SelectionStart = 0;
                        txtCST.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus50:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'CreditLimit' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreditLimit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCreditPeriod.Focus();
                }
                if (txtCreditLimit.Text == string.Empty || txtCreditLimit.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbPricingLevel.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus51:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Pan' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbRoute.Focus();
                }
                if (txtPan.Text == string.Empty || txtPan.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbArea.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus52:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'CST' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCST_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtTin.Focus();
                }
                if (txtCST.Text == string.Empty || txtCST.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtCreditPeriod.Focus();
                        txtCreditPeriod.SelectionStart = 0;
                        txtCreditPeriod.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus53:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbRoute.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus54:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Area' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbArea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPan.Focus();
                }
                if (cmbArea.Text == string.Empty || cmbArea.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtTin.Focus();
                        txtTin.SelectionStart = 0;
                        txtTin.SelectionLength = 0;
                    }
                }
                if (e.Alt && e.KeyCode == Keys.C)//To open Area Form
                {
                    SendKeys.Send("{F10}");
                    btnAreaAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus55:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Route' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRoute_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (cmbRoute.Text == string.Empty || cmbRoute.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtPan.Focus();
                        txtPan.SelectionStart = 0;
                        txtPan.SelectionLength = 0;
                    }
                }
                if (e.Alt && e.KeyCode == Keys.C)//To open Route Form
                {
                    SendKeys.Send("{F10}");
                    btnRoutAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus56:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save' button keydown
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
                   formMDI.infoError.ErrorString = "Cus57:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'BranchName' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBranchName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBranchCode.Focus();
                }
                if (txtBranchName.Text == string.Empty || txtBranchName.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtAccountNo.Focus();
                        txtAccountNo.SelectionStart = 0;
                        txtAccountNo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "Cus58:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Branchcode' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBranchCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtMobile.Focus();
                }
                if (txtBranchCode.Text == string.Empty || txtBranchCode.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtBranchName.Focus();
                        txtBranchName.SelectionStart = 0;
                        txtBranchName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus59:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Address' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtAddress.Text == string.Empty || txtAddress.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtphone.Focus();
                        txtphone.SelectionStart = 0;
                        txtphone.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus60:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Credotperiod' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreditPeriod_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCST.Focus();
                }
                if (txtCreditPeriod.Text == string.Empty || txtCreditPeriod.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtCreditLimit.Focus();
                        txtCreditLimit.SelectionStart = 0;
                        txtCreditLimit.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus61:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'CustomerName' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAreaSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus62:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Area' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAreaSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbRoutSearch.Focus();
                }
                if (cmbAreaSearch.Text == string.Empty || cmbAreaSearch.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtCustomerNameSearch.Focus();
                        txtCustomerNameSearch.SelectionStart = 0;
                        txtCustomerNameSearch.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus63:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Route' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRoutSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbAreaSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus64:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbRoutSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus65:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Address' textbox keypress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inAddressCount++;
                    if (inAddressCount == 2)
                    {
                        inAddressCount = 0;
                        txtEmail.Focus();
                    }
                }
                else
                {
                    inAddressCount = 0;
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus66:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox keypress
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
                  formMDI.infoError.ErrorString = "Cus67:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox keyenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == string.Empty)
                {
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                    txtNarration.Focus();
                }
                else
                {
                    txtNarration.SelectionStart = txtNarration.Text.Length;
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus68:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview  keyup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvCustomer.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvCustomer.CurrentCell.ColumnIndex, dgvCustomer.CurrentCell.RowIndex);
                        dgvCustomer_CellDoubleClick(sender, ex);
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbRoutSearch.Focus();
                    cmbRoutSearch.SelectionStart = 0;
                    cmbRoutSearch.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus69:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'OpeningBalance' textbox key enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOpeningBalance_Enter(object sender, EventArgs e)
        {
            try
            {
                if (txtOpeningBalance.Text==string.Empty || Convert.ToDecimal(txtOpeningBalance.Text) == 0)
                {
                    txtOpeningBalance.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "Cus70:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'openingBalance' textbox keyLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOpeningBalance_Leave(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Convert.ToDecimal(txtOpeningBalance.Text);
                }
                catch
                {
                    txtOpeningBalance.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "Cus71:" + ex.Message;
            }
        }
        #endregion
    }
}
