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
    public partial class frmMonthlyAttendanceReport : Form
    {
        #region Functions
        /// <summary>
        /// Creates an instance of frmMonthlyAttendanceReport class
        /// </summary>
        public frmMonthlyAttendanceReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Designation combobox
        /// </summary>
        public void DesignationComboFill()
        {
            try
            {
                DesignationSP spDesignation = new DesignationSP();
                DataTable dtbl = spDesignation.DesignationViewAll();
                DataRow dr = dtbl.NewRow();
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbDesigantion.DataSource = dtbl;
                cmbDesigantion.ValueMember = "designationId";
                cmbDesigantion.DisplayMember = "designationName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR1 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                string strEmployeeStatus = string.Empty;
                if (rbtnAll.Checked)
                {
                    strEmployeeStatus = "All";
                }
                else if (rbtnActive.Checked)
                {
                    strEmployeeStatus = "Active";
                }
                else if (rbtnInActive.Checked)
                {
                    strEmployeeStatus = "InActive";
                }
                if (strEmployeeStatus != string.Empty && cmbDesigantion.SelectedIndex > -1)
                {
                    CreateGrid();
                    DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                    EmployeeSP spEmployee = new EmployeeSP();
                    DataTable dtblEmployee = new DataTable();
                    dtblEmployee = spEmployee.EmployeeViewByDesignationAndStatus(cmbDesigantion.Text, strEmployeeStatus);
                    if (dtblEmployee.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtblEmployee.Rows.Count; i++)
                        {
                            dgvMonthlyAttendance.Rows.Add();
                            dgvMonthlyAttendance.Rows[i].Cells["employeeId"].Value = dtblEmployee.Rows[i]["employeeId"].ToString();
                            dgvMonthlyAttendance.Rows[i].Cells["SlNo"].Value = i + 1;
                            dgvMonthlyAttendance.Rows[i].Cells["employeeName"].Value = dtblEmployee.Rows[i]["employeeName"].ToString();
                            dgvMonthlyAttendance.Rows[i].Cells["employeeCode"].Value = dtblEmployee.Rows[i]["employeeCode"].ToString();
                            DataTable dtblStatus = spDailyAttendanceMaster.MonthlyAttendanceReportFill(dtpMonth.Value, Convert.ToDecimal(dtblEmployee.Rows[i]["employeeId"].ToString()));
                            if (dtblStatus.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtblStatus.Rows.Count; j++)
                                {
                                    dgvMonthlyAttendance.Rows[i].Cells[DateTime.Parse(dtblStatus.Rows[j]["date"].ToString()).Day.ToString()].Value = dtblStatus.Rows[j]["status"].ToString();
                                }
                            }
                            FindTotal(i);
                        }
                    }
                    CheckedChange();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR2 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to add columns to Datagridview dynamically
        /// </summary>
        public void CreateGrid()
        {
            try
            {
                dgvMonthlyAttendance.Rows.Clear();
                dgvMonthlyAttendance.Columns.Clear();
                DailyAttendanceMasterSP spDailyAttendanceMaster = new DailyAttendanceMasterSP();
                int inDays = DateTime.DaysInMonth(dtpMonth.Value.Year, dtpMonth.Value.Month);
                dgvMonthlyAttendance.Columns.Add("employeeId", "Employee Id");
                dgvMonthlyAttendance.Columns.Add("SlNo", "SlNO");
                dgvMonthlyAttendance.Columns.Add("employeeCode", "Employee Code");
                dgvMonthlyAttendance.Columns.Add("employeeName", "Employee Name");
                for (int i = 1; i <= inDays; ++i)
                {
                    dgvMonthlyAttendance.Columns.Add(i.ToString(), i.ToString());
                    DateTime dt = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, i);
                }
                dgvMonthlyAttendance.Columns.Add("Present", "Present");
                dgvMonthlyAttendance.Columns.Add("Absent", "Absent");
                dgvMonthlyAttendance.Columns["employeeId"].Visible = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR3 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to find Total days of Present/Absent
        /// </summary>
        /// <param name="inIndex"></param>
        public void FindTotal(int inIndex)
        {
            try
            {
                decimal decPresent = 0;
                decimal decAbsent = 0;
                decimal decHalfDay = 0;
                foreach (DataGridViewColumn dgvCol in dgvMonthlyAttendance.Columns)
                {
                    if (dgvMonthlyAttendance.Rows[inIndex].Cells[dgvCol.Name].Value != null && dgvMonthlyAttendance.Rows[inIndex].Cells[dgvCol.Name].Value.ToString() != string.Empty)
                    {
                        if (dgvMonthlyAttendance.Rows[inIndex].Cells[dgvCol.Name].Value.ToString() == "Present")
                            decPresent++;
                        else if (dgvMonthlyAttendance.Rows[inIndex].Cells[dgvCol.Name].Value.ToString() == "Absent")
                            decAbsent++;
                        else if (dgvMonthlyAttendance.Rows[inIndex].Cells[dgvCol.Name].Value.ToString() == "Half Day")
                            decHalfDay++;
                    }
                }
                if (decHalfDay > 0)
                {
                    decPresent += decHalfDay * 0.5m;
                    decAbsent += decHalfDay * 0.5m;
                }
                dgvMonthlyAttendance.Rows[inIndex].Cells["Present"].Value = decPresent;
                dgvMonthlyAttendance.Rows[inIndex].Cells["Absent"].Value = decAbsent;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR4 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to check the status of Radio buttons and displays Datagridview as Such
        /// </summary>
        public void CheckedChange()
        {
            try
            {
                if (rbtnCondensed.Checked == true)
                {
                    foreach (DataGridViewColumn dgvCol in dgvMonthlyAttendance.Columns)
                    {
                        try
                        {
                            int inCol = Convert.ToInt32(dgvCol.Name);
                            dgvCol.Visible = false;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    dgvMonthlyAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                else if (rbtnDetailed.Checked)
                {
                    foreach (DataGridViewColumn dgvCol in dgvMonthlyAttendance.Columns)
                    {
                        try
                        {
                            int inCol = Convert.ToInt32(dgvCol.Name);
                            dgvCol.Visible = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    dgvMonthlyAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                }
                for (int incolIndex = 0; incolIndex < dgvMonthlyAttendance.Columns.Count; ++incolIndex)//to set columns in the grid not sortable
                    dgvMonthlyAttendance.Columns[incolIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR5 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbDesigantion.SelectedIndex = 0;
                dtpMonth.Value = PublicVariables._dtCurrentDate;
                rbtnCondensed.Checked = true;
                rbtnAll.Checked = true;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR6 " + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMonthlyAttendanceReport_Load(object sender, EventArgs e)
        {
            try
            {
                DesignationComboFill();
                rbtnCondensed.Checked = true;
                rbtnAll.Checked = true;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR7 " + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview as per rbtnCondensed status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnCondensed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR8 " + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview as per rbtnDetailed status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDetailed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR9 " + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview as per rbtnAll status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR10 " + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview as per rbtnActive status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnActive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR11 " + ex.Message;
            }
        }
        /// <summary>
        /// Fills Datagridview as per  rbtnInActive status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnInActive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR12 " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Search'button click fills datagridview
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
                formMDI.infoError.ErrorString = "MAR13 " + ex.Message;
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
                formMDI.infoError.ErrorString = "MAR14 " + ex.Message;
            }
        }
        /// <summary>
        /// ON 'Print' button click to take print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMonthlyAttendance.Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    ds.Tables.Add(dtblCompany);
                    DataTable dtblMonthlyAttendance = new DataTable();
                    dtblMonthlyAttendance.Columns.Add("SlNo", typeof(Int32));
                    dtblMonthlyAttendance.Columns.Add("employeeCode", typeof(string));
                    dtblMonthlyAttendance.Columns.Add("employeeName", typeof(string));
                    dtblMonthlyAttendance.Columns.Add("Present", typeof(string));
                    dtblMonthlyAttendance.Columns.Add("Absent", typeof(string));
                    foreach (DataGridViewRow dgvrow in dgvMonthlyAttendance.Rows)
                    {
                        dtblMonthlyAttendance.Rows.Add();
                        dtblMonthlyAttendance.Rows[dgvrow.Index]["SlNo"] = dgvrow.Cells["SlNo"].Value;
                        dtblMonthlyAttendance.Rows[dgvrow.Index]["employeeCode"] = dgvrow.Cells["employeeCode"].Value;
                        dtblMonthlyAttendance.Rows[dgvrow.Index]["employeeName"] = dgvrow.Cells["employeeName"].Value;
                        dtblMonthlyAttendance.Rows[dgvrow.Index]["Present"] = dgvrow.Cells["Present"].Value;
                        dtblMonthlyAttendance.Rows[dgvrow.Index]["Absent"] = dgvrow.Cells["Absent"].Value;
                    }
                    ds.Tables.Add(dtblMonthlyAttendance);
                    frmReport frmReportObj = new frmReport();
                    frmReportObj.MdiParent = formMDI.MDIObj;
                    frmReportObj.MonthlyAttendancePrinting(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR15 " + ex.Message;
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
                ex.ExportExcel(dgvMonthlyAttendance, "Monthly Attendance Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR16 " + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMonthlyAttendanceReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "MAR17 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDesigantion_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dtpMonth.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR18 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpMonth_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbDesigantion.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR19 " + ex.Message;
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
                    dtpMonth.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "MAR20 " + ex.Message;
            }
        }
        #endregion

    }
}
