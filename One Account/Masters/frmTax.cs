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
    public partial class frmTax : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        bool isDefault = false;
        decimal decTaxId;
        int inNarrationCount;
        string strTaxName;
        decimal decTaxIdForEdit;
        decimal decIdForOtherForms;
        frmProductCreation frmProductCreationObj;
        decimal decLedgerId = 0;

        #endregion

        #region Functions
        /// <summary>
        /// creating an instance for frmTax Class
        /// </summary>
        public frmTax()
        {
            InitializeComponent();
        }
        /// <summary>
        /// its a call function from product creation from to create a new tax
        /// </summary>
        /// <param name="frmProduct"></param>
        public void CallFromProdutCreation(frmProductCreation frmProduct)
        {
            try
            {
                frmProduct.Enabled = false;
                this.frmProductCreationObj = frmProduct;
                base.Show();
                groupBox2.Enabled = false;
            }
            catch(Exception ex)
            {
                formMDI.infoError.ErrorString = "TC1:" + ex.Message;
             
            }
        }
        /// <summary>
        /// reset the form here
        /// </summary>
        public void Clear()
        {
            try
            {
                txtTaxName.Clear();
                txtRate.Clear();
                txtNarration.Clear();
                dgvTaxSelection.Enabled = false;
                cmbApplicableFor.SelectedIndex = -1;
                cmbCalculationMode.SelectedIndex = -1;
                cmbCalculationMode.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                cbxActive.Checked = true;
                dgvTaxSearch.ClearSelection();
                TaxSelectionGridFill();
                decLedgerId = 0;
                txtTaxName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC2:" + ex.Message;
            }
        }
        /// <summary>
        ///  search function clear function
        /// </summary>
        public void SearchClear()
        {
            try
            {
                txtTaxNameSearch.Clear();
                cmbActiveSearch.SelectedIndex = 0;
                cmbApplicableForSearch.SelectedIndex = 0;
                cmbCalculationModeSearch.SelectedIndex = 0;
                txtTaxNameSearch.Focus();
                TaxSearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC3:" + ex.Message;
            }
        }
        /// <summary>
        /// save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                TaxInfo infoTax = new TaxInfo();
                TaxSP spTax = new TaxSP();
                TaxDetailsInfo infoTaxDetails = new TaxDetailsInfo();
                TaxDetailsSP spTaxDetails = new TaxDetailsSP();
                infoTax.TaxName = txtTaxName.Text.Trim();
                infoTax.Rate = Convert.ToDecimal(txtRate.Text.ToString());
                infoTax.ApplicableOn = cmbApplicableFor.SelectedItem.ToString();
                if (cmbCalculationMode.Enabled != true)
                {
                    infoTax.CalculatingMode = string.Empty;
                }
                else
                {
                    infoTax.CalculatingMode = cmbCalculationMode.SelectedItem.ToString();
                }
                infoTax.Narration = txtNarration.Text.Trim();
                if (cbxActive.Checked)
                {
                    infoTax.IsActive = true;
                }
                else
                {
                    infoTax.IsActive = false;
                }
                infoTax.Extra1 = string.Empty;
                infoTax.Extra2 = string.Empty;
                if (spTax.TaxCheckExistence(0, txtTaxName.Text.Trim()) == false)
                {
                    decTaxId = spTax.TaxAddWithIdentity(infoTax);
                    decIdForOtherForms = decTaxId;
                    if (dgvTaxSelection.RowCount > 0)
                    {
                        bool isOk = false;
                        foreach (DataGridViewRow dgvRow in dgvTaxSelection.Rows)
                        {
                            isOk = Convert.ToBoolean(dgvRow.Cells["dgvcbxSelect"].Value);
                            if (isOk)
                            {
                                infoTaxDetails.TaxId = decTaxId;
                                infoTaxDetails.SelectedtaxId = Convert.ToDecimal(dgvRow.Cells["dgvtxtTaxId"].Value.ToString());//dgvRow.Cells[0].Value.ToString();
                                infoTaxDetails.ExtraDate = DateTime.Now;
                                infoTaxDetails.Extra1 = string.Empty;
                                infoTaxDetails.Extra2 = string.Empty;
                                spTaxDetails.TaxDetailsAddWithoutId(infoTaxDetails);
                            }
                        }
                    }
                    CreateLedger();
                    Messages.SavedMessage();
                    Clear();
                    SearchClear();
                }
                else
                {
                    Messages.InformationMessage(" Tax or ledger already exist");
                    txtTaxName.Focus();
                }
                if (frmProductCreationObj != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC4:" + ex.Message;
            }
        }
        /// <summary>
        /// update function
        /// </summary>
        public void EditFunction()
        {
            try
            {
                TaxInfo infoTax = new TaxInfo();
                TaxSP spTax = new TaxSP();
                TaxDetailsInfo infoTaxDetails = new TaxDetailsInfo();
                TaxDetailsSP spTaxDetails = new TaxDetailsSP();
                infoTax.TaxName = txtTaxName.Text.Trim();
                infoTax.Rate = Convert.ToDecimal(txtRate.Text.ToString());
                infoTax.ApplicableOn = cmbApplicableFor.SelectedItem.ToString();
                if (cmbCalculationMode.Enabled != true)
                {
                    infoTax.CalculatingMode = string.Empty;
                }
                else
                {
                    infoTax.CalculatingMode = cmbCalculationMode.SelectedItem.ToString();
                }
                infoTax.Narration = txtNarration.Text.Trim();
                if (cbxActive.Checked)
                {
                    infoTax.IsActive = true;
                }
                else
                {
                    infoTax.IsActive = false;
                }
                infoTax.Extra1 = string.Empty;
                infoTax.Extra2 = string.Empty;
                if (txtTaxName.Text.ToString() != strTaxName)
                {
                    if (spTax.TaxCheckExistence(decTaxIdForEdit, txtTaxName.Text.Trim()) == false)
                    {
                        infoTax.TaxId = decTaxId;
                        spTax.TaxEdit(infoTax);
                        //-- Delete And Add Tax details --//
                        spTaxDetails.TaxDetailsDeleteWithTaxId(decTaxId);
                        if (dgvTaxSelection.RowCount > 0)
                        {
                            bool isOk = false;
                            foreach (DataGridViewRow dgvRow in dgvTaxSelection.Rows)
                            {
                                isOk = Convert.ToBoolean(dgvRow.Cells["dgvcbxSelect"].Value);
                                if (isOk)
                                {
                                    infoTaxDetails.TaxId = decTaxId;
                                    infoTaxDetails.SelectedtaxId = Convert.ToDecimal(dgvRow.Cells["dgvtxtTaxId"].Value.ToString());//dgvRow.Cells[0].Value.ToString();
                                    infoTaxDetails.ExtraDate = DateTime.Now;
                                    infoTaxDetails.Extra1 = string.Empty;
                                    infoTaxDetails.Extra2 = string.Empty;
                                    spTaxDetails.TaxDetailsAddWithoutId(infoTaxDetails);
                                }
                            }
                        }
                        LedgerEdit();
                        Messages.UpdatedMessage();
                        Clear();
                    }
                    else
                    {
                        Messages.InformationMessage(" Tax or ledger already exist");
                        txtTaxName.Focus();
                    }
                }
                else
                {
                    infoTax.TaxId = decTaxId;
                    spTax.TaxEdit(infoTax);
                    spTaxDetails.TaxDetailsDeleteWithTaxId(decTaxId);
                    if (dgvTaxSelection.RowCount > 0)
                    {
                        bool isOk = false;
                        foreach (DataGridViewRow dgvRow in dgvTaxSelection.Rows)
                        {
                            isOk = Convert.ToBoolean(dgvRow.Cells["dgvcbxSelect"].Value);
                            if (isOk)
                            {
                                infoTaxDetails.TaxId = decTaxId;
                                infoTaxDetails.SelectedtaxId = Convert.ToDecimal(dgvRow.Cells["dgvtxtTaxId"].Value.ToString());//dgvRow.Cells[0].Value.ToString();
                                infoTaxDetails.ExtraDate = DateTime.Now;
                                infoTaxDetails.Extra1 = string.Empty;
                                infoTaxDetails.Extra2 = string.Empty;
                                spTaxDetails.TaxDetailsAddWithoutId(infoTaxDetails);
                            }
                        }
                    }
                    LedgerEdit();
                    Messages.UpdatedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC5:" + ex.Message;
            }
        }
        /// <summary>
        /// checking checking existance entries for save or edit function
        /// </summary>
        public void SaveOrEditMessage()
        {
            try
            {
                //create ledger for the tax
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                string strTaxNameForLedger = string.Empty;
                strTaxNameForLedger = txtTaxName.Text;

                
                    if (btnSave.Text == "Save")
                    {
                        if (!spAccountLedger.AccountLedgerCheckExistence(strTaxNameForLedger, 0))
                        {
                            if (PublicVariables.isMessageAdd)
                            {
                                if (Messages.SaveMessage())
                                {

                                    SaveFunction();
                                    TaxSelectionGridFill();
                                    TaxSearchGridFill();
                                }
                            }
                            else
                            {
                                SaveFunction();
                                TaxSelectionGridFill();
                                TaxSearchGridFill();
                            }
                        }
                        else
                        {
                            Messages.InformationMessage("Cannot save. Ledger already exists");
                        }
                    }
                    else
                    {
                        if (!spAccountLedger.AccountLedgerCheckExistence(strTaxNameForLedger, decLedgerId))
                        {
                            if (PublicVariables.isMessageEdit)
                            {
                                if (Messages.UpdateMessage())
                                {
                                    EditFunction();
                                    TaxSelectionGridFill();
                                    TaxSearchGridFill();
                                }
                            }
                            else
                            {
                                EditFunction();
                                TaxSelectionGridFill();
                                TaxSearchGridFill();
                            }
                        }
                        else
                        {
                            Messages.InformationMessage("Cannot save. Ledger already exists");
                        }
                    }
                
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC6:" + ex.Message;
            }
        }
        /// <summary>
        /// check invalid entries for save or update function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                isDefault = false;
                if (txtTaxName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter tax name");
                    txtTaxName.Focus();
                }
                else if (txtRate.Text.Trim() == string.Empty || (Convert.ToDecimal(txtRate.Text.ToString()) <= 0))
                {
                    Messages.InformationMessage("Enter rate");
                    txtRate.Focus();
                }
                else if (cmbApplicableFor.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select applicable for");
                    cmbApplicableFor.Focus();
                }
                else if (cmbCalculationMode.Enabled)
                {
                    if (cmbCalculationMode.SelectedIndex == -1)
                    {
                        Messages.InformationMessage("Select calculation mode");
                        cmbCalculationMode.Focus();
                    }
                    else if (dgvTaxSelection.Enabled)
                    {
                        int inRowCount = dgvTaxSelection.RowCount;
                        for (int i = 0; i <= inRowCount - 1; i++)
                        {
                            if (dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value != null && dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value.ToString() != "False")
                            {
                                isDefault = Convert.ToBoolean(dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value.ToString());
                            }
                        }
                        if (isDefault == false)
                        {
                            Messages.InformationMessage("Select tax items");
                            dgvTaxSelection.Focus();
                        }
                        else
                        {

                            SaveOrEditMessage();
                        }
                    }
                    else
                    {
                        SaveOrEditMessage();
                    }
                }
                else
                {
                    SaveOrEditMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC7:" + ex.Message;
            }
        }
        /// <summary>
        /// tax Selection grid fill function
        /// </summary>
        public void TaxSelectionGridFill()
        {
            try
            {
                TaxSP spTax = new TaxSP();
                DataTable dtblTax = new DataTable();
                dtblTax = spTax.TaxViewAllForTaxSelection();
                dgvTaxSelection.DataSource = dtblTax;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC8:" + ex.Message;
            }
        }
        /// <summary>
        /// tax search grid fill
        /// </summary>
        public void TaxSearchGridFill()
        {
            string strCmbActiveSearchText = string.Empty;
            try
            {
                DataTable dtblTaxSearch = new DataTable();
                TaxSP spTax = new TaxSP();
                if (cmbActiveSearch.Text == "Yes")
                {
                    strCmbActiveSearchText = "True";
                }
                else if (cmbActiveSearch.Text == "No")
                {
                    strCmbActiveSearchText = "False";
                }
                else
                {
                    strCmbActiveSearchText = "All";
                }
                dtblTaxSearch = spTax.TaxSearch(txtTaxNameSearch.Text.Trim(), cmbApplicableForSearch.Text, cmbCalculationModeSearch.Text, strCmbActiveSearchText);
                dgvTaxSearch.DataSource = dtblTaxSearch;
                int inRowCount = dgvTaxSearch.RowCount;
                for (int i = 0; i <= inRowCount - 1; i++)
                {
                    if (dgvTaxSearch.Rows[i].Cells["dgvtxtActive"].Value.ToString() == "1")
                    {
                        dgvTaxSearch.Rows[i].Cells["dgvtxtActive"].Value = "Yes";
                    }
                    if (dgvTaxSearch.Rows[i].Cells["dgvtxtActive"].Value.ToString() == "0")
                    {
                        dgvTaxSearch.Rows[i].Cells["dgvtxtActive"].Value = "No";
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC9:" + ex.Message;
            }
        }
        /// <summary>
        /// fill the curresponding details for update
        /// </summary>
        public void TaxSelectionFillForUpdate()
        {
            try
            {
                int inRowCount = dgvTaxSelection.RowCount;
                for (int i = 0; i < inRowCount; i++)
                {
                    dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value = false;
                }
                decTaxId = Convert.ToDecimal(dgvTaxSearch.CurrentRow.Cells["dgvtxtTaxIdSearch"].Value.ToString());
                TaxInfo infoTax = new TaxInfo();
                TaxSP spTax = new TaxSP();
                TaxDetailsInfo infoTaxDetails = new TaxDetailsInfo();
                TaxDetailsSP spTaxDetails = new TaxDetailsSP();
                infoTax = spTax.TaxView(decTaxId);
                txtTaxName.Text = infoTax.TaxName;
                txtRate.Text = infoTax.Rate.ToString();
                cmbApplicableFor.Text = infoTax.ApplicableOn;
                cmbCalculationMode.Text = infoTax.CalculatingMode;
                txtNarration.Text = infoTax.Narration;
                if (infoTax.IsActive.ToString() == "True")
                {
                    cbxActive.Checked = true;
                }
                else
                {
                    cbxActive.Checked = false;
                }
                strTaxName = infoTax.TaxName;
                decTaxIdForEdit = infoTax.TaxId;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                DataTable dtbl = new DataTable();
                dtbl = spTax.TaxIdForTaxSelectionUpdate(decTaxId);
                foreach (DataRow dr in dtbl.Rows)
                {
                    string strTaxId = dr["selectedtaxId"].ToString();
                    for (int i = 0; i < inRowCount; i++)
                    {
                        if (dgvTaxSelection.Rows[i].Cells["dgvtxtTaxId"].Value.ToString() == strTaxId)
                        {
                            dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value = true;
                        }
                    }
                }

                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                decLedgerId = spAccountLedger.AccountLedgerIdGetByName(txtTaxName.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC10:" + ex.Message;
            }
        }
        /// <summary>
        /// delete function
        /// </summary>
        public void Delete()
        {
            try
            {
                if (PublicVariables.isMessageDelete)
                {
                    if (Messages.DeleteMessage())
                    {
                        TaxInfo infoTax = new TaxInfo();
                        TaxSP spTax = new TaxSP();
                        TaxDetailsInfo infoTaxDetails = new TaxDetailsInfo();
                        TaxDetailsSP spTaxDetails = new TaxDetailsSP();
                        AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                        bool isExist = spTax.TaxReferenceCheck(decTaxId);
                        if (!isExist)
                        {
                            if ((spTax.TaxReferenceDelete(decTaxId,decLedgerId)) == -1)
                            {
                                Messages.ReferenceExistsMessage();
                            }
                            else
                            {
                                spTaxDetails.TaxDetailsDeleteWithTaxId(decTaxId);
                                spAccountLedger.AccountLedgerDelete(decLedgerId);
                                Messages.DeletedMessage();
                                TaxSearchGridFill();
                                TaxSelectionGridFill();
                                Clear();
                                SearchClear();
                            }
                        }
                        else
                        {
                            Messages.ReferenceExistsMessage();
                        }
                    }
                }
                else
                {
                    TaxInfo infoTax = new TaxInfo();
                    TaxSP spTax = new TaxSP();
                    TaxDetailsInfo infoTaxDetails = new TaxDetailsInfo();
                    TaxDetailsSP spTaxDetails = new TaxDetailsSP();
                    bool isExist = spTax.TaxReferenceCheck(decTaxId);
                    if (!isExist)
                    {
                        if ((spTax.TaxReferenceDelete(decTaxId,decLedgerId)) == -1)
                        {
                            Messages.ReferenceExistsMessage();
                        }
                        else
                        {
                            spTaxDetails.TaxDetailsDeleteWithTaxId(decTaxId);
                            Messages.DeletedMessage();
                            TaxSearchGridFill();
                            TaxSelectionGridFill();
                            Clear();
                            SearchClear();
                        }
                    }
                    else
                    {
                        Messages.ReferenceExistsMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC11:" + ex.Message;
            }
        }
        /// <summary>
        /// Creating one ledger for the purticular tax
        /// </summary>
        public void CreateLedger()
        {
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                infoAccountLedger.AccountGroupId = 20;
                infoAccountLedger.LedgerName = txtTaxName.Text;
                infoAccountLedger.OpeningBalance = 0;
                infoAccountLedger.IsDefault = false;
                infoAccountLedger.CrOrDr = "Cr";
                infoAccountLedger.Narration = string.Empty;
                infoAccountLedger.MailingName = txtTaxName.Text;
                infoAccountLedger.Address = string.Empty;
                infoAccountLedger.Phone = string.Empty;
                infoAccountLedger.Mobile = string.Empty;
                infoAccountLedger.Email = string.Empty;
                infoAccountLedger.CreditPeriod = 0;
                infoAccountLedger.CreditLimit = 0;
                infoAccountLedger.PricinglevelId = 0;
                infoAccountLedger.BillByBill = false;
                infoAccountLedger.Tin = string.Empty;
                infoAccountLedger.Cst = string.Empty;
                infoAccountLedger.Pan = string.Empty;
                infoAccountLedger.RouteId = 1;
                infoAccountLedger.BankAccountNumber = string.Empty;
                infoAccountLedger.BranchName = string.Empty;
                infoAccountLedger.BranchCode = string.Empty;
                infoAccountLedger.ExtraDate = DateTime.Now;
                infoAccountLedger.Extra1 = string.Empty;
                infoAccountLedger.Extra2 = string.Empty;
                infoAccountLedger.AreaId = 1;
                spAccountLedger.AccountLedgerAddWithIdentity(infoAccountLedger);
               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC12:" + ex.Message;
            }

        }
        /// <summary>
        /// editing the ledger on update
        /// </summary>
        public void LedgerEdit()
        {
            try
            {
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                infoAccountLedger.LedgerId = decLedgerId;
                infoAccountLedger.AccountGroupId = 20;
                infoAccountLedger.LedgerName = txtTaxName.Text;
                infoAccountLedger.OpeningBalance = 0;
                infoAccountLedger.IsDefault = false;
                infoAccountLedger.CrOrDr = "Cr";
                infoAccountLedger.Narration = string.Empty;
                infoAccountLedger.MailingName = txtTaxName.Text;
                infoAccountLedger.Address = string.Empty;
                infoAccountLedger.Phone = string.Empty;
                infoAccountLedger.Mobile = string.Empty;
                infoAccountLedger.Email = string.Empty;
                infoAccountLedger.CreditPeriod = 0;
                infoAccountLedger.CreditLimit = 0;
                infoAccountLedger.PricinglevelId = 0;
                infoAccountLedger.BillByBill = false;
                infoAccountLedger.Tin = string.Empty;
                infoAccountLedger.Cst = string.Empty;
                infoAccountLedger.Pan = string.Empty;
                infoAccountLedger.RouteId = 1;
                infoAccountLedger.BankAccountNumber = string.Empty;
                infoAccountLedger.BranchCode = string.Empty;
                infoAccountLedger.BranchName = string.Empty;
                infoAccountLedger.ExtraDate = DateTime.Now;
                infoAccountLedger.Extra1 = string.Empty;
                infoAccountLedger.Extra2 = string.Empty;
                infoAccountLedger.AreaId = 1;
                spAccountLedger.AccountLedgerEdit(infoAccountLedger);
            }
            catch(Exception ex)
            {
                formMDI.infoError.ErrorString = "TC13:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTax_Load(object sender, EventArgs e)
        {
            try
            {
                TaxSelectionGridFill();
                Clear();
                cmbActiveSearch.SelectedIndex = 0;
                cmbApplicableForSearch.SelectedIndex = 0;
                cmbCalculationModeSearch.SelectedIndex = 0;
                TaxSearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC14:" + ex.Message;
            }
        }
        /// <summary>
        /// form keydown for save,update and delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (dgvTaxSelection.Focused)
                    {
                        txtNarration.Focus();
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
                formMDI.infoError.ErrorString = "TC15:" + ex.Message;
            }
        }
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
                   txtRate.Focus();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC16:" + ex.Message;
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
                formMDI.infoError.ErrorString = "TC17:" + ex.Message;
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
                TaxSearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC18:" + ex.Message;
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
                formMDI.infoError.ErrorString = "TC19:" + ex.Message;
            }
        }
        /// <summary>
        /// search clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC20:" + ex.Message;
            }
        }
        /// <summary>
        /// combobox tax applicable index changed for calculating mode selection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbApplicableFor_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cmbApplicableFor.Text == "Bill")
                {
                    cmbCalculationMode.Enabled = true;
                }
                else
                {
                    cmbCalculationMode.Enabled = false;
                    cmbCalculationMode.SelectedIndex = -1;
                    int inRowCount = dgvTaxSelection.RowCount;
                    for (int i = 0; i <= inRowCount - 1; i++)
                    {
                        dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC21:" + ex.Message;
            }
        }
        /// <summary>
        /// to make enable the selection tax grid if the condition is satisfied
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCalculationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCalculationMode.Text == "Tax Amount")
                {
                    dgvTaxSelection.Enabled = true;
                }
                else
                {
                    dgvTaxSelection.Enabled = false;
                    int inRowCount = dgvTaxSelection.RowCount;
                    for (int i = 0; i <= inRowCount - 1; i++)
                    {
                        dgvTaxSelection.Rows[i].Cells["dgvcbxSelect"].Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC22:" + ex.Message;
            }
        }
        /// <summary>
        /// rate keypress for decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC23:" + ex.Message;
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
                    txtTaxName.Focus();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC24:" + ex.Message;
            }
        }
        /// <summary>
        /// grid binding complete event for clear the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaxSelection_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvTaxSelection.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC25:" + ex.Message;
            }
        }
        /// <summary>
        /// cell doublr click for update the items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaxSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    TaxSelectionFillForUpdate();
                    txtTaxName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC26:" + ex.Message;
            }
        }
        /// <summary>
        /// Form closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTax_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmProductCreationObj != null)
                {
                    frmProductCreationObj.ReturnFromTaxForm(decIdForOtherForms);
                    groupBox2.Enabled = true;
                    frmProductCreationObj.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC27:" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// for enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTaxName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtRate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC28:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbApplicableFor.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtRate.Text == string.Empty || txtRate.SelectionStart == 0)
                    {
                        txtTaxName.SelectionStart = 0;
                        txtTaxName.SelectionLength = 0;
                        txtTaxName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC29:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter key and backspace for navigation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbApplicableFor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbCalculationMode.Enabled)
                    {
                        cmbCalculationMode.Focus();
                    }
                    else if (dgvTaxSelection.Enabled)
                    {
                        dgvTaxSelection.Focus();
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbApplicableFor.Text == string.Empty || cmbApplicableFor.SelectionStart == 0)
                    {
                        txtRate.SelectionStart = 0;
                        txtRate.SelectionLength = 0;
                        txtRate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC30:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCalculationMode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvTaxSelection.Enabled)
                    {
                        dgvTaxSelection.Focus();
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbCalculationMode.Text == string.Empty || cmbCalculationMode.SelectionStart == 0)
                    {
                        cmbApplicableFor.SelectionStart = 0;
                        cmbApplicableFor.SelectionLength = 0;
                        cmbApplicableFor.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC31:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaxSelection_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbCalculationMode.Enabled)
                    {
                        cmbCalculationMode.SelectionStart = 0;
                        cmbCalculationMode.SelectionLength = 0;
                        cmbCalculationMode.Focus();
                    }
                    else
                    {
                        cmbApplicableFor.SelectionStart = 0;
                        cmbApplicableFor.SelectionLength = 0;
                        cmbApplicableFor.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC32:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
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
                        if (dgvTaxSelection.Enabled)
                        {
                            dgvTaxSelection.Focus();
                        }
                        else if (cmbCalculationMode.Enabled)
                        {
                            cmbCalculationMode.SelectionStart = 0;
                            cmbCalculationMode.SelectionLength = 0;
                            cmbCalculationMode.Focus();
                        }
                        else
                        {
                            cmbApplicableFor.SelectionStart = 0;
                            cmbApplicableFor.SelectionLength = 0;
                            cmbApplicableFor.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC33:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
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
                formMDI.infoError.ErrorString = "TC34:" + ex.Message;
            }
        }
        /// <summary>
        /// get count of textbox narration
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
                        cbxActive.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC35:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxActive_KeyDown(object sender, KeyEventArgs e)
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
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC36:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cbxActive.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC37:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter  navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTaxNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbApplicableForSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC38:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbApplicableForSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCalculationModeSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbApplicableForSearch.Text == string.Empty || cmbApplicableForSearch.SelectionStart == 0)
                    {
                        txtTaxNameSearch.SelectionStart = 0;
                        txtTaxNameSearch.SelectionLength = 0;
                        txtTaxNameSearch.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC39:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCalculationModeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbActiveSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbCalculationModeSearch.Text == string.Empty || cmbCalculationModeSearch.SelectionStart == 0)
                    {
                        cmbApplicableForSearch.SelectionStart = 0;
                        cmbApplicableForSearch.SelectionLength = 0;
                        cmbApplicableForSearch.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC40:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbActiveSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbActiveSearch.Text == string.Empty || cmbActiveSearch.SelectionStart == 0)
                    {
                        cmbCalculationModeSearch.SelectionStart = 0;
                        cmbCalculationModeSearch.SelectionLength = 0;
                        cmbCalculationModeSearch.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC41:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnClearSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbActiveSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC42:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvTaxSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC43:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTaxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbActiveSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "TC44:" + ex.Message;
            }
        }

        #endregion

        

        
    }
}
