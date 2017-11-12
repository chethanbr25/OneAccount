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
    public partial class frmPricingLevel : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strPricingLevel;
        decimal decPricingLevel;
        int inNarrationCount = 0;
        int q = 0;
        decimal decPricingLevelId;
        frmCustomer frmCustomerObj;
        decimal decIdforOtherForms = 0;
        frmDeliveryNote frmDeliveryNoteObj;
        frmSalesReturn frmSalesReturnObj;
        frmSalesInvoice frmSalesInvoiceObj;
        frmPOS frmPOSObj;
        #endregion
        #region Functions
        /// <summary>
        /// Create instance of frmPricingLevel
        /// </summary>
        public frmPricingLevel()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to save the Pricinglevel
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                PricingLevelInfo infoPricingLevel = new PricingLevelInfo();
                infoPricingLevel.PricinglevelName = txtPricingLevelName.Text.Trim();
                infoPricingLevel.Narration = txtNarration.Text.Trim();
                infoPricingLevel.Extra1 = string.Empty;
                infoPricingLevel.Extra2 = string.Empty;
                if (spPricingLevel.PricingLevelCheckIfExist(txtPricingLevelName.Text.Trim().ToString(), 0) == false)
                {
                    decPricingLevelId = spPricingLevel.PricingLevelAddWithoutSamePricingLevel(infoPricingLevel);
                    Messages.SavedMessage();
                    Clear();
                    decIdforOtherForms = decPricingLevelId;
                    if (frmCustomerObj != null)
                    {
                        this.Close();
                    }
                    if (frmSalesReturnObj != null)
                    {
                        this.Close();
                    }
                    if (frmSalesInvoiceObj != null)
                    {
                        this.Close();
                    }
                }
                else
                {
                    Messages.InformationMessage("Pricing level name already exist");
                    txtPricingLevelName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL1" + ex.Message;
              
            }
        }
        /// <summary>
        /// Function to edit the Pricing level
        /// </summary>
        public void EditFunction()
        {
            try
            {
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                PricingLevelInfo infoPricingLevel = new PricingLevelInfo();
                infoPricingLevel.PricinglevelName = txtPricingLevelName.Text.Trim();
                infoPricingLevel.Narration = txtNarration.Text.Trim();
                infoPricingLevel.Extra1 = string.Empty;
                infoPricingLevel.Extra2 = string.Empty;
                infoPricingLevel.PricinglevelId = Convert.ToDecimal(dgvPricingLevel.CurrentRow.Cells[1].Value.ToString());
                if (txtPricingLevelName.Text.ToString() != strPricingLevel)
                {
                    if (spPricingLevel.PricingLevelCheckIfExist(txtPricingLevelName.Text.Trim().ToString(), decPricingLevel) == false)
                    {
                        if (spPricingLevel.PricingLevelEditParticularFields(infoPricingLevel))
                        {
                            Messages.UpdatedMessage();
                            Clear();
                        }
                        else if (infoPricingLevel.PricinglevelId == 1)
                        {
                            Messages.InformationMessage("Cannot update");
                            Clear();
                            txtPricingLevelName.Focus();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Pricing level name already exist");
                        txtPricingLevelName.Focus();
                    }
                }
                else
                {
                    spPricingLevel.PricingLevelEditParticularFields(infoPricingLevel);
                    Messages.UpdatedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to determine whether to call save edit function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtPricingLevelName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter pricing level name");
                    txtPricingLevelName.Focus();
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
                        if (frmDeliveryNoteObj != null)
                        {
                            if (decPricingLevelId != 0)
                            {
                                this.Close();
                            }
                            else
                            {
                                txtPricingLevelName.Focus();
                            }
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
                formMDI.infoError.ErrorString = "PL3" + ex.Message;
            }
        }
        /// <summary>
        /// function to fill all pricing level on the datagridview
        /// </summary>
        public void Gridfill()
        {
            try
            {
                DataTable dtblPricingLevel = new DataTable();
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                dtblPricingLevel = spPricingLevel.PricingLevelOnlyViewAll();
                dgvPricingLevel.DataSource = dtblPricingLevel;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL4" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                txtPricingLevelName.Text = string.Empty;
                txtNarration.Text = string.Empty;
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                txtPricingLevelName.Focus();
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL5" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete selected pricing level
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                if (spPricingLevel.PricingLevelCheckReferenceAndDelete(decPricingLevel) <= 0)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    spPricingLevel.PricingLevelDelete(Convert.ToDecimal(dgvPricingLevel.CurrentRow.Cells[1].Value.ToString()));
                    Messages.DeletedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL6" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call DeleteFunction after user confirmation
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
                formMDI.infoError.ErrorString = "PL7" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the fields for Edit or Delete 
        /// </summary>
        public void FillControls()
        {
            try
            {
                PricingLevelSP spPricingLevel = new PricingLevelSP();
                PricingLevelInfo infoPricingLevel = new PricingLevelInfo();
                infoPricingLevel = spPricingLevel.PricingLevelWithNarrationView(Convert.ToDecimal(dgvPricingLevel.CurrentRow.Cells[1].Value.ToString()));
                txtPricingLevelName.Text = infoPricingLevel.PricinglevelName;
                txtNarration.Text = infoPricingLevel.Narration;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                decPricingLevel = infoPricingLevel.PricinglevelId;
                strPricingLevel = infoPricingLevel.PricinglevelName;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL8" + ex.Message;
            }
        }
        /// <summary>
        /// Function to load the form while calling from the Customer form to add new Pricing level
        /// </summary>
        /// <param name="frmCustomer"></param>
        public void CallFromCustomer(frmCustomer frmCustomer)
        {
            try
            {
                dgvPricingLevel.Enabled = false;
                this.frmCustomerObj = frmCustomer;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL9" + ex.Message;
            }
        }
        /// <summary>
        /// Function to load the form while calling from deliverynote form to add new pricinglevel
        /// </summary>
        /// <param name="frmDeliveryNote"></param>
        public void CallFromDeliveryNote(frmDeliveryNote frmDeliveryNote)
        {
            try
            {
                dgvPricingLevel.Enabled = false;
                this.frmDeliveryNoteObj = frmDeliveryNote;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL10" + ex.Message;
            }
        }
        /// <summary>
        /// Function to load the form while calling from Salesreturn form to add new pricinglevel
        /// </summary>
        /// <param name="frmSalesReturn"></param>
        public void CallFromSalesReturn(frmSalesReturn frmSalesReturn)
        {
            try
            {
                dgvPricingLevel.Enabled = false;
                this.frmSalesReturnObj = frmSalesReturn;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL11" + ex.Message;
            }
        }
        /// <summary>
        /// Function to load the form while calling from SalesInvoice form to add new pricinglevel
        /// </summary>
        /// <param name="frmSalesInvoice"></param>
        public void callFromSalesInvoice(frmSalesInvoice frmSalesInvoice)
        {
            try
            {
                dgvPricingLevel.Enabled = false;
                this.frmSalesInvoiceObj = frmSalesInvoice;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL12" + ex.Message;
            }
        }
        /// <summary>
        /// Function to load the form while calling from POS form to add new pricinglevel
        /// </summary>
        /// <param name="frmPOS"></param>
        public void callFromPOS(frmPOS frmPOS)
        {
            try
            {
                dgvPricingLevel.Enabled = false;
                this.frmPOSObj = frmPOS;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL13" + ex.Message;
            }
        }
        #endregion
        #region Events
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
                formMDI.infoError.ErrorString = "PL14" + ex.Message;
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
                btnSave.Text = "Save";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL15" + ex.Message;
            }
        }
        /// <summary>
        /// On Delete button Click
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
                formMDI.infoError.ErrorString = "PL16" + ex.Message;
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
                formMDI.infoError.ErrorString = "PL17" + ex.Message;
            }
        }
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPricingLevel_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL18" + ex.Message;
            }
        }
        /// <summary>
        /// On double clicking the datagridview, It fills the details for edit or save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPricingLevel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvPricingLevel.CurrentRow != null)
                {
                    if (dgvPricingLevel.Rows.Count > 0 && e.ColumnIndex > -1)
                    {
                        if (dgvPricingLevel.CurrentRow.Cells["dgvtxtPricingLevel"].Value != null)
                        {
                            if (dgvPricingLevel.CurrentRow.Cells["dgvtxtPricingLevelId"].Value.ToString() != "")
                            {
                                FillControls();
                                txtPricingLevelName.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL19" + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPricingLevel_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmCustomerObj != null)
                {
                    frmCustomerObj.ReturnFromPricingLevelForm(decPricingLevelId);
                }
                if (frmDeliveryNoteObj != null)
                {
                    frmDeliveryNoteObj.ReturnFromPricingLevelForm(decPricingLevelId);
                }
                if (frmSalesReturnObj != null)
                {
                    frmSalesReturnObj.ReturnFromPricingLevel(decIdforOtherForms);
                }
                if (frmSalesInvoiceObj != null)
                {
                    frmSalesInvoiceObj.ReturnFromPricingLevel(decIdforOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL20" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// for the navigation from dgvPricingLevel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPricingLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvPricingLevel.CurrentCell == dgvPricingLevel[dgvPricingLevel.Columns.Count - 1, dgvPricingLevel.Rows.Count - 1])
                    {
                        if (q == 1)
                        {
                            q = 0;
                            btnClose.Focus();
                            dgvPricingLevel.ClearSelection();
                            e.Handled = true;
                        }
                        else
                        {
                            q++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL21" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key navigation of txtPricingLevelName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPricingLevelName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL22" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key navigation of txtNarration
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
                        inNarrationCount = 0;
                        btnSave.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL23" + ex.Message;
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
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL24" + ex.Message;
            }
        }
        /// <summary>
        /// For the backspace navigation of txtNarration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        txtPricingLevelName.Focus();
                        txtPricingLevelName.SelectionStart = 0;
                        txtPricingLevelName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL25" + ex.Message;
            }
        }
        /// <summary>
        /// For the shortcut keys
        /// Esc for closing the form
        /// ctrl+s for save
        /// ctrl+d for for delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPricingLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
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
                formMDI.infoError.ErrorString = "PL26" + ex.Message;
            }
        }
        /// <summary>
        /// To perform celldoubleclick on enter key in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPricingLevel_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvPricingLevel.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvPricingLevel.CurrentCell.ColumnIndex, dgvPricingLevel.CurrentCell.RowIndex);
                        dgvPricingLevel_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PL27" + ex.Message;
            }
        }
        #endregion
    }
}
