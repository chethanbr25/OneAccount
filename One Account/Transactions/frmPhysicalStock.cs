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
using System.Collections;
namespace One_Account
{
    public partial class frmPhysicalStock : Form
    {
        #region Public variables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        SettingsSP spSettings = new SettingsSP();
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        PhysicalStockMasterInfo infoPhysicalStockMaster = new PhysicalStockMasterInfo();
        frmProductSearchPopup frmProductSearchPopupObj = new frmProductSearchPopup();
        frmDayBook frmDayBookObj = null;//To use in call from frmDayBook
        frmPhysicalStockRegister frmPhysicalStockRegisterObj = null;
        frmPhysicalStockReport frmPhysicalStockReportObj = null;
        frmVoucherSearch objfrmVoucherSearch = null;
        frmVoucherWiseProductSearch objfrmVoucherProduct = null;
        ArrayList arrlstMasterId = new ArrayList();
        ArrayList lstArrOfRemove = new ArrayList();
        ArrayList arrlstOfRemove = new ArrayList();//to get the removed rows physicalStockDetailsId
        int inArrOfRemove = 0;//number of rows removed by clicking remove button
        int inNarrationCount = 0;
        decimal decPhysicalStockVoucherTypeId = 0;//to get the selected voucher type id from frmVoucherTypeSelection
        decimal decConversionId = 0;
        decimal decBatchId = 0;
        decimal decPhysicalStockMasterIdentity = 0;//Id of Physicalstock master
        decimal decPhysicalStockSuffixPrefixId = 0;
        decimal decDailySuffixPrefixId = 0;//to store the selected voucher type's suffixpreffixid from frmVoucherTypeSelection
        decimal decMasterId = 0;//to get physical stock master id when viewing from register
        string strTableName = "PhysicalStockMaster";
        string strVoucherNo = string.Empty;
        string strPrefix = string.Empty;//to get the prefix string from frmvouchertypeselection
        string strSuffix = string.Empty;//to get the suffix string from frmvouchertypeselection
        string strProductCode = string.Empty;
        bool isAutomatic = false;//to check whether the voucher number is automatically generated or not
        bool isAmountcalc = true;
        bool isValueChanged = false;
        bool isGridValueChanged = true;
        bool isFromEditMode = false;
        DataTable dtblunitconversionViewAll = new DataTable();
        DataGridViewTextBoxEditingControl TextBoxControl;
        AutoCompleteStringCollection ProductNames = new AutoCompleteStringCollection();
        AutoCompleteStringCollection ProductCodes = new AutoCompleteStringCollection();
        PhysicalStockDetailsInfo infoPhysicalStockDetails = new PhysicalStockDetailsInfo();
        string strProductCodetoFill = string.Empty;
        #endregion
        #region Functions
        /// <summary>
        /// Function to call this form from frmPhysicalStockRegister to view details and for updation
        /// </summary>
        /// <param name="frmPhysicalStockRegister"></param>
        /// <param name="decId"></param>
        public void View(frmPhysicalStockRegister frmPhysicalStockRegister, decimal decId)
        {
            try
            {
                base.Show();
                this.frmPhysicalStockRegisterObj = frmPhysicalStockRegister;
                frmPhysicalStockRegisterObj.Enabled = false;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtVoucherNo.ReadOnly = true;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS1:" + ex.Message;
            }
        }
        /// <summary>
        /// Print function for crystel report
        /// </summary>
        /// <param name="decPhysicalStockMasterId"></param>
        public void Print(decimal decPhysicalStockMasterId)
        {
            try
            {
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                DataSet dsPhysicalStock = spPhysicalStockMaster.PhysicalStockPrinting(decPhysicalStockMasterId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.PhysicalStockPrinting(dsPhysicalStock);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS2:" + ex.Message;
            }
        }
        /// <summary>
        /// print function for dotmatrix printer
        /// </summary>
        /// <param name="decPhysicalStockMasterId"></param>
        public void PrintForDotMatrix(decimal decPhysicalStockMasterId)
        {
            try
            {
                DataTable dtblOtherDetails = new DataTable();
                CompanySP spComapany = new CompanySP();
                dtblOtherDetails = spComapany.CompanyViewForDotMatrix();
                DataTable dtblGridDetails = new DataTable();
                dtblGridDetails.Columns.Add("SlNo");
                dtblGridDetails.Columns.Add("BarCode");
                dtblGridDetails.Columns.Add("ProductCode");
                dtblGridDetails.Columns.Add("ProductName");
                dtblGridDetails.Columns.Add("Qty");
                dtblGridDetails.Columns.Add("Unit");
                dtblGridDetails.Columns.Add("Godown");
                dtblGridDetails.Columns.Add("Rack");
                dtblGridDetails.Columns.Add("Batch");
                dtblGridDetails.Columns.Add("Rate");
                dtblGridDetails.Columns.Add("Amount");
                int inRowCount = 0;
                foreach (DataGridViewRow dRow in dgvPhysicalStock.Rows)
                {
                    if (!dRow.IsNewRow)
                    {
                        DataRow dr = dtblGridDetails.NewRow();
                        dr["SlNo"] = ++inRowCount;
                        if (dRow.Cells["dgvtxtBarcode"].Value != null)
                        {
                            dr["BarCode"] = dRow.Cells["dgvtxtBarcode"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtProductCode"].Value != null)
                        {
                            dr["ProductCode"] = dRow.Cells["dgvtxtProductCode"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtProductName"].Value != null)
                        {
                            dr["ProductName"] = dRow.Cells["dgvtxtProductName"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtQty"].Value != null)
                        {
                            dr["Qty"] = dRow.Cells["dgvtxtQty"].Value.ToString();
                        }
                        if (dRow.Cells["dgvcmbUnit"].Value != null)
                        {
                            dr["Unit"] = dRow.Cells["dgvcmbUnit"].FormattedValue.ToString();
                        }
                        if (dRow.Cells["dgvcmbGodown"].Value != null)
                        {
                            dr["Godown"] = dRow.Cells["dgvcmbGodown"].FormattedValue.ToString();
                        }
                        if (dRow.Cells["dgvcmbRack"].Value != null)
                        {
                            dr["Rack"] = dRow.Cells["dgvcmbRack"].FormattedValue.ToString();
                        }
                        if (dRow.Cells["dgvcmbBatch"].Value != null)
                        {
                            dr["Batch"] = dRow.Cells["dgvcmbBatch"].FormattedValue.ToString();
                        }
                        if (dRow.Cells["dgvtxtRate"].Value != null)
                        {
                            dr["Rate"] = dRow.Cells["dgvtxtRate"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtAmount"].Value != null)
                        {
                            dr["Amount"] = dRow.Cells["dgvtxtAmount"].Value.ToString();
                        }
                        dtblGridDetails.Rows.Add(dr);
                    }
                }
                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("TotalAmount");
                dtblOtherDetails.Columns.Add("AmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtVoucherNo.Text;
                dRowOther["date"] = txtDate.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["TotalAmount"] = txtTotalAmount.Text;
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtTotalAmount.Text), PublicVariables._decCurrencyId);
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decPhysicalStockVoucherTypeId);
                dRowOther["Declaration"] = dtblDeclaration.Rows[0]["Declaration"].ToString();
                dRowOther["Heading1"] = dtblDeclaration.Rows[0]["Heading1"].ToString();
                dRowOther["Heading2"] = dtblDeclaration.Rows[0]["Heading2"].ToString();
                dRowOther["Heading3"] = dtblDeclaration.Rows[0]["Heading3"].ToString();
                dRowOther["Heading4"] = dtblDeclaration.Rows[0]["Heading4"].ToString();
                int inFormId = spVoucherType.FormIdGetForPrinterSettings(Convert.ToInt32(dtblDeclaration.Rows[0]["masterId"].ToString()));
                DotMatrixPrint.PrintDesign(inFormId, dtblOtherDetails, dtblGridDetails, dtblOtherDetails);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS3:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete function
        /// </summary>
        /// <param name="decPhysicalStockId"></param>
        public void DeleteFunction(decimal decPhysicalStockId)
        {
            try
            {
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                spPhysicalStockMaster.PhysicalStockDelete(decPhysicalStockId, decPhysicalStockVoucherTypeId, strVoucherNo);
                Messages.DeletedMessage();
                if (objfrmVoucherSearch != null)
                {
                    this.Close();
                    objfrmVoucherSearch.GridFill();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPhysicalStockReport to view details and for updation
        /// </summary>
        /// <param name="frmPhysicalStockReportObj"></param>
        /// <param name="decId"></param>
        public void CallFromPhysicalStockReport(frmPhysicalStockReport frmPhysicalStockReportObj, decimal decId)
        {
            try
            {
                base.Show();
                this.frmPhysicalStockReportObj = frmPhysicalStockReportObj;
                frmPhysicalStockReportObj.Enabled = false;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtVoucherNo.Enabled = false;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS5:" + ex.Message;
            }
        }
        /// <summary>
        ///   Function to call this form from frmVoucherWiseProductSearch to view details and for updation
        /// </summary>
        /// <param name="frmVoucherwiseProductSearch"></param>
        /// <param name="decmasterId"></param>
        public void CallFromVoucherWiseProductSearch(frmVoucherWiseProductSearch frmVoucherwiseProductSearch, decimal decmasterId)
        {
            try
            {
                base.Show();
                frmVoucherwiseProductSearch.Enabled = false;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtVoucherNo.Enabled = false;
                objfrmVoucherProduct = frmVoucherwiseProductSearch;
                decMasterId = decmasterId;
                arrlstMasterId.Add(decMasterId);
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVoucherSearch to view details and for updation
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallFromVoucherSearch(frmVoucherSearch frm, decimal decId)
        {
            try
            {
                this.objfrmVoucherSearch = frm;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS7:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill function for updation
        /// </summary>
        public void FillFunction()
        {
            try
            {
                PhysicalStockMasterInfo infoPhysicalStockMaster = new PhysicalStockMasterInfo();
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                infoPhysicalStockMaster = spPhysicalStockMaster.PhysicalStockMasterView(decMasterId);
                txtVoucherNo.Text = infoPhysicalStockMaster.InvoiceNo;
                strVoucherNo = infoPhysicalStockMaster.VoucherNo.ToString();
                decPhysicalStockSuffixPrefixId = Convert.ToDecimal(infoPhysicalStockMaster.SuffixPrefixId);
                decPhysicalStockVoucherTypeId = Convert.ToDecimal(infoPhysicalStockMaster.VoucherTypeId);
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPhysicalStockVoucherTypeId);
                txtDate.Text = infoPhysicalStockMaster.Date.ToString("dd-MMM-yyyy");
                txtNarration.Text = infoPhysicalStockMaster.Narration;
                txtTotalAmount.Text = infoPhysicalStockMaster.TotalAmount.ToString();
                DataTable dtbl = new DataTable();
                dtbl = spPhysicalStockMaster.PhysicalStockViewbyMasterId(decMasterId);
                isFromEditMode = true;
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {

                    dgvPhysicalStock.Rows.Add();
                    dgvPhysicalStock.Rows[i].HeaderCell.Value = string.Empty;
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtPhysicalStockDetailId"].Value = Convert.ToDecimal(dtbl.Rows[i]["physicalStockDetailsId"].ToString());
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtSlNo"].Value = dtbl.Rows[i]["slno"].ToString();
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtProductCode"].Value = dtbl.Rows[i]["productCode"].ToString();
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtProductName"].Value = dtbl.Rows[i]["productName"].ToString();
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtQty"].Value = dtbl.Rows[i]["qty"].ToString();
                    BatchComboFill(Convert.ToDecimal(dtbl.Rows[i]["productId"].ToString()), i, dgvPhysicalStock.Rows[i].Cells["dgvcmbBatch"].ColumnIndex);
                    dgvPhysicalStock.Rows[i].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(dtbl.Rows[i]["batchId"].ToString());
                    dgvPhysicalStock.Rows[i].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(dtbl.Rows[i]["unitId"].ToString());
                    dgvPhysicalStock.Rows[i].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(dtbl.Rows[i]["godownId"].ToString());
                    dgvPhysicalStock.Rows[i].Cells["dgvcmbRack"].Value = Convert.ToDecimal(dtbl.Rows[i]["rackId"].ToString());
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtRate"].Value = dtbl.Rows[i]["rate"].ToString();
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtAmount"].Value = dtbl.Rows[i]["amount"].ToString();
                    dgvPhysicalStock.Rows[i].Cells["dgvtxtBarcode"].Value = dtbl.Rows[i]["barcode"].ToString();
                    if (dgvPhysicalStock.Columns.Count > 0)
                    {
                        dgvPhysicalStock.Columns["dgvtxtRate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvPhysicalStock.Columns["dgvtxtAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                isFromEditMode = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from VoucherType Selection form
        /// </summary>
        /// <param name="DecPhysicalStockVoucherType"></param>
        /// <param name="strPhysicalStockVouchertypeName"></param>
        public void CallFromVoucherTypeSelection(decimal DecPhysicalStockVoucherType, string strPhysicalStockVouchertypeName)
        {
            try
            {
                decPhysicalStockVoucherTypeId = DecPhysicalStockVoucherType;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPhysicalStockVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPhysicalStockVoucherTypeId, dtpDate.Value);
                decDailySuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                strPrefix = infoSuffixPrefix.Prefix;
                strSuffix = infoSuffixPrefix.Suffix;
                this.Text = strPhysicalStockVouchertypeName;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS9:" + ex.Message;
            }
        }
        /// <summary>
        /// Form dettings check based on the settings
        /// </summary>
        public void SettingsCheck()
        {
            try
            {
                if (spSettings.SettingsStatusCheck("ShowProductCode") == "Yes")
                {
                    dgvPhysicalStock.Columns["dgvtxtProductCode"].Visible = true;
                }
                else
                {
                    dgvPhysicalStock.Columns["dgvtxtProductCode"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("AllowGodown") == "Yes")
                {
                    dgvPhysicalStock.Columns["dgvcmbGodown"].Visible = true;
                }
                else
                {
                    dgvPhysicalStock.Columns["dgvcmbGodown"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("AllowRack") == "Yes")
                {
                    dgvPhysicalStock.Columns["dgvcmbRack"].Visible = true;
                }
                else
                {
                    dgvPhysicalStock.Columns["dgvcmbRack"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("AllowBatch") == "Yes")
                {
                    dgvPhysicalStock.Columns["dgvcmbBatch"].Visible = true;
                }
                else
                {
                    dgvPhysicalStock.Columns["dgvcmbBatch"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("Barcode") == "Yes")
                {
                    dgvPhysicalStock.Columns["dgvtxtBarcode"].Visible = true;
                }
                else
                {
                    dgvPhysicalStock.Columns["dgvtxtBarcode"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("ShowUnit") == "Yes")
                {
                    dgvPhysicalStock.Columns["dgvcmbUnit"].Visible = true;
                }
                else
                {
                    dgvPhysicalStock.Columns["dgvcmbUnit"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("TickPrintAfterSave") == "Yes")
                {
                    cbxPrint.Checked = true;
                }
                else
                {
                    cbxPrint.Checked = false;
                }
                if (isAutomatic)
                {
                    VoucherNumberGeneration();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS10:" + ex.Message;
            }
        }
        /// <summary>
        /// Create an instance for frmPhysicalStock class
        /// </summary>
        public frmPhysicalStock()
        {
            InitializeComponent();
        }
        /// <summary>
        /// To fill godown field of grid
        /// </summary>
        public void GodownComboFill()
        {
            try
            {
                GodownSP spGodown = new GodownSP();
                DataTable dtblGodown = new DataTable();
                dtblGodown = spGodown.GodownViewAll();
                dgvcmbGodown.DataSource = dtblGodown;
                dgvcmbGodown.ValueMember = "godownId";
                dgvcmbGodown.DisplayMember = "godownName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS11:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill unit field of grid
        /// </summary>
        public void UnitComboFill()
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                DataTable dtblUnit = new DataTable();
                dtblUnit = spUnit.UnitViewAll();
                dgvcmbUnit.DataSource = dtblUnit;
                dgvcmbUnit.ValueMember = "unitId";
                dgvcmbUnit.DisplayMember = "unitName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS12:" + ex.Message;
            }
        }
        /// <summary>
        /// To fill rack field of grid
        /// </summary>
        public void RackAllComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                RackSP spRack = new RackSP();
                dtbl = spRack.RackViewAll();
                dgvcmbRack.DataSource = dtbl;
                dgvcmbRack.ValueMember = "rackId";
                dgvcmbRack.DisplayMember = "rackName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS13:" + ex.Message;
            }
        }
        /// <summary>
        /// Setting the product code and name for grid quick access
        /// </summary>
        /// <param name="isProductName"></param>
        /// <param name="editControl"></param>
        public void FillProducts(bool isProductName, DataGridViewTextBoxEditingControl editControl)
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                DataTable dtblProducts = new DataTable();
                dtblProducts = spProduct.ProductViewAll();
                ProductNames = new AutoCompleteStringCollection();
                ProductCodes = new AutoCompleteStringCollection();
                foreach (DataRow dr in dtblProducts.Rows)
                {
                    ProductNames.Add(dr["productName"].ToString());
                    ProductCodes.Add(dr["productCode"].ToString());
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS14:" + ex.Message;
            }
        }
        /// <summary>
        /// CHeck the settings status for print after save checkbox
        /// </summary>
        /// <returns></returns>
        public bool PrintAfetrSave()
        {
            bool isTick = false;
            try
            {
                isTick = TransactionGeneralFillObj.StatusOfPrintAfterSave();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS15:" + ex.Message;
            }
            return isTick;
        }
        /// <summary>
        /// Serial no generation for gridview
        /// </summary>
        public void SerialNo()
        {
            try
            {
                int inRowSlNo = 1;
                foreach (DataGridViewRow dr in dgvPhysicalStock.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowSlNo;
                    inRowSlNo++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS16:" + ex.Message;
            }
        }
        /// <summary>
        /// Amount calculation for grid
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="inIndexOfRow"></param>
        public void NewAmountCalculation(int inIndexOfRow)
        {
            try
            {
                decimal decRate = 0;
                decimal decQty = 0;
                decimal decGrossValue = 0;
                if (dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtProductName"].Value != null && dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtProductName"].Value.ToString() != string.Empty)
                {
                    if (dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtQty"].Value != null)
                    {
                        decimal.TryParse(Convert.ToString(dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtQty"].Value), out decQty);
                    }
                    if (dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtRate"].Value != null)
                    {
                        decimal.TryParse(Convert.ToString(dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtRate"].Value), out decRate);
                    }
                    decGrossValue = decQty * decRate;
                    dgvPhysicalStock.Rows[inIndexOfRow].Cells["dgvtxtAmount"].Value = Math.Round(decGrossValue, PublicVariables._inNoOfDecimalPlaces);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS17:" + ex.Message;
            }
        }
        /// <summary>
        /// Voucher no generation function based on settings
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                if (strVoucherNo == string.Empty)
                {
                    strVoucherNo = "0";
                }
                strVoucherNo = TransactionGeneralFillObj.VoucherNumberAutomaicGeneration(decPhysicalStockVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, strTableName);
                if (Convert.ToDecimal(strVoucherNo) != (spPhysicalStockMaster.PhysicalStockMasterVoucherMax(decPhysicalStockVoucherTypeId)))
                {
                    strVoucherNo = spPhysicalStockMaster.PhysicalStockMasterVoucherMax(decPhysicalStockVoucherTypeId).ToString();
                    strVoucherNo = TransactionGeneralFillObj.VoucherNumberAutomaicGeneration(decPhysicalStockVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, strTableName);
                    if (spPhysicalStockMaster.PhysicalStockMasterVoucherMax(decPhysicalStockVoucherTypeId) == 0)
                    {
                        strVoucherNo = "0";
                        strVoucherNo = TransactionGeneralFillObj.VoucherNumberAutomaicGeneration(decPhysicalStockVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, strTableName);
                    }
                }
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPhysicalStockVoucherTypeId, dtpDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    decPhysicalStockSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                    txtVoucherNo.Text = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS18:" + ex.Message;
            }
        }
        /// <summary>
        /// The form will be reset here
        /// </summary>
        public void clear()
        {
            try
            {
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                txtNarration.Clear();
                txtTotalAmount.Clear();
                txtTotalAmount.Text = "0";
                dgvPhysicalStock.Rows.Clear();
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.Value = PublicVariables._dtCurrentDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                if (isAutomatic)
                {
                    VoucherNumberGeneration();
                }
                if (!txtVoucherNo.ReadOnly)
                {
                    txtVoucherNo.Clear();
                    txtVoucherNo.Focus();
                }
                else
                {
                    txtDate.Select();
                }
                SettingsCheck();
                GodownComboFill();
                UnitComboFill();
                RackAllComboFill();
                dtpDate.Value = PublicVariables._dtCurrentDate;
                FillProducts(false, null);
                txtTotalAmount.Text = "0";
                SerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS19:" + ex.Message;
            }
        }
        /// <summary>
        /// Edit function
        /// </summary>
        public void EditFunction()
        {
            try
            {

                PhysicalStockDetailsSP spPhysicalStockDetails = new PhysicalStockDetailsSP();
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                infoPhysicalStockMaster.PhysicalStockMasterId = decMasterId;
                infoPhysicalStockMaster.VoucherNo = txtVoucherNo.Text.Trim();
                infoPhysicalStockMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoPhysicalStockMaster.Narration = txtNarration.Text.Trim();
                infoPhysicalStockMaster.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                if (!isAutomatic)
                {
                    infoPhysicalStockMaster.SuffixPrefixId = decPhysicalStockSuffixPrefixId;
                    infoPhysicalStockMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoPhysicalStockMaster.SuffixPrefixId = 0;
                    infoPhysicalStockMaster.VoucherNo = txtVoucherNo.Text;
                }
                infoPhysicalStockMaster.VoucherTypeId = decPhysicalStockVoucherTypeId;
                infoPhysicalStockMaster.InvoiceNo = txtVoucherNo.Text;
                infoPhysicalStockMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoPhysicalStockMaster.Extra1 = string.Empty;
                infoPhysicalStockMaster.Extra2 = string.Empty;
                infoPhysicalStockMaster.ExtraDate = DateTime.Now;
                spPhysicalStockMaster.PhysicalStockMasterEdit(infoPhysicalStockMaster);
                spPhysicalStockDetails.PhysicalStockDetailsDeleteWhenUpdate(decMasterId);
                EditPhysicalStockDetails();
                Messages.UpdatedMessage();
                if (frmPhysicalStockRegisterObj != null)
                {
                    if (cbxPrint.Checked)
                    {
                        if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(decMasterId);
                        }
                        else
                        {
                            Print(decMasterId);
                        }
                    }
                    this.Close();
                    frmPhysicalStockRegisterObj.gridfill();
                }
                if (frmPhysicalStockReportObj != null)
                {
                    if (cbxPrint.Checked)
                    {
                        if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(decMasterId);
                        }
                        else
                        {
                            Print(decMasterId);
                        }
                    }
                    this.Close();
                    frmPhysicalStockReportObj.gridfill();
                }
                if (frmDayBookObj != null)
                {
                    if (cbxPrint.Checked)
                    {
                        if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(decMasterId);
                        }
                        else
                        {
                            Print(decMasterId);
                        }
                    }
                    this.Close();
                    frmDayBookObj.dayBookGridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS20:" + ex.Message;
            }
        }
        /// <summary>
        /// Stock posting add function
        /// </summary>
        /// <param name="decProductId"></param>
        /// <param name="strMaster"></param>
        public void AddtoStockPosting(decimal decProductId, string strMaster)
        {
            try
            {
                StockPostingSP spStockPosting = new StockPostingSP();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                decimal decCurrentQty = infoPhysicalStockDetails.Qty;
                decimal decGId = infoPhysicalStockDetails.GodownId;
                decimal decBId = infoPhysicalStockDetails.BatchId;
                decimal decRId = infoPhysicalStockDetails.RackId;
                decimal decOldStock = spStockPosting.ProductGetCurrentStock(decProductId, decGId, decBId, decRId);
                if (decCurrentQty >= 0)
                {
                    if (decOldStock >= 0)
                    {
                        decimal decBalance = decCurrentQty - decOldStock;
                        if (decBalance >= 0)
                        {
                            infoStockPosting.InwardQty = decBalance;
                            infoStockPosting.OutwardQty = 0;
                        }
                        else
                        {
                            infoStockPosting.InwardQty = 0;
                            infoStockPosting.OutwardQty = -decBalance;
                        }
                    }
                    else
                    {
                        infoStockPosting.InwardQty = -decOldStock + decCurrentQty;
                        infoStockPosting.OutwardQty = 0;
                    }
                }
                else
                {
                    if (decOldStock >= 0)
                    {
                        infoStockPosting.InwardQty = 0;
                        infoStockPosting.OutwardQty = -decCurrentQty + decOldStock;
                    }
                    else
                    {
                        decimal decBalance = -decCurrentQty + decOldStock;
                        if (decBalance >= 0)
                        {
                            infoStockPosting.InwardQty = 0;
                            infoStockPosting.OutwardQty = decBalance;
                        }
                        else
                        {
                            infoStockPosting.InwardQty = -decBalance;
                            infoStockPosting.OutwardQty = 0;
                        }
                    }
                }
                infoStockPosting.VoucherNo = strMaster;
                infoStockPosting.BatchId = infoPhysicalStockDetails.BatchId;
                infoStockPosting.Date = Convert.ToDateTime(txtDate.Text);
                infoStockPosting.Extra1 = string.Empty;
                infoStockPosting.Extra2 = string.Empty;
                infoStockPosting.GodownId = infoPhysicalStockDetails.GodownId;
                infoStockPosting.ProductId = decProductId;
                infoStockPosting.Rate = infoPhysicalStockDetails.Rate;
                infoStockPosting.UnitId = infoPhysicalStockDetails.UnitId;
                infoStockPosting.RackId = infoPhysicalStockDetails.RackId;
                infoStockPosting.AgainstVoucherTypeId = 0;
                infoStockPosting.AgainstVoucherNo = "NA";
                infoStockPosting.AgainstInvoiceNo = "NA";
                infoStockPosting.VoucherTypeId = decPhysicalStockVoucherTypeId;
                infoStockPosting.InvoiceNo = strMaster;
                spStockPosting.StockPostingAdd(infoStockPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS21:" + ex.Message;
            }
        }
        /// <summary>
        /// Batch combofillBased on product
        /// </summary>
        /// <param name="decProductId"></param>
        /// <param name="inRow"></param>
        /// <param name="inColumn"></param>
        public void BatchComboFill(decimal decProductId, int inRow, int inColumn)
        {
            try
            {
                DataTable dtbl = new DataTable();
                BatchSP spBatch = new BatchSP();
                dtbl = spBatch.BatchNamesCorrespondingToProduct(decProductId);
                DataGridViewComboBoxCell dgvcmbBatchCell = (DataGridViewComboBoxCell)dgvPhysicalStock.Rows[inRow].Cells[inColumn];
                dgvcmbBatchCell.DataSource = dtbl;
                dgvcmbBatchCell.ValueMember = "batchId";
                dgvcmbBatchCell.DisplayMember = "batchNo";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS22:" + ex.Message;
            }
        }
        /// <summary>
        /// Adding PhysicalStockDetails when updating
        /// </summary>
        public void EditPhysicalStockDetails()
        {
            try
            {
                ProductInfo infoProduct = new ProductInfo();
                ProductSP spProduct = new ProductSP();
                PhysicalStockDetailsInfo infoPhysicalStockDetails = new PhysicalStockDetailsInfo();
                PhysicalStockDetailsSP spPhysicalStockDetails = new PhysicalStockDetailsSP();
                int inRowcount = dgvPhysicalStock.Rows.Count;
                for (int inI = 0; inI < inRowcount - 1; inI++)
                {
                    infoPhysicalStockDetails.PhysicalStockMasterId = decMasterId;
                    infoPhysicalStockDetails.PhysicalStockDetailsId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtPhysicalStockDetailId"].Value);
                    if (dgvPhysicalStock.Rows[inI].Cells["dgvtxtProductCode"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvtxtProductCode"].Value.ToString() != string.Empty)
                    {
                        infoProduct = spProduct.ProductViewByCode(dgvPhysicalStock.Rows[inI].Cells["dgvtxtProductCode"].Value.ToString());
                        infoPhysicalStockDetails.ProductId = infoProduct.ProductId;
                    }
                    if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbGodown"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbGodown"].Value.ToString() != string.Empty)
                    {
                        infoPhysicalStockDetails.GodownId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvcmbGodown"].Value.ToString());
                    }
                    else
                    {
                        infoPhysicalStockDetails.GodownId = 0;
                    }
                    if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbRack"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbRack"].Value.ToString() != string.Empty)
                    {
                        infoPhysicalStockDetails.RackId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvcmbRack"].Value.ToString());
                    }
                    else
                    {
                        infoPhysicalStockDetails.RackId = 0;
                    }
                    if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbBatch"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbBatch"].Value.ToString() != string.Empty)
                    {
                        infoPhysicalStockDetails.BatchId = Convert.ToDecimal(Convert.ToString(dgvPhysicalStock.Rows[inI].Cells["dgvcmbBatch"].Value));
                    }
                    else
                    {
                        infoPhysicalStockDetails.BatchId = 0;
                    }
                    if (dgvPhysicalStock.Rows[0].Cells["dgvcmbBatch"].Value == null && dgvPhysicalStock.Rows[0].Cells["dgvcmbBatch"].Value == null)
                    {
                        MessageBox.Show("Can't update physical stock without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvPhysicalStock.ClearSelection();
                        dgvPhysicalStock.Focus();
                    }
                    if (dgvPhysicalStock.Rows[inI].Cells["dgvtxtQty"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvtxtQty"].Value.ToString() != string.Empty)
                    {
                        infoPhysicalStockDetails.Qty = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtQty"].Value.ToString());
                    }
                    if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbUnit"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbUnit"].Value.ToString() != string.Empty)
                    {
                        infoPhysicalStockDetails.UnitId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvcmbUnit"].Value.ToString());
                    }
                    infoPhysicalStockDetails.Rate = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtRate"].Value.ToString());
                    infoPhysicalStockDetails.Amount = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                    infoPhysicalStockDetails.Slno = Convert.ToInt32(dgvPhysicalStock.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                    infoPhysicalStockDetails.Extra1 = string.Empty;
                    infoPhysicalStockDetails.Extra2 = string.Empty;
                    spPhysicalStockDetails.PhysicalStockDetailsAdd(infoPhysicalStockDetails);
                    decimal decPId = infoPhysicalStockDetails.ProductId;
                    string strVoucher = infoPhysicalStockMaster.VoucherNo;
                    AddtoStockPosting(decPId, strVoucher);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS23:" + ex.Message;
            }
        }
        /// <summary>
        ///  Calculating the Grand total amount
        /// </summary>
        private void CalculateTotalAmount()
        {
            try
            {
                {
                    decimal decTotalAmount = 0;
                    decimal decSelectedCurrencyRate = 0;
                    ExchangeRateSP SpExchangRate = new ExchangeRateSP();
                    foreach (DataGridViewRow dr in dgvPhysicalStock.Rows)
                    {
                        if (dr.Cells["dgvtxtAmount"].Value != null && dr.Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            if (dr.Cells["dgvcmbCurrency"].Value != null)
                            {
                                decSelectedCurrencyRate = SpExchangRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(dr.Cells["dgvcmbCurrency"].Value.ToString()));//Exchange rate of grid's row
                                decTotalAmount = decTotalAmount + (Convert.ToDecimal(dr.Cells["dgvtxtAmount"].Value.ToString()) * decSelectedCurrencyRate);
                            }
                            else
                            {
                                decTotalAmount = decTotalAmount + Convert.ToDecimal(dr.Cells["dgvtxtAmount"].Value.ToString());
                            }
                        }
                    }
                    txtTotalAmount.Text = Math.Round(decTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS24:" + ex.Message;
            }
        }
        /// <summary>
        /// Save or edit function, checking the invalid entries here
        /// </summary>
        public void SaveorEdit()
        {
            try
            {
                int inIfGridColumnMissing = 0;
                ArrayList arrLst = new ArrayList();
                string output = string.Empty;
                int inRow = dgvPhysicalStock.RowCount;
                if (inRow == 1)
                {
                    Messages.InformationMessage("Can't save without atleast one complete details");
                    dgvPhysicalStock.Focus();
                    inIfGridColumnMissing = 1;
                }
                else
                {
                    int inJ = 0;
                    for (int inI = 0; inI < inRow - 1; inI++)
                    {
                        if (dgvPhysicalStock.Rows[inI].HeaderCell.Value.ToString() == "X")
                        {
                            arrLst.Add(Convert.ToString(inI + 1));
                            inIfGridColumnMissing = 1;
                            inJ++;
                        }
                    }
                    if (inJ != 0)
                    {
                        if (inJ == inRow - 1)
                        {
                            Messages.InformationMessage("Can't save without atleat one complete details");
                            inIfGridColumnMissing = 1;
                        }
                        else
                        {
                            foreach (object obj in arrLst)
                            {
                                string str = Convert.ToString(obj);
                                if (str != null)
                                {
                                    output += str + ",";
                                }
                                else
                                {
                                    break;
                                }
                            }
                            bool isOk = Messages.UpdateMessageCustom("Row No " + output + " not completed.Do you want to continue?");
                            if (isOk)
                            {
                                inIfGridColumnMissing = 0;
                            }
                            else
                            {
                                inIfGridColumnMissing = 1;
                            }
                        }
                    }
                    if (inIfGridColumnMissing == 0)
                    {
                        if (btnSave.Text == "Save")
                        {
                            if (PublicVariables.isMessageAdd)
                            {
                                if (Messages.SaveMessage())
                                {
                                    SaveFunction();
                                }
                            }
                            else
                            {
                                SaveFunction();
                            }
                        }
                        if (btnSave.Text == "Update")
                        {
                            if (PublicVariables.isMessageEdit)
                            {
                                if (Messages.UpdateMessage())
                                {
                                    EditFunction();
                                }
                            }
                            else
                            {
                                EditFunction();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS25:" + ex.Message;
            }
        }
        /// <summary>
        /// Checking the column's value missing in grid , search and replace the header cell based on it
        /// </summary>
        public void CheckColumnMissing()
        {
            try
            {
                if (dgvPhysicalStock.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductName"].Value == null || dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductName"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvPhysicalStock.CurrentRow.HeaderCell.Value = "X";
                            dgvPhysicalStock.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvPhysicalStock.CurrentRow.Cells["dgvtxtQty"].Value == null || dgvPhysicalStock.CurrentRow.Cells["dgvtxtQty"].Value.ToString().Trim() == string.Empty || Convert.ToDecimal(dgvPhysicalStock.CurrentRow.Cells["dgvtxtQty"].Value) == 0)
                        {

                            isValueChanged = true;
                            dgvPhysicalStock.CurrentRow.HeaderCell.Value = "X";
                            dgvPhysicalStock.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvPhysicalStock.CurrentRow.Cells["dgvtxtRate"].Value == null || dgvPhysicalStock.CurrentRow.Cells["dgvtxtRate"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvPhysicalStock.CurrentRow.HeaderCell.Value = "X";
                            dgvPhysicalStock.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            isValueChanged = true;
                            dgvPhysicalStock.CurrentRow.HeaderCell.Value = string.Empty;
                        }
                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS26:" + ex.Message;
            }
        }
        /// <summary>
        /// Save Function
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                ProductInfo infoProduct = new ProductInfo();
                ProductSP spProduct = new ProductSP();
                PhysicalStockMasterInfo infoPhysicalStockMaster = new PhysicalStockMasterInfo();
                PhysicalStockDetailsSP spPhysicalStockDetails = new PhysicalStockDetailsSP();
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                infoPhysicalStockMaster.VoucherNo = txtVoucherNo.Text.Trim();
                infoPhysicalStockMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoPhysicalStockMaster.Narration = txtNarration.Text.Trim();
                string s = txtTotalAmount.Text;
                infoPhysicalStockMaster.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                if (isAutomatic)
                {
                    infoPhysicalStockMaster.SuffixPrefixId = decPhysicalStockSuffixPrefixId;
                    infoPhysicalStockMaster.VoucherNo = strVoucherNo;
                    infoPhysicalStockMaster.InvoiceNo = txtVoucherNo.Text;
                }
                else
                {
                    infoPhysicalStockMaster.SuffixPrefixId = 0;
                    infoPhysicalStockMaster.VoucherNo = txtVoucherNo.Text;
                    infoPhysicalStockMaster.InvoiceNo = txtVoucherNo.Text;
                }
                infoPhysicalStockMaster.VoucherTypeId = decPhysicalStockVoucherTypeId;
                infoPhysicalStockMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoPhysicalStockMaster.Extra1 = string.Empty;
                infoPhysicalStockMaster.Extra2 = string.Empty;
                decPhysicalStockMasterIdentity = Convert.ToDecimal(spPhysicalStockMaster.PhysicalStockMasterAdd(infoPhysicalStockMaster));
                int inRowcount = dgvPhysicalStock.Rows.Count;
                for (int inI = 0; inI < inRowcount - 1; inI++)
                {
                    if (dgvPhysicalStock.Rows[inI].HeaderCell.Value.ToString() != "X")
                    {
                        infoPhysicalStockDetails.PhysicalStockMasterId = decPhysicalStockMasterIdentity;
                        if (dgvPhysicalStock.Rows[inI].Cells["dgvtxtProductCode"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvtxtProductCode"].Value.ToString() != string.Empty)
                        {
                            infoProduct = spProduct.ProductViewByCode(dgvPhysicalStock.Rows[inI].Cells["dgvtxtProductCode"].Value.ToString());
                            infoPhysicalStockDetails.ProductId = infoProduct.ProductId;
                        }
                        if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbGodown"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbGodown"].Value.ToString() != string.Empty)
                        {
                            infoPhysicalStockDetails.GodownId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvcmbGodown"].Value.ToString());
                        }
                        else
                        {
                            infoPhysicalStockDetails.GodownId = 0;
                        }
                        if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbRack"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbRack"].Value.ToString() != string.Empty)
                        {
                            infoPhysicalStockDetails.RackId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvcmbRack"].Value.ToString());
                        }
                        else
                        {
                            infoPhysicalStockDetails.RackId = 0;
                        }
                        if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbBatch"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbBatch"].Value.ToString() != string.Empty)
                        {
                            infoPhysicalStockDetails.BatchId = Convert.ToDecimal(Convert.ToString(dgvPhysicalStock.Rows[inI].Cells["dgvcmbBatch"].Value));
                        }
                        else
                        {
                            infoPhysicalStockDetails.BatchId = 0;
                        }
                        if (dgvPhysicalStock.Rows[inI].Cells["dgvtxtQty"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvtxtQty"].Value.ToString() != string.Empty)
                        {
                            infoPhysicalStockDetails.Qty = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtQty"].Value.ToString());
                        }
                        if (dgvPhysicalStock.Rows[inI].Cells["dgvcmbUnit"].Value != null && dgvPhysicalStock.Rows[inI].Cells["dgvcmbUnit"].Value.ToString() != string.Empty)
                        {
                            infoPhysicalStockDetails.UnitId = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvcmbUnit"].Value.ToString());
                            infoPhysicalStockDetails.UnitConversionId = decConversionId;
                        }
                        infoPhysicalStockDetails.Rate = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtRate"].Value.ToString());
                        infoPhysicalStockDetails.Amount = Convert.ToDecimal(dgvPhysicalStock.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                        infoPhysicalStockDetails.Slno = Convert.ToInt32(dgvPhysicalStock.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                        infoPhysicalStockDetails.Extra1 = string.Empty;
                        infoPhysicalStockDetails.Extra2 = string.Empty;
                        spPhysicalStockDetails.PhysicalStockDetailsAdd(infoPhysicalStockDetails);
                        decimal decPId = infoPhysicalStockDetails.ProductId;
                        string strVoucher = infoPhysicalStockMaster.VoucherNo;
                        AddtoStockPosting(decPId, strVoucher);
                    }
                }
                Messages.SavedMessage();
                if (dgvPhysicalStock.RowCount > 1)
                {
                    if (cbxPrint.Checked)
                    {
                        if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(decPhysicalStockMasterIdentity);
                        }
                        else
                        {
                            Print(decPhysicalStockMasterIdentity);
                        }
                    }
                }
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS27:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDayBook to view details and for updation
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decId)
        {
            try
            {
                base.Show();
                frmDayBook.Enabled = false;
                this.frmDayBookObj = frmDayBook;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtVoucherNo.Enabled = false;
                decMasterId = decId;
                FillFunction();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS28:" + ex.Message;
            }
        }


        /// <summary>
        /// Function to call frmProductSearchPopup form to select and view a product
        /// </summary>
        /// <param name="frmProductSearchPopup"></param>
        /// <param name="decproductId"></param>
        /// <param name="decCurrentRowIndex"></param>
        public void CallFromProductSearchPopup(frmProductSearchPopup frmProductSearchPopup, decimal decproductId, decimal decCurrentRowIndex)
        {
            ProductInfo infoProduct = new ProductInfo();
            ProductSP spProduct = new ProductSP();
            try
            {
                base.Show();
                this.frmProductSearchPopupObj = frmProductSearchPopup;
                infoProduct = spProduct.ProductView(decproductId);
                int inRowcount = dgvPhysicalStock.Rows.Count;
                for (int i = 0; i < inRowcount; i++)
                {
                    if (i == decCurrentRowIndex)
                    {
                        strProductCode = infoProduct.ProductCode;
                        productDetailsFill(strProductCode, i, "ProductCode");
                    }
                }
                frmProductSearchPopupObj.Close();
                frmProductSearchPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS29:" + ex.Message;
            }
        }


        #endregion
        #region Events
        /// <summary>
        /// Form loads , call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPhysicalStock_Load(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS30:" + ex.Message;
            }
        }
        /// <summary>
        /// Form closing , checking the other form status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPhysicalStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmPhysicalStockRegisterObj != null)
                {
                    frmPhysicalStockRegisterObj.Enabled = true;
                    frmPhysicalStockRegisterObj.gridfill();
                }
                if (frmPhysicalStockReportObj != null)
                {
                    frmPhysicalStockReportObj.Enabled = true;
                    frmPhysicalStockReportObj.gridfill();
                }
                if (objfrmVoucherSearch != null)
                {
                    objfrmVoucherSearch.Enabled = true;
                    objfrmVoucherSearch.GridFill();
                }
                if (objfrmVoucherProduct != null)
                {
                    objfrmVoucherProduct.Enabled = true;
                    objfrmVoucherProduct.FillGrid();
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();
                    frmDayBookObj = null;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS31:" + ex.Message;
            }
        }
        /// <summary>
        ///  delete button click, checking the user privilage and call the delete function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, "Delete"))
                {
                    if (PublicVariables.isMessageDelete)
                    {
                        if (Messages.DeleteMessage())
                        {
                            DeleteFunction(decMasterId);
                        }
                    }
                    else
                    {
                        DeleteFunction(decMasterId);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS32:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation for amount column in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvtxtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvPhysicalStock.CurrentCell != null)
                {
                    if (dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtAmount")
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS33:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation for Rate column in datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvtxtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvPhysicalStock.CurrentCell != null)
                {
                    if (dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtRate")
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS34:" + ex.Message;
            }
        }
        /// <summary>
        /// Calling the corresponding events and set the column SuggestAppend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPhysicalStock_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBoxControl = e.Control as DataGridViewTextBoxEditingControl;
                if (TextBoxControl != null)
                {
                    if (dgvPhysicalStock.CurrentCell != null && dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductName")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = ProductNames;
                    }
                    if (dgvPhysicalStock.CurrentCell != null && dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = ProductCodes;
                    }
                    if (dgvPhysicalStock.CurrentCell != null && dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name != "dgvtxtProductCode" && dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name != "dgvtxtProductName")
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)dgvPhysicalStock.EditingControl;
                        editControl.AutoCompleteMode = AutoCompleteMode.None;
                    }
                    TextBoxControl.KeyPress += TextBoxCellEditControlKeyPress;
                    if (dgvPhysicalStock.CurrentCell.ColumnIndex == dgvPhysicalStock.Columns["dgvtxtAmount"].Index)
                    {
                        TextBoxControl.KeyPress += keypressevent;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS35:" + ex.Message;
            }
        }
        /// <summary>
        /// call the decimal validation for event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keypressevent(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS36:" + ex.Message;
            }
        }
        /// <summary>
        /// call the DecimalValidation for Qty and rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCellEditControlKeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvPhysicalStock.CurrentCell != null)
                {
                    if (dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtQty" || dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtRate")
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS37:" + ex.Message;
            }
        }
        /// <summary>
        /// set the txtDate value based on the dtp's value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtDate.Text = dtpDate.Value.ToString("dd-MMM-yyyy");
                txtDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS38:" + ex.Message;
            }
        }
        /// <summary>
        /// validate the date format and set the date
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
                dtpDate.Value = Convert.ToDateTime(txtDate.Text);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS39:" + ex.Message;
            }
        }
        /// <summary>
        /// get the narration line count for navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inNarrationCount++;
                    if (inNarrationCount == 2)
                    {
                        inNarrationCount = 0;
                        txtTotalAmount.Focus();
                        txtNarration.SelectionStart = 0;
                        txtNarration.SelectionLength = 0;
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS40:" + ex.Message;
            }
        }
        private void dgvPhysicalStock_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                e.ThrowException = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS41:" + ex.Message;
            }
        }
        /// <summary>
        /// set tye columns as based on settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPhysicalStock_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            decimal decGodownId = 0;
            try
            {
                if (dgvPhysicalStock.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvPhysicalStock.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvPhysicalStock.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
                if (e.ColumnIndex == dgvPhysicalStock.Columns["dgvcmbRack"].Index)
                {
                    if (dgvPhysicalStock.CurrentRow.Cells["dgvcmbGodown"].Value != null && dgvPhysicalStock.CurrentRow.Cells["dgvcmbGodown"].Value.ToString() != string.Empty)
                    {

                        decGodownId = Convert.ToDecimal(dgvPhysicalStock.CurrentRow.Cells["dgvcmbGodown"].Value);
                        RackComboFillCorrespondingGodown(decGodownId, e.RowIndex, dgvPhysicalStock.CurrentRow.Cells["dgvcmbRack"].ColumnIndex);
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS42:" + ex.Message;
            }
        }
        /// <summary>
        /// Cleare button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS43:" + ex.Message;
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
                if (Messages.CloseConfirmation())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS44:" + ex.Message;
            }
        }
        /// <summary>
        /// save button click, checking the user privilage and call save function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveorEdit();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS45:" + ex.Message;
            }
        }
        /// <summary>
        /// ClearSelection in gridview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPhysicalStock_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgvPhysicalStock.ClearSelection();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS46:" + ex.Message;
            }
        }
        public void productDetailsFill(string strProduct, int inRowIndex, string strFillMode)
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                DataTable dtbl = new DataTable();
                if (strFillMode == "Barcode")
                {
                    dtbl = spProduct.ProductDetailsCoreespondingToBarcode(strProduct);
                }
                else if (strFillMode == "ProductCode")
                {
                    dtbl = spProduct.ProductDetailsCoreespondingToProductCode(strProduct);
                }
                else if (strFillMode == "ProductName")
                {
                    dtbl = spProduct.ProductDetailsCoreespondingToProductName(strProduct);
                }
                if (dtbl.Rows.Count != 0)
                {
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtProductCode"].Value = dtbl.Rows[0]["productCode"].ToString();
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtProductName"].Value = dtbl.Rows[0]["productName"].ToString();
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(dtbl.Rows[0]["unitId"].ToString());
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(dtbl.Rows[0]["unitId"].ToString());
                    decimal decproductId = Convert.ToDecimal(dtbl.Rows[0]["productId"].ToString());
                    BatchComboFill(decproductId, inRowIndex, Convert.ToInt32(dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbBatch"].ColumnIndex));
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(dtbl.Rows[0]["batchId"].ToString());
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtRate"].Value = dtbl.Rows[0]["purchaseRate"].ToString();
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(dtbl.Rows[0]["godownId"].ToString());
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbRack"].Value = Convert.ToDecimal(dtbl.Rows[0]["rackId"].ToString());
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtQty"].Value = string.Empty;
                }
                else
                {
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtProductName"].Value = string.Empty;
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtRate"].Value = string.Empty;
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtProductCode"].Value = string.Empty;
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbUnit"].Value = string.Empty;
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvtxtBarcode"].Value = string.Empty;
                    dgvPhysicalStock.Rows[inRowIndex].Cells["dgvcmbBatch"].Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS47:" + ex.Message;
            }
        }

        /// <summary>
        /// Doing basic calculations in cell value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPhysicalStock_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ProductInfo infoProduct = new ProductInfo();
                ProductSP spProduct = new ProductSP();
                BatchSP spBatch = new BatchSP();
                PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    string strBarcode = string.Empty;
                    string strProductCode = string.Empty;
                    if (dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtBarcode")
                    {
                        string strBCode = string.Empty;
                        DataTable dtbl = new DataTable();
                        if (dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value != null && dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value.ToString() != string.Empty)
                        {
                            strBCode = dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value.ToString();
                            productDetailsFill(strBCode, dgvPhysicalStock.CurrentRow.Index, "Barcode");
                            CheckColumnMissing();
                        }
                    }
                    else if (dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        UnitInfo infoUnit = new UnitInfo();
                        string strPrdCode = string.Empty;
                        if (dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtProductCode"].Value != null && dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtProductCode"].Value.ToString() != string.Empty)
                        {
                            strPrdCode = dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtProductCode"].Value.ToString();
                            productDetailsFill(strPrdCode, dgvPhysicalStock.CurrentRow.Index, "ProductCode");
                            CheckColumnMissing();
                        }
                    }
                    else if (dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtProductName")
                    {
                        string strProductName = string.Empty;
                        DataTable dtbl = new DataTable();
                        if (dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtProductName"].Value != null && dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtProductName"].Value.ToString() != string.Empty)
                        {
                            strProductName = dgvPhysicalStock.Rows[e.RowIndex].Cells["dgvtxtProductName"].Value.ToString();
                            productDetailsFill(strProductName, dgvPhysicalStock.CurrentRow.Index, "ProductName");
                            CheckColumnMissing();
                        }
                    }
                    if (dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtQty" && isAmountcalc || dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtRate" && isAmountcalc)
                    {
                        NewAmountCalculation(e.RowIndex);
                        CheckColumnMissing();
                    }
                    if (dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtQty" && isAmountcalc || dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvtxtRate" && isAmountcalc)
                    {
                        CalculateTotalAmount();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS48:" + ex.Message;
            }
        }
        /// <summary>
        /// Link button click to remove one row from grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int inRowCount = dgvPhysicalStock.RowCount;
                if (inRowCount > 1 && !dgvPhysicalStock.CurrentRow.IsNewRow)
                {
                    if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (dgvPhysicalStock.CurrentRow.Cells["dgvtxtPhysicalStockDetailId"].Value != null && dgvPhysicalStock.CurrentRow.Cells["dgvtxtPhysicalStockDetailId"].Value.ToString() != "")
                        {
                            arrlstOfRemove.Add(dgvPhysicalStock.CurrentRow.Cells["dgvtxtPhysicalStockDetailId"].Value.ToString());
                            inArrOfRemove++;
                        }
                        dgvPhysicalStock.Rows.RemoveAt(dgvPhysicalStock.CurrentRow.Index);
                        SerialNo();
                        CalculateTotalAmount();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS49:" + ex.Message;
            }
        }
        private void dgvPhysicalStock_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS50:" + ex.Message;
            }

        }

        private void dgvPhysicalStock_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!isFromEditMode)
            {
                try
                {
                    string strBarcode = string.Empty;
                    string strProductCode = string.Empty;
                    ProductInfo infoProduct = new ProductInfo();
                    ProductSP spProduct = new ProductSP();
                    BatchSP spBatch = new BatchSP();
                    PhysicalStockMasterSP spPhysicalStockMaster = new PhysicalStockMasterSP();
                    if (e.RowIndex != -1 && e.ColumnIndex != -1)
                    {
                        if (dgvPhysicalStock.Columns[e.ColumnIndex].Name == "dgvcmbBatch")
                        {
                            if (dgvPhysicalStock.CurrentRow.Cells["dgvcmbBatch"].Value != null)
                            {
                                if (Convert.ToString(dgvPhysicalStock.CurrentRow.Cells["dgvcmbBatch"].Value) != string.Empty &&
                                   Convert.ToDecimal(dgvPhysicalStock.CurrentRow.Cells["dgvcmbBatch"].Value) != 0)
                                {
                                    if (isGridValueChanged)
                                    {
                                        decBatchId = Convert.ToDecimal(dgvPhysicalStock.CurrentRow.Cells["dgvcmbBatch"].Value);
                                        strBarcode = spBatch.ProductBatchBarcodeViewByBatchId(decBatchId);
                                        isGridValueChanged = false;
                                        dgvPhysicalStock.CurrentRow.Cells["dgvtxtBarcode"].Value = strBarcode;
                                        isGridValueChanged = true;
                                    }
                                }
                            }
                        }
                        CheckColumnMissing();
                    }
                }
                catch (Exception ex)
                {
                    formMDI.infoError.ErrorString = "PS51:" + ex.Message;
                }
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvPhysicalStock.Focus();
                    dgvPhysicalStock.Rows[dgvPhysicalStock.RowCount - 1].Cells["dgvtxtBarcode"].Selected = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS52:" + ex.Message;
            }
        }
        /// <summary>
        /// For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text.Trim() == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        dgvPhysicalStock.Focus();
                        dgvPhysicalStock.Rows[dgvPhysicalStock.RowCount - 1].Cells["dgvtxtProductCode"].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS53:" + ex.Message;
            }
        }
        public void RackComboFillCorrespondingGodown(decimal decGodownId, int inRow, int inColumn)
        {
            try
            {
                DataTable dtbl = new DataTable();
                RackSP spRack = new RackSP();
                dtbl = spRack.RackNamesCorrespondingToGodownId(decGodownId);
                DataRow drow = dtbl.NewRow();
                DataGridViewComboBoxCell dgvcmbRackCellConsumption = (DataGridViewComboBoxCell)dgvPhysicalStock.Rows[inRow].Cells[inColumn];
                dgvcmbRackCellConsumption.DataSource = dtbl;
                dgvcmbRackCellConsumption.ValueMember = "rackId";
                dgvcmbRackCellConsumption.DisplayMember = "rackName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS54:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPhysicalStock_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    if (dgvPhysicalStock.CurrentCell != null)
                    {
                        if (dgvPhysicalStock.CurrentCell == dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductName"] || dgvPhysicalStock.CurrentCell == dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductCode"])
                        {
                            if (dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                            {
                                frmProductCreation frmProductCreationObj = new frmProductCreation();
                                frmProductCreationObj.MdiParent = formMDI.MDIObj;
                                frmProductCreationObj.CallFromPhysicalStock(this);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS55:" + ex.Message;
            }
        }

        public void ReturnFromProductCreation(decimal decProductId)
        {
            ProductInfo infoProduct = new ProductInfo();
            ProductSP spProduct = new ProductSP();
            try
            {
                this.Enabled = true;
                this.Activate();
                if (decProductId != 0)
                {
                    infoProduct = spProduct.ProductView(decProductId);
                    strProductCodetoFill = infoProduct.ProductCode;
                    productDetailsFill(strProductCodetoFill, dgvPhysicalStock.CurrentRow.Index, "ProductCode");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS56:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTotalAmount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtTotalAmount.Text == string.Empty || txtTotalAmount.SelectionStart == 0)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        txtNarration.Focus();
                        txtNarration.SelectionStart = 0;
                        txtNarration.SelectionLength = 0;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS57:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnSave.Focus();
                }
                if (e.KeyCode == Keys.Enter)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete.Focus();
                    }
                    else
                    {
                        btnClear.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS58:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (btnDelete.Enabled)
                    {
                        btnDelete.Focus();
                    }
                    else
                    {
                        btnClear.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS59:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnClear.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS60:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtTotalAmount.Focus();
                    txtTotalAmount.SelectionStart = 0;
                    txtTotalAmount.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS61:" + ex.Message;
            }
        }
        /// <summary>
        ///  For Enter key and Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPhysicalStock_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvPhysicalStock.CurrentCell == dgvPhysicalStock.Rows[dgvPhysicalStock.Rows.Count - 1].Cells["dgvtxtAmount"])
                    {
                        txtNarration.Focus();
                        txtNarration.SelectionStart = 0;
                        txtNarration.SelectionLength = 0;
                        dgvPhysicalStock.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvPhysicalStock.CurrentCell == dgvPhysicalStock.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                        dgvPhysicalStock.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
                {
                    if (dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        frmProductSearchPopup frmProductSearchPopupObj = new frmProductSearchPopup();
                        frmProductSearchPopupObj.MdiParent = formMDI.MDIObj;
                        if (dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductCode"].Value != null || dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductName"].Value != null)
                        {
                            frmProductSearchPopupObj.CallFromPhysicalStock(this, dgvPhysicalStock.CurrentRow.Index, dgvPhysicalStock.CurrentRow.Cells["dgvSitxtProductCode"].Value.ToString());
                        }
                        else
                        {
                            frmProductSearchPopupObj.CallFromPhysicalStock(this, dgvPhysicalStock.CurrentRow.Index, string.Empty);
                        }
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    if (dgvPhysicalStock.CurrentCell != null)
                    {
                        if (dgvPhysicalStock.CurrentCell == dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductName"] || dgvPhysicalStock.CurrentCell == dgvPhysicalStock.CurrentRow.Cells["dgvtxtProductCode"])
                        {
                            SendKeys.Send("{F10}");
                            if (dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvPhysicalStock.Columns[dgvPhysicalStock.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                            {
                                frmProductCreation frmProductCreationObj = new frmProductCreation();
                                frmProductCreationObj.MdiParent = formMDI.MDIObj;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PS62:" + ex.Message;
            }
        }
        #endregion


    }
}
