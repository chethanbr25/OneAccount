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
    public partial class frmRoute : Form
    {
       

        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decRoute; // for storing the scope identity of route
        decimal decRouteId;
        int inNarrationCount = 0;
        frmCustomer frmCustomerobj;
        frmSupplier frmSupplierobj;
        string strAreaId;

        #endregion

        #region Function
        /// <summary>
        /// Create instance of frmRoute
        /// </summary>
        public frmRoute()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to fill the Area combobox
        /// </summary>
        public void AreaComboFill()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                DataTable dtbl = new DataTable();
                dtbl = spRoute.AreafillInRoute();
                cmbArea.DataSource = dtbl;
                cmbArea.ValueMember = "areaId";
                cmbArea.DisplayMember = "areaName";
                cmbArea.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT1" + ex.Message;
               
            }
        }

        /// <summary>
        /// Area combobox fill for Search
        /// </summary>
        public void AreaComboFillSearch()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                DataTable dtbl = new DataTable();
                dtbl = spRoute.AreafillInRoute();
                DataRow dr = dtbl.NewRow();
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbAreaSearch.DataSource = dtbl;
                cmbAreaSearch.ValueMember = "areaId";
                cmbAreaSearch.DisplayMember = "areaName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT2" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill all Routes in datagridview based on the search key
        /// </summary>
        public void GridFill()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                DataTable dtbl = new DataTable();
                dtbl = spRoute.RouteSearch(txtRouteNameSearch.Text.Trim(), cmbAreaSearch.Text.ToString());
                dgvRoute.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT3" + ex.Message;
            }
        }

        /// <summary>
        /// Function to save the new route
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                RouteInfo infoRoute = new RouteInfo();
                infoRoute.RouteName = txtRouteName.Text.Trim();
                infoRoute.AreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                infoRoute.Narration = txtNarration.Text.Trim();
                infoRoute.Extra1 = String.Empty;
                infoRoute.Extra2 = String.Empty;
                if (spRoute.RouteCheckExistence(txtRouteName.Text.Trim(), 0, Convert.ToDecimal(cmbArea.SelectedValue.ToString())) == false)
                {
                    decRoute = spRoute.RouteAddParticularFields(infoRoute);
                    {
                        Messages.SavedMessage();
                        Clear();
                        GridFill();
                        if (frmCustomerobj != null)
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    Messages.InformationMessage(" Route name already exist");
                    txtRouteName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT4" + ex.Message;
            }
        }

        /// <summary>
        /// Function to edit already existing route
        /// </summary>
        public void EditFunction()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                RouteInfo infoRoute = new RouteInfo();
                infoRoute.RouteName = txtRouteName.Text.Trim();
                infoRoute.AreaId = Convert.ToDecimal(cmbArea.SelectedValue.ToString());
                infoRoute.Narration = txtNarration.Text.Trim();
                infoRoute.Extra1 = String.Empty;
                infoRoute.Extra2 = String.Empty;
                infoRoute.RouteId = decRouteId;
                if (spRoute.RouteCheckExistence(txtRouteName.Text.Trim(), decRouteId, Convert.ToDecimal(cmbArea.SelectedValue.ToString())) == false)
                {
                    if (spRoute.RouteEditing(infoRoute))
                    {
                        Messages.UpdatedMessage();
                        GridFill();
                        Clear();
                    }
                    else if (infoRoute.RouteId == 1)
                    {
                        Messages.InformationMessage("Cannot update");
                        Clear();
                        txtRouteName.Focus();
                    }
                }
                else
                {
                    Messages.InformationMessage(" Route name already exist");
                    txtRouteName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT5" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call Save or Edit function after user confirmation and also checks the invalid entries
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtRouteName.Text.Trim() == String.Empty)
                {
                    Messages.InformationMessage("Enter route name");
                    txtRouteName.Focus();
                }
                else if (cmbArea.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage(" Select area");
                    cmbArea.Focus();
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


                                if (frmSupplierobj != null)
                                {
                                    this.Close();
                                }
                            }
                        }
                        else
                        {
                            SaveFunction();

                            if (frmSupplierobj != null)
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
                formMDI.infoError.ErrorString = "RT6" + ex.Message;
            }
        }

        /// <summary>
        /// Function to Delete the Route
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                RouteSP spRoute = new RouteSP();
                if (spRoute.RouteDeleting(decRouteId) == -1)
                {
                    Messages.ReferenceExistsMessage();
                }
                else
                {
                    Messages.DeletedMessage();
                    GridFill();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT7" + ex.Message;
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
                formMDI.infoError.ErrorString = "RT8" + ex.Message;
            }
        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                txtRouteName.Text = String.Empty;
                txtNarration.Text = String.Empty;
                cmbAreaSearch.Text = "All";
                //txtRouteNameSearch.Text = String.Empty;
                cmbArea.SelectedIndex = -1;
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                txtRouteName.Focus();
                //GridFill();

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT9" + ex.Message;
            }
        }

        /// <summary>
        /// Function to clear the Search fields
        /// </summary>
        public void SearchClear()
        {
            try
            {
                cmbAreaSearch.Text = "All";
                txtRouteNameSearch.Text = String.Empty;
                GridFill();
                txtRouteNameSearch.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT10" + ex.Message;
            }
        }

        /// <summary>
        /// function to fill the area combobox while returning from the Area form
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAreaForm(decimal decId)
        {
            try
            {
                AreaComboFill();
                if (decId.ToString() != "0")
                {
                    cmbArea.SelectedValue = decId;
                }
                else if (strAreaId != string.Empty)
                {
                    cmbArea.SelectedValue = strAreaId;
                }
                else
                {
                    cmbArea.SelectedIndex = -1;
                }
                this.Enabled = true;
                AreaComboFillSearch();
                cmbArea.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT11" + ex.Message;
            }
        }

       
        /// <summary>
        /// Function to load the form while calling from the customer form to add the new route
        /// </summary>
        /// <param name="frmCustomer"></param>
        public void CallFromCustomer(frmCustomer frmCustomer)
        {
            try
            {
                dgvRoute.Enabled = false;
                groupBox2.Enabled = false;
                btnAreaAdd.Enabled = false;
                btnAreaAdd.Visible = false;
                this.frmCustomerobj = frmCustomer;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT12" + ex.Message;
            }
        }
        
        /// <summary>
        /// Function to load the form while calling from the Supplier form to add the new route
        /// </summary>
        /// <param name="frmSupplier"></param>
        public void CallFromSupplier(frmSupplier frmSupplier)
        {
            try
            {

                groupBox2.Enabled = false;
                dgvRoute.Enabled = false;
                this.frmSupplierobj = frmSupplier;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT13" + ex.Message;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRoute_Load(object sender, EventArgs e)
        {
            try
            {
                txtRouteName.Focus();
                AreaComboFill();
                AreaComboFillSearch();
                btnDelete.Enabled = false;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT14" + ex.Message;
            }
        }

        /// <summary>
        /// On delete button click
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
                formMDI.infoError.ErrorString = "RT15" + ex.Message;
            }
        }

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
                formMDI.infoError.ErrorString = "RT16" + ex.Message;
            }
        }

        /// <summary>
        /// On Area "+" button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAreaAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbArea.SelectedValue != null)
                {
                    strAreaId = cmbArea.SelectedValue.ToString();
                }
                else
                {
                    strAreaId = string.Empty;
                }
                frmArea frmArea = new frmArea();

                frmArea open = Application.OpenForms["frmArea"] as frmArea;
                if (open == null)
                {
                    frmArea.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmArea.MdiParent = formMDI.MDIObj;
                    frmArea.CallFromRoute(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromRoute(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }

                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT17" + ex.Message;
            }
        }

        /// <summary>
        /// On double clicking on the datagridview, It displays the details of the rack to edit or delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRoute_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (dgvRoute.Rows[e.RowIndex].Cells["dgvtxtRouteName"].Value.ToString() != "NA")
                    {
                        RouteSP spRoute = new RouteSP();
                        RouteInfo infoRoute = new RouteInfo();
                        infoRoute = spRoute.RouteView(Convert.ToDecimal(dgvRoute.CurrentRow.Cells["dgvtxtRouteId"].Value.ToString()));
                        decRouteId = Convert.ToDecimal(dgvRoute.CurrentRow.Cells["dgvtxtRouteId"].Value.ToString());
                        txtRouteName.Text = infoRoute.RouteName;
                        cmbArea.SelectedValue = infoRoute.AreaId.ToString();
                        txtNarration.Text = infoRoute.Narration;
                        btnSave.Text = "Update";
                        btnDelete.Enabled = true;
                        txtRouteName.Focus();

                    }
                    else
                    {
                        Messages.InformationMessage("Default Route cannot update or delete");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT18" + ex.Message;
            }
        }

        /// <summary>
        /// On search button click
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
                formMDI.infoError.ErrorString = "RT19" + ex.Message;
            }
        }


        /// <summary>
        /// On clear button in search groupbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT20" + ex.Message;
            }
        }


        /// <summary>
        /// On close button click
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
                formMDI.infoError.ErrorString = "RT21" + ex.Message;
            }
        }

        /// <summary>
        /// On clear button click
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
                formMDI.infoError.ErrorString = "RT22" + ex.Message;
            }
        }

        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRoute_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (frmCustomerobj != null)  //Coded by Fahad
                {
                    frmCustomerobj.ReturnFromRouteForm(decRoute);
                }
                if (frmSupplierobj != null)  //Coded by Neiluffer Shama
                {

                    frmSupplierobj.ReturnFromRoutForm(decRoute);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT23" + ex.Message;
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// For the shortcut keys
        /// Escape for closing the form
        /// ctrl+s for Save
        /// ctrl+d for delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRoute_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save
                {
                    if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                    {

                        if (cmbArea.Focused)
                        {
                            cmbArea.DropDownStyle = ComboBoxStyle.DropDown;
                        }
                        else
                        {
                            cmbArea.DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                        btnSave_Click(sender, e);

                    }
                    else
                    {
                        Messages.NoPrivillageMessage();
                    }
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control) //Delete
                {
                    if (btnDelete.Enabled)
                    {
                        if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                        {

                            btnDelete_Click(sender, e);
                        }
                        else
                        {
                            Messages.NoPrivillageMessage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT24" + ex.Message;
            }
        }

        /// <summary>
        /// On enter key navigation of routename textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbArea.Focus();
                    cmbArea.SelectionLength = 0;
                    cmbArea.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT25" + ex.Message;
            }
        }

        /// <summary>
        /// On enter key and backspace navigation of area combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbArea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionLength = 0;
                    txtNarration.SelectionStart = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbArea.Text == string.Empty || cmbArea.SelectionStart == 0)
                    {
                        txtRouteName.Focus();
                        txtRouteName.SelectionStart = 0;
                        txtRouteName.SelectionLength = 0;
                    }
                }

                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnAreaAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT26" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key navigation of narration textbox
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
                 formMDI.infoError.ErrorString = "RT27" + ex.Message;
            }
        }


        /// <summary>
        /// backspace navigation of narration textbox
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
                        cmbArea.Focus();
                        cmbArea.SelectionStart = 0;
                        cmbArea.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "RT28" + ex.Message;
            }
        }

        /// <summary>
        /// backspace navigation of Save button
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
                  formMDI.infoError.ErrorString = "RT29" + ex.Message;
            }
        }

        /// <summary>
        /// backspace navigation of datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRoute_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbAreaSearch.Focus();
                    cmbAreaSearch.SelectionLength = 0;
                    cmbAreaSearch.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "RT30" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key navigation of txtRouteNameSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbAreaSearch.Focus();
                    cmbAreaSearch.SelectionStart = 0;
                    cmbAreaSearch.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "RT31" + ex.Message;
            }
        }

        /// <summary>
        /// Enter key and backspace navigation of cmbAreaSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAreaSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }

                if (e.KeyCode == Keys.Back)
                {
                    if (cmbAreaSearch.Text == string.Empty || cmbAreaSearch.SelectionStart == 0)
                    {
                        txtRouteNameSearch.Focus();
                        txtRouteNameSearch.SelectionStart = 0;
                        txtRouteNameSearch.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "RT32" + ex.Message;
            }
        }

        /// <summary>
        /// backspace navigation of btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbAreaSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RT33" + ex.Message;
            }
        }

        /// <summary>
        /// On enter key in grid, It performs celldoubleclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRoute_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvRoute.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvRoute.CurrentCell.ColumnIndex, dgvRoute.CurrentCell.RowIndex);
                        dgvRoute_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "RT34" + ex.Message;
            }
        }

        /// <summary>
        /// On clearbutton for Search clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try{
            if (e.KeyCode == Keys.Back)
            {
                btnSearch.Focus();
            }
            }
              catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "RT35" + ex.Message;
            }
        }

        #endregion
    }
}
