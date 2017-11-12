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

    public partial class frmSelectCompany : Form
    {
        /// <summary>
        /// Creates an instance of frmSelectCompany class
        /// </summary>
        public frmSelectCompany()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void CompanyGridFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                CompanySP spComapny = new CompanySP();
                dtbl = spComapny.CompanyViewAllForSelectCompany();
                dgvSelectCompany.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SELCMPNY : 1" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Get the Current Date
        /// </summary>
        public void CurrentDate()
        {
            try
            {
                CompanyInfo infoComapany = new CompanyInfo();
                CompanySP spCompany = new CompanySP();
                FinancialYearInfo infoFinancialYear = new FinancialYearInfo();
                FinancialYearSP spFinancialYear = new FinancialYearSP();

                infoComapany = spCompany.CompanyView(1);
                PublicVariables._dtCurrentDate = infoComapany.CurrentDate;
                infoFinancialYear = spFinancialYear.FinancialYearView(1);
                PublicVariables._dtFromDate = infoFinancialYear.FromDate;
                PublicVariables._dtToDate = infoFinancialYear.ToDate;

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SELCMPNY : 2" + ex.Message;
            }
        }

        /// <summary>
        /// Function to call this form from MDI page
        /// </summary>
        public void CallFromMdi()
        {
            try
            {
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SELCMPNY : 3" + ex.Message;
            }
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSelectCompany_Load(object sender, EventArgs e)
        {
            try
            {
                PublicVariables._decCurrentCompanyId = 0;

                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.GetType() == typeof(frmLogin))
                    {

                        frm.Close();
                        break;
                    }
                }
                PublicVariables._decCurrentCompanyId = 0;
                CompanyGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SELCMPNY : 4" + ex.Message;
            }
        }
        /// <summary>
        /// On Datagridview cell double click enables to loginto that company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSelectCompany_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSelectCompany.CurrentRow.Index == e.RowIndex)
                {
                    PublicVariables._decCurrentCompanyId = Convert.ToDecimal(dgvSelectCompany.Rows[e.RowIndex].Cells["dgvtxtCompanyId"].Value.ToString());
                    CurrentDate();
                    frmLogin frmLoginObj = new frmLogin();
                    frmLoginObj.MdiParent = formMDI.MDIObj;
                    frmLoginObj.CallFromSelectCompany(this);
                   
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SELCMPNY : 5" + ex.Message;
            }
        }
        /// <summary>
        /// Logs into selected company on Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSelectCompany_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvSelectCompany.CurrentRow.Index != -1)
                    {
                        PublicVariables._decCurrentCompanyId = Convert.ToDecimal(dgvSelectCompany.SelectedRows[0].Cells["dgvtxtCompanyId"].Value.ToString());
                        CurrentDate();
                        frmLogin frmLoginObj = new frmLogin();
                        frmLoginObj.MdiParent = formMDI.MDIObj;
                        frmLoginObj.CallFromSelectCompany(this);
                    }
                }
                else if (e.KeyCode == Keys.Escape)
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
                formMDI.infoError.ErrorString = "SELCMPNY : 6" + ex.Message;

            }
        }

        private void frmSelectCompany_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SELCMPNY : 7" + ex.Message;

            }
        }

    }
}
