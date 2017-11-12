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
    public partial class frmStandardRatePopup : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        decimal decUnitId;
        decimal decProduct;
        decimal decStandardRate;
        decimal decProductId;
        decimal decStandardRateId;
        frmStandardRate StandardrateObj = null;
        decimal decUnit;
        decimal decBatchId;
        #endregion

        #region Function
        /// <summary>
        /// create an instance for frmStandardRatePopup class
        /// </summary>
        public frmStandardRatePopup()
        {
            InitializeComponent();
        }
        /// <summary>
        /// to Reset the form here
        /// </summary>
        public void Clear()
        {
            try
            {
                if (btnSave.Text == "Save")
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    dtpToDate.MinDate = PublicVariables._dtFromDate;
                    dtpToDate.MaxDate = PublicVariables._dtToDate;
                    dtpFromDate.Value = PublicVariables._dtCurrentDate;
                    dtpToDate.Value = PublicVariables._dtCurrentDate;
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    dtpFromDate.MinDate = PublicVariables._dtFromDate;
                    dtpFromDate.MaxDate = PublicVariables._dtToDate;
                    txtRate.Clear();
                    cmbBatch.SelectedIndex = -1;
                    btnDelete.Enabled = false;
                    btnSave.Text = "Save";
                    txtFromDate.Focus();
                    txtFromDate.Select();
                }
                else if (btnSave.Text == "Update")
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    dtpFromDate.Value = PublicVariables._dtCurrentDate;
                    dtpToDate.Value = PublicVariables._dtCurrentDate;
                    dtpToDate.MinDate = PublicVariables._dtFromDate;
                    dtpToDate.MaxDate = PublicVariables._dtToDate;
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    dtpFromDate.MinDate = PublicVariables._dtFromDate;
                    dtpFromDate.MaxDate = PublicVariables._dtToDate;
                    txtRate.Clear();
                    cmbBatch.SelectedIndex = -1;
                    btnSave.Text = "Save";
                    btnDelete.Enabled = false;
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP1" + ex.Message;
              
            }
        }
        /// <summary>
        /// Function to add new standard rate for the purticular product
        /// </summary>
        /// <param name="decProductId"></param>
        /// <param name="frmStandardRate"></param>
        public void CallFromStandardRate(decimal decProductId, frmStandardRate frmStandardRate)
        {
            try
            {
                base.Show();
                StandardrateObj = frmStandardRate;
                StandardrateObj.Enabled = false;
                ProductInfo infoProduct = new ProductInfo();
                ProductSP spProduct = new ProductSP();
                UnitSP spUnit = new UnitSP();
                UnitInfo infoUnit = new UnitInfo();
                infoProduct = spProduct.ProductViewForStandardRate(decProductId);
                txtProductCode.Text = infoProduct.ProductCode;
                txtProductName.Text = infoProduct.ProductName;
                decProduct = infoProduct.ProductId;
                infoUnit = spUnit.unitVieWForStandardRate(decProductId);
                decUnitId = infoUnit.UnitId;
                txtUnitName.Text = infoUnit.UnitName;
                txtProductName.ReadOnly = true;
                txtProductCode.ReadOnly = true;
                txtUnitName.ReadOnly = true;
                BatchUnderProductComboFill(decProductId);
                GridFill(decProductId);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP2" + ex.Message;
            }
        }
        /// <summary>
        /// gridfill function for search and update
        /// </summary>
        /// <param name="decProductId"></param>
        public void GridFill(decimal decProductId)
        {
            try
            {
                StandardRateSP spStandardRate = new StandardRateSP();
                DataTable dtblStandardRate = new DataTable();
                dtblStandardRate = spStandardRate.StandardRateGridFill(decProductId);
                dgvStandardRate.DataSource = dtblStandardRate;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP3" + ex.Message;
            }
        }
        /// <summary>
        /// save function
        /// </summary>
        public void SaveFunction()
        {
            decimal decBatchId = 0;
            try
            {

                StandardRateInfo infoStandardRate = new StandardRateInfo();
                StandardRateSP spStandardRate = new StandardRateSP();
                infoStandardRate.ApplicableFrom = Convert.ToDateTime(txtFromDate.Text.ToString());
                infoStandardRate.ApplicableTo = Convert.ToDateTime(txtToDate.Text.ToString());
                infoStandardRate.ProductId = decProduct;
                infoStandardRate.UnitId = decUnitId;
                decBatchId = Convert.ToDecimal(cmbBatch.SelectedValue.ToString());
                infoStandardRate.BatchId = decBatchId;
                infoStandardRate.Rate = Convert.ToDecimal(txtRate.Text.ToString());
                infoStandardRate.Extra1 = string.Empty;
                infoStandardRate.Extra2 = string.Empty;
                if (spStandardRate.StandardrateCheckExistence(0, Convert.ToDateTime(txtFromDate.Text.ToString()), Convert.ToDateTime(txtToDate.Text.ToString()), decProduct, decBatchId) == false)
                {
                    spStandardRate.StandardRateAddParticularfields(infoStandardRate);
                    Messages.SavedMessage();
                    GridFill(decProduct);
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Standard rate already exist for selected product,Batch and dates");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP4" + ex.Message;
            }
        }
        /// <summary>
        /// fill Items into the purticular controls for Update or delete
        /// </summary>
        public void FillControls()
        {
            try
            {
                StandardRateInfo infoStandardRate = new StandardRateInfo();
                StandardRateSP spStandardRate = new StandardRateSP();
                infoStandardRate = spStandardRate.StandardRateView(decStandardRate);
                dtpFromDate.Value = Convert.ToDateTime(infoStandardRate.ApplicableFrom.ToString());
                dtpToDate.Value = Convert.ToDateTime(infoStandardRate.ApplicableTo.ToString());
                dtpFromDate.Text = infoStandardRate.ApplicableFrom.ToString();
                dtpToDate.Text = infoStandardRate.ApplicableTo.ToString();
                txtRate.Text = infoStandardRate.Rate.ToString();
                decProduct = infoStandardRate.ProductId;
                decUnitId = infoStandardRate.UnitId;
                ProductSP spProduct = new ProductSP();
                ProductInfo infoProduct = new ProductInfo();
                infoProduct = spProduct.ProductViewForStandardRate(decProductId);
                txtProductCode.Text = infoProduct.ProductCode;
                txtProductName.Text = infoProduct.ProductName;
                decStandardRateId = infoStandardRate.StandardRateId;
                UnitInfo infoUnit = new UnitInfo();
                UnitSP spUnit = new UnitSP();
                infoUnit = spUnit.UnitView(decUnit);
                txtUnitName.Text = infoUnit.UnitName;
                txtProductName.ReadOnly = true;
                txtProductCode.ReadOnly = true;
                txtUnitName.ReadOnly = true;
                BatchInfo infoBatch = new BatchInfo();
                BatchSP spBatch = new BatchSP();
                decBatchId = infoStandardRate.BatchId;
                infoBatch = spBatch.BatchView(decBatchId);
                cmbBatch.SelectedValue = infoBatch.BatchId;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP5" + ex.Message;
            }
        }
        /// <summary>
        /// Edit function
        /// </summary>
        public void EditFunction()
        {
            try
            {
                StandardRateInfo infoStandardRate = new StandardRateInfo();
                StandardRateSP spStandardRate = new StandardRateSP();
                infoStandardRate.StandardRateId = decStandardRate;
                infoStandardRate.ApplicableFrom = Convert.ToDateTime(txtFromDate.Text.ToString());
                infoStandardRate.ApplicableTo = Convert.ToDateTime(txtToDate.Text.ToString());
                infoStandardRate.ProductId = decProduct;
                infoStandardRate.UnitId = decUnitId;
                infoStandardRate.BatchId = Convert.ToDecimal(cmbBatch.SelectedValue);
                infoStandardRate.Rate = Convert.ToDecimal(txtRate.Text.ToString());
                infoStandardRate.Extra1 = string.Empty;
                infoStandardRate.Extra2 = string.Empty;
                if (spStandardRate.StandardrateCheckExistence(decStandardRateId, Convert.ToDateTime(txtFromDate.Text.ToString()), Convert.ToDateTime(txtToDate.Text.ToString()), decProduct, infoStandardRate.BatchId) == false)
                {
                    spStandardRate.StandardRateEdit(infoStandardRate);
                    Messages.UpdatedMessage();
                    GridFill(decProduct);
                    Clear();
                }
                else
                {
                    Messages.InformationMessage("Standard rate already exist for selected product and dates");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP6" + ex.Message;
            }
        }
        /// <summary>
        /// checking invalid entries for Save and Update and call the curresponding function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtProductCode.Text == string.Empty)
                {
                    Messages.InformationMessage("Product not Selected");
                }
                else if (cmbBatch.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter batch");
                    cmbBatch.Focus();
                }
                else if (txtRate.Text.Trim() == string.Empty || Convert.ToDecimal(txtRate.Text.ToString()) == 0)
                {
                    Messages.InformationMessage("Enter rate");
                    txtRate.Focus();
                }
                else if(txtToDate.Text.ToString()== string.Empty)
                {
                    Messages.InformationMessage("Enter Valid Date");
                    txtToDate.Focus();
                }
                else if (Convert.ToDateTime(txtFromDate.Text.ToString()) > Convert.ToDateTime(txtToDate.Text.ToString()))
                {
                    Messages.InformationMessage("FromDate greater than ToDate");
                    txtFromDate.Focus();
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
                formMDI.infoError.ErrorString = "SRP7" + ex.Message;
            }
        }
        /// <summary>
        /// Delete function
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                StandardRateSP spStandardRate = new StandardRateSP();
                spStandardRate.StandardRateDelete(decStandardRate);
                Messages.DeletedMessage();
                decimal decProductId = Convert.ToDecimal(dgvStandardRate.CurrentRow.Cells["dgvtxtProductId"].Value);
                GridFill(decProductId);
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP8" + ex.Message;
            }
        }
        /// <summary>
        /// checking confermation for delete and call the delete function
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
                formMDI.infoError.ErrorString = "SRP9" + ex.Message;
            }
        }
        /// <summary>
        /// batch combo fill
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
                formMDI.infoError.ErrorString = "SRP10" + ex.Message;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// When form load call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStandardRatePopup_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP11" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click, here checking the user privilege
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmStandardRate", btnSave.Text))
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
                formMDI.infoError.ErrorString = "SRP12" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click and checking the User privilage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmStandardRate", "Delete"))
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
                formMDI.infoError.ErrorString = "SRP13" + ex.Message;
            }
        }
        /// <summary>
        /// To set the Fromdate here if the user enter date as Manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFromDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtFromDate);
                if (txtFromDate.Text == string.Empty)
                {
                    txtFromDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP14" + ex.Message;
            }
        }
        /// <summary>
        /// To set the Todate here if the user enter date as Manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objDateValidation = new DateValidation();
                objDateValidation.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //string strdate = txtToDate.Text.Trim();
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpToDate.Value = Convert.ToDateTime(txtToDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP15" + ex.Message;
            }
        }
        /// <summary>
        /// change the textbox fromDate value when change the curresponding dateTimePicker value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtFromDate.Text = this.dtpFromDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP16" + ex.Message;
            }
        }
        /// <summary>
        /// change the textbox ToDate value when change the curresponding dateTimePicker value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtToDate.Text = this.dtpToDate.Value.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP17" + ex.Message;
            }
        }
        /// <summary>
        /// clear selection for once data binding complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStandardRate_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvStandardRate.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP18" + ex.Message;
            }
        }
        /// <summary>
        /// grid cell double click for Update or Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStandardRate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    decStandardRate = Convert.ToDecimal(dgvStandardRate.Rows[e.RowIndex].Cells["dgvtxtStandardRateId"].Value.ToString());
                    decProductId = Convert.ToDecimal(dgvStandardRate.Rows[e.RowIndex].Cells["dgvtxtProductId"].Value.ToString());
                    decUnit = Convert.ToDecimal(dgvStandardRate.Rows[e.RowIndex].Cells["dgvtxtUnitId"].Value.ToString());
                    decBatchId = Convert.ToDecimal(dgvStandardRate.Rows[e.RowIndex].Cells["dgvtxtBatchId"].Value.ToString());
                    FillControls();
                    btnDelete.Enabled = true;
                    btnSave.Text = "Update";
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP19" + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click
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
                formMDI.infoError.ErrorString = "SRP20" + ex.Message;
            }
        }
        /// <summary>
        /// Close button click
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
                    StandardrateObj.Enabled = true;
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP21" + ex.Message;
            }
        }
        /// <summary>
        /// Call the deciml validation function for Rate field
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
                formMDI.infoError.ErrorString = "SRP22" + ex.Message;
            }
        }
        /// <summary>
        /// To activate the form Standard rate When close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStandardRatePopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (StandardrateObj != null)
                {
                    StandardrateObj.Enabled = true;
                }
            }
            catch (Exception ex)
            {

                formMDI.infoError.ErrorString = "SRP23" + ex.Message;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// For Save,Update and Delete functions call when form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStandardRatePopup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);

                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) 
                {
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
                formMDI.infoError.ErrorString = "SRP24" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key navigation
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
                    txtProductName.SelectionStart = 0;
                    txtProductName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP25" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!txtUnitName.Enabled)
                    {
                        txtUnitName.Focus();
                        txtUnitName.SelectionStart = 0;
                        txtUnitName.SelectionLength = 0;
                    }
                    else
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductName.Text == string.Empty || txtProductName.SelectionStart == 0)
                    {
                        txtProductCode.Focus();
                        txtProductCode.SelectionLength = 0;
                        txtProductCode.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP26" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnitName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFromDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (!txtProductName.Enabled)
                    {
                        if (txtUnitName.Text == string.Empty || txtUnitName.SelectionStart == 0)
                        {
                            txtProductName.Focus();
                            txtProductName.SelectionLength = 0;
                            txtProductName.SelectionStart = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP27" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
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
                if (e.KeyCode == Keys.Back)
                {
                    if (!txtUnitName.Enabled)
                    {
                        if (txtFromDate.Text == string.Empty || txtFromDate.SelectionStart == 0)
                        {
                            txtUnitName.Focus();
                            txtUnitName.SelectionLength = 0;
                            txtUnitName.SelectionStart = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP28" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBatch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtToDate.Text == string.Empty || txtToDate.SelectionStart == 0)
                    {
                        txtFromDate.Focus();
                        txtFromDate.SelectionStart = 0;
                        txtFromDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP29" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
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
                    txtRate.SelectionStart = txtRate.TextLength;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                    txtToDate.SelectionStart = 0;
                    txtToDate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP30" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
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
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SRP31" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
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
                formMDI.infoError.ErrorString = "SRP32" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter Key and BackSpace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStandardRate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvStandardRate.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvStandardRate.CurrentCell.ColumnIndex, dgvStandardRate.CurrentCell.RowIndex);
                        dgvStandardRate_CellDoubleClick(sender, ex);
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
                formMDI.infoError.ErrorString = "SRP33" + ex.Message;
            }
        }
        #endregion


    }
}
