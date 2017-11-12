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
using System.Collections;

namespace One_Account
{

    public partial class frmRejectionOut : Form
    {

        #region PublicVariables
        /// <summary>
        /// Public variable declaration part
        /// </summary>
        string strCashOrParty = string.Empty;
        string strVoucherNo = string.Empty;
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        string strRejectionOutNo = string.Empty;
        string strMaterialReceiptNo = string.Empty;
        string tableName = "RejectionOutMaster";
        string strRejectionOutVoucherNo = string.Empty;
        bool isAutomatic = false;
        bool isOrderFill = false;
        bool isValueChange = false;
        bool isAmountcalc = true;
        bool isDoAfterGridFill = false;
        bool isDoCellValueChange = false;
        bool isFromRegister = false;
        bool isDontExecuteCashorParty = false;
        int inKeyPrsCount = 0;
        decimal decRejectionOutVoucherTypeId = 0;
        decimal decRejectionOutSuffixPrefixId = 0;
        decimal decRejectionOutId = 0;
        decimal decTotAmnt = 0;
        decimal decRejectionOutMasterIdentity = 0;
        decimal decRejectionOutDetailsIdentity = 0;
        decimal decRejectionOutTypeId = 0;
        decimal decRejectionOutDetailId = 0;
        decimal decOldQty = 0;
        decimal decSelectedCurrencyRate = 0;
        decimal decCurrQty = 0;
        decimal decCurrentConversionRate = 0;
        decimal decCurrentRate = 0;
        MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
        ArrayList lstArrOfRemove = new ArrayList();
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        MaterialReceiptDetailsSP spMaterialReceiptDetails = new MaterialReceiptDetailsSP();
        ProductInfo infoProduct = new ProductInfo();
        ProductSP spProduct = new ProductSP();
        SettingsSP spSettings = new SettingsSP();
        RejectionOutDetailsInfo infoRejectionOutDetails = new RejectionOutDetailsInfo();
        RejectionOutMasterInfo infoRejectionOutMaster = new RejectionOutMasterInfo();
        RejectionOutMasterSP spRejectionOutMaster = new RejectionOutMasterSP();
        RejectionOutDetailsSP spRejectionOutDetails = new RejectionOutDetailsSP();
        StockPostingSP spStockPosting = new StockPostingSP();
        DataGridViewTextBoxEditingControl TextBoxControl;
        AutoCompleteStringCollection ProductNames = new AutoCompleteStringCollection();
        AutoCompleteStringCollection ProductCodes = new AutoCompleteStringCollection();
        frmRejectionOutRegister frmRejectionOutRegisterObj = null;//To use in call from frmRejectionOutRegister
        frmRejectionOutReport frmRejectionOutReportObj = null;//To use in call from frmRejectionOutReport
        VoucherTypeSP spVoucherType = new VoucherTypeSP();
        frmLedgerPopup frmLedgerPopUpObj = new frmLedgerPopup();
        frmProductSearchPopup frmProductSearchPopupObj = new frmProductSearchPopup();
        frmVoucherSearch objVoucherSearch = null;
        frmDayBook frmDayBookObj = null;//To use in call from frmDayBook
        frmVoucherWiseProductSearch objVoucherProduct = null;//To use in call from frmVoucherWiseProductSearch

        #endregion

        #region Functions
        /// <summary>
        /// Create instance of frmRejectionOut
        /// </summary>
        public frmRejectionOut()
        {
            InitializeComponent();
        }

        /// <summary>
        /// It is a function for vouchertypeselection form to select perticular voucher and open the form under the vouchertype
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strVoucherTypeName"></param>
        public void MaterialReceiptComboClear()
        {
            try
            {
                cmbMaterialReceiptNo.DataSource = null;
                dgvProduct.Rows.Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO1:" + ex.Message;
            }
        }

        /// <summary>
        /// For VoucherNumberGeneration
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                RejectionOutMasterSP spRejectionOut = new RejectionOutMasterSP();
                strVoucherNo = "0";
                if (strVoucherNo == string.Empty)
                {
                    strVoucherNo = "0";
                }
                strVoucherNo = obj.VoucherNumberAutomaicGeneration(decRejectionOutVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, tableName);
                if (Convert.ToDecimal(strVoucherNo) != spRejectionOut.RejectionOutMasterGetMaxPlusOne(decRejectionOutVoucherTypeId))
                {
                    strVoucherNo = spRejectionOut.RejectionOutMasterGetMax(decRejectionOutVoucherTypeId).ToString();
                    strVoucherNo = obj.VoucherNumberAutomaicGeneration(decRejectionOutVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, tableName);
                    if (spRejectionOut.RejectionOutMasterGetMax(decRejectionOutVoucherTypeId) == "0")
                    {
                        strVoucherNo = "0";
                        strVoucherNo = obj.VoucherNumberAutomaicGeneration(decRejectionOutVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpDate.Value, tableName);
                    }
                }
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decRejectionOutVoucherTypeId, dtpDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    decRejectionOutSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                    strRejectionOutNo = strPrefix + strVoucherNo + strSuffix;
                    txtRejectionOutNo.Text = strRejectionOutNo;
                    txtRejectionOutNo.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO2:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from VoucherWiseProductSearch form
        /// </summary>
        /// <param name="frmVoucherWiseProductSearch"></param>
        /// <param name="decMasterId"></param>
        public void CallFromVoucherWiseProductSearch(frmVoucherWiseProductSearch frmVoucherwiseProductSearch, decimal decmasterId)
        {
            try
            {
                base.Show();
                frmVoucherwiseProductSearch.Enabled = true;
                objVoucherProduct = frmVoucherwiseProductSearch;
                decRejectionOutId = decmasterId;
                MaterialReceiptComboFill();
                FillRejectionOutReportForEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO3:" + ex.Message;
            }
        }

        /// <summary>
        /// It is a function for vouchertypeselection form to select perticular voucher and open the form under the vouchertype
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strVoucherTypeName"></param>
        public void CallFromVoucherTypeSelection(decimal decVoucherTypeId, string strVoucherTypeName)
        {
            try
            {
                decRejectionOutVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decRejectionOutVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();

                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decRejectionOutVoucherTypeId, dtpDate.Value);
                decRejectionOutSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                this.Text = strVoucherTypeName;
                base.Show();
                if (isAutomatic)
                {
                    txtDate.Focus();
                }
                else
                {
                    txtRejectionOutNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO4:" + ex.Message;
            }
        }


        /// <summary>
        /// Load the form while calling from the voucherSearch in editmode
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallFromVoucherSerach(frmVoucherSearch frm, decimal decId)
        {
            try
            {
                base.Show();
                objVoucherSearch = frm;
                decRejectionOutId = decId;
                FillRejectionOutReportForEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO5:" + ex.Message;
            }
        }

