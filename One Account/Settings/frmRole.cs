//This is a source code or part of Oneaccount project
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
    public partial class frmRole : Form
    {
        #region PublicVeriable
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decRoleId;
        int inNarrationCount = 0;
        frmUserCreation frmUserCreationobj;
        #endregion
        #region Function
        /// <summary>
        /// Create an instance for frmRole class
        /// </summary>
        public frmRole()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void Gridfill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                RoleSP spRole = new RoleSP();
                dtbl = spRole.RoleViewGridFill();
                dgvRole.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:1" + ex.Message;
                
            }
        }
        /// <summary>
        /// Function to Clear the controls in form
        /// </summary>
        public void ClearFunction()
        {
            try
            {
                txtRole.Clear();
                txtNarration.Clear();
                Gridfill();
                txtRole.Focus();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:2" + ex.Message;
            }
        }
        /// <summary>
        /// Save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                RoleInfo infoRole = new RoleInfo();
                RoleSP spRole = new RoleSP();
                infoRole.Role = txtRole.Text.Trim();
                infoRole.Narration = txtNarration.Text.Trim();
                infoRole.Extra1 = string.Empty;
                infoRole.Extra2 = string.Empty;
                string strRole = txtRole.Text.Trim();
                if (spRole.RoleCheckExistence(decRoleId, strRole) == false)
                {
                    decRoleId = spRole.RoleAdd(infoRole);
                    Messages.SavedMessage();
                    ClearFunction();
                }
                else
                {
                    Messages.InformationMessage("Role already exists");
                    txtRole.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Update the items
        /// </summary>
        public void EditFunction()
        {
            try
            {
                RoleInfo infoRole = new RoleInfo();
                RoleSP spRole = new RoleSP();
                infoRole.RoleId = Convert.ToDecimal(dgvRole.CurrentRow.Cells["dgvtxtRoleId"].Value);
                infoRole.Role = txtRole.Text.Trim();
                infoRole.Narration = txtNarration.Text.Trim();
                infoRole.Extra1 = string.Empty;
                infoRole.Extra2 = string.Empty;
                string strRole = txtRole.Text.Trim();
                if (spRole.RoleCheckExistence(decRoleId, strRole) == false)
                {
                    spRole.RoleEdit(infoRole);
                    Messages.UpdatedMessage();
                    ClearFunction();
                }
                else
                {
                    Messages.InformationMessage("Role already exists");
                    txtRole.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:4" + ex.Message;
            }
        }
        /// <summary>
        /// Save Or Update Function, to checking invalid entries
        /// </summary>
        public void SaveOrEditFunction()
        {
            try
            {
                if (txtRole.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter role");
                    txtRole.Focus();
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
                        if (frmUserCreationobj != null)
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
                formMDI.infoError.ErrorString = "RL:5" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the Controls for updation
        /// </summary>
        public void FillControls()
        {
            try
            {
                RoleInfo infoRole = new RoleInfo();
                RoleSP spRole = new RoleSP();
                infoRole = spRole.RoleView(decRoleId);
                txtRole.Text = infoRole.Role;
                txtNarration.Text = infoRole.Narration;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:6" + ex.Message;
            }
        }
        /// <summary>
        /// Delete Function
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                if (PublicVariables.isMessageDelete)
                {
                    if (Messages.DeleteMessage())
                    {
                        RoleInfo infoRole = new RoleInfo();
                        RoleSP spRole = new RoleSP();
                        if ((spRole.RoleReferenceDelete(decRoleId) == -1))
                        {
                            Messages.ReferenceExistsMessage();
                        }
                        else
                        {
                            Messages.DeletedMessage();
                            btnSave.Text = "Save";
                            btnDelete.Enabled = false;
                            ClearFunction();
                        }
                    }
                }
                else
                {
                    RoleInfo infoRole = new RoleInfo();
                    RoleSP spRole = new RoleSP();
                    if (spRole.RoleReferenceDelete(decRoleId) == -1)
                    {
                        Messages.ReferenceExistsMessage();
                    }
                    else
                    {
                        Messages.DeletedMessage();
                        btnSave.Text = "Save";
                        btnDelete.Enabled = false;
                        ClearFunction();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:7" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmUserCreation to view details and for updation
        /// </summary>
        /// <param name="frmUserCreation"></param>
        public void CallFromUserCreation(frmUserCreation frmUserCreation)
        {
            try
            {
                dgvRole.Enabled = false;
                this.frmUserCreationobj = frmUserCreation;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:8" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form load call the clear function and fill grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRole_Load(object sender, EventArgs e)
        {
            try
            {
                Gridfill();
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:9" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click, check the user privilage and call save or edit function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEditFunction();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:10" + ex.Message;
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
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:11" + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click, call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:12" + ex.Message;
            }
        }
        /// <summary>
        /// Cell double click for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRole_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    decRoleId = Convert.ToDecimal(dgvRole.CurrentRow.Cells["dgvtxtRoleId"].Value);
                    FillControls();
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:13" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    DeleteFunction();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:14" + ex.Message;
            }
        }
        /// <summary>
        /// Form closing, checking the other form status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRole_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmUserCreationobj != null)
                {
                    frmUserCreationobj.ReturnFromRoleForm(decRoleId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:15" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRole_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "RL:16" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                        txtRole.Focus();
                        txtRole.SelectionStart = 0;
                        txtRole.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:17" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "RL:18" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == String.Empty)
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
                formMDI.infoError.ErrorString = "RL:19" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRole_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ( e.KeyCode == Keys.Enter)
                {
                    if (dgvRole.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvRole.CurrentCell.ColumnIndex, dgvRole.CurrentCell.RowIndex);
                        dgvRole_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RL:20" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRole_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "RL:21" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
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
                formMDI.infoError.ErrorString = "RL:22" + ex.Message;
            }
        }
        /// <summary>
        /// For Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRole_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "RL:23" + ex.Message;
            }
        }
        #endregion
    }
}
