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
    public partial class frmPriceList : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        frmPriceListPopup frmPriceListPopupObj;
        #endregion
        #region Function
        /// <summary>
        /// Create instance of frmPriceList class 
        /// </summary>
        public frmPriceList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to clear the Fields
        /// </summary>
        public void Clear()
        {
            try
            {
                ProductGroupComboFill();
                PricelevelComboFill();
                cmbProductGroup.Focus();
                txtProductCode.Text = string.Empty;
                txtProductName.Text = string.Empty;
                cmbPricingLevel.SelectedIndex = -1;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL1" + ex.Message;
                
            }
        }
        /// <summary>
        /// Function to fill the ProductGroup comboBox
        /// </summary>
        public void ProductGroupComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                PriceListSP spPriceList = new PriceListSP();
                dtbl = spPriceList.PricelistProductGroupViewAllForComboBox();
                DataRow dr = dtbl.NewRow();
                dr[0] = 0;
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbProductGroup.DataSource = dtbl;
                cmbProductGroup.ValueMember = "groupId";
                cmbProductGroup.DisplayMember = "groupName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the pricelevel combobox
        /// </summary>
        public void PricelevelComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                PriceListSP spPriceList = new PriceListSP();
                dtbl = spPriceList.PricelistPricingLevelViewAllForComboBox();
                cmbPricingLevel.DataSource = dtbl;
                cmbPricingLevel.ValueMember = "pricinglevelId";
                cmbPricingLevel.DisplayMember = "pricinglevelName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid based on the
        /// </summary>
        public void GridFill()
        {
            try
            {
                {
                    DataTable dtbl = new DataTable();
                    PriceListSP spPriceList = new PriceListSP();
                    if (cmbProductGroup.Text == "All")
                    {
                        cmbProductGroup.Text = "All";
                    }
                    cmbPricingLevel.Text = cmbPricingLevel.Text;
                    dtbl = spPriceList.ProductDetailsViewGridfill(Convert.ToDecimal(cmbProductGroup.SelectedValue.ToString()), txtProductCode.Text, txtProductName.Text, cmbPricingLevel.Text);
                    dgvPricingList.DataSource = dtbl;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL4" + ex.Message;
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
                formMDI.infoError.ErrorString = "PCL5" + ex.Message;
            }
        }
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPriceList_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
                this.dgvPricingList.Columns["dgvtxtrate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                this.dgvPricingList.Columns["dgvtxtpurchaserate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                this.dgvPricingList.Columns["dgvtxtsalesrate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                this.dgvPricingList.Columns["dgvtxtmrp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvPricingList.Columns["dgvtxtsalesrate"].DisplayIndex = 6;
                dgvPricingList.Columns["dgvbtnpricelist"].DisplayIndex = 13;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL6" + ex.Message;
            }
        }
        /// <summary>
        /// On serach button click
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
                formMDI.infoError.ErrorString = "PCL7" + ex.Message;
            }
        }
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
                formMDI.infoError.ErrorString = "PCL8" + ex.Message;
            }
        }
        /// <summary>
        /// On pricelist button click in datagridview ,it displays a popup window of pricelistpopup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPricingList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvPricingList.CurrentCell == dgvPricingList.CurrentRow.Cells["dgvbtnpricelist"])
                    {
                        if (cmbPricingLevel.SelectedIndex != -1)
                        {
                            frmPriceListPopupObj = Application.OpenForms["frmPriceListPopup"] as frmPriceListPopup;
                            if (frmPriceListPopupObj == null)
                            {
                                frmPriceListPopupObj = new frmPriceListPopup();
                                frmPriceListPopupObj.MdiParent = formMDI.MDIObj;
                                frmPriceListPopupObj.Show();
                            }
                            else
                            {
                                frmPriceListPopupObj.Activate();
                            }
                            if (dgvPricingList.Rows[e.RowIndex].Cells["dgvtxtpricelistId"].Value.ToString() == string.Empty) //&& dgvPricingList.Rows[e.RowIndex].Cells["dgvtxtpricelistId"].Value.ToString()==null)
                            {
                                dgvPricingList.Rows[e.RowIndex].Cells["dgvtxtpricelistId"].Value = "0";
                            }
                            frmPriceListPopupObj.CallFromPriceListForPricingLevel(Convert.ToDecimal(cmbPricingLevel.SelectedValue.ToString()), Convert.ToDecimal(dgvPricingList.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value.ToString()), Convert.ToDecimal(dgvPricingList.Rows[e.RowIndex].Cells["dgvtxtpricelistId"].Value.ToString()), this);
                            dgvPricingList.ClearSelection();
                            txtProductName.Focus();
                            this.Enabled = false;
                        }
                        else
                        {
                            Messages.InformationMessage("Enter Pricing Level");
                            cmbPricingLevel.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL9" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// The form will close on escape key press 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPriceList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL10" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbPricingLevel.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL11" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPricingLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbPricingLevel.Text == string.Empty || cmbPricingLevel.SelectionStart == 0)
                    {
                        cmbProductGroup.Focus();
                        cmbProductGroup.SelectionStart = 0;
                        cmbProductGroup.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL12" + ex.Message;
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
                    txtProductName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        cmbPricingLevel.Focus();
                        cmbPricingLevel.SelectionStart = 0;
                        cmbPricingLevel.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL13" + ex.Message;
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
                    btnSearch.Focus();
                    btnSearch_Click(sender, e);
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductName.Text == string.Empty || txtProductName.SelectionStart == 0)
                    {
                        txtProductCode.Focus();
                        txtProductCode.SelectionStart = 0;
                        txtProductCode.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL14" + ex.Message;
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
                if (e.KeyCode == Keys.Back)
                {
                    txtProductName.Focus();
                    txtProductName.SelectionStart = 0;
                    txtProductName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL15" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvPricingList.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PCL16" + ex.Message;
            }
        }
        #endregion
    }
}
