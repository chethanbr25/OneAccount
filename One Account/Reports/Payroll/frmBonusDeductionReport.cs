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
    public partial class frmBonusDeductionReport : Form
    {
        #region PublicVariables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strBonusOrDeduction;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmBonusDeductionReport class
        /// </summary>
        public frmBonusDeductionReport()
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
                dr[1] = "All";
                dtblDesignation.Rows.InsertAt(dr, 0);
                cmbDesignation.DataSource = dtblDesignation;
                cmbDesignation.ValueMember = "designationId";
                cmbDesignation.DisplayMember = "designationName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:1" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill employee Combobox
        /// </summary>
        private void EmployeeComboFill()
        {
            try
            {
                EmployeeSP spEmployee = new EmployeeSP();
                DataTable dtblEmployee = new DataTable();
                dtblEmployee = spEmployee.EmployeeViewAll();
                DataRow dr = dtblEmployee.NewRow();
                dr[3] = "All";
                dtblEmployee.Rows.InsertAt(dr, 0);
                cmbEmployee.DataSource = dtblEmployee;
                cmbEmployee.DisplayMember = "employeeCode";
                cmbEmployee.ValueMember = "employeeId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        private void GridFill()
        {
            try
            {
                if (rbtnBonus.Checked)
                {
                    strBonusOrDeduction = rbtnBonus.Text;
                }
                else if (rbtnDeduction.Checked)
                {
                    strBonusOrDeduction = rbtnDeduction.Text;
                }
                BonusDedutionSP spBonusDeduction = new BonusDedutionSP();
                dgvBonusDeductionReport.DataSource = spBonusDeduction.BonusDeductionReportGridFill(txtDate.Text, txtTodate.Text, dtpSalaryMonth.Text, cmbDesignation.Text, cmbEmployee.Text, strBonusOrDeduction);
                dgvBonusDeductionReport.Columns["employeeId"].Visible = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate the Total Amount Paid
        /// </summary>
        private void TotalAmount()
        {
            try
            {
                decimal decAmount = 0;
                for (int i = 0; i < dgvBonusDeductionReport.RowCount; i++)
                {
                    decAmount = decAmount + Convert.ToDecimal(dgvBonusDeductionReport.Rows[i].Cells["amount"].Value);
                }
                txtTotalAmount.Text = Math.Round(decAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:4" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBonusDeductionReport_Load(object sender, EventArgs e)
        {
            try
            {
                dtpSalaryMonth.Value = PublicVariables._dtCurrentDate;
                txtDate.Text = PublicVariables._dtFromDate.ToString("dd-MMM-yyyy");
                txtTodate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                rbtnBonus.Checked = true;
                EmployeeComboFill();
                DesignationComboFill();
                GridFill();
                TotalAmount();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:5" + ex.Message;
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
                TotalAmount();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:6" + ex.Message;
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
                dtpFromDate.Value = PublicVariables._dtFromDate;
                dtpToDate.Value = PublicVariables._dtToDate;
                dtpSalaryMonth.Value = PublicVariables._dtCurrentDate;
                cmbDesignation.SelectedIndex = 0;
                cmbEmployee.SelectedIndex = 0;
                rbtnBonus.Checked = true;
                GridFill();
                TotalAmount();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:7" + ex.Message;
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
                if (dgvBonusDeductionReport.RowCount > 0)
                {
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    ds.Tables.Add(dtblCompany);
                    BonusDedutionSP spBonusDeduction = new BonusDedutionSP();
                    DataTable dtblBonusDeduction = spBonusDeduction.BonusDeductionReportGridFill(dtpFromDate.Text, dtpToDate.Text, dtpSalaryMonth.Text, cmbDesignation.Text, cmbEmployee.Text, strBonusOrDeduction);
                    ds.Tables.Add(dtblBonusDeduction);
                    frmReport frmReportObj = new frmReport();
                    frmReportObj.MdiParent = formMDI.MDIObj;
                    frmReportObj.BonusDeductionReportPrinting(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:8" + ex.Message;
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
                dtpFromDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:9" + ex.Message;
            }
        }
        /// <summary>
        /// DateValidation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTodate_Leave(object sender, EventArgs e)
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
                dtpToDate.Value = Convert.ToDateTime(strDate.ToString());
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:10" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtDate textbox on dtpFromDate Datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpFromDate.Value;
                this.txtDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:11" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtTodate textbox on dtpToDate Datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpToDate.Value;
                this.txtTodate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:12" + ex.Message;
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
                ex.ExportExcel(dgvBonusDeductionReport, "Bonus Deduction Report", 0, 0,"Excel", txtDate.Text, txtTodate.Text, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:13" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBonusDeductionReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "BDR:14" + ex.Message;
            }
        }
        private void txtTodate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dtpSalaryMonth.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtTodate.Text == string.Empty || txtTodate.SelectionStart == 0)
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:15" + ex.Message;
            }
        }
        private void dtpSalaryMonth_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbDesignation.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtTodate.Focus();
                    txtTodate.SelectionStart = 0;
                    txtTodate.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:16" + ex.Message;
            }
        }
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
                    dtpSalaryMonth.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:17" + ex.Message;
            }
        }
        private void cmbEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbDesignation.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:18" + ex.Message;
            }
        }
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtTodate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BDR:19" + ex.Message;
            }
        }
        #endregion

       
    }
}
