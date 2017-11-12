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
    public partial class frmStockReport : Form
    {

        #region Variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        TransactionsGeneralFill TransactionGenericFillObj = new TransactionsGeneralFill();//used to fill product combofill
        ProductSP spproduct = new ProductSP();
        StockPostingSP spStock = new StockPostingSP();
        bool isFormLoad = false;
        #endregion

        #region functions
        /// <summary>
        /// Create an Instance of a frmShortExpiryReport class
        /// </summary>
        public frmStockReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// To fill productgroup combobox
        /// </summary>
        public void ProductGroupComboFill()
        {
            try
            {
                DataTable dtbl = TransactionGenericFillObj.ProductGroupViewAll(cmbProductgroup, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR1:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill modelNo combobox
        /// </summary>
        public void ModelNoComboFill()
        {
            try
            {
                DataTable dtbl = TransactionGenericFillObj.ModelNoViewAll(cmbModel, true);

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR2:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill size combobox
        /// </summary>
        public void SizeComboFill()
        {
            try
            {
                DataTable datbl = TransactionGenericFillObj.SizeViewAll(cmbSize, true);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR3:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill brand combobox
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
                formMDI.infoError.ErrorString = "STKR4:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill godown combobox
        /// </summary>
        public void GodownComboFill()
        {
            try
            {
                GodownSP spGodown = new GodownSP();
                DataTable dtbl = new DataTable();
                dtbl = spGodown.GodownViewAll();
                DataRow drowSelect = dtbl.NewRow();
                drowSelect[0] = 0;
                drowSelect[1] = "All";
                dtbl.Rows.InsertAt(drowSelect, 0);
                cmbGodown.DataSource = dtbl;
                cmbGodown.DisplayMember = "godownName";
                cmbGodown.ValueMember = "godownId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR5:" + ex.Message;
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
                if (cmbGodown.SelectedValue.ToString() != "System.Data.DataRowView")
                {


                    dtbl = spRack.RackViewAllByGodown(Convert.ToDecimal(cmbGodown.SelectedValue.ToString()));

                    DataRow drowSelect = dtbl.NewRow();
                    drowSelect[0] = 0;
                    drowSelect["rackName"] = "All";
                    cmbRack.DataSource = dtbl;
                    cmbRack.DisplayMember = "rackName";
                    cmbRack.ValueMember = "rackId";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR6:" + ex.Message;
            }
        }
       /// <summary>
       /// To fill tax combobox
       /// </summary>
        public void TaxComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TaxSP spTax = new TaxSP();
                dtbl = spTax.TaxViewAllForProduct();
                DataRow drowSelect = dtbl.NewRow();
                drowSelect[0] = 0;
                drowSelect["taxName"] = "All";
                dtbl.Rows.InsertAt(drowSelect, 0);
                cmbTax.DataSource = dtbl;
                cmbTax.ValueMember = "taxId";
                cmbTax.DisplayMember = "taxName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the grid
        /// </summary>
        public void GridFill()
        {
            try
            {

                CurrencyInfo InfoCurrency = new CurrencyInfo();
                CurrencySP SpCurrency = new CurrencySP();
                InfoCurrency = SpCurrency.CurrencyView(1);
                int inDecimalPlaces = InfoCurrency.NoOfDecimalPlaces;
                string calculationMethod = string.Empty;
                SettingsInfo InfoSettings = new SettingsInfo();
                SettingsSP SpSettings = new SettingsSP();
                //--------------- Selection Of Calculation Method According To Settings ------------------// 

                if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "FIFO")
                {
                    calculationMethod = "FIFO";
                }
                else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "Average Cost")
                {
                    calculationMethod = "Average Cost";
                }
                else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "High Cost")
                {
                    calculationMethod = "High Cost";
                }
                else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "Low Cost")
                {
                    calculationMethod = "Low Cost";
                }
                else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "Last Purchase Rate")
                {
                    calculationMethod = "Last Purchase Rate";
                }

                StockPostingSP spstock = new StockPostingSP();
                decimal decrackId = 0;
                DataSet dsstock = new DataSet();
                DataTable dtbl = new DataTable();

                if (cmbRack.SelectedValue != null)
                {
                    decrackId = Convert.ToDecimal(cmbRack.SelectedValue.ToString());
                }

                dtbl = spstock.StockReportGridFill1(txtproductName.Text, Convert.ToDecimal(cmbBrand.SelectedValue.ToString()), Convert.ToDecimal(cmbModel.SelectedValue.ToString()), (txtProductCode.Text), Convert.ToDecimal(cmbGodown.SelectedValue.ToString()), decrackId, Convert.ToDecimal(cmbSize.SelectedValue.ToString()), Convert.ToDecimal(cmbTax.SelectedValue.ToString()), Convert.ToDecimal(cmbProductgroup.SelectedValue.ToString()), txtBatchName.Text);

                if (dtbl.Rows.Count > 0)
                {
                    decimal decTotal = 0;
                    for (int i = 0; i < dtbl.Rows.Count; i++)
                    {
                        if (dtbl.Rows[i]["stockvalue"].ToString() != string.Empty)
                        {
                            decTotal = decTotal + Convert.ToDecimal(dtbl.Rows[i]["stockvalue"].ToString());
                        }

                    }

                    decTotal = Math.Round(decTotal, 2);
                    txtTotal.Text = decTotal.ToString();
                }
                else
                {
                    txtTotal.Text = "0.00";
                }


                dgvStockReport.DataSource = dtbl;
                //if (dtbl.Columns.Count > 0)
                //{
                //    dgvStockReport.Columns["stockvalue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //}

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void Clear()
        {
            try
            {
                BrandComboFill();
                ProductGroupComboFill();
                ModelNoComboFill();
                SizeComboFill();
                GodownComboFill();
                TaxComboFill();
                txtproductName.Text = string.Empty;
                txtProductCode.Text = string.Empty;
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR9:" + ex.Message;
            }
        }
        
        #endregion

        #region Events
        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStockReport_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR10:" + ex.Message;
            }
        }
        /// <summary>
        /// On selected index change of cmbGodown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGodown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbGodown.SelectedIndex > 0)
                {
                    RackComboFill();
                    decimal decGodownId = Convert.ToDecimal(cmbGodown.SelectedValue.ToString());
                }
                else
                {
                    RackComboFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR11:" + ex.Message;
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
                GridFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR12:" + ex.Message;
            }
        }
        /// <summary>
        /// On reset button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnreset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR13:" + ex.Message;
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
                if (dgvStockReport.Rows.Count > 0)
                {
                    decimal decrackId = 0;
                    DataSet dsStockReport = spStock.StockReportPrint(txtproductName.Text, Convert.ToDecimal(cmbBrand.SelectedValue.ToString()), Convert.ToDecimal(cmbModel.SelectedValue.ToString()), txtProductCode.Text, Convert.ToDecimal(cmbGodown.SelectedValue.ToString()), decrackId, Convert.ToDecimal(cmbSize.SelectedValue.ToString()), Convert.ToDecimal(cmbTax.SelectedValue.ToString()), Convert.ToDecimal(cmbProductgroup.SelectedValue.ToString()), txtBatchName.Text);

                    frmReport frmReport = new frmReport();
                    frmReport.MdiParent = formMDI.MDIObj;
                    frmReport.StockReportPrinting(dsStockReport, txtTotal.Text);
                }
                else
                {
                    Messages.InformationMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR14:" + ex.Message;
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
                ex.ExportExcel(dgvStockReport, "Stock Report", 0, 0, "Excel", null, null, "");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR15:" + ex.Message;
            }
        }
        #endregion
        
        #region navigation
        /// <summary>
        /// Esc for formclose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStockReport_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "STKR16:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey navigation of cmbProductgroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductgroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    cmbSize.Focus();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR17:" + ex.Message;
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

                    cmbBrand.Focus();

                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbProductgroup.Focus();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR18:" + ex.Message;
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
                    cmbTax.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbSize.Focus();
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR19:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbTax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    cmbModel.Focus();

                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbBrand.Focus();

                }


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR20:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    cmbRack.Focus();

                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbTax.Focus();

                }




            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR21:" + ex.Message;
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

                    cmbGodown.Focus();

                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbModel.Focus();

                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR22:" + ex.Message;
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

                    txtproductName.Focus();
                    txtproductName.SelectionStart = txtproductName.Text.Length;

                }
                else if (e.KeyCode == Keys.Back)
                {
                    cmbRack.Focus();

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR23:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtproductName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtproductName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtProductCode.SelectionStart = txtProductCode.Text.Length;
                    txtProductCode.Focus();
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtproductName.SelectionStart == 0 || txtproductName.Text == string.Empty)
                    {
                        cmbGodown.Focus();
                    }

                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR24:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtProductCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    txtBatchName.Focus();
                    txtBatchName.SelectionStart = 0;

                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.SelectionStart == 0 || txtProductCode.Text == string.Empty)
                    {

                        txtproductName.Focus();
                        txtproductName.SelectionStart = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR25:" + ex.Message;
            }
        }
        /// <summary>
        /// backspace navigation of btnSearch
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
                formMDI.infoError.ErrorString = "STKR26:" + ex.Message;
            }
        }
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
                    txtproductName.Focus();
                    txtproductName.SelectionStart = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "STKR27:" + ex.Message;
            }
        }
        #endregion

        
    }
}






