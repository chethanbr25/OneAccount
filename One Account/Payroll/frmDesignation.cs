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
    public partial class frmDesignation : Form
    {

        #region PublicVariables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decDesignationId;
        decimal decIdentity;
        string strDesignation = string.Empty; // To hold current designation while ediitng
        int inNarrationCount = 0;  // to count number of times enter key pressed
        decimal decUserId = PublicVariables._decCurrentUserId;
        string strFormName = "frmDesignation";
        frmEmployeeCreation frmEmployeeObj;
        decimal decIdForOtherForms = 0;
        #endregion

        #region Functions
        /// <summary>
        /// Creates an instance of frmDesignation class
        /// </summary>
        public frmDesignation()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to call Save or Edit
        /// </summary>
        private void SaveOrEdit()
        {
            try
            {
                if (txtAdvanceAmount.Text == string.Empty)
                {
                    txtAdvanceAmount.Text = "0";
                }
                if (txtCLInMonth.Text == string.Empty)
                {
                    txtCLInMonth.Text = "0";
                }
                if (txtDesignationName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter designation name");
                    txtDesignationName.Focus();
                }
                else if (decimal.Parse(txtCLInMonth.Text) > 30)
                {
                    Messages.InformationMessage("Maximum CL in a month is 30");
                    txtCLInMonth.Focus();
                }

                else
                {
                    if (btnSave.Text == "Save")
                    {
                        if (PublicVariables.isMessageAdd)// for message option
                        {
                            if (Messages.SaveMessage())
                            {
                                SaveFunction();
                                Clear();
                                GridFill();
                            }
                        }
                        else
                        {
                            SaveFunction();
                            Clear();
                            GridFill();
                        }

                        decIdForOtherForms = decIdentity;
                        if (frmEmployeeObj != null)
                        {
                            this.Close();
                        }
                    }

                    if (btnSave.Text == "Update")
                    {
                        if (PublicVariables.isMessageEdit)
                        {

                            if (Messages.UpdateMessage())
                            {
                                EditFunction();
                                Clear();
                                GridFill();
                            }
                        }

                        else
                        {
                            EditFunction();
                            Clear();
                            GridFill();
                        }

                    }
                    GridFill();

                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Delete
        /// </summary>
        private void Delete()
        {
            try
            {

                if (PublicVariables.isMessageDelete)
                {
                    if (Messages.DeleteMessage() == true)
                    {
                        bool isDeleteStatus = false;
                        DesignationSP spDestination = new DesignationSP();
                        isDeleteStatus = spDestination.DesignationDelete(decDesignationId);
                        if (isDeleteStatus == true)
                        {
                            Messages.DeletedMessage();
                            Clear();
                            GridFill();
                        }
                        else
                        {
                            Messages.ReferenceExistsMessage();
                        }
                    }
                }

                else
                {

                    bool isDeleteStatus = false;
                    DesignationSP spDestination = new DesignationSP();
                    isDeleteStatus = spDestination.DesignationDelete(decDesignationId);
                    if (isDeleteStatus == true)
                    {

                        Messages.DeletedMessage();
                        Clear();
                        GridFill();
                    }
                    else
                    {
                        Messages.ReferenceExistsMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D2:" + ex.Message;
            }

        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        private void GridFill()
        {
            try
            {
                DesignationSP spDesignation = new DesignationSP();
                dgvDesignation.DataSource = spDesignation.DesignationViewForGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D3:" + ex.Message;
            }

        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        private void Clear()
        {
            try
            {
                txtAdvanceAmount.Text = string.Empty;
                txtCLInMonth.Text = string.Empty;
                txtDesignationName.Text = string.Empty;
                txtNarration.Text = string.Empty;
                txtSearch.Text = string.Empty;
                btnSave.Text = "Save";
                txtDesignationName.Enabled = true;
                btnDelete.Enabled = false;
                txtDesignationName.Focus();
                dgvDesignation.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D4:" + ex.Message;
            }

        }
        /// <summary>
        /// Function to fill Datagridview on Search
        /// </summary>
        private void Search()
        {
            try
            {
                DesignationSP spDesignation = new DesignationSP();
                dgvDesignation.DataSource = spDesignation.DesignationSearch(txtSearch.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill controls fpor update
        /// </summary>
        private void FillControls()
        {
            try
            {
                DesignationInfo infoDesignaion = new DesignationInfo();
                DesignationSP spDesignation = new DesignationSP();
                infoDesignaion = spDesignation.DesignationView(decDesignationId);
                txtDesignationName.Text = infoDesignaion.DesignationName;
                txtCLInMonth.Text = infoDesignaion.LeaveDays.ToString();
                txtAdvanceAmount.Text = infoDesignaion.AdvanceAmount.ToString();
                txtNarration.Text = infoDesignaion.Narration;
                btnSave.Text = "Update";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check existence of Designation
        /// </summary>
        /// <param name="strDesignation"></param>
        /// <param name="decDesignationId"></param>
        /// <returns></returns>
        public bool CheckExistanceOfDesignation(string strDesignation, decimal decDesignationId)
        {
            try
            {
                bool isExist = false;
                DesignationSP spDesignation = new DesignationSP();
                isExist = spDesignation.DesignationCheckExistanceOfName(strDesignation, decDesignationId);
                return isExist;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D7:" + ex.Message;
            }
            return false;
        }
        /// <summary>
        /// Function for save
        /// </summary>
        private void SaveFunction()
        {
            try
            {
                DesignationInfo infoDesignation = new DesignationInfo();
                DesignationSP spDesignation = new DesignationSP();
                if (txtAdvanceAmount.Text == string.Empty)
                {
                    infoDesignation.AdvanceAmount = 0;
                }
                else
                {
                    infoDesignation.AdvanceAmount = Convert.ToDecimal(txtAdvanceAmount.Text.Trim());
                }
                infoDesignation.DesignationName = txtDesignationName.Text.Trim();
                if (txtCLInMonth.Text == string.Empty)
                {
                    infoDesignation.LeaveDays = 0;
                }
                else
                {
                    infoDesignation.LeaveDays = Convert.ToDecimal(txtCLInMonth.Text.Trim());
                }
                decimal decD = infoDesignation.LeaveDays;
                infoDesignation.Narration = txtNarration.Text.Trim();
                infoDesignation.Extra1 = string.Empty;
                infoDesignation.Extra2 = string.Empty;
                infoDesignation.ExtraDate = DateTime.Now;
                if (CheckExistanceOfDesignation(txtDesignationName.Text.Trim(), 0) == false)
                {
                    decIdentity = spDesignation.DesignationAddWithReturnIdentity(infoDesignation);
                    if (decIdentity > 0)
                    {
                        Messages.SavedMessage();
                    }

                }
                else
                {
                    Messages.InformationMessage("Designation already exist");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Edit
        /// </summary>
        private void EditFunction()
        {
            try
            {
                DesignationInfo infoDesignation = new DesignationInfo();
                DesignationSP spDesignation = new DesignationSP();
                if (txtAdvanceAmount.Text == string.Empty)
                {
                    infoDesignation.AdvanceAmount = 0;
                }
                else
                {
                    infoDesignation.AdvanceAmount = Convert.ToDecimal(txtAdvanceAmount.Text.Trim());
                }
                infoDesignation.DesignationName = txtDesignationName.Text.TrimEnd();
                if (txtCLInMonth.Text == string.Empty)
                {
                    infoDesignation.LeaveDays = 0;
                }
                else
                {
                    infoDesignation.LeaveDays = Convert.ToDecimal(txtCLInMonth.Text.Trim());
                }
                infoDesignation.Narration = txtNarration.Text.Trim();
                infoDesignation.Extra1 = string.Empty;
                infoDesignation.Extra2 = string.Empty;
                infoDesignation.ExtraDate = DateTime.Now;
                if (CheckExistanceOfDesignation(txtDesignationName.Text.Trim(), decDesignationId) == false)
                {

                    infoDesignation.DesignationId = decDesignationId;
                    if (spDesignation.DesignationEdit(infoDesignation))
                    {
                        Messages.UpdatedMessage();
                    }
                }
                else
                {
                    Messages.InformationMessage("Designation already exist");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmEmployeeCreation form to create new Employee
        /// </summary>
        /// <param name="frmEmployee"></param>
        public void CallFromEmployee(frmEmployeeCreation frmEmployee)
        {
            try
            {
                dgvDesignation.Enabled = false;
                this.frmEmployeeObj = frmEmployee;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D10:" + ex.Message;
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// On 'Save' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(decUserId, strFormName, btnSave.Text))
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
                formMDI.infoError.ErrorString = "D11:" + ex.Message;
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
                if (CheckUserPrivilege.PrivilegeCheck(decUserId, strFormName, "Delete"))
                {
                    Delete();
                    GridFill();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D12:" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDesignation_Load(object sender, EventArgs e)
        {
            try
            {
                GridFill();
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D13:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'close' button click
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
                formMDI.infoError.ErrorString = "D14:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCLInMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D15:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdvanceAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D16:" + ex.Message;
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
                formMDI.infoError.ErrorString = "D17:" + ex.Message;
            }
        }
        /// <summary>
        /// Clears selection on Databind
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDesignation_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvDesignation.ClearSelection();
                dgvDesignation.CurrentCell = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D18:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview on txtSearch textbox TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D19:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills controls on cell double click in Datagridview for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDesignation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvDesignation.CurrentRow != null)
                {
                    if (dgvDesignation.CurrentRow.Cells["dgvtxtDesignationId"].Value != null)
                    {
                        if (dgvDesignation.CurrentRow.Cells["dgvtxtDesignationId"].Value.ToString() != string.Empty)
                        {

                            decDesignationId = decimal.Parse(dgvDesignation.CurrentRow.Cells["dgvtxtDesignationId"].Value.ToString());
                            strDesignation = dgvDesignation.CurrentRow.Cells["dgvtxtDesignationName"].Value.ToString();
                            btnDelete.Enabled = true;
                            FillControls();
                            if (Convert.ToDecimal(dgvDesignation.CurrentRow.Cells["dgvtxtDesignationId"].Value) != 1)
                            {
                                txtDesignationName.Enabled = true;
                            }
                            else
                            {
                                txtDesignationName.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D20:" + ex.Message;
            }
        }
        /// <summary>
        /// Checks on enter key 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdvanceAmount_Enter(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (Convert.ToDecimal(txtAdvanceAmount.Text) == 0)
                        txtAdvanceAmount.Text = string.Empty;
                }
                catch
                {
                    txtAdvanceAmount.Text = string.Empty;
                }
            }
            catch (Exception ex) 
            {
                formMDI.infoError.ErrorString = "D 34:" + ex.Message;
            }
        }
        /// <summary>
        /// Checks on leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdvanceAmount_Leave(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Convert.ToDecimal(txtAdvanceAmount.Text);
                }
                catch
                {
                    txtAdvanceAmount.Text = "0";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D 35:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills control while closing the form when used in other form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDesignation_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmEmployeeObj != null)
                {
                    frmEmployeeObj.ReturnFromDesignationForm(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D21:" + ex.Message;
            }
        }
        #endregion

        #region Navigation

        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDesignation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }

                if (btnDelete.Enabled == true)
                {
                    if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDesignationName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCLInMonth.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCLInMonth_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtAdvanceAmount.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtCLInMonth.Text == "" || txtCLInMonth.SelectionStart == 0)
                    {
                        txtDesignationName.Focus();
                        txtDesignationName.SelectionStart = 0;
                        txtDesignationName.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAdvanceAmount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtAdvanceAmount.Text == "" || txtAdvanceAmount.SelectionStart == 0)
                    {
                        txtCLInMonth.Focus();
                        txtCLInMonth.SelectionStart = 0;
                        txtCLInMonth.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D25:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                    if (e.KeyCode == Keys.Back)
                    {
                        if (txtNarration.Text == "" || txtNarration.SelectionStart == 0)
                        {
                            txtAdvanceAmount.Focus();
                            txtAdvanceAmount.SelectionStart = 0;
                            txtAdvanceAmount.SelectionLength = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D26:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
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
                formMDI.infoError.ErrorString = "D27:" + ex.Message;
            }
        }
        /// <summary>
        /// Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text.Trim();
                if (txtNarration.Text == "")
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
                formMDI.infoError.ErrorString = "D28:" + ex.Message;
            }

        }
        /// <summary>
        /// Fills controls on for updation on Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDesignation_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvDesignation.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvDesignation.CurrentCell.ColumnIndex, dgvDesignation.CurrentCell.RowIndex);
                        dgvDesignation_CellDoubleClick(sender, ex);
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D29:" + ex.Message;
            }
        }
       /// <summary>
       /// Backspace navigation
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                    if (btnDelete.Enabled)
                        btnDelete.Focus();
                    else
                        btnClear.Focus();
            }

            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D30:" + ex.Message;
            }
        }
        /// <summary>
        ///  Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDesignation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                txtSearch.Focus();
                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D31:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvDesignation.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D32:" + ex.Message;
            }
        }
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDesignation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 27)
                {
                    if (PublicVariables.isMessageClose == true)
                    {
                        Messages.CloseMessage(this);

                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "D33:" + ex.Message;
            }

        }
        #endregion
    }
}
