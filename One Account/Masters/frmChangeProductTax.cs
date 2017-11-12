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
    public partial class frmChangeProductTax : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        int inGridRowCount = 0;  //  Number of rows in grid
        bool isDefault = false; //  Set TRUE when checkbox is checked in the grid           
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmChangeProductTax class
        /// </summary>
        public frmChangeProductTax()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Funcion for Clear and Gridfill
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbSearchBy.SelectedIndex = 0;
                ProductGroupTaxComboFill();
                txtProductCode.Text = string.Empty;
                txtProductName.Text = string.Empty;
                cmbSearchBy.Enabled = true;
                cmbProductGroupTax.Enabled = true;
                NewTaxTypeComboFill();
                cmbNewTaxType.SelectedIndex = 0;
                cmbNewTaxType.Enabled = true;
                cmbSearchBy.Focus();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 1 : " + ex.Message;
                
            }
        }
        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                foreach (DataGridViewRow dgvRow in dgvChangeProductTax.Rows)
                {
                    if (dgvRow.Cells["dgvtxtSelect"].Value != null)
                    {
                        if ((bool)(dgvRow.Cells["dgvtxtSelect"].Value))
                        {
                            int inProductId = Convert.ToInt32(dgvChangeProductTax.Rows[dgvRow.Index].Cells["dgvtxtProductId"].Value.ToString());
                            ProductSP spProduct = new ProductSP();
                            ProductInfo infoProduct = new ProductInfo();
                            infoProduct.TaxId = Convert.ToInt32(cmbNewTaxType.SelectedValue.ToString());
                            infoProduct.ProductId = inProductId;
                            spProduct.ChangeProductTaxSave(infoProduct);
                            isDefault = false;
                        }
                    }
                }
                Messages.SavedMessage();
                cmbSearchBy.Focus();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 2 : " + ex.Message;
            }
        }
        /// <summary>
        /// Function to call save
        /// </summary>
        public void Save()
        {
            try
            {
                int inRowCount = dgvChangeProductTax.RowCount;
                for (int ini = 0; ini < inRowCount; ini++)
                {
                    //Checking checkbox is checked or not
                    if (dgvChangeProductTax.Rows[ini].Cells["dgvtxtSelect"].Value != null && (bool)(dgvChangeProductTax.Rows[ini].Cells["dgvtxtSelect"].Value) != false)
                    {
                        isDefault = Convert.ToBoolean(dgvChangeProductTax.Rows[ini].Cells["dgvtxtSelect"].Value.ToString());
                    }
                }
                if (!isDefault)
                {
                    Messages.InformationMessage("Select product");
                    dgvChangeProductTax.Focus();
                    dgvChangeProductTax.CurrentCell = dgvChangeProductTax.Rows[0].Cells["dgvtxtSelect"];
                }
                else
                {
                    if (PublicVariables.isMessageAdd)
                    {
                        if (Messages.SaveMessage())
                        {
                            SaveFunction();
                        }
                        else
                        {
                            isDefault = false;
                        }
                    }
                    else
                    {
                        SaveFunction();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 3 : " + ex.Message;
            }
        }
        /// <summary>
        /// Function to Fill new tax type combo box
        /// </summary>
        public void NewTaxTypeComboFill()
        {
            try
            {
                TaxSP spTax = new TaxSP();
                DataTable dtblTax = new DataTable();
                dtblTax = spTax.TaxViewAll();
                cmbNewTaxType.DataSource = dtblTax;
                cmbNewTaxType.ValueMember = "taxId";
                cmbNewTaxType.DisplayMember = "taxName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 4 : " + ex.Message;
            }
        }
        /// <summary>
        /// Function to Fill product group or tax according to search by combo box selection in Product Group/Tax combo box 
        /// </summary>
        public void ProductGroupTaxComboFill()
        {
            try
            {
                if (cmbSearchBy.SelectedIndex == 0)
                {
                    ProductGroupSP spProductGroup = new ProductGroupSP();
                    DataTable dtblProductGroup = new DataTable();
                    dtblProductGroup = spProductGroup.ProductGroupViewAll();
                    cmbProductGroupTax.DataSource = dtblProductGroup;
                    cmbProductGroupTax.ValueMember = "groupId";
                    cmbProductGroupTax.DisplayMember = "groupName";
                }
                else
                {
                    TaxSP spTax = new TaxSP();
                    DataTable dtblTax = new DataTable();
                    dtblTax = spTax.TaxViewAll();
                    cmbProductGroupTax.DataSource = dtblTax;
                    cmbProductGroupTax.ValueMember = "taxId";
                    cmbProductGroupTax.DisplayMember = "taxName";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 5 : " + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                ProductInfo infoProduct = new ProductInfo();
                int inSelect = 0;
                if (cmbSearchBy.SelectedIndex == 0)
                {
                    infoProduct.GroupId = Convert.ToInt32(cmbProductGroupTax.SelectedValue.ToString());
                }
                else
                {
                    inSelect = 1;
                    infoProduct.TaxId = Convert.ToInt32(cmbProductGroupTax.SelectedValue.ToString());
                }
                infoProduct.ProductCode = txtProductCode.Text.Trim();
                infoProduct.ProductName = txtProductName.Text.Trim();
                DataTable dtbl = spProduct.ChangeProductTaxSearch(infoProduct, inSelect);
                dgvChangeProductTax.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 6 : " + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        public void FormClose()
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
                formMDI.infoError.ErrorString = "CPT 7 : " + ex.Message; ;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On 'Close' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                FormClose();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 8 : " + ex.Message;
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
                formMDI.infoError.ErrorString = "CPT 9 : " + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangeProductTax_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 10 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'SearchBy' combobox selected index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ProductGroupTaxComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 11 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'searc' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridFill();
                cmbNewTaxType.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 12 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Selectall' link clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int inRowCount = dgvChangeProductTax.RowCount;
                if (inRowCount > 0)
                {
                    for (int ini = 0; ini < inRowCount; ini++)
                    {
                        dgvChangeProductTax.Rows[ini].Cells["dgvtxtSelect"].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 13 : " + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Clearall' link clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int inRowCount = dgvChangeProductTax.RowCount;
                if (inRowCount > 0)
                {
                    for (int ini = 0; ini < inRowCount; ini++)
                    {
                        dgvChangeProductTax.Rows[ini].Cells["dgvtxtSelect"].Value = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 14 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'save' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    if (dgvChangeProductTax.RowCount < 1)
                    {
                        Messages.InformationMessage("There is no items in the grid");
                        cmbSearchBy.Focus();
                    }
                    else
                    {
                        Save();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 15 : " + ex.Message; ;
            }
        }
        #endregion
        #region Navigation
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
                    dgvChangeProductTax.Focus();
                    dgvChangeProductTax.ClearSelection();
                    dgvChangeProductTax.CurrentCell = dgvChangeProductTax.Rows[dgvChangeProductTax.Rows.Count - 1].Cells["dgvtxtSelect"];
                    dgvChangeProductTax.Rows[dgvChangeProductTax.Rows.Count - 1].Cells["dgvtxtSelect"].Selected = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 16 : " + ex.Message;
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
                if (e.KeyCode == Keys.Enter)
                {
                    cmbNewTaxType.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtProductName.Focus();
                    txtProductName.SelectionLength = 0;
                    txtProductName.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 17 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'SelectAll' link button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSelectAll_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbNewTaxType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 18 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'SearchBy' combobox keydown
        /// </summary
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSearchBy_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbProductGroupTax.Enabled)
                    {
                        cmbProductGroupTax.Focus();
                    }
                    else
                    {
                        txtProductCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 19 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'ProductGroupTax' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroupTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbProductGroupTax.SelectionStart == 0 || cmbProductGroupTax.Text == string.Empty)
                    {
                        cmbSearchBy.Focus();
                        cmbSearchBy.SelectionStart = 0;
                        cmbSearchBy.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 20 : " + ex.Message; ;
            }
        }
        /// <summary>
        /// On 'ProductCode' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        cmbProductGroupTax.Focus();
                        cmbProductGroupTax.SelectionStart = 0;
                        cmbProductGroupTax.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 21 : " + ex.Message;
            }
        }
        /// <summary>
        ///  On 'ProductName' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductName.Text == string.Empty || txtProductName.SelectionStart == 0)
                    {
                        txtProductCode.SelectionStart = 0;
                        txtProductCode.SelectionLength = 0;
                        txtProductCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 22 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Tab)
                {
                    cmbSearchBy.Focus();
                    cmbSearchBy.SelectionLength = 0;
                    cmbSearchBy.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnClear.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 23 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear' button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvChangeProductTax.RowCount > 1)
                    {
                        btnSave.Focus();
                    }
                    else
                    {
                        cmbNewTaxType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 24 : " + ex.Message;
            }
        }
        /// <summary>
        /// On Datagridview keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvChangeProductTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvChangeProductTax.Rows.Count > 0)
                    {
                        if ((dgvChangeProductTax.CurrentCell == dgvChangeProductTax[dgvChangeProductTax.Columns["dgvtxtSelect"].Index, dgvChangeProductTax.Rows.Count - 1]) || dgvChangeProductTax.CurrentRow.Index == dgvChangeProductTax.RowCount - 1)
                        {
                            if (inGridRowCount == 1)
                            {
                                inGridRowCount = 0;
                                btnSave.Focus();
                                dgvChangeProductTax.ClearSelection();
                            }
                            else
                            {
                                inGridRowCount = 1;
                            }
                        }
                    }
                    else
                    {
                        btnSave.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvChangeProductTax.CurrentCell == dgvChangeProductTax[dgvChangeProductTax.Columns["dgvtxtSelect"].Index, 0])
                    {
                        cmbNewTaxType.Focus();
                        cmbNewTaxType.SelectionStart = 0;
                        cmbNewTaxType.SelectionLength = 0;
                    }
                    else
                    {
                        dgvChangeProductTax.CurrentCell = dgvChangeProductTax[dgvChangeProductTax.Columns["dgvtxtSelect"].Index, dgvChangeProductTax.CurrentRow.Index - 1];
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 25 : " + ex.Message;
            }
        }
        /// <summary>
        /// On 'NewTaxType' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNewTaxType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvChangeProductTax.RowCount > 0)
                    {
                        lnklblSelectAll.Focus();
                        dgvChangeProductTax.Focus();
                        dgvChangeProductTax.CurrentCell = dgvChangeProductTax.Rows[0].Cells["dgvtxtSelect"];
                        dgvChangeProductTax.Rows[0].Cells["dgvtxtSelect"].Selected = true;
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbNewTaxType.Text == string.Empty || cmbNewTaxType.SelectionStart == 0)
                    {
                        txtProductName.Focus();
                        txtProductName.SelectionStart = 0;
                        txtProductName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 26 : " + ex.Message;
            }
        }
        /// <summary>
        /// form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangeProductTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    FormClose();
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save Ctrl + S
                {
                    if (dgvChangeProductTax.Focused)
                    {
                        btnSave.Focus();
                        dgvChangeProductTax.Focus();
                    }
                    if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                    {
                        if (cmbNewTaxType.Focused || cmbSearchBy.Focused || cmbProductGroupTax.Focused)
                        {
                            cmbNewTaxType.DropDownStyle = ComboBoxStyle.DropDown;
                            cmbProductGroupTax.DropDownStyle = ComboBoxStyle.DropDown;
                            cmbSearchBy.DropDownStyle = ComboBoxStyle.DropDown;
                        }
                        else
                        {
                            cmbNewTaxType.DropDownStyle = ComboBoxStyle.DropDownList;
                            cmbProductGroupTax.DropDownStyle = ComboBoxStyle.DropDownList;
                            cmbSearchBy.DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                        cmbNewTaxType.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbProductGroupTax.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbSearchBy.DropDownStyle = ComboBoxStyle.DropDownList;
                        btnSave_Click(sender, e);
                    }
                    else
                    {
                        Messages.NoPrivillageMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CPT 27 : " + ex.Message;
            }
        }
        #endregion
    }
}
