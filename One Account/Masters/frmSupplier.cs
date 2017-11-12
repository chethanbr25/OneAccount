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
    
    public partial class frmSupplier : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        string strLedgerId;
        decimal decLedger;
        public string strAreaId;
        public string strRouteId;
        int inNarrationCount = 0;
        decimal decLedgerId = 0;
        #endregion

        #region Functions
        /// <summary>
        /// create an instance for frmSupplier class here
        /// </summary>
        public frmSupplier()
        {
            InitializeComponent();
        }
        /// <summary>
        /// for area combo fill
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
                formMDI.infoError.ErrorString = "SUP1" + ex.Message;
                
            }
        }
        /// <summary>
        /// for area search combofill
        /// </summary>
        public void AreaSearchComboFill()
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
                formMDI.infoError.ErrorString = "SUP2" + ex.Message;
            }
        }
        /// <summary>
        /// for route combofill
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
                dr["routeName"] = "All";
                dr["routeId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbRoute.DataSource = dtbl;
                cmbRoute.ValueMember = "routeId";
                cmbRoute.DisplayMember = "routeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP3" + ex.Message;
            }
        }
        /// <summary>
        /// for route search combofill
        /// </summary>
        /// <param name="decAreaId"></param>
        public void RouteSearchComboFill(decimal decAreaId)
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
                cmbRouteSearch.DataSource = dtbl;
                cmbRouteSearch.ValueMember = "routeId";
                cmbRouteSearch.DisplayMember = "routeName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP4" + ex.Message;
            }
        }
        /// <summary>
        /// to reset the form here
        /// </summary>
        public void Clear()
        {
            try
            {
                AreaComboFill();
                AreaSearchComboFill();
                txtSupplierName.Text = string.Empty;
                txtOpeningBalance.Text = string.Empty;
                txtMailingName.Text = string.Empty;
                txtAcNo.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtBranchName.Text = string.Empty;
                txtBranchCode.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtCst.Text = string.Empty;
                txtTin.Text = string.Empty;
                txtPan.Text = string.Empty;
                txtNarration.Text = string.Empty;
                cmbDrOrCr.Text = "Cr";
                if (new SettingsSP().SettingsStatusCheck("BillByBill") == "Yes")
                {
                    cmbBillbyBill.Enabled = true;
                    cmbBillbyBill.SelectedIndex = 1;
                }
                else
                {
                    cmbBillbyBill.Enabled = false;
                }
                cmbArea.SelectedIndex = 0;
                cmbRoute.SelectedIndex = 0;
                txtSupplierNameSearch.Text = string.Empty;
                cmbAreaSearch.SelectedIndex = 0;
                cmbRouteSearch.SelectedIndex = 0;
                txtSupplierName.Focus();
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP5" + ex.Message;
            }
        }
        /// <summary>
        /// fill controlls here for editing
        /// </summary>
        public void FillControls()
        {
            try
            {
                AccountLedgerInfo infoAccountledger = new AccountLedgerInfo();
                AccountLedgerSP spAccountledger = new AccountLedgerSP();
                infoAccountledger = spAccountledger.AccountLedgerViewForSupplier(Convert.ToDecimal(strLedgerId.ToString()));
                txtSupplierName.Text = infoAccountledger.LedgerName;
                txtMailingName.Text = infoAccountledger.MailingName;
                txtOpeningBalance.Text = Convert.ToString(Math.Round(infoAccountledger.OpeningBalance, PublicVariables._inNoOfDecimalPlaces));
                cmbDrOrCr.Text = infoAccountledger.CrOrDr.ToString();
                txtAcNo.Text = infoAccountledger.BankAccountNumber;
                txtBranchName.Text = infoAccountledger.BranchName;
                txtBranchCode.Text = infoAccountledger.BranchCode;
                txtMobile.Text = infoAccountledger.Mobile.ToString();
                txtPhone.Text = infoAccountledger.Phone.ToString();
                txtAddress.Text = infoAccountledger.Address;
                txtEmail.Text = infoAccountledger.Email;
                if (infoAccountledger.BillByBill)
                {
                    cmbBillbyBill.Text = "Yes";
                }
                else
                {
                    cmbBillbyBill.Text = "No";
                }
                txtTin.Text = infoAccountledger.Tin;
                txtPan.Text = infoAccountledger.Pan;
                txtCst.Text = infoAccountledger.Cst;
                cmbArea.SelectedValue = infoAccountledger.AreaId.ToString();
                cmbRoute.SelectedValue = infoAccountledger.RouteId.ToString();
                txtNarration.Text = infoAccountledger.Narration;
                decLedger = infoAccountledger.LedgerId;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP6" + ex.Message;
            }
        }
        /// <summary>
        /// gridfill function
        /// </summary>
        public void GridFill()
        {
            DataTable dtblSupplier = new DataTable();
            AccountLedgerSP spAccountledger = new AccountLedgerSP();
            decimal decAreaId = 0;
            decimal decRouteId = 0;
            try
            {
                if (cmbAreaSearch.Text == "All")
                {
                    decAreaId = 0;
                }
                else
                {
                    decAreaId = Convert.ToDecimal(cmbAreaSearch.SelectedValue.ToString());
                }
                if (cmbRouteSearch.Text == "All")
                {
                    decRouteId = 0;
                }
                else
                {
                    decRouteId = Convert.ToDecimal(cmbRouteSearch.SelectedValue.ToString());
                }
                dtblSupplier = spAccountledger.SupplierSearchAll(decAreaId, decRouteId, txtSupplierNameSearch.Text);
                dgvSupplier.DataSource = dtblSupplier;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP7" + ex.Message;
            }
        }
        /// <summary>
        /// save function
        /// </summary>
        public void SaveFunction()
        {
            decimal decOpeningBalance = 0;
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                infoAccountLedger.AccountGroupId = 22;
                infoAccountLedger.LedgerName = txtSupplierName.Text.Trim();
                if (txtOpeningBalance.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.OpeningBalance = Convert.ToDecimal(txtOpeningBalance.Text.ToString());
                    decOpeningBalance = infoAccountLedger.OpeningBalance;
                }
                else
                {
                    infoAccountLedger.OpeningBalance = 0;
                }
                infoAccountLedger.CrOrDr = cmbDrOrCr.Text.Trim().ToString();
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                infoAccountLedger.BankAccountNumber = txtAcNo.Text.Trim();
                infoAccountLedger.BranchName = txtBranchName.Text.Trim();
                infoAccountLedger.BranchCode = txtBranchCode.Text.Trim();
                infoAccountLedger.Phone = txtPhone.Text.Trim();
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
                infoAccountLedger.CreditLimit = 0;
                infoAccountLedger.CreditPeriod = 0;
                infoAccountLedger.Cst = txtCst.Text.Trim();
                infoAccountLedger.AreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                infoAccountLedger.RouteId = Convert.ToDecimal(cmbRoute.SelectedValue.ToString());
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                infoAccountLedger.Email = txtEmail.Text.Trim();
                infoAccountLedger.PricinglevelId = 26;
                infoAccountLedger.Tin = txtTin.Text.Trim();
                infoAccountLedger.Pan = txtPan.Text.Trim();
                infoAccountLedger.Narration = txtNarration.Text.Trim();
                infoAccountLedger.IsDefault = false;
                infoAccountLedger.Extra1 = string.Empty;
                infoAccountLedger.Extra2 = string.Empty;
                infoAccountLedger.ExtraDate = PublicVariables._dtCurrentDate;
                if (spAccountLedger.AccountLedgerCheckExistenceForSalesman(txtSupplierName.Text.Trim().ToString(), 0) == false)
                {
                    decLedgerId = spAccountLedger.AccountLedgerAddForCustomer(infoAccountLedger);
                    if (decOpeningBalance > 0)
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
                    txtSupplierName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP8" + ex.Message;
            }
        }
        /// <summary>
        /// save or edit function. checking invalid entries
        /// </summary>
        private void SaveOrEdit()
        {
            try
            {
                if (txtSupplierName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter supplier name");
                    txtSupplierName.Focus();
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
                formMDI.infoError.ErrorString = "SUP9" + ex.Message;
            }
        }
        /// <summary>
        /// edit function
        /// </summary>
        private void EditFunction()
        {
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                infoAccountLedger.LedgerName = txtSupplierName.Text.Trim();
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                if (txtOpeningBalance.Text.Trim() != string.Empty)
                {
                    infoAccountLedger.OpeningBalance = Convert.ToDecimal(txtOpeningBalance.Text.ToString());
                }
                else
                {
                    infoAccountLedger.OpeningBalance = 0;
                }
                infoAccountLedger.CrOrDr = cmbDrOrCr.Text.ToString();
                infoAccountLedger.BankAccountNumber = txtAcNo.Text.Trim();
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
                infoAccountLedger.Cst = txtCst.Text.Trim();
                infoAccountLedger.AreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                infoAccountLedger.RouteId = Convert.ToDecimal(cmbRoute.SelectedValue.ToString());
                infoAccountLedger.MailingName = txtMailingName.Text.Trim();
                infoAccountLedger.Phone = txtPhone.Text.Trim();
                infoAccountLedger.Email = txtEmail.Text.Trim();
                infoAccountLedger.Tin = txtTin.Text.Trim();
                infoAccountLedger.Pan = txtPan.Text.Trim();
                infoAccountLedger.Narration = txtNarration.Text.Trim();
                infoAccountLedger.LedgerId = decLedger;
                infoAccountLedger.ExtraDate = PublicVariables._dtCurrentDate;
                if (spAccountLedger.AccountLedgerCheckExistenceForSalesman(txtSupplierName.Text.Trim().ToString(), decLedger) == false)
                {
                    spAccountLedger.AccountLedgerEditForSalesman(infoAccountLedger);
                    ledgerUpdate();
                    if (cmbBillbyBill.Text == "Yes")
                    {
                        partyBalanceUpdate();
                    }
                    else
                    {
                        AccountLedgerSP spLedger = new AccountLedgerSP();
                        spLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(decLedger.ToString(), 1);
                    }
                    Messages.UpdatedMessage();
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Supplier name already exist");
                    txtSupplierName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP10" + ex.Message;
            }
        }
        /// <summary>
        /// delete function
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                if (spAccountLedger.SupplierCheckreferenceAndDelete(Convert.ToDecimal(strLedgerId.ToString())) == -1)
                {
                    Messages.ReferenceExistsMessage();

                }
                else
                {
                    spAccountLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(decLedger.ToString(), 1);
                    spAccountLedger.LedgerPostingDeleteByVoucherTypeAndVoucherNo(decLedger.ToString(), 1);
                    spAccountLedger.AccountLedgerDelete(decLedger);
                    Messages.DeletedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP11" + ex.Message;
            }
        }
        /// <summary>
        /// call delete function
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
                formMDI.infoError.ErrorString = "SUP12" + ex.Message;
            }
        }
        /// <summary>
        /// Return function From RoutForm for create a new route
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromRoutForm(decimal decId)
        {
            try
            {
                RouteComboFill(Convert.ToDecimal(cmbArea.SelectedValue.ToString()));
                if (decId!=0)
                {
                    cmbRoute.SelectedValue = decId.ToString();
                }
                else if (strRouteId != string.Empty)
                {
                    cmbRoute.SelectedValue = strRouteId;
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
                formMDI.infoError.ErrorString = "SUP13" + ex.Message;
            }
        }
        /// <summary>
        /// Return function From AreaForm for create a new area
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAreaForm(decimal decId)
        {
            try
            {
                AreaComboFill();
                if (decId !=0)
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP14" + ex.Message;
            }
        }
        /// <summary>
        /// ledger posting for save function
        /// </summary>
        public void ledgerPosting()
        {
            try
            {
                string strfinancialId;
                decimal decOpeningBlnc = Convert.ToDecimal(txtOpeningBalance.Text);
                LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
                LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
                FinancialYearSP spFinancialYear = new FinancialYearSP();
                FinancialYearInfo infoFinancialYear = new FinancialYearInfo();
                infoFinancialYear = spFinancialYear.FinancialYearViewForAccountLedger(1);
                strfinancialId = infoFinancialYear.FromDate.ToString("dd-MMM-yyyy");
                if (cmbDrOrCr.Text == "Dr")
                {
                    infoLedgerPosting.Debit = decOpeningBlnc;
                }
                else
                {
                    infoLedgerPosting.Credit = decOpeningBlnc;
                }
                infoLedgerPosting.VoucherTypeId = 1;
                infoLedgerPosting.VoucherNo = decLedgerId.ToString();
                infoLedgerPosting.Date = Convert.ToDateTime(strfinancialId.ToString());
                infoLedgerPosting.LedgerId = decLedgerId;
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = decLedgerId.ToString();
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP15" + ex.Message;
            }
        }
        /// <summary>
        /// party balance for save function
        /// </summary>
        public void partyBalance()
        {
            decimal decOpeningBlnc = 0;
            try
            {

                PartyBalanceInfo infoPatryBalance = new PartyBalanceInfo();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                AccountLedgerSP spLedger = new AccountLedgerSP();
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                decOpeningBlnc = Convert.ToDecimal(txtOpeningBalance.Text);
                if (decOpeningBlnc > 0)
                {
                    if (cmbBillbyBill.Text == "Yes")
                    {
                        infoPatryBalance.Date = PublicVariables._dtFromDate;
                        infoPatryBalance.LedgerId = decLedgerId;
                        infoPatryBalance.VoucherTypeId = 1;
                        infoPatryBalance.VoucherNo = decLedgerId.ToString();
                        infoPatryBalance.AgainstVoucherTypeId = 0;
                        infoPatryBalance.AgainstVoucherNo = "0";
                        infoPatryBalance.ReferenceType = "New";
                        if (cmbDrOrCr.Text == "Dr")
                        {
                            infoPatryBalance.Debit = decOpeningBlnc;
                            infoPatryBalance.Credit = 0;
                        }
                        else
                        {
                            infoPatryBalance.Debit = 0;
                            infoPatryBalance.Credit = decOpeningBlnc;
                        }
                        infoPatryBalance.InvoiceNo = decLedgerId.ToString();
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
                formMDI.infoError.ErrorString = "SUP16" + ex.Message;
            }
        }
        /// <summary>
        /// ledger posting edit for update
        /// </summary>
        public void ledgerUpdate()
        {
            decimal decOpeningBlnc = 0;
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
                DataTable dtbl = spLedgerPosting.GetLedgerPostingIds(decLedger.ToString(), 1);
                foreach (DataRow dr in dtbl.Rows)
                {
                    decLedgerPostingId = Convert.ToDecimal(dr.ItemArray[0].ToString());
                }
                if (cmbDrOrCr.Text == "Dr")
                {
                    infoLedgerPosting.Debit = decOpeningBlnc;
                }
                else
                {
                    infoLedgerPosting.Credit = decOpeningBlnc;
                }
                infoLedgerPosting.LedgerPostingId = decLedgerPostingId;
                infoLedgerPosting.VoucherTypeId = 1;
                infoLedgerPosting.VoucherNo = decLedger.ToString();
                infoLedgerPosting.Date = Convert.ToDateTime(strfinancialId.ToString());
                infoLedgerPosting.LedgerId = decLedger;
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = decLedger.ToString();
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
                        spAccountLedger.LedgerPostingDeleteByVoucherTypeAndVoucherNo(decLedger.ToString(), 1);
                    }
                }
                else
                {
                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP17" + ex.Message;
            }
        }
        /// <summary>
        /// party balance update for edit function
        /// </summary>
        public void partyBalanceUpdate()
        {
            try
            {
                decimal decOpeningBlnc = 0;
                if (Convert.ToDecimal(txtOpeningBalance.Text) > 0)
                {
                    decOpeningBlnc = Convert.ToDecimal(txtOpeningBalance.Text);
                }
                AccountLedgerSP spLedger = new AccountLedgerSP();
                spLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(decLedgerId.ToString(), 1);
                if (decOpeningBlnc > 0)
                {
                    partyBalance();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP18" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// save button click
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
                formMDI.infoError.ErrorString = "SUP19" + ex.Message;
            }
        }
        /// <summary>
        /// clear button click
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
                formMDI.infoError.ErrorString = "SUP20" + ex.Message;
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
                formMDI.infoError.ErrorString = "SUP21" + ex.Message;
            }
        }
        /// <summary>
        /// search button click
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
                formMDI.infoError.ErrorString = "SUP22" + ex.Message;
            }
        }
        /// <summary>
        /// delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
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
                formMDI.infoError.ErrorString = "SUP23" + ex.Message;
            }
        }
        /// <summary>
        /// grid double click for edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSupplier_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSupplier.Rows.Count > 0)
                {
                    if (dgvSupplier.CurrentRow.Index == e.RowIndex)
                    {
                        if (dgvSupplier.CurrentRow != null)
                        {
                            if (dgvSupplier.Rows.Count > 0 && e.ColumnIndex > -1)
                            {
                                if (dgvSupplier.CurrentRow.Cells["dgvtxtledgerid"].Value != null)
                                {
                                    if (dgvSupplier.CurrentRow.Cells["dgvtxtledgerid"].Value.ToString() != "")
                                    {
                                        strLedgerId = dgvSupplier.CurrentRow.Cells["dgvtxtledgerid"].Value.ToString();
                                        decLedgerId = Convert.ToDecimal(dgvSupplier.CurrentRow.Cells["dgvtxtledgerid"].Value.ToString());
                                        FillControls();
                                        btnDelete.Enabled = true;
                                        btnSave.Text = "Update";
                                        txtSupplierName.Focus();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP24" + ex.Message;
            }
        }
        /// <summary>
        /// area add button click for creating a new area
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
                    frmArea.WindowState = FormWindowState.Normal;
                    frmArea.MdiParent = formMDI.MDIObj;
                    frmArea.CallFromSupplier(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromSupplier(this);
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
                formMDI.infoError.ErrorString = "SUP25" + ex.Message;
            }
        }
        /// <summary>
        /// route add button click for creating a new Route
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRouteAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRoute.SelectedValue != null)
                {
                    strRouteId = cmbRoute.SelectedValue.ToString();
                }
                else
                {
                    strRouteId = string.Empty;
                }
                frmRoute frmroute = new frmRoute();

                frmRoute open = Application.OpenForms["frmRoute"] as frmRoute;
                if (open == null)
                {
                    frmroute.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmroute.MdiParent = formMDI.MDIObj;
                    frmroute.CallFromSupplier(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromSupplier(this);
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
                formMDI.infoError.ErrorString = "SUP26" + ex.Message;
            }
        }
        /// <summary>
        /// when form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSupplier_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP27" + ex.Message;
            }
        }
        /// <summary>
        /// route combo fill under the area for add
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
                formMDI.infoError.ErrorString = "SUP28" + ex.Message;
            }
        }
        /// <summary>
        /// route combo fill under the area for search
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
                        RouteSearchComboFill(Convert.ToDecimal(cmbAreaSearch.SelectedValue.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "SUP29" + ex.Message;

            }
        }



        #endregion

        #region Navigation
        /// <summary>
        /// form keydown for save and delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
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
                formMDI.infoError.ErrorString = "SUP30" + ex.Message;
            }
        }
        /// <summary>
        /// for navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSupplier_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter )
                {
                    if (dgvSupplier.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvSupplier.CurrentCell.ColumnIndex, dgvSupplier.CurrentCell.RowIndex);
                        dgvSupplier_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP31" + ex.Message;
            }
        }
        /// <summary>
        /// for enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSupplierName_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SUP32" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                        txtSupplierName.Focus();
                        txtSupplierName.SelectionStart = 0;
                        txtSupplierName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP33" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOpeningBalance_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDrOrCr.Focus();
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
                formMDI.infoError.ErrorString = "SUP34" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDrOrCr_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtAcNo.Focus();
                }
                if (cmbDrOrCr.Text == string.Empty || cmbDrOrCr.SelectionStart == 0)
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
                formMDI.infoError.ErrorString = "SUP35" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAcNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBranchName.Focus();
                }
                if (txtAcNo.Text == string.Empty || txtAcNo.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbDrOrCr.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP36" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                        txtAcNo.Focus();
                        txtAcNo.SelectionStart = 0;
                        txtAcNo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP37" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                formMDI.infoError.ErrorString = "SUP38" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPhone.Focus();
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
                formMDI.infoError.ErrorString = "SUP39" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtAddress.Focus();
                }
                if (txtPhone.Text == string.Empty || txtPhone.SelectionStart == 0)
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
                formMDI.infoError.ErrorString = "SUP40" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                        txtPhone.Focus();
                        txtPhone.SelectionStart = 0;
                        txtPhone.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP41" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inNarrationCount++;
                    if (inNarrationCount == 2)
                    {
                        inNarrationCount = 0;
                        txtEmail.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP42" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_Enter(object sender, EventArgs e)
        {
            try
            {
                txtAddress.Text = txtAddress.Text.Trim();
                if (txtAddress.Text == string.Empty)
                {
                    txtAddress.SelectionStart = 0;
                    txtAddress.SelectionLength = 0;
                    txtAddress.Focus();
                }
                else
                {
                    txtAddress.SelectionStart = txtAddress.Text.Length;
                    txtAddress.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP43" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                formMDI.infoError.ErrorString = "SUP44" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCst.Focus();
                }
                if (txtTin.Text == string.Empty || txtTin.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        cmbBillbyBill.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP45" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBillbyBill_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtTin.Focus();
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
                formMDI.infoError.ErrorString = "SUP46" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbArea.Focus();
                }
                if (txtPan.Text == string.Empty || txtPan.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtCst.Focus();
                        txtCst.SelectionStart = 0;
                        txtCst.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP47" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCst_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPan.Focus();
                }
                if (txtCst.Text == string.Empty || txtCst.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtTin.Focus();
                        txtTin.SelectionStart = 0;
                        txtTin.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP48" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                        cmbArea.Focus();
                    }
                }
                if (e.Alt && e.KeyCode == Keys.C)
                {
                    SendKeys.Send("{F10}");
                    btnRouteAdd_Click(sender, e);
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP49" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbArea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbRoute.Focus();
                }
                if (cmbArea.Text == string.Empty || cmbArea.SelectionStart == 0)
                {

                    if (e.KeyCode == Keys.Back)
                    {
                        txtPan.Focus();
                        txtPan.SelectionStart = 0;
                        txtPan.SelectionLength = 0;

                    }
                }
                if (e.Alt && e.KeyCode == Keys.C)
                {
                    SendKeys.Send("{F10}");
                    btnAreaAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP50" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                formMDI.infoError.ErrorString = "SUP51" + ex.Message;
            }
        }
        /// <summary>
        /// Get count of narratioon text lines for enterkey and backspace navigation
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
                formMDI.infoError.ErrorString = "SUP52" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                formMDI.infoError.ErrorString = "SUP53" + ex.Message;
            }
        }
        /// <summary>
        /// for navigation
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
                formMDI.infoError.ErrorString = "SUP54" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSupplierNameSearch_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SUP55" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAreaSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbRouteSearch.Focus();
                }
                if (cmbAreaSearch.Text == string.Empty || cmbAreaSearch.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtSupplierNameSearch.Focus();
                        txtSupplierNameSearch.SelectionStart = 0;
                        txtSupplierNameSearch.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP56" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRouteSearch_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SUP57" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbRouteSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SUP58" + ex.Message;
            }
        }
        /// <summary>
        /// for enterkey and backspace navigation
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
                formMDI.infoError.ErrorString = "SUP59" + ex.Message;
            }
        }
        #endregion

        
    }
}

