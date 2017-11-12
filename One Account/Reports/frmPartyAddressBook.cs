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
    public partial class frmPartyAddressBook : Form
    {
        #region Function
        /// <summary>
        /// Create an Instance of a frmPartyAddressBook class
        /// </summary>
        public frmPartyAddressBook()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void Gridfill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                if (cmbAccountGroup.Text.Trim() == string.Empty)
                {
                    cmbAccountGroup.Text = "All";
                }
                if (txtMobile.Text.Trim() == string.Empty)
                {
                    txtMobile.Text = "All";
                }
                if (txtPhone.Text.Trim() == string.Empty)
                {
                    txtPhone.Text = "All";
                }
                if (txtEmail.Text.Trim() == string.Empty)
                {
                    txtEmail.Text = "All";
                }
                if (txtLedgerName.Text.Trim() == string.Empty)
                {
                    txtLedgerName.Text = "All";
                }
                if (txtMobile.Text == "All")
                {
                    txtMobile.Text = string.Empty;
                }
                if (txtPhone.Text == "All")
                {
                    txtPhone.Text = string.Empty;
                }
                if (txtEmail.Text == "All")
                {
                    txtEmail.Text = string.Empty;
                }
                if (txtLedgerName.Text == "All")
                {
                    txtLedgerName.Text = string.Empty;
                }
                dtbl = spAccountLedger.PartyAddressBookSearch(cmbAccountGroup.Text, txtMobile.Text, txtPhone.Text, txtEmail.Text, txtLedgerName.Text);
                dvgPartyAddressBook.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbAccountGroup.Text = "All";
                txtMobile.Text = string.Empty;
                txtLedgerName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtEmail.Text = string.Empty;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print partyaddress book
        /// </summary>
        public void Print()
        {
            try
            {
                if (dvgPartyAddressBook.RowCount > 0)
                {
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    frmReport reportobj = new frmReport();
                    AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    DataTable dtbl = spAccountLedger.PartyAddressBookPrint(cmbAccountGroup.Text, txtMobile.Text, txtPhone.Text, txtEmail.Text, txtLedgerName.Text);
                    ds.Tables.Add(dtblCompany);
                    ds.Tables.Add(dtbl);
                    reportobj.MdiParent = formMDI.MDIObj;
                    reportobj.PartyAddressBookPrint(ds);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB3:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Enterkey and Backspace navigation of cmbAccountGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAccountGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtMobile.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbAccountGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB4:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of txtMobile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtLedgerName.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtMobile.Text == string.Empty || txtMobile.SelectionStart == 0)
                    {
                        cmbAccountGroup.Focus();
                        cmbAccountGroup.SelectionLength = 0;
                        cmbAccountGroup.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB5:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of txtPhone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtEmail.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtPhone.Text == string.Empty || txtPhone.SelectionStart == 0)
                    {
                        txtLedgerName.Focus();
                        txtLedgerName.SelectionLength = 0;
                        txtLedgerName.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB6:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of txtLedgerName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLedgerName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPhone.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtLedgerName.Text == string.Empty || txtLedgerName.SelectionStart == 0)
                    {
                        txtMobile.Focus();
                        txtMobile.SelectionLength = 0;
                        txtMobile.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB7:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of txtEmail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtEmail.Text == string.Empty || txtEmail.SelectionStart == 0)
                    {
                        txtPhone.Focus();
                        txtPhone.SelectionLength = 0;
                        txtPhone.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB8:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnReset.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtEmail.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB9:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of btnReset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dvgPartyAddressBook.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB10:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and Backspace navigation of dvgPartyAddressBook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dvgPartyAddressBook_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    dvgPartyAddressBook.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB11:" + ex.Message;
            }
        }

        /// <summary>
        /// On 'Export' button click to export the report to Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportNew ex = new ExportNew();
                ex.ExportExcel(dvgPartyAddressBook, "Party Address Book", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB12:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPartyAddressBook_Load(object sender, EventArgs e)
        {
            try
            {
                cmbAccountGroup.Focus();
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB13:" + ex.Message;
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
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB14:" + ex.Message;
            }
        }
        /// <summary>
        /// On reset button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                cmbAccountGroup.Focus();
                Clear();
                Gridfill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB15:" + ex.Message;
            }
        }
        /// <summary>
        /// On print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dvgPartyAddressBook.Rows.Count > 0)
                {
                    Print();
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB16:" + ex.Message;
            }
        }
        /// <summary>
        /// Esc for formclose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPartyAddressBook_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose)
                    {
                        Messages.CloseMessage(this);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PAB17:" + ex.Message;
            }
        }
        #endregion


    }
}
        
        
       
    
