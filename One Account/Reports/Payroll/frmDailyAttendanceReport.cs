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
    public partial class frmDailyAttendanceReport : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        frmEmployeePopup frmEmployeePopupObj;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmDailyAttendanceReport class
        /// </summary>
        public frmDailyAttendanceReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Designation combobox
        /// </summary>
        private void DesignationComboFill()
        {
            try
            {
                DesignationSP spDesignation = new DesignationSP();
                DataTable dtblDesignation = new DataTable();
                dtblDesignation = spDesignation.DesignationViewAll();
                DataRow dr = dtblDesignation.NewRow();
                dr["designationName"] = "All";
                dtblDesignation.Rows.InsertAt(dr, 0);
                cmbDesignation.DataSource = dtblDesignation;
                cmbDesignation.ValueMember = "designationId";
                cmbDesignation.DisplayMember = "designationName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:1" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Employee combobox
        /// </summary>
        private void EmployeeComboFill()
        {
            try
            {
                EmployeeSP spEmployee = new EmployeeSP();
                DataTable dtblEmployee = new DataTable();
                dtblEmployee = spEmployee.EmployeeViewAll();
                DataRow dr = dtblEmployee.NewRow();
                dr["employeeCode"] = "All";
                dtblEmployee.Rows.InsertAt(dr, 0);
                cmbEmployee.DataSource = dtblEmployee;
                cmbEmployee.DisplayMember = "employeeCode";
                cmbEmployee.ValueMember = "employeeId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmEmployeePopup to view and select Employees
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decEmployeeId"></param>
        public void CallFromEmployeePopUp(frmEmployeePopup frm, decimal decEmployeeId)
        {
            try
            {
                base.Show();
                frmEmployeePopupObj = frm;
                cmbEmployee.SelectedValue = decEmployeeId;
                cmbEmployee.Focus();
                frmEmployeePopupObj.Close();
                frmEmployeePopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                dgvDailyAttendanceReport.DataSource = spDailyAttendanceMaster.DailyAttendanceViewForDailyAttendanceReport(dtpDate.Text.Trim(), cmbStatus.Text.Trim(), cmbEmployee.Text.Trim(), cmbDesignation.Text.Trim());
                dgvDailyAttendanceReport.Columns["employeeId"].Visible = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:4" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMMM-yyyy");
                dtpDate.Value = Convert.ToDateTime(txtDate.Text);
                cmbDesignation.SelectedIndex = 0;
                cmbEmployee.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:5" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailyAttendanceReport_Load(object sender, EventArgs e)
        {
            try
            {
                txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                EmployeeComboFill();
                DesignationComboFill();
                cmbStatus.SelectedIndex = 0;
                txtDate.Focus();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:6" + ex.Message;
            }
        }
        /// <summary>
        /// DateValidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation DateValidationObj = new DateValidation();
                DateValidationObj.DateValidationFunction(txtDate);
                if (txtDate.Text == string.Empty)
                {
                    txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                //---for change date in Date time picker----//
                string strDate = txtDate.Text;
                dtpDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:7" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDailyAttendanceReport.RowCount > 0)
                {
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    ds.Tables.Add(dtblCompany);
                    DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                    DataTable dtblAttendance = spDailyAttendanceMaster.DailyAttendanceViewForDailyAttendanceReport(dtpDate.Text.Trim(), cmbStatus.Text.Trim(), cmbEmployee.Text.Trim(), cmbDesignation.Text.Trim());
                    ds.Tables.Add(dtblAttendance);
                    frmReport frmReportObj = new frmReport();
                    frmReportObj.MdiParent = formMDI.MDIObj;
                    frmReportObj.DailyAttendanceReportPrinting(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:8" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search' button click fills Datagridview
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
                formMDI.infoError.ErrorString = "DAR:9" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Reset' button click resets form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:10" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtDate textbox on dtpDate Datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpDate.Value;
                this.txtDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:11" + ex.Message;
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
                ex.ExportExcel(dgvDailyAttendanceReport, "Daily Attendance Report", 0, 0, "Excel", txtDate.Text, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:12" + ex.Message;
            }
        }
        #endregion
        # region Navigation
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbStatus.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbDesignation.Focus();
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    frmEmployeePopupObj = new frmEmployeePopup();
                    frmEmployeePopupObj.MdiParent = formMDI.MDIObj;
                    if (cmbEmployee.SelectedIndex > 0)
                    {
                        frmEmployeePopupObj.CallFromDailyAttendanceReport(this, Convert.ToDecimal(cmbEmployee.SelectedValue.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:13" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDesignation.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDate.Text == string.Empty || txtDate.SelectionStart == 0)
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:14" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDesignation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbEmployee.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:15" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbEmployee.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:16" + ex.Message;
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
                    cmbStatus.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "DAR:17" + ex.Message;
            }
        }
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDailyAttendanceReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "DAR:18" + ex.Message;
            }
        }
        #endregion

 
    }
}
