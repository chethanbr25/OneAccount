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
    public partial class frmOverdueSalesOrder : Form
    {
        #region  Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        TransactionsGeneralFill TransactionsGeneralFillObj = new TransactionsGeneralFill();
        frmReminderPopUp frmReminderPopupObj;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of  frmOverdueSalesOrder class
        /// </summary>
        public frmOverdueSalesOrder()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Cash/Party combobox
        /// </summary>
        public void AccountLedgerComboFill()
        {
            try
            {
                TransactionsGeneralFillObj.CashOrPartyUnderSundryDrComboFill(cmbAccountLeadger, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void OverDueSalesOrderGridFill()
        {
            try
            {
                ReminderSP spReminder = new ReminderSP();
                if (cmbAccountLeadger.SelectedValue.ToString() != "System.Data.DataRowView" && cmbAccountLeadger.Text != "System.Data.DataRowView")
                {
                    decimal decLedgerId = decimal.Parse(cmbAccountLeadger.SelectedValue.ToString());
                    dgvOverdueSalesOrder.DataSource = spReminder.OverdueSalesOrderCorrespondingAccountLedger(decLedgerId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmReminderPopUp to view details
        /// </summary>
        /// <param name="frmreminder"></param>
        public void CallFromReminder(frmReminderPopUp frmreminder)
        {
            try
            {
                base.Show();
                this.frmReminderPopupObj = frmreminder;
                frmReminderPopupObj.Enabled = false;
                OverDueSalesOrderGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO3:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOverdueSalesOrder_Load(object sender, EventArgs e)
        {
            try
            {
                AccountLedgerComboFill();
                OverDueSalesOrderGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO4:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview on cmbAccountLeadger combobox SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLeadger_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                OverDueSalesOrderGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO5:" + ex.Message;
            }
        }
        /// <summary>
        /// Enables the object of other forms on Formclosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOverdueSalesOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmReminderPopupObj != null)
                {
                    frmReminderPopupObj.Enabled = true;
                    frmReminderPopupObj.Activate();
                    frmReminderPopupObj.BringToFront();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO6:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Close' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnclose_Click(object sender, EventArgs e)
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
                formMDI.infoError.ErrorString = "ODSO7:" + ex.Message;
            }
        }
        #endregion
        # region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOverdueSalesOrder_KeyDown(object sender, KeyEventArgs e)
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO8:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountLeadger_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnclose.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "ODSO9:" + ex.Message;
            }
        }
        #endregion
    }
}
