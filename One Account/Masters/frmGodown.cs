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
    public partial class frmGodown : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decGodownId;
        decimal decIdForOtherForms;
        string strGodownName;
        int inNarrationCount = 0;
        int q = 0;
        public frmProductCreation frmProductCreationObj;
        public frmMultipleProductCreation frmMultipleProductCreationObj;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmGodown class
        /// </summary>
        public frmGodown()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to call this form from ProductCreation form
        /// </summary>
        /// <param name="frmProduct"></param>
        public void CallFromProdutCreation(frmProductCreation frmProduct)
        {
            try
            {
                frmProduct.Enabled = false;
                this.frmProductCreationObj = frmProduct;
                base.Show();
                dgvGodown.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G1:" + ex.Message;
                
            }
        }
        /// <summary>
        /// Function to call this form from MultipleProductCreation form
        /// </summary>
        /// <param name="frmMultipleProductCreation"></param>
        public void CallFromMultipleProdutCreation(frmMultipleProductCreation frmMultipleProductCreation)
        {
            try
            {
                dgvGodown.Enabled = false;
                this.frmMultipleProductCreationObj = frmMultipleProductCreation;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Save Rack 
        /// </summary>
        public void RackAddCurrespondingtoGodown()
        {
            try
            {
                RackSP spRack = new RackSP();
                RackInfo infoRack = new RackInfo();
                infoRack.RackName = "NA";
                infoRack.GodownId = decIdForOtherForms;
                infoRack.Narration = string.Empty;
                infoRack.Extra1 = string.Empty;
                infoRack.Extra2 = string.Empty;
                infoRack.ExtraDate = DateTime.Now;
                spRack.RackAdd(infoRack);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                GodownInfo infoGodown = new GodownInfo();
                GodownSP spGodown = new GodownSP();
                infoGodown.GodownName = txtGodownName.Text.Trim();
                infoGodown.Narration = txtNarration.Text.Trim();
                infoGodown.Extra1 = string.Empty;
                infoGodown.Extra2 = string.Empty;
                if (spGodown.GodownCheckIfExist(txtGodownName.Text.Trim(), 0) == false)
                {
                    decIdForOtherForms = spGodown.GodownAddWithoutSameName(infoGodown);
                    RackAddCurrespondingtoGodown();
                    if (decIdForOtherForms > 0)
                    {
                        Messages.SavedMessage();
                        Clear();
                    }
                }
                else
                {
                    Messages.InformationMessage("Godown name already exist");
                    txtGodownName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Edit
        /// </summary>
        public void EditFunction()
        {
            try
            {
                GodownInfo infoGodown = new GodownInfo();
                GodownSP spGodown = new GodownSP();
                infoGodown.GodownName = txtGodownName.Text.Trim();
                infoGodown.Narration = txtNarration.Text.Trim();
                infoGodown.Extra1 = string.Empty;
                infoGodown.Extra2 = string.Empty;
                infoGodown.GodownId = Convert.ToDecimal(dgvGodown.CurrentRow.Cells["dgvtxtGodownId"].Value.ToString());
                if (txtGodownName.Text != strGodownName)
                {
                    if (spGodown.GodownCheckIfExist(txtGodownName.Text.Trim(), decGodownId) == false)
                    {
                        if (spGodown.GodownEditParticularField(infoGodown))
                        {
                            Messages.UpdatedMessage();
                            Clear();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Godown name already exist");
                        txtGodownName.Focus();
                    }
                }
                else
                {
                    spGodown.GodownEditParticularField(infoGodown);
                    Messages.UpdatedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call save
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtGodownName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter godown name");
                    txtGodownName.Focus();
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
                        if (frmProductCreationObj != null)
                        {
                            this.Close();
                        }
                        if (frmMultipleProductCreationObj != null)
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
                formMDI.infoError.ErrorString = "G6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void Gridfill()
        {
            try
            {
                DataTable dtblGodown = new DataTable();
                GodownSP spGodown = new GodownSP();
                dtblGodown = spGodown.GodownOnlyViewAll();
                dgvGodown.DataSource = dtblGodown;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                GodownSP spGodown = new GodownSP();
                if (spGodown.GodownCheckReferenceAndDelete(decGodownId) <= 0)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    // spGodown.GodownDelete(Convert.ToDecimal(dgvGodown.CurrentRow.Cells[1].Value.ToString()));
                    //spGodown.GodownDelete(Convert.ToDecimal(dgvGodown.CurrentRow.Cells["dgvtxtGodownId"].Value.ToString()));
                    Clear();
                    btnSave.Text = "Save";
                    Messages.DeletedMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G8:" + ex.Message;
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
                formMDI.infoError.ErrorString = "G9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear
        /// </summary>
        public void Clear()
        {
            try
            {
                txtGodownName.Text = string.Empty;
                txtNarration.Text = string.Empty;
                txtGodownName.Focus();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls for update
        /// </summary>
        public void FillControls()
        {
            try
            {
                GodownInfo infoGodown = new GodownInfo();
                GodownSP spGodown = new GodownSP();
                infoGodown = spGodown.GodownWithNarrationView(Convert.ToDecimal(dgvGodown.CurrentRow.Cells[1].Value.ToString()));
                txtGodownName.Text = infoGodown.GodownName;
                txtNarration.Text = infoGodown.Narration;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                decGodownId = infoGodown.GodownId;
                strGodownName = infoGodown.GodownName;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G11:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// on form close
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
                formMDI.infoError.ErrorString = "G12:" + ex.Message;
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
                formMDI.infoError.ErrorString = "G13:" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGodown_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G14:" + ex.Message;
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
                formMDI.infoError.ErrorString = "G15:" + ex.Message;
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
                formMDI.infoError.ErrorString = "G16:" + ex.Message;
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
                formMDI.infoError.ErrorString = "G17:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview cell doubleclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGodown_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    string strBatchName = dgvGodown.CurrentRow.Cells["dgvtxtGodownName"].Value.ToString();
                    if (strBatchName != "NA")
                    {
                        FillControls();
                        txtGodownName.Focus();
                    }
                    else
                    {
                        Messages.WarningMessage("NA Godown cannot update or delete");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G18:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on datagridview databinding complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGodown_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvGodown.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G19:" + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGodown_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmProductCreationObj != null)
                {
                    frmProductCreationObj.ReturnFromGodownForm(decIdForOtherForms);
                    dgvGodown.Enabled = true;
                    frmProductCreationObj.Enabled = true;
                }
                if (frmMultipleProductCreationObj != null)//Coded by shihab
                {
                    frmMultipleProductCreationObj.ReturnFromGodownForm(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G20:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// On 'Narratrion' textbox keypress 
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
                formMDI.infoError.ErrorString = "G21:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narratrion' textbox key enter
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
                formMDI.infoError.ErrorString = "G22:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'GodownName' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGodownName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = txtNarration.TextLength;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G23:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Narratrion' textbox keydown
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
                        txtGodownName.Focus();
                        txtGodownName.SelectionLength = 0;
                        txtGodownName.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G24:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Save' textbox keypress 
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
                formMDI.infoError.ErrorString = "G25:" + ex.Message;
            }
        }
        /// <summary>
        /// On form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGodown_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "G26:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGodown_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvGodown.CurrentCell == dgvGodown[dgvGodown.Columns.Count - 1, dgvGodown.Rows.Count - 1])
                    {
                        if (q == 1)
                        {
                            q = 0;
                            btnClose.Focus();
                            dgvGodown.ClearSelection();
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
                formMDI.infoError.ErrorString = "G27:" + ex.Message;
            }
        }
        /// <summary>
        ///  On datagridview keyup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGodown_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvGodown.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvGodown.CurrentCell.ColumnIndex, dgvGodown.CurrentCell.RowIndex);
                        dgvGodown_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "G28:" + ex.Message;
            }
        }
        #endregion
    }
}
