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
    public partial class frmUnit : Form
    {


        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        decimal decUnitid = 0;
        decimal decIdforOtherForm = 0;
        int inNarrationCount = 0;
        string strUnitName;
        decimal decUnit = 0;
        frmMultipleProductCreation frmMultipleProductCreationObj;
        frmProductCreation frmProductCreationObj;
        #endregion

        #region Functions
        /// <summary>
        /// create an instance for frmUnit class
        /// </summary>
        public frmUnit()
        {
            InitializeComponent();
        }
        /// <summary>
        /// grid fill function 
        /// </summary>
        public void GridFill()
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                DataTable dtblUnit = new DataTable();
                dtblUnit = spUnit.UnitSearch(txtUnitSearch.Text.Trim());
                dgvUnitSearch.DataSource = dtblUnit;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U1:" + ex.Message;
                
            }
        }
        /// <summary>
        /// here checking the unit name if already exists for create new unit
        /// </summary>
        /// <returns></returns>
        public bool CheckExistenceOfUnitName()
        {
            bool isExist = false;
            try
            {
                UnitSP spUnit = new UnitSP();
                isExist = spUnit.UnitCheckExistence(txtUnitname.Text.Trim(), 0);
                if (isExist)
                {
                    string strUnitNames = txtUnitname.Text.Trim();
                    if (strUnitNames.ToLower() == strUnitName.ToLower())
                    {
                        isExist = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U2:" + ex.Message;
            }
            return isExist;
        }
        /// <summary>
        /// save function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                UnitInfo infoUnit = new UnitInfo();
                infoUnit.UnitName = txtUnitname.Text.Trim();
                infoUnit.noOfDecimalplaces = Convert.ToDecimal(txtDecimalPlaces.Text.ToString());
                infoUnit.Narration = txtNarration.Text.Trim();
                infoUnit.formalName = txtFormalName.Text.Trim();
                infoUnit.Extra1 = string.Empty;
                infoUnit.Extra2 = string.Empty;
                infoUnit.ExtraDate = DateTime.Now;
                if (spUnit.UnitCheckExistence(txtUnitname.Text.Trim(), 0) == false)
                {
                    decUnit = spUnit.UnitAdd(infoUnit);
                    if (decUnit > 0)
                    {
                        Messages.SavedMessage();
                        GridFill();
                        Clear();
                    }
                }
                else
                {
                    Messages.InformationMessage(" Unit name already exist");
                    txtUnitname.Focus();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U3:" + ex.Message;
            }
        }
        /// <summary>
        /// update function
        /// </summary>
        public void EditFunction()
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                UnitInfo infoUnit = new UnitInfo();
                infoUnit.UnitName = txtUnitname.Text.Trim();
                infoUnit.noOfDecimalplaces = Convert.ToDecimal(txtDecimalPlaces.Text.ToString());
                infoUnit.Narration = txtNarration.Text.Trim();
                infoUnit.formalName = txtFormalName.Text.Trim();
                infoUnit.UnitId = decUnitid;
                infoUnit.Extra1 = string.Empty;
                infoUnit.Extra2 = string.Empty;
                if (CheckExistenceOfUnitName() == false)
                {
                    if (spUnit.UnitEdit(infoUnit))
                    {
                        Messages.UpdatedMessage();
                        Clear();
                    }
                }
                else
                {
                    Messages.InformationMessage("Already exists");
                    txtUnitname.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U4:" + ex.Message;
            }
        }
        /// <summary>
        /// checking the invalid entries and call asve function or edit function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {

                if (txtUnitname.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage(" Enter unit name ");
                    txtUnitname.Focus();
                }
                else if (txtFormalName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage(" Enter formal name");
                    txtFormalName.Focus();
                }
                else if (txtDecimalPlaces.Text == string.Empty)
                {
                    Messages.InformationMessage(" Enter no of decimal places");
                    txtDecimalPlaces.Focus();
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
                                decIdforOtherForm = decUnit;
                                if (frmMultipleProductCreationObj != null)
                                {
                                    this.Close();
                                }
                                if (frmProductCreationObj != null)
                                {
                                    this.Close();
                                }
                            }
                        }
                        else
                        {
                            SaveFunction();
                            decIdforOtherForm = decUnit;
                            if (frmMultipleProductCreationObj != null)
                            {
                                this.Close();
                            }
                            if (frmProductCreationObj != null)
                            {
                                this.Close();
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
                formMDI.infoError.ErrorString = "U5:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete function
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
                formMDI.infoError.ErrorString = "U6:" + ex.Message;
            }
        }
        /// <summary>
        /// checking references fro delete
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                if (spUnit.UnitDeleteCheck(decUnitid) <= 0)
                {
                    Messages.ReferenceExistsMessage();

                }
                else
                {
                    Clear();
                    btnSave.Text = "Save";
                    Messages.DeletedMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U7:" + ex.Message;
            }
        }
        /// <summary>
        /// form will be reset here
        /// </summary>
        public void Clear()
        {
            try
            {
                txtDecimalPlaces.Clear();
                txtFormalName.Clear();
                txtNarration.Clear();
                txtUnitname.Clear();
                txtUnitSearch.Clear();
                GridFill();
                btnDelete.Enabled = false;
                btnSave.Text = "Save";
                txtUnitname.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U8:" + ex.Message;
            }
        }
        /// <summary>
        /// its a call from multiple product creation form for creating a new unit
        /// </summary>
        /// <param name="frmMultipleProductCreation"></param>
        public void CallFromMultipleProductCreation(frmMultipleProductCreation frmMultipleProductCreation)
        {
            try
            {
                dgvUnitSearch.Enabled = false;
                this.frmMultipleProductCreationObj = frmMultipleProductCreation;
                base.Show();
                groupBox2.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U9:" + ex.Message;
            }
        }
        /// <summary>
        /// its a call from  product Creation form for creating a new unit
        /// </summary>
        /// <param name="frmProduct"></param>
        public void CallFromProdutCreation(frmProductCreation frmProduct)
        {
            try
            {
                frmProduct.Enabled = false;
                this.frmProductCreationObj = frmProduct;
                base.Show();
                groupBox2.Enabled = false;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U10:" + ex.Message;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// save button click
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
                formMDI.infoError.ErrorString = "U11:" + ex.Message;
            }
        }
        /// <summary>
        /// clear button click
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
                formMDI.infoError.ErrorString = "U12:" + ex.Message;
            }
        }
        /// <summary>
        /// delete button click
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
                formMDI.infoError.ErrorString = "U13:" + ex.Message;
            }
        }
        /// <summary>
        /// grid cell double click event for editing purpose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvUnitSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvUnitSearch.CurrentRow != null)
                {
                    if (dgvUnitSearch.Rows.Count > 0 && e.ColumnIndex > -1)
                    {
                        if (dgvUnitSearch.CurrentRow.Cells["dgvtxtUnitId"].Value != null)
                        {
                            if (dgvUnitSearch.CurrentRow.Cells["dgvtxtUnitId"].Value.ToString() != string.Empty)
                            {
                                if (Convert.ToDecimal(dgvUnitSearch.CurrentRow.Cells["dgvtxtUnitId"].Value) != 1)
                                {
                                    UnitSP spUnit = new UnitSP();
                                    UnitInfo infoUnit = new UnitInfo();
                                    decUnitid = decimal.Parse(dgvUnitSearch.CurrentRow.Cells["dgvtxtUnitId"].Value.ToString());
                                    infoUnit = spUnit.UnitView(decUnitid);
                                    txtFormalName.Text = infoUnit.formalName;
                                    txtDecimalPlaces.Text = infoUnit.noOfDecimalplaces.ToString();
                                    txtNarration.Text = infoUnit.Narration;
                                    txtUnitSearch.Text = string.Empty;
                                    txtUnitname.Text = infoUnit.UnitName;
                                    strUnitName = infoUnit.UnitName;
                                    btnSave.Text = "Update";
                                    txtUnitname.Focus();
                                    btnDelete.Enabled = true;
                                }
                                else
                                {
                                    Messages.InformationMessage("Default unit cannot update or delete");
                                    Clear();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U14:" + ex.Message;
            }
        }
        /// <summary>
        /// grid keyup event for edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvUnitSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvUnitSearch.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvUnitSearch.CurrentCell.ColumnIndex, dgvUnitSearch.CurrentCell.RowIndex);
                        dgvUnitSearch_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U15:" + ex.Message;
            }
        }
        /// <summary>
        /// when form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUnit_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U16:" + ex.Message;
            }
        }
        /// <summary>
        /// checking the no and decimal values only in the textbox of txt decimal places
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDecimalPlaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U17:" + ex.Message;
            }
        }
        /// <summary>
        /// close button click
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
                formMDI.infoError.ErrorString = "U18:" + ex.Message;
            }
        }
        /// <summary>
        /// text change event for search the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnitSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U19:" + ex.Message;
            }
        }
        /// <summary>
        /// form closing event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUnit_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmMultipleProductCreationObj != null)
                {
                    dgvUnitSearch.Enabled = false;
                    frmMultipleProductCreationObj.ReturnFromUnitForm(decIdforOtherForm);
                }
                if (frmProductCreationObj != null)
                {
                    groupBox2.Enabled = false;
                    frmProductCreationObj.ReturnFromUnitForm(decIdforOtherForm);
                    frmProductCreationObj.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U20:" + ex.Message;
            }

        }

        #endregion

        #region Navigation
        /// <summary>
        /// form keydown event for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUnit_KeyDown(object sender, KeyEventArgs e)
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
                    if (btnDelete.Enabled == true)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U21:" + ex.Message;
            }
        }
        /// <summary>
        /// naration textbox enter
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
                formMDI.infoError.ErrorString = "U22:" + ex.Message;
            }
        }
        /// <summary>
        /// narration key press for navigation
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
                formMDI.infoError.ErrorString = "U23:" + ex.Message;
            }
        }
        /// <summary>
        /// unot name textbox keydown for enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnitname_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtFormalName.Focus();
                    txtFormalName.SelectionStart = txtFormalName.TextLength;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U24:" + ex.Message;
            }
        }
        /// <summary>
        /// textbox fromal name keydown for enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFormalName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDecimalPlaces.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtFormalName.Text.Trim() == "" || txtFormalName.SelectionStart == 0)
                    {
                        txtUnitname.Focus();
                        txtUnitname.SelectionStart = 0;
                        txtUnitname.SelectionLength = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U25:" + ex.Message;
            }
        }
        /// <summary>
        /// decimal place keydown for enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDecimalPlaces_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDecimalPlaces.Text.Trim() == string.Empty || txtDecimalPlaces.SelectionStart == 0)
                    {
                        txtFormalName.Focus();
                        txtFormalName.SelectionStart = 0;
                        txtFormalName.SelectionLength = 0;
                       
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U26:" + ex.Message;
            }
        }
        /// <summary>
        /// narration keydown backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text.Trim() == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        txtDecimalPlaces.Focus();
                        txtDecimalPlaces.SelectionStart = 0;
                        txtDecimalPlaces.SelectionLength = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U27:" + ex.Message;
            }
        }
        /// <summary>
        /// keydown of unit search for navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnitSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvUnitSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U28:" + ex.Message;
            }
        }
        /// <summary>
        /// key down event for backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvUnitSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtUnitSearch.Focus();
                    txtUnitSearch.SelectionStart = 0;
                    txtUnitSearch.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "U29:" + ex.Message;
            }
        }



        #endregion

        private void txtDecimalPlaces_MouseClick(object sender, MouseEventArgs e)
        {

        }



    }
}
