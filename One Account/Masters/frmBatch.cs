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
    public partial class frmBatch : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part 
        /// </summary>
        decimal decBatchId;
        int inNarrationCount = 0;
        decimal decId;
        #endregion
        #region Function
        /// <summary>
        /// Creates an instance of frmBatch class
        /// </summary>
        public frmBatch()
        {
            InitializeComponent();
        }
        //Jamshi
        //public void SaveFunction()
        //{
        //    try
        //    {
        //        BatchSP spBatch = new BatchSP();
        //        BatchInfo infoBatch = new BatchInfo();
        //        infoBatch.BatchNo = txtBatchName.Text.Trim();
        //        infoBatch.ProductId = Convert.ToDecimal(cmbProduct.SelectedValue.ToString());
        //        infoBatch.ManufacturingDate = Convert.ToDateTime(txtMfgDate.Text);
        //        infoBatch.ExpiryDate = Convert.ToDateTime(txtExpiryDate.Text);
        //        infoBatch.narration = txtNarration.Text.Trim();
        //        infoBatch.Extra1 = string.Empty;
        //        infoBatch.Extra2 = string.Empty;
        //        if (spBatch.BatchNameAndProductNameCheckExistence(txtBatchName.Text.Trim(), Convert.ToDecimal(cmbProduct.SelectedValue.ToString()), 0) == false)
        //        {
        //            if (dtpMfgDate.Value <= dtpExpiryDate.Value)
        //            {
        //                spBatch.BatchAddParticularFields(infoBatch);
        //                Messages.SavedMessage();
        //                Clear();
        //            }
        //            else
        //            {
        //                Messages.InformationMessage(" Can't save batch with mfg date greater than expiry date");
        //                txtMfgDate.Select();
        //            }
        //        }
        //        else
        //        {
        //            Messages.InformationMessage(" Already exist");
        //            txtBatchName.Focus();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("B1:" + ex.Message, "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        //public void EditFunction()
        //{
        //    try
        //    {
        //        BatchSP spBatch = new BatchSP();
        //        BatchInfo infoBatch = new BatchInfo();
        //        infoBatch.BatchNo = txtBatchName.Text.Trim();
        //        infoBatch.ProductId = Convert.ToDecimal(cmbProduct.SelectedValue.ToString());
        //        infoBatch.ManufacturingDate = Convert.ToDateTime(txtMfgDate.Text);
        //        infoBatch.ExpiryDate = Convert.ToDateTime(txtExpiryDate.Text);
        //        infoBatch.narration = txtNarration.Text.Trim();
        //        infoBatch.Extra1 = string.Empty;
        //        infoBatch.Extra2 = string.Empty;
        //        infoBatch.BatchId = decId;
        //        if (spBatch.BatchNameAndProductNameCheckExistence(txtBatchName.Text.Trim(), Convert.ToDecimal(cmbProduct.SelectedValue.ToString()), decBatchId) == false)
        //        {
        //            spBatch.BatchEdit(infoBatch);
        //            Messages.UpdatedMessage();
        //            SearchClear();
        //            Clear();
        //        }
        //        else
        //        {
        //            Messages.InformationMessage("Already exists");
        //            txtBatchName.Focus();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("B2:" + ex.Message, "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        /// <summary>
        /// Functin to save and update batch
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtBatchName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter batch name");
                    txtBatchName.Focus();
                }
                else if (cmbProduct.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select product");
                    cmbProduct.Focus();
                }
                else
                {
                    BatchSP spBatch = new BatchSP();
                    BatchInfo infoBatch = new BatchInfo();
                    infoBatch.BatchNo = txtBatchName.Text.Trim();
                    infoBatch.ProductId = Convert.ToDecimal(cmbProduct.SelectedValue.ToString());
                    infoBatch.ManufacturingDate = Convert.ToDateTime(txtMfgDate.Text);
                    infoBatch.ExpiryDate = Convert.ToDateTime(txtExpiryDate.Text);
                    infoBatch.narration = txtNarration.Text.Trim();
                    infoBatch.Extra1 = string.Empty;
                    infoBatch.Extra2 = string.Empty;
                    Int32 inBarcode = spBatch.AutomaticBarcodeGeneration(); // updated by jaseel
                    infoBatch.barcode = Convert.ToString(inBarcode);
                    if (btnSave.Text == "Save")
                    {
                        //Jamshi
                        //if (PublicVariables.isMessageAdd)
                        //{
                        //    if (Messages.SaveMessage())
                        //    {
                        //        SaveFunction();
                        //    }
                        //}
                        //else
                        //{
                        //    SaveFunction();
                        //}
                        if (Messages.SaveConfirmation())
                        {
                            if (spBatch.BatchNameAndProductNameCheckExistence(txtBatchName.Text.Trim(), Convert.ToDecimal(cmbProduct.SelectedValue.ToString()), 0) == false)
                            {
                                if (dtpMfgDate.Value <= dtpExpiryDate.Value)
                                {
                                    spBatch.BatchAddParticularFields(infoBatch);
                                    Messages.SavedMessage();
                                    Clear();
                                }
                                else
                                {
                                    Messages.InformationMessage(" Can't save batch with mfg date greater than expiry date");
                                    txtMfgDate.Select();
                                }
                            }
                            else
                            {
                                Messages.InformationMessage(" Already exist");
                                txtBatchName.Focus();
                            }
                        }
                    }
                    else
                    {
                        //Jamshi
                        //if (PublicVariables.isMessageEdit)
                        //{
                        //    if (Messages.UpdateMessage())
                        //    {
                        //        EditFunction();
                        //    }
                        //}
                        //else
                        //{
                        //    EditFunction();
                        //}
                        if (Messages.UpdateConfirmation())
                        {
                            infoBatch.BatchId = decId;
                            if (spBatch.BatchNameAndProductNameCheckExistence(txtBatchName.Text.Trim(), Convert.ToDecimal(cmbProduct.SelectedValue.ToString()), decBatchId) == false)
                            {
                                spBatch.BatchEdit(infoBatch);
                                Messages.UpdatedMessage();
                                SearchClear();
                                Clear();
                            }
                            else
                            {
                                Messages.InformationMessage("Already exists");
                                txtBatchName.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B1:" + ex.Message;
                
            }
        }
        /// <summary>
        /// Function to fill product combobox
        /// </summary>
        public void ProductNameComboFill()
        {
            try
            {
                ProductSP spproduct = new ProductSP();
                DataTable dtblProductName = new DataTable();
                dtblProductName = spproduct.ProductViewAllForBatchByAllowBatch();
                DataRow dr = dtblProductName.NewRow();
                dr[1] = "All";
                dtblProductName.Rows.InsertAt(dr, 0);
                cmbProductName.DataSource = dtblProductName;
                cmbProductName.ValueMember = "productId";
                cmbProductName.DisplayMember = "productName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B2:" + ex.Message;
            }
        }
        /// <summary>
        /// Funtion to fill produdct combobox
        /// </summary>
        public void ProductViewAllForBatchByAllowBatch()
        {
            try
            {
                ProductSP spproduct = new ProductSP();
                DataTable dtblProductName = new DataTable();
                dtblProductName = spproduct.ProductViewAllForBatchByAllowBatch();
                cmbProduct.DataSource = dtblProductName;
                cmbProduct.ValueMember = "productId";
                cmbProduct.DisplayMember = "productName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B3:" + ex.Message;
            }
        }
        /// <summary>
        /// Funtion to fill produdct combobox
        /// </summary>
        public void ProductComboFill()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                DataTable dtblProduct = new DataTable();
                dtblProduct = spProduct.ProductViewAllForComboBox();
                cmbProduct.DataSource = dtblProduct;
                cmbProduct.ValueMember = "productId";
                cmbProduct.DisplayMember = "productName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear fields
        /// </summary>
        public void Clear()
        {
            try
            {
                txtBatchName.Clear();
                txtNarration.Clear();
                txtExpiryDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                txtMfgDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                cmbProduct.SelectedIndex = -1;
                cmbProductName.SelectedIndex = 0;
                txtBatchName.Focus();
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                //GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear fields
        /// </summary>
        public void SearchClear()
        {
            try
            {
                txtBatch.Clear();
                cmbProductName.SelectedIndex = 0;
                txtBatch.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B6:" + ex.Message;
            }
        }
        //Jamshi
        //public void DeleteFunction()
        //{
        //    try
        //    {
        //        BatchSP spBatch = new BatchSP();
        //        if (spBatch.BatchCheckReferences(decId) == -1)
        //        {
        //            Messages.ReferenceExistsMessage();
        //        }
        //        else
        //        {
        //            Messages.DeletedMessage();
        //            Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("B8:" + ex.Message, "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        /// <summary>
        /// Function to delete batch
        /// </summary>
        public void Delete()
        {
            try
            {
                //Jamshi
                //if (PublicVariables.isMessageDelete)
                //{
                //    if (Messages.DeleteMessage())
                //    {
                //        DeleteFunction();
                //    }
                //}
                //else
                //{
                //    DeleteFunction();
                //}
                if (Messages.DeleteConfirmation())
                {
                    BatchSP spBatch = new BatchSP();
                    if (spBatch.BatchCheckReferences(decId) == -1)
                    {
                        Messages.ReferenceExistsMessage();
                    }
                    else
                    {
                        Messages.DeletedMessage();
                        Clear();
                        GridFill();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B7:" + ex.Message;
            }
        }
        /// <summary>
        /// Funtion to fill datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                DataTable dtblBatch = new DataTable();
                dtblBatch = spBatch.BatchSearch(txtBatch.Text.Trim(), cmbProductName.Text);
                dgvBatch.DataSource = dtblBatch;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to set values to controls when click in grid
        /// </summary>
        public void FillControls()
        {
            try
            {
                BatchInfo infoBatch = new BatchInfo();
                BatchSP spBatch = new BatchSP();
                infoBatch = spBatch.BatchView(decId);
                txtBatchName.Text = infoBatch.BatchNo;
                cmbProduct.SelectedValue = infoBatch.ProductId.ToString();
                txtExpiryDate.Text = infoBatch.ExpiryDate.ToString("dd-MMM-yyyy");
                txtMfgDate.Text = infoBatch.ManufacturingDate.ToString("dd-MMM-yyyy");
                txtNarration.Text = infoBatch.narration;
                decBatchId = infoBatch.BatchId;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B9:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBatch_Load(object sender, EventArgs e)
        {
            try
            {
                ProductComboFill();
                ProductNameComboFill();
                ProductViewAllForBatchByAllowBatch();
                Clear();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B10:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtExpiryDate.Text) < Convert.ToDateTime(txtMfgDate.Text))
                {
                    MessageBox.Show("ExpiryDate should be greater than MfgDate", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtExpiryDate.Focus();
                }
                else if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEdit();
                    GridFill();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B11:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' buton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
        {
            try
            {
                //Jamshi
                //if (PublicVariables.isMessageClose)
                //{
                //    Messages.CloseMessage(this);
                //}
                //else
                //{
                //    this.Close();
                //}
                if (Messages.CloseConfirmation())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B12:" + ex.Message;
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
                //Jamshi
                //if (PublicVariables.isMessageClose)
                //{
                //    Messages.CloseMessage(this);
                //}
                //else
                //{
                //    this.Close();
                //}
                if (Messages.CloseConfirmation())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B13:" + ex.Message;
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
                formMDI.infoError.ErrorString = "B14:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearsearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchClear();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B15:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill controls on double click in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBatch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    string strBatchName = dgvBatch.CurrentRow.Cells["dgvtxtBatchName"].Value.ToString();
                    if (strBatchName != "NA")
                    {
                        decId = Convert.ToDecimal(dgvBatch.CurrentRow.Cells["dgvtxtBatchId"].Value.ToString());
                        FillControls();
                        btnSave.Text = "Update";
                        txtBatchName.Focus();
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        Messages.WarningMessage("NA btach cannot update or delete");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B16:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Delete' button click
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
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B17:" + ex.Message;
            }
        }
        /// <summary>
        /// on 'Serach' button click
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
                formMDI.infoError.ErrorString = "B18:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBatch_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvBatch.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B19:" + ex.Message;
            }
        }
        /// <summary>
        /// On MFG date datetimepicker valuechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpMfgDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtMfgDate.Text = this.dtpMfgDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B20:" + ex.Message;
            }
        }
        /// <summary>
        /// On MFG date Text leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMfgDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtMfgDate);
                if (txtMfgDate.Text == string.Empty)
                {
                    txtMfgDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B21:" + ex.Message;
            }
        }
        /// <summary>
        /// On Expiry date datetimepicker valuechanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpExpiryDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtExpiryDate.Text = this.dtpExpiryDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B22:" + ex.Message;
            }
        }
        /// <summary>
        /// On Expirydate Text leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpiryDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtExpiryDate);
                if (txtExpiryDate.Text == string.Empty)
                {
                    txtExpiryDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B23:" + ex.Message;
            }
        }
        /// <summary>
        /// On MFG date Text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMfgDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtMfgDate.Text.Trim() == string.Empty)
                {
                    txtMfgDate_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B24:" + ex.Message;
            }
        }
        /// <summary>
        /// On Expiry date Text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpiryDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtExpiryDate.Text.Trim() == string.Empty)
                {
                    txtExpiryDate_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B25:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Frorm keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBatch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    //if (PublicVariables.isMessageClose)
                    //{
                    //    Messages.CloseMessage(this);
                    //}
                    //else
                    //{
                    //    this.Close();
                    //}
                    if (Messages.CloseConfirmation())
                    {
                        this.Close();
                    }
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (cmbProduct.Focused)
                    {
                        cmbProduct.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                    else
                    {
                        cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
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
                formMDI.infoError.ErrorString = "B26:" + ex.Message;
            }
        }
        /// <summary>
        /// On BatchName textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBatchName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProduct.Focus();
                    cmbProduct.SelectionLength = 0;
                    cmbProduct.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B27:" + ex.Message;
            }
        }
        /// <summary>
        /// On Product combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProduct_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtMfgDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbProduct.Text == string.Empty || cmbProduct.SelectionStart == 0)
                    {
                        txtBatchName.Focus();
                        txtBatchName.SelectionStart = 0;
                        txtBatchName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B28:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox keydown
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
                        btnSave.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        txtExpiryDate.Focus();
                        txtExpiryDate.SelectionLength = 0;
                        txtExpiryDate.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B29:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox key enter
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
                formMDI.infoError.ErrorString = "B30:" + ex.Message;
            }
        }
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
                    txtNarration.Focus();
                    txtNarration.SelectionLength = 0;
                    txtNarration.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B31:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Batch' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBatch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B32:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Product name' combobox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbProductName.Text == string.Empty || cmbProductName.SelectionStart == 0)
                    {
                        txtBatch.Focus();
                        txtBatch.SelectionStart = 0;
                        txtBatch.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B33:" + ex.Message;
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
                if (e.KeyCode == Keys.Back)
                {
                    cmbProductName.Focus();
                    cmbProductName.SelectionStart = 0;
                    cmbProductName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B34:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear' button keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearsearch_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "B35:" + ex.Message;
            }
        }
        /// <summary>
        /// On Datagridview keyup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBatch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvBatch.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvBatch.CurrentCell.ColumnIndex, dgvBatch.CurrentCell.RowIndex);
                        dgvBatch_CellDoubleClick(sender, ex);
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbProductName.Focus();
                    cmbProductName.SelectionStart = 0;
                    cmbProductName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B36:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'MFG DATE' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMfgDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtExpiryDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtMfgDate.Text == string.Empty || txtMfgDate.SelectionStart == 0)
                    {
                        cmbProduct.Focus();
                        cmbProduct.SelectionStart = 0;
                        cmbProduct.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B37:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Expiry date' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpiryDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtExpiryDate.Text == string.Empty || txtExpiryDate.SelectionStart == 0)
                    {
                        txtMfgDate.Focus();
                        txtMfgDate.SelectionStart = 0;
                        txtMfgDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "B38:" + ex.Message;
            }
        }
        #endregion
    }
}
