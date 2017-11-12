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
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
namespace One_Account
{
    
    public partial class frmStoredProcedureInserter : Form
    {
        #region Public Variables
        string success = string.Empty;
        string failed = string.Empty;
        public static string PathForExecuter = string.Empty;
        #endregion
        #region Functions
        /// <summary>
        ///  Create instance of frmJournalVoucher
        /// </summary>
        public frmStoredProcedureInserter()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill company path
        /// </summary>
        public void FillDBS()
        {
            
            
            decimal decCompanyId = PublicVariables._decCurrentCompanyId;
            PublicVariables._decCurrentCompanyId = 0;
            CompanyPathSP spCompanyPath = new CompanyPathSP();
            DataTable dtbl = new DataTable();
            try
            {
            dtbl = spCompanyPath.CompanyPathViewAll();
            DataRow dr = dtbl.NewRow();
            dr["companyPath"] = (ConfigurationManager.AppSettings["ApplicationPath"] == null || ConfigurationManager.AppSettings["ApplicationPath"].ToString() == null) ? null : ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "\\Data\\COMP";
            dr["companyId"] = -1;
            dtbl.Rows.InsertAt(dr, 0);
            cbUpdation.BindingContext = new BindingContext();
            cbUpdation.DataSource = dtbl;
            cbUpdation.DisplayMember = "companyPath";
            cbUpdation.ValueMember = "companyId";
            for (int i = 0; i < cbUpdation.Items.Count; ++i)
                cbUpdation.SetItemChecked(i, true);
            PublicVariables._decCurrentCompanyId = decCompanyId;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPIns:1" + ex.Message;
            }
        }
        /// <summary>
        /// Function to execute query from RichTextBox to company path
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public bool ExecuteQueries(RichTextBox txt)
        {
            decimal decCompanyIdForTemp = PublicVariables._decCurrentCompanyId;
            bool iscontinue = true;
            try
            {
            if (cbUpdation.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select DB to execute", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbUpdation.Focus();
            }
            else
            {
                foreach (System.Data.DataRowView p in cbUpdation.CheckedItems)
                {
                    PublicVariables._decCurrentCompanyId = Convert.ToDecimal(p[0].ToString());
                    PathForExecuter = p[2].ToString();
                    
                    CompanySP SpcompanyForExecuter = new CompanySP();
                    if (File.Exists(PathForExecuter + "\\DBOneaccount.mdf"))
                    {
                        string error = SpcompanyForExecuter.StoredProcedureInserter(txtNarration.Text.Trim());
                        if (error.Trim() != string.Empty)
                        {
                            if (MessageBox.Show(PathForExecuter + " :\n" + error + "\n Continue?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                iscontinue = true;
                            }
                            else
                            {
                                iscontinue = false;
                            }
                            if (failed == string.Empty)
                                failed = PathForExecuter;
                            else
                                failed = failed + " , " + PathForExecuter;
                        }
                        else
                        {
                            if (success == "")
                                success = PathForExecuter;
                            else
                                success = success + " , " + PathForExecuter;
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("DB does not exist in " + PathForExecuter + "\n Continue?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            iscontinue = true;
                        }
                        else
                        {
                            iscontinue = false;
                        }
                        if (failed == string.Empty)
                            failed = PathForExecuter;
                        else
                            failed = failed + " , " + PathForExecuter;
                    }
                    if (!iscontinue)
                        break;
                }
            }
            PublicVariables._decCurrentCompanyId = decCompanyIdForTemp;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPIns:2" + ex.Message;
            }
            return iscontinue;
        }
        #endregion
        #region Events
        /// <summary>
        /// On 'CopyData' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyData_Click(object sender, EventArgs e)
        {
            try
            {
            this.Close();
            frmCopyData frmCopyData = new frmCopyData();
            frmCopyData _isOpen = Application.OpenForms["frmCopyData"] as frmCopyData;
            if (_isOpen == null)
            {
                frmCopyData.WindowState = FormWindowState.Normal;
                frmCopyData.MdiParent = formMDI.MDIObj;
                frmCopyData.Show();
            }
            else
            {
                _isOpen.MdiParent = formMDI.MDIObj;
                if (_isOpen.WindowState == FormWindowState.Minimized)
                {
                    _isOpen.WindowState = FormWindowState.Normal;
                }
                if (_isOpen.Enabled)
                {
                    _isOpen.Activate();
                    _isOpen.BringToFront();
                }
            }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPIns:3" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Execute' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExecute_Click(object sender, EventArgs e)
        {
            lblSuccess.Text = string.Empty;
            lblFail.Text = string.Empty;
            success = string.Empty;
            failed = string.Empty;
            btnExecute.Enabled = false;
            try
            {
            if (txtNarration.Text.Trim() != string.Empty)
            {
                cbUpdation.Enabled = false;
                ExecuteQueries(txtNarration);
                txtNarration.Clear();
                if (success != string.Empty)
                    lblSuccess.Text = "Succecc in :" + success;
                if (failed != string.Empty)
                    lblFail.Text = "Failed in :" + failed;
                success = string.Empty;
                failed = string.Empty;
                cbUpdation.Enabled = true;
            }
            btnExecute.Enabled = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPIns:4" + ex.Message;
            }
        }
        /// <summary>
        /// On frmStoredProcedureInserter form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStoredProcedureInserter_Load(object sender, EventArgs e)
        {
            lblSuccess.Text = string.Empty;
            lblFail.Text = string.Empty;
            try
            {
            FillDBS();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPIns:5" + ex.Message;
            }
        }
        /// <summary>
        /// On 'CopyPrinterData' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyPrinterData_Click(object sender, EventArgs e)
        {
            try
            {
            this.Close();
            frmCopyDataPrinter frmCopyData = new frmCopyDataPrinter();
            frmCopyDataPrinter _isOpen = Application.OpenForms["frmCopyDataPrinter"] as frmCopyDataPrinter;
            if (_isOpen == null)
            {
                frmCopyData.WindowState = FormWindowState.Normal;
                frmCopyData.MdiParent = formMDI.MDIObj;
                frmCopyData.Show();
            }
            else
            {
                _isOpen.MdiParent = formMDI.MDIObj;
                if (_isOpen.WindowState == FormWindowState.Minimized)
                {
                    _isOpen.WindowState = FormWindowState.Normal;
                }
                if (_isOpen.Enabled)
                {
                    _isOpen.Activate();
                    _isOpen.BringToFront();
                }
            }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SPIns:6" + ex.Message;
            }
        }
        #endregion
    }
}
