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
    public partial class frmCounter : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        string strCounterName;
        int inNarrationCount = 0;
        int inq = 0;
        decimal decCounterId;
        frmPOS frmPOSObj;
        decimal decIdForOtherForms = 0;
        decimal decLedgerId;
        bool isFromPOSCounterCombo = false;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmCounter class
        /// </summary>
        public frmCounter()
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
                txtCounterName.Text = string.Empty;
                txtNarration.Text = string.Empty;
                txtCounterName.Focus();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                Gridfill();
                dgvCounter.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT1" + ex.Message;
               
            }
        }
        /// <summary>
        /// Function to fill datagridview
        /// </summary>
        public void Gridfill()
        {
            try
            {
                DataTable dtblCounter = new DataTable();
                CounterSP spCounter = new CounterSP();
                dtblCounter = spCounter.CounterOnlyViewAll();
                dgvCounter.DataSource = dtblCounter;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                CounterInfo infoCounter = new CounterInfo();
                CounterSP spCounter = new CounterSP();
                infoCounter.CounterName = txtCounterName.Text.Trim();
                infoCounter.Narration = txtNarration.Text.Trim();
                infoCounter.Extra1 = string.Empty;
                infoCounter.Extra2 = string.Empty;
                if (spCounter.CounterCheckIfExist(txtCounterName.Text.Trim(), 0) == false)
                {
                    decLedgerId = spCounter.CounterAddWithIdentity(infoCounter);
                    Messages.SavedMessage();
                    Clear();
                    decIdForOtherForms = decLedgerId;
                }
                else
                {
                    Messages.InformationMessage("Counter name already exist");
                    txtCounterName.Focus();
                }
                if (frmPOSObj != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit
        /// </summary>
        public void EditFunction()
        {
            try
            {
                CounterInfo infoCounter = new CounterInfo();
                CounterSP spCounter = new CounterSP();
                infoCounter.CounterName = txtCounterName.Text.Trim();
                infoCounter.Narration = txtNarration.Text.Trim();
                infoCounter.Extra1 = string.Empty;
                infoCounter.Extra2 = string.Empty;
                infoCounter.CounterId = Convert.ToDecimal(dgvCounter.CurrentRow.Cells["dgvtxtcounterId"].Value.ToString());
                if (txtCounterName.Text.ToString() != strCounterName)
                {
                    if (spCounter.CounterCheckIfExist(txtCounterName.Text.Trim(), decCounterId) == false)
                    {
                        if (spCounter.CounterEditParticularField(infoCounter))
                        {
                            Messages.UpdatedMessage();
                            Clear();
                        }
                        else if (infoCounter.CounterId == 1)
                        {
                            Messages.InformationMessage("Cannot update");
                            Clear();
                            txtCounterName.Focus();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Counter name already exist");
                        txtCounterName.Focus();
                    }
                }
                else
                {
                    spCounter.CounterEditParticularField(infoCounter);
                    Messages.UpdatedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT4" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Save or Edit 
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtCounterName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter counter name");
                    txtCounterName.Focus();
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
                formMDI.infoError.ErrorString = "CT5" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                CounterSP spCounter = new CounterSP();
                if (spCounter.CounterCheckReferenceAndDelete(decCounterId) <= 0)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    spCounter.CounterDelete(Convert.ToDecimal(dgvCounter.CurrentRow.Cells[1].Value.ToString()));
                    Messages.DeletedMessage();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT6" + ex.Message;
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
                formMDI.infoError.ErrorString = "CT7" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls to update
        /// </summary>
        public void FillControls()
        {
            try
            {
                CounterInfo infoCounter = new CounterInfo();
                CounterSP spCounter = new CounterSP();
                infoCounter = spCounter.CounterWithNarrationView(Convert.ToDecimal(dgvCounter.CurrentRow.Cells[1].Value.ToString()));
                txtCounterName.Text = infoCounter.CounterName;
                txtNarration.Text = infoCounter.Narration;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                decCounterId = infoCounter.CounterId;
                strCounterName = infoCounter.CounterName;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT8" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from POS
        /// </summary>
        /// <param name="frmPOS"></param>
        /// <param name="isFromCounter"></param>
        public void callFromPOS(frmPOS frmPOS, bool isFromCounter)
        {
            try
            {
                isFromPOSCounterCombo = isFromCounter;
                dgvCounter.Enabled = false;
                this.frmPOSObj = frmPOS;
                frmPOSObj.Enabled = false;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT9" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On form close
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
                formMDI.infoError.ErrorString = "CT10" + ex.Message;
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
                formMDI.infoError.ErrorString = "CT11" + ex.Message;
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
                formMDI.infoError.ErrorString = "CT12" + ex.Message;
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
                formMDI.infoError.ErrorString = "CT13" + ex.Message;
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
                formMDI.infoError.ErrorString = "CT14" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCounter_Load(object sender, EventArgs e)
        {
            try
            {
                btnDelete.Enabled = false;
                Gridfill();
                dgvCounter.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT15" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview cell doubleclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCounter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvCounter.Rows[e.RowIndex].Cells["dgvtxtCounterName"].Value.ToString() != "NA")
                {
                    if (e.RowIndex != -1)
                    {
                        FillControls();
                        txtCounterName.Focus();
                    }

                }
                else
                {
                    Messages.InformationMessage("Default Counter cannot update or delete");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT16" + ex.Message;
            }
        }
        /// <summary>
        /// Clears datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCounter_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvCounter.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT17" + ex.Message;
            }
        }
        #endregion
        #region Navigation
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
                formMDI.infoError.ErrorString = "CT18" + ex.Message;
            }
        }
        /// <summary>
        ///  On 'Narration' textbox keyenter
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
                formMDI.infoError.ErrorString = "CT19" + ex.Message;
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
                        txtCounterName.Focus();
                        txtCounterName.SelectionStart = 0;
                        txtCounterName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT20" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Counter' textbox keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCounterName_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "CT21" + ex.Message;
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
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT22" + ex.Message;
            }
        }
        /// <summary>
        /// On form keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCounter_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "CT23" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCounter_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvCounter.CurrentCell == dgvCounter[dgvCounter.Columns.Count - 1, dgvCounter.Rows.Count - 1])
                    {
                        if (inq == 1)
                        {
                            inq = 0;
                            btnClose.Focus();
                            dgvCounter.ClearSelection();
                            e.Handled = true;
                        }
                        else
                        {
                            inq++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT24" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridview key up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCounter_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvCounter.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvCounter.CurrentCell.ColumnIndex, dgvCounter.CurrentCell.RowIndex);
                        dgvCounter_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT25" + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCounter_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmPOSObj != null)
                {
                    frmPOSObj.ReturnFromCounter(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CT26" + ex.Message;
            }
        }
        #endregion
    }
}
