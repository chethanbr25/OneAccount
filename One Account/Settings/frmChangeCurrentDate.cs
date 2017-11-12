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
    public partial class frmChangeCurrentDate : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable Declaration Part
        /// </summary>
        frmLogin frmLoginObj;
        frmCompanyCreation frmCompanyCreationObj;
        #endregion
        #region Function
        /// <summary>
        /// Create An Instance for frmChangeCurrentDate Class
        /// </summary>
        public frmChangeCurrentDate()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 1 : " + ex.Message;
               
            }
        }
        /// <summary>
        /// To close the form
        /// </summary>
        public void FormClose()
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
                formMDI.infoError.ErrorString = "CCD 2 : " + ex.Message;
            }
        }
        /// <summary>
        /// To set current date
        /// </summary>
        public void CurrentDate()
        {
            try
            {
                dtpCompanyCurrentDate.MinDate = PublicVariables._dtFromDate;
                dtpCompanyCurrentDate.MaxDate = PublicVariables._dtToDate;
                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();
                infoComapany = spCompany.CompanyView(1);
                DateTime dtCurrentDate = PublicVariables._dtFromDate;
                if (infoComapany.CurrentDate != null)
                {
                    dtCurrentDate = infoComapany.CurrentDate;
                }
                if (dtCurrentDate > PublicVariables._dtToDate)
                {
                    dtCurrentDate = PublicVariables._dtToDate;
                }
                if (dtCurrentDate < PublicVariables._dtFromDate)
                {
                    dtCurrentDate = PublicVariables._dtFromDate;
                }
                dtpCompanyCurrentDate.Value = dtCurrentDate;
                PublicVariables._dtCurrentDate = dtCurrentDate;
                DateTime sysDate = System.DateTime.Today;
                if (sysDate >= PublicVariables._dtFromDate && sysDate <= PublicVariables._dtToDate)
                {
                    txtCompanyCurrentdate.Text = sysDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    txtCompanyCurrentdate.Text = dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                dtpCompanyCurrentDate.Value = Convert.ToDateTime(txtCompanyCurrentdate.Text);
                txtCompanyCurrentdate.Focus();
                txtCompanyCurrentdate.SelectAll();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 3 : " + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmLogin to view details and for updation 
        /// </summary>
        /// <param name="frmLogin"></param>
        public void CallFromLogin(frmLogin frmLogin)
        {
            try
            {
                frmLoginObj = frmLogin;
                frmLogin.Close();
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 4 : " + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmCompanyCreation to view details and for updation 
        /// </summary>
        /// <param name="frmCompanyCreation"></param>
        public void CallFromNewFinancialYear(frmCompanyCreation frmCompanyCreation)
        {
            try
            {
                frmCompanyCreationObj = frmCompanyCreation;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 5: " + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangeCurrentDate_Load(object sender, EventArgs e)
        {
            try
            {
                this.MdiParent = formMDI.MDIObj;
                CurrentDate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 6 : " + ex.Message;
            }
        }
        /// <summary>
        /// Close button click Call the Form Close function here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                FormClose();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 7 : " + ex.Message;
            }
        }
        /// <summary>
        /// To Set Dtp Value into textbox value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpCompanyCurrentDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                txtCompanyCurrentdate.Text = dtpCompanyCurrentDate.Value.ToString("dd-MMM-yyyy");
                txtCompanyCurrentdate.Focus();
                txtCompanyCurrentdate.SelectionStart = 0;
                txtCompanyCurrentdate.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 8 : " + ex.Message;
            }
        }
        /// <summary>
        /// Date Validation in Text leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCompanyCurrentdate_Leave(object sender, EventArgs e)
        {
            bool isValid = true;
            try
            {
                DateValidation obj = new DateValidation();
                isValid = obj.DateValidationFunction(txtCompanyCurrentdate);
                if (!isValid)
                {
                    txtCompanyCurrentdate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 9 : " + ex.Message;
            }
        }
        /// <summary>
        /// Resest Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentDate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 10 : " + ex.Message;
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
                if (txtCompanyCurrentdate.Text == string.Empty)
                {
                    Messages.InformationMessage("Select a date in between financial year");
                    txtCompanyCurrentdate.Focus();
                }
                else
                {
                    PublicVariables._dtCurrentDate = Convert.ToDateTime(txtCompanyCurrentdate.Text);
                    CompanySP spCompany = new CompanySP();
                    spCompany.CompanyCurrentDateEdit(Convert.ToDateTime(txtCompanyCurrentdate.Text));
                    if (frmCompanyCreationObj != null)
                    {
                        if ((MessageBox.Show("Saved successfully...Do you want to save settings now? ", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
                        {
                            frmSettings frmSettingsObj = new frmSettings();
                            frmSettingsObj.MdiParent = formMDI.MDIObj;
                            frmSettingsObj.CallFromChangeCurrentDate(frmCompanyCreationObj);
                        }
                    }
                    this.Close();
                    if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmShortExpiry", "View") || CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmStock", "View") ||
                        CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmOverdueSalesOrder", "View") || CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmOverduePurchaseOrder", "View") ||
                        CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmPersonalReminder", "Delete") || CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmPersonalReminder", "Update") ||
                        CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmPersonalReminder", "Save"))
                    {
                        ShowReminderIfAny();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 11 : " + ex.Message;
            }
        }
        /// <summary>
        /// Form closing event for showing current date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangeCurrentDate_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                formMDI.MDIObj.ShowCurrentDate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 12 : " + ex.Message;
            }
        }
        /// <summary>
        /// Showing Reminder If Any one have
        /// </summary>
        public void ShowReminderIfAny()
        {
            try
            {
                string Purchasereminder = string.Empty;
                string Salesreminder = string.Empty;
                string Personalreminder = string.Empty;
                string NegativeStkreminder = string.Empty;
                string MinStkreminder = string.Empty;
                string MaxStkreminder = string.Empty;
                string ReordrStkreminder = string.Empty;
                string ShortExpiryReminder = string.Empty;
                DataTable dtbl = new DataTable();
                ReminderSP SpReminder = new ReminderSP();
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmOverduePurchaseOrder", "View"))
                {
                    PurchaseOrderMasterSP SpPurchaseOrderMaster = new PurchaseOrderMasterSP();
                    dtbl = SpPurchaseOrderMaster.PurchaseOrderOverDueReminder(PublicVariables._dtFromDate, PublicVariables._dtToDate, "Due", PublicVariables._dtCurrentDate, "All");
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            Purchasereminder = Purchasereminder + "1 Overdue purchase order\n\n";
                        else
                            Purchasereminder = Purchasereminder + dtbl.Rows.Count + " Overdue purchase orders\n\n";
                    }
                }
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmOverdueSalesOrder", "View"))
                {
                    SalesOrderMasterSP SpSalesOrderMaster = new SalesOrderMasterSP();
                    dtbl = SpSalesOrderMaster.SalesOrderOverDueReminder(PublicVariables._dtFromDate, PublicVariables._dtToDate, "Due", PublicVariables._dtCurrentDate, "All");
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            Salesreminder = Salesreminder + "1 Overdue sales order\n\n";
                        else
                            Salesreminder = Salesreminder + dtbl.Rows.Count + " Overdue sales orders\n\n";
                    }
                }
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmPersonalReminder", "Delete") ||
                    CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmPersonalReminder", "Update") ||
                    CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmPersonalReminder", "Save"))
                {
                    string FromDate = Convert.ToString(PublicVariables._dtCurrentDate);
                    string ToDate = Convert.ToString(PublicVariables._dtCurrentDate);
                    dtbl = SpReminder.ReminderSearch(FromDate, ToDate);
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            Personalreminder = Personalreminder + "1 personal reminder \n\n";
                        else
                            Personalreminder = Personalreminder + dtbl.Rows.Count + " Personal reminders\n\n";
                    }
                }
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmShortExpiry", "View"))
                {
                    dtbl = SpReminder.ShortExpiryReminder(0, 0, 0, 0, 0, 0, 0, 0);
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            ShortExpiryReminder = ShortExpiryReminder + "1 Product batch is going to expire";
                        else
                            ShortExpiryReminder = ShortExpiryReminder + dtbl.Rows.Count + " Product batches going to expire";
                    }
                }
                //frmStock
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, "frmStock", "View"))
                {
                    dtbl = SpReminder.StockSearch(0, 0, 0, 0, 0, 0, 0, 0, "Negative Stock");
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            NegativeStkreminder = NegativeStkreminder + "1 Negative stock product \n\n";
                        else
                            NegativeStkreminder = NegativeStkreminder + dtbl.Rows.Count + " Negative stock products\n\n";
                    }

                    dtbl = SpReminder.StockSearch(0, 0, 0, 0, 0, 0, 0, 0, "Minimum Level");
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            MinStkreminder = MinStkreminder + "1 Minimum stock product \n\n";
                        else
                            MinStkreminder = MinStkreminder + dtbl.Rows.Count + "  Minimum stock products \n\n";
                    }

                    dtbl = SpReminder.StockSearch(0, 0, 0, 0, 0, 0, 0, 0, "Maximum Level");
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            MaxStkreminder = MaxStkreminder + "1 Maximum stock product \n\n";
                        else
                            MaxStkreminder = MaxStkreminder + dtbl.Rows.Count + "  Maximum stock products \n\n";
                    }

                    dtbl = SpReminder.StockSearch(0, 0, 0, 0, 0, 0, 0, 0, "Reorder Level");
                    if (dtbl.Rows.Count > 0)
                    {
                        if (dtbl.Rows.Count == 1)
                            ReordrStkreminder = ReordrStkreminder + "1 Reorder Level product \n\n";
                        else
                            ReordrStkreminder = ReordrStkreminder + dtbl.Rows.Count + "  Reorder Level products \n\n";
                    }
                }
                if (Salesreminder.Trim() != string.Empty || Purchasereminder.Trim() != string.Empty || Personalreminder.Trim() != string.Empty || NegativeStkreminder.Trim() != string.Empty || MinStkreminder.Trim() != string.Empty || MaxStkreminder.Trim() != string.Empty || ReordrStkreminder.Trim() != string.Empty)
                {
                    frmReminderPopUp frmReminder = new frmReminderPopUp();
                    frmReminderPopUp isOpen = Application.OpenForms["frmReminderPopUp"] as frmReminderPopUp;
                    if (isOpen == null)
                    {
                        frmReminder.WindowState = FormWindowState.Normal;
                        frmReminder.MdiParent = formMDI.MDIObj;
                        frmReminder.BringToFront();
                        frmReminder.ChangeLabel(Salesreminder, Purchasereminder, Personalreminder, ShortExpiryReminder, NegativeStkreminder, MinStkreminder, MaxStkreminder, ReordrStkreminder);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 13: " + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Form Keydown for Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangeCurrentDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    FormClose();
                }
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 14 : " + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCompanyCurrentdate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 15 : " + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 16 : " + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 17 : " + ex.Message;
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
                    txtCompanyCurrentdate.Focus();
                    txtCompanyCurrentdate.SelectionLength = 0;
                    txtCompanyCurrentdate.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CCD 18 : " + ex.Message;
            }
        }
        #endregion
    }
}
