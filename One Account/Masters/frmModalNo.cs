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
//This is a source code or part of Oneaccount project
//Copyright (C) 2013  Oneaccount
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
    public partial class frmModalNo : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strModelNo;
        decimal decModelNo;
        decimal decIdForOtherForms;
        int inNarrationCount = 0;
        int q = 0;
        frmProductCreation frmProductCreationObj;
        frmMultipleProductCreation frmMultipleProductCreationObj;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmModalNo class
        /// </summary>
        public frmModalNo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to clear
        /// </summary>
        public void Clear()
        {
            try
            {
                txtModalNo.Text = string.Empty;
                txtNarration.Text = string.Empty;
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                txtModalNo.Focus();
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN1" + ex.Message;
             
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void Gridfill()
        {
            try
            {
                DataTable dtblModelNo = new DataTable();
                ModelNoSP spModelNo = new ModelNoSP();
                dtblModelNo = spModelNo.ModelNoOnlyViewAll();
                dgvModalNo.DataSource = dtblModelNo;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                ModelNoSP spModelNo = new ModelNoSP();
                ModelNoInfo infoModelNo = new ModelNoInfo();
                infoModelNo.ModelNo = txtModalNo.Text.Trim();
                infoModelNo.Narration = txtNarration.Text.Trim();
                infoModelNo.Extra1 = string.Empty;
                infoModelNo.Extra2 = string.Empty;
                if (spModelNo.ModelCheckIfExist(txtModalNo.Text.Trim().ToString(), 0) == false)
                {
                    decIdForOtherForms = spModelNo.ModelNoAddWithDifferentModelNo(infoModelNo);
                    if (decIdForOtherForms > 0)
                    {
                        Messages.SavedMessage();
                        Clear();
                    }
                }
                else
                {
                    Messages.InformationMessage("Model number already exist");
                    txtModalNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit
        /// </summary>
        public void EditFunction()
        {
            try
            {
                ModelNoSP spModelNo = new ModelNoSP();
                ModelNoInfo infoModelNo = new ModelNoInfo();
                infoModelNo.ModelNo = txtModalNo.Text.Trim();
                infoModelNo.Narration = txtNarration.Text.Trim();
                infoModelNo.Extra1 = string.Empty;
                infoModelNo.Extra2 = string.Empty;
                infoModelNo.ModelNoId = Convert.ToDecimal(dgvModalNo.CurrentRow.Cells[1].Value.ToString());
                if (txtModalNo.Text != strModelNo)
                {
                    if (spModelNo.ModelCheckIfExist(txtModalNo.Text.Trim(), decModelNo) == false)
                    {
                        if (spModelNo.ModelNoEditParticularFeilds(infoModelNo))
                        {
                            Messages.UpdatedMessage();
                            Clear();
                        }
                        else if (infoModelNo.ModelNoId == 1)
                        {
                            Messages.InformationMessage("Cannot update");
                            Clear();
                            txtModalNo.Focus();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Model number already exist");
                        txtModalNo.Focus();
                    }
                }
                else
                {
                    spModelNo.ModelNoEditParticularFeilds(infoModelNo);
                    Messages.UpdatedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN4" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call save or edit
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtModalNo.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter model number");
                    txtModalNo.Focus();
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
                                if (frmProductCreationObj != null)
                                {
                                    this.Close();
                                }
                                if (frmMultipleProductCreationObj != null)
                                {
                                    this.Close();
                                }
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
                formMDI.infoError.ErrorString = "MN5" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                ModelNoSP spModelNo = new ModelNoSP();
                if (spModelNo.ModelNoCheckReferenceAndDelete(decModelNo) <= 0)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    spModelNo.ModelNoDelete(Convert.ToDecimal(dgvModalNo.CurrentRow.Cells[1].Value.ToString()));
                    Clear();
                    Messages.DeletedMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN6" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call delete
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
                formMDI.infoError.ErrorString = "MN7" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls for update
        /// </summary>
        public void FillControls()
        {
            try
            {
                ModelNoInfo infoModelNo = new ModelNoInfo();
                ModelNoSP spModelNo = new ModelNoSP();
                infoModelNo = spModelNo.ModelNoWithNarrationView(Convert.ToDecimal(dgvModalNo.CurrentRow.Cells[1].Value.ToString()));
                txtModalNo.Text = infoModelNo.ModelNo;
                txtNarration.Text = infoModelNo.Narration;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                decModelNo = infoModelNo.ModelNoId;
                strModelNo = infoModelNo.ModelNo;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN8" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from ProductCreation Form
        /// </summary>
        /// <param name="frmProduct"></param>
        public void CallFromProdutCreation(frmProductCreation frmProduct)
        {
            try
            {
                frmProduct.Enabled = false;
                this.frmProductCreationObj = frmProduct;
                base.Show();
                dgvModalNo.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN9" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from MultipleProductCreation Form
        /// </summary>
        /// <param name="frmMultipleProductCreation"></param>
        public void CallMultipleProductCreation(frmMultipleProductCreation frmMultipleProductCreation)
        {
            try
            {
                this.frmMultipleProductCreationObj = frmMultipleProductCreation;
                base.Show();
                dgvModalNo.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN10" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On form Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
        {
            try
            {
                btnClose_Click(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN11" + ex.Message;
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
                formMDI.infoError.ErrorString = "MN12" + ex.Message;
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
                formMDI.infoError.ErrorString = "MN13" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' button click
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
                formMDI.infoError.ErrorString = "MN14" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmModalNo_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN15" + ex.Message;
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
                    //Messages.DeletedMessage();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN16" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview cell double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvModalNo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    string strModelNo = dgvModalNo.CurrentRow.Cells["dgvtxtModelNo"].Value.ToString();
                    if (strModelNo != "NA")
                    {
                        FillControls();
                        txtModalNo.Focus();
                    }
                    else
                    {
                        Messages.WarningMessage("NA ModelNo cannot update or delete");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN17" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on datagridview databinding complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvModalNo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvModalNo.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN18" + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmModalNo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmProductCreationObj != null)
                {
                    frmProductCreationObj.ReturnFromModelNoForm(decIdForOtherForms);
                    dgvModalNo.Enabled = true;
                    frmProductCreationObj.Enabled = true;
                }
                if (frmMultipleProductCreationObj != null)
                {
                    frmMultipleProductCreationObj.ReturnFromModelNoForm(decIdForOtherForms);
                    dgvModalNo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN19" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// On form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmModalNo_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "MN20" + ex.Message;
            }
        }
        /// <summary>
        /// On 'ModalNo' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtModalNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN21" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narration' textbox keypress
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
                formMDI.infoError.ErrorString = "MN22" + ex.Message;
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
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == string.Empty)
                {
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
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
                formMDI.infoError.ErrorString = "MN23" + ex.Message;
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
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        txtModalNo.Focus();
                        txtModalNo.SelectionStart = 0;
                        txtModalNo.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN24" + ex.Message;
            }
        }
        //On 'Save' button keydown
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
                formMDI.infoError.ErrorString = "MN25" + ex.Message;
            }
        }
        /// <summary>
        /// On Datagridview keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvModalNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvModalNo.CurrentCell == dgvModalNo[dgvModalNo.Columns.Count - 1, dgvModalNo.Rows.Count - 1])
                    {
                        if (q == 1)
                        {
                            q = 0;
                            btnClose.Focus();
                            dgvModalNo.ClearSelection();
                            e.Handled = true;
                        }
                        else
                        {
                            q++;
                        }
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnClose.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN26" + ex.Message;
            }
        }
        /// <summary>
        ///  On Datagridview keyup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvModalNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvModalNo.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvModalNo.CurrentCell.ColumnIndex, dgvModalNo.CurrentCell.RowIndex);
                        dgvModalNo_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MN27" + ex.Message;
            }
        }
        #endregion
    }
}
