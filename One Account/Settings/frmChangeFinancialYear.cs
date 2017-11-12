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

    public partial class frmChangeFinancialYear : Form
    {
      

        #region Functions
        /// <summary>
        /// Create An Instance for frmChangeFinancialYear Class
        /// </summary>
        public frmChangeFinancialYear()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to Fill the grid
        /// </summary>
        public void GridFill()
        {
            try
            {
                FinancialYearSP spFinancialYear = new FinancialYearSP();
                DataTable dtbl = new DataTable();

                dtbl = spFinancialYear.FinancialYearViewAll();
                dgvChangeFinancialYear.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGFINYR:1" + ex.Message;
                
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Form Load call the grid fill function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void frmChangeFinancialYear_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (Form child in this.MdiParent.MdiChildren)
                {
                        if (this != child)
                        {
                            child.Close();
                        }
                        GridFill();
                }
                formMDI.MDIObj.ShowQuickLaunchMenu();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGFINYR:2" + ex.Message;
            }
        }
        /// <summary>
        /// Close button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
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
                formMDI.infoError.ErrorString = "CHGFINYR:3" + ex.Message;
            }
        }
        /// <summary>
        /// Grid cell double click, call the Select button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvChangeFinancialYear_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    btnSelect_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGFINYR:4" + ex.Message;
            }
        }
        /// <summary>
        /// Changing company's  financial year
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to change the financial year?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FinancialYearInfo infoFinancialYear = new FinancialYearInfo();
                    FinancialYearSP spFinancialYear = new FinancialYearSP();
                    decimal decFinacialId = Convert.ToDecimal(dgvChangeFinancialYear.CurrentRow.Cells["dgvtxtfinancialYearId"].Value);
                    DateTime dtmFromDate = Convert.ToDateTime(dgvChangeFinancialYear.CurrentRow.Cells["dgvtxtFromDate"].Value);
                    DateTime dtmToDate = Convert.ToDateTime(dgvChangeFinancialYear.CurrentRow.Cells["dgvtxtToDate"].Value);
                    PublicVariables._decCurrentFinancialYearId = decFinacialId;
                    PublicVariables._dtFromDate = dtmFromDate;
                    PublicVariables._dtToDate = dtmToDate;
                    DateTime dtGetCurrentdate = DateTime.Now;
                    CompanySP spCompany = new CompanySP();
                    spCompany.CompanyCurrentDateEdit(dtmFromDate); 
                    if (dtGetCurrentdate < dtmFromDate)
                    {
                        PublicVariables._dtCurrentDate = dtmFromDate;
                        spCompany.CompanyCurrentDateEdit(dtmFromDate); 
                        formMDI.MDIObj.ShowCurrentDate();
                    }
                    else
                    {

                        PublicVariables._dtCurrentDate = dtGetCurrentdate;
                        spCompany.CompanyCurrentDateEdit(dtmFromDate); 
                        formMDI.MDIObj.ShowCurrentDate();
                    }
                    CompanyInfo infoCompany = new CompanyInfo();
                    infoCompany = spCompany.CompanyView(1);
                    formMDI.MDIObj.Text = "Oneaccount " + infoCompany.CompanyName + " [ " + PublicVariables._dtFromDate.ToString("dd-MMM-yyyy") + " To " + PublicVariables._dtToDate.ToString("dd-MMM-yyyy") + " ]";
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGFINYR:5" + ex.Message;
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// Form keydown for Quick access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmChangeFinancialYear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (PublicVariables.isMessageClose == true)
                    {
                        Messages.CloseMessage(this);
                    }
                    else
                    {
                        btnClose_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "CHGFINYR:6" + ex.Message;
            }
        }

        #endregion

      
    }
}
