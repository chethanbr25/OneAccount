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
    public partial class frmRolePrivilegeSettings : Form
    {
       
        #region PublicVariable
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        bool isCombo = false;
        bool bl = true;
        #endregion
        #region Funtions
        /// <summary>
        /// Create an instance for frmRolePrivilegeSettings class
        /// </summary>
        public frmRolePrivilegeSettings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the Role Combobox
        /// </summary>
        public void RoleComboFill()
        {
            try
            {
                DataTable dtblRoleCombo = new DataTable();
                RoleSP spRole = new RoleSP();
                dtblRoleCombo = spRole.RoleViewAll();
                cmbRole.DataSource = dtblRoleCombo;
                cmbRole.ValueMember = "roleId";
                cmbRole.DisplayMember = "role";
                cmbRole.SelectedIndex = -1;
                isCombo = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:1" + ex.Message;
               
            }
        }
        /// <summary>
        /// Function to fill the controls
        /// </summary>
        public void FillControls()
        {
            try
            {
                PrivilegeSP spPrivillage = new PrivilegeSP();
                DataTable dtbl = new DataTable();
                DataTable dtblAction = new DataTable();
                decimal decRoleId = Convert.ToDecimal(cmbRole.SelectedValue.ToString());
                dtbl = spPrivillage.PrivilegeSettingsSearch(decRoleId);
                foreach (TabPage tabPage in tcPrivillage.Controls)
                {
                    if (tabPage.Text == "Company")
                    {
                        string strCompany = "frm" + tabPage.Text + "Creation";
                        foreach (DataRow dtCompany in dtbl.Rows)
                        {
                            foreach (Control cbxCompany in tabPage.Controls)
                            {
                                if (cbxCompany is CheckBox)
                                {
                                    CheckBox cbx = (CheckBox)cbxCompany;
                                    string strCbx = cbx.Text.Replace(" ", string.Empty);
                                    dtblAction = spPrivillage.PrivilegeActionSearch(strCompany, decRoleId);
                                    foreach (DataRow drAction in dtblAction.Rows)
                                    {
                                        if (drAction["formName"].ToString() == strCompany)
                                        {
                                            if (drAction["action"].ToString() == strCbx)
                                            {
                                                cbx.Checked = true;
                                            }
                                            else if (drAction["action"].ToString() == "Update")
                                            {
                                                if (cbx.Text =="Edit")
                                                {
                                                    cbx.Checked = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (tabPage.Text == "Register" || tabPage.Text == "Reports1" || tabPage.Text == "Reports2")
                    {
                        foreach (Control cbx1 in tabPage.Controls)
                        {
                            if (cbx1 is CheckBox)
                            {
                                CheckBox cbx2 = (CheckBox)cbx1;
                                string strFormName = "frm" + cbx2.Text.Replace(" ", string.Empty);
                                dtblAction = spPrivillage.PrivilegeActionSearch(strFormName, decRoleId);
                                foreach (DataRow drAction in dtblAction.Rows)
                                {
                                    if (drAction["formName"].ToString() == strFormName)
                                    {
                                        if (drAction["action"].ToString() == "View")
                                        {
                                            cbx2.Checked = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cbx1 is GroupBox)
                                {
                                    foreach (Control cbxInGbx in cbx1.Controls)
                                    {
                                        CheckBox cbx3 = (CheckBox)cbxInGbx;
                                        string strFormName = "frm" + cbx3.Text.Replace(" ", string.Empty);
                                        dtblAction = spPrivillage.PrivilegeActionSearch(strFormName, decRoleId);
                                        foreach (DataRow drFormName in dtblAction.Rows)
                                        {
                                            if (drFormName["formName"].ToString() == strFormName)
                                            {
                                                if (drFormName["action"].ToString() == "View")
                                                {
                                                    cbx3.Checked = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Control GbxControle in tabPage.Controls)
                        {
                            if (GbxControle is GroupBox)
                            {
                                foreach (Control CbxControls in GbxControle.Controls)
                                {
                                    if (CbxControls is CheckBox)
                                    {
                                        CheckBox cbx = (CheckBox)CbxControls;
                                        string strFormName = "frm" + GbxControle.Text.Replace(" ", string.Empty);
                                        if (GbxControle.Text == "Price List" || GbxControle.Text == "Standerd Rate")
                                        {
                                            string strPopName = strFormName + "Popup";
                                            dtblAction = spPrivillage.PrivilegeActionSearch(strPopName, decRoleId);
                                            foreach (DataRow dtaction in dtblAction.Rows)
                                            {
                                                if (dtaction["action"].ToString() == "Save")
                                                {
                                                    if (dtaction["formName"].ToString() == strPopName)
                                                    {
                                                        if (cbx.Text =="Add")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                                else if (dtaction["action"].ToString() == "Update")
                                                {
                                                    if (dtaction["formName"].ToString() == strPopName)
                                                    {
                                                        if (cbx.Text =="Edit")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                                else if (dtaction["action"].ToString() == "Delete")
                                                {
                                                    if (dtaction["formName"].ToString() == strPopName)
                                                    {
                                                        if (cbx.Text =="Delete")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                                else if (dtaction["action"].ToString() == "View")
                                                {
                                                    if (dtaction["formName"].ToString() == strPopName)
                                                    {
                                                        if (cbx.Text =="Register")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                        else if (dtaction["action"].ToString() == cbx.Text)
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                            
                                        else
                                        {
                                           
                                            dtblAction = spPrivillage.PrivilegeActionSearch(strFormName, decRoleId);
                                            foreach (DataRow dtaction in dtblAction.Rows)
                                            {
                                                if (dtaction["action"].ToString() == "Save")
                                                {
                                                    if (dtaction["formName"].ToString() == strFormName)
                                                    {
                                                        if (cbx.Text == "Add")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                                else if (dtaction["action"].ToString() == "Update")
                                                {
                                                    if (dtaction["formName"].ToString() == strFormName)
                                                    {
                                                        if (cbx.Text =="Edit")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                                else if (dtaction["action"].ToString() == "Delete")
                                                {
                                                    if (dtaction["formName"].ToString() == strFormName)
                                                    {
                                                        if (cbx.Text =="Delete")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                                else if (dtaction["action"].ToString() == "View")
                                                {
                                                    if (dtaction["formName"].ToString() == strFormName)
                                                    {
                                                        if (cbx.Text =="Register")
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                        else if (dtaction["action"].ToString() == cbx.Text)
                                                        {
                                                            cbx.Checked = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (GbxControle is CheckBox)
                                {
                                    CheckBox cbx = (CheckBox)GbxControle;
                                    string strAction = "frm" + GbxControle.Text.Replace(" ", string.Empty);
                                    dtblAction = spPrivillage.PrivilegeActionSearch(strAction, decRoleId);
                                    foreach (DataRow dtaction in dtblAction.Rows)
                                    {
                                        if (dtaction["formName"].ToString() == strAction)
                                        {
                                            if (dtaction["action"].ToString() == "View")
                                            {
                                                cbx.Checked = true;
                                            }
                                            else if (dtaction["action"].ToString() == "Change")
                                            {
                                                cbx.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Check the status of Check all checkbox status
        /// </summary>
        /// <param name="sender"></param>
        public void ToClickAll(object sender)
        {
            try
            {
                TabPage tabPage = tcPrivillage.SelectedTab;
                foreach (Control groupBox in tabPage.Controls)
                {
                    foreach (Control checkBox in groupBox.Controls)
                    {
                        if (checkBox is CheckBox)
                        {
                            CheckBox cbx = (CheckBox)checkBox;
                            if (cbx.Checked)
                            {
                                string strCbx = cbx.Text;
                                if (strCbx == "All")
                                {
                                    foreach (Control cb in groupBox.Controls)
                                    {
                                        CheckBox c = (CheckBox)cb;
                                        c.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:3" + ex.Message;
            }
        }
        /// <summary>
        /// Function To Tick All CheckBox
        /// </summary>
        /// <param name="sender"></param>
        public void ToTickAllCheckBox(object sender)
        {
            string strAdd = string.Empty;
            string strEdit = string.Empty;
            string strDelete = string.Empty;
            string strView = string.Empty;
            try
            {
                TabPage tabPage = tcPrivillage.SelectedTab;
                CheckBox cb = (CheckBox)sender;
                GroupBox gbx = (GroupBox)cb.Parent;
                foreach (Control cb2 in gbx.Controls)
                {
                    CheckBox cb1 = (CheckBox)cb2;
                    if (!cb1.Checked)
                    {
                        if (cb1.Text == "Add")
                        {
                            cb1.Checked = false;
                        }
                        else if (cb1.Text == "Edit")
                        {
                            cb1.Checked = false;
                        }
                        else if (cb1.Text == "Delete")
                        {
                            cb1.Checked = false;
                        }
                        else if (cb1.Text == "View")
                        {
                            cb1.Checked = false;
                        }
                    }
                    else if (cb1.Text == "Add")
                    {
                        cb1.Checked = true;
                        strAdd = cb1.Text;
                    }
                    else if (cb1.Text == "Edit")
                    {
                        cb1.Checked = true;
                        strEdit = cb1.Text;
                    }
                    else if (cb1.Text == "Delete")
                    {
                        cb1.Checked = true;
                        strDelete = cb1.Text;
                    }
                    else if (cb1.Text == "View")
                    {
                        cb1.Checked = true;
                        strView = cb1.Text;
                    }
                    else
                    {
                        if (cb1.Text == "All")
                        {
                            cb1.Checked = false;
                        }
                    }
                    if (strAdd != string.Empty && strEdit != string.Empty && strDelete != string.Empty)
                    {
                        if (cb1.Text == "All")
                        {
                            cb1.Checked = true;
                        }
                    }
                    else if (strAdd != string.Empty && strView != string.Empty)
                    {
                        if (cb1.Text == "All")
                        {
                            cb1.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:4" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Select all items in list
        /// </summary>
        public void SelectAll()
        {
            try
            {
                TabPage page = tcPrivillage.SelectedTab;
                foreach (Control groupBox in page.Controls)
                {
                    if (groupBox is GroupBox)
                    {
                        foreach (Control checkBox in groupBox.Controls)
                        {
                            if (checkBox is CheckBox)
                            {
                                CheckBox cbx = (CheckBox)checkBox;
                                string strCbx = cbx.Text;
                                if (!cbx.Checked)
                                {
                                    cbx.Checked = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Control checkBox in page.Controls)
                        {
                            if (checkBox is CheckBox)
                            {
                                CheckBox cbx = (CheckBox)checkBox;
                                string strCbx = cbx.Text;
                                if (!cbx.Checked)
                                {
                                    cbx.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:5" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to LinkLabel Click Clear All
        /// </summary>
        public void ClearAll()
        {
            try
            {
                TabPage page = tcPrivillage.SelectedTab;
                foreach (Control groupBox in page.Controls)
                {
                    if (groupBox is GroupBox)
                    {
                        foreach (Control checkBox in groupBox.Controls)
                        {
                            if (checkBox is CheckBox)
                            {
                                CheckBox cbx = (CheckBox)checkBox;
                                string strCbx = cbx.Text;
                                if (cbx.Checked)
                                {
                                    cbx.Checked = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Control checkBox in page.Controls)
                        {
                            if (checkBox is CheckBox)
                            {
                                CheckBox cbx = (CheckBox)checkBox;
                                string strCbx = cbx.Text;
                                if (cbx.Checked)
                                {
                                    cbx.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:6" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear All Tabpage
        /// </summary>
        public void AllTabpageClear()
        {
            try
            {
                tcPrivillage.SelectedIndex = 0;
                foreach (Control tabPage in tcPrivillage.Controls)
                {
                    foreach (Control groupBox in tabPage.Controls)
                    {
                        if (groupBox is GroupBox)
                        {
                            foreach (Control checkBox in groupBox.Controls)
                            {
                                if (checkBox is CheckBox)
                                {
                                    CheckBox cbx = (CheckBox)checkBox;
                                    string strCbx = cbx.Text;
                                    if (cbx.Checked)
                                    {
                                        cbx.Checked = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Control checkBox in tabPage.Controls)
                            {
                                if (checkBox is CheckBox)
                                {
                                    CheckBox cbx = (CheckBox)checkBox;
                                    string strCbx = cbx.Text;
                                    if (cbx.Checked)
                                    {
                                        cbx.Checked = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:7" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Delete an items
        /// </summary>
        public void DeleteFunction()
        {
            try
            {
                PrivilegeInfo infoPrivilege = new PrivilegeInfo();
                PrivilegeSP spPrivilege = new PrivilegeSP();
                decimal decRoleId = Convert.ToDecimal(cmbRole.SelectedValue);
                spPrivilege.PrivilegeDeleteTabel(decRoleId);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:8" + ex.Message;
            }
        }
        /// <summary>
        /// Save Function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                PrivilegeInfo infoPrivilege = new PrivilegeInfo();
                PrivilegeSP spPrivilege = new PrivilegeSP();
                infoPrivilege.RoleId = Convert.ToDecimal(cmbRole.SelectedValue);
                infoPrivilege.Extra1 = string.Empty;
                infoPrivilege.Extra2 = string.Empty;
                foreach (Control tabPage in tcPrivillage.Controls)
                {
                    string strTabPage = tabPage.Text;
                    if (strTabPage == "Company")
                    {
                        infoPrivilege.FormName = "frm" + strTabPage +"Creation";
                        foreach (Control cbxCompany in tabPage.Controls)
                        {
                            if (cbxCompany is CheckBox)
                            {
                                CheckBox cbx = (CheckBox)cbxCompany;
                                if (cbx.Checked == true)
                                {
                                    string strCbx = cbx.Text.Replace(" ",string.Empty);
                                    if (strCbx == "Edit")
                                    {
                                        infoPrivilege.Action = "Update";
                                        
                                 
                                    }
                                    else
                                    {
                                        infoPrivilege.Action = strCbx;
                                        
                                    
                                    }
                                    spPrivilege.PrivilegeAdd(infoPrivilege);
                                 
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Control gbxControl in tabPage.Controls)
                        {
                            if (gbxControl is GroupBox)
                            {
                                string strFormName = gbxControl.Text.Replace(" ", string.Empty);
                                foreach (Control cbxControl in gbxControl.Controls)
                                {
                                    CheckBox cbx = (CheckBox)cbxControl;
                                    if (cbx.Checked == true)
                                    {
                                        string strCbx = cbx.Text.Replace(" ", string.Empty);
                                        if (gbxControl.Text == "Transactions" || gbxControl.Text == "Payroll")
                                        {
                                            infoPrivilege.FormName = "frm" + strCbx;
                                            infoPrivilege.Action = "View";
                                            spPrivilege.PrivilegeAdd(infoPrivilege);
                                        }
                                            /*------------------------------------*/
                                        else
                                        {
                                            if (strCbx != "All" && strCbx != "Register")
                                            {
                                                if (gbxControl.Text == "Price List" || gbxControl.Text == "Standerd Rate")
                                                {
                                                    infoPrivilege.FormName = "frm" + strFormName + "Popup";
                                                    if (strCbx == "Add")
                                                    {
                                                        infoPrivilege.Action = "Save";
                                                    }
                                                    else if (strCbx == "Edit")
                                                    {
                                                        infoPrivilege.Action = "Update";
                                                    }
                                                    else if (strCbx == "Delete")
                                                    {
                                                        infoPrivilege.Action = "Delete";
                                                    }
                                                    spPrivilege.PrivilegeAdd(infoPrivilege);
                                                }
                                                   
                                                else
                                                {
                                                    infoPrivilege.FormName = "frm" + strFormName;
                                                    if (strCbx == "Add")
                                                    {
                                                        infoPrivilege.Action = "Save";
                                                    }
                                                    else if (strCbx == "Edit")
                                                    {
                                                        infoPrivilege.Action = "Update";
                                                    }
                                                    else if (strCbx == "Delete")
                                                    {
                                                        infoPrivilege.Action = "Delete";
                                                    }
                                                    else if (strCbx == "View")
                                                    {
                                                        infoPrivilege.Action = "View";
                                                    }
                                                    spPrivilege.PrivilegeAdd(infoPrivilege);
                                                }
                                            }
                                            
                                            else if (strCbx == "Register")
                                            {
                                                string strFormNameCreationRemove = "frm" + strFormName.Replace("Creation", string.Empty) + "Register";
                                                string strFormNameVoucherRemove = "frm" + strFormName.Replace("Voucher", string.Empty) + "Register";
                                                infoPrivilege.FormName = strFormNameCreationRemove;
                                                infoPrivilege.Action = "View";
                                                spPrivilege.PrivilegeAdd(infoPrivilege);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                               
                                if (gbxControl is CheckBox)
                                {
                                    CheckBox cbx = (CheckBox)gbxControl;
                                    if (cbx.Checked == true)
                                    {
                                        string strFormName = "frm" + gbxControl.Text.Replace(" ", string.Empty);
                                        infoPrivilege.FormName = strFormName;
                                        infoPrivilege.Action = "View";
                                        spPrivilege.PrivilegeAdd(infoPrivilege);
                                    }
                                }
                            }
                        }
                    }
                }
                AllTabpageClear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:9" + ex.Message;
            }
        }
        /// <summary>
        /// Save or edit function
        /// </summary>
        public void SaveOrEditFunction()
        {
            try
            {
                DataTable dtbl = new DataTable();
                PrivilegeSP spPrivillage = new PrivilegeSP();
                if (cmbRole.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Select role");
                    cmbRole.Focus();
                }
                else
                {
                    decimal decRoleId = Convert.ToDecimal(cmbRole.SelectedValue.ToString());
                    if (spPrivillage.RolePrivilegeSaveCheckExistence(decRoleId) == false)
                    {
                        if (PublicVariables.isMessageAdd)
                        {
                            if (Messages.SaveMessage())
                            {
                                SaveFunction();
                                Messages.SavedMessage();
                                cmbRole.SelectedIndex = -1;
                                if (cmbRole.SelectedIndex == 0)
                                {
                                    cmbRole.SelectedIndex = -1;
                                    AllTabpageClear();
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
                                DeleteFunction();
                                SaveFunction();
                                Messages.UpdatedMessage();
                                cmbRole.SelectedIndex = -1;
                                if (cmbRole.SelectedIndex == 0)
                                {
                                    cmbRole.SelectedIndex = -1;
                                    AllTabpageClear();
                                }
                            }
                        }
                        else
                        {
                            DeleteFunction();
                            SaveFunction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:10" + ex.Message;
            }
        }
        /// <summary>
        /// Register Unchecked is Edit Or Delete  UnChecked
        /// </summary>
        /// <param name="strCbx"></param>
        public void RegisterUnchecked(string strCbx)
        {
            try
            {
                foreach (Control tbPage in tcPrivillage.Controls)
                {
                    if (tbPage.Text == "Transactions1" || tbPage.Text == "Transactions2" || tbPage.Text == "Transactions3")
                    {
                        foreach (Control gbx in tbPage.Controls)
                        {
                            foreach (Control cbx in gbx.Controls)
                            {
                                if (cbx is CheckBox)
                                {
                                    CheckBox CbxTr = (CheckBox)cbx;
                                    if (CbxTr.Text == "Edit" || CbxTr.Text == "Delete")
                                    {
                                        if (CbxTr.Checked)
                                        {
                                            string strTr = gbx.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                                            if (strTr == strCbx)
                                            {
                                                CbxTr.Checked = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (tbPage.Text == "Payroll1" || tbPage.Text == "Payroll2")
                    {
                        foreach (Control gbxP in tbPage.Controls)
                        {
                            if (gbxP is GroupBox)
                            {
                                foreach (Control cbxP in gbxP.Controls)
                                {
                                    if (cbxP is CheckBox)
                                    {
                                        CheckBox cbxPayroll = (CheckBox)cbxP;
                                        if (cbxPayroll.Text == "Edit" || cbxPayroll.Text == "Delete")
                                        {
                                            if (cbxPayroll.Checked)
                                            {
                                                string strPayrollRegister = gbxP.Text.Replace("Voucher", string.Empty).Replace("Creation", string.Empty).Replace("Payment", string.Empty).Replace(" ", string.Empty);
                                                if (strPayrollRegister == strCbx)
                                                {
                                                    cbxPayroll.Checked = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:11" + ex.Message;
            }
        }
        /// <summary>
        /// Tick Edit Or Delete Register Tick
        /// </summary>
        /// <param name="strCbx"></param>
        public void RegisterTickEditOrDeleteTicked(string strCbx)
        {
            try
            {
                TabPage tabPage = tcPrivillage.SelectedTab;
                foreach (Control gbxTr in tabPage.Controls)
                {
                    if (gbxTr is GroupBox)
                    {
                        foreach (Control cbxTr in gbxTr.Controls)
                        {
                            if (cbxTr is CheckBox)
                            {
                                CheckBox cbxTr1 = (CheckBox)cbxTr;
                                if (cbxTr1.Text == "Edit" || cbxTr1.Text == "Delete")
                                {
                                    if (cbxTr1.Checked)
                                    {
                                        foreach (Control tabPageRt in tcPrivillage.Controls)
                                        {
                                            if (tabPageRt.Text == "Register")
                                            {
                                                foreach (Control cbxRt in tabPageRt.Controls)
                                                {
                                                    if (cbxRt is CheckBox)
                                                    {
                                                        CheckBox cbxRg = (CheckBox)cbxRt;
                                                        string strCbxRg = cbxRt.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                                                        if (strCbxRg == strCbx)
                                                        {
                                                            cbxRg.Checked = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (tabPage.Text == tabPageRt.Text)
                                            {
                                                foreach (Control cbx in tabPage.Controls)
                                                {
                                                    if (cbx is CheckBox)
                                                    {
                                                        CheckBox CbxReg = (CheckBox)cbx;
                                                        string strCbxRg = CbxReg.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                                                        if (strCbxRg == strCbx)
                                                        {
                                                             CbxReg.Checked = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:12" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// When form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRolePrivilegeSettings_Load(object sender, EventArgs e)
        {
            try
            {
                isCombo = false;
                RoleComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:13" + ex.Message;
            }
        }
        /// <summary>
        /// Combobox index change for clear the tab and fill the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isCombo)
                {
                    if (cmbRole.SelectedIndex > -1)
                    {
                        AllTabpageClear();
                        FillControls();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:14" + ex.Message;
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
                if (PublicVariables.isMessageClose == true)
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
                formMDI.infoError.ErrorString = "RLPRST:15" + ex.Message;
            }
        }
        /// <summary>
        /// Save button click, check the orivilage and call the save function
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
                formMDI.infoError.ErrorString = "RLPRST:16" + ex.Message;
            }
        }
        /// <summary>
        /// Reset button click, make the form as reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRole.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Select role");
                    cmbRole.Focus();
                }
                else
                {
                    AllTabpageClear();
                    FillControls();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:17" + ex.Message;
            }
        }
        
        /// <summary>
        /// To Select all the checkboxes under company tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblCompanySelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:18" + ex.Message;
            }
        }
        /// <summary>
        /// To deSelect all the checkboxes under company tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblCompanyClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:19" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Master tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblMaster1SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:20" + ex.Message;
            }
        }
        /// <summary>
        /// To deSelect all the checkboxes under company tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblMaster1ClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:21" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Customer tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCustomerAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:22" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a new customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCustomerAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:23" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to edit a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCustomerEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:24" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to delete a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCustomerDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:25" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Supplier tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSupplierAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:26" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to add a Supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSupplierAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:27" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to edit a Supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSupplierEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:28" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to delete a Supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSupplierDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:29" + ex.Message;
            }
        }
        /// <summary>
        /// /// To Select all the checkboxes under AccountGroup tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountGroupAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:30" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to add a AccountGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountGroupAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:31" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a AccountGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountGroupEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:32" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a AccountGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountGroupDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:33" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under AccountLedger tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountLedgerAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:34" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a AccountLedger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountLedgerAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:35" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a AccountLedger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountLedgerEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:36" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a AccountLedger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAccountLedgerDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:37" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under ProductGroup tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductGroupAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:38" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a ProductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductGroupAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:39" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a ProductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductGroupEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:40" + ex.Message;
            }
        }
        /// <summary>
        /// /// To Set the privilage to Delete a ProductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductGroupDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:41" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under ProductCreation tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductCreationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:42" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a ProductCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductCreationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:43" + ex.Message;
            }
        }
        /// <summary>
        /// /// To Set the privilage to Edit a ProductCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductCreationEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:44" + ex.Message;
            }
        }
        /// <summary>
        /// /// To Set the privilage to Delete a ProductCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductCreationDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:45" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Batch tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBatchAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:46" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBatchAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:47" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBatchEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:48" + ex.Message;
            }
        }
        /// <summary>
        /// /// To Set the privilage to Delete a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBatchDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:49" + ex.Message;
            }
        }
        /// <summary>
        /// /// To Select all the checkboxes under Brand tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBrandAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:50" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Brand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBrandAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:51" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Brand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBrandEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:52" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Set the privilage to Delete a Brand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBrandDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:53" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under ModelNumber tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxModelNumberAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:54" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a ModelNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxModelNumberAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:55" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a ModelNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxModelNumberEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:56" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a ModelNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxModelNumberDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:57" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Size tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSizeAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:58" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSizeAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:59" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Set the privilage to Edit a Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSizeEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:60" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Set the privilage to Delete a Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSizeDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:61" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Master2 tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblMaster2SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:62" + ex.Message;
            }
        }
        /// <summary>
        /// /// To DeSelect all the checkboxes under Master2 tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblMaster2ClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:63" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Unit tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUnitAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:64" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUnitAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:65" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUnitEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST 66" + ex.Message;
            }
        }
        /// <summary>
        /// ///   To Set the privilage to Delete a Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUnitDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:67" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Select all the checkboxes under Godown tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxGodownAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:68" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Godown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxGodownAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:69" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Set the privilage to Edit a Godown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxGodownEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:70" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Set the privilage to Delete a Godown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxGodownDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:71" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Select all the checkboxes under Rack tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRackAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:72" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRackAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:73" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a Rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRackEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:74" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Delete a Rack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRackDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:75" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PricingLevel tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPricingLevelAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:76" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Add a PricingLevel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPricingLevelAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:77" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a PricingLevel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPricingLevelEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:78" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Delete a PricingLevel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPricingLevelDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:79" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PriceList tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPriceListAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:80" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Add a PriceList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPriceListAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:81" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a PriceList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPriceListEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:82" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Delete a PriceList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPriceListDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:83" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under StanderdRate tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStanderdRateAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:84" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Add a StanderdRate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStanderdRateAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:85" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a StanderdRate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStanderdRateEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:86" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Delete a StanderdRate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStanderdRateDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:87" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Tax tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTaxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:88" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Tax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTaxAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:89" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Tax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTaxEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:90" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a Tax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxTaxDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:91" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Currency tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCurrencyAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:92" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCurrencyAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:93" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCurrencyEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:94" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCurrencyDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:95" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under ExchangeRate tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxExchangeRateAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:96" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxExchangeRateAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:97" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxExchangeRateEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:98" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a Currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxExchangeRateDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:99" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under ServiceCategory tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceCategoryAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:100" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a ServiceCategory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceCategoryAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:101" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a ServiceCategory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceCategoryEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:102" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a ServiceCategory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceCategoryDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:103" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Services tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServicesAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:104" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Add a Services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServicesAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:105" + ex.Message;
            }
        }
        /// <summary>
        /// ///   To Set the privilage to Edit a Services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServicesEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:106" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Delete a Services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServicesDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:107" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under VoucherType tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxVoucherTypeAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:108" + ex.Message;
            }
        }
        /// <summary>
        ///   To Set the privilage to Add a VoucherType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxVoucherTypeAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:109" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a VoucherType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxVoucherTypeEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:110" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a VoucherType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxVoucherTypeDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:111" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under ProductTax tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxChangeProductTaxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:112" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a ProductTax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxChangeProductTaxAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:113" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a ProductTax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxChangeProductTaxEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:114" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a ProductTax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxChangeProductTaxDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:115" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Master2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:116" + ex.Message;
            }
        }
        /// <summary>
        ///  To DeSelect all the checkboxes under Master2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:117" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Area tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAreaAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:118" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAreaAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:119" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a Area 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAreaEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:120" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a Area 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAreaDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:121" + ex.Message;
            }
        }
        /// <summary>
        /// ///  To Select all the checkboxes under Route tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRouteAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:122" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Route
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRouteAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:123" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a Route
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRouteEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:124" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a Route
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRouteDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:125" + ex.Message;
            }
        }
        /// <summary>
        ///   To Select all the checkboxes under Counter tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCounterAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:126" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Counter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCounterAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:127" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Counter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCounterEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:128" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a Counter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCounterDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:129" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Batch tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductBatchAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
             formMDI.infoError.ErrorString = "RLPRST:130" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductBatchAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:131" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductBatchEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:132" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxProductBatchDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:133" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under SalesMan tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesManAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:134" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a SalesMan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesManAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:135" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a SalesMan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesManEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:136" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a SalesMan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesManDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:137" + ex.Message;
            }
        }
        /*------------------------- TAB TRANSACTION 1 -------------------------*/
        /// <summary>
        ///  To Select all the checkboxes under Transactions1 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblTransactions1SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:138" + ex.Message;
            }
        }
        /// <summary>
        ///  To deSelect all the checkboxes under Transactions1 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblTransaction1ClearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:139" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under ContraVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxContraVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:140" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a ContraVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxContraVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:141" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a ContraVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxContraVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxContraVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:142" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a ContraVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxContraVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxContraVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:143" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under PaymentVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPaymentVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:144" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PaymentVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPaymentVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:145" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PaymentVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPaymentVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPaymentVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:146" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PaymentVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPaymentVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPaymentVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:147" + ex.Message;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under ReceiptVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReceiptVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:148" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a ReceiptVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReceiptVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:149" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a ReceiptVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReceiptVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxReceiptVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:150" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a ReceiptVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReceiptVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxReceiptVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:151" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under JuornalVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJuornalVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:152" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a JuornalVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJournalVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:153" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a JuornalVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJournalVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxJournalVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:154" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a JuornalVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJournalVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxJournalVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:155" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PDCPayble tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCPaybleAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:156" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PDCPayble
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCPaybleAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:157" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PDCPayble
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCPaybleEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPDCPayable.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:158" + ex.Message;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PDCPayble
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCPaybleDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPDCPayable.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:159" + ex.Message;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PDCReceivable tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCReceivableAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:160" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a PDCReceivable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCReceivableAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:161" + ex.Message;
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a PDCReceivable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCReceivableEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPDCReceivable.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:162" + ex.Message; ;
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a PDCReceivable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCReceivableDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPDCReceivable.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:163" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under PDCClearence tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCClearenceAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:164" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PDCClearence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCClearenceAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:165" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PDCClearence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCClearenceEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPDCClearence.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:166" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PDCClearence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCClearenceDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPDCClearence.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:167" + ex.Message; ; ;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under PurchaseOrder tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseOrderAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:168" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PurchaseOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseOrderAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:169" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PurchaseOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseOrderEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPurchaseOrder.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:170" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PurchaseOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseOrderDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPurchaseOrder.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:171" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under MaterialReceipt tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMaterialReceiptAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:172" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a MaterialReceipt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMaterialReceiptAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:173" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a MaterialReceipt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMaterialReceiptEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxMaterialReceipt.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:174" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a MaterialReceipt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMaterialReceiptDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxMaterialReceipt.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:175" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under RejectionOut tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionOutAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:176" + ex.Message; ;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a RejectionOut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionOutAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:177" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to edit a RejectionOut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionOutEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxRejectionOut.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:178" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a RejectionOut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionOutDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxRejectionOut.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:179" + ex.Message; ;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PurchaseInvoice tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseInvoiceAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:180" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PurchaseInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseInvoiceAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:181" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PurchaseInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseInvoiceEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPurchaseInvoice.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:182" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PurchaseInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseInvoiceDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPurchaseInvoice.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:183" + ex.Message; ;
            }
        }
        /// <summary>
        ///   To Select all the checkboxes under Transactions2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblTransactions2SelectAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:184" + ex.Message; ;
            }
        }
        /// <summary>
        /// To DeSelect all the checkboxes under Transactions2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblTransactions2CearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:185" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Select all the checkboxes under PurchaseReturn tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseReturnAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:186" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PurchaseReturn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseReturnAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:187" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PurchaseReturn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseReturnEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPurchaseReturn.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:188" + ex.Message; ;
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PurchaseReturn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseReturnDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPurchaseReturn.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:189" + ex.Message; ;
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under SalesQuotation tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesQuotationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:190" + ex.Message; ;
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a SalesQuotation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesQuotationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:191" + ex.Message; 
            }
        }
        /// <summary>
        ///   To Set the privilage to Edit a SalesQuotation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesQuotationEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxSalesQuotation.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:192" + ex.Message; 
            }
        }
        /// <summary>
        ///   To Set the privilage to Delete a SalesQuotation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesQuotationDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxSalesQuotation.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:193" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under SalesOrder tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesOrderAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:194" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a SalesOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesOrderAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:195" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a SalesOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesOrderEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxSalesOrder.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:196" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a SalesOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesOrderDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxSalesOrder.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:197" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under DeliveryNote tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDeliveryNoteAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:198" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a DeliveryNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDeliveryNoteAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:199" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a DeliveryNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDeliveryNoteEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxDeliveryNote.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:200" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a DeliveryNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDeliveryNoteDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxDeliveryNote.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:201" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under RejectionIn tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionInAll_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:202" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a RejectionIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionInAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:203" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a RejectionIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionInEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxRejectionIn.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:204" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a RejectionIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionInDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxRejectionIn.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:205" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under SalesInvoice tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesInvoiceAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:206" + ex.Message; 
            }
        }
        /// <summary>
        ///   To Set the privilage to Add a SalesInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesInvoiceAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:207" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a SalesInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesInvoiceEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxSalesInvoice.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:208" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a SalesInvoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesInvoiceDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxSalesInvoice.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:209" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under POS tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPOSAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:210" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a POS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPOSAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:211" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a POS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPOSEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:212" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a POS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPOSDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:213" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under SalesReturn tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesReturnAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:214" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a SalesReturn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesReturnAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:215" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a SalesReturn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesReturnEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxSalesReturn.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:216" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a SalesReturn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesReturnDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxSalesReturn.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:217" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PhysicalStock tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPhysicalStockAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:218" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PhysicalStock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPhysicalStockAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:219" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PhysicalStock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPhysicalStockEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxPhysicalStock.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:220" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PhysicalStock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPhysicalStockDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxPhysicalStock.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:221" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under ServiceVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:222" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a ServiceVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:223" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a ServiceVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxServiceVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:224" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a ServiceVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxServiceVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:225" + ex.Message; 
            }
        }
        /// <summary>
        ///   To Select all the checkboxes under CreditNote tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCreditNoteAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:226" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a CreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCreditNoteAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:227" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a CreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCreditNoteEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxCreditNote.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:228" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a CreditNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCreditNoteDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxCreditNote.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:229" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under DebitNote tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDebitNoteAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:230" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a DebitNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDebitNoteAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:231" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a DebitNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDebitNoteEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxDebitNote.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:232" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a DebitNote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDebitNoteDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxDebitNote.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:233" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Transactions3 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblTransactions3SelectAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:234" + ex.Message; 
            }
        }
        /// <summary>
        ///  To DeSelect all the checkboxes under Transactions3 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblTransactions3CearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:235" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under StockJournal tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStockJournalAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:236" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a StockJournal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStockJournalAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:237" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a StockJournal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStockJournalEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strEdit = gbxStockJournal.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strEdit);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:238" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a StockJournal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStockJournalDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strDelete = gbxStockJournal.Text.Replace(" ", string.Empty);
                RegisterTickEditOrDeleteTicked(strDelete);
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:239" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under BillAllocation tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBillAllocationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:240" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a BillAllocation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBillAllocationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:241" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View a BillAllocation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBillAllocationView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:242" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under REGISTER tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:243" + ex.Message; 
            }
        }
        /// <summary>
        /// To DeSelect all the checkboxes under REGISTER tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:244" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  ContraVoucherRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxContraVoucherRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxContraVoucherRegister.Checked == false)
                {
                    string strTr = cbxContraVoucherRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:245" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PaymentRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPaymentRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPaymentRegister.Checked == false)
                {
                    string strTr = cbxPaymentRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:246" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  ReceiptRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReceiptRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxReceiptRegister.Checked == false)
                {
                    string strTr = cbxReceiptRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:247" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  JournalRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJournalRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxJournalRegister.Checked == false)
                {
                    string strTr = cbxJournalRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:248" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  PDCPayableRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCPayableRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPDCPayableRegister.Checked == false)
                {
                    string strTr = cbxPDCPayableRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:249" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PDCReceivableRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCReceivableRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPDCReceivableRegister.Checked == false)
                {
                    string strTr = cbxPDCReceivableRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:250" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PDCClearanceRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPDCClearanceRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPDCClearanceRegister.Checked == false)
                {
                    string strTr = cbxPDCClearanceRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:251" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PurchaseOrderRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseOrderRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPurchaseOrderRegister.Checked == false)
                {
                    string strTr = cbxPurchaseOrderRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:252" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  MaterialReceiptRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMaterialReceiptRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxMaterialReceiptRegister.Checked == false)
                {
                    string strTr = cbxMaterialReceiptRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:253" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  RejectionOutRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionOutRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxRejectionOutRegister.Checked == false)
                {
                    string strTr = cbxRejectionOutRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:254" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PurchaseInvoiceRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseInvoiceRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPurchaseInvoiceRegister.Checked == false)
                {
                    string strTr = cbxPurchaseInvoiceRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:255" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PurchaseReturnRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPurchaseReturnRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPurchaseReturnRegister.Checked == false)
                {
                    string strTr = cbxPurchaseReturnRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:256" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  SalesQuotationRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesQuotationRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalesQuotationRegister.Checked == false)
                {
                    string strTr = cbxSalesQuotationRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:257" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  SalesOrderRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesOrderRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalesOrderRegister.Checked == false)
                {
                    string strTr = cbxSalesOrderRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:258" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  DeliveryNoteRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDeliveryNoteRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxDeliveryNoteRegister.Checked == false)
                {
                    string strTr = cbxDeliveryNoteRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:259" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  RejectionInRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRejectionInRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxRejectionInRegister.Checked == false)
                {
                    string strTr = cbxRejectionInRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:260" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  SalesInvoiceRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesInvoiceRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalesInvoiceRegister.Checked == false)
                {
                    string strTr = cbxSalesInvoiceRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:261" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  SalesReturnRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalesReturnRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalesReturnRegister.Checked == false)
                {
                    string strTr = cbxSalesReturnRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:262" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  PhysicalStockRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPhysicalStockRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPhysicalStockRegister.Checked == false)
                {
                    string strTr = cbxPhysicalStockRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:263" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  ServiceRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxServiceRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxServiceRegister.Checked == false)
                {
                    string strTr = cbxServiceRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:264" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  CreditNotRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxCreditNotRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxCreditNotRegister.Checked == false)
                {
                    string strTr = cbxCreditNotRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:265" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  DebitNotRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDebitNotRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxDebitNotRegister.Checked == false)
                {
                    string strTr = cbxDebitNotRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:266" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  StockJournalRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxStockJournalRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxStockJournalRegister.Checked == false)
                {
                    string strTr = cbxStockJournalRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strTr);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:267" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under PAYROLL1 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSettingsSelecall_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:268" + ex.Message; 
            }
        }
        /// <summary>
        /// To DeSelect all the checkboxes under PAYROLL1 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSettingsClearall_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:269" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Designation tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDesignationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:270" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a Designation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDesignationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:271" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a Designation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDesignationEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:272" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a Designation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDesignationDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:273" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under PayHead tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPayHeadAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:274" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a PayHead
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPayHeadAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:275" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PayHead
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPayHeadEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:276" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PayHead
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPayHeadDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:277" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under SalaryPackageCreation tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalaryPackageCreationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:278" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a SalaryPackageCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalaryPackageCreationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:279" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a SalaryPackageCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalaryPackageCreationEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalaryPackageCreationEdit.Checked)
                {
                    string strEdit = gbxSalaryPackageCreation.Text.Replace("Creation", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strEdit);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:280" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a SalaryPackageCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalaryPackageCreationDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalaryPackageCreationDelete.Checked)
                {
                    string strDelete = gbxSalaryPackageCreation.Text.Replace("Creation", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strDelete);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:281" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under EmployeeCreation tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEmployeeCreationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:282" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a EmployeeCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEmployeeCreationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:283" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a EmployeeCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEmployeeCreationEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxEmployeeCreationEdit.Checked)
                {
                    string strEdit = gbxEmployeeCreation.Text.Replace("Creation", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strEdit);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:284" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a EmployeeCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEmployeeCreationDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxEmployeeCreationDelete.Checked)
                {
                    string strDelete = gbxEmployeeCreation.Text.Replace("Creation", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strDelete);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:285" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under MonthlySalarySettings tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalarySettingsAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:286" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a MonthlySalarySettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalarySettingsAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:287" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a MonthlySalarySettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalarySettingsEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:288" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a MonthlySalarySettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalarySettingsDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:289" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Attendance tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAttendanceAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:290" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Attendance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAttendanceAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:291" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a Attendance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAttendanceEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:292" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a Attendance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAttendanceDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:293" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under AdvancePayment tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAdvancePaymentAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:294" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a AdvancePayment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAdvancePaymentAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:295" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a AdvancePayment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAdvancePaymentEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxAdvancePaymentEdit.Checked)
                {
                    string strEdit = gbxAdvancePayment.Text.Replace("Payment", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strEdit);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:296" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a AdvancePayment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAdvancePaymentDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxAdvancePaymentDelete.Checked)
                {
                    string strDelete = gbxAdvancePayment.Text.Replace("Payment", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strDelete);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:297" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under BonusDeduction tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBonusDeductionAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:298" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a BonusDeduction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBonusDeductionAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:299" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a BonusDeduction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBonusDeductionEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxBonusDeductionEdit.Checked)
                {
                    string strEdit = gbxBonusDeduction.Text.Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strEdit);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:300" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a BonusDeduction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBonusDeductionDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxBonusDeductionDelete.Checked)
                {
                    string strDelete = gbxBonusDeduction.Text.Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strDelete);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:301" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under MonthlySalaryVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalaryVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:302" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a MonthlySalaryVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalaryVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:303" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a MonthlySalaryVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalaryVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxMonthlySalaryVoucherEdit.Checked)
                {
                    string strEdit = gbxMonthlySalaryVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strEdit);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:304" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a MonthlySalaryVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalaryVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxMonthlySalaryVoucherDelete.Checked)
                {
                    string strDelete = gbxMonthlySalaryVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strDelete);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:305" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  SalaryPackageRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSalaryPackageRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxSalaryPackageRegister.Checked == false)
                {
                    string strRegister = cbxSalaryPackageRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strRegister);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:306" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  EmbloyeeRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxEmbloyeeRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxEmployeeRegister.Checked == false)
                {
                    string strRegister = cbxEmployeeRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strRegister);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:307" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to View  AdvanceRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAdvanceRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxAdvanceRegister.Checked == false)
                {
                    string strRegister = cbxAdvanceRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strRegister);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:308" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  BonusDeductionRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBonusDeductionRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxBonusDeductionRegister.Checked == false)
                {
                    string strRegister = cbxBonusDeductionRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strRegister);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:309" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  MonthlySalaryRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMonthlySalaryRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxMonthlySalaryRegister.Checked == false)
                {
                    string strRegister = cbxMonthlySalaryRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strRegister);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:310" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under PAYROLL2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSearchSelectAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:311" + ex.Message; 
            }
        }
        /// <summary>
        ///  To DeSelect all the checkboxes under PAYROLL2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSearchClearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:312" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under DailySalaryVoucher tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDailySalaryVoucherAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:313" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a DailySalaryVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDailySalaryVoucherAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:314" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a DailySalaryVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDailySalaryVoucherEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxDailySalaryVoucherEdit.Checked)
                {
                    string strEdit = gbxDailySalaryVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strEdit);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:315" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Delete a DailySalaryVoucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDailySalaryVoucherDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxDailySalaryVoucherDelete.Checked)
                {
                    string strDelete = gbxDailySalaryVoucher.Text.Replace("Voucher", string.Empty).Replace(" ", string.Empty);
                    RegisterTickEditOrDeleteTicked(strDelete);
                }
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:316" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View  DailySalaryRegister
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDailySalaryRegister_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxDailySalaryRegister.Checked == false)
                {
                    string strRegister = cbxDailySalaryRegister.Text.Replace("Register", string.Empty).Replace(" ", string.Empty);
                    RegisterUnchecked(strRegister);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:317" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under BUDGET tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblBudgetSelectAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:318" + ex.Message; 
            }
        }
        /// <summary>
        /// To DeSelect all the checkboxes under BUDGET tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblBudgetClearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:319" + ex.Message; 
            }
        }
        /// <summary>
        ///   To Select all the checkboxes under Budgeting tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBudgetingAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:320" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Budgeting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBudgetingAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:321" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a Budgeting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBudgetingEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:322" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a Budgeting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBudgetingDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:323" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under SETTINGS tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblPayrollSelectAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:324" + ex.Message; 
            }
        }
        /// <summary>
        /// To DeSelect all the checkboxes under SETTINGS tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblPayrollClearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:325" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under UserCreation tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUserCreationAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:326" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a UserCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUserCreationAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:327" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Edit a UserCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUserCreationEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:328" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a UserCreation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxUserCreationDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:329" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Role tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:330" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:331" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a Role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:332" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a Role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:333" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under SuffixPrefixSettings tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSuffixPrefixSettingsAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:334" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a SuffixPrefixSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSuffixPrefixSettingsAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:335" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a SuffixPrefixSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSuffixPrefixSettingsEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:336" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a SuffixPrefixSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSuffixPrefixSettingsDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:337" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under BarcodeSettings tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBarcodeSettingsAll_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:338" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a BarcodeSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBarcodeSettingsAdd_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:339" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View a BarcodeSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBarcodeSettingsView_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:340" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under Settings tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSettingsAll_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:341" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSettingsAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:342" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View a Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSettingsView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:343" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under RoleprivilegeSettings tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleprivilegeSettingsAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:344" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Set the privilage to Add a RoleprivilegeSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleprivilegeSettingsAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:345" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to View the RoleprivilegeSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRoleprivilegeSettingsView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:346" + ex.Message; 
            }
        }
        /// <summary>
        /// To Select all the checkboxes under SEARCH tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblFinancialStatementsSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:347" + ex.Message; 
            }
        }
        /// <summary>
        ///  To deSelect all the checkboxes under SEARCH tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblFinancialStatementsClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:348" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under REMINDER tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblReports1SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:349" + ex.Message; 
            }
        }
        /// <summary>
        /// To DeSelect all the checkboxes under REMINDER tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblReports1ClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:350" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under Reports1Clear  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPersonalReminderAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToClickAll(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:351" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Add a PersonalReminder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPersonalReminderAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:352" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Edit a PersonalReminder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPersonalReminderEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:353" + ex.Message; 
            }
        }
        /// <summary>
        /// To Set the privilage to Delete a PersonalReminder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPersonalReminderDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToTickAllCheckBox(sender);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:354" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under FINANCIAL STATEMENTS tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblReports2SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:355" + ex.Message; 
            }
        }
        /// <summary>
        ///  To DeSelect all the checkboxes under FINANCIAL STATEMENTS tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblReports2ClearAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:356" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under REPORTS1 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:357" + ex.Message; 
            }
        }
        /// <summary>
        ///  To DeSelect all the checkboxes under REPORTS1 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:358" + ex.Message; 
            }
        }
        /// <summary>
        ///  To Select all the checkboxes under REPORTS2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblReports2SelectAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:359" + ex.Message; 
            }
        }
        /// <summary>
        ///  To DeSelect all the checkboxes under REPORTS2 tab 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblReports2ClearAll_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:360" + ex.Message; 
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Form keydown for quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRolePrivilegeSettings_KeyDown(object sender, KeyEventArgs e)
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
                        this.Close();
                    }
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RLPRST:361" + ex.Message; 
            }
        }
        #endregion

        private void tcPrivillage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbSettings_Click(object sender, EventArgs e)
        {

        }
        
    }
}
