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
    public partial class frmSalaryPackageReport : Form
    {
        #region Functions
        /// <summary>
        /// Creates an instance of frmSalaryPackageReport class
        /// </summary>
        public frmSalaryPackageReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill PackageName combobox
        /// </summary>
        public void PackageNameComboFill()
        {
            try
            {
                SalaryPackageSP spSalaryPackage = new SalaryPackageSP();
                DataTable dtbl = spSalaryPackage.SalaryPackageViewAll();
                DataRow dr = dtbl.NewRow();
                dr[1] = "All";
                dtbl.Rows.InsertAt(dr, 0);
                cmbPackageName.DataSource = dtbl;
                cmbPackageName.ValueMember = "salaryPackageId";
                cmbPackageName.DisplayMember = "salaryPackageName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:1 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void GridFill()
        {
            try
            {
                SalaryPackageSP spSalaryPackage = new SalaryPackageSP();
                DataTable dtbl = spSalaryPackage.SalaryPackageViewAllForSalaryPackageReport(cmbPackageName.Text, cmbStatus.Text);
                dgvSalaryPackage.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:2 " + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Clear()
        {
            try
            {
                cmbPackageName.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:3 " + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalaryPackageReport_Load(object sender, EventArgs e)
        {
            try
            {
                cmbStatus.SelectedIndex = 0;
                PackageNameComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:4 " + ex.Message;
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
                formMDI.infoError.ErrorString = "SPR:5 " + ex.Message;
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
                formMDI.infoError.ErrorString = "SPR:6 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPackageName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbStatus.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:7 " + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
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
                    cmbPackageName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:8 " + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click to print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSalaryPackage.RowCount > 0)
                {
                    frmReport frmreport = new frmReport();
                    DataSet ds = new DataSet();
                    SalaryPackageSP spSalaryPackage = new SalaryPackageSP();
                    CompanySP spCompany = new CompanySP();
                    DataTable dtblSalaryPackage = spSalaryPackage.SalaryPackageViewAllForSalaryPackageReport(cmbPackageName.Text, cmbStatus.Text);
                    DataTable dtblCompany = spCompany.CompanyViewDataTable(1);
                    ds.Tables.Add(dtblSalaryPackage);
                    ds.Tables.Add(dtblCompany);
                    frmreport.MdiParent = formMDI.MDIObj;
                    frmreport.SalaryPackageReport(ds);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:9 " + ex.Message;
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
                ex.ExportExcel(dgvSalaryPackage, "Salary Package Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPR:10 " + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalaryPackageReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SPR:11 " + ex.Message;
            }
        }
        #endregion

    }
}
