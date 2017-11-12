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
   
    public partial class frmProductRegister : Form
    {
       
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        int inCurrenRowIndex = 0;
        TransactionsGeneralFill TrGeneralFill = new TransactionsGeneralFill();
        DataTable dtbl = new DataTable();

        #endregion

        #region Functions
        /// <summary>
        /// Create instance of frmProductRegister
        /// </summary>
        public frmProductRegister()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbProductGroup.SelectedIndex = 0;
                txtProductName.Clear();
                txtProductCode.Clear();
                cmbSize.SelectedIndex = 0;
                cmbModelNo.SelectedIndex = 0;
                cmbBrand.SelectedIndex = 0;
                cmbTax.SelectedIndex = 0;
                cmbTaxApplicableOn.SelectedIndex = 0;
                txtSalesRateFrom.Clear();
                txtSalesRateTo.Clear();
                cmbActive.SelectedIndex = 0;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR1:" + ex.Message;
               
            }
        }

        /// <summary>
        /// Function to fill the productgroup combobox
        /// </summary>
        public void ProductGroupComboFill()
        {
            try
            {
                dtbl = TrGeneralFill.ProductGroupViewAll(cmbProductGroup, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the Size combobox
        /// </summary>
        public void SizeComboFill()
        {
            try
            {
                dtbl = TrGeneralFill.SizeViewAll(cmbSize, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR3:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the ModelNumber combobox
        /// </summary>
        public void ModelNoComboFill()
        {
            try
            {
                dtbl = TrGeneralFill.ModelNoViewAll(cmbModelNo, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR4:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill brand combobox
        /// </summary>
        public void BrandComboFill()
        {
            try
            {
                dtbl = TrGeneralFill.BrandViewAll(cmbBrand, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR5:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill tax combobox
        /// </summary>
        public void TaxComboFill()
        {
            try
            {
                dtbl = TrGeneralFill.TaxViewAll(cmbTax,true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR6:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the grid based on the search keys
        /// </summary>
        public void GridFill()
        {
            try
            {
                string strComboText = string.Empty;
                DataTable dtbl = new DataTable();
                ProductSP spProduct = new ProductSP();
                ProductInfo infoProduct = new ProductInfo();
                if (cmbActive.Text == "Yes")
                {
                    strComboText = "True";
                }
                else if (cmbActive.Text == "No")
                {
                    strComboText = "False";
                }
                else
                {
                    cmbActive.SelectedIndex = 0;
                    strComboText = "All";
                }
                if (txtSalesRateFrom.Text == string.Empty && txtSalesRateTo.Text == string.Empty)
                {
                    txtSalesRateFrom.Text = "0";
                    txtSalesRateTo.Text = "0";
                }
                decimal decProductGroup = Convert.ToDecimal(cmbProductGroup.SelectedValue);
                string strProductName = txtProductName.Text.Trim();
                string strProductCode = txtProductCode.Text.Trim();
                decimal decSize = Convert.ToDecimal(cmbSize.SelectedValue);
                decimal decModelNo = Convert.ToDecimal(cmbModelNo.SelectedValue);
                decimal decBrand = Convert.ToDecimal(cmbBrand.SelectedValue);
                decimal decTax = Convert.ToDecimal(cmbTax.SelectedValue);
                string strTaxApplicable = cmbTaxApplicableOn.Text.Trim();
                decimal decSalesRateFrom = Convert.ToDecimal(txtSalesRateFrom.Text);
                decimal decSalesRateTo = Convert.ToDecimal(txtSalesRateTo.Text);

                dtbl = spProduct.ProductRegisterSearch(decProductGroup, strProductName, strProductCode, decSize, decModelNo, decBrand, decTax, strTaxApplicable, decSalesRateFrom, decSalesRateTo, strComboText);

                dgvProductRegister.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR7:" + ex.Message;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// On close button click
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
                formMDI.infoError.ErrorString = "PR8:" + ex.Message;
            }
        }

        /// <summary>
        /// On Clear button click
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
                formMDI.infoError.ErrorString = "PR9:" + ex.Message;
            }
        }

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductRegister_Load(object sender, EventArgs e)
        {
            try
            {
                cmbProductGroup.SelectionLength = 0;
                cmbActive.SelectedIndex = 0;
                cmbTaxApplicableOn.SelectedIndex = 0;
                ProductGroupComboFill();
                SizeComboFill();
                ModelNoComboFill();
                BrandComboFill();
                TaxComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR10:" + ex.Message;
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
                if (txtSalesRateFrom.Text == string.Empty)
                {
                    txtSalesRateFrom.Text = "0";
                }
                if (txtSalesRateTo.Text == string.Empty)
                {
                    txtSalesRateTo.Text = "0";
                }
                if (Convert.ToDecimal(txtSalesRateFrom.Text) > Convert.ToDecimal(txtSalesRateTo.Text))
                {
                    Messages.InformationMessage("Salesrateto should be greater than salesratefrom");
                    txtSalesRateTo.Focus();
                }
                else
                {
                    GridFill();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR11:" + ex.Message;
            }
        }

        /// <summary>
        /// On doubleclicking the grid, it dispalys productcreation form and fill the correspnding product details to edit or delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductRegister_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             try
            {
                if (e.RowIndex != -1 && e.ColumnIndex > -1)
                {
                if (dgvProductRegister.CurrentRow != null)
                {
                    if (dgvProductRegister.Rows.Count > 0 )
                    {
                        if (dgvProductRegister.CurrentRow.Cells["dgvtxtproductId"].Value != null)
                        {
                            if (dgvProductRegister.CurrentRow.Cells["dgvtxtproductId"].Value.ToString() != "")
                            {
                                frmProductCreation objfrmProductCreation;
                                decimal decRegister = Convert.ToDecimal(dgvProductRegister.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value);

                                
                                    if (dgvProductRegister.CurrentRow.Index == e.RowIndex)
                                   {
                                        objfrmProductCreation = Application.OpenForms["frmProductCreation"] as frmProductCreation;
                                        if (objfrmProductCreation == null)
                                        {
                                            objfrmProductCreation = new frmProductCreation();
                                            objfrmProductCreation.MdiParent = formMDI.MDIObj;
                                            objfrmProductCreation.CallFromProductRegister(decRegister, this);
                                            GridFill();
                                        }
                                        else
                                        {
                                            objfrmProductCreation.CallFromProductRegister(decRegister, this);
                                        }
                                        if (dgvProductRegister.CurrentRow != null)
                                        {
                                            inCurrenRowIndex = dgvProductRegister.CurrentRow.Index;
                                       }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR12:" + ex.Message;
            }
        }

        /// <summary>
        /// On ViewDetails button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductRegister.SelectedRows.Count > 0)
                {
                    frmProductCreation objfrmProductCreation;
                    decimal decRegister=Convert.ToDecimal(dgvProductRegister.CurrentRow.Cells["dgvtxtproductId"].Value.ToString());

                    objfrmProductCreation = Application.OpenForms["frmProductCreation"] as frmProductCreation;
                    if (objfrmProductCreation == null)
                    {
                        objfrmProductCreation = new frmProductCreation();
                        objfrmProductCreation.MdiParent = formMDI.MDIObj;
                        objfrmProductCreation.CallFromProductRegister(decRegister, this);
                    }
                    else
                    {
                        objfrmProductCreation.CallFromProductRegister(decRegister, this);
                    }
                    inCurrenRowIndex = dgvProductRegister.CurrentRow.Index;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR13:" + ex.Message;
            }
        }

        /// <summary>
        /// For avoiding invalid entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRateForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR14:" + ex.Message;
            }
        }

        /// <summary>
        /// For avoiding invalid entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRateTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR15:" + ex.Message;
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Enter key navigation for ProductGroup combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR16:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for txtProductName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductName.Text == string.Empty || txtProductName.SelectionStart == 0)
                    {
                        cmbProductGroup.SelectionStart = 0;
                        cmbProductGroup.SelectionLength = 0;
                        cmbProductGroup.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR17:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for txtProductCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbSize.Enabled)
                    {
                        cmbSize.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        txtProductName.SelectionStart = 0;
                        txtProductName.SelectionLength = 0;
                        txtProductName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR18:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for cmbSize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbModelNo.Enabled)
                    {
                        cmbModelNo.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbSize.SelectionStart == 0 || cmbSize.Text == string.Empty)
                    {
                        txtProductCode.SelectionStart = 0;
                        txtProductCode.SelectionLength = 0;
                        txtProductCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR19:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for cmbModelNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModelNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbBrand.Enabled)
                    {
                        cmbBrand.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbModelNo.SelectionStart == 0 || cmbModelNo.Text == string.Empty)
                    {
                        cmbSize.SelectionStart = 0;
                        cmbSize.SelectionLength = 0;
                        cmbSize.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR20:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for cmbBrand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbTax.Enabled)
                    {
                        cmbTax.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbBrand.SelectionStart == 0 || cmbBrand.Text == string.Empty)
                    {
                        cmbModelNo.SelectionStart = 0;
                        cmbModelNo.SelectionLength = 0;
                        cmbModelNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR21:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for cmbBrand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbTaxApplicableOn.Enabled)
                    {
                        cmbTaxApplicableOn.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbTax.SelectionStart == 0 || cmbTax.Text == string.Empty)
                    {
                        cmbBrand.SelectionStart = 0;
                        cmbBrand.SelectionLength = 0;
                        cmbBrand.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR22:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for cmbTaxApplication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTaxApplicationon_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSalesRateFrom.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbTaxApplicableOn.SelectionStart == 0 || cmbTaxApplicableOn.Text == string.Empty)
                    {
                        cmbTax.SelectionStart = 0;
                        cmbTax.SelectionLength = 0;
                        cmbTax.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR23:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for txtSalesRateFrom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRateForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSalesRateTo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSalesRateFrom.SelectionStart == 0 || txtSalesRateFrom.Text == string.Empty)
                    {
                        cmbTaxApplicableOn.SelectionStart = 0;
                        cmbTaxApplicableOn.SelectionLength = 0;
                        cmbTaxApplicableOn.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR24:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for txtSalesRateTo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRateTo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbActive.Enabled)
                    {
                        cmbActive.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSalesRateTo.SelectionStart == 0 || txtSalesRateTo.Text == string.Empty)
                    {
                        txtSalesRateFrom.SelectionStart = 0;
                        txtSalesRateFrom.SelectionLength = 0;
                        txtSalesRateFrom.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR25:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for cmbStatus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbActive.SelectionStart == 0 || cmbActive.Text == string.Empty)
                    {
                        txtSalesRateTo.SelectionStart = 0;
                        txtSalesRateTo.SelectionLength = 0;
                        txtSalesRateTo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR26:" + ex.Message;
            }
        }

        /// <summary>
        /// For closing on Escape key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductRegister_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose == true)
                    {
                        Messages.CloseMessage(this);
                    }
                    else
                    {
                        btnClose_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR27:" + ex.Message;
            }
        }

        /// <summary>
        /// backspace navigation for btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbActive.SelectionStart == 0 || cmbActive.Text == string.Empty)
                    {
                        cmbActive.SelectionStart = 0;
                        cmbActive.SelectionLength = 0;
                        cmbActive.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR28:" + ex.Message;
            }
        }

        /// <summary>
        /// backspace navigation for btnClear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbActive.SelectionStart == 0 || cmbActive.Text == string.Empty)
                    {
                        cmbActive.SelectionStart = 0;
                        cmbActive.SelectionLength = 0;
                        cmbActive.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR29:" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation for dgvProductRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductRegister_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbActive.SelectionStart == 0 || cmbActive.Text == string.Empty)
                    {
                        cmbActive.SelectionStart = 0;
                        cmbActive.SelectionLength = 0;
                        cmbActive.Focus();
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProductRegister.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvProductRegister.CurrentCell.ColumnIndex, dgvProductRegister.CurrentCell.RowIndex);
                        dgvProductRegister_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PR30:" + ex.Message;
            }
        }
        #endregion

    }
}
