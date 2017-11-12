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
    public partial class frmChangePassword : Form
    {
        /// <summary>
        ///  Create an instance for frmChangePassword
        /// </summary>
        public frmChangePassword()
        {
            InitializeComponent();
        }

        #region Functions
        /// <summary>
        /// Cleare function, to clear the form controls
        /// </summary>
        public void Clear()
        {
            try
            {
                txtOldPassword.Clear();
                txtNewPassword.Clear();
                txtRetype.Clear();
                txtOldPassword.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:1" + ex.Message;
               
            }
        }
        /// <summary>
        /// Save function, Checking the Invalid entries
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                UserInfo infoUser = new UserInfo();
                UserSP spUser = new UserSP();
                string strUserName = txtUserName.Text.Trim();
                string strPassword = spUser.LoginCheck(strUserName);

                if (txtOldPassword.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter password");
                    txtOldPassword.Focus();
                }
                else if (txtNewPassword.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter new password");
                    txtNewPassword.Focus();
                }
                else if (txtRetype.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter confirmation password");
                    txtRetype.Focus();
                }
                else if (txtNewPassword.Text != txtRetype.Text)
                {
                    Messages.InformationMessage("Password and confirm password should match");
                    txtRetype.Focus();
                    txtRetype.SelectAll();
                }
                else if (strPassword == txtOldPassword.Text.Trim())
                {
                    if (PublicVariables.isMessageAdd)
                    {
                        if (Messages.SaveMessage())
                        {
                            infoUser.UserId = PublicVariables._decCurrentUserId;
                            infoUser.UserName = txtUserName.Text.Trim();
                            infoUser.Password = txtNewPassword.Text.Trim();
                            spUser.ChangePasswordEdit(infoUser);
                            Clear();
                            Messages.SavedMessage();
                            this.Close();
                        }
                    }
                    else
                    {
                        infoUser.UserId = PublicVariables._decCurrentUserId;
                        infoUser.UserName = txtUserName.Text.Trim();
                        infoUser.Password = txtNewPassword.Text.Trim();
                        spUser.ChangePasswordEdit(infoUser);
                    }
                }
                else
                {
                    Messages.InformationMessage("Invalid password");
                    txtOldPassword.Focus();
                    txtOldPassword.SelectAll();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:2" + ex.Message;
            }
        }

        #endregion

        #region Events
        /// <summary>
        ///  Form load make the form controls based on settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            try
            {
                UserInfo infoUser = new UserInfo();
                UserSP spUser = new UserSP();
                infoUser = spUser.UserView(PublicVariables._decCurrentUserId);
                txtUserName.Text = infoUser.UserName;
                txtUserName.ReadOnly = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:3" + ex.Message;
            }
        }
        /// <summary>
        /// Save button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveFunction();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:4" + ex.Message;
            }
        }
        /// <summary>
        /// Reset Button Click, Call the Clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:5" + ex.Message;
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
                formMDI.infoError.ErrorString = "CHGPWD:6" + ex.Message;
            }
        }

        #endregion

        #region Navigations
        /// <summary>
        /// For enter key Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtOldPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:7" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and Backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOldPassword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNewPassword.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtOldPassword.Text == string.Empty || txtOldPassword.SelectionStart == 0)
                    {
                        txtUserName.Focus();
                        txtUserName.SelectionStart = 0;
                        txtUserName.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:8" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and Backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNewPassword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtRetype.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNewPassword.Text == string.Empty || txtNewPassword.SelectionStart == 0)
                    {
                        txtOldPassword.Focus();
                        txtOldPassword.SelectionStart = 0;
                        txtOldPassword.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:9" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and Backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRetype_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtRetype.Text == string.Empty || txtRetype.SelectionStart == 0)
                    {
                        txtNewPassword.Focus();
                        txtNewPassword.SelectionStart = 0;
                        txtNewPassword.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:10" + ex.Message;
            }
        }
        /// <summary>
        /// Form Key down for Quick Access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangePassword_KeyDown(object sender, KeyEventArgs e)
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:11" + ex.Message;
            }
        }
        /// <summary>
        ///  For enter key and Backspace Navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtRetype.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGPWD:12" + ex.Message;
            }
        }

        #endregion

    }
}
