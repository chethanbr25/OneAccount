﻿
//This is a source code or part of Oneaccount project
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
    public partial class frmVoucherType : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declarations
        /// </summary>
        bool IsActive = false;
        bool isEditMode = false;
        bool isTax = false;
        decimal decVoucherTypeId = 0;
        int inNarrationCount = 0;
        int inDeclarationCount = 0;
        int q = 0;
        #endregion

        #region Functions
        /// <summary>
        /// combo fill function of dotmatrix
        /// </summary>
        public void DotMatrixComboFill()
        {
            try
            {
                MasterSP spMaster = new MasterSP();
                DataTable dtbl = new DataTable();
                dtbl = spMaster.DotMatrxPrinterFormatComboFillForVoucherType();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[1] = "Not Applicable";
                dtbl.Rows.InsertAt(dr, 0);
                cmbDotMatrix.DataSource = dtbl;
                cmbDotMatrix.ValueMember = "masterId";
                cmbDotMatrix.DisplayMember = "formName";
                cmbDotMatrix.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT1:" + ex.Message;
             

            }
        }
        /// <summary>
        /// reset the form here
        /// </summary>
        public void Clear()
        {
            try
            {
                btnSave.Text = "Save";
                isEditMode = false;
                txtNarration.Text = string.Empty;
                txtVoucherName.Text = string.Empty;
                btnDelete.Enabled = false;
                cmbMethodOfvoucherNumbering.SelectedIndex = -1;
                cmbTypeOfVoucher.SelectedIndex = -1;
                cmbDotMatrix.SelectedIndex = -1;
                cbxActive.Checked = false;
                int inRowCount = dgvApplicableTaxes.RowCount;
                for (int i = 0; i < inRowCount; i++)
                {
                    dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value = false;
                }
                dgvApplicableTaxes.ClearSelection();
                dgvVoucherType.ClearSelection();
                txtVoucherName.Enabled = true;
                cmbTypeOfVoucher.Enabled = true;
                dgvApplicableTaxes.Enabled = false;
                txtDeclaration.Text = string.Empty;
                txtHeading1.Text = string.Empty;
                txtHeading2.Text = string.Empty;
                txtHeading3.Text = string.Empty;
                txtHeading4.Text = string.Empty;
                gbxHeading.Visible = false;
                txtVoucherName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT2:" + ex.Message;
            }
        }
        /// <summary>
        /// checking the invalid entry's for save or update
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtVoucherName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter voucher name");
                    txtVoucherName.Focus();
                }
                else if (cmbTypeOfVoucher.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select voucher type");
                    cmbTypeOfVoucher.Focus();

                }
                else if (cmbMethodOfvoucherNumbering.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select method of voucher numbering");
                    cmbMethodOfvoucherNumbering.Focus();

                }
                else if (cmbDotMatrix.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select Dot-Matrix print format");
                    cmbDotMatrix.Focus();
                }
                else
                {
                    if (isEditMode == false)
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
                    else if (isEditMode)
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


                SearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT3:" + ex.Message;
            }
        }
        /// <summary>
        /// save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
                if (spVoucherType.VoucherTypeCheckExistence(txtVoucherName.Text.Trim(), 0) == false)
                {
                    infoVoucherType.VoucherTypeName = txtVoucherName.Text.Trim();
                    infoVoucherType.TypeOfVoucher = cmbTypeOfVoucher.Text;
                    infoVoucherType.MethodOfVoucherNumbering = cmbMethodOfvoucherNumbering.Text;
                    int inRowCount = dgvApplicableTaxes.RowCount;
                    for (int i = 0; i <= inRowCount - 1; i++)
                    {
                        if (dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value != null)
                        {

                            if (dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value.ToString() != "False")
                            {
                                isTax = bool.Parse(dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value.ToString());
                            }
                        }
                    }
                    infoVoucherType.IsTaxApplicable = isTax;
                    if (cbxActive.Checked)
                    {
                        IsActive = true;
                    }
                    infoVoucherType.IsActive = IsActive;
                    infoVoucherType.IsDefault = false;
                    infoVoucherType.Narration = txtNarration.Text.Trim();
                    infoVoucherType.Extra1 = string.Empty;
                    infoVoucherType.Extra2 = string.Empty;
                    infoVoucherType.MasterId = Convert.ToInt32(cmbDotMatrix.SelectedValue);
                    infoVoucherType.Declarartion = txtDeclaration.Text;
                    infoVoucherType.Heading1 = txtHeading1.Text;
                    infoVoucherType.Heading2 = txtHeading2.Text;
                    infoVoucherType.Heading3 = txtHeading3.Text;
                    infoVoucherType.Heading4 = txtHeading4.Text;

                    decVoucherTypeId = spVoucherType.VoucherTypeAddWithIdentity(infoVoucherType);
                    if (isTax)
                    {
                        for (int i = 0; i <= inRowCount - 1; i++)
                        {
                            if (dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value != null)
                            {
                                if (dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value.ToString() != "False")
                                {
                                    VoucherTypeTaxSP spVoucherTypeTax = new VoucherTypeTaxSP();
                                    VoucherTypeTaxInfo infoVoucherTypeTax = new VoucherTypeTaxInfo();
                                    infoVoucherTypeTax.VoucherTypeId = decVoucherTypeId;
                                    infoVoucherTypeTax.TaxId = Convert.ToDecimal(dgvApplicableTaxes.Rows[i].Cells["dgvtxtTaxId"].Value.ToString());
                                    infoVoucherTypeTax.Extra1 = string.Empty;
                                    infoVoucherTypeTax.Extra2 = string.Empty;

                                    spVoucherTypeTax.VoucherTypeTaxAdd(infoVoucherTypeTax);
                                }
                            }

                        }

                    }
                    Messages.SavedMessage();
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Voucher name already exists");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT4:" + ex.Message;
            }
        }
        /// <summary>
        /// update function
        /// </summary>
        public void EditFunction()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
                VoucherTypeTaxSP spVoucherTypeTax = new VoucherTypeTaxSP();
                VoucherTypeTaxInfo infoVoucherTypeTax = new VoucherTypeTaxInfo();

                if (spVoucherType.VoucherTypeCheckExistence(txtVoucherName.Text.Trim(), decVoucherTypeId) == false)
                {
                    if (spVoucherType.VoucherTypeChechReferences(decVoucherTypeId) == false)
                    {
                        infoVoucherType.VoucherTypeId = decVoucherTypeId;
                        infoVoucherType.VoucherTypeName = txtVoucherName.Text.Trim();
                        infoVoucherType.TypeOfVoucher = cmbTypeOfVoucher.Text;
                        infoVoucherType.MethodOfVoucherNumbering = cmbMethodOfvoucherNumbering.Text;
                        infoVoucherType.Narration = txtNarration.Text.Trim();
                        if (cbxActive.Checked)
                        {
                            IsActive = true;
                        }
                        infoVoucherType.IsActive = IsActive;
                        int inRowCount = dgvApplicableTaxes.RowCount;
                        for (int i = 0; i <= inRowCount - 1; i++)
                        {

                            if (dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value != null && dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value.ToString() != "False")
                            {
                                isTax = bool.Parse(dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value.ToString());

                            }
                        }
                        infoVoucherType.IsTaxApplicable = isTax;
                        infoVoucherType.IsDefault = false;
                        infoVoucherType.Extra1 = string.Empty;
                        infoVoucherType.Extra2 = string.Empty;
                        infoVoucherType.MasterId = int.Parse(cmbDotMatrix.SelectedValue.ToString());
                        infoVoucherType.Declarartion = txtDeclaration.Text;
                        infoVoucherType.Heading1 = txtHeading1.Text;
                        infoVoucherType.Heading2 = txtHeading2.Text;
                        infoVoucherType.Heading3 = txtHeading3.Text;
                        infoVoucherType.Heading4 = txtHeading4.Text;
                        if (spVoucherType.CheckForDefaultVoucherType(decVoucherTypeId) == false)
                        {

                            spVoucherType.VoucherTypeEdit(infoVoucherType);
                        }
                        else
                        {

                            spVoucherType.VoucherTypeEditForDefaultVouchers(infoVoucherType);

                        }
                        //------------------------------------- Delete And Add Tax details From VoucherTypeTax Using VoucherTypeId-------------------------------//
                        spVoucherTypeTax.DeleteVoucherTypeTaxUsingVoucherTypeId(decVoucherTypeId);
                        if (isTax)
                        {
                            for (int i = 0; i <= inRowCount - 1; i++)
                            {
                                if (dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value != null && dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value.ToString() != "False")
                                {
                                    infoVoucherTypeTax.VoucherTypeId = decVoucherTypeId;
                                    infoVoucherTypeTax.TaxId = Convert.ToDecimal(dgvApplicableTaxes.Rows[i].Cells["dgvtxtTaxId"].Value.ToString());
                                    infoVoucherTypeTax.Extra1 = string.Empty;
                                    infoVoucherTypeTax.Extra2 = string.Empty;

                                    spVoucherTypeTax.VoucherTypeTaxAdd(infoVoucherTypeTax);
                                }
                            }
                        }

                        //-----------------------------------------------------------Deleted And Added--------------------------------------------------------//
                        Messages.UpdatedMessage();
                        Clear();
                    }
                    else
                    {
                        Messages.InformationMessage("You can't update,voucher already in use");
                    }
                }
                else
                {
                    Messages.InformationMessage("Voucher name already exists");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT5:" + ex.Message;
            }
        }
        /// <summary>
        /// vouchertype search function
        /// </summary>
        public void SearchGridFill()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblSearch = new DataTable();
                dtblSearch = spVoucherType.VoucherTypeSearch(txtVoucherNameSearch.Text.Trim(), cmbTypeOfVoucherSearch.Text);
                dgvVoucherType.DataSource = dtblSearch;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT6:" + ex.Message;
            }
        }
        /// <summary>
        /// combo fill function of vouchertype
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblVouchertype = new DataTable();
                dtblVouchertype = spVoucherType.TypeOfVoucherComboFill();
                cmbTypeOfVoucher.DataSource = dtblVouchertype;
                cmbTypeOfVoucher.ValueMember = "voucherTypeId";
                cmbTypeOfVoucher.DisplayMember = "typeOfVoucher";
                cmbTypeOfVoucher.SelectedIndex = -1;
                gbxHeading.Visible = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT7:" + ex.Message;
            }

        }
        /// <summary>
        /// vouchertype search combofill function
        /// </summary>
        public void VoucherTypeSearchComboFill()
        {
            try
            {
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblVouchertype = new DataTable();
                dtblVouchertype = spVoucherType.TypeOfVoucherComboFill();
                cmbTypeOfVoucherSearch.DataSource = dtblVouchertype;
                cmbTypeOfVoucherSearch.ValueMember = "voucherTypeId";
                cmbTypeOfVoucherSearch.DisplayMember = "typeOfVoucher";
                cmbTypeOfVoucherSearch.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT8:" + ex.Message;
            }

        }
        /// <summary>
        /// tax grid fill function
        /// </summary>
        public void TaxGridFill()
        {
            try
            {
                TaxSP spTax = new TaxSP();
                DataTable dtblTax = new DataTable();
                dtblTax = spTax.TaxViewAllForVoucherType();
                dgvApplicableTaxes.DataSource = dtblTax;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT9:" + ex.Message;
            }
        }
        /// <summary>
        /// reset the search option
        /// </summary>
        public void SearchClear()
        {
            try
            {
                txtVoucherNameSearch.Text = string.Empty;
                cmbTypeOfVoucherSearch.Text = string.Empty;
                txtVoucherNameSearch.Focus();
                cmbTypeOfVoucherSearch.SelectedIndex = -1;
                SearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT10:" + ex.Message;
            }
        }
        /// <summary>
        /// checking privilage for delete
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
                formMDI.infoError.ErrorString = "VT11:" + ex.Message;
            }
        }
        /// <summary>
        /// delete function
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                VoucherTypeTaxSP spVoucherTypeTax = new VoucherTypeTaxSP();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                if (spVoucherType.CheckForDefaultVoucherType(decVoucherTypeId) == false)
                {
                    if (spVoucherType.VoucherTypeChechReferences(decVoucherTypeId) == false)
                    {
                        spVoucherType.VoucherTypeDelete(decVoucherTypeId);
                        spVoucherTypeTax.DeleteVoucherTypeTaxUsingVoucherTypeId(decVoucherTypeId);
                        Messages.DeletedMessage();
                        SearchGridFill();
                    }
                    else
                    {
                        Messages.ReferenceExistsMessage();
                    }
                }
                else
                {
                    Messages.InformationMessage("You can't delete default voucher types");
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT12:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Creates an instance of a frmVoucherType class.
        /// </summary>
        public frmVoucherType()
        {
            InitializeComponent();
        }
        /// <summary>
        /// checking the vouchertype for displaying heading fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypeOfVoucher_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbTypeOfVoucher.Text == "Sales Invoice" || cmbTypeOfVoucher.Text == "Sales Return" || cmbTypeOfVoucher.Text == "Purchase Invoice" || cmbTypeOfVoucher.Text == "Purchase Return")
                {
                    dgvApplicableTaxes.Enabled = true;
                }
                else
                {
                    dgvApplicableTaxes.Enabled = false;
                }
                if (cmbTypeOfVoucher.Text == "Sales Invoice" || cmbTypeOfVoucher.Text == "Purchase Invoice" || cmbTypeOfVoucher.Text == "Sales Return" || cmbTypeOfVoucher.Text == "Purchase Return")
                {
                    gbxHeading.Visible = true;
                }
                else
                {
                    gbxHeading.Visible = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT13:" + ex.Message;
            }

        }
        /// <summary>
        /// save button click, checking privilage for save
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
                formMDI.infoError.ErrorString = "VT14:" + ex.Message;
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
                SearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT15:" + ex.Message;
            }
        }
        /// <summary>
        /// when form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmVoucherType_Load(object sender, EventArgs e)
        {
            try
            {
                SearchGridFill();
                TaxGridFill();
                VoucherTypeComboFill();
                VoucherTypeSearchComboFill();
                DotMatrixComboFill();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT16:" + ex.Message;
            }
        }
        /// <summary>
        /// search clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                SearchClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT17:" + ex.Message;
            }
        }
        /// <summary>
        /// when cell double click for update function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvVoucherType_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    isEditMode = true;
                    btnSave.Text = "Update";
                    txtVoucherNameSearch.Focus();
                    btnDelete.Enabled = true;
                    int inRowCount = dgvApplicableTaxes.RowCount;
                    for (int i = 0; i < inRowCount; i++)
                    {
                        dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value = false;
                    }

                    VoucherTypeSP spVoucherType = new VoucherTypeSP();
                    VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
                    decVoucherTypeId = Convert.ToDecimal(dgvVoucherType.CurrentRow.Cells["dgvtxtvoucherTypeId"].Value.ToString());
                    infoVoucherType = spVoucherType.VoucherTypeView(decVoucherTypeId);
                    txtVoucherName.Text = infoVoucherType.VoucherTypeName;
                    cmbTypeOfVoucher.Text = infoVoucherType.TypeOfVoucher;
                    cmbMethodOfvoucherNumbering.Text = infoVoucherType.MethodOfVoucherNumbering;
                    txtNarration.Text = infoVoucherType.Narration;
                    txtDeclaration.Text = infoVoucherType.Declarartion;
                    cmbDotMatrix.SelectedValue = infoVoucherType.MasterId;
                    txtHeading1.Text = infoVoucherType.Heading1;
                    txtHeading2.Text = infoVoucherType.Heading2;
                    txtHeading3.Text = infoVoucherType.Heading3;
                    txtHeading4.Text = infoVoucherType.Heading4;
                    if (infoVoucherType.IsActive)
                    {
                        cbxActive.Checked = true;
                    }
                    else
                    {
                        cbxActive.Checked = false;
                    }
                    DataTable dtbl = new DataTable();
                    dtbl = spVoucherType.GetTaxIdForTaxSelection(decVoucherTypeId);
                    foreach (DataRow dr in dtbl.Rows)
                    {
                        string strTaxId = dr["taxId"].ToString();

                        for (int i = 0; i < inRowCount; i++)
                        {
                            if (dgvApplicableTaxes.Rows[i].Cells["dgvtxtTaxId"].Value.ToString() == strTaxId)
                            {
                                dgvApplicableTaxes.Rows[i].Cells["dgvcbxSelect"].Value = true;
                            }
                        }


                    }
                    if (spVoucherType.CheckForDefaultVoucherType(decVoucherTypeId) == false)
                    {
                        txtVoucherName.Enabled = true;
                        cmbTypeOfVoucher.Enabled = true;
                    }
                    else
                    {
                        txtVoucherName.Enabled = false;
                        cmbTypeOfVoucher.Enabled = false;
                    }
                    if (cmbTypeOfVoucher.Text == "Sales Return" || cmbTypeOfVoucher.Text == "Sales Invoice" || cmbTypeOfVoucher.Text == "Purchase Return" || cmbTypeOfVoucher.Text == "Purchase Invoice")
                    {
                        dgvApplicableTaxes.Enabled = true;

                    }
                    else
                    {
                        dgvApplicableTaxes.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT18:" + ex.Message;
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
                formMDI.infoError.ErrorString = "VT19:" + ex.Message;
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
                formMDI.infoError.ErrorString = "VT20:" + ex.Message;
            }
        }
        /// <summary>
        /// to clear the tax grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvApplicableTaxes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvApplicableTaxes.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT21:" + ex.Message;
            }
        }
        /// <summary>
        /// vouchertype grid clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvVoucherType_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvVoucherType.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT22:" + ex.Message;
            }
        }
        /// <summary>
        /// butten delete click, privilage check and delete function
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
                    Clear();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT23:" + ex.Message;
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// voucher name keydown event. its for working enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbTypeOfVoucher.Focus();
                    cmbTypeOfVoucher.SelectionStart = 0;
                    cmbTypeOfVoucher.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT24:" + ex.Message;
            }
        }
        /// <summary>
        /// vouchertype combobox keydown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypeOfVoucher_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbMethodOfvoucherNumbering.Focus();
                    cmbMethodOfvoucherNumbering.SelectionStart = 0;
                    cmbMethodOfvoucherNumbering.SelectionLength = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtVoucherName.Focus();
                    txtVoucherName.SelectionStart = 0;
                    txtVoucherName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT25:" + ex.Message;
            }
        }
        /// <summary>
        /// combobox MethodOfvoucherNumbering key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMethodOfvoucherNumbering_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = txtNarration.Text.Length;

                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbTypeOfVoucher.Focus();
                    cmbTypeOfVoucher.SelectionStart = 0;
                    cmbTypeOfVoucher.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT26:" + ex.Message;
            }
        }
        /// <summary>
        /// textbox narration keydown
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
                        if (dgvApplicableTaxes.Enabled)
                        {
                            if (dgvApplicableTaxes.RowCount > 0)
                            {
                                dgvApplicableTaxes.Focus();
                                dgvApplicableTaxes.Rows[0].Cells["dgvcbxSelect"].Selected = true;
                                dgvApplicableTaxes.CurrentCell = dgvApplicableTaxes.Rows[0].Cells["dgvcbxSelect"];
                            }
                            else
                            {
                                cmbDotMatrix.Focus();
                            }
                        }
                        else
                        {
                            cmbDotMatrix.Focus();
                        }
                    }

                }
                else
                {
                    inNarrationCount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text.Trim() == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        cmbMethodOfvoucherNumbering.Focus();
                        cmbMethodOfvoucherNumbering.SelectionStart = 0;
                        cmbMethodOfvoucherNumbering.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT27:" + ex.Message;
            }
        }
        /// <summary>
        /// VoucherType form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmVoucherType_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 27)
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
                formMDI.infoError.ErrorString = "VT28:" + ex.Message;
            }

        }
        /// <summary>
        /// tax grid keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvApplicableTaxes_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvApplicableTaxes.CurrentCell == dgvApplicableTaxes.Rows[dgvApplicableTaxes.Rows.Count - 1].Cells["dgvcbxSelect"])
                    {
                        cmbDotMatrix.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT29:" + ex.Message;
            }
        }
        /// <summary>
        /// active checkbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxActive_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvApplicableTaxes.RowCount > 1)
                    {
                        dgvApplicableTaxes.Focus();
                        dgvApplicableTaxes.Rows[dgvApplicableTaxes.RowCount - 1].Cells["dgvcbxSelect"].Selected = true;
                    }
                    else
                    {
                        txtDeclaration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT30:" + ex.Message;
            }
        }
        /// <summary>
        /// textbox narration key enter event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text.Trim() == string.Empty)
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
                formMDI.infoError.ErrorString = "VT31:" + ex.Message;
            }
        }
        /// <summary>
        /// enter key of textbox narration for enter key 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbTypeOfVoucherSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT32:" + ex.Message;
            }
        }
        /// <summary>
        /// vouchertype search combobox key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTypeOfVoucherSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtVoucherNameSearch.Focus();
                    txtVoucherNameSearch.SelectionStart = 0;
                    txtVoucherNameSearch.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT33:" + ex.Message;
            }

        }
        /// <summary>
        /// butten close key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (btnDelete.Enabled == true)
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
                formMDI.infoError.ErrorString = "VT34:" + ex.Message;
            }

        }
        /// <summary>
        /// delete button key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnClear.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT35:" + ex.Message;
            }

        }
        /// <summary>
        /// clear button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "VT36:" + ex.Message;
            }
        }
        /// <summary>
        /// save button key down
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
                formMDI.infoError.ErrorString = "VT37:" + ex.Message;
            }
        }
        /// <summary>
        /// vouchertype grid keyup event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvVoucherType_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvVoucherType.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvVoucherType.CurrentCell.ColumnIndex, dgvVoucherType.CurrentCell.RowIndex);
                        dgvVoucherType_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT38:" + ex.Message;
            }
        }
        /// <summary>
        /// vouchertype grid keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbTypeOfVoucherSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT39:" + ex.Message;
            }
        }
        /// <summary>
        /// form keydown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    if (dgvApplicableTaxes.Focused)
                    {
                        cbxActive.Focus();
                    }
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT40:" + ex.Message;
            }
        }
        /// <summary>
        /// search button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbTypeOfVoucherSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT41:" + ex.Message;
            }
        }
        /// <summary>
        /// dotmatrix combobox key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDotMatrix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDeclaration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "VT42:" + ex.Message;

            }
        }
        /// <summary>
        /// declaration text box keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDeclaration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    inDeclarationCount++;
                    if (inDeclarationCount == 2)
                    {
                        inDeclarationCount = 0;
                        if (gbxHeading.Visible == true)
                        {
                            txtHeading1.Focus();
                        }
                        else
                        {
                            cbxActive.Focus();
                        }
                    }
                }
                else
                {
                    inDeclarationCount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDeclaration.Text.Trim() == string.Empty || txtDeclaration.SelectionStart == 0)
                    {
                        cmbDotMatrix.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT43:" + ex.Message;
            }
        }
        /// <summary>
        /// heading 1 textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHeading1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtHeading2.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtHeading1.Text == string.Empty || txtHeading1.SelectionStart == 0)
                    {
                        txtDeclaration.Focus();
                        txtDeclaration.SelectionStart = 0;
                        txtDeclaration.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "VT44:" + ex.Message;
            }
        }
        /// <summary>
        /// heading 2 textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHeading2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtHeading3.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtHeading2.Text == string.Empty || txtHeading2.SelectionStart == 0)
                    {
                        txtHeading1.Focus();
                        txtHeading1.SelectionStart = 0;
                        txtHeading1.SelectionLength = 0;

                    }
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "VT45:" + ex.Message;
            }
        }
        /// <summary>
        /// heading 3 textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHeading3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtHeading4.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtHeading3.Text == string.Empty || txtHeading3.SelectionStart == 0)
                    {
                        txtHeading2.Focus();
                        txtHeading2.SelectionStart = 0;
                        txtHeading2.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "VT46:" + ex.Message;
            }
        }
        /// <summary>
        /// heading 4 textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtHeading4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxActive.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtHeading4.Text == string.Empty || txtHeading4.SelectionStart == 0)
                    {
                        txtHeading3.Focus();
                        txtHeading3.SelectionStart = 0;
                        txtHeading3.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "VT47:" + ex.Message;
            }
        }
        /// <summary>
        /// textbox declaration key enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDeclaration_Enter(object sender, EventArgs e)
        {
            try
            {
                inDeclarationCount = 0;
                txtDeclaration.Text = txtDeclaration.Text.Trim();
                if (txtDeclaration.Text.Trim() == string.Empty)
                {
                    txtDeclaration.SelectionStart = 0;
                    txtDeclaration.SelectionLength = 0;
                    txtDeclaration.Focus();
                }
                else
                {
                    txtDeclaration.SelectionStart = txtDeclaration.Text.Trim().Length;
                    txtDeclaration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "VT48:" + ex.Message;
            }
        }
        #endregion
    }
}
