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
    public partial class frmShortExpiryReport : Form
    {
  
        #region Functions
        /// <summary>
        /// Create an Instance of a frmShortExpiryReport class
        /// </summary>
        public frmShortExpiryReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the Productgroup combobox
        /// </summary>
        public void ProductGroupComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ProductGroupSP spProductGroup = new ProductGroupSP();
                dtbl = spProductGroup.ProductGroupViewAll();
                DataRow dr = dtbl.NewRow();
                dr["groupName"] = "All";
                dr["groupId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbProductGroup.DataSource = dtbl;
                cmbProductGroup.DisplayMember = "groupName";
                cmbProductGroup.ValueMember = "groupId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the Product combobox
        /// </summary>
        public void ProductNameComboFill()
        {
            try
            {
                ProductSP spproduct = new ProductSP();
                DataTable dtblProductName = new DataTable();
                dtblProductName = spproduct.ProductViewAllForComboBox();
                DataRow dr = dtblProductName.NewRow();
                dr["ProductName"] = "All";
                dr["ProductId"] = 0;
                dtblProductName.Rows.InsertAt(dr, 0);
                cmbProductName.DataSource = dtblProductName;
                cmbProductName.ValueMember = "productId";
                cmbProductName.DisplayMember = "productName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the barnd combobox
        /// </summary>
        public void BrandComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                BrandSP spBrand = new BrandSP();
                dtbl = spBrand.BrandViewAll();
                DataRow dr = dtbl.NewRow();
                dr["brandName"] = "All";
                dr["brandId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbBrand.DataSource = dtbl;
                cmbBrand.DisplayMember = "brandName";
                cmbBrand.ValueMember = "brandId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill ModelNo combobox
        /// </summary>
        public void ModelNoComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ModelNoSP spModelNo = new ModelNoSP();
                dtbl = spModelNo.ModelNoViewAll();
                DataRow dr = dtbl.NewRow();
                dr["modelno"] = "All";
                dr["modelNoId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbModelno.DataSource = dtbl;
                cmbModelno.DisplayMember = "modelNo";
                cmbModelno.ValueMember = "modelNoId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill size combobox
        /// </summary>
        public void SizeComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                SizeSP spSize = new SizeSP();
                dtbl = spSize.SizeViewAll();
                DataRow dr = dtbl.NewRow();
                dr["size"] = "All";
                dr["sizeId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbSize.DataSource = dtbl;
                cmbSize.DisplayMember = "size";
                cmbSize.ValueMember = "sizeId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill godown combobox
        /// </summary>
        public void GodownComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                GodownSP spGodown = new GodownSP();
                dtbl = spGodown.GodownViewAll();
                DataRow dr = dtbl.NewRow();
                dr["godownName"] = "All";
                dr["godownId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbGodown.DataSource = dtbl;
                cmbGodown.DisplayMember = "godownName";
                cmbGodown.ValueMember = "godownId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER6:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill rack combobox
        /// </summary>
        public void RackComboFill()
        {
            try
            {
                RackSP spRack = new RackSP();
                DataTable dtbl = new DataTable();
                dtbl = spRack.RackViewAll();
                DataRow dr = dtbl.NewRow();
                dr["rackName"] = "All";
                dr["rackId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                cmbRack.DataSource = dtbl;
                cmbRack.DisplayMember = "rackName";
                cmbRack.ValueMember = "rackId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the short expiry products
        /// </summary>
        public void GridFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ReminderSP SpRemainder = new ReminderSP();
                decimal decA = Convert.ToDecimal(cmbProductGroup.SelectedValue.ToString());
                decimal decB = Convert.ToDecimal(cmbProductName.SelectedValue.ToString());
                decimal decC = Convert.ToDecimal(cmbBrand.SelectedValue.ToString());
                decimal decD = Convert.ToDecimal(cmbSize.SelectedValue.ToString());
                decimal decE = Convert.ToDecimal(cmbModelno.SelectedValue.ToString());
                decimal decF = Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                decimal decG = Convert.ToDecimal(cmbRack.SelectedValue.ToString());
                decimal decproExp = 0;
                string strproExp = string.Empty;
                if (txtProductExpire.Text != string.Empty)
                {
                    decproExp = Convert.ToDecimal(txtProductExpire.Text.ToString());
                }
                else
                {
                    decproExp = 0;
                }
                if (cmbProductExpire.Text != string.Empty)
                {
                    strproExp = cmbProductExpire.Text;
                }
                else
                {
                    strproExp = string.Empty;
                }
                dtbl = SpRemainder.ShortExpiryReportGridFill(decA, decB, decC, decD, decE, decF, decG, decproExp, strproExp, PublicVariables._dtCurrentDate);
                dgvShortExpiryReport.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void clear()
        {
            try
            {
                cmbBrand.SelectedIndex = 0;
                cmbGodown.SelectedIndex = 0;
                cmbProductName.SelectedIndex = 0;
                cmbProductGroup.SelectedIndex = 0;
                cmbRack.SelectedIndex = 0;
                cmbSize.SelectedIndex = 0;
                cmbModelno.SelectedIndex = 0;
                txtProductExpire.Text = String.Empty;
                cmbProductExpire.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print the report
        /// </summary>
        public void PrintReport()
        {
            try
            {
                ReminderSP SPReminder = new ReminderSP();
                decimal decA = Convert.ToDecimal(cmbProductGroup.SelectedValue.ToString());
                decimal decB = Convert.ToDecimal(cmbProductName.SelectedValue.ToString());
                decimal decC = Convert.ToDecimal(cmbBrand.SelectedValue.ToString());
                decimal decD = Convert.ToDecimal(cmbSize.SelectedValue.ToString());
                decimal decE = Convert.ToDecimal(cmbModelno.SelectedValue.ToString());
                decimal decF = Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                decimal decG = Convert.ToDecimal(cmbRack.SelectedValue.ToString());
                decimal decproExp = 0;
                string strproExp = string.Empty;
                if (txtProductExpire.Text != string.Empty)
                {
                    decproExp = Convert.ToDecimal(txtProductExpire.Text.ToString());
                }
                else
                {
                    decproExp = 0;
                }
                if (cmbProductExpire.Text != string.Empty)
                {
                    strproExp = cmbProductExpire.Text;
                }
                else
                {
                    strproExp = string.Empty;
                }
                DataSet dsShortExpiryReport = SPReminder.ShortExpiryReportPrinting(decA, decB, decC, decD, decE, decF, decG, decproExp, strproExp, PublicVariables._dtCurrentDate, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                if (dgvShortExpiryReport.Rows.Count > 0)
                {
                    frmReport.ShortExpiryReportPrinting(dsShortExpiryReport);
                }
                else
                {
                    Messages.InformationMessage("No Data Found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER10:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Short_Expiry_Report_Load(object sender, EventArgs e)
        {
            try
            {
                txtProductExpire.Focus();
                ProductGroupComboFill();
                ProductNameComboFill();
                BrandComboFill();
                ModelNoComboFill();
                SizeComboFill();
                GodownComboFill();
                RackComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER11:" + ex.Message;
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
                if (txtProductExpire.Text == string.Empty)
                {
                    Messages.InformationMessage("Please enter expiry within");
                    txtProductExpire.Focus();
                }
                else if(cmbProductExpire.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Please select expiry within");
                    cmbProductExpire.Focus();
                }
                else if (Convert.ToDecimal(txtProductExpire.Text) > 7999 && cmbProductExpire.SelectedIndex==2)
                {
                    Messages.InformationMessage("Please enter a valid year");
                    txtProductExpire.Focus();
                }
                else
                {
                    GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER12:" + ex.Message;
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
                txtProductExpire.Focus();
                txtProductExpire.Text = string.Empty;
                cmbProductExpire.SelectedIndex = -1;
                ProductGroupComboFill();
                ProductNameComboFill();
                BrandComboFill();
                ModelNoComboFill();
                SizeComboFill();
                GodownComboFill();
                RackComboFill();
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER13:" + ex.Message;
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
                if (dgvShortExpiryReport.Rows.Count>0)
                {
                PrintReport();
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER14:" + ex.Message;
            }
        }
        /// <summary>
        /// validation for txtProductExpire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductExpire_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.NumberOnly(sender, e);
                txtProductExpire.MaxLength = 4;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER15:" + ex.Message;
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
                ex.ExportExcel(dgvShortExpiryReport, "Short Expiry Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER16:" + ex.Message;
            }
        }
        #endregion
        #region Navigations
        /// <summary>
        /// Enterkey navigation of txtProductExpire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductExpire_KeyDown(object sender, KeyEventArgs e)
        {
            {
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        cmbProductExpire.Focus();
                    }
                }
                catch (Exception ex)
                {
                    formMDI.infoError.ErrorString = "SER17:" + ex.Message;
                }
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbProductExpire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductExpire_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductName.Focus();
                }
                
                else if (e.KeyCode == Keys.Back)
                {
                    txtProductExpire.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER18:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbProductGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroup_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbModelno.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbProductName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER19:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbProductName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductName_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbProductGroup.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbProductExpire.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbBrand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbGodown.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbModelno.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER21:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbModelno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModelno_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBrand.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbProductGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER22:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbSize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbRack.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbGodown.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbGodown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGodown_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSize.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbBrand.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbRack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRack_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbSize.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER25:" + ex.Message;
            }
        }
        /// <summary>
        /// Esc for formclose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmShortExpiryReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "SER26:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of btnSearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cmbRack.Focus();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER27:" + ex.Message;
            }
        }
        /// <summary>
        /// backspace navigation of btnPrint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "SER28:" + ex.Message;
            }
        }
        #endregion
 
    }
}
