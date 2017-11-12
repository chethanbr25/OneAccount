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
    public partial class frmProductStatistics : Form
    {
        #region Public Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        int maxSerialNo = 0;
        public frmProductStatistics()
        {
            InitializeComponent();
        }
        TransactionsGeneralFill TransactionsGeneralFillObj = new TransactionsGeneralFill();
        ProductSP spProduct = new ProductSP();
        #endregion
        #region Functions
        /// <summary>
        /// Function to reset form
        /// </summary>
        public void Reset()
        {
            try
            {
                rbtnNegativeStock.Checked = true;
                ProductStatisticsGridFill();
                cmbProductGroup.Focus();
                ProductGroupComboFill();
                BrandComboFill();
                ModelNoComboFill();
                SizeComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Product group Combobox
        /// </summary>
        public void ProductGroupComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.ProductGroupViewAll(cmbProductGroup, true);
                cmbProductGroup.DataSource = dtbl;
                cmbProductGroup.DisplayMember = "groupName";
                cmbProductGroup.ValueMember = "groupId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill  Brand Combobox
        /// </summary>
        public void BrandComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.BrandViewAll(cmbBrand, true);
                cmbBrand.DataSource = dtbl;
                cmbBrand.DisplayMember = "brandName";
                cmbBrand.ValueMember = "brandId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS3:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill ModelNo Combobox
        /// </summary>
        public void ModelNoComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.ModelNoViewAll(cmbModelNo, true);
                cmbModelNo.DataSource = dtbl;
                cmbModelNo.DisplayMember = "modelNo";
                cmbModelNo.ValueMember = "modelNoId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS4:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill Size Combobox
        /// </summary>
        public void SizeComboFill()
        {
            try
            {
                DataTable dtbl = TransactionsGeneralFillObj.SizeViewAll(cmbSize, true);
                cmbSize.DataSource = dtbl;
                cmbSize.DisplayMember = "size";
                cmbSize.ValueMember = "sizeId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS5:" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to fill Product statistics DatagridView
        /// </summary>
        public void ProductStatisticsGridFill()
        {
            try
            {
                DataTable dtblProduct = new DataTable();
                string strCriteria = string.Empty;
                if (rbtnMaximumLevel.Checked)
                {
                    strCriteria = "Maximum Level";
                }
                if (rbtnMinimumLevel.Checked)
                {
                    strCriteria = "Minimum Level";
                }
                if (rbtnNegativeStock.Checked)
                {
                    strCriteria = "Negative Stock";
                }
                if (rbtnReorderLevel.Checked)
                {
                    strCriteria = "Reorder Level";
                }
                if (rbtnUnused.Checked)
                {
                    strCriteria = "UnUsed";
                }
                if (rbtnFastMovings.Checked)
                {
                    strCriteria = "Fast Movings";
                }
                if (rbtnSlowMovings.Checked)
                {
                    strCriteria = "Slow Movings";
                }
                dtblProduct = spProduct.ProductStatisticsFill(Convert.ToDecimal(cmbBrand.SelectedValue), Convert.ToDecimal(cmbModelNo.SelectedValue), Convert.ToDecimal(cmbSize.SelectedValue), Convert.ToDecimal(cmbProductGroup.SelectedValue),/*decUnitId,*/strCriteria, txtBatchName.Text);
                dgvProductStatistics.DataSource = dtblProduct;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate SerialNo
        /// </summary>
        public void SerialNo()
        {
            try
            {
                int inCount = 1;
                foreach (DataGridViewRow row in dgvProductStatistics.Rows)
                {
                    row.Cells["dgvtxtSlNo"].Value = inCount.ToString();
                    inCount++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS7:" + ex.Message;
            }
        }
        #endregion
        #region Events
       /// <summary>
        ///  On reset button click
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS8:" + ex.Message;
            }
        }
        /// <summary>
        /// On form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductStatistics_Load(object sender, EventArgs e)
        {
            try
            {
                cmbProductGroup.Focus();
                Reset();
                ProductStatisticsGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS9:" + ex.Message;
            }
        }
        /// <summary>
        /// On Search Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ProductStatisticsGridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS10:" + ex.Message;
            }
        }
        /// <summary>
        /// On Print button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductStatistics.RowCount > 0)
                {
                    string strCriteria = string.Empty;
                    if (rbtnMaximumLevel.Checked)
                    {
                        strCriteria = "Maximum Level";
                    }
                    if (rbtnMinimumLevel.Checked)
                    {
                        strCriteria = "Minimum Level";
                    }
                    if (rbtnNegativeStock.Checked)
                    {
                        strCriteria = "Negative Stock";
                    }
                    if (rbtnReorderLevel.Checked)
                    {
                        strCriteria = "Reorder Level";
                    }
                    if (rbtnUnused.Checked)
                    {
                        strCriteria = "UnUsed";
                    }
                    if (rbtnFastMovings.Checked)
                    {
                        strCriteria = "Fast Movings";
                    }
                    if (rbtnSlowMovings.Checked)
                    {
                        strCriteria = "Slow Movings";
                    }
                    DataSet dsProductStatistics = spProduct.ProductStatisticsReport(1, Convert.ToDecimal(cmbBrand.SelectedValue), Convert.ToDecimal(cmbModelNo.SelectedValue), Convert.ToDecimal(cmbSize.SelectedValue), Convert.ToDecimal(cmbProductGroup.SelectedValue),/*decUnitId,*/strCriteria, txtBatchName.Text);
                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.ProductStatisticsReport(dsProductStatistics, strCriteria);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS11:" + ex.Message;
            }
        }
        /// <summary>
        /// On datagridviews rows added generate serial no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductStatistics_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SerialNo();
                maxSerialNo++;
                dgvProductStatistics.Rows[e.RowIndex].Cells["rowId"].Value = maxSerialNo.ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS12:" + ex.Message;
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
                ex.ExportExcel(dgvProductStatistics, "Product Statistics Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS13:" + ex.Message;
            }
        }
        #endregion
        #region Navigations
        /// <summary>
        /// On escape button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductStatistics_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PS14:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbBrand.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS15:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbModelNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbProductGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS16:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModelNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbSize.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbBrand.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS17:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBatchName.Focus();
                    txtBatchName.SelectionStart = 0;
                    txtBatchName.SelectionLength = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbModelNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS18:" + ex.Message;
            }
        }
        /// <summary>
        ///   Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtBatchName.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS19:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProductStatistics.Rows.Count > 0)
                    {
                        dgvProductStatistics.Focus();
                    }
                    else
                    {
                        btnPrint.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnSearch.Focus();
                }
            }
          catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS20:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductStatistics_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int inDgvRejectionOutRowCount = dgvProductStatistics.Rows.Count;
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProductStatistics.CurrentCell == dgvProductStatistics.Rows[inDgvRejectionOutRowCount - 1].Cells["unitName"])
                    {
                        btnPrint.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvProductStatistics.CurrentCell == dgvProductStatistics.Rows[0].Cells["dgvtxtSlno"])
                    {
                        btnReset.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS21:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbModelNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS22:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnReset.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS23:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBatchName_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PS24:" + ex.Message;
            }
        }
        #endregion
    }
}
