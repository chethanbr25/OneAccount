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
    public partial class frmArea : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variables declaration part
        /// </summary>
        int inNarrationCount = 0;
        int dgvcell = 0;
        frmRoute frmRouteObj;
        frmCustomer frmCustomerobj;
        frmSupplier frmSupplierobj;
        decimal decAreaId;
        decimal decIdForOtherForms = 0;
        #endregion
        #region Functions
        /// <summary>
        /// creates instance of frmArea class
        /// </summary>
        public frmArea()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill area datagridview
        /// </summary>
        public void AreaGridfill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                AreaSP spArea = new AreaSP();
                dtbl = spArea.AreaOnlyViewAll();
                dgvArea.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR1:" + ex.Message;
               
            }
        }
        /// <summary>
        /// creating a default route under the creating area
        /// </summary>
        public void RouteNACreateUnderTheArea()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                RouteInfo infoRoute = new RouteInfo();
                infoRoute.RouteName = "NA";
                infoRoute.AreaId = decAreaId;
                infoRoute.Narration = txtNarration.Text.Trim();
                infoRoute.Extra1 = string.Empty;
                infoRoute.Extra2 = string.Empty;
                infoRoute.ExtraDate = DateTime.Today;
                spRoute.RouteAdd(infoRoute);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR2:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the invalid entries fro save or update
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtAreaName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter area name");
                    txtAreaName.Focus();
                }
                else
                {
                    AreaInfo infoArea = new AreaInfo();
                    AreaSP spArea = new AreaSP();
                    infoArea.AreaName = txtAreaName.Text.Trim();
                    infoArea.Narration = txtNarration.Text.Trim();
                    infoArea.Extra1 = string.Empty;
                    infoArea.Extra2 = string.Empty;
                    infoArea.AreaId = decAreaId;
                    if (btnSave.Text == "Save")
                    {
                        if (Messages.SaveConfirmation())
                        {
                            if (spArea.AreaNameCheckExistence(txtAreaName.Text.Trim(), 0) == false)
                            {
                                decAreaId = spArea.AreaAddWithIdentity(infoArea);
                                //RouteNACreateUnderTheArea();
                                AreaGridfill();
                                Messages.SavedMessage();
                                Clear();
                                decIdForOtherForms = decAreaId;
                                if (frmRouteObj != null)
                                {
                                    this.Close();
                                }
                                if (frmCustomerobj != null)
                                {
                                    this.Close();
                                }
                                if (frmSupplierobj != null)
                                {
                                    this.Close();
                                }
                            }
                            else
                            {
                                Messages.InformationMessage("Area name already exist");
                                txtAreaName.Focus();
                            }
                        }
                    }
                    else
                    {
                        if (Messages.UpdateConfirmation())
                        {
                            if (spArea.AreaNameCheckExistence(txtAreaName.Text.Trim(), decAreaId) == false)
                            {
                                infoArea.AreaId = decAreaId;
                                if (spArea.AreaUpdate(infoArea))
                                {
                                    AreaGridfill();
                                    Messages.UpdatedMessage();
                                    Clear();
                                }
                                //else if (infoArea.AreaId == 1)
                                //{
                                //    Messages.WarningMessage("Cannot Update or Delete");
                                //    Clear();
                                //    txtAreaName.Focus();
                                //}
                            }
                            else
                            {
                                Messages.InformationMessage("Area name already exist");
                                txtAreaName.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR3:" + ex.Message;
            }
        }
        /// <summary>
        /// delete function
        /// </summary>
        public void Delete()
        {
            try
            {
                if (Messages.DeleteConfirmation())
                {
                    AreaSP spArea = new AreaSP();
                    if (spArea.AreaDeleteReference(decAreaId) <= 0)
                    {
                        Messages.ReferenceExistsMessage();
                    }
                    else
                    {
                        Messages.DeletedMessage();
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear fields
        /// </summary>
        public void Clear()
        {
            try
            {
                txtAreaName.Text = string.Empty;
                txtNarration.Text = string.Empty;
                txtAreaName.Focus();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                AreaGridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmRoute to add new area
        /// </summary>
        /// <param name="frmRoute"></param>
        public void CallFromRoute(frmRoute frmRoute)
        {
            try
            {
                this.frmRouteObj = frmRoute;
                dgvArea.Enabled = false;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmCustomer to add new area
        /// </summary>
        /// <param name="frmCustomer"></param>
        public void CallFromCustomer(frmCustomer frmCustomer)
        {
            try
            {
                dgvArea.Enabled = false;
                btnDelete.Enabled = false;
                this.frmCustomerobj = frmCustomer;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmSupplier to add new area
        /// </summary>
        /// <param name="frmSupplier"></param>
        public void CallFromSupplier(frmSupplier frmSupplier)
        {
            try
            {
                dgvArea.Enabled = false;
                this.frmSupplierobj = frmSupplier;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR8:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Messages.CloseConfirmation())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR9:" + ex.Message;
            }
        }
        /// <summary>
        /// form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmArea_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR10:" + ex.Message;
            }
        }
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
                formMDI.infoError.ErrorString = "AR11:" + ex.Message;
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
                formMDI.infoError.ErrorString = "AR12:" + ex.Message;
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
                formMDI.infoError.ErrorString = "AR13:" + ex.Message;
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
                if (Messages.CloseConfirmation())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR14:" + ex.Message;
            }
        }
        /// <summary>
        /// when form closing check the other forms is opend then pass the created area's id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmArea_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmCustomerobj != null)
                {
                    frmCustomerobj.ReturnFromAreaForm(decIdForOtherForms);
                }
                if (frmRouteObj != null)
                {
                    frmRouteObj.ReturnFromAreaForm(decIdForOtherForms);
                    frmRouteObj.Enabled = true;
                }
                if (frmSupplierobj != null)
                {
                    frmSupplierobj.ReturnFromAreaForm(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR15:" + ex.Message;
            }
        }
        /// <summary>
        /// area grid cell double click for update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvArea_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    
                    string strArea = dgvArea.CurrentRow.Cells["dgvtxtarea"].Value.ToString();
                    if (strArea != "NA")
                    {
                        AreaInfo infoArea = new AreaInfo();
                        AreaSP spArea = new AreaSP();
                        infoArea = spArea.AreaFill(Convert.ToDecimal(dgvArea.CurrentRow.Cells[1].Value.ToString()));
                        txtAreaName.Text = infoArea.AreaName;
                        txtNarration.Text = infoArea.Narration;
                        btnSave.Text = "Update";
                        btnDelete.Enabled = true;
                        txtAreaName.Focus();
                        decAreaId = Convert.ToDecimal(dgvArea.CurrentRow.Cells[1].Value.ToString());
                    }
                    else
                    {
                        Messages.WarningMessage("NA Area cannot update or delete");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR16:" + ex.Message;
            }
        }
        /// <summary>
        /// claere selection once completed the grids data binding completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvArea_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvArea.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR17:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// for enter key navigation
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
                formMDI.infoError.ErrorString = "AR18:" + ex.Message;
            }
        }
        /// <summary>
        /// for enter and backspace navigation
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
                formMDI.infoError.ErrorString = "AR19:" + ex.Message;
            }
        }
        /// <summary>
        /// enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAreaName_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "AR20:" + ex.Message;
            }
        }
        /// <summary>
        /// backspace navigation
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
                        txtAreaName.Focus();
                        txtAreaName.SelectionStart = 0;
                        txtAreaName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR21:" + ex.Message;
            }
        }
        /// <summary>
        /// form area keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmArea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    if (btnDelete.Enabled == true)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR22:" + ex.Message;
            }
        }
        /// <summary>
        /// grid keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvArea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvArea.CurrentCell == dgvArea[dgvArea.Columns.Count - 1, dgvArea.Rows.Count - 1])
                    {
                        if (dgvcell == 1)
                        {
                            dgvcell = 0;
                            btnClose.Focus();
                            dgvArea.ClearSelection();
                            e.Handled = true;
                        }
                        else
                        {
                            dgvcell++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR23:" + ex.Message;
            }
        }
        /// <summary>
        /// grid area keyup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvArea_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    if (dgvArea.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvArea.CurrentCell.ColumnIndex, dgvArea.CurrentCell.RowIndex);
                        dgvArea_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "AR24:" + ex.Message;
            }
        }
        #endregion
    }
}