        /// <summary>
        /// To select the ledger from ledger popup
        /// </summary>
        /// <param name="frmLedgerPopUp"></param>
        /// <param name="decId"></param>
        /// /// <param name="strComboTypes"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId, string strComboTypes) //PopUp
        {
            try
            {
                base.Show();
                this.frmLedgerPopUpObj = frmLedgerPopup;
                if (strComboTypes == "CashOrSundryCreditors")
                {
                    TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, false);
                    cmbCashOrParty.SelectedValue = decId;
                }
                frmLedgerPopUpObj.Close();
                frmLedgerPopUpObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO6:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from RejectionOutRegister form
        /// </summary>
        /// <param name="frmRejectionOutRegister"></param>
        /// <param name="decRejectionOutMasterid"></param>
        public void CallFromRejectionOutRegister(frmRejectionOutRegister frmRejectionOutRegister, decimal decRejectionOutMasterid)
        {
            try
            {
                base.Show();
                this.frmRejectionOutRegisterObj = frmRejectionOutRegister;
                frmRejectionOutRegisterObj.Enabled = false;
                decRejectionOutId = decRejectionOutMasterid;
                FillRejectionOutReportForEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO7:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from RejectionOutReport form
        /// </summary>
        /// <param name="frmRejectionOutReport"></param>
        /// <param name="decRejectionOutMasterid"></param>
        public void CallFromRejectionOutReport(frmRejectionOutReport frmRejectionOutReport, decimal decRejectionOutMasterid)
        {
            try
            {
                base.Show();
                decRejectionOutId = decRejectionOutMasterid;

                frmRejectionOutReport.Enabled = false;
                this.frmRejectionOutReportObj = frmRejectionOutReport;
                FillRejectionOutReportForEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO8:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to load the form while calling from DayBook form
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decMasterId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmDayBook.Enabled = false;
                decRejectionOutId = decMasterId;
                MaterialReceiptComboFill();
                this.frmDayBookObj = frmDayBook;
                FillRejectionOutReportForEdit();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO9:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to clear the fields
        /// </summary>
        public void clear()
        {
            try
            {
                decRejectionOutId = 0;
                btnDelete.Enabled = false;
                txtLrNo.Clear();
                txtNarration.Clear();
                txtRejectionOutNo.Clear();
                txtTotalAmount.Clear();
                txtTransportationCompany.Clear();
                VoucherTypeComboFill();
                CashOrPartyComboFill();
                cmbMaterialReceiptNo.Visible = false;
                lblMaterialReceiptNo.Visible = false;
                label2.Visible = false;
                dtpDate.Value = PublicVariables._dtCurrentDate;
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                dtpDate.CustomFormat = "dd-MMMM-yyyy";
                txtTotalAmount.Text = "0.00";
                if (isAutomatic)
                {
                    VoucherNumberGeneration();
                    txtDate.Focus();
                }
                else
                {
                    txtRejectionOutNo.Focus();
                    txtRejectionOutNo.Text = string.Empty;
                    txtRejectionOutNo.ReadOnly = false;
                }
                CurrencyComboFill();
                dgvProduct.Rows.Clear();
                isOrderFill = false;
                cmbMaterialReceiptNo.DataSource = null;
                btnSave.Text = "Save";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO10:" + ex.Message;
            }
        }

        /// <summary>
        /// Unit combofill function
        /// </summary>
        /// <param name="decProductId"></param>
        /// <param name="inRow"></param>
        /// <param name="inColumn"></param>
        public void UnitComboFill(decimal decProductId, int inRow, int inColumn)
        {
            try
            {
                DataTable dtbl = new DataTable();
                UnitSP spUnit = new UnitSP();
                dtbl = spUnit.UnitViewAllByProductId(decProductId);
                DataGridViewComboBoxCell dgvcmbUnitCell = (DataGridViewComboBoxCell)dgvProduct.Rows[inRow].Cells[inColumn];
                dgvcmbUnitCell.DataSource = dtbl;
                dgvcmbUnitCell.DisplayMember = "unitName";
                dgvcmbUnitCell.ValueMember = "unitId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO11:" + ex.Message;
            }
        }

        /// <summary>
        /// Cash or party combo box fill
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                isDontExecuteCashorParty = true;
                TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, false);
                cmbCashOrParty.SelectedIndex = 0;
                isDontExecuteCashorParty = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO12:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill the currency combobox
        /// </summary>
        public void CurrencyComboFill()
        {
            try
            {
                SettingsSP spSetting = new SettingsSP();
                DataTable dtblCurrency = new DataTable();
                DateTime dtDate = Convert.ToDateTime(dtpDate.Value);
                TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
                dtblCurrency = TransactionGeneralFillObj.CurrencyComboByDate(dtDate);
                cmbCurrency.DataSource = dtblCurrency;
                cmbCurrency.DisplayMember = "currencyName";
                cmbCurrency.ValueMember = "exchangeRateId";
                cmbCurrency.SelectedValue = 1m;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO13:" + ex.Message;
            }
        }

        /// <summary>
        /// Serial no Generation function for grid
        /// </summary>
        public void SlNo()
        {
            try
            {
                int inRowNo = 1;
                foreach (DataGridViewRow dr in dgvProduct.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowNo;
                    inRowNo++;
                    if (dr.Index == dgvProduct.Rows.Count - 1)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO14:" + ex.Message;
            }
        }

        /// <summary>
        /// Remove function , to remove a row from grid
        /// </summary>
        public void Remove()
        {
            try
            {
                dgvProduct.Rows.RemoveAt(dgvProduct.CurrentRow.Index);
                dgvProduct.CurrentCell = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO15:" + ex.Message;
            }
        }

        /// <summary>
        /// To check the Checkbox status based on settings
        /// </summary>
        /// <returns></returns>
        public void PrintAfterSave()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();

                if (spSettings.SettingsStatusCheck("TickPrintAfterSave") == "Yes")
                {
                    cbxPrintAfterSave.Checked = true;
                }
                else
                {
                    cbxPrintAfterSave.Checked = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO16:" + ex.Message;
            }
        }

        /// <summary>
        /// Print function
        /// </summary>
        /// <param name="decMasterId"></param>
        public void Print(decimal decMasterId)
        {
            try
            {
                DataSet dsRejectionOut = spRejectionOutMaster.RejectionOutPrinting(decMasterId, 1);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.RejectionOutPrinting(dsRejectionOut);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO17:" + ex.Message;
            }
        }

        /// <summary>
        ///  Print function for dotmatrix printer
        /// </summary>
        /// <param name="decMasterId"></param>
        public void PrintForDotMatrix(decimal decReceiptMasterId)
        {
            try
            {
                DataTable dtblOtherDetails = new DataTable();
                CompanySP spComapany = new CompanySP();
                dtblOtherDetails = spComapany.CompanyViewForDotMatrix();
                //-------------Grid Details-------------------\\
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
                foreach (DataGridViewRow dRow in dgvProduct.Rows)
                {
                    if (!dRow.IsNewRow)
                    {
                        DataRow dr = dtblGridDetails.NewRow();
                        dr["SlNo"] = ++inRowCount;
                        if (dRow.Cells["dgvtxtBarcode"].Value != null && dRow.Cells["dgvtxtBarcode"].Value.ToString() != String.Empty)
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

                //-------------Other Details-------------------\\

                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("ledgerName");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("Currency");
                dtblOtherDetails.Columns.Add("TotalAmount");


                dtblOtherDetails.Columns.Add("VoucherType");

                dtblOtherDetails.Columns.Add("CustomerAddress");
                dtblOtherDetails.Columns.Add("CustomerTIN");
                dtblOtherDetails.Columns.Add("CustomerCST");

                dtblOtherDetails.Columns.Add("AmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtRejectionOutNo.Text;
                dRowOther["date"] = txtDate.Text;
                dRowOther["ledgerName"] = cmbCashOrParty.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["Currency"] = cmbCurrency.Text;
                dRowOther["TotalAmount"] = txtTotalAmount.Text;

                dRowOther["VoucherType"] = cmbVoucherType.Text;

                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                infoAccountLedger = spAccountLedger.AccountLedgerView(Convert.ToDecimal(cmbCashOrParty.SelectedValue));
                dRowOther["CustomerAddress"] = (infoAccountLedger.Address.ToString().Replace("\n", ", ")).Replace("\r", "");
                dRowOther["CustomerTIN"] = infoAccountLedger.Tin;
                dRowOther["CustomerCST"] = infoAccountLedger.Cst;


                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtTotalAmount.Text), PublicVariables._decCurrencyId);
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decRejectionOutVoucherTypeId);
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
                formMDI.infoError.ErrorString = "RO18:" + ex.Message;
            }
        }

        /// <summary>
        /// Checking the settings and arrange the form controlls based on settings
        /// </summary>
        public void SettingStatusCheck()
        {
            try
            {
                SettingsSP Spsetting = new SettingsSP();
                if (Spsetting.SettingsStatusCheck("ShowProductCode") == "Yes")
                {
                    dgvProduct.Columns["dgvtxtProductCode"].Visible = true;
                }
                else
                {
                    dgvProduct.Columns["dgvtxtProductCode"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("Barcode") == "Yes")
                {
                    dgvProduct.Columns["dgvtxtBarcode"].Visible = true;
                }
                else
                {
                    dgvProduct.Columns["dgvtxtBarcode"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("ShowUnit") == "Yes")
                {
                    dgvProduct.Columns["dgvcmbUnit"].Visible = true;
                }
                else
                {
                    dgvProduct.Columns["dgvcmbUnit"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("ShowSalesRate") == "Yes")
                {
                    dgvProduct.Columns["dgvtxtRate"].Visible = true;
                }
                else
                {
                    dgvProduct.Columns["dgvtxtRate"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("AllowGodown") == "Yes")
                {
                    dgvProduct.Columns["dgvcmbGodown"].Visible = true;
                    dgvProduct.Columns["dgvcmbRack"].Visible = true;
                }
                else
                {
                    dgvProduct.Columns["dgvcmbGodown"].Visible = false;
                    dgvProduct.Columns["dgvcmbRack"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("AllowRack") == "Yes")
                {
                    if (Spsetting.SettingsStatusCheck("AllowGodown") == "Yes")
                        dgvProduct.Columns["dgvcmbRack"].Visible = true;
                    else
                        dgvProduct.Columns["dgvcmbRack"].Visible = false;
                }
                else
                {
                    dgvProduct.Columns["dgvcmbRack"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("AllowBatch") == "Yes")
                {
                    dgvProduct.Columns["dgvcmbBatch"].Visible = true;
                }
                else
                {
                    dgvProduct.Columns["dgvcmbBatch"].Visible = false;
                }
                if (Spsetting.SettingsStatusCheck("TickPrintAfterSave") == "Yes")
                {
                    cbxPrintAfterSave.Checked = true;
                }
                else
                {
                    cbxPrintAfterSave.Checked = false;
                }
                if (spSettings.SettingsStatusCheck("MultiCurrency") == "Yes")
                {
                    cmbCurrency.Enabled = true;
                }
                else
                {
                    cmbCurrency.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO19:" + ex.Message;
            }
        }
        /// <summary>
        /// To Calculate TotalAmount
        /// </summary>
        private void CalculateTotalAmount()
        {
            try
            {
                ExchangeRateSP spExchangeRate = new ExchangeRateSP();
                decimal decTotal = 0;
                foreach (DataGridViewRow dgvrow in dgvProduct.Rows)
                {
                    if (dgvrow.Cells["dgvtxtAmount"].Value != null && dgvrow.Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                    {
                        decTotal = decTotal + Convert.ToDecimal(Convert.ToString(dgvrow.Cells["dgvtxtAmount"].Value));
                        decSelectedCurrencyRate = spExchangeRate.GetExchangeRateByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                        decTotal = decTotal * decSelectedCurrencyRate;
                        decTotal = Math.Round(decTotal, PublicVariables._inNoOfDecimalPlaces);
                        txtTotalAmount.Text = Convert.ToString(decTotal);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO20:" + ex.Message;
            }
        }

        /// <summary>
        /// To Calculate TotalAmount
        /// </summary>
        public void TotalAmount()
        {
            try
            {
                decimal decTotAmnt = 0;
                foreach (DataGridViewRow dgvrow in dgvProduct.Rows)
                {
                    if (dgvrow.Cells["dgvtxtAmount"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtAmount"].Value.ToString() != string.Empty)
                        {
                            decTotAmnt = decTotAmnt + Convert.ToDecimal(dgvrow.Cells["dgvtxtAmount"].Value.ToString());
                        }
                    }
                }
                decTotAmnt = Math.Round(decTotAmnt, PublicVariables._inNoOfDecimalPlaces);
                txtTotalAmount.Text = decTotAmnt.ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO21:" + ex.Message;
            }
        }


        public void CalcTotalAmt()
        {
            string strCurrencySymbol = string.Empty;

            ExchangeRateSP SpExchangeRate = new ExchangeRateSP();
            CurrencySP SpCurrency = new CurrencySP();
            try
            {
                decimal decTotal = 0;
                if (dgvProduct.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dgvrow in dgvProduct.Rows)
                    {
                        if (dgvrow.Cells["dgvtxtAmount"].Value != null && dgvrow.Cells["dgvtxtAmount"].Value.ToString() != "")
                        {
                            decTotal = decTotal + decimal.Parse(dgvrow.Cells["dgvtxtAmount"].Value.ToString());
                        }
                    }
                    txtTotalAmount.Text = decTotal.ToString();

                }
                else
                {
                    txtTotalAmount.Text = "0.00";

                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO22:" + ex.Message;
            }
        }
        /// <summary>
        /// Amount calculations of grid
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="inIndexOfRow"></param>
        public void NewAmountCalculation(string columnName, int inIndexOfRow)
        {
            try
            {
                decimal decRate = 0;
                decimal decQty = 0;
                decimal decGrossValue = 0;
                if (dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtQty"].Value != null && dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtQty"].Value.ToString() != string.Empty)
                {
                    if (dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtRate"].Value != null && dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtRate"].Value.ToString() != string.Empty)
                    {
                        decimal.TryParse(Convert.ToString(dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtQty"].Value), out decQty);
                        decimal.TryParse(Convert.ToString(dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtRate"].Value), out decRate);
                        decGrossValue = decQty * decRate;
                        dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtAmount"].Value = Math.Round(decGrossValue, PublicVariables._inNoOfDecimalPlaces);
                    }
                    else
                    {
                        dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtAmount"].Value = Math.Round(decGrossValue, PublicVariables._inNoOfDecimalPlaces);
                    }
                }
                else
                {
                    dgvProduct.Rows[inIndexOfRow].Cells["dgvtxtAmount"].Value = Math.Round(decGrossValue, PublicVariables._inNoOfDecimalPlaces);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO23:" + ex.Message;
            }
        }

        /// <summary>
        /// Godown combo box fill
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
                formMDI.infoError.ErrorString = "RO24:" + ex.Message;
            }
        }

        /// <summary>
        /// Rack combo box fill
        /// </summary>
        /// <param name="decGodownId"></param>
        /// <param name="dgvCurProduct"></param>
        /// <param name="inRowIndex"></param>
        public void RackComboFill(decimal decGodownId, DataGridView dgvCurProduct, int inRowIndex)
        {
            try
            {
                RackSP spRack = new RackSP();
                DataTable dtblRack = new DataTable();
                dtblRack = spRack.RackViewAllByGodownForCombo(decGodownId);
                DataGridViewComboBoxCell dgvcmbCurRack = (DataGridViewComboBoxCell)dgvCurProduct[dgvCurProduct.Columns["dgvcmbRack"].Index, inRowIndex];
                dgvcmbCurRack.DataSource = dtblRack;
                dgvcmbCurRack.ValueMember = "rackId";
                dgvcmbCurRack.DisplayMember = "rackName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO25:" + ex.Message;
            }
        }
        /// <summary>
        /// Batch combo box fill
        /// </summary>
        /// <param name="decProductId"></param>
        /// <param name="inRow"></param>
        /// <param name="inColumn"></param>
        public void BatchComboFill(decimal decProductId, int inRow, int inColumn)
        {
            BatchSP spBatch = new BatchSP();
            try
            {
                DataTable dtbl = new DataTable();
                dtbl = spBatch.BatchNamesCorrespondingToProduct(decProductId);
                DataGridViewComboBoxCell dgvcmbBatchCell = (DataGridViewComboBoxCell)dgvProduct.Rows[inRow].Cells[inColumn];
                dgvcmbBatchCell.DataSource = dtbl;
                dgvcmbBatchCell.ValueMember = "batchId";
                dgvcmbBatchCell.DisplayMember = "batchNo";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO26:" + ex.Message;
            }
        }

        /// <summary>
        /// Voucher Type Combobox Fill
        /// </summary>
        public void VoucherTypeComboFill()
        {
            try
            {
                DataTable dtblVouchetType = new DataTable();
                VoucherTypeSP SpVoucherType = new VoucherTypeSP();
                dtblVouchetType = SpVoucherType.VoucherTypeSelectionComboFill("Material Receipt");
                cmbVoucherType.DataSource = dtblVouchetType;
                cmbVoucherType.ValueMember = "voucherTypeId";
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO27:" + ex.Message;
            }
        }

        /// <summary>
        /// It is a function for Add Stock Posting
        /// </summary>
        public void AddStockPosting()
        {
            try
            {
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                StockPostingSP spStockPosting = new StockPostingSP();
                MaterialReceiptMasterInfo infoMaterialReceiptMaster = new MaterialReceiptMasterInfo();
                infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(Convert.ToDecimal(cmbMaterialReceiptNo.SelectedValue));
                infoStockPosting.Date = DateTime.Parse(txtDate.Text);
                infoStockPosting.VoucherTypeId = infoMaterialReceiptMaster.VoucherTypeId;
                infoStockPosting.VoucherNo = infoMaterialReceiptMaster.VoucherNo;
                infoStockPosting.InvoiceNo = infoMaterialReceiptMaster.InvoiceNo;
                if (isAutomatic)
                {
                    infoStockPosting.AgainstVoucherNo = strVoucherNo;
                    infoStockPosting.AgainstInvoiceNo = txtRejectionOutNo.Text;
                }
                else
                {
                    infoStockPosting.AgainstVoucherNo = txtRejectionOutNo.Text;
                    infoStockPosting.AgainstInvoiceNo = txtRejectionOutNo.Text;
                }
                if (decRejectionOutVoucherTypeId != 0)
                {
                    infoStockPosting.AgainstVoucherTypeId = decRejectionOutVoucherTypeId;
                }
                if (strVoucherNo != string.Empty)
                {
                    infoStockPosting.AgainstVoucherNo = strVoucherNo;
                }
                if (txtRejectionOutNo.Text != string.Empty)
                {
                    infoStockPosting.AgainstInvoiceNo = txtRejectionOutNo.Text;
                }
                infoStockPosting.InwardQty = 0;
                infoStockPosting.OutwardQty = infoRejectionOutDetails.Qty;
                infoStockPosting.ProductId = infoRejectionOutDetails.ProductId;
                infoStockPosting.BatchId = infoRejectionOutDetails.BatchId;
                infoStockPosting.UnitId = infoRejectionOutDetails.UnitId;
                infoStockPosting.GodownId = infoRejectionOutDetails.GodownId;
                infoStockPosting.RackId = infoRejectionOutDetails.RackId;
                infoStockPosting.Rate = infoRejectionOutDetails.Rate;
                infoStockPosting.Extra1 = string.Empty;
                infoStockPosting.Extra2 = string.Empty;
                infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                spStockPosting.StockPostingAdd(infoStockPosting);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO28:" + ex.Message;
            }
        }

        /// <summary>
        /// It is a function for CommomInitialSettings 
        /// </summary>
        public void CommomInitialSettings()
        {
            try
            {
                dtpDate.MinDate = PublicVariables._dtFromDate;
                dtpDate.MaxDate = PublicVariables._dtToDate;
                txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                dtpDate.Value = PublicVariables._dtCurrentDate;
                CashOrPartyComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO29:" + ex.Message;
            }
        }

        /// <summary>
        /// RemoveIncompleteRowsFrom dataGrid view  
        /// </summary>
        /// <returns></returns>
        public bool RemoveIncompleteRowsFromGrid()
        {
            //int inI = 0;
            bool isOk = true;
            try
            {
                string strMessage = "Rows";
                int inC = 0, inForFirst = 0;
                int inRowcount = dgvProduct.RowCount;
                foreach (DataGridViewRow dgvRow in dgvProduct.Rows)
                {
                    try
                    {
                        if (Convert.ToDecimal(dgvRow.Cells["dgvtxtQty"].Value.ToString()) <= 0 || dgvRow.Cells["dgvtxtQty"].Value == null)
                        {
                            isOk = false;
                            if (inC == 0)
                            {
                                strMessage = strMessage + Convert.ToString(dgvRow.Index + 1);
                                inForFirst = dgvRow.Index;
                                inC++;
                            }
                            else
                            {
                                strMessage = strMessage + ", " + Convert.ToString(dgvRow.Index + 1);
                            }
                        }
                    }
                    catch
                    {
                        isOk = false;
                        if (inC == 0)
                        {
                            strMessage = strMessage + Convert.ToString(dgvRow.Index + 1);
                            inForFirst = dgvRow.Index;
                            inC++;
                        }
                        else
                        {
                            strMessage = strMessage + ", " + Convert.ToString(dgvRow.Index + 1);
                        }
                    }
                }
                if (!isOk)
                {
                    strMessage = strMessage + " contains invalid entries. Do you want to continue?";
                    if (MessageBox.Show(strMessage, "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        isOk = true;
                        for (int inK = 0; inK < dgvProduct.Rows.Count; inK++)
                        {
                            try
                            {
                                if (Convert.ToDecimal(dgvProduct.Rows[inK].Cells["dgvtxtQty"].Value.ToString()) <= 0 || dgvProduct.Rows[inK].Cells["dgvtxtQty"].Value == null)
                                {
                                    dgvProduct.Rows.RemoveAt(inK);
                                    inK--;
                                    CalcTotalAmt();
                                }
                            }
                            catch
                            {
                                dgvProduct.Rows.RemoveAt(inK);
                                inK--;
                                CalcTotalAmt();
                            }
                        }
                        if (dgvProduct.RowCount < 1)
                        {
                            isOk = false;
                            if (decRejectionOutId == 0)
                            {
                                MessageBox.Show("Can't save rejection in without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Can't update rejection in without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        dgvProduct.Rows[inForFirst].Cells["dgvtxtProductName"].Selected = true;
                        dgvProduct.CurrentCell = dgvProduct.Rows[inForFirst].Cells["dgvtxtProductName"];
                        dgvProduct.Focus();
                    }
                }
                SlNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO30:" + ex.Message;
            }
            return isOk;
        }

        /// <summary>
        /// Save Function
        /// </summary>
        /// <summary>
        /// Function for Grid HeaderStyle For Invalid Entries of grid
        /// </summary>
        public void gridHeaderStyleForInvalidEntries()
        {
            try
            {
                isValueChange = true;
                dgvProduct.CurrentRow.HeaderCell.Value = "X";
                dgvProduct.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO31:" + ex.Message;
            }

        }

        /// <summary>
        /// Function To check whether the values in grid is valid
        /// </summary>
        public void CheckInvalidEntries(DataGridViewCellEventArgs e)
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();
                if (dgvProduct.CurrentRow != null)
                {
                    if (!isValueChange)
                    {
                        if (dgvProduct.CurrentRow.Cells["dgvtxtQty"].Value == null ||
                                 dgvProduct.CurrentRow.Cells["dgvtxtQty"].Value.ToString().Trim() == string.Empty ||
                           Convert.ToDecimal(dgvProduct.CurrentRow.Cells["dgvtxtQty"].Value.ToString()) == 0)
                        {
                            gridHeaderStyleForInvalidEntries();
                        }
                        else if (spSettings.SettingsStatusCheck("AllowZeroValueEntry") == "No" && Convert.ToDecimal(dgvProduct.CurrentRow.Cells["dgvtxtrate"].Value) == 0)  /*error*/
                        {
                            gridHeaderStyleForInvalidEntries();
                        }
                        else if (dgvProduct.CurrentRow.Cells["dgvtxtRate"].Value == null ||
                                 dgvProduct.CurrentRow.Cells["dgvtxtRate"].Value.ToString().Trim() == string.Empty)
                        {
                            gridHeaderStyleForInvalidEntries();
                        }
                        else
                        {
                            isValueChange = true;
                            dgvProduct.CurrentRow.HeaderCell.Value = string.Empty;
                        }
                    }
                    isValueChange = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO32:" + ex.Message;
            }
        }

        public void SaveFunction()
        {
            try
            {

                ProductSP spProduct = new ProductSP();
                SettingsSP Spsetting = new SettingsSP();
                infoRejectionOutMaster.Date = Convert.ToDateTime(txtDate.Text);
                infoRejectionOutMaster.LedgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());

                if (isAutomatic == true)
                {
                    infoRejectionOutMaster.SuffixPrefixId = decRejectionOutSuffixPrefixId;
                    infoRejectionOutMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoRejectionOutMaster.SuffixPrefixId = 0;
                    infoRejectionOutMaster.VoucherNo = txtRejectionOutNo.Text.Trim();
                }

                if (cmbMaterialReceiptNo.SelectedValue != null)
                {
                    infoRejectionOutMaster.MaterialReceiptMasterId = Convert.ToDecimal(Convert.ToString(cmbMaterialReceiptNo.SelectedValue));
                }
                else
                {
                    infoRejectionOutMaster.MaterialReceiptMasterId = 0;
                }

                if (cmbCurrency.SelectedValue != null)
                {
                    infoRejectionOutMaster.ExchangeRateId = Convert.ToDecimal(Convert.ToString(cmbCurrency.SelectedValue));
                }
                infoRejectionOutMaster.VoucherTypeId = decRejectionOutVoucherTypeId;

                infoRejectionOutMaster.InvoiceNo = txtRejectionOutNo.Text.Trim();
                infoRejectionOutMaster.UserId = PublicVariables._decCurrentUserId;
                infoRejectionOutMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoRejectionOutMaster.Narration = txtNarration.Text.Trim();
                infoRejectionOutMaster.LrNo = txtLrNo.Text.Trim();
                infoRejectionOutMaster.TransportationCompany = txtTransportationCompany.Text.Trim();
                infoRejectionOutMaster.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                infoRejectionOutMaster.Extra1 = string.Empty;
                infoRejectionOutMaster.Extra2 = string.Empty;
                if (decRejectionOutId == 0)
                {
                    decRejectionOutMasterIdentity = spRejectionOutMaster.RejectionOutMasterAddWithReturnIdentity(infoRejectionOutMaster);
                }
                else
                {
                    infoRejectionOutMaster.RejectionOutMasterId = decRejectionOutId;
                    spRejectionOutMaster.RejectionOutMasterEdit(infoRejectionOutMaster);
                }
                if (decRejectionOutId == 0)
                {
                    infoRejectionOutDetails.RejectionOutMasterId = decRejectionOutMasterIdentity;
                }
                else
                {
                    spRejectionOutDetails.RejectionOutDetailsDeleteByRejectionOutMasterId(decRejectionOutId);
                    spStockPosting.DeleteStockPostingByAgnstVouTypeIdAndAgnstVouNo(decRejectionOutVoucherTypeId, strRejectionOutVoucherNo);
                    infoRejectionOutDetails.RejectionOutMasterId = decRejectionOutId;
                }
                int inRowcount = dgvProduct.Rows.Count;
                for (int inI = 0; inI <= inRowcount - 1; inI++)
                {

                    if (dgvProduct.Rows[inI].Cells["dgvtxtSlNo"].Value != null && dgvProduct.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.Slno = Convert.ToInt32(dgvProduct.Rows[inI].Cells["dgvtxtSlNo"].Value.ToString());
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvtxtmaterialReceiptDetailsId"].Value != null && dgvProduct.Rows[inI].Cells["dgvtxtmaterialReceiptDetailsId"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.MaterialReceiptDetailsId = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvtxtmaterialReceiptDetailsId"].Value.ToString());
                    }
                    else
                    {
                        infoRejectionOutDetails.MaterialReceiptDetailsId = 0;
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvtxtProductCode"].Value != null && dgvProduct.Rows[inI].Cells["dgvtxtProductCode"].Value.ToString() != string.Empty)
                    {
                        infoProduct = spProduct.ProductViewByCode(dgvProduct.Rows[inI].Cells["dgvtxtProductCode"].Value.ToString());
                        infoRejectionOutDetails.ProductId = infoProduct.ProductId;
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvtxtQty"].Value != null && dgvProduct.Rows[inI].Cells["dgvtxtQty"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.Qty = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvtxtQty"].Value.ToString());
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvcmbUnit"].Value != null && dgvProduct.Rows[inI].Cells["dgvcmbUnit"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.UnitId = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvcmbUnit"].Value.ToString());
                        infoRejectionOutDetails.UnitConversionId = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvtxtunitConversionId"].Value.ToString());
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvcmbBatch"].Value != null && dgvProduct.Rows[inI].Cells["dgvcmbBatch"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.BatchId = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvcmbBatch"].Value);
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvcmbGodown"].Value != null && dgvProduct.Rows[inI].Cells["dgvcmbGodown"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.GodownId = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvcmbGodown"].Value);
                    }
                    if (dgvProduct.Rows[inI].Cells["dgvcmbRack"].Value != null && dgvProduct.Rows[inI].Cells["dgvcmbRack"].Value.ToString() != string.Empty)
                    {
                        infoRejectionOutDetails.RackId = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvcmbRack"].Value);
                    }
                    infoRejectionOutDetails.Rate = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvtxtRate"].Value.ToString());
                    infoRejectionOutDetails.Amount = Convert.ToDecimal(dgvProduct.Rows[inI].Cells["dgvtxtAmount"].Value.ToString());
                    infoRejectionOutDetails.Extra1 = string.Empty;
                    infoRejectionOutDetails.Extra2 = string.Empty;
                    decRejectionOutDetailsIdentity = spRejectionOutDetails.RejectionOutDetailsAddWithReturnIdentity(infoRejectionOutDetails);
                    AddStockPosting();
                }

                if (decRejectionOutId == 0)
                {
                    Messages.SavedMessage();
                    if (cbxPrintAfterSave.Checked)
                    {
                        if (Spsetting.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(decRejectionOutMasterIdentity);
                        }
                        else
                        {
                            Print(decRejectionOutMasterIdentity);
                        }
                    }
                    clear();
                }
                else
                {
                    Messages.UpdatedMessage();
                    if (cbxPrintAfterSave.Checked)
                    {
                        if (Spsetting.SettingsStatusCheck("Printer") == "Dot Matrix")
                        {
                            PrintForDotMatrix(decRejectionOutId);
                        }
                        else
                        {
                            Print(decRejectionOutId);
                        }
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO33:" + ex.Message;
            }
        }
        /// <summary>
        /// To Remove Rejection Out Details or Grid
        /// </summary>
        public void RemoveRejectionOutDetails()
        {
            try
            {
                foreach (var strId in lstArrOfRemove)
                {
                    decimal decDeleteId = Convert.ToDecimal(strId);
                    spRejectionOutDetails.RejectionOutDetailsDelete(decDeleteId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO34:" + ex.Message;
            }
        }

        /// <summary>
        /// Save or edit function , to check invalid entries
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                int inRow = dgvProduct.RowCount;
                if (txtRejectionOutNo.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter voucher number");
                    txtRejectionOutNo.Focus();
                }
                else if (spRejectionOutMaster.RejectionOutNumberCheckExistence(txtRejectionOutNo.Text.Trim(), 0, decRejectionOutVoucherTypeId) == true && btnSave.Text == "Save")
                {
                    Messages.InformationMessage("Rejection Out number already exist");
                    txtRejectionOutNo.Focus();
                }
                else if (txtDate.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Select a date in between financial year");
                    txtDate.Focus();
                }
                else if (cmbCashOrParty.SelectedValue == null)
                {
                    Messages.InformationMessage("Select Cash/Party");
                    cmbCashOrParty.Focus();
                }
                else if (cmbVoucherType.SelectedValue == null)
                {
                    Messages.InformationMessage("Select Voucher Type");
                    cmbVoucherType.Focus();
                }
                else if (cmbMaterialReceiptNo.SelectedValue == null)
                {
                    Messages.InformationMessage("Select MaterialRecieptNo");
                    cmbMaterialReceiptNo.Focus();
                }
                else if (cmbCurrency.SelectedValue == null)
                {
                    Messages.InformationMessage("Select Currency");
                    cmbCurrency.Focus();
                }
                else if (inRow <= 0)
                {
                    Messages.InformationMessage("Can't save rejection out without atleast one product with complete details");
                }
                else
                {
                    if (RemoveIncompleteRowsFromGrid())
                    {
                        if (dgvProduct.Rows[0].Cells["dgvtxtProductName"].Value == null && dgvProduct.Rows[0].Cells["dgvtxtProductCode"].Value == null)
                        {
                            MessageBox.Show("Can't save rejection out without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvProduct.ClearSelection();
                            dgvProduct.Focus();
                        }
                        else
                        {
                            if (decRejectionOutId == 0)
                            {
                                if (dgvProduct.Rows[0].Cells["dgvtxtProductName"].Value == null)
                                {
                                    MessageBox.Show("Can't save rejection out without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    dgvProduct.ClearSelection();
                                    dgvProduct.Focus();
                                }
                                else if (btnSave.Text == "Save")
                                {
                                    if (Messages.SaveConfirmation())
                                    {
                                        SaveFunction();
                                    }
                                }
                            }
                            else if (btnSave.Text == "Update")
                            {
                                if (Messages.UpdateConfirmation())
                                {
                                    SaveFunction();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO35:" + ex.Message;
            }
        }

        /// <summary>
        /// Fill function for edit
        /// </summary>
        public void FillRejectionOutReportForEdit()
        {
            try
            {
                dgvProduct.Rows.Clear();
                isDoCellValueChange = false;
                isFromRegister = true;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtRejectionOutNo.ReadOnly = true;
                infoRejectionOutMaster = spRejectionOutMaster.RejectionOutMasterView(decRejectionOutId);
                MaterialReceiptMasterSP SpMaterialReceiptMaster = new MaterialReceiptMasterSP();
                MaterialReceiptMasterInfo InfoMaterialReceiptMaster = new MaterialReceiptMasterInfo();
                VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
                InfoMaterialReceiptMaster = SpMaterialReceiptMaster.MaterialReceiptMasterView(infoRejectionOutMaster.MaterialReceiptMasterId);
                strRejectionOutVoucherNo = spRejectionOutMaster.GetRejectionOutVoucherNo(infoRejectionOutMaster.RejectionOutMasterId);
                txtRejectionOutNo.Text = infoRejectionOutMaster.InvoiceNo;
                strVoucherNo = infoRejectionOutMaster.VoucherNo.ToString();
                decRejectionOutSuffixPrefixId = Convert.ToDecimal(infoRejectionOutMaster.SuffixPrefixId);
                decRejectionOutVoucherTypeId = Convert.ToDecimal(infoRejectionOutMaster.VoucherTypeId);
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decRejectionOutVoucherTypeId);
                infoVoucherType = spVoucherType.VoucherTypeView(decRejectionOutVoucherTypeId);
                this.Text = infoVoucherType.VoucherTypeName;
                if (!isAutomatic)
                {
                    txtRejectionOutNo.ReadOnly = false;
                    txtRejectionOutNo.Focus();
                }
                else
                {
                    txtRejectionOutNo.ReadOnly = true;
                    txtDate.Focus();
                }
                decRejectionOutTypeId = decRejectionOutVoucherTypeId;
                txtDate.Text = infoRejectionOutMaster.Date.ToString("dd-MMM-yyyy");
                cmbCashOrParty.SelectedValue = infoRejectionOutMaster.LedgerId;
                cmbVoucherType.SelectedValue = InfoMaterialReceiptMaster.VoucherTypeId;
                cmbMaterialReceiptNo.SelectedValue = Convert.ToDecimal(infoRejectionOutMaster.MaterialReceiptMasterId.ToString());
                txtTransportationCompany.Text = infoRejectionOutMaster.TransportationCompany;
                txtNarration.Text = infoRejectionOutMaster.Narration;
                cmbCurrency.SelectedValue = infoRejectionOutMaster.ExchangeRateId;
                txtTotalAmount.Text = infoRejectionOutMaster.TotalAmount.ToString();
                txtLrNo.Text = infoRejectionOutMaster.LrNo.ToString();
                DataTable dtblRejectionOut = spRejectionOutDetails.RejectionOutDetailsViewByRejectionOutMasterId(infoRejectionOutMaster.RejectionOutMasterId);
                for (int i = 0; i < dtblRejectionOut.Rows.Count; i++)
                {
                    dgvProduct.Rows.Add();
                    GodownComboFill();
                    isAmountcalc = false;
                    decRejectionOutDetailId = Convert.ToDecimal(dtblRejectionOut.Rows[i]["rejectionOutDetailsId"].ToString());
                    dgvProduct.Rows[i].Cells["dgvtxtSlNo"].Value = dtblRejectionOut.Rows[i]["slno"].ToString();
                    dgvProduct.Rows[i].Cells["dgvtxtProductId"].Value = dtblRejectionOut.Rows[i]["productId"].ToString();
                    ProductDefaultValues(i, Convert.ToDecimal(dtblRejectionOut.Rows[i]["productId"].ToString()));
                    BatchComboFill(Convert.ToDecimal(dtblRejectionOut.Rows[i]["productId"].ToString()), i, dgvProduct.Rows[i].Cells["dgvcmbBatch"].ColumnIndex);
                    dgvProduct.Rows[i].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(dtblRejectionOut.Rows[i]["batchId"].ToString());
                    dgvProduct.Rows[i].Cells["dgvtxtQty"].Value = dtblRejectionOut.Rows[i]["qty"].ToString();
                    UnitComboFill(Convert.ToDecimal(dtblRejectionOut.Rows[i]["productId"].ToString()), i, dgvProduct.Rows[i].Cells["dgvcmbUnit"].ColumnIndex);
                    isDoCellValueChange = true;
                    isDoAfterGridFill = true;
                    isValueChange = true;
                    isAmountcalc = true;
                    dgvProduct.Rows[i].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(dtblRejectionOut.Rows[i]["unitId"].ToString());
                    isDoCellValueChange = false;
                    isDoAfterGridFill = false;
                    isValueChange = false;
                    dgvProduct.Rows[i].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(dtblRejectionOut.Rows[i]["godownId"].ToString());
                    RackComboFill(Convert.ToDecimal(dtblRejectionOut.Rows[i]["godownId"].ToString()), dgvProduct, i);
                    dgvProduct.Rows[i].Cells["dgvcmbRack"].Value = Convert.ToDecimal(dtblRejectionOut.Rows[i]["rackId"].ToString());
                    dgvProduct.Rows[i].Cells["dgvtxtBarcode"].Value = dtblRejectionOut.Rows[i]["barcode"].ToString();
                    dgvProduct.Rows[i].Cells["dgvtxtRejectionOutDetailsId"].Value = decRejectionOutDetailId;
                    dgvProduct.Rows[i].Cells["dgvtxtRate"].Value = dtblRejectionOut.Rows[i]["rate"].ToString();
                    dgvProduct.Rows[i].Cells["dgvtxtAmount"].Value = dtblRejectionOut.Rows[i]["amount"].ToString();
                    dgvProduct.Rows[i].Cells["dgvtxtunitConversionId"].Value = dtblRejectionOut.Rows[i]["UnitConversionId"].ToString();
                    dgvProduct.Rows[i].Cells["dgvtxtconversionRate"].Value = dtblRejectionOut.Rows[i]["conversionRate"].ToString();
                    dgvProduct.Rows[i].Cells["dgvtxtmaterialReceiptDetailsId"].Value = dtblRejectionOut.Rows[i]["materialReceiptDetailsId"].ToString();
                }
                TotalAmount();
                isAmountcalc = true;
                isDoAfterGridFill = true;
                isFromRegister = false;
                isDoCellValueChange = true;
                isValueChange = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO36:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete function
        /// </summary>
        public void Delete()
        {
            try
            {
                spRejectionOutMaster.RejectionOutMasterAndDetailsDelete(decRejectionOutId);
                spStockPosting.DeleteStockPostingByAgnstVouTypeIdAndAgnstVouNo(decRejectionOutVoucherTypeId, strRejectionOutVoucherNo);
                Messages.DeletedMessage();
                this.Close();
                if (objVoucherSearch != null)
                {
                    this.Close();
                    objVoucherSearch.GridFill();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO37:" + ex.Message;
            }
        }
        /// <summary>
        /// Fill Function For MaterialReciept
        /// </summary>
        public void MaterialReceiptComboFill()
        {
            try
            {
                label2.Visible = true;
                isOrderFill = true;
                DataTable dtbl = new DataTable();
                if (cmbCashOrParty.Items.Count > 0 && cmbVoucherType.Items.Count > 0)
                {
                    if (cmbCashOrParty.SelectedValue != null && cmbVoucherType.SelectedValue != null)
                    {
                        decimal decLedgerIdInCashOrParty = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                        decimal decVoucherTypeId = Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString());
                        dtbl = spMaterialReceiptMaster.ShowMaterialReceiptNoForRejectionOut(decLedgerIdInCashOrParty, decRejectionOutId, decVoucherTypeId);
                        cmbMaterialReceiptNo.DataSource = dtbl;
                        if (cmbMaterialReceiptNo.DataSource != null)
                        {
                            cmbMaterialReceiptNo.DisplayMember = "invoiceNo";
                            cmbMaterialReceiptNo.ValueMember = "materialReceiptMasterId";
                            cmbMaterialReceiptNo.SelectedIndex = -1;
                        }
                        isOrderFill = false;
                        GodownComboFill();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO38:" + ex.Message;
            }
        }
        /// <summary>
        /// To Get Default Product Values
        /// </summary>
        /// <param name="inindex"></param>
        /// <param name="decproductId"></param>
        public void MaterialReceiptComboFillDetails()
        {
            try
            {
                dgvProduct.Rows.Clear();
                DataTable dtblReceiptDetails = spMaterialReceiptDetails.ShowMaterialReceiptDetailsViewbyMaterialReceiptDetailsIdWithPending(Convert.ToDecimal(cmbMaterialReceiptNo.SelectedValue.ToString()), decRejectionOutId);
                foreach (DataRow drowDetails in dtblReceiptDetails.Rows)
                {
                    isDoAfterGridFill = false;
                    isDoCellValueChange = false;
                    dgvProduct.Rows.Add();
                    GodownComboFill();
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtproductId"].Value = drowDetails["productId"];
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtmaterialReceiptDetailsId"].Value = drowDetails["materialReceiptDetailsId"];
                    ProductDefaultValues(dgvProduct.Rows.Count - 1, Convert.ToDecimal(drowDetails.ItemArray[2].ToString()));
                    BatchComboFill(Convert.ToDecimal(drowDetails["productId"]), dgvProduct.Rows.Count - 1, dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvcmbBatch"].ColumnIndex);
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(drowDetails["batchId"]);
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(drowDetails["godownId"]);
                    RackComboFill(decimal.Parse(drowDetails["godownId"].ToString() == "0" ? "1" : drowDetails["godownId"].ToString()), dgvProduct, dgvProduct.Rows.Count - 1);
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvcmbRack"].Value = Convert.ToDecimal(drowDetails["rackId"]);
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtBarcode"].Value = drowDetails["barcode"];
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtQty"].Value = drowDetails["qty"];
                    isDoCellValueChange = true;
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(drowDetails["unitId"]);
                    isDoCellValueChange = true;
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtRate"].Value = drowDetails["rate"];
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtAmount"].Value = drowDetails["amount"]; ;
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtunitConversionId"].Value = drowDetails["unitConversionId"];
                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["dgvtxtconversionRate"].Value = drowDetails["conversionRate"];
                    isValueChange = true;
                }
                TotalAmount();
                //GridviewReadOnlySettings("Against MaterialReceipt");
                isDoCellValueChange = true;
                isDoAfterGridFill = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO39:" + ex.Message;
            }
        }

        /// <summary>
        /// To Get Default Product Values
        /// </summary>
        /// <param name="inindex"></param>
        /// <param name="decproductId"></param>
        public void ProductDefaultValues(int inindex, decimal decproductId)
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                isValueChange = false;
                infoProduct = spProduct.ProductView(decproductId);
                dgvProduct.Rows[inindex].Cells["dgvtxtProductCode"].Value = infoProduct.ProductCode;
                dgvProduct.Rows[inindex].Cells["dgvtxtProductName"].Value = infoProduct.ProductName;
                TransactionsGeneralFill obj = new TransactionsGeneralFill();
                DataTable dtblUnitViewall = new DataTable();
                dtblUnitViewall = obj.UnitViewAllByProductId(dgvProduct, decproductId.ToString(), inindex);
                BatchSP spBatch = new BatchSP();
                spBatch.BatchViewbyProductIdForComboFillInGrid(decproductId, dgvProduct, inindex);
                isValueChange = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO40:" + ex.Message;
            }
        }
        /// <summary>
        ///  For GridView ReadOnly
        /// </summary>
        public void GridviewReadOnlySettings(string strPurchaseMode)
        {
            try
            {
                int inI = 0;
                foreach (DataGridViewRow dgvRow in dgvProduct.Rows)
                {
                    if (dgvRow.Cells["dgvtxtProductId"].Value != null)
                    {
                        dgvRow.Cells["dgvtxtProductCode"].ReadOnly = true;
                        dgvRow.Cells["dgvtxtProductName"].ReadOnly = true;
                        dgvRow.Cells["dgvtxtBarcode"].ReadOnly = true;
                        if (strPurchaseMode == "Against MaterialReceipt")
                        {
                            dgvRow.Cells["dgvcmbUnit"].ReadOnly = true;
                            dgvRow.Cells["dgvcmbGodown"].ReadOnly = true;
                            dgvRow.Cells["dgvcmbRack"].ReadOnly = true;
                            dgvRow.Cells["dgvcmbBatch"].ReadOnly = true;
                        }
                    }
                    NewAmountCalculation("dgvtxtAmount", inI);
                    TotalAmount();
                    inI++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO41:" + ex.Message;
            }
        }

        /// <summary>
        /// Function to fill Account ledger combobox while return from ledger creation when creating new ledger 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromAccountLedgerForm(decimal decLedgerId)
        {
            AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
            AccountLedgerSP spAccountLedger = new AccountLedgerSP();
            RejectionInMasterSP spRejectionOutMaster = new RejectionInMasterSP();
            DataTable dtbl = new DataTable();
            try
            {
                CashOrPartyComboFill();
                if (decLedgerId.ToString() != "0")
                {
                    cmbCashOrParty.SelectedValue = decLedgerId;
                    cmbCashOrParty.Focus();
                }
                else if (strCashOrParty != string.Empty)
                {
                    cmbCashOrParty.SelectedItem = strCashOrParty;
                    cmbCashOrParty.Focus();
                }
                else
                {
                    cmbCashOrParty.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO42:" + ex.Message;
            }
        }
        /// <summary>
        ///  Save or edit function to checking the negative stock status
        /// </summary>
        public void SaveOrEditFunction()
        {
            try
            {
                decimal decProductId = 0;
                decimal decBatchId = 0;
                decimal decCalcQty = 0;
                StockPostingSP spStockPosting = new StockPostingSP();
                SettingsSP spSettings = new SettingsSP();
                string strStatus = spSettings.SettingsStatusCheck("NegativeStockStatus");
                bool isNegativeLedger = false;
                int inRowCount = dgvProduct.RowCount;
                for (int i = 0; i < inRowCount; i++)
                {
                    if (dgvProduct.Rows[i].Cells["dgvtxtproductId"].Value != null && dgvProduct.Rows[i].Cells["dgvtxtproductId"].Value.ToString() != string.Empty)
                    {
                        decProductId = Convert.ToDecimal(dgvProduct.Rows[i].Cells["dgvtxtproductId"].Value.ToString());

                        if (dgvProduct.Rows[i].Cells["dgvcmbBatch"].Value != null && dgvProduct.Rows[i].Cells["dgvcmbBatch"].Value.ToString() != string.Empty)
                        {
                            decBatchId = Convert.ToDecimal(dgvProduct.Rows[i].Cells["dgvcmbBatch"].Value.ToString());
                        }
                        decimal decCurrentStock = spStockPosting.StockCheckForProductSale(decProductId, decBatchId);
                        if (dgvProduct.Rows[i].Cells["dgvtxtQty"].Value != null && dgvProduct.Rows[i].Cells["dgvtxtQty"].Value.ToString() != string.Empty)
                        {
                            decCalcQty = decCurrentStock - Convert.ToDecimal(dgvProduct.Rows[i].Cells["dgvtxtQty"].Value.ToString());
                        }
                        if (decCalcQty < 0)
                        {
                            isNegativeLedger = true;
                            break;
                        }
                    }
                }
                if (isNegativeLedger)
                {
                    if (strStatus == "Warn")
                    {
                        if (MessageBox.Show("Negative Stock balance exists,Do you want to Continue", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            SaveOrEdit();
                        }
                    }
                    else if (strStatus == "Block")
                    {
                        MessageBox.Show("Cannot continue ,due to negative stock balance", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        SaveOrEdit();
                    }
                }
                else
                {
                    SaveOrEdit();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO43:" + ex.Message;
            }
        }
       
        #endregion

        #region Events

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
                formMDI.infoError.ErrorString = "RO44:" + ex.Message;
            }
        }
        /// <summary>
        /// Keypress event for Decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCellEditControlKeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvProduct.CurrentCell != null)
                {
                    if (dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtQty" || dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtRate")
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO45:" + ex.Message;
            }
        }
        /// <summary>
        /// To add a new ledger using this button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLedgerAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCashOrParty.SelectedValue != null)
                {
                    strCashOrParty = cmbCashOrParty.SelectedValue.ToString();
                }
                else
                {
                    strCashOrParty = string.Empty;
                }
                frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();
                frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                if (open == null)
                {
                    frmAccountLedgerObj.WindowState = FormWindowState.Normal;
                    frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                    frmAccountLedgerObj.callFromRejectionOut(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.callFromRejectionOut(this);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }

                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO46:" + ex.Message;
            }
        }
        /// <summary>
        /// Form load, call the clear function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRejectionOut_Load(object sender, EventArgs e)
        {
            try
            {
                lblMaterialReceiptNo.Visible = false;
                cmbMaterialReceiptNo.Visible = false;
                clear();
                SettingStatusCheck();
                txtTotalAmount.Text = "0.00";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO47:" + ex.Message;
            }

        }
        /// <summary>
        /// Set the dtp value as textbox value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpRejectionOutDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpDate.Value;
                this.txtDate.Text = date.ToString("dd-MMM-yyyy");
                CurrencyComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO48:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the seial no generation function in row added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                SlNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO49:" + ex.Message;
            }
        }
        /// <summary>
        /// Call the meterial reciept 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!isDontExecuteCashorParty)
            {
                dgvProduct.Rows.Clear();
                if (cmbCashOrParty.SelectedValue != null && cmbVoucherType.SelectedValue != null)
                {
                    if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashOrParty.Text != "System.Data.DataRowView" && cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView" && cmbVoucherType.Text != "System.Data.DataRowView")
                    {
                        lblMaterialReceiptNo.Visible = true;
                        cmbMaterialReceiptNo.Visible = true;
                        MaterialReceiptComboFill();
                    }
                }
            }
        }

        /// <summary>
        /// Clear button click
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
                formMDI.infoError.ErrorString = "RO50:" + ex.Message;
            }

        }
        /// <summary>
        /// Remove button click, call the remove function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dgvProduct.Rows.Count == 0)
                {
                    MessageBox.Show("No row to remove!", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (dgvProduct.SelectedCells.Count > 0 && dgvProduct.CurrentRow != null)
                {
                    if (!dgvProduct.Rows[dgvProduct.CurrentRow.Index].IsNewRow)
                    {
                        if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Remove();
                            //dgvProduct.ClearSelection();
                            dgvProduct.Focus();
                            CalculateTotalAmount();

                        }
                    }
                }
                SlNo();
                TotalAmount();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO51:" + ex.Message;
            }

        }
        /// <summary>
        /// MaterialReceipt ComboFill Details in Index change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMaterialReceiptNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isFromRegister)
                {
                    if (!isOrderFill)
                    {
                        if (cmbMaterialReceiptNo.SelectedIndex > -1)
                        {
                            if (cmbMaterialReceiptNo.Text != "System.Data.DataRowView" && cmbMaterialReceiptNo.SelectedValue.ToString() != "System.Data.DataRowView")
                            {
                                decTotAmnt = 0;
                                MaterialReceiptComboFillDetails();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO52:" + ex.Message;
            }
        }
        /// <summary>
        /// to handle unhandle exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                e.ThrowException = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO53:" + ex.Message;
            }

        }

        /// <summary>
        /// Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    if (RemoveIncompleteRowsFromGrid())
                    {

                        if (decRejectionOutId == 0)
                        {
                            SaveOrEditFunction();
                        }
                        else
                        {
                            SaveOrEditFunction();
                        }
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO54:" + ex.Message;
            }
        }
        /// <summary>
        /// Delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    if (PublicVariables.isMessageDelete)
                    {
                        if (Messages.DeleteMessage())
                        {
                            Delete();
                        }
                    }
                    else
                    {
                        Delete();
                    }
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO55:" + ex.Message;
            }

        }
        /// <summary>
        /// to set the currency index change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_TextChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            try
            {
                if (cmb.Text.Trim() == string.Empty)
                {
                    cmb.Text = cmb.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO56:" + ex.Message;
            }
        }
        /// <summary>
        /// To handle the keypress event for validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {

                TextBoxControl = e.Control as DataGridViewTextBoxEditingControl;
                if (TextBoxControl != null)
                {

                    if (dgvProduct.CurrentCell != null && dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtProductName")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = ProductNames;

                    }
                    if (dgvProduct.CurrentCell != null && dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = ProductCodes;

                    }
                    if (dgvProduct.CurrentCell != null && dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name != "dgvtxtProductCode" && dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name != "dgvtxtProductName")
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)dgvProduct.EditingControl;
                        editControl.AutoCompleteMode = AutoCompleteMode.None;
                    }
                    TextBoxControl.KeyPress += TextBoxCellEditControlKeyPress;
                    if (dgvProduct.CurrentCell.ColumnIndex == dgvProduct.Columns["dgvtxtAmount"].Index)
                    {
                        TextBoxControl.KeyPress += keypressevent;
                    }
                    else if (dgvProduct.CurrentCell.ColumnIndex == dgvProduct.Columns["dgvtxtQty"].Index)
                    {
                        TextBoxControl.KeyPress += keypressevent;

                    }
                    else if (dgvProduct.CurrentCell.ColumnIndex == dgvProduct.Columns["dgvtxtRate"].Index)
                    {
                        TextBoxControl.KeyPress += keypressevent;
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO57:" + ex.Message;
            }
        }
        /// <summary>
        /// key press event for decimal validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keypressevent(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtQty")
                {
                    Common.DecimalValidation(sender, e, false);
                }
                else
                {
                    Common.DecimalValidation(sender, e, false);

                }


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO58:" + ex.Message;
            }
        }
        /// <summary>
        /// MaterialReceipt ComboFill
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_SelectedValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                strCashOrParty = cmbCashOrParty.Text;
                dgvProduct.Rows.Clear();
                lblMaterialReceiptNo.Visible = false;
                cmbMaterialReceiptNo.Visible = false;
                cmbVoucherType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO59:" + ex.Message;
            }
        }
        /// <summary>
        /// Grid combo fill functions and basic calculations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
       
        /// <summary>
        /// Date validation and Setting the dtp value as textbox value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtDate);
                if (!isInvalid)
                {
                    txtDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtDate.Text;
                dtpDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO60:" + ex.Message;
            }
        }
        /// <summary>
        /// Cell enter for unit conversion and get qty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            decimal decNewQty = 0;
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    if (dgvProduct.Columns[e.ColumnIndex].Name == "dgvtxtQty")
                    {
                        if (dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value.ToString() != string.Empty)
                        {
                            decNewQty = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value.ToString());
                        }
                        else
                        {
                            decNewQty = 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO61:" + ex.Message;
            }

        }
        /// <summary>
        /// enables the enter key navigation for Cell EndEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            decimal decCurQty = 0;
            decimal decNewAmount = 0;
            try
            {
                // CheckInvalidEntries(e);
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                if (isValueChange)
                {
                    if (isDoCellValueChange)
                    {
                        if (dgvProduct.Rows.Count > 0)
                        {
                            if (e.ColumnIndex == dgvProduct.Columns["dgvcmbRack"].Index)
                            {
                                if (dgvProduct.Columns[e.ColumnIndex].Name == "dgvcmbGodown")
                                {
                                    if (dgvProduct.Rows[e.RowIndex].Cells["dgvcmbGodown"].Value != null)
                                    {
                                        RackComboFill(Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvcmbGodown"].Value.ToString()), dgvProduct, e.RowIndex);
                                    }
                                }
                            }
                            if (dgvProduct.Columns[e.ColumnIndex].Name == "dgvcmbBatch")
                            {
                                if (dgvProduct.Rows[e.RowIndex].Cells["dgvcmbBatch"].Value != null)
                                {
                                    BatchSP SpBatch = new BatchSP();
                                    dgvProduct.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value = SpBatch.ProductBatchBarcodeViewByBatchId(Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvcmbBatch"].Value));
                                }
                            }



                            if (dgvProduct.Columns[e.ColumnIndex].Name == "dgvcmbUnit")
                            {
                                if (dgvProduct.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value.ToString() != string.Empty)
                                {
                                    if (dgvProduct.Rows[e.RowIndex].Cells["dgvtxtConversionRate"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["dgvtxtConversionRate"].Value.ToString() != string.Empty)
                                    {
                                        if (dgvProduct.Rows[e.RowIndex].Cells["dgvtxtRate"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["dgvtxtRate"].Value.ToString() != string.Empty)
                                        {
                                            decCurrentConversionRate = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtConversionRate"].Value.ToString());
                                            decCurrentRate = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtRate"].Value.ToString());
                                        }
                                    }
                                }

                            }
                            //---------------while changing Qty,corresponding change in amount----
                            if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvtxtQty" && isAmountcalc)
                            {
                                if (cmbMaterialReceiptNo.SelectedValue == null || cmbMaterialReceiptNo.SelectedValue.ToString() == string.Empty)
                                {
                                    NewAmountCalculation("dgvtxtQty", e.RowIndex);
                                }
                                else
                                {
                                    DataTable dtblReceiptDetails = spMaterialReceiptDetails.ShowMaterialReceiptDetailsViewbyMaterialReceiptDetailsIdWithPending(Convert.ToDecimal(cmbMaterialReceiptNo.SelectedValue.ToString()), decRejectionOutId);
                                    if (dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value != null)
                                    {
                                        DataTable dtblUnitByProduct = new DataTable();

                                        dtblUnitByProduct = spUnitConvertion.UnitConversionIdAndConRateViewallByProductId(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value.ToString());
                                        foreach (DataRow drdtbl in dtblReceiptDetails.Rows)
                                        {
                                            foreach (DataRow drUnitviewall in dtblUnitByProduct.Rows)
                                            {
                                                if (drdtbl.ItemArray[6].ToString() == drUnitviewall.ItemArray[2].ToString())//Checking UnitconversionId 
                                                {
                                                    decimal decQty = Convert.ToDecimal(drdtbl.ItemArray[3].ToString());
                                                    decimal decConRate = Convert.ToDecimal(drUnitviewall.ItemArray[3].ToString());
                                                    decimal decCurConRateInGrid = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtconversionRate"].Value.ToString());
                                                    decOldQty = (decQty / decConRate) * decCurConRateInGrid;
                                                    decOldQty = Math.Round(decOldQty, PublicVariables._inNoOfDecimalPlaces);
                                                    decCurrQty = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value.ToString());
                                                    if (decCurrQty > decOldQty)
                                                    {
                                                        dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value = decOldQty.ToString();
                                                    }
                                                }
                                            }
                                        }
                                        //
                                        //decCurrQty = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value.ToString());
                                        //decOldQty = Convert.ToDecimal(dtblReceiptDetails.Rows[e.RowIndex].ItemArray[3].ToString());
                                        if (decCurrQty > decOldQty)
                                        {
                                            dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value = decOldQty.ToString();
                                        }
                                    }
                                    else
                                    {
                                        dgvProduct.Focus();
                                    }
                                    NewAmountCalculation("dgvtxtQty", e.RowIndex);
                                    TotalAmount();
                                }
                            }
                            //if (dgvProduct.Columns[e.ColumnIndex].Name == "dgvcmbUnit")
                            //{
                            //    if (dgvProduct.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value.ToString() != string.Empty)
                            //    {
                            //        if (dgvProduct.Rows[e.RowIndex].Cells["dgvcmbUnit"].Value != null && dgvProduct.Rows[e.RowIndex].Cells["dgvcmbUnit"].Value.ToString() != string.Empty)
                            //        {
                            //            DataTable dtblUnitByProduct = new DataTable();
                            //            dtblUnitByProduct = spUnitConvertion.UnitConversionIdAndConRateViewallByProductId(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtproductId"].Value.ToString());
                            //            foreach (DataRow drUnitByProduct in dtblUnitByProduct.Rows)
                            //            {
                            //                if (dgvProduct.Rows[e.RowIndex].Cells["dgvcmbUnit"].Value.ToString() == drUnitByProduct.ItemArray[0].ToString())
                            //                {
                            //                    dgvProduct.Rows[e.RowIndex].Cells["dgvtxtunitConversionId"].Value = Convert.ToDecimal(drUnitByProduct.ItemArray[2].ToString());
                            //                    dgvProduct.Rows[e.RowIndex].Cells["dgvtxtconversionRate"].Value = Convert.ToDecimal(drUnitByProduct.ItemArray[3].ToString());
                            //                    if (isDoAfterGridFill)
                            //                    {
                            //                        decimal decNewConversionRate = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtconversionRate"].Value.ToString());
                            //                        decCurQty = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value.ToString());
                            //                        decimal decNewRate = (decCurrentRate * decCurrentConversionRate) / decNewConversionRate;
                            //                        //dgvProduct.Rows[e.RowIndex].Cells["dgvtxtRate"].Value = Math.Round(decNewRate);
                            //                        dgvProduct.Rows[e.RowIndex].Cells["dgvtxtRate"].Value = Math.Round(decNewRate, 2);
                            //                        decNewAmount = (decCurrentRate * decCurQty * decCurrentConversionRate) / decNewConversionRate;
                            //                        dgvProduct.Rows[e.RowIndex].Cells["dgvtxtAmount"].Value = Math.Round(decNewAmount, 2);
                            //                        TotalAmount();
                            //                        txtTotalAmount.Text = Math.Round(decNewRate, 2).ToString();

                            //                    }
                            //                }
                            //            }
                            //            if (decCurrQty > decOldQty)
                            //            {
                            //                dgvProduct.Rows[e.RowIndex].Cells["dgvtxtQty"].Value = decOldQty.ToString();
                            //            }
                            //        }
                            //    }
                            //}

                            else if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvtxtRate" && isAmountcalc)
                            {
                                NewAmountCalculation("dgvtxtRate", e.RowIndex);
                                TotalAmount();
                            }

                            else if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "dgvtxtAmount" && isAmountcalc)
                            {
                                TotalAmount();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO62:" + ex.Message;
            }
        }

        /// <summary>
        /// Form closing, checking the other forms status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRejectionOut_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmRejectionOutRegisterObj != null)
                {
                    frmRejectionOutRegisterObj.Enabled = true;
                    frmRejectionOutRegisterObj.GridFill();
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();

                    frmDayBookObj = null;
                }
                if (objVoucherProduct != null)
                {
                    objVoucherProduct.Enabled = true;
                    objVoucherProduct.FillGrid();

                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Enabled = true;
                    objVoucherSearch.GridFill();

                }
                if (frmRejectionOutReportObj != null)
                {
                    frmRejectionOutReportObj.Enabled = true;
                    frmRejectionOutReportObj.GridFill();

                }


            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO63:" + ex.Message;
            }
        }

        /// <summary>
        /// Commit the each and every changes in grid cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvProduct.IsCurrentCellDirty)
                {
                    dgvProduct.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO64:" + ex.Message;
            }
        }


        #endregion

        #region Navigation
        /// <summary>
        /// Form keydown for quickaccess
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRejectionOut_KeyDown(object sender, KeyEventArgs e)
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
                //----------------------------------------CTRL+S--------------------------------------------//
                if (e.Control && e.KeyCode == Keys.S)//Save or Edit
                {
                    btnSave.PerformClick();
                }
                //----------------------------------------CTRL+D--------------------------------------------//
                if (e.Control && e.KeyCode == Keys.D)//Delete
                {
                    btnDelete.PerformClick();
                }
                //-------------------------------------------ALT+C-----------------------------------------//
                if (e.Alt && e.KeyCode == Keys.C)
                {
                    SendKeys.Send("{F10}");
                    btnLedgerAdd_Click(sender, e);
                }
                //-------------------------------------------CTRL+F-----------------------------------------//
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
                {
                    if (cmbCashOrParty.Focus())
                    {
                        if (cmbCashOrParty.Focused)
                        {
                            cmbCashOrParty.DropDownStyle = ComboBoxStyle.DropDown;
                        }
                        else
                        {
                            cmbCashOrParty.DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                        if (cmbCashOrParty.SelectedIndex != -1)
                        {
                            frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                            frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                            frmLedgerPopupObj.CallFromRejectionOut(this, Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), "CashOrSundryCreditors");
                        }
                        else
                        {
                            Messages.InformationMessage("select any ledger");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO65:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRejectionOutNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO66:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrParty.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtDate.SelectionStart == 0 && txtRejectionOutNo.ReadOnly == false)
                    {
                        txtRejectionOutNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO67:" + ex.Message;
            }

        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbVoucherType.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbCashOrParty.Text == string.Empty || cmbCashOrParty.SelectionStart == 0)
                    {
                        txtDate.Focus();
                        txtDate.SelectionStart = 0;
                        txtDate.SelectionLength = 0;
                    }
                }

                if (e.Alt && e.KeyCode == Keys.C)
                {
                    SendKeys.Send("{F10}");
                    btnLedgerAdd_Click(sender, e);
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control)
                {
                    if (cmbCashOrParty.Focus())
                    {
                        if (cmbCashOrParty.Focused)
                        {
                            cmbCashOrParty.DropDownStyle = ComboBoxStyle.DropDown;
                        }
                        else
                        {
                            cmbCashOrParty.DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                        if (cmbCashOrParty.SelectedIndex != -1)
                        {

                            frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                            frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                            frmLedgerPopupObj.CallFromRejectionOut(this, Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), "CashOrSundryCreditors");
                        }
                        else
                        {
                            Messages.InformationMessage("select any ledger");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO68:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbVoucherType.SelectedIndex == -1)
                    {
                        if (cmbCurrency.Enabled == true)
                        {
                            cmbCurrency.Focus();
                        }
                        else
                        {
                            txtTransportationCompany.Focus();

                        }
                    }
                    else
                    {
                        cmbMaterialReceiptNo.Focus();
                    }

                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbVoucherType.Text == string.Empty || cmbVoucherType.SelectionLength == 0)
                    {
                        cmbCashOrParty.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO69:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMaterialReceiptNo_KeyDown(object sender, KeyEventArgs e)
        {
             try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbCurrency.Enabled)
                    {
                        cmbCurrency.Focus();
                    }
                    else
                    {
                        if (dgvProduct.RowCount > 0)
                        {
                            dgvProduct.Focus();
                            dgvProduct.CurrentCell = dgvProduct.Rows[0].Cells["dgvtxtSlNo"];
                            dgvProduct.Rows[0].Cells["dgvtxtSlNo"].Selected = true;
                        }
                        else
                        {
                            txtTransportationCompany.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }
            }
             catch (Exception ex)
             {
                 formMDI.infoError.ErrorString = "RO70:" + ex.Message;
             }
            
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProduct.Rows.Count > 0)
                    {
                        dgvProduct.Focus();
                        dgvProduct.CurrentCell = dgvProduct.Rows[0].Cells["dgvtxtSlNo"];
                    }
                    else
                    {
                        txtTransportationCompany.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbMaterialReceiptNo.Visible == false)
                    {
                        cmbVoucherType.Focus();
                    }
                    else
                    {
                        cmbMaterialReceiptNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO71:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTransportationCompany_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.SelectionStart = txtNarration.Text.Length;
                    txtNarration.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvProduct.Rows.Count > 0)
                    {
                        dgvProduct.Focus();
                    }
                    else if (txtTransportationCompany.Text == string.Empty || txtTransportationCompany.SelectionStart == 0)
                    {
                        if (cmbCurrency.Enabled)
                        {
                            cmbCurrency.Focus();
                        }
                        else
                        {
                            cmbMaterialReceiptNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO72:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.SelectionStart == 0)
                    {
                        txtTransportationCompany.SelectionStart = 0;
                        txtTransportationCompany.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO73:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    inKeyPrsCount++;
                    if (inKeyPrsCount == 2)
                    {
                        inKeyPrsCount = 0;
                        txtLrNo.Focus();
                    }
                }
                else
                {
                    inKeyPrsCount = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO74:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLrNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxPrintAfterSave.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtLrNo.Text == string.Empty || txtLrNo.SelectionStart == 0)
                    {
                        txtNarration.SelectionStart = 0;
                        txtNarration.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO75:" + ex.Message;
            }

        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPrintAfterSave_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtLrNo.SelectionStart = 0;
                    txtLrNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO76:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int inDgvRejectionOutRowCount = dgvProduct.Rows.Count;
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProduct.CurrentCell == dgvProduct.Rows[inDgvRejectionOutRowCount - 1].Cells["dgvtxtAmount"])
                    {
                        txtTransportationCompany.Focus();
                        dgvProduct.ClearSelection();
                    }

                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvProduct.CurrentCell == dgvProduct.Rows[0].Cells["dgvtxtSlNo"])
                    {

                        cmbMaterialReceiptNo.Focus();
                        dgvProduct.ClearSelection();
                    }

                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvProduct.Columns[dgvProduct.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        frmProductSearchPopupObj.MdiParent = formMDI.MDIObj;
                        if (dgvProduct.CurrentRow.Cells["dgvtxtProductCode"].Value != null || dgvProduct.CurrentRow.Cells["dgvtxtProductName"].Value != null)
                        {
                            frmProductSearchPopupObj.CallFromRejectionOut(this, dgvProduct.CurrentRow.Index, dgvProduct.CurrentRow.Cells["dgvtxtProductCode"].Value.ToString());
                        }
                        else
                        {
                            frmProductSearchPopupObj.CallFromRejectionOut(this, dgvProduct.CurrentRow.Index, string.Empty);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO77:" + ex.Message;
            }
        }
        /// <summary>
        /// For enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cbxPrintAfterSave.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "RO78:" + ex.Message;
            }
        }

        #endregion

      
    }
}
