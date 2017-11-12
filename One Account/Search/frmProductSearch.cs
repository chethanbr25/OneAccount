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
    public partial class frmProductSearch : Form
    {
        #region Public variables
        /// <summary>
        /// Public variable declaration Part
        /// </summary>
        TransactionsGeneralFill TransactionsGeneralFillObj = new TransactionsGeneralFill();
        string strCondition = string.Empty;
        ProductSP spProduct = new ProductSP();
        #endregion
        #region Functions
        /// <summary>
        ///  Create an Instance for frmProductSearch class
        /// </summary>
        public frmProductSearch()
        {
            InitializeComponent();
        }
        /// <summary>
        /// The form will be reset here
        /// </summary>
        public void clear()
        {
            try
            {
                cmbProductGroup.Focus();
                txtProductCode.Text = string.Empty;
                txtProductName.Text = string.Empty;
                txtBatchName.Text = string.Empty;
                cmbStatus.SelectedIndex = 0;
                rbtnAll.Checked = true;
                ProductComboFill();
                BrandComboFill();
                GodownComboFill();
                SizeComboFill();
                TaxComboFill();
                RackComboFill();
                RackComboFillForLoad();
                ModelComboFill();
                ProductSearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:01" + ex.Message;
            }
        }
        /// <summary>
        /// ProductSearchGridFill function Based on the Search Criteria
        /// </summary>
        public void ProductSearchGridFill()
        {
            try
            {
                DataTable dtblProduct = new DataTable();
                string strCriteria = string.Empty;
                if (rbtnAll.Checked)
                {
                    strCriteria = "All";
                }
                if (rbtnMaxLevel.Checked)
                {
                    strCriteria = "Maximum Level";
                }
                if (rbtnMinLevel.Checked)
                {
                    strCriteria = "Minimum Level";
                }
                if (rbtnNegStock.Checked)
                {
                    strCriteria = "Negative Stock";
                }
                if (rbtnRecordLevel.Checked)
                {
                    strCriteria = "Reorder Level";
                }
                dtblProduct = spProduct.ProductSearchForGriddFill(Convert.ToDecimal(cmbGodown.SelectedValue), Convert.ToDecimal(cmbBrand.SelectedValue), Convert.ToDecimal(cmbModelNo.SelectedValue), Convert.ToDecimal(cmbRack.SelectedValue), Convert.ToDecimal(cmbSize.SelectedValue), Convert.ToDecimal(cmbTax.SelectedValue), Convert.ToDecimal(cmbProductGroup.SelectedValue), Convert.ToString(cmbStatus.Text), txtProductCode.Text, txtProductName.Text, strCriteria, txtBatchName.Text);
                dgvProductSearch.DataSource = dtblProduct;
                if (rbtnMinLevel.Checked)
                {
                    if (dgvProductSearch.Rows.Count > 0)
                    {
                        dgvProductSearch.Columns["MinimumStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvProductSearch.Columns["MinimumStock"].MinimumWidth = 95;
                    }
                }
                if (rbtnMaxLevel.Checked)
                {
                    if (dgvProductSearch.Rows.Count > 0)
                    {
                        dgvProductSearch.Columns["MaximumStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvProductSearch.Columns["MaximumStock"].MinimumWidth = 95;
                    }
                }
                if (rbtnRecordLevel.Checked)
                {
                    if (dgvProductSearch.Rows.Count > 0)
                    {
                        dgvProductSearch.Columns["ReorderLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvProductSearch.Columns["ReorderLevel"].MinimumWidth = 95;
                    }
                }
                
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:02" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill ProductCombobox
        /// </summary>
        public void ProductComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.ProductGroupViewAll(cmbProductGroup, true);
                cmbProductGroup.DataSource = dtbl;
                cmbProductGroup.DisplayMember = "groupName";
                cmbProductGroup.ValueMember = "groupId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:03" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Brand Combobox
        /// </summary>
        public void BrandComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.BrandViewAll(cmbBrand, true);
                cmbBrand.DataSource = dtbl;
                cmbBrand.DisplayMember = "brandName";
                cmbBrand.ValueMember = "brandId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:04" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Model Combobox
        /// </summary>
        public void ModelComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.ModelNoViewAll(cmbModelNo, true);
                cmbModelNo.DataSource = dtbl;
                cmbModelNo.DisplayMember = "modelNo";
                cmbModelNo.ValueMember = "modelNoId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:05" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Godown Combobox
        /// </summary>
        public void GodownComboFill()
        {
            try
            {
                GodownSP spGodown = new GodownSP();
                DataTable dtbl = new DataTable();
                dtbl = spGodown.GodownViewAll();
                DataRow drowSelect = dtbl.NewRow();
                drowSelect[0] = 0;
                drowSelect[1] = "All";
                dtbl.Rows.InsertAt(drowSelect, 0);
                cmbGodown.DataSource = dtbl;
                cmbGodown.DisplayMember = "godownName";
                cmbGodown.ValueMember = "godownId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:06" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Size Godown Combobox
        /// </summary>
        public void SizeComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.SizeViewAll(cmbSize, true);
                cmbSize.DataSource = dtbl;
                cmbSize.DisplayMember = "size";
                cmbSize.ValueMember = "sizeId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:07" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Tax Godown Combobox
        /// </summary>
        public void TaxComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TaxSP spTax = new TaxSP();
                dtbl = spTax.TaxViewAllForProduct();
                DataRow drowSelect = dtbl.NewRow();
                drowSelect[0] = 0;
                drowSelect["taxName"] = "All";
                dtbl.Rows.InsertAt(drowSelect, 0);
                cmbTax.DataSource = dtbl;
                cmbTax.ValueMember = "taxId";
                cmbTax.DisplayMember = "taxName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:08" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Rack Godown Combobox
        /// </summary>
        public void RackComboFill()
        {
            try
            {
                RackSP spRack = new RackSP();
                DataTable dtbl = new DataTable();
                if (cmbGodown.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    dtbl = spRack.RackFillForStock(Convert.ToDecimal(cmbGodown.SelectedValue.ToString()));
                    cmbRack.DataSource = dtbl;
                    cmbRack.DisplayMember = "rackName";
                    cmbRack.ValueMember = "rackId";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:09" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Rack Godown Combobox
        /// </summary>
        public void RackComboFillForLoad()
        {
            try
            {
                RackSP spRack = new RackSP();
                DataTable dtbl = new DataTable();
                if (cmbGodown.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    dtbl = spRack.RackFillForStock(Convert.ToDecimal(cmbGodown.SelectedValue.ToString()));
                    DataRow dr = dtbl.NewRow();
                    dr["rackName"] = "All";
                    dr["rackId"] = 0;
                    dtbl.Rows.InsertAt(dr, 0);
                    cmbRack.DataSource = dtbl;
                    cmbRack.DisplayMember = "rackName";
                    cmbRack.ValueMember = "rackId";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:10" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Close button Click
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
                formMDI.infoError.ErrorString = "PS:11" + ex.Message;
            }
        }
        /// <summary>
        /// Reset Button Click, to clear the form controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:12" + ex.Message;
            }
        }
        /// <summary>
        /// When form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductSearch_Load(object sender, EventArgs e)
        {
            try
            {
                clear();
                ProductSearchGridFill();
                cmbProductGroup.Focus();
                RackComboFillForLoad();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:13" + ex.Message;
            }
        }
        /// <summary>
        /// Godown index changed, to fill the rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGodown_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbGodown.SelectedIndex > 0)
                {
                    decimal decGodownId = Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                    RackComboFill();
                    cmbRack.Enabled = true;
                }
                else
                {
                    cmbRack.Enabled = false;
                    RackComboFillForLoad();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:14" + ex.Message;
            }
        }
        /// <summary>
        /// Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ProductSearchGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:15" + ex.Message;
            }
        }
        /// <summary>
        /// Print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string strCriteria = string.Empty;
                if (rbtnAll.Checked)
                {
                    strCriteria = "All";
                }
                if (rbtnMaxLevel.Checked)
                {
                    strCriteria = "Maximum Level";
                }
                if (rbtnMinLevel.Checked)
                {
                    strCriteria = "Minimum Level";
                }
                if (rbtnNegStock.Checked)
                {
                    strCriteria = "Negative Stock";
                }
                if (rbtnRecordLevel.Checked)
                {
                    strCriteria = "Reorder Level";
                }
                DataSet dsProductSearch = spProduct.ProductSearchReport(1, Convert.ToDecimal(cmbGodown.SelectedValue), Convert.ToDecimal(cmbBrand.SelectedValue), Convert.ToDecimal(cmbModelNo.SelectedValue), Convert.ToDecimal(cmbRack.SelectedValue), Convert.ToDecimal(cmbSize.SelectedValue), Convert.ToDecimal(cmbTax.SelectedValue), Convert.ToDecimal(cmbProductGroup.SelectedValue), Convert.ToString(cmbStatus.Text), txtProductCode.Text, txtProductName.Text, strCriteria,txtBatchName.Text);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                if (dgvProductSearch.Rows.Count > 0)
                {
                    frmReport.ProductSearchReport(dsProductSearch, strCriteria);
                }
                else
                {
                    MessageBox.Show("No Data To Print", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:16" + ex.Message;
            }
        }
        /// <summary>
        /// butten viwDetails click to view the purticular ites in detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductSearch.SelectedCells.Count > 0 && dgvProductSearch.CurrentRow != null)
                {
                    DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvProductSearch.CurrentCell.ColumnIndex, dgvProductSearch.CurrentCell.RowIndex);
                    dgvProductSearch_CellDoubleClick(sender, ex);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:17" + ex.Message;
            }
        }
        /// <summary>
        /// Grid cell double click for View the selected details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (e.RowIndex > -1 || e.ColumnIndex > -1)
                    {
                        frmProductDetails frmProductDetailsObj = new frmProductDetails();
                        frmProductDetailsObj.MdiParent = formMDI.MDIObj;
                        decimal strId = decimal.Parse(dgvProductSearch.CurrentRow.Cells["productId"].Value.ToString());
                        frmProductDetailsObj.callFromProductSearchTo(strId, this);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:18" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbTax.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:19" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBrand.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbProductGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:20" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbModelNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbTax.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:21" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModelNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbGodown.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbBrand.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:22" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGodown_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbRack.Enabled)
                    {
                        cmbRack.Focus();
                    }
                    else
                    {
                        cmbSize.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbGodown.SelectionStart == 0)
                        cmbModelNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:23" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRack_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSize.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbGodown.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:24" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbStatus.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbRack.Enabled)
                    {
                        cmbRack.Focus();
                    }
                    else
                    {
                        cmbGodown.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:25" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbSize.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:26" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
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
                    if (txtProductName.SelectionStart == 0)
                        cmbStatus.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:27" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBatchName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.SelectionStart == 0 || txtProductCode.Text == string.Empty)
                    {
                        txtProductName.Focus();
                        txtProductName.SelectionStart = 0;
                        txtProductName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:28" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnAll_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (rbtnMinLevel.Enabled)
                    {
                        rbtnMinLevel.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtBatchName.Focus();
                    txtProductCode.SelectionStart = 0;
                    txtProductCode.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:29" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnMinLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (rbtnRecordLevel.Enabled)
                    {
                        rbtnRecordLevel.Focus();
                    }
                    rbtnRecordLevel.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnAll.Enabled)
                    {
                        rbtnAll.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:30" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnRecordLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (rbtnMaxLevel.Enabled)
                    {
                        rbtnMaxLevel.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnMinLevel.Enabled)
                    {
                        rbtnMinLevel.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:31" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnMaxLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (rbtnNegStock.Enabled)
                    {
                        rbtnNegStock.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnRecordLevel.Enabled)
                    {
                        rbtnRecordLevel.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:32" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnNegStock_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnMaxLevel.Enabled)
                    {
                        rbtnMaxLevel.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:33" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnReset.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (rbtnNegStock.Enabled)
                    {
                        rbtnNegStock.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:34" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductSearch_KeyDown(object sender, KeyEventArgs e)
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:35" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:36" + ex.Message;
            }
        }
        private void txtBatchName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rbtnAll.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtProductCode.Focus();
                    txtProductCode.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS:37" + ex.Message;
            }
        }
        #endregion
    }
}
