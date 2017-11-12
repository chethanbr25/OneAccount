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
    public partial class frmEmployeeAddressBook : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        EmployeeSP spEmployee = new EmployeeSP();
        frmEmployeePopup frmEmployeePopupObj;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmEmployeeAddressBook class
        /// </summary>
        public frmEmployeeAddressBook()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                DataTable dtbl = spEmployee.EmployeeViewAllForEmployeeAddressBook(cmbEmployeeCode.Text, txtMobile.Text, txtPhone.Text, txtEmail.Text);
                dgvEmployeeAddressBook.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB1 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill EmployeeCode combobox
        /// </summary>
        public void EmployeeCodeComboFill()
        {
            try
            {
                DataTable dtblEmployee = spEmployee.EmployeeViewAll();
                DataRow dr = dtblEmployee.NewRow();
                dr[3] = "All";
                dtblEmployee.Rows.InsertAt(dr, 0);
                cmbEmployeeCode.DataSource = dtblEmployee;
                cmbEmployeeCode.ValueMember = "employeeId";
                cmbEmployeeCode.DisplayMember = "employeeCode";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB2 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmEmployeePopup to select and view details of Employees
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decEmployeeId"></param>
        public void CallFromEmployeePopUp(frmEmployeePopup frm, decimal decEmployeeId)
        {
            try
            {
                base.Show();
                frmEmployeePopupObj = frm;
                cmbEmployeeCode.SelectedValue = decEmployeeId;
                cmbEmployeeCode.Focus();
                frmEmployeePopupObj.Close();
                frmEmployeePopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB3 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbEmployeeCode.SelectedIndex = 0;
                txtEmail.Text = "";
                txtMobile.Text = "";
                txtPhone.Text = "";
                cmbEmployeeCode.Focus();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB4 " + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEmployeeAddressBook_Load(object sender, EventArgs e)
        {
            try
            {
                EmployeeCodeComboFill();
                GridFill();
                cmbEmployeeCode.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB5 " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button click
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
                formMDI.infoError.ErrorString = "EMP-AB6 " + ex.Message;
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
                formMDI.infoError.ErrorString = "EMP-AB7 " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click to take print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmployeeAddressBook.Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    ds.Tables.Add(dtblCompany);
                    EmployeeSP spEmployee = new EmployeeSP();
                    DataTable dtblEmployee = spEmployee.EmployeeViewAllForEmployeeAddressBook(cmbEmployeeCode.Text, txtMobile.Text, txtPhone.Text, txtEmail.Text);
                    ds.Tables.Add(dtblEmployee);
                    frmReport frmReportObj = new frmReport();
                    frmReportObj.MdiParent = formMDI.MDIObj;
                    frmReportObj.EmployeeAddressBookPrinting(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB8 " + ex.Message;
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
                ex.ExportExcel(dgvEmployeeAddressBook, "Employee Address Book", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB9 " + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEmployeeAddressBook_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "EMP-AB10 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation and Form Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEmployeeCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPhone.Focus();
                }
                if (e.Control && e.KeyCode == Keys.F)
                {
                    frmEmployeePopupObj = new frmEmployeePopup();
                    frmEmployeePopupObj.MdiParent = formMDI.MDIObj;
                    if (cmbEmployeeCode.SelectedIndex > 0)
                    {
                        frmEmployeePopupObj.CallFromEmployeeAddressBook(this, Convert.ToDecimal(cmbEmployeeCode.SelectedValue.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB11 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtMobile.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtPhone.Text == string.Empty || txtPhone.SelectionStart == 0)
                    {
                        cmbEmployeeCode.SelectionLength = 0;
                        cmbEmployeeCode.SelectionStart = 0;
                        cmbEmployeeCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB12 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtEmail.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtMobile.Text == string.Empty || txtMobile.SelectionStart == 0)
                    {
                        txtPhone.SelectionLength = 0;
                        txtPhone.SelectionStart = 0;
                        txtPhone.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB13 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                        txtMobile.SelectionLength = 0;
                        txtMobile.SelectionStart = 0;
                        txtMobile.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB14 " + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtEmail.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "EMP-AB15 " + ex.Message;
            }
        }
        #endregion


    }
}
