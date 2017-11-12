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
    

    public partial class frmProductGroup : Form
    {

        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        frmProductCreation frmProductCreationObj;
        frmMultipleProductCreation frmMultipleProductCreationObj;
        decimal decId;
        decimal decIdForOtherForms = 0;
        string strProductGroupName;
        decimal inNarrationcount;

        #endregion

        #region Functions

        /// <summary>
        /// Create instance of frmProductGroup
        /// </summary>
        public frmProductGroup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to fill GroupUnderComboBox
        /// </summary>
        public void ProductGroupUnderComboFill()
        {
            try
            {

                ProductGroupSP spProductGroup = new ProductGroupSP();
                DataTable dtbl = new DataTable();
                dtbl = spProductGroup.ProductGroupViewForComboFillForProductGroup();
                cmbUnder.DataSource = dtbl;
                cmbUnder.ValueMember = "GroupId";
                cmbUnder.DisplayMember = "GroupName";

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG1:" + ex.Message;
              
            }
        }

        /// <summary>
        /// Function to fill GroupUnder combobox for search
        /// </summary>
        public void ProductGroupUnderComboFillForSearch()
        {
            try
            {

                ProductGroupSP spProductGroup = new ProductGroupSP();
                DataTable dtbl = new DataTable();
                dtbl = spProductGroup.ProductGroupViewForComboFill();
                cmbUnderSearch.DataSource = dtbl;
                cmbUnderSearch.ValueMember = "GroupId";
                cmbUnderSearch.DisplayMember = "GroupName";

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to determine whether to call save or edit function and also checking invalid entries 
        /// </summary>
        private void SaveOrEdit()
        {
            try
            {
                if (txtProductGroupName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter product group name");
                    txtProductGroupName.Focus();
                }
                else if (cmbUnder.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select under");
                    cmbUnder.Focus();
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


                        if (frmMultipleProductCreationObj != null)
                        {
                            this.Close();
                        }


                        if (frmProductCreationObj != null)
                        {
                            this.Close();
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
                formMDI.infoError.ErrorString = "PG3:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to save new productgroup
        /// </summary>
        private void SaveFunction()
        {
            try
            {
                ProductGroupInfo infoProductGroup = new ProductGroupInfo();
                ProductGroupSP spProductGroup = new ProductGroupSP();
                infoProductGroup.GroupName = txtProductGroupName.Text.Trim();
                infoProductGroup.GroupUnder = Convert.ToDecimal(cmbUnder.SelectedValue.ToString());
                infoProductGroup.Narration = txtNarration.Text.Trim();
                infoProductGroup.Extra1 = string.Empty;
                infoProductGroup.Extra2 = string.Empty;
               
                            
                if (spProductGroup.ProductGroupCheckExistence(txtProductGroupName.Text.Trim(), 0) == false)
                {
                    decIdForOtherForms = spProductGroup.ProductGroupAdd(infoProductGroup);
                    Messages.SavedMessage();
                    ProductGroupUnderComboFill();
                    ProductGroupUnderComboFillForSearch();
                    GridFill();
                    Clear();
                    txtProductGroupName.Focus();
                }
                else
                {
                    Messages.InformationMessage("Product group already exists");
                    txtProductGroupName.Focus();
                }


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG4:" + ex.Message;
            }

        }

        /// <summary>
        /// Function To edit existing productgroup
        /// </summary>
        private void EditFunction()
        {
            try
            {
                ProductGroupInfo infoProductGroup = new ProductGroupInfo();
                ProductGroupSP spProductGroup = new ProductGroupSP();
                infoProductGroup.GroupId = Convert.ToDecimal(dgvProductGroup.CurrentRow.Cells["dgvtxtgroupId"].Value);
                infoProductGroup.GroupName = txtProductGroupName.Text.Trim();
                infoProductGroup.GroupUnder = Convert.ToDecimal(cmbUnder.SelectedValue.ToString());
                infoProductGroup.Narration = txtNarration.Text.Trim();
                infoProductGroup.Extra1 = string.Empty;
                infoProductGroup.Extra2 = string.Empty;

                if (spProductGroup.ProductGroupCheckExistence(txtProductGroupName.Text.Trim().ToString(), decId) == false)
                {
                    spProductGroup.ProductGroupEdit(infoProductGroup);
                    Messages.UpdatedMessage();
                    ProductGroupUnderComboFill();
                    ProductGroupUnderComboFillForSearch();
                    GridFill();
                    Clear();
                    txtProductGroupName.Focus();
                }
                else
                {
                    Messages.InformationMessage(" Product group already exists");
                    txtProductGroupName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG5:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to delete productgroup
        /// </summary>
        private void Delete()
        {
            try
            {
                if (PublicVariables.isMessageDelete)
                {
                    if (Messages.DeleteMessage() == true)
                    {
                        ProductGroupInfo InfoProductGroup = new ProductGroupInfo();
                        ProductGroupSP spProductGroup = new ProductGroupSP();
                        if ((spProductGroup.ProductGroupReferenceDelete(decId) == -1))
                        {
                            Messages.ReferenceExistsMessage();
                        }
                        else
                        {
                            Messages.DeletedMessage();
                            btnSave.Text = "Save";
                            btnDelete.Enabled = false;
                            ProductGroupUnderComboFillForSearch();
                            Clear();
                        }
                    }
                }
                else
                {
                    ProductGroupInfo InfoProductGroup = new ProductGroupInfo();
                    ProductGroupSP spProductGroup = new ProductGroupSP();
                    if ((spProductGroup.ProductGroupReferenceDelete(decId) == -1))
                    {
                        Messages.ReferenceExistsMessage();
                    }
                    else
                    {
                        Messages.DeletedMessage();
                        btnSave.Text = "Save";
                        btnDelete.Enabled = false;
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG6:" + ex.Message;
            }
        }

       /// <summary>
       /// Function to clear the fields
       /// </summary>
        public void Clear()
        {
            try
            {
                txtProductGroupName.Clear();
                cmbUnder.SelectedIndex = -1;
                txtNarration.Clear();
                ProductGroupUnderComboFill();
                txtProductGroupSearch.Clear();
                cmbUnderSearch.SelectedIndex = -1;
                GridFill();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                cmbUnder.Enabled = true;
                txtProductGroupName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG7:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the grid to view all the product groups
        /// </summary>
        private void GridFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ProductGroupSP spProducttGroup = new ProductGroupSP();
                ProductGroupInfo info = new ProductGroupInfo();
                if (cmbUnderSearch.Text == "")
                {
                    cmbUnderSearch.Text = "All";
                }

                dtbl = spProducttGroup.ProductGroupViewForGridFill(txtProductGroupSearch.Text, cmbUnderSearch.Text);
                dgvProductGroup.DataSource = dtbl;


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG8:" + ex.Message;
            }

        }

        /// <summary>
        /// Function to fill the fields to edit or delete
        /// </summary>
        private void FillControls()
        {
            try
            {
                ProductGroupInfo infoProductGroup = new ProductGroupInfo();
                ProductGroupSP spProductGroup = new ProductGroupSP();
                infoProductGroup = spProductGroup.ProductGroupView(decId);
                txtProductGroupName.Text = infoProductGroup.GroupName;
                DataTable dtbl = new DataTable();
                dtbl = spProductGroup.ProductGroupViewForComboFillForProductGroup();
                cmbUnder.DataSource = dtbl;
                foreach (DataRow dr in dtbl.Rows)
                {
                    if (dr["GroupName"].ToString() == txtProductGroupName.Text)
                        dr.Delete();
                }
                cmbUnder.ValueMember = "GroupId";
                cmbUnder.DisplayMember = "GroupName";
                cmbUnder.SelectedValue = infoProductGroup.GroupUnder;
                if (spProductGroup.ProductGroupCheckExistenceOfUnderGroup(decId) == false)
                {
                    cmbUnder.Enabled = false;
                }
                else
                {
                    cmbUnder.Enabled = true;
                }
                txtNarration.Text = infoProductGroup.Narration;


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG9:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to check whether the groupname exist or not while save or edit group
        /// </summary>
        /// <returns></returns>
        public bool CheckExistanceOfGroupName()
        {
            bool isExist = false;
            try
            {
                //Check whether a group name already exist in DB
                ProductGroupSP spProductGroup = new ProductGroupSP();

                isExist = spProductGroup.ProductGroupCheckExistence(txtProductGroupName.Text.Trim(), 0);
                if (isExist)
                {
                    if (txtProductGroupName.Text.ToLower() == strProductGroupName.ToLower())
                    {
                        isExist = false;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG10:" + ex.Message;
            }

            return isExist;


        }

        /// <summary>
        /// Function to load this form when calling from frmProductCreation to add new productgroup
        /// </summary>
        /// <param name="frmProduct"></param>
        public void CallFromProdutCreation(frmProductCreation frmProduct)
        {
            try
            {
                this.frmProductCreationObj = frmProduct;
                groupBox2.Enabled = false;
                base.Show();


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG11:" + ex.Message;
            }
        }

        
        /// <summary>
        /// Function to load this form when calling from frmMultipleProdutCreation to add new productgroup
        /// </summary>
        /// <param name="frmMultipleProductCreation"></param>
        public void CallFromMultipleProdutCreation(frmMultipleProductCreation frmMultipleProductCreation)
        {
            try
            {
                dgvProductGroup.Enabled = false;
                txtProductGroupSearch.Enabled = false;
                cmbUnderSearch.Enabled = false;
                btnSearch.Enabled = false;
                this.frmMultipleProductCreationObj = frmMultipleProductCreation;
                base.Show();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG12:" + ex.Message;
            }
        }

        #endregion

        #region Events
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
                formMDI.infoError.ErrorString = "PG13:" + ex.Message;
            }
        }

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductGroup_Load(object sender, EventArgs e)
        {
            try
            {
                btnDelete.Enabled = false;
                ProductGroupUnderComboFill();
                ProductGroupUnderComboFillForSearch();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG14:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PG15:" + ex.Message;
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
                formMDI.infoError.ErrorString = "PG16:" + ex.Message;
            }

        }

        /// <summary>
        /// On Search button click
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
                formMDI.infoError.ErrorString = "PG17:" + ex.Message;
            }
        }

        /// <summary>
        /// On double clicking the datagridview, it fills the details of selected row for edit or delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductGroup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    decId = Convert.ToDecimal(dgvProductGroup.CurrentRow.Cells["dgvtxtgroupId"].Value);
                    FillControls();
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                    txtProductGroupName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG18:" + ex.Message;
            }

        }

        /// <summary>
        /// On clicking delete button
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
                formMDI.infoError.ErrorString = "PG19:" + ex.Message;
            }
        }

        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmProductCreationObj != null)
                {
                    frmProductCreationObj.ReturnFromProductGroupForm(decIdForOtherForms);
                    groupBox2.Enabled = true;
                }

                if (frmMultipleProductCreationObj != null)
                {
                    frmMultipleProductCreationObj.ReturnFromProductGroupForm(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG20:" + ex.Message;
            }
        }


        #endregion

        #region Navigation

        /// <summary>
        /// For shortcut keys
        /// Esc for closing the form
        /// ctrl+s for save
        /// ctrl+d for delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose)
                    {
                        btnClose_Click(sender, e);
                    }
                    else
                    {
                        this.Close();
                    }
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control) //Delete
                {
                    if (btnDelete.Enabled == true)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG21:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key and backspace navigation of groupunder combobox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUnder_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbUnder.SelectionStart == 0 || cmbUnder.Text == string.Empty)
                    {
                        txtProductGroupName.Focus();
                        txtProductGroupName.SelectionStart = 0;
                        txtProductGroupName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG22:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key and backspace navigation of txtnarration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    inNarrationcount++;
                    if (inNarrationcount == 2)
                    {
                        inNarrationcount = 0;
                        btnSave.Focus();
                    }
                }
                else
                {
                    inNarrationcount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        cmbUnder.Focus();

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG23:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of btnSave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SaveOrEdit();
                }
                if (e.KeyCode == Keys.Back)
                {

                    txtNarration.Focus();
                    txtNarration.SelectionLength = 0;
                    txtNarration.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG24:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key navigation of txtProductGroupName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbUnder.Enabled == true)
                    {
                        cmbUnder.Focus();
                    }
                    else
                    {
                        txtNarration.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG25:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key navigation of txtProductGroupSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductGroupSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbUnderSearch.Focus();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG26:" + ex.Message;
            }

        }

        /// <summary>
        /// For Enter key and Backspace navigation of GroupUnder Combobox for search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUnderSearch_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductGroupSearch.Text == string.Empty)
                    {
                        txtProductGroupSearch.Focus();
                    }
                    else if (txtProductGroupSearch.Text != string.Empty)
                    {
                        txtProductGroupSearch.Focus();
                        txtProductGroupSearch.SelectionStart = 0;
                        txtProductGroupSearch.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG27:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key and backspace navigation of Search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    GridFill();
                    dgvProductGroup.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbUnderSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG28:" + ex.Message;
            }
        }

        /// <summary>
        /// for the backspace navigation of dgvproductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbUnderSearch.Focus();
                    cmbUnderSearch.SelectionStart = 0;
                    cmbUnderSearch.SelectionLength = 0;

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG29:" + ex.Message;
            }
        }

        /// <summary>
        /// To perform CelldoubleClick on enter key in datagridview
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductGroup_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProductGroup.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvProductGroup.CurrentCell.ColumnIndex, dgvProductGroup.CurrentCell.RowIndex);
                        dgvProductGroup_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PG30:" + ex.Message;
            }
        }

       
       /// <summary>
       /// For Enter key navigation of txtNarration
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationcount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == string.Empty)
                {
                    txtNarration.SelectionStart = 0;
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
                formMDI.infoError.ErrorString = "PG31:" + ex.Message;
            }
        }

        #endregion


    }
}
