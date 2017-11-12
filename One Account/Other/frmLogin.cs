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
    public partial class frmLogin : Form
    {
        #region PublicVariables
        formMDI formMdiObj = null;
        frmSelectCompany frmSelectCompanyObj = null;
        #endregion
        #region Function
        /// <summary>
        /// Creates an instance of a frmLogin class.
        /// </summary>
        public frmLogin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Start a login
        /// </summary>
        public void Login()
        {
            try
            {
                UserSP spUser = new UserSP();
                CompanySP spCompany = new CompanySP();
                CompanyInfo infoCompany = new CompanyInfo();
                string strUserName = txtUserName.Text.Trim();
                string strPassword = spUser.LoginCheck(strUserName);
                
                if (strPassword == txtPassword.Text.Trim() && strPassword != string.Empty)
                {
                    int inUserId = spUser.GetUserIdAfterLogin(strUserName, strPassword);
                    PublicVariables._decCurrentUserId = inUserId;
                    infoCompany = spCompany.CompanyView(1);
                    PublicVariables._decCurrencyId = infoCompany.CurrencyId;
                    formMDI.MDIObj.CallFromLogin();
                    SettingsCheck();
                    //for Quock Launch menu
                    formMDI.MDIObj.ShowQuickLaunchMenu();
                    formMDI.MDIObj.CurrentSettings();
                    //Display ChangeCurrentDate form//
                    frmChangeCurrentDate frmCurrentDateChangeObj = new frmChangeCurrentDate();
                    frmCurrentDateChangeObj.MdiParent = formMDI.MDIObj;
                    frmCurrentDateChangeObj.CallFromLogin(this);
                    formMDI.MDIObj.Text = "Oneaccount " + infoCompany.CompanyName + " [ " + PublicVariables._dtFromDate.ToString("dd-MMM-yyyy") + " To " + PublicVariables._dtToDate.ToString("dd-MMM-yyyy") + " ]";
                    // For showing the Oneaccount message from the website
                    formMDI.MDIObj.logoutToolStripMenuItem.Enabled = true;
                    if (PublicVariables.MessageToShow != string.Empty)
                    {
                        frmMessage frmMsg = new frmMessage();
                        frmMsg.lblHeading.Text = PublicVariables.MessageHeadear;
                        frmMsg.lblMessage.Text = PublicVariables.MessageToShow;
                        frmMsg.MdiParent = formMDI.MDIObj;
                        frmMsg.Show();
                        frmMsg.Location = new Point(0, formMDI.MDIObj.Height - 270);
                        foreach (Form form in Application.OpenForms)
                        {
                            if (form.GetType() == typeof(frmChangeCurrentDate))
                            {
                                form.Focus();
                            }
                        }
                    }
                }
                else
                {
                    lblError.Visible = true;
                    Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call from MDI form to show Login form and passing the MDI form Object
        /// </summary>
        /// <param name="frmMdiObj"></param>
        public void CallFromFormMdi(formMDI frmMdiObj)
        {
            try
            {
                formMdiObj = frmMdiObj;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call from 'Select Company' form, To show Login form and close Select Company form
        /// </summary>
        /// <param name="frmSelectCompanyObj"></param>
        public void CallFromSelectCompany(frmSelectCompany frmSelectCompanyObj)
        {
            try
            {
                base.Show();
                this.frmSelectCompanyObj = frmSelectCompanyObj;
                frmSelectCompanyObj.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN3:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the general settings before loading the MDI page.
        /// This either enables or disables the controls on the form depending on the current settings
        /// </summary>
        public void SettingsCheck()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();
                if (spSettings.SettingsStatusCheck("Tax") == "Yes")
                {
                    formMDI.MDIObj.taxToolStripMenuItem.Enabled = true;
                }
                else
                {
                    formMDI.MDIObj.taxToolStripMenuItem.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("Budget") == "Yes")
                {
                    formMDI.MDIObj.budgetToolStripMenuItem.Enabled = true;
                }
                else
                {
                    formMDI.MDIObj.budgetToolStripMenuItem.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("Payroll") == "Yes")
                {
                    formMDI.MDIObj.payrollToolStripMenuItem.Enabled = true;
                    formMDI.MDIObj.payrollToolStripMenuItem1.Enabled = true;
                    foreach (ToolStripMenuItem toolItem in formMDI.MDIObj.payrollToolStripMenuItem1.DropDownItems)
                    {
                        toolItem.Enabled = true;
                    }
                }
                else
                {
                    formMDI.MDIObj.payrollToolStripMenuItem.Enabled = false;
                    formMDI.MDIObj.payrollToolStripMenuItem1.Enabled = false;
                    foreach (ToolStripMenuItem toolItem in formMDI.MDIObj.payrollToolStripMenuItem1.DropDownItems)
                    {
                        toolItem.Enabled = false;
                    }
                }
                if (spSettings.SettingsStatusCheck("MultiCurrency") == "Yes")
                {
                    formMDI.MDIObj.currencyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    formMDI.MDIObj.currencyToolStripMenuItem.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("AllowBatch") == "Yes")
                {
                    formMDI.MDIObj.batchToolStripMenuItem.Enabled = true;
                }
                else
                {
                    formMDI.MDIObj.batchToolStripMenuItem.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("AllowSize") == "Yes")
                {
                    formMDI.MDIObj.sizeToolStripMenuItem.Enabled = true;
                }
                else
                {
                    formMDI.MDIObj.sizeToolStripMenuItem.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("AllowGodown") == "Yes")
                {
                    formMDI.MDIObj.godownToolStripMenuItem.Enabled = true;
                    if (spSettings.SettingsStatusCheck("AllowRack") == "Yes")
                    {
                        formMDI.MDIObj.rackToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        formMDI.MDIObj.rackToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    formMDI.MDIObj.godownToolStripMenuItem.Enabled = false;
                    formMDI.MDIObj.rackToolStripMenuItem.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("AllowModelNo") == "Yes")
                {
                    formMDI.MDIObj.modelNumberToolStripMenuItem.Enabled = true;
                }
                else
                {
                    formMDI.MDIObj.modelNumberToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN4:" + ex.Message;
            }
        }
        /// <summary>
        /// Reseting the login page
        /// </summary>
        public void Clear()
        {
            try
            {
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtUserName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN5:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN6:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Login' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Login();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN7:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Reset' button click
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
                formMDI.infoError.ErrorString = "LOGIN8:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN9:" + ex.Message;
            }
        }
        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose)
                    {
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN10:" + ex.Message;
            }
        }
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblUserName.Visible = (txtUserName.Text == "") ? true : false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN11:" + ex.Message;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblPassword.Visible = (txtPassword.Text == "") ? true : false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN12:" + ex.Message;
            }
        }
        private void lblUserName_Click(object sender, EventArgs e)
        {
            try
            {
                txtUserName.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN13:" + ex.Message;
            }
        }

        private void lblPassword_Click(object sender, EventArgs e)
        {
            try
            {
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN14:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// On Keydown, For navigating to Password textbox using Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN15:" + ex.Message;
            }
        }
        /// <summary>
        /// On Keydown, For navigating to Login button/Username textbox using Enter/Backspace keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnLogin.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtPassword.Text == string.Empty || txtPassword.SelectionStart == 0)
                    {
                        txtUserName.SelectionStart = 0;
                        txtUserName.SelectionLength = 0;
                        txtUserName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN16:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation of btnLogin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "LOGIN17:" + ex.Message;
            }
        }
        #endregion
    }
}
