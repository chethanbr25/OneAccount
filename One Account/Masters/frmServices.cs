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
    public partial class frmServices : Form
    {

        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decIdForOtherForms = 0;
        decimal decServiceId = 0;
        decimal inNarrationCount = 0;
        string strServiceName = string.Empty;
        string strCategory;
        frmServiceVoucher frmServiceVoucherObj;

        #endregion

        #region Functions
        /// <summary>
        /// Create an instance for frmServices Class
        /// </summary>
        public frmServices()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Category ComboFill Function
        /// </summary>
        public void CategoryComboFill()
        {
            try
            {
                ServiceCategorySP spServiceCatogery = new ServiceCategorySP();
                DataTable dtblServiceCatogery = new DataTable();
                dtblServiceCatogery = spServiceCatogery.ServiceCategoryViewAll();
                cmbCategory.DataSource = dtblServiceCatogery;
                cmbCategory.ValueMember = "serviceCategoryId";
                cmbCategory.DisplayMember = "categoryName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser1:" + ex.Message;
                
            }
        }
        /// <summary>
        /// Its a call from Serviece voucher to create a new service
        /// </summary>
        /// <param name="frmServiceVoucher"></param>
        public void CallFromServiceVoucher(frmServiceVoucher frmServiceVoucher)
        {
            try
            {
                this.frmServiceVoucherObj = frmServiceVoucher;
                frmServiceVoucherObj.Enabled = false;
                base.Show();
                grpServiecesSearch.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser2:" + ex.Message;
            }
        }
        /// <summary>
        /// Category Search combo fill
        /// </summary>
        public void CategorySearchFill()
        {
            try
            {
                ServiceCategorySP spServiceCatogery = new ServiceCategorySP();
                DataTable dtblServiceCatogery = new DataTable();
                dtblServiceCatogery = spServiceCatogery.ServiceCategoryViewAll();
                DataRow dr = dtblServiceCatogery.NewRow();
                dr[1] = "All";
                dtblServiceCatogery.Rows.InsertAt(dr, 0);
                cmbCategorySearch.DataSource = dtblServiceCatogery;
                cmbCategorySearch.ValueMember = "serviceCategoryId";
                cmbCategorySearch.DisplayMember = "categoryName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser3:" + ex.Message;
            }
        }
        /// <summary>
        /// Gridfill function
        /// </summary>
        public void GridFill()
        {
            try
            {
                ServiceSP spService = new ServiceSP();
                DataTable dtblService = new DataTable();
                dtblService = spService.ServiceGridFill();
                dgvService.DataSource = dtblService;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser4:" + ex.Message;
            }
        }
        /// <summary>
        /// To reset the form here
        /// </summary>
        public void Clear()
        {
            try
            {
                CategoryComboFill();
                CategorySearchFill();
                txtServiceName.Clear();
                cmbCategory.SelectedIndex = -1;
                txtRate.Clear();
                txtNarration.Clear();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                txtServiceName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser5:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear the grid controlls here
        /// </summary>
        public void GridClear()
        {
            try
            {
                txtServiceNameSearch.Clear();
                cmbCategorySearch.SelectedIndex = 0;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser6:" + ex.Message;
            }
        }
        /// <summary>
        /// Call Delete function and checking conformation
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
                formMDI.infoError.ErrorString = "Ser7:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete function and reference exists checking
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                ServiceSP spService = new ServiceSP();
                if (spService.ServiceDeleteReferenceCheck(decServiceId) == -1)
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
                formMDI.infoError.ErrorString = "Ser8:" + ex.Message;
            }
        }
        /// <summary>
        /// Service Search function
        /// </summary>
        /// <param name="strBrandName"></param>
        /// <param name="strCategoryname"></param>
        public void ServiceSearch(string strBrandName, string strCategoryname)
        {
            try
            {
                ServiceSP spService = new ServiceSP();
                ServiceInfo infoService = new ServiceInfo();
                DataTable dtblService = new DataTable();
                dtblService = spService.ServiceSearch(strBrandName, strCategoryname);
                dgvService.DataSource = dtblService;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser9:" + ex.Message;
            }
        }
        /// <summary>
        /// ServiceName Checking Existance
        /// </summary>
        /// <returns></returns>
        public bool CheckExistenceOfServiceName()
        {
            bool isExist = false;

            try
            {
                ServiceSP spService = new ServiceSP();
                isExist = spService.ServiceCheckExistence(txtServiceName.Text.Trim(), 0);
                if (isExist)
                {
                    string strServiceNames = txtServiceName.Text.Trim();
                    if (strServiceNames.ToLower() == strServiceName.ToLower())
                    {
                        isExist = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser10:" + ex.Message;
            }
            return isExist;
        }
        /// <summary>
        /// Save Function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                ServiceSP spService = new ServiceSP();
                ServiceInfo infoService = new ServiceInfo();
                infoService.ServiceName = txtServiceName.Text.Trim();
                infoService.ServiceCategoryId = Convert.ToDecimal(cmbCategory.SelectedValue.ToString());
                infoService.Rate = Convert.ToDecimal(txtRate.Text.ToString());
                infoService.Narration = txtNarration.Text.Trim();
                infoService.ExtraDate = PublicVariables._dtCurrentDate;
                infoService.Extra1 = string.Empty;
                infoService.Extra2 = string.Empty;
                if (spService.ServiceCheckExistence(txtServiceName.Text.Trim(), 0) == false)
                {
                    decIdForOtherForms = spService.ServiceAddWithReturnIdentity(infoService);
                    Messages.SavedMessage();
                    Clear();
                    GridFill();
                }
                else
                {
                    Messages.InformationMessage("Service name already exist");
                    txtServiceName.Focus();
                }
                if (frmServiceVoucherObj != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser11:" + ex.Message;
            }
        }
        /// <summary>
        /// Edit Function
        /// </summary>
        public void EditFunction()
        {
            try
            {
                ServiceSP spService = new ServiceSP();
                ServiceInfo infoService = new ServiceInfo();
                infoService.ServiceId = Convert.ToDecimal(dgvService.CurrentRow.Cells["dgvtxtServiceId"].Value.ToString());
                infoService.ServiceName = txtServiceName.Text.Trim();
                infoService.ServiceCategoryId = Convert.ToDecimal(cmbCategory.SelectedValue.ToString());
                infoService.Rate = Convert.ToDecimal(txtRate.Text.ToString());
                infoService.Narration = txtNarration.Text.Trim();
                infoService.Extra1 = string.Empty;
                infoService.Extra2 = string.Empty;
                if (CheckExistenceOfServiceName() == false)
                {
                    if (spService.ServiceEdit(infoService))
                    {
                        Messages.UpdatedMessage();
                        Clear();
                        txtServiceName.Focus();
                    }
                }
                else
                {
                    Messages.InformationMessage("Already exists");
                    txtServiceName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser12:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking invalid entries For save or Update function
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                if (txtServiceName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter service name");
                    txtServiceName.Focus();
                }
                else if (cmbCategory.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select category name");
                    cmbCategory.Focus();
                }
                else if (txtRate.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter rate");
                    txtRate.Focus();
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
                formMDI.infoError.ErrorString = "Ser13:" + ex.Message;
            }
        }
        /// <summary>
        /// Return function from serviece category form
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromServiceCategoryForm(decimal decId)
        {
            try
            {
                CategoryComboFill();
                if (decId!=0)
                {
                    cmbCategory.SelectedValue = decId;
                }
                else if (strCategory != string.Empty)
                {
                    cmbCategory.SelectedValue = strCategory;
                }
                else
                {
                    cmbCategory.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbCategory.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser14:" + ex.Message;
            }
        }


        #endregion

        #region Events
        /// <summary>
        /// When form loads Clear the grid function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmServices_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
                GridClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser15:" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click
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
                    GridClear();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser16:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill controls for Update or delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvService_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    ServiceSP spService = new ServiceSP();
                    ServiceInfo infoService = new ServiceInfo();
                    decServiceId = Convert.ToDecimal(dgvService.Rows[e.RowIndex].Cells["dgvtxtServiceId"].Value.ToString());
                    infoService = spService.ServiceView(decServiceId);
                    txtServiceName.Text = infoService.ServiceName;
                    cmbCategory.SelectedValue = infoService.ServiceCategoryId.ToString();
                    txtRate.Text = infoService.Rate.ToString();
                    txtNarration.Text = infoService.Narration;
                    txtServiceNameSearch.Text = string.Empty;
                    cmbCategorySearch.SelectedIndex = 0;
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                    strServiceName = infoService.ServiceName;
                    txtServiceName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser17:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear button click
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
                formMDI.infoError.ErrorString = "Ser18:" + ex.Message;
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
                formMDI.infoError.ErrorString = "Ser19:" + ex.Message;
            }
        }
        /// <summary>
        /// Clear the grid function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                GridClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser20:" + ex.Message;
            }
        }
        /// <summary>
        /// Button delete click , call delete function and user privilage check
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
                formMDI.infoError.ErrorString = "Ser21:" + ex.Message;

            }
        }
        /// <summary>
        /// Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceSearch(txtServiceNameSearch.Text.Trim(), cmbCategorySearch.Text);
                txtServiceNameSearch.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser22:" + ex.Message;
            }
        }
        /// <summary>
        /// Serviece grid KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvService_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvService.CurrentRow != null)
                    {
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(dgvService.CurrentCell.ColumnIndex, dgvService.CurrentCell.RowIndex);
                        dgvService_CellDoubleClick(sender, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser23:" + ex.Message;
            }
        }
        /// <summary>
        /// Category leave function for set the textbox as clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedIndex == -1)
                {
                    cmbCategory.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser24:" + ex.Message;
            }
        }

        /// <summary>
        /// Form closing event . checking for other forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmServices_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmServiceVoucherObj != null)
                {
                    frmServiceVoucherObj.ReturnFromServiceFormForGridCombo(decIdForOtherForms);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser25:" + ex.Message;
            }
        }
        /// <summary>
        /// Add new serviece from here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedValue != null)
                {
                    strCategory = cmbCategory.SelectedValue.ToString();
                }
                else
                {
                    strCategory = string.Empty;
                }
                frmServiceCategory frmServiceCategory = new frmServiceCategory();
                frmServiceCategory.MdiParent = formMDI.MDIObj;
                frmServiceCategory open = Application.OpenForms["frmServiceCategory"] as frmServiceCategory;
                if (open == null)
                {
                    frmServiceCategory.WindowState = FormWindowState.Normal;
                    frmServiceCategory.MdiParent = formMDI.MDIObj;
                    frmServiceCategory.callFromServices(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.callFromServices(this);
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
                formMDI.infoError.ErrorString = "Ser26:" + ex.Message;
            }

        }

        #endregion

        #region Navigation
        /// <summary>
        /// For enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCategory.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser27:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key  and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = txtNarration.TextLength;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtRate.Text == string.Empty || txtRate.SelectionStart == 0)
                    {
                        cmbCategory.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser28:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key  and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtRate.Focus();
                    txtRate.SelectionStart = txtRate.SelectionLength;
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtServiceName.Focus();
                    txtServiceName.SelectionStart = 0;
                    txtServiceName.SelectionLength = 0;
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnGroupAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser29:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key  and backspace navigation
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
                        txtRate.Focus();
                        txtRate.SelectionStart = 0;
                        txtRate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser30:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key  and backspace navigation
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
                formMDI.infoError.ErrorString = "Ser31:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key  and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtServiceNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCategorySearch.Focus();
                    cmbCategorySearch.SelectionStart = cmbCategorySearch.SelectionLength;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser32:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key  and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategorySearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtServiceNameSearch.Focus();
                }
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser33:" + ex.Message;
            }
        }

        /// <summary>
        /// Form keydown for Quick access save and delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmServices_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) 
                {
                    if (cmbCategory.Focused)
                    {
                        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    else
                    {
                        cmbCategory.DropDownStyle = ComboBoxStyle.DropDown;

                    }
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
                formMDI.infoError.ErrorString = "Ser34:" + ex.Message;
            }
        }
        /// <summary>
        /// To get count For enter key  and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_Enter(object sender, EventArgs e)
        {
            try
            {
                inNarrationCount = 0;
                txtNarration.Text = txtNarration.Text;
                if (txtNarration.Text == string.Empty)
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
                formMDI.infoError.ErrorString = "Ser35:" + ex.Message;
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
                formMDI.infoError.ErrorString = "Ser36:" + ex.Message;
            }
        }
        /// <summary>
        /// Call decimal validation class here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser37:" + ex.Message;
            }
        }
        /// <summary>
        /// For backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvService_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbCategorySearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser38:" + ex.Message;
            }
        }
        /// <summary>
        /// For Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbCategorySearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "Ser39:" + ex.Message; ;
            }
        }
        #endregion




    }
}
