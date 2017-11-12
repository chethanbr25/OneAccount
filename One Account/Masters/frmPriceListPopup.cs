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
    public partial class frmPriceListPopup : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decProductMain;
        decimal decPricingLevelMain;
        decimal decUnitId;
        decimal decPriceLevelId;
        frmPriceList frmPriceListobj;
        decimal deca;
        decimal decBatchId;
        decimal decpriceListId;
        decimal decProductId;
        string strComboTypes;
        frmSalesReturn frmSalesReturnObj;
        frmDeliveryNote frmDeliveryNoteObj;
        #endregion
        #region Functions
        /// <summary>
        /// Create instance of frmPriceListPopup
        /// </summary>
        public frmPriceListPopup()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to execute while calling from frmPriceList form to add new pricelist for the product
        /// </summary>
        /// <param name="decPricingLevel"></param>
        /// <param name="decProduct"></param>
        /// <param name="decPriceListId"></param>
        /// <param name="frmPriceList"></param>
        public void CallFromPriceListForPricingLevel(decimal decPricingLevel, decimal decProduct, decimal decPriceListId, frmPriceList frmPriceList)
        {
            try
            {
                ProductInfo infoProduct = new ProductInfo();
                ProductSP spProduct = new ProductSP();
                UnitSP spunit = new UnitSP();
                UnitInfo infoUnit = new UnitInfo();
                PriceListSP spPriceList = new PriceListSP();
                PriceListInfo infoPriceList = new PriceListInfo();
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                PricingLevelInfo infoPricingLevel = new PricingLevelInfo();
                infoProduct = spProduct.PriceListPopUpView(decProduct);
                txtProductCode.Text = infoProduct.ProductCode;
                txtProductName.Text = infoProduct.ProductName;
                decProduct = infoProduct.ProductId;
                decProductMain = infoProduct.ProductId;
                infoUnit = spunit.UnitViewForPriceListPopUp(decProduct);
                decUnitId = infoUnit.UnitId;
                txtUnitName.Text = infoUnit.UnitName;
                decPriceLevelId = infoPriceList.PricinglevelId;
                infoPricingLevel = spPricingLevel.PricingLevelNameViewForPriceListPopUp(decPricingLevel, decProduct, decUnitId);
                decPriceLevelId = infoPricingLevel.PricinglevelId;
                txtPricingLevel.Text = infoPricingLevel.PricinglevelName;
                if (txtPricingLevel.Text == string.Empty)
                {
                    infoPricingLevel = spPricingLevel.PricingLevelView(decPricingLevel);
                    txtPricingLevel.Text = infoPricingLevel.PricinglevelName;
                }
                decPricingLevelMain = infoPricingLevel.PricinglevelId;
                txtPricingLevel.ReadOnly = true;
                txtProductCode.ReadOnly = true;
                txtProductName.ReadOnly = true;
                txtUnitName.ReadOnly = true;
                txtRate.Focus();
                BatchUnderProductComboFill(decProduct);
                PriceListPopupGridFill();
                deca = decPriceListId;
                this.frmPriceListobj = frmPriceList;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP1" + ex.Message + ex.Message;
            }
            
        }
        /// <summary>
        /// Function to save the new pricelist for the product
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                PriceListSP spPriceList = new PriceListSP();
                PriceListInfo infoPriceList = new PriceListInfo();
                infoPriceList.ProductId = decProductMain;
                infoPriceList.PricinglevelId = decPricingLevelMain;
                infoPriceList.UnitId = decUnitId;
                infoPriceList.BatchId = Convert.ToDecimal(cmbBatch.SelectedValue.ToString());
                infoPriceList.Rate = Convert.ToDecimal(txtRate.Text);
                infoPriceList.Extra1 = string.Empty;
                infoPriceList.Extra2 = string.Empty;
                if (spPriceList.PriceListCheckExistence(0, decPricingLevelMain, Convert.ToDecimal(cmbBatch.SelectedValue.ToString()), decProductMain) == true)
                {
                    spPriceList.PriceListAdd(infoPriceList);
                    Messages.SavedMessage();
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Price List already exist for selected product and batches");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP2" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit the pricelist
        /// </summary>
        public void EditFunction()
        {
            try
            {
                if (txtRate.Text.Trim() == String.Empty)
                {
                    Messages.InformationMessage("Enter rate");
                    txtRate.Focus();
                }
                else if (Convert.ToDecimal(txtRate.Text) == 0)
                {
                    PriceListSP spPriceList = new PriceListSP();
                    spPriceList.PriceListDelete(deca);
                    Messages.UpdatedMessage();
                    if (frmPriceListobj != null)
                    {
                        this.Close();
                    }
                }
                else
                {
                    PriceListSP spPriceList = new PriceListSP();
                    PriceListInfo infoPriceList = new PriceListInfo();
                    infoPriceList.ProductId = decProductMain;
                    infoPriceList.PricelistId = Convert.ToDecimal(dgvProductGroup.CurrentRow.Cells["dgvtxtPriceListId"].Value.ToString());
                    infoPriceList.PricinglevelId = decPricingLevelMain;
                    infoPriceList.UnitId = decUnitId;
                    infoPriceList.BatchId = decBatchId;
                    infoPriceList.Rate = Convert.ToDecimal(txtRate.Text);
                    infoPriceList.Extra1 = string.Empty;
                    infoPriceList.Extra2 = string.Empty;
                    spPriceList.PriceListEdit(infoPriceList);
                    Messages.UpdatedMessage();
                    Clear();
                    if (frmPriceListobj != null)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP3" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to determine whether to call SaveFunction or EditFunction and also checking invalid entries
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (cmbBatch.Text.Trim() == string.Empty)
                    {
                        Messages.InformationMessage("Enter batch");
                        cmbBatch.Focus();
                        cmbBatch.SelectionStart = 0;
                    }
                    else if (txtRate.Text.Trim() == String.Empty || Convert.ToDecimal(txtRate.Text.ToString()) == 0)
                    {
                        Messages.InformationMessage("Enter rate");
                        txtRate.Focus();
                    }
                    else if (PublicVariables.isMessageAdd)
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
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP4" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the batch corresponding to product
        /// </summary>
        /// <param name="decProductId"></param>
        public void BatchUnderProductComboFill(decimal decProductId)
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                DataTable dtblBatch = new DataTable();
                dtblBatch = spBatch.BatchViewbyProductIdForComboFill(decProductId);
                cmbBatch.DataSource = dtblBatch;
                cmbBatch.ValueMember = "batchId";
                cmbBatch.DisplayMember = "batchNo";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP5" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function To clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    cmbBatch.SelectedIndex = -1;
                    txtRate.Clear();
                    PriceListPopupGridFill();
                    btnDelete.Enabled = false;
                }
                else if (btnSave.Text == "Update")
                {
                    cmbBatch.SelectedIndex = -1;
                    txtRate.Clear();
                    PriceListPopupGridFill();
                    btnSave.Text = "Save";
                    btnDelete.Enabled = false;
                }
            }
            catch (Exception ex) 
            {
                formMDI.infoError.ErrorString = "PLP6" + ex.Message + ex.Message;
            
            }
        }
        /// <summary>
        /// Function to fill the pricelist details for the products
        /// </summary>
        public void GridFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                PriceListSP spPriceList = new PriceListSP();
                dtbl = spPriceList.PriceListGridFill();
                dgvProductGroup.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP7" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete the pricelist for the product 
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                PriceListSP spPriceList = new PriceListSP();
                if (spPriceList.PriceListCheckReferenceAndDelete(decPriceLevelId) <= 0)
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
                formMDI.infoError.ErrorString = "PLP8" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to call the DeleteFunction after user confirmation
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
                formMDI.infoError.ErrorString = "PLP9" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the fields while user double click on the datagridview
        /// </summary>
        public void FillControls()
        {
            try
            {
                PriceListInfo infoPricelist = new PriceListInfo();
                PriceListSP spPricelist = new PriceListSP();
                infoPricelist.PricelistId = decpriceListId;
                infoPricelist = spPricelist.PriceListView(decpriceListId);
                txtRate.Text = infoPricelist.Rate.ToString();
                ProductInfo infoProduct = new ProductInfo();
                ProductSP spProduct = new ProductSP();
                infoProduct = spProduct.PriceListPopUpView(decProductId);
                txtProductCode.Text = infoProduct.ProductCode;
                txtProductName.Text = infoProduct.ProductName;
                BatchInfo infobatch = new BatchInfo();
                BatchSP spBatch = new BatchSP();
                infobatch = spBatch.BatchView(decBatchId);
                cmbBatch.SelectedValue = infobatch.BatchId;
                UnitInfo infoUnit = new UnitInfo();
                UnitSP spUnit = new UnitSP();
                infoUnit = spUnit.UnitView(decUnitId);
                txtUnitName.Text = infoUnit.UnitName;
                PricingLevelInfo infoPricingLevel = new PricingLevelInfo();
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                infoPricingLevel = spPricingLevel.PricingLevelView(decPriceLevelId);
                txtPricingLevel.Text = infoPricingLevel.PricinglevelName;
                txtProductName.ReadOnly = true;
                txtProductCode.ReadOnly = true;
                txtUnitName.ReadOnly = true;
                txtPricingLevel.ReadOnly = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP10" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the pricelist details in datagridview 
        /// </summary>
        public void PriceListPopupGridFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                PriceListSP spPriceList = new PriceListSP();
                dtbl = spPriceList.PriceListPopupGridFill(decPricingLevelMain, decProductMain);
                dgvProductGroup.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP11" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to add new pricinglevel from the SalesReturn form
        /// </summary>
        /// <param name="frmSalesReturn"></param>
        /// <param name="decId"></param>
        /// <param name="strComboType"></param>
        public void CallFromSalesReturnForPricingLevel(frmSalesReturn frmSalesReturn, decimal decId, string strComboType) //PopUp
        {
            try
            {
                strComboTypes = strComboType;
                base.Show();
                this.frmSalesReturnObj = frmSalesReturn;
                int inRowCount = dgvProductGroup.Rows.Count;
                for (int i = 0; i < inRowCount; i++)
                {
                    if (Convert.ToDecimal(dgvProductGroup.Rows[i].Cells["pricelistId"].Value.ToString()) == decId)
                    {
                        dgvProductGroup.Rows[i].DefaultCellStyle.BackColor = Color.MediumSeaGreen;
                    }
                }
                txtPricingLevel.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP12" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// Function to add new pricinglevel from DeliveryNote form
        /// </summary>
        /// <param name="frmdeliveryNote"></param>
        /// <param name="decId"></param>
        /// <param name="strComboType"></param>
        public void CallFromDeliveryNoteForPricingfLevelPopUp(frmDeliveryNote frmdeliveryNote, decimal decId, string strComboType) //PopUp
        {
            try
            {
                strComboTypes = strComboType;
                base.Show();
                this.frmDeliveryNoteObj = frmdeliveryNote;
                int inRowCount = dgvProductGroup.Rows.Count;
                for (int i = 0; i < inRowCount; i++)
                {
                    if (Convert.ToDecimal(dgvProductGroup.Rows[i].Cells["pricelistId"].Value.ToString()) == decId)
                    {
                        dgvProductGroup.Rows[i].DefaultCellStyle.BackColor = Color.MediumSeaGreen;
                    }
                }
                txtPricingLevel.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP13" + ex.Message + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPriceListPopup_Load(object sender, EventArgs e)
        {
            try
            {
                PriceListPopupGridFill();
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP14" + ex.Message + ex.Message;
            }
        }
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
                    frmPriceListobj.Enabled = true;
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP15" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// On save button click
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
                    if (frmPriceListobj != null)
                    {
                        frmPriceListobj.Clear();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP16" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// On keypress of txtRate, it avoids invalid entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
                txtRate.MaxLength = 8;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP17" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// On delete button click
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
                    if (frmPriceListobj != null)
                    {
                        frmPriceListobj.Clear();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP18" + ex.Message + ex.Message;
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
                formMDI.infoError.ErrorString = "PLP19" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// On double clicking on the datagridview, it fills the fields for editing or deleting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductGroup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    decpriceListId = Convert.ToDecimal(dgvProductGroup.Rows[e.RowIndex].Cells["dgvtxtPriceListId"].Value.ToString());
                    decProductId = Convert.ToDecimal(dgvProductGroup.Rows[e.RowIndex].Cells["dgvtxtProductId"].Value.ToString());
                    decUnitId = Convert.ToDecimal(dgvProductGroup.Rows[e.RowIndex].Cells["dgvtxtunitId"].Value.ToString());
                    decBatchId = Convert.ToDecimal(dgvProductGroup.Rows[e.RowIndex].Cells["dgvtxtBatchId"].Value.ToString());
                    decPriceLevelId = Convert.ToDecimal(dgvProductGroup.Rows[e.RowIndex].Cells["dgvtxtpricelevelId"].Value.ToString());
                    FillControls();
                    btnDelete.Enabled = true;
                    cmbBatch.Focus();
                    btnSave.Text = "Update";
                    if (frmSalesReturnObj != null)
                    {
                        if (dgvProductGroup.CurrentRow.Cells["dgvtxtPriceListId"].Selected)
                        {
                            frmSalesReturnObj.CallPriceListPopUp(this, Convert.ToDecimal(dgvProductGroup.CurrentRow.Cells["dgvtxtPriceListId"].Value.ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP20" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPriceListPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmPriceListobj != null)
                {
                    frmPriceListobj.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP21" + ex.Message + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// For Enter key and backspace navigation of txtRate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtRate.Text == string.Empty || txtRate.SelectionStart == 0)
                    {
                        cmbBatch.Focus();
                        cmbBatch.SelectionStart = 0;
                        cmbBatch.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP22" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// For backspace navigation of btnSave button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtRate.Focus();
                    txtRate.SelectionStart = 0;
                    txtRate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP23" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// For backspace navigation of btnClose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PLP24" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// For the shortcut keys
        /// Escape for close
        /// ctrl+s for save
        /// ctrl+d for delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPriceListPopup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    cmbBatch.Focus();
                }
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose)
                    {
                        Messages.CloseMessage(this);
                        frmPriceListobj.Enabled = true;
                    }
                    else
                    {
                        this.Close();
                    }
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                    {
                        SaveOrEdit();
                        if (frmPriceListobj != null)
                        {
                            frmPriceListobj.Clear();
                        }
                    }
                    else
                    {
                        Messages.NoPrivillageMessage();
                    }
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
                formMDI.infoError.ErrorString = "PLP25" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// For the navigation of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductGroup_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvProductGroup.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvProductGroup.CurrentCell.ColumnIndex, dgvProductGroup.CurrentCell.RowIndex);
                        dgvProductGroup_CellDoubleClick(sender, ex);
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtRate.Focus();
                    txtRate.SelectionStart = 0;
                    txtRate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP26" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// For the navigation of the dgvProductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvProductGroup.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvProductGroup.CurrentCell.ColumnIndex, dgvProductGroup.CurrentCell.RowIndex);
                        dgvProductGroup_CellDoubleClick(sender, ex);
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtRate.Focus();
                    txtRate.SelectionStart = 0;
                    txtRate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PLP27" + ex.Message + ex.Message;
            }
        }
        /// <summary>
        /// For the backspace navigation of batch combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBatch_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PLP28" + ex.Message + ex.Message;
            }
        }
        #endregion
    }
}
