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
    public partial class frmSalaryPackageDetailsReport : Form
    {
        #region PublicVariables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        decimal decCountAdditon = 0;
        decimal decCountDeduction = 0;
        decimal decTotalAmount = 0;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmSalaryPackageDetailsReport class
        /// </summary>
        public frmSalaryPackageDetailsReport()
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
                SalaryPackageDetailsSP spSalaryPackageDetails = new SalaryPackageDetailsSP();
                DataTable dtbl = spSalaryPackageDetails.SalaryPackageDetailsForSalaryPackageDetailsReport(cmbSalaryPackage.Text);
                dgvSalaryPackageDetails.DataSource = dtbl;
                decCountAdditon = 0;
                decCountDeduction = 0;
                decTotalAmount = 0;
                foreach (DataGridViewRow dgvRaw in dgvSalaryPackageDetails.Rows)
                {
                    if (dgvRaw.Cells["dgvtxttype"].Value.ToString() == "Addition")
                    {
                        decimal decAmt = Convert.ToDecimal(dgvRaw.Cells["txtAmount"].Value.ToString());
                        decCountAdditon = decCountAdditon + decAmt;
                    }
                    else
                    {
                        decimal decAmt = Convert.ToDecimal(dgvRaw.Cells["txtAmount"].Value.ToString());
                        decCountDeduction = decCountDeduction + decAmt;
                    }
                    if (dgvRaw.Cells["txtAmount"].Value != null && dgvRaw.Cells["txtAmount"].Value.ToString() != "")
                    {
                        decimal decAmt = Convert.ToDecimal(dgvRaw.Cells["txtAmount"].Value.ToString());
                        decTotalAmount = decTotalAmount + decAmt;
                    }
                }
                txttotalReduction.Text = Math.Round(decCountDeduction, PublicVariables._inNoOfDecimalPlaces).ToString();
                txttotalAddition.Text = Math.Round(decCountAdditon, PublicVariables._inNoOfDecimalPlaces).ToString(); //Convert.ToString(decCountAdditon);
                txtTotal.Text = Math.Round(decTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString(); //Convert.ToString(decTotalAmount);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:1" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill SalaryPackage combobox
        /// </summary>
        public void SalaryPackageComboFill()
        {
            try
            {
                SalaryPackageSP spSalaryPackage = new SalaryPackageSP();
                DataTable dtbl = spSalaryPackage.SalaryPackageViewAll();
                DataRow dr = dtbl.NewRow();
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbSalaryPackage.DataSource = dtbl;
                cmbSalaryPackage.ValueMember = "salaryPackageId";
                cmbSalaryPackage.DisplayMember = "salaryPackageName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbSalaryPackage.SelectedIndex = 0;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:3" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalaryPackageDetailsReport_Load(object sender, EventArgs e)
        {
            try
            {
                cmbSalaryPackage.Focus();
                SalaryPackageComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:4" + ex.Message;
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
                formMDI.infoError.ErrorString = "SPDR:5" + ex.Message;
            }
        }
        /// <summary>
        /// ON 'Reset' button click
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
                formMDI.infoError.ErrorString = "SPDR:6" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click to Print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSalaryPackageDetails.RowCount > 0)
                {
                    DataTable dtblOther = new DataTable();
                    frmReport frmreport = new frmReport();
                    DataSet ds = new DataSet();
                    CompanySP spCompany = new CompanySP();
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    SalaryPackageDetailsSP spSalaryPackageDetails = new SalaryPackageDetailsSP();
                    DataTable dtbl = spSalaryPackageDetails.SalaryPackageDetailsForSalaryPackageDetailsReport(cmbSalaryPackage.Text);
                    ds.Tables.Add(dtblCompany);
                    ds.Tables.Add(dtbl);
                    ds.Tables.Add(dtblOther);
                    DataColumn dc = new DataColumn("Addition", typeof(decimal));
                    dtblOther.Columns.Add(dc);
                    dc = new DataColumn("Deduction", typeof(decimal));
                    dtblOther.Columns.Add(dc);
                    DataRow dr = dtblOther.NewRow();
                    dr[0] = decCountAdditon;
                    dr[1] = decCountDeduction;
                    dtblOther.Rows.InsertAt(dr, 0);
                    frmreport.MdiParent = formMDI.MDIObj;
                    frmreport.SalaryPackageDetailsReport(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:7" + ex.Message;
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
                ex.ExportExcel(dgvSalaryPackageDetails, "Salary Package Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:8" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalaryPackageDetailsReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SPDR:9" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSalaryPackage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPDR:10" + ex.Message;
            }
        }
        #endregion

    }
}
