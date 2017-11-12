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
    public partial class frmRack : Form
    {
        
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        int inNarrationCount = 0;
        string strRackName;
        decimal decIdForOtherForms;
        decimal decRackId = 0;
        frmProductCreation frmProductCreationObj;
        frmMultipleProductCreation frmMultipleProductCreationObj;
        #endregion

        #region  Functions
        /// <summary>
        /// Create instance of frmRack
        /// </summary>
        public frmRack()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill cmbGodown 
        /// </summary>
        public void FillGodownCombo()
        {
            try
            {
                GodownSP spGodown = new GodownSP();
                DataTable dtblGodown = new DataTable();
                dtblGodown = spGodown.GodownViewAll();
                cmbGodown.DataSource = dtblGodown;
                cmbGodown.ValueMember = "godownId";
                cmbGodown.DisplayMember = "godownName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R1:" + ex.Message;
               
            }
        }

        /// <summary>
        /// Function to fill cmbGodownSearch 
        /// </summary>
        public void FillSearchCombo()
        {
            try
            {
                GodownSP spGodown = new GodownSP();
                DataTable dtblGodown = new DataTable();
                dtblGodown = spGodown.GodownViewAll();
                cmbGodownSearch.DataSource = dtblGodown;
                DataRow dr = dtblGodown.NewRow();
                dr[1] = "All";
                dtblGodown.Rows.InsertAt(dr, 0);
                cmbGodownSearch.ValueMember = "godownId";
                cmbGodownSearch.DisplayMember = "godownName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                txtRackName.Clear();
                cmbGodown.SelectedIndex = 0;
                txtNarration.Clear();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                txtRackName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R3:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to reset search groupbox
        /// </summary>
        public void SearchClear()
        {
            try
            {
                cmbGodownSearch.SelectedIndex = 0;
                txtRackNameSearch.Clear();
                txtRackNameSearch.Focus();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R4:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill all rack in datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                RackSP spRack = new RackSP();
                DataTable dtblRack = new DataTable();
                dtblRack = spRack.RackSearch(txtRackNameSearch.Text.Trim(), cmbGodownSearch.Text.ToString());
                dgvRack.DataSource = dtblRack;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R5:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to check rack name alredy exist or not
        /// </summary>
        /// <returns></returns>
        public bool CheckExistenceOfRackName()
        {
            bool isExist = false;
            try
            {
                RackSP spRack = new RackSP();
                isExist = spRack.RackCheckExistence(txtRackName.Text.Trim(), 0,Convert.ToDecimal(cmbGodown.SelectedValue.ToString()));
                if (isExist)
                {
                    string strRackNames = txtRackName.Text.Trim();
                    if (strRackNames.ToLower() == strRackName.ToLower())
                    {
                        isExist = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R6:" + ex.Message;
            }
            return isExist;
        }

        /// <summary>
        /// Function to save new rack
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                    RackSP spRack = new RackSP();
                    RackInfo rackInfo = new RackInfo();
                    rackInfo.RackName = txtRackName.Text.Trim();
                    rackInfo.GodownId = Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                    rackInfo.Narration = txtNarration.Text.Trim();
                    rackInfo.ExtraDate = DateTime.Now;
                    rackInfo.Extra1 = string.Empty;
                    rackInfo.Extra2 = string.Empty;
                    decimal decGodounId=Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                    if (spRack.RackCheckExistence(txtRackName.Text.Trim(), 0, decGodounId) == false)
                    {
                        decIdForOtherForms = spRack.RackAdd(rackInfo);
                        if (decIdForOtherForms > 0)
                        {
                            Messages.SavedMessage();
                            Clear();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage(" Rack name already exist");
                        txtRackName.Focus();
                    }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R7:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to edit the existing rack
        /// </summary>
        public void EditFunction()
        {
            try
            {
                RackSP spRack = new RackSP();
                RackInfo rackInfo = new RackInfo();
                rackInfo.RackName = txtRackName.Text.Trim();
                rackInfo.GodownId = Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                rackInfo.Narration = txtNarration.Text.Trim();
                rackInfo.ExtraDate = DateTime.Now;
                rackInfo.Extra1 = string.Empty;
                rackInfo.Extra2 = string.Empty;
                rackInfo.RackId = decRackId;
                if (txtRackName.Text != strRackName)
                {
                    if (CheckExistenceOfRackName() == false)
                    {
                        if (spRack.RackEdit(rackInfo))
                        {
                            Messages.UpdatedMessage();
                            Clear();
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Already exists");
                        txtRackName.Focus();
                    }
                }
                else if (rackInfo.RackId == 1)
                {
                    Messages.InformationMessage("Cannot update");
                    Clear();
                    txtRackName.Focus();
                }
                else
                {
                    if (spRack.RackEdit(rackInfo))
                    {
                        Messages.UpdatedMessage();
                        Clear();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R8:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to determine whether to call Save or Edit function and also checks invalid entries
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtRackName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage(" Enter rack name ");
                    txtRackName.Focus();
                }
                else if (cmbGodown.SelectedIndex == -1)
                {
                    Messages.InformationMessage(" Select godown");
                    cmbGodown.Focus();
                }

                else if (btnSave.Text == "Save")
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
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R9:" + ex.Message;

            }
        }

        /// <summary>
        /// Function to Call the DeleteFunction after user confirmation
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
                formMDI.infoError.ErrorString = "R10:" + ex.Message;

            }
        }

        /// <summary>
        /// Function to delete a Rack
        /// </summary>
        public void DeleteFunction()
        { 
            try
            {

                RackSP spRack = new RackSP();
                if (spRack.RackDeleteReference(decRackId) <=0)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    Clear();
                    btnSave.Text = "Save";
                    Messages.DeletedMessage();
                    GridFill();
                }
        }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R11:" + ex.Message;

            }
        }

        /// <summary>
        /// Function to load the form while calling from the frmProductCreation to add the rack
        /// </summary>
        /// <param name="frmProduct"></param>
        public void CallFromProdutCreation(frmProductCreation frmProduct)
        {
            try
            {
                frmProduct.Enabled = false;
                groupBox2.Enabled = false;
                this.frmProductCreationObj = frmProduct;               
                base.Show();
                

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R12:" + ex.Message;
            }
        }


        /// <summary>
        /// Function to load the form while calling from the frmProductCreation to add the rack
        /// </summary>
        /// <param name="frmProduct"></param>
        /// <param name="decGowdownId"></param>
        public void CallFromProdutCreationForRackCreation(frmProductCreation frmProduct, decimal decGowdownId)
        {
            try
            {
                this.frmProductCreationObj = frmProduct;

                base.Show();
                groupBox2.Enabled = false;
                cmbGodown.SelectedValue = decGowdownId;
                cmbGodown.Enabled = false;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R13:" + ex.Message;
            }
        }
 
        /// <summary>
        /// Function to load the form while calling from the frmMultipleProductCreation to add the rack
        /// </summary>
        /// <param name="frmMultipleProductCreation"></param>
        /// <param name="decGodownId"></param>
        public void CallFromMultipleProdutCreation(frmMultipleProductCreation frmMultipleProductCreation,decimal decGodownId)
        {
            try
            {
                base.Show();
                dgvRack.Enabled = false;
                groupBox2.Enabled = false;
                cmbGodown.SelectedValue = decGodownId;
                cmbGodown.Enabled = false;
                this.frmMultipleProductCreationObj = frmMultipleProductCreation;


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R14:" + ex.Message;
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRack_Load(object sender, EventArgs e)
        {
            try
            {
                FillGodownCombo();
                FillSearchCombo();
                Clear();
                SearchClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R15:" + ex.Message;

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
                formMDI.infoError.ErrorString = "R16:" + ex.Message;

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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R17:" + ex.Message;

            }
        }

        /// <summary>
        /// On Clear button in search groupbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                SearchClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R18:" + ex.Message;
            }
        }

        /// <summary>
        /// On delete button click, calls the Delete function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
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
                formMDI.infoError.ErrorString = "R19:" + ex.Message;
            }
        }

        /// <summary>
        /// On Save buton click, calls the Save function
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
                    GridFill();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R20:" + ex.Message;

            }
        }

        /// <summary>
        /// On doubleclicking on the grid, It displays the details to edit or delete a rack 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRack_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvRack.Rows[e.RowIndex].Cells["dgvtxtRackName"].Value.ToString() != "NA")
                    {
                        RackSP spRack = new RackSP();
                        RackInfo infoRack = new RackInfo();
                        decRackId = Convert.ToDecimal(dgvRack.Rows[e.RowIndex].Cells["dgvtxtRackId"].Value.ToString());
                        infoRack = spRack.RackView(decRackId);
                        txtRackName.Text = infoRack.RackName;
                        cmbGodown.SelectedValue = infoRack.GodownId.ToString();
                        txtNarration.Text = infoRack.Narration;
                        btnSave.Text = "Update";
                        btnDelete.Enabled = true;
                        strRackName = infoRack.RackName;
                        txtRackName.Focus();
                    }
                    else
                    {
                        Messages.InformationMessage("Default Rack cannot update or delete");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R21:" + ex.Message;
            }
        }

        /// <summary>
        /// On search button click, Calls the GridFill function to display the racks based on the search key
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
                formMDI.infoError.ErrorString = "R22:" + ex.Message;
            }
        }

        /// <summary>
        /// On enter key from the grid, It displays the details to edit or delete a rack 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRack_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvRack.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvRack.CurrentCell.ColumnIndex, dgvRack.CurrentCell.RowIndex);
                        dgvRack_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R23:" + ex.Message;
            }
        }

        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRack_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmProductCreationObj != null)
                {
                    frmProductCreationObj.ReturnFromRackForm(decIdForOtherForms);
                    groupBox2.Enabled = true;
                    cmbGodown.Enabled = true;
                    frmProductCreationObj.Enabled = true;
                }
                if (frmMultipleProductCreationObj  != null)//Coded by shihab
                {
                    frmMultipleProductCreationObj.ReturnFromRackForm(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R24:" + ex.Message;
            }
        }


        #endregion

        #region Navigation

        /// <summary>
        /// For enter key and backspace navigation of godown combo box in the Search area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGodownSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtRackNameSearch.Focus();
                    txtRackNameSearch.SelectionStart = 0;
                    txtRackNameSearch.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R25:" + ex.Message;

            }
        }

        /// <summary>
        /// For the backspace navigation of Search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbGodownSearch.Focus();
                }
                

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R26:" + ex.Message;
            }
        }
        
        /// <summary>
        /// For the shortcut keys
        /// Esc for escape
        /// ctrl+s for Save
        /// ctrl+d for Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRack_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (cmbGodown.Focused)
                    {
                        cmbGodown.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    else
                    {
                        cmbGodown.DropDownStyle = ComboBoxStyle.DropDown;
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
                formMDI.infoError.ErrorString = "R27:" + ex.Message;
            }
        }

        /// <summary>
        /// For enter key navigation of Rackname textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRackName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbGodown.Focus();
                    if (cmbGodown.Enabled==false)
                    {
                        txtNarration.Focus();
                        txtNarration.SelectionStart = txtNarration.TextLength;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R28:" + ex.Message;

            }
        }

        /// <summary>
        /// For Enter key and backspace navigation of godown combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGodown_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtRackName.Focus();
                    txtRackName.SelectionStart = 0;
                    txtRackName.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R29:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of narration textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbGodown.Enabled==true)
                    {
                        if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                        {
                            cmbGodown.Focus();
                        }
                    }
                    else
                    {
                        txtRackName.Focus();
                        txtRackName.SelectionStart = 0;
                        txtRackName.SelectionLength = 0;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R30:" + ex.Message;
            }
        }

        /// <summary>
        /// For enter key navigation of Narrtaion textbox
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
                formMDI.infoError.ErrorString = "R31:" + ex.Message;
            }
        }

        /// <summary>
        /// For the backspace navigtion of Save button
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
                formMDI.infoError.ErrorString = "R32:" + ex.Message;
            }
        }

        /// <summary>
        /// For Enter key navigation of rackname textbox in search area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRackNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbGodownSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R33:" + ex.Message;
            }
        }

        /// <summary>
        /// For backspace navigation of rack datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRack_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbGodownSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "R34:" + ex.Message;
            }
        }
      
        #endregion

    }
}
