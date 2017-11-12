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
    public partial class frmSuffixPrefixSettings : Form
    {


        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decSuffixPrefixId;
        int inNarrationCount = 0;
        #endregion

        #region Function

        public frmSuffixPrefixSettings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// vouchertype conbofill function
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                DataTable dtbl = new DataTable();
                dtbl = spSuffixPrefix.VoucherTypeViewAllInSuffixPrefix();
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS1" + ex.Message;
                
            }
        }
        /// <summary>
        /// voucherType combofill for search function
        /// </summary>
        public void VoucherTypeComboFillSearch()
        {
            try
            {
                SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                DataTable dtbl = new DataTable();
                dtbl = spSuffixPrefix.VoucherTypeViewAllInSuffixPrefix();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbVoucherTypeSearch.DataSource = dtbl;
                cmbVoucherTypeSearch.ValueMember = "voucherTypeId";
                cmbVoucherTypeSearch.DisplayMember = "voucherTypeName";
                cmbVoucherTypeSearch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS2" + ex.Message;
            }
        }
        /// <summary>
        /// grid fill function
        /// </summary>
        public void GridFill()
        {
            try
            {
                SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                DataTable dtbl = new DataTable();
                dtbl = spSuffixPrefix.VoucherTypeSearchInSuffixPrefix(cmbVoucherTypeSearch.Text);
                dgvSuffixPrefixSettings.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS3" + ex.Message;
            }
        }
        /// <summary>
        /// save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix.FromDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                infoSuffixPrefix.ToDate = Convert.ToDateTime(txtToDate.Text.ToString());
                String strvouchertype = (cmbVoucherType.SelectedValue.ToString());
                infoSuffixPrefix.VoucherTypeId = Convert.ToDecimal(strvouchertype.ToString());
                infoSuffixPrefix.Suffix = txtSufix.Text.Trim();
                infoSuffixPrefix.Prefix = txtPrefix.Text.Trim();
                infoSuffixPrefix.StartIndex = Convert.ToDecimal(txtStartIndex.Text.ToString());
                if (cbxPrefillWithZero.Checked)
                {
                    infoSuffixPrefix.PrefillWithZero = true;
                    infoSuffixPrefix.WidthOfNumericalPart = Convert.ToInt32(txtWidthofNumericalPart.Text.ToString());
                }
                else
                {
                    infoSuffixPrefix.PrefillWithZero = false;
                    infoSuffixPrefix.WidthOfNumericalPart = 0;
                }
                infoSuffixPrefix.Narration = txtNarration.Text.Trim();
                infoSuffixPrefix.Extra1 = string.Empty;
                infoSuffixPrefix.Extra2 = string.Empty;
                if (spSuffixPrefix.SuffixPrefixCheckExistenceForAdd(txtFromDate.Text.ToString(), txtToDate.Text.ToString(), Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()), 0) == false)
                {
                    if (spSuffixPrefix.SuffixPrefixAddWithId(infoSuffixPrefix))
                    {
                        Messages.SavedMessage();
                        Clear();
                    }
                }
                else
                {
                    Messages.InformationMessage("Voucher type already exist for  dates");
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS4" + ex.Message;
            }
        }
        /// <summary>
        /// Edit function
        /// </summary>
        public void EditFunction()
        {
            try
            {
                SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix.FromDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                infoSuffixPrefix.ToDate = Convert.ToDateTime(txtToDate.Text.ToString());
                String strvouchertype = (cmbVoucherType.SelectedValue.ToString());
                infoSuffixPrefix.VoucherTypeId = Convert.ToDecimal(strvouchertype.ToString());
                infoSuffixPrefix.Prefix = txtPrefix.Text.Trim();
                infoSuffixPrefix.Suffix = txtSufix.Text.Trim();
                infoSuffixPrefix.StartIndex = Convert.ToDecimal(txtStartIndex.Text.ToString());
                if (cbxPrefillWithZero.Checked)
                {
                    infoSuffixPrefix.PrefillWithZero = true;
                    infoSuffixPrefix.WidthOfNumericalPart = Convert.ToInt32(txtWidthofNumericalPart.Text.ToString());
                }
                else
                {
                    infoSuffixPrefix.PrefillWithZero = false;
                    infoSuffixPrefix.WidthOfNumericalPart = 0;
                }
                infoSuffixPrefix.Narration = txtNarration.Text.Trim();
                infoSuffixPrefix.Extra1 = string.Empty;
                infoSuffixPrefix.Extra2 = string.Empty;
                infoSuffixPrefix.SuffixprefixId = decSuffixPrefixId;
                if (spSuffixPrefix.SuffixPrefixCheckExistenceForAdd(txtFromDate.Text.ToString(), txtToDate.Text.ToString(), Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()), decSuffixPrefixId) == false)
                {
                    if (spSuffixPrefix.SuffixPrefixSettingsEdit(infoSuffixPrefix))
                    {
                        Messages.UpdatedMessage();
                        Clear();
                    }
                    else
                    {
                        Messages.ReferenceExistsMessageForUpdate();
                        cmbVoucherType.Focus();
                    }
                }
                else
                {
                    Messages.InformationMessage("Voucher type already exist for  dates");
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS5" + ex.Message;
            }
        }
        /// <summary>
        /// checking invalid entries for save or update function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtFromDate.Text == String.Empty)
                {
                    Messages.InformationMessage("Enter from date");
                    txtFromDate.Focus();
                }
                else if (txtToDate.Text == String.Empty)
                {
                    Messages.InformationMessage("Enter to date");
                    txtToDate.Focus();
                }
                else if (cmbVoucherType.Text.Trim() == String.Empty)
                {
                    Messages.InformationMessage("Select voucher type");
                    cmbVoucherType.Focus();
                }
                else if (txtStartIndex.Text.Trim() == String.Empty || Convert.ToDecimal(txtStartIndex.Text.ToString()) == 0)
                {
                    Messages.InformationMessage("Start index should be greater than zero");
                    txtStartIndex.Focus();
                }
                else if (cbxPrefillWithZero.Checked && txtWidthofNumericalPart.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage(" Enter Width of numerical part");
                    txtWidthofNumericalPart.Focus();
                }
                else if (Convert.ToDateTime(txtFromDate.Text.ToString()) > Convert.ToDateTime(txtToDate.Text.ToString()))
                {
                    Messages.InformationMessage("Selected fromDate is greater than toDate.Select different fromDate");
                    txtToDate.Focus();
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
                formMDI.infoError.ErrorString = "SPS6" + ex.Message;
            }
        }
        /// <summary>
        /// delete function
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                if (spSuffixPrefix.SuffixPrefixSettingsDeleting(decSuffixPrefixId) == -1)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    Messages.DeletedMessage();
                    Clear();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS7" + ex.Message;
            }
        }
        /// <summary>
        /// call delete function and check confirmation
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
                formMDI.infoError.ErrorString = "SPS8" + ex.Message;
            }
        }
        /// <summary>
        /// form will be reset here
        /// </summary>
        public void Clear()
        {
            try
            {
                VoucherTypeComboFill();
                VoucherTypeComboFillSearch();
                cbxPrefillWithZero.Checked = false;
                txtWidthofNumericalPart.Enabled = false;
                txtFromDate.Focus();
                btnDelete.Enabled = false;
                txtStartIndex.Enabled = true;
                dtpFromDate.MinDate = PublicVariables._dtFromDate;
                dtpFromDate.MaxDate = PublicVariables._dtToDate;
                dtpToDate.MinDate = PublicVariables._dtFromDate;
                dtpToDate.MaxDate = PublicVariables._dtToDate;
                dtpFromDate.Value = PublicVariables._dtCurrentDate;
                dtpToDate.Value = PublicVariables._dtCurrentDate;
                txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                cmbVoucherType.SelectedIndex = -1;
                txtStartIndex.Text = String.Empty;
                txtPrefix.Text = String.Empty;
                txtSufix.Text = String.Empty;
                cbxPrefillWithZero.Checked = false;
                txtWidthofNumericalPart.Text = String.Empty;
                txtWidthofNumericalPart.Enabled = false;
                txtNarration.Text = String.Empty;
                btnDelete.Enabled = false;
                btnSave.Enabled = true;
                btnSave.Text = "Save";
                txtFromDate.Focus();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS9" + ex.Message;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// clode buton click
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
                formMDI.infoError.ErrorString = "SPS10" + ex.Message;
            }
        }
        /// <summary>
        /// save button click, check privilage to save
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
                formMDI.infoError.ErrorString = "SPS11" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click, check privilage to Delete
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
                formMDI.infoError.ErrorString = "SPS12" + ex.Message;
            }
        }
        /// <summary>
        /// clear button click for reset the form
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
                formMDI.infoError.ErrorString = "SPS13" + ex.Message;
            }
        }
        /// <summary>
        /// when form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSuffixPrefixSettings_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS14" + ex.Message;
            }
        }
        /// <summary>
        /// to set date in from date field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFromDate.Value;
                this.txtFromDate.Text = date.ToString("dd MMM yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS15" + ex.Message;
            }
        }
        /// <summary>
        /// to set date in Todate field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtToDate.Text = date.ToString("dd MMM yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS16" + ex.Message;
            }
        }
        /// <summary>
        /// changing Fromdate as per textbox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == String.Empty)
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpFromDate.Value = DateTime.Parse(txtFromDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS17" + ex.Message;
            }
        }
        /// <summary>
        /// changing Todate as per textbox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                obj.DateValidationFunction(txtToDate);
                if (txtToDate.Text == String.Empty)
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpToDate.Value = DateTime.Parse(txtToDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS18" + ex.Message;
            }
        }
        /// <summary>
        /// search button click, call the gridfill function
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
                formMDI.infoError.ErrorString = "SPS19" + ex.Message;
            }
        }
        /// <summary>
        /// grid double click fill the items in controll for edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSuffixPrefixSettings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    SuffixPrefixSP spSuffixPrefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffixPrefix.SuffixPrefixView(Convert.ToDecimal(dgvSuffixPrefixSettings.Rows[e.RowIndex].Cells["dgvtxtSuffixPrefixId"].Value.ToString()));
                    decSuffixPrefixId = Convert.ToDecimal(dgvSuffixPrefixSettings.Rows[e.RowIndex].Cells["dgvtxtSuffixPrefixId"].Value.ToString());
                    txtFromDate.Text = infoSuffixPrefix.FromDate.ToString("dd-MMM-yyyy");
                    txtToDate.Text = infoSuffixPrefix.ToDate.ToString("dd-MMM-yyyy");
                    cmbVoucherType.SelectedValue = infoSuffixPrefix.VoucherTypeId.ToString();
                    txtStartIndex.Text = infoSuffixPrefix.StartIndex.ToString();
                    txtStartIndex.Enabled = false;
                    txtSufix.Text = infoSuffixPrefix.Suffix;
                    txtPrefix.Text = infoSuffixPrefix.Prefix;
                    if (infoSuffixPrefix.PrefillWithZero == true)
                    {
                        cbxPrefillWithZero.Checked = true;
                        txtWidthofNumericalPart.Text = infoSuffixPrefix.WidthOfNumericalPart.ToString();
                    }
                    else
                    {
                        cbxPrefillWithZero.Checked = false;
                        txtWidthofNumericalPart.Text = infoSuffixPrefix.WidthOfNumericalPart.ToString();
                    }
                    txtNarration.Text = infoSuffixPrefix.Narration;
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                    txtFromDate.Focus();
                    txtWidthofNumericalPart.Enabled = true;
                    if (cbxPrefillWithZero.Checked == true)
                    {
                        txtWidthofNumericalPart.Enabled = true;
                    }
                    else
                    {
                        txtWidthofNumericalPart.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS20" + ex.Message;
            }
        }
        /// <summary>
        /// checkbox checked change for handle numeric part field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPrefillWithZero_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPrefillWithZero.Checked)
                {
                    txtWidthofNumericalPart.Enabled = true;
                }
                else
                {
                    txtWidthofNumericalPart.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS21" + ex.Message;
            }
        }
        /// <summary>
        /// call numberonly function to protect entering string in NumericalPart textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWidthofNumericalPart_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.NumberOnly(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS22" + ex.Message;
            }
        }
        /// <summary>
        /// call numberonly function to protect entering string in txtStartIndex textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStartIndex_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.NumberOnly(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS23" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// form keydown for save delete and close functions using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSuffixPrefixSettings_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                    {
                        if (cmbVoucherType.Focused)
                        {
                            cmbVoucherType.DropDownStyle = ComboBoxStyle.DropDown;
                        }
                        else
                        {
                            cmbVoucherType.DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                        btnSave_Click(sender, e);
                    }
                    else
                    {
                        Messages.NoPrivillageMessage();
                    }
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    if (btnDelete.Enabled)
                    {
                        if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                        {

                            btnDelete_Click(sender, e);
                        }
                        else
                        {
                            Messages.NoPrivillageMessage();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS24" + ex.Message;
            }
        }
        /// <summary>
        /// for enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtToDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS25" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                    cmbVoucherType.SelectionLength = 0;
                    cmbVoucherType.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text.Trim() == string.Empty || txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionLength = 0;
                        txtFromDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS26" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtStartIndex.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbVoucherType.Text == String.Empty || cmbVoucherType.SelectionStart == 0)
                    {
                        txtToDate.Focus();
                        txtToDate.SelectionLength = 0;
                        txtToDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS27" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStartIndex_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPrefix.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtStartIndex.Text == String.Empty || txtStartIndex.SelectionStart == 0)
                    {
                        cmbVoucherType.Focus();
                        cmbVoucherType.SelectionStart = 0;
                        cmbVoucherType.SelectionLength = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS28" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrefix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSufix.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtPrefix.Text == String.Empty || txtPrefix.SelectionStart == 0)
                    {
                        txtStartIndex.Focus();
                        txtStartIndex.SelectionStart = 0;
                        txtStartIndex.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS29" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSufix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxPrefillWithZero.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSufix.Text == String.Empty || txtSufix.SelectionStart == 0)
                    {
                        txtPrefix.Focus();
                        txtPrefix.SelectionStart = 0;
                        txtPrefix.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS30" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPrefillWithZero_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cbxPrefillWithZero.Checked)
                    {
                        txtWidthofNumericalPart.Focus();
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtSufix.Focus();
                    txtSufix.SelectionStart = 0;
                    txtSufix.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS31" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWidthofNumericalPart_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtWidthofNumericalPart.Text == String.Empty || txtWidthofNumericalPart.SelectionStart == 0)
                    {
                        cbxPrefillWithZero.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS32" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == String.Empty || txtNarration.SelectionStart == 0)
                    {
                        if (txtWidthofNumericalPart.Enabled)
                        {
                            txtWidthofNumericalPart.Focus();
                            txtWidthofNumericalPart.SelectionStart = 0;
                            txtWidthofNumericalPart.SelectionLength = 0;
                        }
                        else
                        {
                            cbxPrefillWithZero.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS33" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {

                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == String.Empty)
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
                formMDI.infoError.ErrorString = "SPS34" + ex.Message;
            }
        }
        /// <summary>
        /// get narration row count for EnterKey and backspace navigation
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
                        if (btnSave.Enabled == true)
                        {
                            inNarrationCount = 0;
                            btnSave.Focus();
                        }
                        else
                        {
                            btnClear.Focus();
                        }
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS35" + ex.Message;
            }
        }
        /// <summary>
        /// for  backspace navigation
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
                formMDI.infoError.ErrorString = "SPS36" + ex.Message;
            }
        }
        /// <summary>
        /// for Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSuffixPrefixSettings_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherTypeSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS37" + ex.Message;
            }
        }
        /// <summary>
        /// for EnterKey,Tab and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSuffixPrefixSettings_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvSuffixPrefixSettings.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvSuffixPrefixSettings.CurrentCell.ColumnIndex, dgvSuffixPrefixSettings.CurrentCell.RowIndex);
                        dgvSuffixPrefixSettings_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS38" + ex.Message;
            }
        }
        /// <summary>
        /// butten search keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherTypeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS39" + ex.Message;
            }
        }
        /// <summary>
        /// for Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherTypeSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPS40" + ex.Message;
            }
        }

        #endregion


    }
}
