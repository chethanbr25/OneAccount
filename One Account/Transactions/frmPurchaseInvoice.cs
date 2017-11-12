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
    public partial class frmPurchaseInvoice : Form
    {
        #region Variables
        /// <summary>
        /// Public variable declaration Part
        /// </summary>
        string strCashOrParty = string.Empty;
        string strPurchaseAccount = string.Empty;
        string strVoucherNo = string.Empty;
        string strTableName = "PurchaseMaster";
        string strPrefix = string.Empty;
        string strSuffix = string.Empty;
        string strProductCode = string.Empty;
        decimal decPurchaseInvoiceVoucherTypeId = 0;
        decimal decPurchaseInvoiceSuffixPrefixId = 0;
        decimal decPurchaseMasterId = 0;
        int inNarrationCount = 0;
        bool isAutomatic = false;
        bool isValueChanged = false;
        bool isLoad = true;
        bool isEditFill = false;
        ArrayList arrlstRemove = new ArrayList();
        ArrayList arrlstRemoveAdditionalCost = new ArrayList();
        TransactionsGeneralFill TransactionGeneralFillObj = new TransactionsGeneralFill();
        SettingsSP spSettings = new SettingsSP();
        frmLedgerPopup frmLedgerPopupObj = null;
        frmProductSearchPopup frmProductSearchPopupObj = null;
        frmPurchaseInvoiceRegister frmPurchaseInvoiceRegisterObj = null;
        frmPurchaseReport frmPurchaseReportObj = null;
        AutoCompleteStringCollection ProductNames = new AutoCompleteStringCollection();
        AutoCompleteStringCollection ProductCodes = new AutoCompleteStringCollection();
        DataGridViewTextBoxEditingControl TextBoxControl;
        frmDayBook frmDayBookObj = null;
        frmLedgerDetails frmLedgerDetailsObj;
        frmVoucherSearch objVoucherSearch = null;
        frmVoucherWiseProductSearch objVoucherProduct = null;
        frmAgeingReport frmAgeingObj = null;
        frmVatReturnReport vatReturnReportobj = null;
        decimal decMeterialReceiptQty = 0;
        DataTable dtblMeterialReceiptQty = new DataTable();

        DataTable dtblMaster = new DataTable();
        DataTable dtblDetails = new DataTable();
        PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
        PurchaseOrderDetailsSP spPurchaseOrderDetails = new PurchaseOrderDetailsSP();
        BatchSP spBatch = new BatchSP();
        #endregion
        #region Functions
        /// <summary>
        /// Create an instance for frmPurchaseInvoice Class
        /// </summary>
        public frmPurchaseInvoice()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to fill the cashorparty combobox
        /// </summary>
        public void CashOrPartyComboFill()
        {
            try
            {
                TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the purchaseaccount combobox
        /// </summary>
        public void PurchaseAccountComboFill()
        {
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            DataTable dtbl = new DataTable();
            try
            {
                dtbl = spPurchaseMaster.PurchaseInvoicePurchaseAccountFill();
                cmbPurchaseAccount.DataSource = dtbl;
                cmbPurchaseAccount.DisplayMember = "ledgerName";
                cmbPurchaseAccount.ValueMember = "ledgerId";
                if (dtbl.Rows.Count > 0)
                {
                    cmbPurchaseAccount.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI2:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Currency combobox fill
        /// </summary>
        public void CurrencyComboFill()
        {
            DataTable dtbl = new DataTable();
            try
            {
                dtbl = TransactionGeneralFillObj.CurrencyComboByDate(Convert.ToDateTime(txtVoucherDate.Text));
                cmbCurrency.DataSource = dtbl;
                cmbCurrency.DisplayMember = "currencyName";
                cmbCurrency.ValueMember = "exchangeRateId";
                cmbCurrency.SelectedValue = 1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI3:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill vouchertype combobox 
        /// </summary>
        /// <param name="strVoucherType"></param>
        public void VoucherTypeComboFill(string strVoucherType)
        {
            VoucherTypeSP spVoucherType = new VoucherTypeSP();
            DataTable dtbl = new DataTable();
            try
            {
                dtbl = spVoucherType.VoucherTypeSelectionComboFill(strVoucherType);
                cmbVoucherType.DataSource = dtbl;
                cmbVoucherType.DisplayMember = "voucherTypeName";
                cmbVoucherType.ValueMember = "voucherTypeId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI4:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for account ledger creation
        /// </summary>
        public void AccountLedgerCreation()
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
                if (cmbPurchaseAccount.SelectedValue != null)
                {
                    strPurchaseAccount = cmbPurchaseAccount.SelectedValue.ToString();
                }
                else
                {
                    strPurchaseAccount = string.Empty;
                }
                frmAccountLedger frmAccountLedgerObj = new frmAccountLedger();
                frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                frmAccountLedger open = Application.OpenForms["frmAccountLedger"] as frmAccountLedger;
                if (open == null)
                {
                    frmAccountLedgerObj.WindowState = FormWindowState.Normal;
                    frmAccountLedgerObj.MdiParent = formMDI.MDIObj;
                    frmAccountLedgerObj.CallFromPurchaseInvoice(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.BringToFront();
                    open.CallFromPurchaseInvoice(this);
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI5:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Account ledger combobox while return from Account ledger creation when creating new ledger 
        /// </summary>
        /// <param name="decLedgerId"></param>
        public void ReturnFromAccountLedgerForm(decimal decLedgerId)
        {
            AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
            AccountLedgerSP spAccountLedger = new AccountLedgerSP();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            DataTable dtbl = new DataTable();
            try
            {
                this.Enabled = true;
                this.Activate();
                CashOrPartyComboFill();
                PurchaseAccountComboFill();
                cmbPurchaseAccount.SelectedValue = decLedgerId;
                cmbCashOrParty.SelectedValue = decLedgerId;
                if (cmbCashOrParty.Text == string.Empty)
                {
                    cmbCashOrParty.SelectedValue = strCashOrParty;
                }
                if (cmbPurchaseAccount.Text == string.Empty)
                {
                    cmbPurchaseAccount.SelectedValue = strPurchaseAccount;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI6:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from VoucherType Selection form
        /// </summary>
        /// <param name="decVoucherTypeId"></param>
        /// <param name="strVoucherTypeName"></param>
        public void CallFromVoucherTypeSelection(decimal decVoucherTypeId, string strVoucherTypeName)
        {
            try
            {
                decPurchaseInvoiceVoucherTypeId = decVoucherTypeId;
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPurchaseInvoiceVoucherTypeId);
                SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPurchaseInvoiceVoucherTypeId, dtpVoucherDate.Value);
                decPurchaseInvoiceSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                this.Text = strVoucherTypeName;
                base.Show();
                if (isAutomatic)
                {
                    txtVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI7:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for voucher number generation
        /// </summary>
        public void VoucherNumberGeneration()
        {
            try
            {
                PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
                if (strVoucherNo == string.Empty)
                {
                    strVoucherNo = "0";
                }
                strVoucherNo = TransactionGeneralFillObj.VoucherNumberAutomaicGeneration(decPurchaseInvoiceVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strTableName);
                if (Convert.ToDecimal(strVoucherNo) != (spPurchaseMaster.PurchaseMasterVoucherMax(decPurchaseInvoiceVoucherTypeId)))
                {
                    strVoucherNo = spPurchaseMaster.PurchaseMasterVoucherMax(decPurchaseInvoiceVoucherTypeId).ToString();
                    strVoucherNo = TransactionGeneralFillObj.VoucherNumberAutomaicGeneration(decPurchaseInvoiceVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strTableName);
                    if (spPurchaseMaster.PurchaseMasterVoucherMax(decPurchaseInvoiceVoucherTypeId) == 0)
                    {
                        strVoucherNo = "0";
                        strVoucherNo = TransactionGeneralFillObj.VoucherNumberAutomaicGeneration(decPurchaseInvoiceVoucherTypeId, Convert.ToDecimal(strVoucherNo), dtpVoucherDate.Value, strTableName);
                    }
                }
                if (isAutomatic)
                {
                    SuffixPrefixSP spSuffisprefix = new SuffixPrefixSP();
                    SuffixPrefixInfo infoSuffixPrefix = new SuffixPrefixInfo();
                    infoSuffixPrefix = spSuffisprefix.GetSuffixPrefixDetails(decPurchaseInvoiceVoucherTypeId, dtpVoucherDate.Value);
                    strPrefix = infoSuffixPrefix.Prefix;
                    strSuffix = infoSuffixPrefix.Suffix;
                    decPurchaseInvoiceSuffixPrefixId = infoSuffixPrefix.SuffixprefixId;
                    txtVoucherNo.Text = strPrefix + strVoucherNo + strSuffix;
                    txtVoucherNo.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI8:" + ex.Message;
            }
        }
        /// <summary>
        /// Function for orderNo combobox
        /// </summary>
        public void OrderComboFill()
        {
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            DataTable dtbl = new DataTable();
            try
            {
                if (cmbVoucherType.SelectedValue != null)
                {
                    if (cmbVoucherType.SelectedValue.ToString() != "System.Data.DataRowView" && cmbVoucherType.Text != "System.Data.DataRowView")
                    {
                        if (cmbCashOrParty.SelectedValue != null)
                        {
                            if (cmbCashOrParty.SelectedValue.ToString() != string.Empty && cmbCashOrParty.Text != string.Empty)
                            {
                                if (cmbPurchaseMode.Text == "Against Purchase Order")
                                {
                                    dtbl = spPurchaseMaster.GetOrderNoCorrespondingtoLedgerByNotInCurrPI(Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), decPurchaseMasterId,
                                        Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()));
                                    DataRow drow = dtbl.NewRow();
                                    drow["purchaseOrderMasterId"] = 0;
                                    drow["invoiceNo"] = string.Empty;
                                    dtbl.Rows.InsertAt(drow, 0);
                                    cmbOrderNo.DataSource = dtbl;
                                    cmbOrderNo.ValueMember = "purchaseOrderMasterId";
                                    cmbOrderNo.DisplayMember = "invoiceNo";
                                }
                                else if (cmbPurchaseMode.Text == "Against Material Receipt")
                                {
                                    dtbl = spPurchaseMaster.GetMaterialReceiptNoCorrespondingtoLedgerByNotInCurrPI(Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), decPurchaseMasterId,
                                        Convert.ToDecimal(cmbVoucherType.SelectedValue.ToString()));
                                    DataRow drow = dtbl.NewRow();
                                    drow["materialReceiptMasterId"] = 0;
                                    drow["invoiceNo"] = string.Empty;
                                    dtbl.Rows.InsertAt(drow, 0);
                                    cmbOrderNo.DataSource = dtbl;
                                    cmbOrderNo.ValueMember = "materialReceiptMasterId";
                                    cmbOrderNo.DisplayMember = "invoiceNo";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI9:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill unit combobox in grid
        /// </summary>
        public void UnitAllComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                UnitSP spUnit = new UnitSP();
                dtbl = spUnit.UnitViewAll();
                dgvcmbUnit.DataSource = dtbl;
                dgvcmbUnit.DisplayMember = "unitName";
                dgvcmbUnit.ValueMember = "unitId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI10:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill unit combobox in grid
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
                DataGridViewComboBoxCell dgvcmbUnitCell = (DataGridViewComboBoxCell)dgvProductDetails.Rows[inRow].Cells[inColumn];
                dgvcmbUnitCell.DataSource = dtbl;
                dgvcmbUnitCell.DisplayMember = "unitName";
                dgvcmbUnitCell.ValueMember = "unitId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI11:" + ex.Message;
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
                dgvcmbGodown.DataSource = dtbl;
                dgvcmbGodown.ValueMember = "godownId";
                dgvcmbGodown.DisplayMember = "godownName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI12:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill rack combobox
        /// </summary>
        /// <param name="decGodownId"></param>
        /// <param name="inRow"></param>
        /// <param name="inColumn"></param>
        public void RackComboFill(decimal decGodownId, int inRow, int inColumn)
        {
            try
            {
                DataTable dtbl = new DataTable();
                RackSP spRack = new RackSP();
                dtbl = spRack.RackNamesCorrespondingToGodownId(decGodownId);
                DataGridViewComboBoxCell dgvcmbRackCell = (DataGridViewComboBoxCell)dgvProductDetails.Rows[inRow].Cells[inColumn];
                dgvcmbRackCell.DataSource = dtbl;
                dgvcmbRackCell.ValueMember = "rackId";
                dgvcmbRackCell.DisplayMember = "rackName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI13:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill rack combobox
        /// </summary>
        public void RackAllComboFill(int inRow)
        {
            try
            {
                DataTable dtbl = new DataTable();
                RackSP spRack = new RackSP();
                dtbl = spRack.RackViewAll();
                DataGridViewComboBoxCell dgvcmbRackCell = (DataGridViewComboBoxCell)dgvProductDetails.Rows[inRow].Cells["dgvcmbRack"];
                dgvcmbRackCell.DataSource = dtbl;
                dgvcmbRackCell.ValueMember = "rackId";
                dgvcmbRackCell.DisplayMember = "rackName";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI14:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill batch combobox
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
                DataGridViewComboBoxCell dgvcmbBatchCell = (DataGridViewComboBoxCell)dgvProductDetails.Rows[inRow].Cells[inColumn];
                dgvcmbBatchCell.DataSource = dtbl;
                dgvcmbBatchCell.ValueMember = "batchId";
                dgvcmbBatchCell.DisplayMember = "batchNo";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI15:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Batch combobox
        /// </summary>
        public void BatchAllComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                BatchSP spBatch = new BatchSP();
                dtbl = spBatch.BatchViewAll();
                dgvcmbBatch.DataSource = dtbl;
                dgvcmbBatch.ValueMember = "batchId";
                dgvcmbBatch.DisplayMember = "batchNo";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI16:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill tax combobox
        /// </summary>
        public void TaxCombofill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TaxSP spTax = new TaxSP();
                dtbl = spTax.TaxViewAllByVoucherTypeIdApplicaleForProduct(decPurchaseInvoiceVoucherTypeId);
                DataRow drow = dtbl.NewRow();
                drow["taxName"] = "      ";
                drow["taxId"] = 0;
                dtbl.Rows.InsertAt(drow, 0);
                dgvcmbTax.DataSource = dtbl;
                dgvcmbTax.DisplayMember = "taxName";
                dgvcmbTax.ValueMember = "taxId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI17:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill grid combos
        /// </summary>
        public void GridComboFill()
        {
            try
            {
                //UnitAllComboFill();
                //GodownComboFill();
                //RackAllComboFill();
                //BatchAllComboFill();
                TaxCombofill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI18:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill purchase order details 
        /// </summary>
        public void PurchaseOrderDetailsFill()
        {
            try
            {
                //DataTable dtblMaster = new DataTable();
                //DataTable dtblDetails = new DataTable();
                //PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
                //PurchaseOrderDetailsSP spPurchaseOrderDetails = new PurchaseOrderDetailsSP();
                //BatchSP spBatch = new BatchSP();
                decimal decPurchaseOrderMasterId = 0;
                decimal decBatchId = 0;
                if (!isEditFill)
                {
                    if (cmbCashOrParty.SelectedValue != null)
                    {
                        if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashOrParty.Text != "System.Data.DataRowView")
                        {
                            GridComboFill();
                            decPurchaseOrderMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                            dtblMaster = spPurchaseOrderMaster.PurchaseOrderMasterViewByOrderMasterId(decPurchaseOrderMasterId);
                            if (dtblMaster.Rows.Count > 0)
                            {
                                cmbCurrency.SelectedValue = Convert.ToDecimal(dtblMaster.Rows[0].ItemArray[10].ToString());
                            }
                            dtblDetails = spPurchaseOrderDetails.PurchaseOrderDetailsViewByOrderMasterIdWithRemainingByNotInCurrPI
                                   (decPurchaseOrderMasterId, decPurchaseMasterId, decPurchaseInvoiceVoucherTypeId);
                            if (dtblDetails.Rows.Count > 0)
                            {
                                //dgvProductDetails.DataSource = dtblDetails;
                                //----------------
                                if (dgvProductDetails.DataSource == null)
                                {
                                    dgvProductDetails.Rows.Clear();
                                }
                                else
                                {
                                    ((DataTable)dgvProductDetails.DataSource).Rows.Clear();

                                }
                                int i = 0;
                                foreach (DataRow dr in dtblDetails.Rows)
                                {
                                    dgvProductDetails.Rows.Add();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtPurchaseDetailsId"].Value = dr["purchaseDetailsId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtPurchaseOrderDetailsId"].Value = dr["purchaseOrderDetailsId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtMaterialReceiptDetailsId"].Value = dr["materialReceiptDetailsId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtProductId"].Value = dr["productId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtBarcode"].Value = dr["barcode"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtProductCode"].Value = dr["productCode"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtProductName"].Value = dr["productName"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtQuantity"].Value = dr["qty"].ToString();
                                    UnitComboFill(Convert.ToDecimal(dr["productId"].ToString()), i, dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].ColumnIndex);
                                    dgvProductDetails.Rows[i].Cells["dgvtxtUnitConversionId"].Value = dr["unitConversionId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(dr["unitId"].ToString());
                                    GodownComboFill();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(dr["godownId"].ToString());
                                    RackComboFill(Convert.ToDecimal(dr["godownId"].ToString()), i, dgvProductDetails.Columns["dgvcmbRack"].Index);
                                    dgvProductDetails.Rows[i].Cells["dgvcmbRack"].Value = Convert.ToDecimal(dr["rackId"].ToString());
                                    BatchComboFill(Convert.ToDecimal(dr["productId"].ToString()), i, dgvProductDetails.Rows[i].Cells["dgvcmbBatch"].ColumnIndex);
                                    decBatchId = spBatch.BatchIdViewByProductId(Convert.ToDecimal(dr["productId"].ToString()));
                                    dgvProductDetails.Rows[i].Cells["dgvcmbBatch"].Value = decBatchId;
                                    dgvProductDetails.Rows[i].Cells["dgvtxtRate"].Value = dr["rate"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtGrossValue"].Value = dr["grossValue"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtDiscountPercent"].Value = dr["discountPercent"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtDiscount"].Value = dr["discount"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtNetValue"].Value = dr["netvalue"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbTax"].Value = Convert.ToDecimal(dr["taxId"].ToString());
                                    dgvProductDetails.Rows[i].Cells["dgvtxtTaxAmount"].Value = dr["taxAmount"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtAmount"].Value = dr["Amount"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].ReadOnly = true;
                                    i++;
                                }

                                //-------------------
                            }
                            else
                            {
                                if (dgvProductDetails.DataSource == null)
                                {
                                    dgvProductDetails.Rows.Clear();
                                }
                                else
                                {
                                    ((DataTable)dgvProductDetails.DataSource).Rows.Clear();
                                }
                            }
                            SerialNo();
                            GridviewReadOnlySettings("Against Purchase Order");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI19:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill material receipt details 
        /// </summary>
        public void MaterialReceiptDetailsFill()
        {
            DataTable dtblMaster = new DataTable();
            DataTable dtblDetails = new DataTable();
            MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
            MaterialReceiptDetailsSP spMaterialReceiptDetails = new MaterialReceiptDetailsSP();
            decimal decMaterialReceiptMasterId = 0;
            decimal decBatchId = 0;
            try
            {
                if (!isEditFill)
                {
                    if (cmbCashOrParty.SelectedValue != null)
                    {
                        if (cmbCashOrParty.SelectedValue.ToString() != "System.Data.DataRowView" && cmbCashOrParty.Text != "System.Data.DataRowView")
                        {
                            GridComboFill();
                            decMaterialReceiptMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                            dtblMaster = spMaterialReceiptMaster.MaterialReceiptMasterViewByReceiptMasterId(decMaterialReceiptMasterId);
                            if (dtblMaster.Rows.Count > 0)
                            {
                                cmbCurrency.SelectedValue = dtblMaster.Rows[0].ItemArray[7];
                            }
                            dtblDetails = spMaterialReceiptDetails.MaterialReceiptDetailsViewByMaterialReceiptMasterIdWithRemainingByNotInCurrPI
                                    (decMaterialReceiptMasterId, decPurchaseMasterId, decPurchaseInvoiceVoucherTypeId);
                            if (dtblDetails.Rows.Count > 0)
                            {
                                if (dgvProductDetails.DataSource == null)
                                {
                                    dgvProductDetails.Rows.Clear();
                                }
                                else
                                {
                                    ((DataTable)dgvProductDetails.DataSource).Rows.Clear();

                                }
                                int i = 0;
                                foreach (DataRow dr in dtblDetails.Rows)
                                {
                                    dgvProductDetails.Rows.Add();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtPurchaseDetailsId"].Value = dr["purchaseDetailsId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtPurchaseOrderDetailsId"].Value = dr["purchaseOrderDetailsId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtMaterialReceiptDetailsId"].Value = dr["materialReceiptDetailsId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtProductId"].Value = dr["productId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtBarcode"].Value = dr["barcode"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtProductCode"].Value = dr["productCode"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtProductName"].Value = dr["productName"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtQuantity"].Value = dr["qty"].ToString();
                                    UnitComboFill(Convert.ToDecimal(dr["productId"].ToString()), i, dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].ColumnIndex);
                                    dgvProductDetails.Rows[i].Cells["dgvtxtUnitConversionId"].Value = dr["unitConversionId"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(dr["unitId"].ToString());
                                    GodownComboFill();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(dr["godownId"].ToString());
                                    RackComboFill(Convert.ToDecimal(dr["godownId"].ToString()), i, dgvProductDetails.Columns["dgvcmbRack"].Index);
                                    dgvProductDetails.Rows[i].Cells["dgvcmbRack"].Value = Convert.ToDecimal(dr["rackId"].ToString());
                                    BatchComboFill(Convert.ToDecimal(dr["productId"].ToString()), i, dgvProductDetails.Rows[i].Cells["dgvcmbBatch"].ColumnIndex);
                                    dgvProductDetails.Rows[i].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(dr["batchId"].ToString());
                                    dgvProductDetails.Rows[i].Cells["dgvtxtRate"].Value = dr["rate"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtGrossValue"].Value = dr["grossValue"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtDiscountPercent"].Value = dr["discountPercent"].ToString();

                                    dgvProductDetails.Rows[i].Cells["dgvtxtDiscount"].Value = dr["discount"].ToString();

                                    dgvProductDetails.Rows[i].Cells["dgvtxtNetValue"].Value = dr["netvalue"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvcmbTax"].Value = Convert.ToDecimal(dr["taxId"].ToString());
                                    dgvProductDetails.Rows[i].Cells["dgvtxtTaxAmount"].Value = dr["taxAmount"].ToString();
                                    dgvProductDetails.Rows[i].Cells["dgvtxtAmount"].Value = dr["Amount"].ToString();

                                    dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].ReadOnly = true;
                                    i++;

                                }
                                dgvProductDetails.AllowUserToAddRows = false;
                            }
                            else
                            {
                                if (dgvProductDetails.DataSource == null)
                                {
                                    dgvProductDetails.Rows.Clear();
                                }
                                else
                                {
                                    ((DataTable)dgvProductDetails.DataSource).Rows.Clear();
                                }
                            }
                            SerialNo();
                            GridviewReadOnlySettings("Against Material Receipt");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI20:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate serialno
        /// </summary>
        public void SerialNo()
        {
            try
            {
                foreach (DataGridViewRow row in dgvProductDetails.Rows)
                {
                    row.Cells["dgvtxtSlNo"].Value = row.Index + 1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI21:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate additional cost serialNo
        /// </summary>
        public void AdditionalCostSerialNo()
        {
            try
            {
                foreach (DataGridViewRow row in dgvAdditionalCost.Rows)
                {
                    row.Cells["dgvtxtSlNoAdditionalCost"].Value = row.Index + 1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI22:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate tax serialNo
        /// </summary>
        public void TaxSerialNo()
        {
            try
            {
                foreach (DataGridViewRow row in dgvTax.Rows)
                {
                    row.Cells["dgvtxtSlNoTax"].Value = row.Index + 1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI23:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Tax grid
        /// </summary>
        public void TaxGridFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TaxSP spTax = new TaxSP();
                PurchaseBillTaxSP spPurchaseBillTax = new PurchaseBillTaxSP();
                if (decPurchaseMasterId == 0)
                {
                    dtbl = spTax.TaxViewAllByVoucherTypeIdForPurchaseInvoice(decPurchaseInvoiceVoucherTypeId);
                }
                else
                {
                    dtbl = spPurchaseBillTax.PurchaseBillTaxViewAllByPurchaseMasterId(decPurchaseMasterId);
                }
                dgvTax.DataSource = dtbl;
                TaxSerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI24:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill additional cost combo box
        /// </summary>
        /// <param name="inRowIndex"></param>
        public void AdditionalCostComboFill(int inRowIndex)
        {
            DataTable dtbl = new DataTable();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            try
            {
                dtbl = spPurchaseMaster.AccountLedgerViewForAdditionalCost();
                DataRow drow = dtbl.NewRow();
                drow["ledgerName"] = string.Empty;
                drow["ledgerId"] = 0;
                dtbl.Rows.InsertAt(drow, 0);
                if (dgvAdditionalCost.RowCount > 1)
                {
                    foreach (DataGridViewRow dgvrow in dgvAdditionalCost.Rows)
                    {
                        foreach (DataRow drow1 in dtbl.Rows)
                        {
                            if (dgvrow.Index != inRowIndex)
                            {
                                if (dgvrow.Cells["dgvcmbLedger"].Value != null)
                                {
                                    if (drow1["ledgerId"].ToString() == dgvrow.Cells["dgvcmbLedger"].Value.ToString())
                                    {
                                        dtbl.Rows.RemoveAt(dtbl.Rows.IndexOf(drow1));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                DataGridViewComboBoxCell dgvcmbLedgerCell = (DataGridViewComboBoxCell)dgvAdditionalCost.Rows[inRowIndex].Cells[dgvAdditionalCost.Columns["dgvcmbLedger"].Index];
                dgvcmbLedgerCell.DataSource = dtbl;
                dgvcmbLedgerCell.DisplayMember = "ledgerName";
                dgvcmbLedgerCell.ValueMember = "ledgerId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI25:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill additional cost grid
        /// </summary>
        public void AdditionalCostGridFill()
        {
            DataTable dtbl = new DataTable();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            AdditionalCostSP spAdditionalCost = new AdditionalCostSP();
            try
            {
                dtbl = spPurchaseMaster.AccountLedgerViewForAdditionalCost();
                DataRow dr = dtbl.NewRow();
                dr["ledgerName"] = string.Empty;
                dr["ledgerId"] = 0;
                dtbl.Rows.InsertAt(dr, 0);
                dgvcmbLedger.DataSource = dtbl;
                dgvcmbLedger.DisplayMember = "ledgerName";
                dgvcmbLedger.ValueMember = "ledgerId";
                if (decPurchaseMasterId == 0)
                {
                    AdditionalCostComboFill(0);
                }
                else
                {
                    dtbl = spAdditionalCost.AdditionalCostViewAllByVoucherTypeIdAndVoucherNo(decPurchaseInvoiceVoucherTypeId, strVoucherNo);
                    dgvAdditionalCost.DataSource = dtbl;
                }
                AdditionalCostSerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI26:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate total additional cost amount
        /// </summary>
        public void TotalAdditionalCostAmount()
        {
            decimal decTotalAdditionalCost = 0;
            decimal decAdditionalCost = 0;
            try
            {
                foreach (DataGridViewRow dgrow in dgvAdditionalCost.Rows)
                {
                    if (dgrow.Cells["dgvcmbLedger"].Value != null)
                    {
                        if (dgrow.Cells["dgvcmbLedger"].Value.ToString() != string.Empty &&
                            dgrow.Cells["dgvcmbLedger"].Value.ToString() != "0")
                        {
                            if (dgrow.Cells["dgvtxtAdditionalCostAmount"].Value != null)
                            {
                                if (dgrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString() != string.Empty &&
                                    dgrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString() != ".")
                                {
                                    decAdditionalCost = Convert.ToDecimal(dgrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString());
                                    decTotalAdditionalCost = decTotalAdditionalCost + decAdditionalCost;
                                    decAdditionalCost = Math.Round(decAdditionalCost, PublicVariables._inNoOfDecimalPlaces);
                                    dgrow.Cells["dgvtxtAdditionalCostAmount"].Value = decAdditionalCost;
                                }
                            }
                        }
                    }
                }
                if (decTotalAdditionalCost == 0)
                {
                    lblAdditionalCostAmount.Text = Math.Round(000.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
                else
                {
                    lblAdditionalCostAmount.Text = decTotalAdditionalCost.ToString();
                }
                AdditionalCostSerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI27:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate total tax amount
        /// </summary>
        public void TotalTaxAmount()
        {
            decimal decTotalTax = 0;
            decimal decTax = 0;
            try
            {
                foreach (DataGridViewRow dgrow in dgvTax.Rows)
                {
                    if (dgrow.Cells["dgvtxtTaxId"].Value != null)
                    {
                        if (dgrow.Cells["dgvtxtTaxId"].Value.ToString() != string.Empty &&
                            dgrow.Cells["dgvtxtTaxId"].Value.ToString() != "0")
                        {
                            if (dgrow.Cells["dgvtxtTotalTax"].Value != null)
                            {
                                if (dgrow.Cells["dgvtxtTotalTax"].Value.ToString() != string.Empty)
                                {
                                    decTax = Convert.ToDecimal(dgrow.Cells["dgvtxtTotalTax"].Value.ToString());
                                    decTotalTax = decTotalTax + decTax;
                                }
                            }
                        }
                    }
                }
                if (decTotalTax == 0)
                {
                    lblTaxAmount.Text = Math.Round(000.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
                else
                {
                    lblTaxAmount.Text = decTotalTax.ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI28:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to find tax amount for a taxtype
        /// </summary>
        public void TaxAmountForTaxType()
        {
            decimal decTaxId = 0;
            decimal decAmount = 0;
            decimal decDefaultAmount = 0;
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            try
            {
                foreach (DataGridViewRow dgvTaxRow in dgvTax.Rows)
                {
                    if (dgvTaxRow.Cells["dgvtxtTaxId"].Value != null)
                    {
                        if (dgvTaxRow.Cells["dgvtxtTaxId"].Value.ToString() != string.Empty &&
                            dgvTaxRow.Cells["dgvtxtTaxId"].Value.ToString() != "0")
                        {
                            decTaxId = Convert.ToDecimal(dgvTaxRow.Cells["dgvtxtTaxId"].Value.ToString());
                            foreach (DataGridViewRow dgvProductRow in dgvProductDetails.Rows)
                            {
                                if (dgvProductRow.Cells["dgvtxtProductId"].Value != null)
                                {
                                    if (dgvProductRow.Cells["dgvtxtProductId"].Value.ToString() != string.Empty &&
                                        dgvProductRow.Cells["dgvtxtProductId"].Value.ToString() != "0")
                                    {
                                        if (dgvProductRow.Cells["dgvcmbTax"].Value != null)
                                        {
                                            if (dgvProductRow.Cells["dgvcmbTax"].Value.ToString() != string.Empty &&
                                                dgvProductRow.Cells["dgvcmbTax"].Value.ToString() != "0")
                                            {
                                                if (dgvProductRow.Cells["dgvtxtTaxAmount"].Value != null)
                                                {
                                                    if (dgvProductRow.Cells["dgvtxtTaxAmount"].Value.ToString() != string.Empty &&
                                                        dgvProductRow.Cells["dgvtxtTaxAmount"].Value.ToString() != "0")
                                                    {
                                                        if (Convert.ToDecimal(dgvProductRow.Cells["dgvcmbTax"].Value.ToString()) == decTaxId)
                                                        {
                                                            decAmount = decAmount + Convert.ToDecimal(dgvProductRow.Cells["dgvtxtTaxAmount"].Value.ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            decDefaultAmount = decAmount * 1;
                            dgvTaxRow.Cells["dgvtxtTotalTax"].Value = Math.Round(decDefaultAmount, PublicVariables._inNoOfDecimalPlaces);
                            decAmount = 0;
                        }
                    }
                }
                TaxSerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI29:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate total amount
        /// </summary>
        public void Calculate()
        {
            decimal decDiscount = 0;
            decimal decDiscountPercent = 0;
            decimal decGrossValue = 0;
            decimal decNetValue = 0;
            decimal decTaxAmount = 0;
            decimal decTaxPercent = 0;
            decimal decTaxId = 0;
            decimal decAmount = 0;
            decimal decTotalAmount = 0;
            decimal decProductId = 0;
            decimal decDefaultTotalAmount = 0;
            decimal decProductRate = 0;
            decimal decQuantity = 0;
            ProductInfo infoProduct = new ProductInfo();
            ProductSP spProduct = new ProductSP();
            TaxInfo infotax = new TaxInfo();
            TaxSP spTax = new TaxSP();
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            try
            {
                foreach (DataGridViewRow dgrow in dgvProductDetails.Rows)
                {
                    if (dgrow.Cells["dgvtxtProductId"].Value != null)
                    {
                        if (dgrow.Cells["dgvtxtProductId"].Value.ToString() != string.Empty)
                        {
                            if (dgrow.Cells["dgvtxtRate"].Value != null)
                            {
                                if (dgrow.Cells["dgvtxtRate"].Value.ToString() != string.Empty && dgrow.Cells["dgvtxtRate"].Value.ToString() != ".")
                                {
                                    decProductRate = Convert.ToDecimal(dgrow.Cells["dgvtxtRate"].Value.ToString());
                                }
                            }
                            if (dgrow.Cells["dgvtxtQuantity"].Value != null)
                            {
                                if (dgrow.Cells["dgvtxtQuantity"].Value.ToString() != string.Empty && dgrow.Cells["dgvtxtQuantity"].Value.ToString() != ".")
                                {
                                    decQuantity = Convert.ToDecimal(dgrow.Cells["dgvtxtQuantity"].Value.ToString());
                                }
                            }
                            decGrossValue = decProductRate * decQuantity;
                            dgrow.Cells["dgvtxtGrossValue"].Value = Math.Round(decGrossValue, PublicVariables._inNoOfDecimalPlaces);
                            if (dgrow.Cells["dgvtxtDiscountPercent"].Value != null)
                            {
                                if (dgrow.Cells["dgvtxtDiscountPercent"].Value.ToString() != string.Empty)
                                {
                                    decDiscountPercent = Convert.ToDecimal(dgrow.Cells["dgvtxtDiscountPercent"].Value.ToString());
                                }
                                else
                                {
                                    dgrow.Cells["dgvtxtDiscountPercent"].Value = 0;
                                }
                            }
                            else
                            {
                                dgrow.Cells["dgvtxtDiscountPercent"].Value = 0;
                            }
                            if (dgrow.Cells["dgvtxtDiscount"].Value != null)
                            {
                                if (dgrow.Cells["dgvtxtDiscount"].Value.ToString() != string.Empty)
                                {
                                    decDiscount = Convert.ToDecimal(dgrow.Cells["dgvtxtDiscount"].Value.ToString());
                                }
                                else
                                {
                                    dgrow.Cells["dgvtxtDiscount"].Value = 0;
                                }
                            }
                            else
                            {
                                dgrow.Cells["dgvtxtDiscount"].Value = 0;
                            }
                            /*------------------------------Calculate-----------------------------------*/
                            /*------------------------------Discount Calculation-----------------------------------*/
                            if (decGrossValue >= decDiscount)
                            {
                                dgrow.Cells["dgvtxtDiscount"].Value = Math.Round(decDiscount, PublicVariables._inNoOfDecimalPlaces);
                            }
                            else
                            {
                                dgrow.Cells["dgvtxtDiscountPercent"].Value = 0;
                                dgrow.Cells["dgvtxtDiscount"].Value = 0;
                                decDiscount = 0;
                            }
                            decNetValue = decGrossValue - decDiscount;
                            dgrow.Cells["dgvtxtNetValue"].Value = Math.Round(decNetValue, PublicVariables._inNoOfDecimalPlaces);
                            /*------------------------------Tax Calculation-----------------------------------*/
                            if (dgvcmbTax.Visible)
                            {
                                if (dgrow.Cells["dgvcmbTax"].Value != null)
                                {
                                    if (dgrow.Cells["dgvcmbTax"].Value.ToString() != string.Empty &&
                                        dgrow.Cells["dgvcmbTax"].Value.ToString() != "0")
                                    {
                                        decTaxId = Convert.ToDecimal(dgrow.Cells["dgvcmbTax"].Value.ToString());
                                        infotax = spTax.TaxView(decTaxId);
                                        decTaxPercent = infotax.Rate;
                                    }
                                    else
                                    {
                                        decTaxPercent = 0;
                                    }
                                }
                                else
                                {
                                    decTaxPercent = 0;
                                }
                                decProductId = Convert.ToDecimal(dgrow.Cells["dgvtxtProductId"].Value.ToString());
                                infoProduct = spProduct.ProductView(decProductId);
                                if (infoProduct.TaxapplicableOn == "MRP")
                                {
                                    decTaxAmount = infoProduct.Mrp * decTaxPercent / 100;
                                }
                                else
                                {
                                    decTaxAmount = decNetValue * decTaxPercent / 100;
                                }
                                dgrow.Cells["dgvtxtTaxAmount"].Value = Math.Round(decTaxAmount, PublicVariables._inNoOfDecimalPlaces);
                            }
                            decAmount = decNetValue + decTaxAmount;
                            dgrow.Cells["dgvtxtAmount"].Value = Math.Round(decAmount, PublicVariables._inNoOfDecimalPlaces);
                            decTotalAmount = decTotalAmount + decAmount;
                            decDefaultTotalAmount = decTotalAmount * 1;
                            txtTotalAmount.Text = Math.Round(decDefaultTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
                            if (dgvTax.Visible)
                            {
                                TotalTaxAmount();
                            }
                            CalculateGrandTotal();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI30:" + ex.Message;
            }
        }
        /// <summary>
        /// Functon to check invalid entries
        /// </summary>
        /// <param name="e"></param>
        public void CheckInvalidEntries(DataGridViewCellEventArgs e)// To check whether the values of grid is valid
        {
            SettingsSP spSettings = new SettingsSP();
            try
            {
                if (dgvProductDetails.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (dgvProductDetails.CurrentRow.Cells["dgvtxtProductName"].Value == null)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductDetails.CurrentRow.Cells["dgvtxtProductName"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductDetails.CurrentRow.Cells["dgvtxtQuantity"].Value == null)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductDetails.CurrentRow.Cells["dgvtxtQuantity"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvtxtQuantity"].Value) == 0)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value == null)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value.ToString().Trim() == string.Empty)
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (spSettings.SettingsStatusCheck("AllowZeroValueEntry") == "No" && (Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value) == 0))
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = "X";
                            dgvProductDetails.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            isValueChanged = true;
                            dgvProductDetails.CurrentRow.HeaderCell.Value = string.Empty;
                        }
                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI31:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate total tax amount
        /// </summary>
        /// <returns></returns>
        public decimal TaxAmountApplicableOnBill()
        {
            decimal decTaxId = 0;
            decimal decTaxRate = 0;
            decimal decTaxOnBill = 0;
            decimal decTotalTaxOnBill = 0;
            decimal decTaxOnTax = 0;
            decimal decTotalTaxOnTax = 0;
            decimal decTotalAmount = 0;
            decimal decTotalTax = 0;
            TaxDetailsSP spTaxDetails = new TaxDetailsSP();
            DataTable dtbl = new DataTable();
            try
            {
                TaxAmountForTaxType();
                if (txtTotalAmount.Text != string.Empty)
                {
                    decTotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                }
                foreach (DataGridViewRow dgvRow in dgvTax.Rows)
                {
                    if (dgvRow.Cells["dgvtxtTaxId"].Value != null)
                    {
                        if (dgvRow.Cells["dgvtxtApplicableOn"].Value != null && dgvRow.Cells["dgvtxtCalculatingMode"].Value != null)
                        {
                            if (dgvRow.Cells["dgvtxtApplicableOn"].Value.ToString() == "Bill" && dgvRow.Cells["dgvtxtCalculatingMode"].Value.ToString() == "Bill Amount")
                            {
                                decTaxRate = Convert.ToDecimal(dgvRow.Cells["dgvtxtTaxRate"].Value.ToString());
                                decTaxOnBill = (decTotalAmount * decTaxRate / 100);
                                dgvRow.Cells["dgvtxtTotalTax"].Value = Math.Round(decTaxOnBill, PublicVariables._inNoOfDecimalPlaces);
                                decTotalTaxOnBill = decTotalTaxOnBill + decTaxOnBill;
                            }
                        }
                    }
                }
                foreach (DataGridViewRow dgvRow1 in dgvTax.Rows)
                {
                    if (dgvRow1.Cells["dgvtxtTaxId"].Value != null)
                    {
                        if (dgvRow1.Cells["dgvtxtApplicableOn"].Value != null && dgvRow1.Cells["dgvtxtCalculatingMode"].Value != null)
                        {
                            if (dgvRow1.Cells["dgvtxtApplicableOn"].Value.ToString() == "Bill" && dgvRow1.Cells["dgvtxtCalculatingMode"].Value.ToString() == "Tax Amount")
                            {
                                decTaxId = Convert.ToDecimal(dgvRow1.Cells["dgvtxtTaxId"].Value.ToString());
                                dtbl = spTaxDetails.TaxDetailsViewallByTaxId(decTaxId);
                                foreach (DataGridViewRow dgvRow2 in dgvTax.Rows)
                                {
                                    foreach (DataRow drow in dtbl.Rows)
                                    {
                                        if (dgvRow2.Cells["dgvtxtTaxId"].Value != null)
                                        {
                                            if (dgvRow2.Cells["dgvtxtTaxId"].Value.ToString() == drow.ItemArray[0].ToString())
                                            {
                                                decTaxRate = Convert.ToDecimal(dgvRow1.Cells["dgvtxtTaxRate"].Value.ToString());
                                                decTotalAmount = Convert.ToDecimal(dgvRow2.Cells["dgvtxtTotalTax"].Value.ToString());
                                                decTaxOnTax = (decTotalAmount * decTaxRate / 100);
                                                dgvRow1.Cells["dgvtxtTotalTax"].Value = Math.Round(decTaxOnTax, PublicVariables._inNoOfDecimalPlaces);
                                                decTotalTaxOnTax = decTotalTaxOnTax + decTaxOnTax;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI32:" + ex.Message;
            }
            decTotalTax = decTotalTaxOnBill + decTotalTaxOnTax;
            return decTotalTax;
        }
        /// <summary>
        /// Function to calculate grand total
        /// </summary>
        public void CalculateGrandTotal()
        {
            decimal decTotalAmount = 0;
            decimal decAdditionalCost = 0;
            decimal decTaxAmount = 0;
            decimal decBillDiscount = 0;
            decimal decGrandTotal = 0;
            try
            {
                if (txtTotalAmount.Text != string.Empty)
                {
                    decTotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                }
                if (lblAdditionalCostAmount.Text != string.Empty)
                {
                    decAdditionalCost = Convert.ToDecimal(lblAdditionalCostAmount.Text);
                }
                if (dgvTax.Visible)
                {
                    TaxAmountApplicableOnBill();
                    foreach (DataGridViewRow dgvrow in dgvTax.Rows)
                    {
                        if (dgvrow.Cells["dgvtxtApplicableOn"].Value != null)
                        {
                            if (dgvrow.Cells["dgvtxtApplicableOn"].Value.ToString() == "Bill")
                            {
                                decTaxAmount = decTaxAmount + Convert.ToDecimal(dgvrow.Cells["dgvtxtTotalTax"].Value.ToString());
                            }
                        }
                    }
                }
                if (txtBillDiscount.Text != string.Empty)
                {
                    decBillDiscount = Convert.ToDecimal(txtBillDiscount.Text);
                }
                decGrandTotal = decTotalAmount + decAdditionalCost + decTaxAmount - decBillDiscount;
                if (decGrandTotal >= 0)
                {
                    txtGrandTotal.Text = Math.Round(decGrandTotal, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
                else
                {
                    txtBillDiscount.Text = Math.Round(0.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                    txtGrandTotal.Text = Math.Round(decTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI33:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill products to arraylist
        /// </summary>
        public void FillProducts()
        {
            ProductSP spProduct = new ProductSP();
            try
            {
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
                formMDI.infoError.ErrorString = "PI34:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check quantity with reference
        /// </summary>
        /// <returns></returns>
        public int QuantityCheckWithReference()
        {
            decimal decQtyPurchaseInvoice = 0;
            decimal decQtyPurchaseReturn = 0;
            int inRef = 0;
            int inF1 = 1;
            decimal decPurchaseDetailsId = 0;
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            PurchaseReturnDetailsSP spPurchaseReturnDetails = new PurchaseReturnDetailsSP();
            try
            {
                foreach (DataGridViewRow dgvrow in dgvProductDetails.Rows)
                {
                    if (dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() != "0" || dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() != string.Empty)
                        {
                            decPurchaseDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString());
                            inRef = spPurchaseMaster.PurchaseMasterReferenceCheck(decPurchaseMasterId, decPurchaseDetailsId);
                            if (inRef == 1)
                            {
                                if (inF1 == 1)
                                {
                                    if (dgvrow.Cells["dgvtxtQuantity"].Value != null)
                                    {
                                        if (dgvrow.Cells["dgvtxtQuantity"].Value.ToString() != "0" && dgvrow.Cells["dgvtxtQuantity"].Value.ToString() != string.Empty)
                                        {
                                            decQtyPurchaseInvoice = Convert.ToDecimal(dgvrow.Cells["dgvtxtQuantity"].Value.ToString());
                                            decQtyPurchaseReturn = Math.Round(spPurchaseReturnDetails.PurchaseReturnDetailsQtyViewByPurchaseDetailsId(decPurchaseDetailsId), PublicVariables._inNoOfDecimalPlaces);
                                            if (decQtyPurchaseInvoice >= decQtyPurchaseReturn)
                                            {
                                                inF1 = 1;
                                            }
                                            else
                                            {
                                                inF1 = 0;
                                                Messages.InformationMessage("Quantity in row " + (dgvrow.Index + 1) + " should be greater than " + decQtyPurchaseReturn);
                                            }
                                        }
                                        else
                                        {
                                            inF1 = 0;
                                            Messages.InformationMessage("Quantity in row " + (dgvrow.Index + 1) + " should be greater than " + decQtyPurchaseReturn);
                                        }
                                    }
                                    else
                                    {
                                        inF1 = 0;
                                        Messages.InformationMessage("Quantity in row " + (dgvrow.Index + 1) + " should be greater than " + decQtyPurchaseReturn);
                                    }
                                }
                            }
                            else
                            {
                                inF1 = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI35:" + ex.Message;
            }
            return inF1;
        }
        /// <summary>
        /// Function to check party balance with reference
        /// </summary>
        /// <returns></returns>
        public int PartyBalanceCheckWithReference()
        {
            int inF1 = 0;
            decimal decPartyBalanceAmount = 0;
            decimal decGrandTotal = 0;
            try
            {
                bool isRef = false;
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                PartyBalanceSP spPartyBalance = new PartyBalanceSP();
                isRef = spAccountLedger.PartyBalanceAgainstReferenceCheck(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                if (isRef)
                {
                    decPartyBalanceAmount = spPartyBalance.PartyBalanceAmountViewByVoucherNoVoucherTypeIdAndReferenceType
                        (strVoucherNo, decPurchaseInvoiceVoucherTypeId, "Against");
                    decGrandTotal = Convert.ToDecimal(txtGrandTotal.Text);
                    if (decGrandTotal >= decPartyBalanceAmount)
                    {
                        inF1 = 1;
                    }
                    else
                    {
                        inF1 = 0;
                        Messages.InformationMessage("There is a payment voucher against this invoice so grand total should not be less than " + decPartyBalanceAmount);
                    }
                }
                else
                {
                    inF1 = 1;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI36:" + ex.Message;
            }
            return inF1;
        }
        /// <summary>
        /// Function for saveoredit
        /// </summary>
        public void SaveOrEdit()
        {
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            try
            {
                int inRowCount = dgvProductDetails.RowCount;
                dgvProductDetails.ClearSelection();
                if (txtVoucherNo.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter voucher number");
                    txtVoucherNo.Focus();
                }
                else if (spPurchaseMaster.PurchaseInvoiceVoucherNoCheckExistance(txtVoucherNo.Text.Trim(), strVoucherNo, decPurchaseInvoiceVoucherTypeId, decPurchaseMasterId) == 1)
                {
                    Messages.InformationMessage("Voucher number already exist");
                    txtVoucherNo.Focus();
                }
                else if (txtVoucherDate.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Select a date in between financial year");
                    txtVoucherDate.Focus();
                }
                else if (txtInvoiceDate.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Select a date in between financial year");
                    txtInvoiceDate.Focus();
                }
                else if (cmbCashOrParty.SelectedValue == null)
                {
                    Messages.InformationMessage("Select Cash/Party");
                    cmbCashOrParty.Focus();
                }
                else if (cmbPurchaseAccount.SelectedValue == null)
                {
                    Messages.InformationMessage("Select PurchaseAccount");
                    cmbPurchaseAccount.Focus();
                }
                else if (cmbCurrency.SelectedValue == null)
                {
                    Messages.InformationMessage("Select Currency");
                    cmbCurrency.Focus();
                }
                else if (cmbCurrency.SelectedValue.ToString() == "0")
                {
                    Messages.InformationMessage("Select Currency");
                    cmbCurrency.Focus();
                }
                else if (cmbPurchaseMode.Text == "Against Purchase Order" && cmbOrderNo.Text == string.Empty)
                {
                    Messages.InformationMessage("Select OrderNo");
                    cmbOrderNo.Focus();
                }
                else if (cmbPurchaseMode.Text == "Against Material Receipt" && cmbOrderNo.Text == string.Empty)
                {
                    Messages.InformationMessage("Select ReceiptNo");
                    cmbOrderNo.Focus();
                }
                else
                {
                    if (RemoveIncompleteRowsFromGrid())
                    {
                        if (dgvProductDetails.Rows.Count != 0)
                        {
                            if (dgvProductDetails.Rows[0].Cells["dgvtxtProductName"].Value == null && dgvProductDetails.Rows[0].Cells["dgvtxtProductCode"].Value == null)
                            {
                                MessageBox.Show("Can't save purchase invoice without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgvProductDetails.ClearSelection();
                                dgvProductDetails.Focus();
                            }
                            else
                            {
                                if (btnSave.Text == "Save")
                                {
                                    if (dgvProductDetails.Rows[0].Cells["dgvtxtProductName"].Value == null)
                                    {
                                        MessageBox.Show("Can't save purchase order without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        dgvProductDetails.ClearSelection();
                                        dgvProductDetails.Focus();
                                    }
                                    else
                                    {
                                        if (PublicVariables.isMessageAdd)
                                        {
                                            if (Messages.SaveMessage())
                                            {
                                                Save();
                                            }
                                        }
                                        else
                                        {
                                            Save();
                                        }
                                    }
                                }
                                if (btnSave.Text == "Update")
                                {
                                    if (QuantityCheckWithReference() == 1 && PartyBalanceCheckWithReference() == 1)
                                    {
                                        if (dgvProductDetails.Rows[0].Cells["dgvtxtProductName"].Value == null)
                                        {
                                            MessageBox.Show("Can't Edit purchase invoice without atleast one product with complete details", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            dgvProductDetails.ClearSelection();
                                            dgvProductDetails.Focus();
                                        }
                                        else
                                        {
                                            if (PublicVariables.isMessageEdit)
                                            {
                                                if (Messages.UpdateMessage())
                                                {
                                                    Edit();
                                                }
                                            }
                                            else
                                            {
                                                Edit();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Messages.InformationMessage("Can't save purchase invoice without atleast one product with complete details");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI37:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save purchase invoice
        /// </summary>
        public void Save()
        {
            decimal decPurchaseMasterId = 0;
            PurchaseMasterInfo infoPurchaseMaster = new PurchaseMasterInfo();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            PurchaseDetailsInfo infoPurchaseDetails = new PurchaseDetailsInfo();
            PurchaseDetailsSP spPurchaseDetails = new PurchaseDetailsSP();
            MaterialReceiptMasterInfo infoMaterialReceiptMaster = new MaterialReceiptMasterInfo();
            MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
            PurchaseOrderMasterInfo infoPurchaseOrderMaster = new PurchaseOrderMasterInfo();
            PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
            StockPostingInfo infoStockPosting = new StockPostingInfo();
            StockPostingSP spStockPosting = new StockPostingSP();
            LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
            LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
            PartyBalanceInfo infoPartyBalance = new PartyBalanceInfo();
            PartyBalanceSP spPartyBalance = new PartyBalanceSP();
            AdditionalCostInfo infoAdditionalCost = new AdditionalCostInfo();
            AdditionalCostSP spAdditionalCost = new AdditionalCostSP();
            PurchaseBillTaxInfo infoPurchaseBillTax = new PurchaseBillTaxInfo();
            PurchaseBillTaxSP spPurchaseBillTax = new PurchaseBillTaxSP();
            AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
            AccountLedgerSP spAccountLedger = new AccountLedgerSP();
            UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            try
            {
                /*-----------------------------------------Purchase Master Add----------------------------------------------------*/
                infoPurchaseMaster.AdditionalCost = Convert.ToDecimal(lblAdditionalCostAmount.Text);
                infoPurchaseMaster.BillDiscount = Convert.ToDecimal(txtBillDiscount.Text);
                infoPurchaseMaster.CreditPeriod = txtCreditPeriod.Text;
                infoPurchaseMaster.Date = Convert.ToDateTime(txtVoucherDate.Text);
                infoPurchaseMaster.ExchangeRateId = Convert.ToDecimal(cmbCurrency.SelectedValue.ToString());
                infoPurchaseMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoPurchaseMaster.GrandTotal = Convert.ToDecimal(txtGrandTotal.Text);
                infoPurchaseMaster.InvoiceNo = txtVoucherNo.Text.Trim();
                if (isAutomatic)
                {
                    infoPurchaseMaster.SuffixPrefixId = decPurchaseInvoiceSuffixPrefixId;
                    infoPurchaseMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoPurchaseMaster.SuffixPrefixId = 0;
                    infoPurchaseMaster.VoucherNo = strVoucherNo;
                }
                infoPurchaseMaster.LedgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                infoPurchaseMaster.LrNo = txtLRNo.Text;
                if (cmbPurchaseMode.Text == "Against Material Receipt")
                {
                    infoPurchaseMaster.MaterialReceiptMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                else
                {
                    infoPurchaseMaster.MaterialReceiptMasterId = 0;
                }
                infoPurchaseMaster.Narration = txtNarration.Text;
                infoPurchaseMaster.PurchaseAccount = Convert.ToDecimal(cmbPurchaseAccount.SelectedValue.ToString());
                if (cmbPurchaseMode.Text == "Against Purchase Order")
                {
                    infoPurchaseMaster.PurchaseOrderMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                else
                {
                    infoPurchaseMaster.PurchaseOrderMasterId = 0;
                }
                infoPurchaseMaster.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                infoPurchaseMaster.TotalTax = Convert.ToDecimal(lblTaxAmount.Text);
                infoPurchaseMaster.TransportationCompany = txtTransportationCompany.Text;
                infoPurchaseMaster.UserId = PublicVariables._decCurrentUserId;
                infoPurchaseMaster.VendorInvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
                infoPurchaseMaster.VendorInvoiceNo = txtVendorInvoiceNo.Text;
                infoPurchaseMaster.VoucherTypeId = decPurchaseInvoiceVoucherTypeId;
                infoPurchaseMaster.Extra1 = string.Empty;
                infoPurchaseMaster.Extra2 = string.Empty;
                infoPurchaseMaster.ExtraDate = Convert.ToDateTime(DateTime.Now);
                decPurchaseMasterId = spPurchaseMaster.PurchaseMasterAdd(infoPurchaseMaster);
                infoPurchaseOrderMaster = spPurchaseOrderMaster.PurchaseOrderMasterView(infoPurchaseMaster.PurchaseOrderMasterId);
                infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(infoPurchaseMaster.MaterialReceiptMasterId);
                foreach (DataGridViewRow dgvrow in dgvProductDetails.Rows)
                {
                    if (dgvrow.Cells["dgvtxtProductId"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtProductId"].Value.ToString() != string.Empty)
                        {
                            /*-----------------------------------------Purchase Details Add----------------------------------------------------*/
                            infoPurchaseDetails.Amount = Convert.ToDecimal(dgvrow.Cells["dgvtxtAmount"].Value.ToString());
                            infoPurchaseDetails.BatchId = Convert.ToDecimal(dgvrow.Cells["dgvcmbBatch"].Value.ToString());
                            infoPurchaseDetails.Discount = Convert.ToDecimal(dgvrow.Cells["dgvtxtDiscount"].Value.ToString());
                            infoPurchaseDetails.GodownId = Convert.ToDecimal(dgvrow.Cells["dgvcmbGodown"].Value.ToString());
                            infoPurchaseDetails.GrossAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtGrossValue"].Value.ToString());
                            infoPurchaseDetails.NetAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtNetValue"].Value.ToString());
                            infoPurchaseDetails.OrderDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseOrderDetailsId"].Value.ToString());
                            infoPurchaseDetails.ProductId = Convert.ToDecimal(dgvrow.Cells["dgvtxtProductId"].Value.ToString());
                            infoPurchaseDetails.PurchaseMasterId = decPurchaseMasterId;
                            infoPurchaseDetails.Qty = Convert.ToDecimal(dgvrow.Cells["dgvtxtQuantity"].Value.ToString());
                            infoPurchaseDetails.RackId = Convert.ToDecimal(dgvrow.Cells["dgvcmbRack"].Value.ToString());
                            infoPurchaseDetails.Rate = Convert.ToDecimal(dgvrow.Cells["dgvtxtRate"].Value.ToString());
                            infoPurchaseDetails.ReceiptDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtMaterialReceiptDetailsId"].Value.ToString());
                            infoPurchaseDetails.SlNo = Convert.ToInt32(dgvrow.Cells["dgvtxtSlNo"].Value.ToString());
                            infoPurchaseDetails.TaxAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtTaxAmount"].Value.ToString());
                            infoPurchaseDetails.TaxId = Convert.ToDecimal(dgvrow.Cells["dgvcmbTax"].Value.ToString());
                            infoPurchaseDetails.UnitConversionId = Convert.ToDecimal(dgvrow.Cells["dgvtxtUnitConversionId"].Value.ToString());
                            infoPurchaseDetails.UnitId = Convert.ToDecimal(dgvrow.Cells["dgvcmbUnit"].Value.ToString());
                            infoPurchaseDetails.Extra1 = string.Empty;
                            infoPurchaseDetails.Extra2 = string.Empty;
                            infoPurchaseDetails.ExtraDate = Convert.ToDateTime(DateTime.Today);
                            spPurchaseDetails.PurchaseDetailsAdd(infoPurchaseDetails);
                            /*-----------------------------------------Stock Posting----------------------------------------------------*/
                            infoStockPosting.BatchId = infoPurchaseDetails.BatchId;
                            infoStockPosting.Date = infoPurchaseMaster.Date;
                            infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                            infoStockPosting.GodownId = infoPurchaseDetails.GodownId;
                            infoStockPosting.InwardQty = infoPurchaseDetails.Qty; /// spUnitConvertion.UnitConversionRateByUnitConversionId(infoPurchaseDetails.UnitConversionId);
                            infoStockPosting.OutwardQty = 0;
                            infoStockPosting.ProductId = infoPurchaseDetails.ProductId;
                            infoStockPosting.RackId = infoPurchaseDetails.RackId;
                            infoStockPosting.Rate = infoPurchaseDetails.Rate;
                            infoStockPosting.UnitId = infoPurchaseDetails.UnitId;
                            if (infoPurchaseDetails.OrderDetailsId != 0)
                            {
                                infoStockPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = "NA";
                                infoStockPosting.AgainstVoucherNo = "NA";
                                infoStockPosting.AgainstVoucherTypeId = 0;
                            }
                            else if (infoPurchaseDetails.ReceiptDetailsId != 0)
                            {
                                infoStockPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = "NA";
                                infoStockPosting.AgainstVoucherNo = "NA";
                                infoStockPosting.AgainstVoucherTypeId = 0;
                            }
                            else if (infoPurchaseDetails.OrderDetailsId == 0 && infoPurchaseDetails.ReceiptDetailsId == 0)
                            {
                                infoStockPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = "NA";
                                infoStockPosting.AgainstVoucherNo = "NA";
                                infoStockPosting.AgainstVoucherTypeId = 0;
                            }
                            infoStockPosting.Extra1 = string.Empty;
                            infoStockPosting.Extra2 = string.Empty;
                            infoStockPosting.ExtraDate = Convert.ToDateTime(DateTime.Today);
                            spStockPosting.StockPostingAdd(infoStockPosting);
                            if (infoPurchaseDetails.ReceiptDetailsId != 0)
                            {
                                infoStockPosting.InvoiceNo = infoMaterialReceiptMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoMaterialReceiptMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoMaterialReceiptMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.AgainstVoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.AgainstVoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.InwardQty = 0;
                                infoStockPosting.OutwardQty = infoPurchaseDetails.Qty;/// spUnitConvertion.UnitConversionRateByUnitConversionId(infoPurchaseDetails.UnitConversionId);
                                spStockPosting.StockPostingAdd(infoStockPosting);
                            }
                        }
                    }
                }
                /*-----------------------------------------Ledger Posting----------------------------------------------------*/
                infoLedgerPosting.Credit = Convert.ToDecimal(txtGrandTotal.Text) * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                infoLedgerPosting.Debit = 0;
                infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                infoLedgerPosting.LedgerId = infoPurchaseMaster.LedgerId;
                infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                infoLedgerPosting.ExtraDate = DateTime.Now;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                decimal decBilldiscount = Convert.ToDecimal(txtBillDiscount.Text.ToString());
                if (decBilldiscount > 0)
                {
                    infoLedgerPosting.Credit = decBilldiscount * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                    infoLedgerPosting.Debit = 0;
                    infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                    infoLedgerPosting.DetailsId = 0;
                    infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                    infoLedgerPosting.LedgerId = 9;//ledger id of discount received
                    infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                    infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.ChequeDate = DateTime.Now;
                    infoLedgerPosting.ChequeNo = string.Empty;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    infoLedgerPosting.ExtraDate = DateTime.Now;
                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                }
                infoLedgerPosting.Credit = 0;
                infoLedgerPosting.Debit = TotalNetAmount(); //* spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                infoLedgerPosting.LedgerId = infoPurchaseMaster.PurchaseAccount;//ledger posting of purchase account
                infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                infoLedgerPosting.ExtraDate = DateTime.Now;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                foreach (DataGridViewRow dgvrow in dgvAdditionalCost.Rows)
                {
                    if (dgvrow.Cells["dgvcmbLedger"].Value != null)
                    {
                        if (dgvrow.Cells["dgvcmbLedger"].Value.ToString() != string.Empty)
                        {
                            if (dgvrow.Cells["dgvtxtAdditionalCostAmount"].Value != null)
                            {
                                if (dgvrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString() != string.Empty)
                                {
                                    /*-----------------------------------------Additional Cost Add----------------------------------------------------*/
                                    infoAdditionalCost.Credit = 0;
                                    infoAdditionalCost.Debit = Convert.ToDecimal(dgvrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString());
                                    infoAdditionalCost.LedgerId = Convert.ToDecimal(dgvrow.Cells["dgvcmbLedger"].Value.ToString());
                                    infoAdditionalCost.VoucherNo = infoPurchaseMaster.VoucherNo;
                                    infoAdditionalCost.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                    infoAdditionalCost.Extra1 = string.Empty;
                                    infoAdditionalCost.Extra2 = string.Empty;
                                    infoAdditionalCost.ExtraDate = DateTime.Now;
                                    spAdditionalCost.AdditionalCostAdd(infoAdditionalCost);
                                    /*-----------------------------------------Additional Cost Ledger Posting----------------------------------------------------*/
                                    infoLedgerPosting.Credit = 0;
                                    infoLedgerPosting.Debit = infoAdditionalCost.Debit * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                                    infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                                    infoLedgerPosting.DetailsId = 0;
                                    infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                    infoLedgerPosting.LedgerId = infoAdditionalCost.LedgerId;
                                    infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                    infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                                    infoLedgerPosting.ChequeDate = DateTime.Now;
                                    infoLedgerPosting.ChequeNo = string.Empty;
                                    infoLedgerPosting.Extra1 = string.Empty;
                                    infoLedgerPosting.Extra2 = string.Empty;
                                    infoLedgerPosting.ExtraDate = DateTime.Now;
                                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                                }
                            }
                        }
                    }
                }
                if (dgvTax.Visible)
                {
                    foreach (DataGridViewRow dgvrow in dgvTax.Rows)
                    {
                        if (dgvrow.Cells["dgvtxtTaxId"].Value != null)
                        {
                            if (dgvrow.Cells["dgvtxtTaxId"].Value.ToString() != string.Empty)
                            {
                                /*-----------------------------------------PurchaseBillTax Add----------------------------------------------------*/
                                infoPurchaseBillTax.PurchaseMasterId = decPurchaseMasterId;
                                infoPurchaseBillTax.TaxAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtTotalTax"].Value.ToString());
                                infoPurchaseBillTax.TaxId = Convert.ToDecimal(dgvrow.Cells["dgvtxtTaxId"].Value.ToString());
                                infoPurchaseBillTax.Extra1 = string.Empty;
                                infoPurchaseBillTax.Extra2 = string.Empty;
                                infoPurchaseBillTax.ExtraDate = DateTime.Now;
                                spPurchaseBillTax.PurchaseBillTaxAdd(infoPurchaseBillTax);
                                /*-----------------------------------------Tax Ledger Posting----------------------------------------------------*/
                                infoLedgerPosting.Credit = 0;
                                infoLedgerPosting.Debit = infoPurchaseBillTax.TaxAmount * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                                infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                                infoLedgerPosting.DetailsId = 0;
                                infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoLedgerPosting.LedgerId = Convert.ToDecimal(dgvrow.Cells["dgvtxtLedgerId"].Value.ToString());
                                infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                                infoLedgerPosting.ChequeDate = DateTime.Now;
                                infoLedgerPosting.ChequeNo = string.Empty;
                                infoLedgerPosting.Extra1 = string.Empty;
                                infoLedgerPosting.Extra2 = string.Empty;
                                infoLedgerPosting.ExtraDate = DateTime.Now;
                                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }
                    }
                }
                /*-----------------------------------------PartyBalance Posting----------------------------------------------------*/
                infoAccountLedger = spAccountLedger.AccountLedgerView(infoPurchaseMaster.LedgerId);
                if (infoAccountLedger.BillByBill == true)
                {
                    infoPartyBalance.Credit = Convert.ToDecimal(txtGrandTotal.Text);
                    infoPartyBalance.Debit = 0;
                    if (txtCreditPeriod.Text != string.Empty)
                    {
                        infoPartyBalance.CreditPeriod = Convert.ToInt32(txtCreditPeriod.Text);
                    }
                    infoPartyBalance.Date = Convert.ToDateTime(txtVoucherDate.Text);
                    infoPartyBalance.ExchangeRateId = infoPurchaseMaster.ExchangeRateId;
                    infoPartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                    infoPartyBalance.LedgerId = infoPurchaseMaster.LedgerId;
                    infoPartyBalance.ReferenceType = "NEW";
                    infoPartyBalance.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                    infoPartyBalance.VoucherNo = infoPurchaseMaster.VoucherNo;
                    infoPartyBalance.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                    infoPartyBalance.AgainstInvoiceNo = "NA";
                    infoPartyBalance.AgainstVoucherNo = "NA";
                    infoPartyBalance.AgainstVoucherTypeId = 0;
                    infoPartyBalance.Extra1 = string.Empty;
                    infoPartyBalance.Extra2 = string.Empty;
                    infoPartyBalance.ExtraDate = DateTime.Now;
                    spPartyBalance.PartyBalanceAdd(infoPartyBalance);
                }
                Messages.SavedMessage();
                if (cbxPrintAfterSave.Checked)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(decPurchaseMasterId);
                    }
                    else
                    {
                        Print(decPurchaseMasterId);
                    }
                }
                Clear();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI38:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit purchase invoice
        /// </summary>
        public void Edit()
        {
            PurchaseMasterInfo infoPurchaseMaster = new PurchaseMasterInfo();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            PurchaseDetailsInfo infoPurchaseDetails = new PurchaseDetailsInfo();
            PurchaseDetailsSP spPurchaseDetails = new PurchaseDetailsSP();
            MaterialReceiptMasterInfo infoMaterialReceiptMaster = new MaterialReceiptMasterInfo();
            MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
            PurchaseOrderMasterInfo infoPurchaseOrderMaster = new PurchaseOrderMasterInfo();
            PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
            StockPostingInfo infoStockPosting = new StockPostingInfo();
            StockPostingSP spStockPosting = new StockPostingSP();
            LedgerPostingInfo infoLedgerPosting = new LedgerPostingInfo();
            LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
            PartyBalanceInfo infoPartyBalance = new PartyBalanceInfo();
            PartyBalanceSP spPartyBalance = new PartyBalanceSP();
            AdditionalCostInfo infoAdditionalCost = new AdditionalCostInfo();
            AdditionalCostSP spAdditionalCost = new AdditionalCostSP();
            PurchaseBillTaxInfo infoPurchaseBillTax = new PurchaseBillTaxInfo();
            PurchaseBillTaxSP spPurchaseBillTax = new PurchaseBillTaxSP();
            AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
            AccountLedgerSP spAccountLedger = new AccountLedgerSP();
            UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            /*---------------------------------Deleting previous stock posting, Ledger posting, partybalanceposting---------------------------------------*/
            infoPurchaseMaster = spPurchaseMaster.PurchaseMasterView(decPurchaseMasterId);
            if (infoPurchaseMaster.MaterialReceiptMasterId != 0)
            {
                infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(infoPurchaseMaster.MaterialReceiptMasterId);
                spStockPosting.StockPostingDeleteForSalesInvoiceAgainstDeliveryNote
                    (infoPurchaseMaster.VoucherTypeId, infoPurchaseMaster.VoucherNo,
                    infoMaterialReceiptMaster.VoucherNo, infoMaterialReceiptMaster.VoucherTypeId);
            }
            spStockPosting.StockPostingDeleteByagainstVoucherTypeIdAndagainstVoucherNoAndVoucherNoAndVoucherType
                    (0, "NA", infoPurchaseMaster.VoucherNo, infoPurchaseMaster.VoucherTypeId);
            try
            {
                RemoveDelete();
                /*-----------------------------------------Purchase Master Edit----------------------------------------------------*/
                infoPurchaseMaster.AdditionalCost = Convert.ToDecimal(lblAdditionalCostAmount.Text);
                infoPurchaseMaster.BillDiscount = Convert.ToDecimal(txtBillDiscount.Text);
                infoPurchaseMaster.CreditPeriod = txtCreditPeriod.Text;
                infoPurchaseMaster.Date = Convert.ToDateTime(txtVoucherDate.Text);
                infoPurchaseMaster.ExchangeRateId = Convert.ToDecimal(cmbCurrency.SelectedValue.ToString());
                infoPurchaseMaster.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoPurchaseMaster.GrandTotal = Convert.ToDecimal(txtGrandTotal.Text);
                infoPurchaseMaster.InvoiceNo = txtVoucherNo.Text;
                if (isAutomatic)
                {
                    infoPurchaseMaster.SuffixPrefixId = decPurchaseInvoiceSuffixPrefixId;
                    infoPurchaseMaster.VoucherNo = strVoucherNo;
                }
                else
                {
                    infoPurchaseMaster.SuffixPrefixId = 0;
                    infoPurchaseMaster.VoucherNo = strVoucherNo;
                }
                infoPurchaseMaster.LedgerId = Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString());
                infoPurchaseMaster.LrNo = txtLRNo.Text;
                if (cmbPurchaseMode.Text == "Against Material Receipt")
                {
                    infoPurchaseMaster.MaterialReceiptMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                else
                {
                    infoPurchaseMaster.MaterialReceiptMasterId = 0;
                }
                infoPurchaseMaster.Narration = txtNarration.Text;
                infoPurchaseMaster.PurchaseAccount = Convert.ToDecimal(cmbPurchaseAccount.SelectedValue.ToString());
                if (cmbPurchaseMode.Text == "Against Purchase Order")
                {
                    infoPurchaseMaster.PurchaseOrderMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                else
                {
                    infoPurchaseMaster.PurchaseOrderMasterId = 0;
                }
                infoPurchaseMaster.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                infoPurchaseMaster.TotalTax = Convert.ToDecimal(lblTaxAmount.Text);
                infoPurchaseMaster.TransportationCompany = txtTransportationCompany.Text;
                infoPurchaseMaster.UserId = PublicVariables._decCurrentUserId;
                infoPurchaseMaster.VendorInvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
                infoPurchaseMaster.VendorInvoiceNo = txtVendorInvoiceNo.Text;
                infoPurchaseMaster.VoucherTypeId = decPurchaseInvoiceVoucherTypeId;
                infoPurchaseMaster.Extra1 = string.Empty;
                infoPurchaseMaster.Extra2 = string.Empty;
                infoPurchaseMaster.ExtraDate = Convert.ToDateTime(DateTime.Now);
                infoPurchaseMaster.PurchaseMasterId = decPurchaseMasterId;
                spPurchaseMaster.PurchaseMasterEdit(infoPurchaseMaster);
                infoPurchaseOrderMaster = spPurchaseOrderMaster.PurchaseOrderMasterView(infoPurchaseMaster.PurchaseOrderMasterId);
                infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(infoPurchaseMaster.MaterialReceiptMasterId);
                spLedgerPosting.LedgerPostDelete(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                spAccountLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                foreach (DataGridViewRow dgvrow in dgvProductDetails.Rows)
                {
                    if (dgvrow.Cells["dgvtxtProductId"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtProductId"].Value.ToString() != string.Empty)
                        {
                            /*-----------------------------------------Purchase Details Add----------------------------------------------------*/
                            infoPurchaseDetails.Amount = Convert.ToDecimal(dgvrow.Cells["dgvtxtAmount"].Value.ToString());
                            infoPurchaseDetails.BatchId = Convert.ToDecimal(dgvrow.Cells["dgvcmbBatch"].Value.ToString());
                            infoPurchaseDetails.Discount = Convert.ToDecimal(dgvrow.Cells["dgvtxtDiscount"].Value.ToString());
                            infoPurchaseDetails.GodownId = Convert.ToDecimal(dgvrow.Cells["dgvcmbGodown"].Value.ToString());
                            infoPurchaseDetails.GrossAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtGrossValue"].Value.ToString());
                            infoPurchaseDetails.NetAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtNetValue"].Value.ToString());
                            infoPurchaseDetails.OrderDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseOrderDetailsId"].Value.ToString());
                            infoPurchaseDetails.ProductId = Convert.ToDecimal(dgvrow.Cells["dgvtxtProductId"].Value.ToString());
                            infoPurchaseDetails.PurchaseMasterId = decPurchaseMasterId;
                            infoPurchaseDetails.Qty = Convert.ToDecimal(dgvrow.Cells["dgvtxtQuantity"].Value.ToString());
                            infoPurchaseDetails.RackId = Convert.ToDecimal(dgvrow.Cells["dgvcmbRack"].Value.ToString());
                            infoPurchaseDetails.Rate = Convert.ToDecimal(dgvrow.Cells["dgvtxtRate"].Value.ToString());
                            infoPurchaseDetails.ReceiptDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtMaterialReceiptDetailsId"].Value.ToString());
                            infoPurchaseDetails.SlNo = Convert.ToInt32(dgvrow.Cells["dgvtxtSlNo"].Value.ToString());
                            infoPurchaseDetails.TaxAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtTaxAmount"].Value.ToString());
                            infoPurchaseDetails.TaxId = Convert.ToDecimal(dgvrow.Cells["dgvcmbTax"].Value.ToString());
                            infoPurchaseDetails.UnitConversionId = Convert.ToDecimal(dgvrow.Cells["dgvtxtUnitConversionId"].Value.ToString());
                            infoPurchaseDetails.UnitId = Convert.ToDecimal(dgvrow.Cells["dgvcmbUnit"].Value.ToString());
                            infoPurchaseDetails.Extra1 = string.Empty;
                            infoPurchaseDetails.Extra2 = string.Empty;
                            infoPurchaseDetails.ExtraDate = Convert.ToDateTime(DateTime.Today);
                            if (dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value != null)
                            {
                                if (dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() == "0" || dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() == string.Empty)
                                {
                                    spPurchaseDetails.PurchaseDetailsAdd(infoPurchaseDetails);
                                }
                                else
                                {
                                    infoPurchaseDetails.PurchaseDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString());
                                    spPurchaseDetails.PurchaseDetailsEdit(infoPurchaseDetails);
                                }
                            }
                            else
                            {
                                spPurchaseDetails.PurchaseDetailsAdd(infoPurchaseDetails);
                            }
                            infoStockPosting.BatchId = infoPurchaseDetails.BatchId;
                            infoStockPosting.Date = infoPurchaseMaster.Date;
                            infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                            infoStockPosting.GodownId = infoPurchaseDetails.GodownId;
                            infoStockPosting.InwardQty = infoPurchaseDetails.Qty; /// spUnitConvertion.UnitConversionRateByUnitConversionId(infoPurchaseDetails.UnitConversionId);
                            infoStockPosting.OutwardQty = 0;
                            infoStockPosting.ProductId = infoPurchaseDetails.ProductId;
                            infoStockPosting.RackId = infoPurchaseDetails.RackId;
                            infoStockPosting.Rate = infoPurchaseDetails.Rate;
                            infoStockPosting.UnitId = infoPurchaseDetails.UnitId;
                            if (infoPurchaseDetails.OrderDetailsId != 0)
                            {
                                infoStockPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = "NA";
                                infoStockPosting.AgainstVoucherNo = "NA";
                                infoStockPosting.AgainstVoucherTypeId = 0;
                            }
                            else if (infoPurchaseDetails.ReceiptDetailsId != 0)
                            {
                                infoStockPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = "NA";
                                infoStockPosting.AgainstVoucherNo = "NA";
                                infoStockPosting.AgainstVoucherTypeId = 0;
                            }
                            else if (infoPurchaseDetails.OrderDetailsId == 0 && infoPurchaseDetails.ReceiptDetailsId == 0)
                            {
                                infoStockPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = "NA";
                                infoStockPosting.AgainstVoucherNo = "NA";
                                infoStockPosting.AgainstVoucherTypeId = 0;
                            }
                            infoStockPosting.Extra1 = string.Empty;
                            infoStockPosting.Extra2 = string.Empty;
                            infoStockPosting.ExtraDate = Convert.ToDateTime(DateTime.Today);
                            spStockPosting.StockPostingAdd(infoStockPosting);
                            if (infoPurchaseDetails.ReceiptDetailsId != 0)
                            {
                                infoStockPosting.InvoiceNo = infoMaterialReceiptMaster.InvoiceNo;
                                infoStockPosting.VoucherNo = infoMaterialReceiptMaster.VoucherNo;
                                infoStockPosting.VoucherTypeId = infoMaterialReceiptMaster.VoucherTypeId;
                                infoStockPosting.AgainstInvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoStockPosting.AgainstVoucherNo = infoPurchaseMaster.VoucherNo;
                                infoStockPosting.AgainstVoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoStockPosting.InwardQty = 0;
                                infoStockPosting.OutwardQty = infoPurchaseDetails.Qty; /// spUnitConvertion.UnitConversionRateByUnitConversionId(infoPurchaseDetails.UnitConversionId);
                                spStockPosting.StockPostingAdd(infoStockPosting);
                            }
                        }
                    }
                }
                /*-----------------------------------------Ledger Posting----------------------------------------------------*/
                infoLedgerPosting.Credit = Convert.ToDecimal(txtGrandTotal.Text) * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                infoLedgerPosting.Debit = 0;
                infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                infoLedgerPosting.LedgerId = infoPurchaseMaster.LedgerId;
                infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                infoLedgerPosting.ExtraDate = DateTime.Now;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                decimal DecBillDiscount = Convert.ToDecimal(txtBillDiscount.Text.Trim().ToString());
                if (DecBillDiscount > 0)
                {
                    infoLedgerPosting.Credit = DecBillDiscount * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                    infoLedgerPosting.Debit = 0;
                    infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                    infoLedgerPosting.DetailsId = 0;
                    infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                    infoLedgerPosting.LedgerId = 9;//ledger id of discount received ledger
                    infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                    infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                    infoLedgerPosting.ChequeDate = DateTime.Now;
                    infoLedgerPosting.ChequeNo = string.Empty;
                    infoLedgerPosting.Extra1 = string.Empty;
                    infoLedgerPosting.Extra2 = string.Empty;
                    infoLedgerPosting.ExtraDate = DateTime.Now;
                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                }
                infoLedgerPosting.Credit = 0;
                infoLedgerPosting.Debit = TotalNetAmount();// * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                infoLedgerPosting.DetailsId = 0;
                infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                infoLedgerPosting.LedgerId = infoPurchaseMaster.PurchaseAccount;
                infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                infoLedgerPosting.ChequeDate = DateTime.Now;
                infoLedgerPosting.ChequeNo = string.Empty;
                infoLedgerPosting.Extra1 = string.Empty;
                infoLedgerPosting.Extra2 = string.Empty;
                infoLedgerPosting.ExtraDate = DateTime.Now;
                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                foreach (DataGridViewRow dgvrow in dgvAdditionalCost.Rows)
                {
                    if (dgvrow.Cells["dgvcmbLedger"].Value != null)
                    {
                        if (dgvrow.Cells["dgvcmbLedger"].Value.ToString() != string.Empty)
                        {
                            if (dgvrow.Cells["dgvtxtAdditionalCostAmount"].Value != null)
                            {
                                if (dgvrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString() != string.Empty)
                                {
                                    /*-----------------------------------------Additional Cost Add----------------------------------------------------*/
                                    infoAdditionalCost.Credit = 0;
                                    infoAdditionalCost.Debit = Convert.ToDecimal(dgvrow.Cells["dgvtxtAdditionalCostAmount"].Value.ToString());
                                    infoAdditionalCost.LedgerId = Convert.ToDecimal(dgvrow.Cells["dgvcmbLedger"].Value.ToString());
                                    infoAdditionalCost.VoucherNo = infoPurchaseMaster.VoucherNo;
                                    infoAdditionalCost.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                    infoAdditionalCost.Extra1 = string.Empty;
                                    infoAdditionalCost.Extra2 = string.Empty;
                                    infoAdditionalCost.ExtraDate = DateTime.Now;
                                    if (dgvrow.Cells["dgvtxtAdditionalCostId"].Value != null)
                                    {
                                        if (dgvrow.Cells["dgvtxtAdditionalCostId"].Value.ToString() != string.Empty && dgvrow.Cells["dgvtxtAdditionalCostId"].Value.ToString() != string.Empty)
                                        {
                                            infoAdditionalCost.AdditionalCostId = Convert.ToDecimal(dgvrow.Cells["dgvtxtAdditionalCostId"].Value.ToString());
                                            spAdditionalCost.AdditionalCostEdit(infoAdditionalCost);
                                        }
                                        else
                                        {
                                            spAdditionalCost.AdditionalCostAdd(infoAdditionalCost);
                                        }
                                    }
                                    else
                                    {
                                        spAdditionalCost.AdditionalCostAdd(infoAdditionalCost);
                                    }
                                    /*-----------------------------------------Additional Cost Ledger Posting----------------------------------------------------*/
                                    infoLedgerPosting.Credit = 0;
                                    infoLedgerPosting.Debit = infoAdditionalCost.Debit * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                                    infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                                    infoLedgerPosting.DetailsId = 0;
                                    infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                    infoLedgerPosting.LedgerId = infoAdditionalCost.LedgerId;
                                    infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                    infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                    infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                                    infoLedgerPosting.ChequeDate = DateTime.Now;
                                    infoLedgerPosting.ChequeNo = string.Empty;
                                    infoLedgerPosting.Extra1 = string.Empty;
                                    infoLedgerPosting.Extra2 = string.Empty;
                                    infoLedgerPosting.ExtraDate = DateTime.Now;
                                    spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                                }
                            }
                        }
                    }
                }
                if (dgvTax.Visible)
                {
                    foreach (DataGridViewRow dgvrow in dgvTax.Rows)
                    {
                        if (dgvrow.Cells["dgvtxtTaxId"].Value != null)
                        {
                            if (dgvrow.Cells["dgvtxtTaxId"].Value.ToString() != string.Empty)
                            {
                                /*-----------------------------------------PurchaseBillTax Add----------------------------------------------------*/
                                infoPurchaseBillTax.PurchaseMasterId = decPurchaseMasterId;
                                infoPurchaseBillTax.TaxAmount = Convert.ToDecimal(dgvrow.Cells["dgvtxtTotalTax"].Value.ToString());
                                infoPurchaseBillTax.TaxId = Convert.ToDecimal(dgvrow.Cells["dgvtxtTaxId"].Value.ToString());
                                infoPurchaseBillTax.Extra1 = string.Empty;
                                infoPurchaseBillTax.Extra2 = string.Empty;
                                infoPurchaseBillTax.ExtraDate = DateTime.Now;
                                if (dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value != null)
                                {
                                    if (dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value.ToString() != string.Empty && dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value.ToString() != "0")
                                    {
                                        infoPurchaseBillTax.PurchaseBillTaxId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value.ToString());
                                        spPurchaseBillTax.PurchaseBillTaxEdit(infoPurchaseBillTax);
                                    }
                                    else
                                    {
                                        spPurchaseBillTax.PurchaseBillTaxAdd(infoPurchaseBillTax);
                                    }
                                }
                                else
                                {
                                    spPurchaseBillTax.PurchaseBillTaxAdd(infoPurchaseBillTax);
                                }
                                /*-----------------------------------------Tax Ledger Posting----------------------------------------------------*/
                                infoLedgerPosting.Credit = 0;
                                infoLedgerPosting.Debit = infoPurchaseBillTax.TaxAmount * spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                                infoLedgerPosting.Date = Convert.ToDateTime(PublicVariables._dtCurrentDate);
                                infoLedgerPosting.DetailsId = 0;
                                infoLedgerPosting.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                                infoLedgerPosting.LedgerId = Convert.ToDecimal(dgvrow.Cells["dgvtxtLedgerId"].Value.ToString());
                                infoLedgerPosting.VoucherNo = infoPurchaseMaster.VoucherNo;
                                infoLedgerPosting.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                                infoLedgerPosting.YearId = PublicVariables._decCurrentFinancialYearId;
                                infoLedgerPosting.ChequeDate = DateTime.Now;
                                infoLedgerPosting.ChequeNo = string.Empty;
                                infoLedgerPosting.Extra1 = string.Empty;
                                infoLedgerPosting.Extra2 = string.Empty;
                                infoLedgerPosting.ExtraDate = DateTime.Now;
                                spLedgerPosting.LedgerPostingAdd(infoLedgerPosting);
                            }
                        }
                    }
                }
                /*-----------------------------------------PartyBalance Posting----------------------------------------------------*/
                infoAccountLedger = spAccountLedger.AccountLedgerView(infoPurchaseMaster.LedgerId);
                if (infoAccountLedger.BillByBill == true)
                {
                    infoPartyBalance.Credit = Convert.ToDecimal(txtGrandTotal.Text);
                    infoPartyBalance.Debit = 0;
                    if (txtCreditPeriod.Text != string.Empty)
                    {
                        infoPartyBalance.CreditPeriod = Convert.ToInt32(txtCreditPeriod.Text);
                    }
                    infoPartyBalance.Date = Convert.ToDateTime(txtVoucherDate.Text);
                    infoPartyBalance.ExchangeRateId = infoPurchaseMaster.ExchangeRateId;
                    infoPartyBalance.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                    infoPartyBalance.LedgerId = infoPurchaseMaster.LedgerId;
                    infoPartyBalance.ReferenceType = "New";
                    infoPartyBalance.InvoiceNo = infoPurchaseMaster.InvoiceNo;
                    infoPartyBalance.VoucherNo = infoPurchaseMaster.VoucherNo;
                    infoPartyBalance.VoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                    infoPartyBalance.AgainstInvoiceNo = "NA";
                    infoPartyBalance.AgainstVoucherNo = "NA";
                    infoPartyBalance.AgainstVoucherTypeId = 0;
                    infoPartyBalance.Extra1 = string.Empty;
                    infoPartyBalance.Extra2 = string.Empty;
                    infoPartyBalance.ExtraDate = DateTime.Now;
                    spPartyBalance.PartyBalanceAdd(infoPartyBalance);
                }
                Messages.UpdatedMessage();
                if (cbxPrintAfterSave.Checked)
                {
                    if (spSettings.SettingsStatusCheck("Printer") == "Dot Matrix")
                    {
                        PrintForDotMatrix(decPurchaseMasterId);
                    }
                    else
                    {
                        Print(decPurchaseMasterId);
                    }
                }
                Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI39:" + ex.Message;
            }
        }
        /// <summary>
        /// deleting the rows from tbl_PurchaseDetails removed by user from the grid while updating after reference check
        /// </summary>
        public void RemoveDelete()
        {
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            PurchaseDetailsSP spPurchaseDetails = new PurchaseDetailsSP();
            AdditionalCostSP spAdditionalCost = new AdditionalCostSP();
            decimal decPurchaseDetailsId = 0;
            decimal decAdditionalCostId = 0;
            int inRef = 0;
            try
            {
                foreach (var item in arrlstRemove)
                {
                    decPurchaseDetailsId = Convert.ToDecimal(item);
                    inRef = spPurchaseMaster.PurchaseMasterReferenceCheck(decPurchaseMasterId, decPurchaseDetailsId);
                    if (inRef == 0)
                    {
                        spPurchaseDetails.PurchaseDetailsDelete(decPurchaseDetailsId);
                    }
                }
                foreach (var item1 in arrlstRemoveAdditionalCost)
                {
                    decAdditionalCostId = Convert.ToDecimal(item1);
                    spAdditionalCost.AdditionalCostDelete(decAdditionalCostId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI40:" + ex.Message;
            }
        }
        public void Delete()
        {
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            PurchaseDetailsSP spPurchaseDetails = new PurchaseDetailsSP();
            PurchaseBillTaxSP spPurchaseBillTax = new PurchaseBillTaxSP();
            AdditionalCostSP spAdditionalCost = new AdditionalCostSP();
            AccountLedgerSP spAccountLedger = new AccountLedgerSP();
            StockPostingSP spStockPosting = new StockPostingSP();
            LedgerPostingSP spLedgerPosting = new LedgerPostingSP();
            PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
            MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
            PurchaseOrderMasterInfo infoPurchaseOrderMaster = new PurchaseOrderMasterInfo();
            MaterialReceiptMasterInfo infoMaterialReceiptMaster = new MaterialReceiptMasterInfo();
            PurchaseMasterInfo infoPurchaseMaster = new PurchaseMasterInfo();
            /*---------------------------------Deleting previous stock posting, Ledger posting, partybalanceposting---------------------------------------*/
            infoPurchaseMaster = spPurchaseMaster.PurchaseMasterView(decPurchaseMasterId);
            if (infoPurchaseMaster.MaterialReceiptMasterId != 0)
            {
                infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(infoPurchaseMaster.MaterialReceiptMasterId);
                spStockPosting.StockPostingDeleteForSalesInvoiceAgainstDeliveryNote
                    (infoPurchaseMaster.VoucherTypeId, infoPurchaseMaster.VoucherNo,
                    infoMaterialReceiptMaster.VoucherNo, infoMaterialReceiptMaster.VoucherTypeId);
            }
            spStockPosting.StockPostingDeleteByagainstVoucherTypeIdAndagainstVoucherNoAndVoucherNoAndVoucherType
                    (0, "NA", infoPurchaseMaster.VoucherNo, infoPurchaseMaster.VoucherTypeId);
            //-------------------------------------------------
            decimal decPurchaseDetailsId = 0;
            decimal decPurchaseOrderMasterId = 0;
            decimal decMaterialReceiptMasterId = 0;
            decimal decAdditionalCostId = 0;
            decimal decPurchaseBillTaxId = 0;
            int inRef = 0;
            bool isRef = false;
            try
            {
                foreach (DataGridViewRow dgvrow in dgvProductDetails.Rows)
                {
                    if (dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() != string.Empty && dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() != "0")
                        {
                            decPurchaseDetailsId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString());
                            inRef = spPurchaseMaster.PurchaseMasterReferenceCheck(decPurchaseMasterId, decPurchaseDetailsId);
                            if (inRef > 0 && !isRef)
                            {
                                isRef = true;
                            }
                        }
                    }
                }
                if (!isRef)
                {
                    isRef = spAccountLedger.PartyBalanceAgainstReferenceCheck(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                    if (!isRef)
                    {
                        spPurchaseMaster.PurchaseMasterDelete(decPurchaseMasterId);
                        spPurchaseDetails.PurchaseDetailsDeleteByPurchaseMasterId(decPurchaseMasterId);
                        foreach (DataGridViewRow dgvrow in dgvAdditionalCost.Rows)
                        {
                            if (dgvrow.Cells["dgvtxtAdditionalCostId"].Value != null)
                            {
                                if (dgvrow.Cells["dgvtxtAdditionalCostId"].Value.ToString() != string.Empty &&
                                    dgvrow.Cells["dgvtxtAdditionalCostId"].Value.ToString() != "0")
                                {
                                    decAdditionalCostId = Convert.ToDecimal(dgvrow.Cells["dgvtxtAdditionalCostId"].Value.ToString());
                                    spAdditionalCost.AdditionalCostDelete(decAdditionalCostId);
                                }
                            }
                        }
                        if (dgvTax.Visible)
                        {
                            foreach (DataGridViewRow dgvrow in dgvTax.Rows)
                            {
                                if (dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value != null)
                                {
                                    if (dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value.ToString() != string.Empty &&
                                        dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value.ToString() != "0")
                                    {
                                        decPurchaseBillTaxId = Convert.ToDecimal(dgvrow.Cells["dgvtxtPurchaseBillTaxId"].Value.ToString());
                                        spPurchaseBillTax.PurchaseBillTaxDelete(decPurchaseBillTaxId);
                                    }
                                }
                            }
                        }
                        if (cmbPurchaseMode.Text == "Against Purchase Order")
                        {
                            decPurchaseOrderMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                            infoPurchaseOrderMaster = spPurchaseOrderMaster.PurchaseOrderMasterView(decPurchaseOrderMasterId);
                        }
                        else if (cmbPurchaseMode.Text == "Against Material Receipt")
                        {
                            decMaterialReceiptMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                            infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(decMaterialReceiptMasterId);
                        }
                        spLedgerPosting.LedgerPostDelete(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                        if (infoPurchaseOrderMaster.PurchaseOrderMasterId != 0)
                        {
                            spStockPosting.StockPostingDeleteByagainstVoucherTypeIdAndagainstVoucherNoAndVoucherNoAndVoucherType
                                (decPurchaseInvoiceVoucherTypeId, strVoucherNo,
                                infoPurchaseOrderMaster.VoucherNo, infoPurchaseOrderMaster.VoucherTypeId);
                        }
                        else if (infoMaterialReceiptMaster.MaterialReceiptMasterId != 0)
                        {
                            spStockPosting.StockPostingDeleteByagainstVoucherTypeIdAndagainstVoucherNoAndVoucherNoAndVoucherType
                                (decPurchaseInvoiceVoucherTypeId, strVoucherNo,
                                infoMaterialReceiptMaster.VoucherNo, infoMaterialReceiptMaster.VoucherTypeId);
                        }
                        spStockPosting.StockPostingDeleteByagainstVoucherTypeIdAndagainstVoucherNoAndVoucherNoAndVoucherType
                                (0, "NA", strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                        spAccountLedger.PartyBalanceDeleteByVoucherTypeVoucherNoAndReferenceType(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                        Messages.DeletedMessage();
                        Clear();
                        this.Close();
                    }
                    else
                    {
                        Messages.InformationMessage("Cannot delete purchase invoice because there is a payment voucher against this invoice");
                    }
                }
                else
                {
                    Messages.InformationMessage("Cannot delete purchase invoice because reference exists");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI41:" + ex.Message;
            }
        }
        //..................PrintAfetrSave Status.........................//
        /// <summary>
        /// Function to check PrintAfetrSave Status
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
                formMDI.infoError.ErrorString = "PI42:" + ex.Message;
            }
            return isTick;
        }
        /// <summary>
        /// Function to check settings status
        /// </summary>
        public void SettingsStatusCheck()
        {
            SettingsSP spSettings = new SettingsSP();
            try
            {
                if (spSettings.SettingsStatusCheck("AllowGodown") == "Yes")
                {
                    dgvProductDetails.Columns["dgvcmbGodown"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvcmbGodown"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("AllowRack") == "Yes")
                {
                    dgvProductDetails.Columns["dgvcmbRack"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvcmbRack"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("ShowUnit") == "Yes")
                {
                    dgvProductDetails.Columns["dgvcmbUnit"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvcmbUnit"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("ShowDiscountAmount") == "Yes")
                {
                    dgvProductDetails.Columns["dgvtxtDiscount"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvtxtDiscount"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("ShowProductCode") == "Yes")
                {
                    dgvProductDetails.Columns["dgvtxtProductCode"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvtxtProductCode"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("Barcode") == "Yes")
                {
                    dgvProductDetails.Columns["dgvtxtBarcode"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvtxtBarcode"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("ShowDiscountPercentage") == "Yes")
                {
                    dgvProductDetails.Columns["dgvtxtDiscountPercent"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvtxtDiscountPercent"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("AllowBatch") == "Yes")
                {
                    dgvProductDetails.Columns["dgvcmbBatch"].Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvcmbBatch"].Visible = false;
                }
                if (spSettings.SettingsStatusCheck("MultiCurrency") == "Yes")
                {
                    cmbCurrency.Enabled = true;
                }
                else
                {
                    cmbCurrency.Enabled = false;
                }
                if (spSettings.SettingsStatusCheck("Tax") == "Yes")
                {
                    dgvProductDetails.Columns["dgvcmbTax"].Visible = true;
                    dgvProductDetails.Columns["dgvtxtTaxAmount"].Visible = true;
                    dgvTax.Visible = true;
                    lblTotalTaxAmount.Visible = true;
                    lblTaxAmount.Visible = true;
                }
                else
                {
                    dgvProductDetails.Columns["dgvcmbTax"].Visible = false;
                    dgvProductDetails.Columns["dgvtxtTaxAmount"].Visible = false;
                    dgvTax.Visible = false;
                    lblTotalTaxAmount.Visible = false;
                    lblTaxAmount.Visible = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI43:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the details
        /// </summary>
        public void ClearDetails()
        {
            try
            {
                txtNarration.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
                dgvProductDetails.AllowUserToAddRows = true;
                if (dgvProductDetails.DataSource != null)
                {
                    ((DataTable)dgvProductDetails.DataSource).Rows.Clear();
                }
                else
                {
                    dgvProductDetails.Rows.Clear();
                }
                GridComboFill();
                if (dgvTax.DataSource != null)
                {
                    ((DataTable)dgvTax.DataSource).Rows.Clear();
                }
                else
                {
                    dgvTax.Rows.Clear();
                }
                TaxGridFill();
                if (dgvAdditionalCost.DataSource != null)
                {
                    ((DataTable)dgvAdditionalCost.DataSource).Rows.Clear();
                }
                else
                {
                    dgvAdditionalCost.Rows.Clear();
                }
                lblTaxAmount.Text = Math.Round(000.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                AdditionalCostGridFill();
                lblAdditionalCostAmount.Text = Math.Round(000.00, PublicVariables._inNoOfDecimalPlaces).ToString();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI44:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to clear the master fields
        /// </summary>
        public void ClearMaster()
        {
            try
            {
                decPurchaseMasterId = 0;
                VoucherNumberGeneration();
                dtpVoucherDate.Value = PublicVariables._dtCurrentDate;
                dtpInvoiceDate.Value = PublicVariables._dtCurrentDate;
                cmbPurchaseMode.SelectedIndex = 0;
                txtVendorInvoiceNo.Text = string.Empty;
                cmbCashOrParty.SelectedIndex = 0;
                txtCreditPeriod.Text = string.Empty;
                cmbCurrency.SelectedValue = 1;
                txtNarration.Text = string.Empty;
                txtTransportationCompany.Text = string.Empty;
                txtLRNo.Text = string.Empty;
                lblAdditionalCostAmount.Text = Math.Round(0.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                lblTaxAmount.Text = Math.Round(0.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                txtTotalAmount.Text = Math.Round(0.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                txtBillDiscount.Text = Math.Round(0.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                if (!isAutomatic)
                {
                    txtVoucherNo.Clear();
                    txtVoucherNo.Focus();
                }
                if (PrintAfetrSave())
                {
                    cbxPrintAfterSave.Checked = true;
                }
                else
                {
                    cbxPrintAfterSave.Checked = false;
                }
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI45:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Clearmaster and ClearDetails
        /// </summary>
        public void Clear()
        {
            try
            {
                ClearMaster();
                ClearDetails();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI46:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmLedgerPopup form to select and view Ledger
        /// </summary>
        /// <param name="frmLedgerPopup"></param>
        /// <param name="decId"></param>
        /// <param name="strComboTypes"></param>
        public void CallFromLedgerPopup(frmLedgerPopup frmLedgerPopup, decimal decId, string strComboTypes) //PopUp
        {
            try
            {
                base.Show();
                this.frmLedgerPopupObj = frmLedgerPopup;
                if (strComboTypes == "CashOrSundryCreditors")
                {
                    TransactionGeneralFillObj.CashOrPartyComboFill(cmbCashOrParty, false);
                    cmbCashOrParty.SelectedValue = decId;
                }
                else if (strComboTypes == "PurchaseAccount")
                {
                    PurchaseAccountComboFill();
                    cmbPurchaseAccount.SelectedValue = decId;
                }
                frmLedgerPopupObj.Close();
                frmLedgerPopupObj = null;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI47:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call frmProductSearch form to select and view product
        /// </summary>
        /// <param name="frmProductSearchPopup"></param>
        /// <param name="decproductId"></param>
        /// <param name="decCurrentRowIndex"></param>
        public void CallFromProductSearchPopup(frmProductSearchPopup frmProductSearchPopup, decimal decproductId, decimal decCurrentRowIndex)
        {
            ProductSP spProduct = new ProductSP();
            ProductInfo infoProduct = new ProductInfo();
            try
            {
                base.Show();
                this.frmProductSearchPopupObj = frmProductSearchPopup;
                if (decproductId != 0)
                {
                    int inCurrentRowIndex = dgvProductDetails.CurrentRow.Index;
                    dgvProductDetails.Rows.Add();
                    infoProduct = spProduct.ProductView(decproductId);
                    strProductCode = infoProduct.ProductCode;
                    ProductDetailsFill(strProductCode, inCurrentRowIndex, "ProductCode");
                    SerialNo();
                    frmProductSearchPopupObj.Close();
                    frmProductSearchPopupObj = null;

                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI48:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the product details
        /// </summary>
        /// <param name="strProduct"></param>
        /// <param name="inRowIndex"></param>
        /// <param name="strFillMode"></param>
        public void ProductDetailsFill(string strProduct, int inRowIndex, string strFillMode)
        {
            decimal decProductId = 0;
            decimal decGodownId = 0;
            PurchaseDetailsSP spPurchaseDetails = new PurchaseDetailsSP();
            DataTable dtbl = new DataTable();
            try
            {
                if (strFillMode == "ProductCode")
                {
                    dtbl = spPurchaseDetails.PurchaseDetailsViewByProductCodeForPI(decPurchaseInvoiceVoucherTypeId, strProduct);
                }
                else if (strFillMode == "ProductName")
                {
                    dtbl = spPurchaseDetails.PurchaseDetailsViewByProductNameForPI(decPurchaseInvoiceVoucherTypeId, strProduct);
                }
                else if (strFillMode == "Barcode")
                {
                    dtbl = spPurchaseDetails.PurchaseDetailsViewByBarcodeForPI(decPurchaseInvoiceVoucherTypeId, strProduct);
                }
                if (dtbl.Rows.Count >= 1)
                {
                    decProductId = Convert.ToDecimal(dtbl.Rows[0]["productId"]);
                    decGodownId = Convert.ToDecimal(dtbl.Rows[0]["godownId"]);
                    UnitComboFill(decProductId, inRowIndex, dgvProductDetails.Columns["dgvcmbUnit"].Index);
                    GodownComboFill();
                    RackComboFill(decGodownId, inRowIndex, dgvProductDetails.Columns["dgvcmbRack"].Index);
                    BatchComboFill(decProductId, inRowIndex, dgvProductDetails.Columns["dgvcmbBatch"].Index);
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtPurchaseDetailsId"].Value = dtbl.Rows[0]["purchaseDetailsId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtPurchaseOrderDetailsId"].Value = dtbl.Rows[0]["purchaseOrderDetailsId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value = dtbl.Rows[0]["materialReceiptDetailsId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductId"].Value = dtbl.Rows[0]["productId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtBarcode"].Value = dtbl.Rows[0]["barcode"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductCode"].Value = dtbl.Rows[0]["productCode"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductName"].Value = dtbl.Rows[0]["productName"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtUnitConversionId"].Value = dtbl.Rows[0]["unitConversionId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbUnit"].Value = dtbl.Rows[0]["unitId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbGodown"].Value = dtbl.Rows[0]["godownId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbRack"].Value = dtbl.Rows[0]["rackId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbBatch"].Value = dtbl.Rows[0]["batchId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtRate"].Value = dtbl.Rows[0]["rate"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtGrossValue"].Value = dtbl.Rows[0]["grossValue"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value = dtbl.Rows[0]["discountPercent"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value = dtbl.Rows[0]["discount"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtNetValue"].Value = dtbl.Rows[0]["netvalue"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbTax"].Value = dtbl.Rows[0]["taxId"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtTaxAmount"].Value = dtbl.Rows[0]["taxAmount"];
                    dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtAmount"].Value = dtbl.Rows[0]["amount"];
                    dgvProductDetails.Rows[inRowIndex].HeaderCell.Value = "X";
                    dgvProductDetails.Rows[inRowIndex].HeaderCell.Style.ForeColor = Color.Red;
                }
                else
                {
                    if (strProductCode != string.Empty)
                    {
                        ProductDetailsFill(strProductCode, inRowIndex, "ProductCode");
                    }
                    else
                    {
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtPurchaseDetailsId"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtPurchaseOrderDetailsId"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductId"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtBarcode"].Value = string.Empty;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductCode"].Value = string.Empty;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductName"].Value = string.Empty;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtQuantity"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtUnitConversionId"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbUnit"].Value = Convert.ToDecimal("0");
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbGodown"].Value = Convert.ToDecimal("0");
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbRack"].Value = Convert.ToDecimal("0");
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbBatch"].Value = Convert.ToDecimal("0");
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtRate"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtGrossValue"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtNetValue"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbTax"].Value = Convert.ToDecimal("0");
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtTaxAmount"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtAmount"].Value = 0;
                        dgvProductDetails.Rows[inRowIndex].HeaderCell.Value = "X";
                        dgvProductDetails.Rows[inRowIndex].HeaderCell.Style.ForeColor = Color.Red;

                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI49:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculete total net amount
        /// </summary>
        /// <returns></returns>
        public decimal TotalNetAmount()
        {
            decimal decNetAmount = 0;
            decimal decRate = 0;
            decimal decDefaultNetAmount = 0;
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            try
            {
                foreach (DataGridViewRow dgvrow in dgvProductDetails.Rows)
                {
                    if (dgvrow.Cells["dgvtxtProductId"].Value != null)
                    {
                        if (dgvrow.Cells["dgvtxtProductId"].Value.ToString() != string.Empty)
                        {
                            decNetAmount = decNetAmount + Convert.ToDecimal(dgvrow.Cells["dgvtxtNetValue"].Value.ToString());
                        }
                    }
                }
                decRate = spExchangeRate.ExchangeRateViewByExchangeRateId(Convert.ToDecimal(cmbCurrency.SelectedValue.ToString()));
                decDefaultNetAmount = decNetAmount * decRate;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI50:" + ex.Message;
            }
            return decDefaultNetAmount;
        }
        /// <summary>
        /// Function for gridview read only settings
        /// </summary>
        /// <param name="strPurchaseMode"></param>
        public void GridviewReadOnlySettings(string strPurchaseMode)
        {
            try
            {
                int inI = 0;
                foreach (DataGridViewRow dgvRow in dgvProductDetails.Rows)
                {
                    if (dgvRow.Cells["dgvtxtProductId"].Value != null)
                    {
                        dgvRow.Cells["dgvtxtProductCode"].ReadOnly = true;
                        dgvRow.Cells["dgvtxtProductName"].ReadOnly = true;
                        dgvRow.Cells["dgvtxtBarcode"].ReadOnly = true;
                        if (strPurchaseMode == "Against Material Receipt" || strPurchaseMode == "Against Purchase Order")
                        {
                            dgvRow.Cells["dgvcmbUnit"].ReadOnly = true;
                            dgvRow.Cells["dgvcmbGodown"].ReadOnly = true;
                            dgvRow.Cells["dgvcmbRack"].ReadOnly = true;
                            dgvRow.Cells["dgvcmbBatch"].ReadOnly = true;
                        }
                    }
                    Calculate(inI);
                    inI++;
                }
                CalculateTotalAmount();
                if (dgvTax.Visible)
                {
                    TotalTaxAmount();
                }
                CalculateGrandTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI51:" + ex.Message;
            }
        }
        /// <summary>
        /// Remove incomplete row from grid
        /// </summary>
        /// <returns></returns>
        public bool RemoveIncompleteRowsFromGrid()
        {
            bool isOk = true;
            try
            {
                string strMessage = "Rows";
                int inC = 0, inForFirst = 0;
                int inRowcount = dgvProductDetails.RowCount;
                int inLastRow = 1;//To eliminate last row from checking
                foreach (DataGridViewRow dgvrow in dgvProductDetails.Rows)
                {
                    if (inLastRow <= inRowcount)
                    {
                        if (dgvrow.HeaderCell.Value != null)
                        {
                            if (dgvrow.HeaderCell.Value.ToString() == "X" )//|| dgvrow.Cells["dgvtxtProductId"].Value == null)
                            {
                                isOk = false;
                                if (inC == 0)
                                {
                                    strMessage = strMessage + Convert.ToString(dgvrow.Index + 1);
                                    inForFirst = dgvrow.Index;
                                    inC++;
                                }
                                else
                                {
                                    strMessage = strMessage + ", " + Convert.ToString(dgvrow.Index + 1);
                                }
                            }
                        }
                    }
                    inLastRow++;
                }
                inLastRow = 1;
                if (!isOk)
                {
                    strMessage = strMessage + " contains invalid entries. Do you want to continue?";
                    if (MessageBox.Show(strMessage, "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        isOk = true;
                        for (int inK = 0; inK < dgvProductDetails.Rows.Count; inK++)
                        {
                            if (dgvProductDetails.Rows[inK].HeaderCell.Value != null && dgvProductDetails.Rows[inK].HeaderCell.Value.ToString() == "X")
                            {
                                if (!dgvProductDetails.Rows[inK].IsNewRow)
                                {
                                    dgvProductDetails.Rows.RemoveAt(inK);
                                    inK--;
                                }
                            }
                        }
                    }
                    else
                    {
                        dgvProductDetails.Rows[inForFirst].Cells["dgvtxtProductName"].Selected = true;
                        dgvProductDetails.CurrentCell = dgvProductDetails.Rows[inForFirst].Cells["dgvtxtProductName"];
                        dgvProductDetails.Focus();
                    }
                }
                SerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI52:" + ex.Message;
            }
            return isOk;
        }
        /// <summary>
        /// Function to call this form from frmPurchaseInvoiceRegister to view details and for updation
        /// </summary>
        /// <param name="frmPurchaseInvoiceRegister"></param>
        /// <param name="decPurchaseInvoiceMasterId"></param>
        public void CallFromPurchaseInvoiceRegister(frmPurchaseInvoiceRegister frmPurchaseInvoiceRegister, decimal decPurchaseInvoiceMasterId)
        {
            try
            {
                base.Show();
                frmPurchaseInvoiceRegister.Enabled = false;
                this.frmPurchaseInvoiceRegisterObj = frmPurchaseInvoiceRegister;
                decPurchaseMasterId = decPurchaseInvoiceMasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI53:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmPurchaseInvoicereport to view details and for updation
        /// </summary>
        /// <param name="frmPurchaseReport"></param>
        /// <param name="decPurchaseInvoiceMasterId"></param>
        public void CallFromPurchaseReport(frmPurchaseReport frmPurchaseReport, decimal decPurchaseInvoiceMasterId)
        {
            try
            {
                base.Show();
                frmPurchaseReport.Enabled = false;
                this.frmPurchaseReportObj = frmPurchaseReport;
                decPurchaseMasterId = decPurchaseInvoiceMasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI54:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the details while calling from register or report
        /// </summary>
        public void FillRegisterOrReport()
        {
            PurchaseMasterInfo infoPurchaseMaster = new PurchaseMasterInfo();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            PurchaseOrderMasterInfo infoPurchaseOrderMaster = new PurchaseOrderMasterInfo();
            PurchaseOrderMasterSP spPurchaseOrderMaster = new PurchaseOrderMasterSP();
            MaterialReceiptMasterInfo infoMaterialReceiptMaster = new MaterialReceiptMasterInfo();
            MaterialReceiptMasterSP spMaterialReceiptMaster = new MaterialReceiptMasterSP();
            VoucherTypeSP spVoucherType = new VoucherTypeSP();
            VoucherTypeInfo infoVoucherType = new VoucherTypeInfo();
            AccountLedgerSP spAccountLedger = new AccountLedgerSP();
            bool isPartyBalanceRef = false;
            try
            {
                isEditFill = true;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtVoucherNo.ReadOnly = true;
                infoPurchaseMaster = spPurchaseMaster.PurchaseMasterView(decPurchaseMasterId);
                strVoucherNo = infoPurchaseMaster.VoucherNo;
                decPurchaseInvoiceVoucherTypeId = infoPurchaseMaster.VoucherTypeId;
                decPurchaseInvoiceSuffixPrefixId = infoPurchaseMaster.SuffixPrefixId;
                isAutomatic = spVoucherType.CheckMethodOfVoucherNumbering(decPurchaseInvoiceVoucherTypeId);
                infoVoucherType = spVoucherType.VoucherTypeView(decPurchaseInvoiceVoucherTypeId);
                this.Text = infoVoucherType.VoucherTypeName;
                if (isAutomatic)
                {
                    txtVoucherDate.Focus();
                }
                else
                {
                    txtVoucherNo.Focus();
                }
                txtVoucherNo.Text = infoPurchaseMaster.InvoiceNo;
                txtVendorInvoiceNo.Text = infoPurchaseMaster.VendorInvoiceNo;
                dtpVoucherDate.Value = infoPurchaseMaster.Date;
                dtpInvoiceDate.Value = infoPurchaseMaster.VendorInvoiceDate;
                cmbCashOrParty.SelectedValue = infoPurchaseMaster.LedgerId;
                if (infoPurchaseMaster.PurchaseOrderMasterId == 0 && infoPurchaseMaster.MaterialReceiptMasterId == 0)
                {
                    cmbPurchaseMode.SelectedItem = "NA";
                }
                else if (infoPurchaseMaster.PurchaseOrderMasterId != 0 && infoPurchaseMaster.MaterialReceiptMasterId == 0)
                {
                    cmbPurchaseMode.SelectedItem = "Against Purchase Order";
                    infoPurchaseOrderMaster = spPurchaseOrderMaster.PurchaseOrderMasterView(infoPurchaseMaster.PurchaseOrderMasterId);
                    cmbVoucherType.SelectedValue = infoPurchaseOrderMaster.VoucherTypeId;
                    OrderComboFill();
                    cmbOrderNo.SelectedValue = infoPurchaseMaster.PurchaseOrderMasterId;
                }
                else if (infoPurchaseMaster.PurchaseOrderMasterId == 0 && infoPurchaseMaster.MaterialReceiptMasterId != 0)
                {
                    cmbPurchaseMode.SelectedItem = "Against Material Receipt";
                    infoMaterialReceiptMaster = spMaterialReceiptMaster.MaterialReceiptMasterView(infoPurchaseMaster.MaterialReceiptMasterId);
                    cmbVoucherType.SelectedValue = infoMaterialReceiptMaster.VoucherTypeId;
                    OrderComboFill();
                    cmbOrderNo.SelectedValue = infoPurchaseMaster.MaterialReceiptMasterId;
                }
                cmbPurchaseAccount.SelectedValue = infoPurchaseMaster.PurchaseAccount;
                txtCreditPeriod.Text = infoPurchaseMaster.CreditPeriod;
                cmbCurrency.SelectedValue = infoPurchaseMaster.ExchangeRateId;
                txtNarration.Text = infoPurchaseMaster.Narration;
                lblAdditionalCostAmount.Text = Math.Round(infoPurchaseMaster.AdditionalCost, PublicVariables._inNoOfDecimalPlaces).ToString();
                lblTaxAmount.Text = Math.Round(infoPurchaseMaster.TotalTax, PublicVariables._inNoOfDecimalPlaces).ToString();
                txtTotalAmount.Text = Math.Round(infoPurchaseMaster.TotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
                txtBillDiscount.Text = Math.Round(infoPurchaseMaster.BillDiscount, PublicVariables._inNoOfDecimalPlaces).ToString();
                txtLRNo.Text = infoPurchaseMaster.LrNo;
                txtTransportationCompany.Text = infoPurchaseMaster.TransportationCompany;
                txtGrandTotal.Text = Math.Round(infoPurchaseMaster.GrandTotal, PublicVariables._inNoOfDecimalPlaces).ToString();
                PurchaseDetailsFill();
                TaxGridFill();
                AdditionalCostGridFill();
                isPartyBalanceRef = spAccountLedger.PartyBalanceAgainstReferenceCheck(strVoucherNo, decPurchaseInvoiceVoucherTypeId);
                if (isPartyBalanceRef)
                {
                    cmbCashOrParty.Enabled = false;
                }
                else
                {
                    cmbCashOrParty.Enabled = true;
                }
                isEditFill = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI55:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill purchase details
        /// </summary>
        public void PurchaseDetailsFill()
        {
            DataTable dtbl = new DataTable();
            PurchaseDetailsSP spPurchaseDetails = new PurchaseDetailsSP();
            PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
            try
            {
                GridComboFill();
                dtbl = spPurchaseDetails.PurchaseDetailsViewByPurchaseMasterId(decPurchaseMasterId);
                int i = 0;

                foreach (DataRow dr in dtbl.Rows)
                {
                    dgvProductDetails.Rows.Add();
                    dgvProductDetails.Rows[i].Cells["dgvtxtPurchaseDetailsId"].Value = dr["purchaseDetailsId"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtPurchaseOrderDetailsId"].Value = dr["purchaseOrderDetailsId"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtMaterialReceiptDetailsId"].Value = dr["materialReceiptDetailsId"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtProductId"].Value = dr["productId"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtBarcode"].Value = dr["barcode"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtProductCode"].Value = dr["productCode"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtProductName"].Value = dr["productName"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtQuantity"].Value = dr["qty"].ToString();
                    UnitComboFill(Convert.ToDecimal(dr["productId"].ToString()), i, dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].ColumnIndex);
                    dgvProductDetails.Rows[i].Cells["dgvtxtUnitConversionId"].Value = dr["unitConversionId"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].Value = Convert.ToDecimal(dr["unitId"].ToString());
                    GodownComboFill();
                    RackAllComboFill(i);
                    dgvProductDetails.Rows[i].Cells["dgvcmbGodown"].Value = Convert.ToDecimal(dr["godownId"].ToString());
                    //RackComboFill(Convert.ToDecimal(dr["godownId"].ToString()), i, dgvProductDetails.Columns["dgvcmbRack"].Index);
                    dgvProductDetails.Rows[i].Cells["dgvcmbRack"].Value = Convert.ToDecimal(dr["rackId"].ToString());
                    BatchComboFill(Convert.ToDecimal(dr["productId"].ToString()), i, dgvProductDetails.Rows[i].Cells["dgvcmbBatch"].ColumnIndex);
                    dgvProductDetails.Rows[i].Cells["dgvcmbBatch"].Value = Convert.ToDecimal(dr["batchId"].ToString());
                    dgvProductDetails.Rows[i].Cells["dgvtxtRate"].Value = dr["rate"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtGrossValue"].Value = dr["grossValue"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtDiscountPercent"].Value = dr["discountPercent"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtDiscount"].Value = dr["discount"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtNetValue"].Value = dr["netvalue"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvcmbTax"].Value = Convert.ToDecimal(dr["taxId"].ToString());
                    dgvProductDetails.Rows[i].Cells["dgvtxtTaxAmount"].Value = dr["taxAmount"].ToString();
                    dgvProductDetails.Rows[i].Cells["dgvtxtAmount"].Value = dr["Amount"].ToString();
                    int inRef = spPurchaseMaster.PurchaseMasterReferenceCheck(decPurchaseMasterId, Convert.ToDecimal(dr["purchaseDetailsId"].ToString()));
                    if (Convert.ToDecimal(dr["purchaseOrderDetailsId"].ToString()) != 0 || Convert.ToDecimal(dr["materialReceiptDetailsId"].ToString()) != 0 || inRef == 1)
                    {
                        dgvProductDetails.Rows[i].Cells["dgvcmbUnit"].ReadOnly = true;
                    }
                    i++;

                }
                if (cmbPurchaseMode.SelectedIndex > 1)
                {
                    dgvProductDetails.AllowUserToAddRows = false;
                }
                SerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI56:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print purchase invoice
        /// </summary>
        /// <param name="decMasterId"></param>
        public void Print(decimal decMasterId)
        {
            try
            {
                PurchaseMasterSP spPurchaseMaster = new PurchaseMasterSP();
                decimal decPurchaseOrderMasterId = 0;
                decimal decMaterialReceiptMasterId = 0;
                if (cmbPurchaseMode.Text == "Against Purchase Order")
                {
                    decPurchaseOrderMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                else if (cmbPurchaseMode.Text == "Against Material Receipt")
                {
                    decMaterialReceiptMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                }
                DataSet dsPurchaseInvoice = spPurchaseMaster.PurchaseInvoicePrinting(1, decPurchaseOrderMasterId, decMaterialReceiptMasterId, decMasterId);
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.PurchaseInvoicePrinting(dsPurchaseInvoice);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI57:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to print for dotmatrix
        /// </summary>
        /// <param name="decMasterId"></param>
        public void PrintForDotMatrix(decimal decMasterId)//used by safiyy
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
                dtblGridDetails.Columns.Add("Tax");
                dtblGridDetails.Columns.Add("TaxAmount");
                dtblGridDetails.Columns.Add("NetAmount");
                dtblGridDetails.Columns.Add("DiscountAmount");
                dtblGridDetails.Columns.Add("DiscountPercentage");
                dtblGridDetails.Columns.Add("GrossAmount");
                dtblGridDetails.Columns.Add("Rack");
                dtblGridDetails.Columns.Add("Batch");
                dtblGridDetails.Columns.Add("Rate");
                dtblGridDetails.Columns.Add("Amount");
                int inRowCount = 0;
                foreach (DataGridViewRow dRow in dgvProductDetails.Rows)
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
                        if (dRow.Cells["dgvtxtQuantity"].Value != null)
                        {
                            dr["Qty"] = dRow.Cells["dgvtxtQuantity"].Value.ToString();
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
                        if (dRow.Cells["dgvcmbTax"].Value != null)
                        {
                            dr["Tax"] = dRow.Cells["dgvcmbTax"].FormattedValue.ToString();
                        }
                        if (dRow.Cells["dgvtxtTaxAmount"].Value != null)
                        {
                            dr["TaxAmount"] = dRow.Cells["dgvtxtTaxAmount"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtNetValue"].Value != null)
                        {
                            dr["NetAmount"] = dRow.Cells["dgvtxtNetValue"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtDiscount"].Value != null)
                        {
                            dr["DiscountAmount"] = dRow.Cells["dgvtxtDiscount"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtDiscountPercent"].Value != null)
                        {
                            dr["DiscountPercentage"] = dRow.Cells["dgvtxtDiscountPercent"].Value.ToString();
                        }
                        if (dRow.Cells["dgvtxtGrossValue"].Value != null)
                        {
                            dr["GrossAmount"] = dRow.Cells["dgvtxtGrossValue"].Value.ToString();
                        }
                        dtblGridDetails.Rows.Add(dr);
                    }
                }
                //-------------Other Details-------------------\\
                dtblOtherDetails.Columns.Add("voucherNo");
                dtblOtherDetails.Columns.Add("date");
                dtblOtherDetails.Columns.Add("ledgerName");
                dtblOtherDetails.Columns.Add("PurchaseMode");
                dtblOtherDetails.Columns.Add("PurchaseAccount");
                dtblOtherDetails.Columns.Add("CreditPeriod");
                dtblOtherDetails.Columns.Add("VoucherType");
                dtblOtherDetails.Columns.Add("Narration");
                dtblOtherDetails.Columns.Add("Currency");
                dtblOtherDetails.Columns.Add("TotalAmount");
                dtblOtherDetails.Columns.Add("BillDiscount");
                dtblOtherDetails.Columns.Add("GrandTotal");
                dtblOtherDetails.Columns.Add("AmountInWords");
                dtblOtherDetails.Columns.Add("Declaration");
                dtblOtherDetails.Columns.Add("Heading1");
                dtblOtherDetails.Columns.Add("Heading2");
                dtblOtherDetails.Columns.Add("Heading3");
                dtblOtherDetails.Columns.Add("Heading4");
                dtblOtherDetails.Columns.Add("CustomerAddress");
                dtblOtherDetails.Columns.Add("CustomerTIN");
                dtblOtherDetails.Columns.Add("CustomerCST");
                DataRow dRowOther = dtblOtherDetails.Rows[0];
                dRowOther["voucherNo"] = txtVoucherNo.Text;
                dRowOther["date"] = txtVoucherDate.Text;
                dRowOther["ledgerName"] = cmbCashOrParty.Text;
                dRowOther["Narration"] = txtNarration.Text;
                dRowOther["Currency"] = cmbCurrency.Text;
                dRowOther["PurchaseMode"] = cmbPurchaseMode.Text;
                dRowOther["PurchaseAccount"] = cmbPurchaseAccount.Text;
                dRowOther["CreditPeriod"] = txtCreditPeriod.Text;
                dRowOther["BillDiscount"] = txtBillDiscount.Text;
                dRowOther["GrandTotal"] = txtGrandTotal.Text;
                dRowOther["TotalAmount"] = txtTotalAmount.Text;
                dRowOther["VoucherType"] = cmbVoucherType.Text;
                dRowOther["address"] = (dtblOtherDetails.Rows[0]["address"].ToString().Replace("\n", ", ")).Replace("\r", "");
                AccountLedgerSP spAccountLedger = new AccountLedgerSP();
                AccountLedgerInfo infoAccountLedger = new AccountLedgerInfo();
                infoAccountLedger = spAccountLedger.AccountLedgerView(Convert.ToDecimal(cmbCashOrParty.SelectedValue));
                dRowOther["CustomerAddress"] = (infoAccountLedger.Address.ToString().Replace("\n", ", ")).Replace("\r", "");
                dRowOther["CustomerTIN"] = infoAccountLedger.Tin;
                dRowOther["CustomerCST"] = infoAccountLedger.Cst;
                dRowOther["AmountInWords"] = new NumToText().AmountWords(Convert.ToDecimal(txtGrandTotal.Text), PublicVariables._decCurrencyId);
                VoucherTypeSP spVoucherType = new VoucherTypeSP();
                DataTable dtblDeclaration = spVoucherType.DeclarationAndHeadingGetByVoucherTypeId(decPurchaseInvoiceVoucherTypeId);
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
                formMDI.infoError.ErrorString = "PI58:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill product Details while return from Product creation when creating new Product 
        /// </summary>
        /// <param name="decProductId"></param>
        public void ReturnFromProductCreation(decimal decProductId)
        {
            ProductInfo infoProduct = new ProductInfo();
            ProductSP spProduct = new ProductSP();
            try
            {
                this.Enabled = true;
                this.BringToFront();
                if (decProductId != 0)
                {
                    int inCurrentRowIndex = dgvProductDetails.CurrentRow.Index;
                    dgvProductDetails.Rows.Add();
                    infoProduct = spProduct.ProductView(decProductId);
                    strProductCode = infoProduct.ProductCode;
                    ProductDetailsFill(strProductCode, inCurrentRowIndex, "ProductCode");
                    SerialNo();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI59:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDaybook to view details and for updation
        /// </summary>
        /// <param name="frmDayBook"></param>
        /// <param name="decMasterId"></param>
        public void callFromDayBook(frmDayBook frmDayBook, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmDayBook.Enabled = false;
                this.frmDayBookObj = frmDayBook;
                decPurchaseMasterId = decMasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI60:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmAgeingReport to view details and for updation
        /// </summary>
        /// <param name="frmAgeing"></param>
        /// <param name="decMasterId"></param>
        public void callFromAgeing(frmAgeingReport frmAgeing, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmAgeing.Enabled = false;
                this.frmAgeingObj = frmAgeing;
                decPurchaseMasterId = decMasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI61:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVoucherWiseProductSearch to view details and for updation
        /// </summary>
        /// <param name="frmVoucherwiseProductSearch"></param>
        /// <param name="decmasterId"></param>
        public void CallFromVoucherWiseProductSearch(frmVoucherWiseProductSearch frmVoucherwiseProductSearch, decimal decmasterId)
        {
            try
            {
                base.Show();
                frmVoucherwiseProductSearch.Enabled = false;
                objVoucherProduct = frmVoucherwiseProductSearch;
                decPurchaseMasterId = decmasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI62:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmLedgerDetails to view details and for updation
        /// </summary>
        /// <param name="LedgerDetailsObj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromLedgerDetails(frmLedgerDetails LedgerDetailsObj, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmLedgerDetailsObj = LedgerDetailsObj;
                frmLedgerDetailsObj.Enabled = false;
                decPurchaseMasterId = decMasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI63:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVatReturnReport to view details and for updation
        /// </summary>
        /// <param name="frmvatReturnReportobj"></param>
        /// <param name="decMasterId"></param>
        public void CallFromVatReturnReport(frmVatReturnReport frmvatReturnReportobj, decimal decMasterId)
        {
            try
            {
                base.Show();
                frmvatReturnReportobj = vatReturnReportobj;
                frmvatReturnReportobj.Enabled = false;
                decPurchaseMasterId = decMasterId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI64:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmVoucherSearch to view details and for updation
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="decId"></param>
        public void CallFromVoucherSerach(frmVoucherSearch frm, decimal decId)
        {
            try
            {
                base.Show();
                objVoucherSearch = frm;
                decPurchaseMasterId = decId;
                FillRegisterOrReport();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI65:" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                CashOrPartyComboFill();
                PurchaseAccountComboFill();
                dtpVoucherDate.MinDate = PublicVariables._dtFromDate;
                dtpVoucherDate.MaxDate = PublicVariables._dtToDate;
                dtpInvoiceDate.MinDate = PublicVariables._dtFromDate;
                dtpInvoiceDate.MaxDate = PublicVariables._dtToDate;
                dtpVoucherDate.Value = PublicVariables._dtCurrentDate;
                dtpInvoiceDate.Value = PublicVariables._dtCurrentDate;
                CurrencyComboFill();

                Clear();
                SettingsStatusCheck();
                FillProducts();
                isLoad = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI66:" + ex.Message;
            }
        }
        /// <summary>
        /// On form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmPurchaseInvoiceRegisterObj != null)
                {
                    frmPurchaseInvoiceRegisterObj.Enabled = true;
                    frmPurchaseInvoiceRegisterObj.GridFill();
                    frmPurchaseInvoiceRegisterObj = null;
                }
                if (frmPurchaseReportObj != null)
                {
                    frmPurchaseReportObj.Enabled = true;
                    frmPurchaseReportObj.GridFill();
                    frmPurchaseReportObj = null;
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Enabled = true;
                    frmDayBookObj.dayBookGridFill();
                    frmDayBookObj = null;
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Enabled = true;
                    frmLedgerDetailsObj.LedgerDetailsView();
                    frmLedgerDetailsObj = null;
                }
                if (objVoucherProduct != null)
                {
                    objVoucherProduct.Enabled = true;
                    objVoucherProduct.FillGrid();
                    objVoucherProduct = null;
                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Enabled = true;
                    objVoucherSearch.GridFill();
                    objVoucherSearch = null;
                }
                if (frmAgeingObj != null)
                {
                    frmAgeingObj.Enabled = true;
                    frmAgeingObj.FillGrid();
                    frmAgeingObj = null;
                }
                if (vatReturnReportobj != null)
                {
                    vatReturnReportobj.Enabled = true;
                    vatReturnReportobj.GridFill();
                    vatReturnReportobj = null;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI67:" + ex.Message;
            }
        }
        /// <summary>
        /// On cell enter of dgvProuctDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvProductDetails.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvProductDetails.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvProductDetails.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (e.ColumnIndex == dgvProductDetails.Columns["dgvcmbRack"].Index)
                    {
                        if (dgvProductDetails.CurrentRow.Cells["dgvcmbGodown"].Value != null)
                        {
                            if (dgvProductDetails.CurrentRow.Cells["dgvcmbGodown"].Value.ToString() != string.Empty)
                            {
                                RackComboFill(Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvcmbGodown"].Value), e.RowIndex, e.ColumnIndex);
                            }
                        }
                    }
                }
                if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value != null)
                {
                    if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value.ToString() != string.Empty)
                    {
                        if (decimal.Parse(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value.ToString()) > 0)
                        {
                            if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value != null && dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value.ToString() != string.Empty)
                            {
                                decMeterialReceiptQty = decimal.Parse(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI68:" + ex.Message;
            }
        }
        
        /// <summary>
        /// For shortcut keys
        /// ctrl+s for save 
        /// ctrl+d for delete
        /// alt+c for product creation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
                {
                    btnSave.Focus();
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)
                {
                    if (btnDelete.Enabled == true)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    if (dgvProductDetails.CurrentCell != null)
                    {
                        if (dgvProductDetails.CurrentCell == dgvProductDetails.CurrentRow.Cells["dgvtxtProductName"] || dgvProductDetails.CurrentCell == dgvProductDetails.CurrentRow.Cells["dgvtxtProductName"])
                        {
                            //SendKeys.Send("{F10}");
                            if (dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductName")
                            {
                                frmProductCreation frmProductCreationObj = new frmProductCreation();
                                frmProductCreationObj.MdiParent = formMDI.MDIObj;
                                frmProductCreationObj.CallFromPurchaseInvoice(this);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI69:" + ex.Message;
            }
        }
        /// <summary>
        /// on value change of dtpVoucherDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpVoucherDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpVoucherDate.Value;
                this.txtVoucherDate.Text = date.ToString("dd-MMM-yyyy");
                txtVoucherDate.Focus();
                CurrencyComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI70:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtVoucherDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtVoucherDate);
                if (!isInvalid)
                {
                    txtVoucherDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtVoucherDate.Text;
                dtpVoucherDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI71:" + ex.Message;
            }
        }
        /// <summary>
        /// On value change of dtpInvoiceDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpInvoiceDate.Value;
                this.txtInvoiceDate.Text = date.ToString("dd-MMM-yyyy");
                txtInvoiceDate.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI72:" + ex.Message;
            }
        }
        /// <summary>
        /// On textchange txtInvoiceDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInvoiceDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtInvoiceDate.Text == string.Empty && !txtInvoiceDate.Focused)
                {
                    DateValidation obj = new DateValidation();
                    bool isInvalid = obj.DateValidationFunction(txtInvoiceDate);
                    if (!isInvalid)
                    {
                        txtInvoiceDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                    }
                    string date = txtInvoiceDate.Text;
                    dtpInvoiceDate.Value = Convert.ToDateTime(date);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI73:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtInvoiceDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInvoiceDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation obj = new DateValidation();
                bool isInvalid = obj.DateValidationFunction(txtInvoiceDate);
                if (!isInvalid)
                {
                    txtInvoiceDate.Text = PublicVariables._dtCurrentDate.ToString("dd-MMM-yyyy");
                }
                string date = txtInvoiceDate.Text;
                dtpInvoiceDate.Value = Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI74:" + ex.Message;
            }
        }
        /// <summary>
        /// On selected index change of cmbCashOrParty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                OrderComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI75:" + ex.Message;
            }
        }
        /// <summary>
        /// Create new cashorparty On + button click of cashorparty 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCashOrParty_Click(object sender, EventArgs e)
        {
            try
            {
                AccountLedgerCreation();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI76:" + ex.Message;
            }
        }
        /// <summary>
        /// Number only validation of txtXredit period
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreditPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.NumberOnly(sender, e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI77:" + ex.Message;
            }
        }
        /// <summary>
        /// On selected index change of cmbPurchaseMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurchaseMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearDetails();
                if (cmbPurchaseMode.Text == "NA")
                {
                    lblOrderNo.Visible = false;
                    cmbOrderNo.Visible = false;
                    lblVoucherType.Visible = false;
                    cmbVoucherType.Visible = false;
                }
                else if (cmbPurchaseMode.Text == "Against Purchase Order")
                {
                    lblOrderNo.Text = "Order No";
                    lblOrderNo.Visible = true;
                    cmbOrderNo.Visible = true;
                    lblVoucherType.Visible = true;
                    cmbVoucherType.Visible = true;
                    VoucherTypeComboFill("Purchase Order");
                }
                else if (cmbPurchaseMode.Text == "Against Material Receipt")
                {
                    lblOrderNo.Text = "Receipt No";
                    lblOrderNo.Visible = true;
                    cmbOrderNo.Visible = true;
                    lblVoucherType.Visible = true;
                    cmbVoucherType.Visible = true;
                    VoucherTypeComboFill("Material Receipt");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI78:" + ex.Message;
            }
        }
        /// <summary>
        /// On selected value change of cmbVoucherType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                OrderComboFill();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI79:" + ex.Message;
            }
        }
        /// <summary>
        /// On selected value change of cmbOrderNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOrderNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((cmbOrderNo.SelectedValue == null ? "" : cmbOrderNo.SelectedValue.ToString()) != string.Empty)
                {
                    if (cmbOrderNo.SelectedValue.ToString() != "System.Data.DataRowView" && cmbOrderNo.Text != "System.Data.DataRowView")
                    {
                        if (cmbOrderNo.SelectedIndex > -1)
                        {
                            if (cmbPurchaseMode.Text == "Against Purchase Order")
                            {
                                PurchaseOrderDetailsFill();
                            }
                            else if (cmbPurchaseMode.Text == "Against Material Receipt")
                            {
                                MaterialReceiptDetailsFill();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI80:" + ex.Message;
            }
        }
        /// <summary>
        /// Create new PurchaseAccount On + button click of PurchaseAccount 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPurchaseAccount_Click(object sender, EventArgs e)
        {
            try
            {
                AccountLedgerCreation();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI81:" + ex.Message;
            }
        }
        /// <summary>
        /// On cell end edit of dgvProductDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strProductCode = string.Empty;
            string strProductName = string.Empty;
            string strBarcode = string.Empty;
            decimal decOldUnitConversionId = 0;
            decimal decNewUnitConversionId = 0;
            decimal decProductRate = 0;
            decimal decOldConversionRate = 0;
            decimal decNewConversionRate = 0;
            decimal decUnitId = 0;
            decimal decProductId = 0;
            UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
            try
            {
                if (e.ColumnIndex == dgvProductDetails.Columns["dgvcmbGodown"].Index)
                {
                    if (dgvProductDetails.CurrentRow.Cells["dgvcmbRack"].Value != null)
                    {
                        //dgvProductDetails.CurrentRow.Cells["dgvcmbRack"].Value = Convert.ToDecimal("1");
                    }
                }
                if (e.ColumnIndex == dgvProductDetails.Columns["dgvcmbUnit"].Index)
                {
                    if (dgvProductDetails.CurrentRow.Cells["dgvcmbUnit"].Value != null)
                    {
                        if (dgvProductDetails.CurrentRow.Cells["dgvcmbUnit"].Value.ToString() != string.Empty &&
                           dgvProductDetails.CurrentRow.Cells["dgvcmbUnit"].Value.ToString() != "0")
                        {
                            if (dgvProductDetails.CurrentRow.Cells["dgvtxtProductId"].Value != null)
                            {
                                if (dgvProductDetails.CurrentRow.Cells["dgvtxtProductId"].Value.ToString() != string.Empty &&
                                   dgvProductDetails.CurrentRow.Cells["dgvtxtProductId"].Value.ToString() != "0")
                                {
                                    decOldUnitConversionId = Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvtxtUnitConversionId"].Value.ToString());
                                    decOldConversionRate = spUnitConvertion.UnitConversionRateByUnitConversionId(decOldUnitConversionId);
                                    decUnitId = Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvcmbUnit"].Value.ToString());
                                    decProductId = Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvtxtProductId"].Value.ToString());
                                    decNewUnitConversionId = spUnitConvertion.UnitconversionIdViewByUnitIdAndProductId(decUnitId, decProductId);
                                    decNewConversionRate = spUnitConvertion.UnitConversionRateByUnitConversionId(decNewUnitConversionId);
                                    dgvProductDetails.CurrentRow.Cells["dgvtxtUnitConversionId"].Value = decNewUnitConversionId;
                                    if (dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value != null)
                                    {
                                        if (dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value.ToString() != string.Empty)
                                        {
                                            decProductRate = Convert.ToDecimal(dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value.ToString());
                                            decProductRate = decProductRate * decOldConversionRate / decNewConversionRate;
                                            dgvProductDetails.CurrentRow.Cells["dgvtxtRate"].Value = Math.Round(decProductRate, PublicVariables._inNoOfDecimalPlaces);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (e.ColumnIndex == dgvProductDetails.Columns["dgvtxtProductCode"].Index)
                {
                    if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductCode"].Value != null)
                    {
                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductCode"].Value.ToString() != string.Empty)
                        {
                            strProductCode = dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductCode"].Value.ToString();
                            ProductDetailsFill(strProductCode, e.RowIndex, "ProductCode");
                        }
                    }
                }
                if (e.ColumnIndex == dgvProductDetails.Columns["dgvtxtProductName"].Index)
                {
                    if (dgvProductDetails.CurrentRow.Cells["dgvcmbBatch"].Value != null)
                    {
                        dgvProductDetails.CurrentRow.Cells["dgvcmbBatch"].Value = 1;
                        dgvProductDetails.CurrentRow.Cells["dgvtxtBarcode"].Value = string.Empty;
                    }
                    if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductName"].Value != null)
                    {
                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductName"].Value.ToString() != string.Empty)
                        {
                            strProductName = dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductName"].Value.ToString();
                            ProductDetailsFill(strProductName, e.RowIndex, "ProductName");
                        }
                    }
                }
                if (e.ColumnIndex == dgvProductDetails.Columns["dgvtxtBarcode"].Index)
                {
                    if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value != null)
                    {
                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value.ToString() != string.Empty)
                        {
                            strBarcode = dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value.ToString();
                            ProductDetailsFill(strBarcode, e.RowIndex, "Barcode");
                        }
                    }
                }
                Calculate(e.RowIndex);
                CalculateTotalAmount();
                if (dgvTax.Visible)
                {
                    TotalTaxAmount();
                }
                CalculateGrandTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI82:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate total amount
        /// </summary>
        public void CalculateTotalAmount()
        {
            decimal decTotalAmount = 0;
            decimal decGridTotalAmount = 0;
            try
            {
                foreach (DataGridViewRow dgrow in dgvProductDetails.Rows)
                {
                    if (dgrow.Cells["dgvtxtAmount"].Value != null)
                    {
                        if (dgrow.Cells["dgvtxtAmount"].Value.ToString() != string.Empty && dgrow.Cells["dgvtxtAmount"].Value.ToString() != "0")
                        {
                            decTotalAmount = Convert.ToDecimal(dgrow.Cells["dgvtxtAmount"].Value.ToString());
                            decGridTotalAmount = decGridTotalAmount + decTotalAmount;
                        }
                    }
                }
                txtTotalAmount.Text = Math.Round(decGridTotalAmount, PublicVariables._inNoOfDecimalPlaces).ToString();
                if (txtTotalAmount.Text.Split('.')[0].Length > 13)
                {
                    MessageBox.Show("Amount exeed than limit");
                    dgvProductDetails.Rows[dgvProductDetails.Rows.Count - 2].Cells["dgvtxtRate"].Value = string.Empty;
                    dgvProductDetails.Rows[dgvProductDetails.Rows.Count - 2].Cells["dgvtxtGrossValue"].Value = string.Empty;
                    dgvProductDetails.Rows[dgvProductDetails.Rows.Count - 2].Cells["dgvtxtAmount"].Value = string.Empty;
                    CalculateTotalAmount();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI83:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to calculate total amount
        /// </summary>
        /// <param name="inRowIndex"></param>
        public void Calculate(int inRowIndex)
        {
            decimal decDiscount = 0;
            decimal decDiscountPercent = 0;
            decimal decGrossValue = 0;
            decimal decNetValue = 0;
            decimal decTaxAmount = 0;
            decimal decTaxPercent = 0;
            decimal decTaxId = 0;
            decimal decAmount = 0;
            decimal decTotalAmount = 0;
            decimal decProductId = 0;
            decimal decDefaultTotalAmount = 0;
            decimal decProductRate = 0;
            decimal decQuantity = 0;
            ProductInfo infoProduct = new ProductInfo();
            ProductSP spProduct = new ProductSP();
            TaxInfo infotax = new TaxInfo();
            TaxSP spTax = new TaxSP();
            ExchangeRateSP spExchangeRate = new ExchangeRateSP();
            try
            {
                if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductId"].Value != null)
                {
                    if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductId"].Value.ToString() != string.Empty)
                    {
                        if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtRate"].Value != null)
                        {
                            if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtRate"].Value.ToString() != string.Empty && dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtRate"].Value.ToString() != ".")
                            {
                                decProductRate = Convert.ToDecimal(dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtRate"].Value.ToString());
                            }
                        }
                        if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtQuantity"].Value != null)
                        {
                            if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtQuantity"].Value.ToString() != string.Empty && dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtQuantity"].Value.ToString() != ".")
                            {
                                decQuantity = Convert.ToDecimal(dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtQuantity"].Value.ToString());
                            }
                        }
                        decGrossValue = decProductRate * decQuantity;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtGrossValue"].Value = Math.Round(decGrossValue, PublicVariables._inNoOfDecimalPlaces);
                        if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value != null)
                        {
                            if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value.ToString() != string.Empty)
                            {
                                decDiscountPercent = Convert.ToDecimal(dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value.ToString());
                            }
                            else
                            {
                                dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value = 0;
                            }
                        }
                        else
                        {
                            dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value = 0;
                        }
                        if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value != null)
                        {
                            if (dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value.ToString() != string.Empty)
                            {
                                decDiscount = Convert.ToDecimal(dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value.ToString());
                            }
                            else
                            {
                                dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value = 0;
                            }
                        }
                        else
                        {
                            dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value = 0;
                        }
                        /*------------------------------Calculate-----------------------------------*/
                        /*------------------------------Discount Calculation-----------------------------------*/
                        if (decGrossValue >= decDiscount)
                        {
                            dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value = Math.Round(decDiscount, PublicVariables._inNoOfDecimalPlaces);
                        }
                        else
                        {
                            dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscountPercent"].Value = 0;
                            dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtDiscount"].Value = 0;
                            decDiscount = 0;
                        }
                        decNetValue = decGrossValue - decDiscount;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtNetValue"].Value = Math.Round(decNetValue, PublicVariables._inNoOfDecimalPlaces);
                        /*------------------------------Tax Calculation-----------------------------------*/
                        if (dgvcmbTax.Visible)
                        {
                            if (dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbTax"].Value != null)
                            {
                                if (dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbTax"].Value.ToString() != string.Empty &&
                                    dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbTax"].Value.ToString() != "0")
                                {
                                    decTaxId = Convert.ToDecimal(dgvProductDetails.Rows[inRowIndex].Cells["dgvcmbTax"].Value.ToString());
                                    infotax = spTax.TaxView(decTaxId);
                                    decTaxPercent = infotax.Rate;
                                }
                                else
                                {
                                    decTaxPercent = 0;
                                }
                            }
                            else
                            {
                                decTaxPercent = 0;
                            }
                            decProductId = Convert.ToDecimal(dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtProductId"].Value.ToString());
                            infoProduct = spProduct.ProductView(decProductId);
                            if (infoProduct.TaxapplicableOn == "MRP")
                            {
                                decTaxAmount = (infoProduct.Mrp*decQuantity) * decTaxPercent / 100;
                            }
                            else
                            {
                                decTaxAmount = decNetValue * decTaxPercent / 100;
                            }
                            dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtTaxAmount"].Value = Math.Round(decTaxAmount, PublicVariables._inNoOfDecimalPlaces);
                        }
                        decAmount = decNetValue + decTaxAmount;
                        dgvProductDetails.Rows[inRowIndex].Cells["dgvtxtAmount"].Value = Math.Round(decAmount, PublicVariables._inNoOfDecimalPlaces);
                        decTotalAmount = decTotalAmount + decAmount;
                        decDefaultTotalAmount = decTotalAmount * 1;
                        //CalculateTotalAmount();
                        //if (dgvTax.Visible)
                        //{
                        //    TotalTaxAmount();
                        //}
                        //CalculateGrandTotal();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI84:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from each cell of dgvProductDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            string strBarcode = string.Empty;
            decimal decDiscountPercent = 0;
            decimal decDiscount = 0;
            decimal decGrossValue = 0;
            try
            {
                BatchSP spBatch = new BatchSP();
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductId"].Value != null)
                    {
                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductId"].Value.ToString() != string.Empty && dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtProductId"].Value.ToString() != "0")
                        {
                            if (e.ColumnIndex == dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].ColumnIndex)
                            {
                                if (dgvProductDetails.RowCount > 1)
                                {
                                    try
                                    {
                                        MaterialReceiptDetailsSP spMaterialReceiptDetails = new MaterialReceiptDetailsSP();
                                        decimal decMaterialReceiptMasterId = Convert.ToDecimal(cmbOrderNo.SelectedValue.ToString());
                                        dtblMeterialReceiptQty = spMaterialReceiptDetails.MaterialReceiptDetailsViewByMaterialReceiptMasterIdWithRemainingByNotInCurrPI
                                        (decMaterialReceiptMasterId, decPurchaseMasterId, decPurchaseInvoiceVoucherTypeId);
                                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value != null)
                                        {
                                            if (decimal.Parse(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtMaterialReceiptDetailsId"].Value.ToString()) > 0)
                                            {
                                                if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value != null)
                                                {
                                                    if (decimal.Parse(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value.ToString()) > decimal.Parse(dtblMeterialReceiptQty.Rows[e.RowIndex]["qty"].ToString()))
                                                    {
                                                        dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value = dtblMeterialReceiptQty.Rows[e.RowIndex]["qty"].ToString();
                                                        if (decMeterialReceiptQty < decimal.Parse(dtblMeterialReceiptQty.Rows[e.RowIndex]["qty"].ToString()))
                                                        {
                                                            dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtQuantity"].Value = decMeterialReceiptQty;
                                                            decMeterialReceiptQty = 0;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            else if (e.ColumnIndex == dgvProductDetails.Columns["dgvcmbBatch"].Index)
                            {
                                if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvcmbBatch"].Value != null && dgvProductDetails.Rows[e.RowIndex].Cells["dgvcmbBatch"].Value.ToString() != string.Empty)
                                {
                                    decimal decBatchId = Convert.ToDecimal(dgvProductDetails.Rows[e.RowIndex].Cells["dgvcmbBatch"].Value);
                                    strBarcode = spBatch.ProductBatchBarcodeViewByBatchId(decBatchId);
                                    dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtBarcode"].Value = strBarcode;
                                }
                            }
                            else if (e.ColumnIndex == dgvProductDetails.Columns["dgvtxtGrossValue"].Index)
                            {
                                dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtDiscountPercent"].Value = 0;
                                dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtDiscount"].Value = 0;
                            }
                            else if (e.ColumnIndex == dgvProductDetails.Columns["dgvtxtDiscountPercent"].Index)
                            {
                                if (dgvProductDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                                {
                                    if (dgvProductDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
                                    {
                                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtGrossValue"].Value != null)
                                        {
                                            if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtGrossValue"].Value.ToString() != string.Empty)
                                            {
                                                decDiscountPercent = Convert.ToDecimal(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtDiscountPercent"].Value.ToString());
                                                decGrossValue = Convert.ToDecimal(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtGrossValue"].Value.ToString());
                                                if (decGrossValue > 0)
                                                {
                                                    decDiscount = decGrossValue * decDiscountPercent / 100;
                                                }
                                                dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtDiscount"].Value = Math.Round(decDiscount, PublicVariables._inNoOfDecimalPlaces);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (e.ColumnIndex == dgvProductDetails.Columns["dgvtxtDiscount"].Index)
                            {
                                if (dgvProductDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                                {
                                    if (dgvProductDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
                                    {
                                        if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtGrossValue"].Value != null)
                                        {
                                            if (dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtGrossValue"].Value.ToString() != string.Empty)
                                            {
                                                decDiscount = Convert.ToDecimal(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtDiscount"].Value.ToString());
                                                decGrossValue = Convert.ToDecimal(dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtGrossValue"].Value.ToString());
                                                if (decGrossValue > 0)
                                                {
                                                    decDiscountPercent = decDiscount * 100 / decGrossValue;
                                                }
                                                dgvProductDetails.Rows[e.RowIndex].Cells["dgvtxtDiscountPercent"].Value = Math.Round(decDiscountPercent, PublicVariables._inNoOfDecimalPlaces);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    SerialNo();
                }
                CheckInvalidEntries(e);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI85:" + ex.Message;
            }
        }
        /// <summary>
        /// make each and every changes of grid has to be commited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductDetails.IsCurrentCellDirty)
                {
                    dgvProductDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI86:" + ex.Message;
            }
        }
        /// <summary>
        /// grid EditingControlShowing event To handle the keypress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBoxControl = e.Control as DataGridViewTextBoxEditingControl;
                if (TextBoxControl != null)
                {
                    if (dgvProductDetails.CurrentCell != null && dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductName")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = ProductNames;
                    }
                    if (dgvProductDetails.CurrentCell != null && dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = ProductCodes;
                    }
                    if (dgvProductDetails.CurrentCell != null && dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name != "dgvtxtProductCode" && dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name != "dgvtxtProductName")
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)dgvProductDetails.EditingControl;
                        editControl.AutoCompleteMode = AutoCompleteMode.None;
                    }
                    TextBoxControl.KeyPress += TextBoxKeyPress;
                }
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                    tb.KeyDown -= dgvProductDetails_KeyDown;
                    tb.KeyDown += new KeyEventHandler(dgvProductDetails_KeyDown);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI87:" + ex.Message;
            }
        }
        /// <summary>
        /// decimal validtaion on key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvProductDetails.CurrentCell != null)
                {
                    if (dgvProductDetails.CurrentCell.ColumnIndex == dgvProductDetails.Columns["dgvtxtQuantity"].Index)
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                    if (dgvProductDetails.CurrentCell.ColumnIndex == dgvProductDetails.Columns["dgvtxtRate"].Index)
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                    if (dgvProductDetails.CurrentCell.ColumnIndex == dgvProductDetails.Columns["dgvtxtDiscountPercent"].Index)
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                    if (dgvProductDetails.CurrentCell.ColumnIndex == dgvProductDetails.Columns["dgvtxtDiscount"].Index)
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI88:" + ex.Message;
            }
        }
        /// <summary>
        /// To remove the rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dgvProductDetails.SelectedCells.Count > 0 && dgvProductDetails.CurrentRow != null)
                {
                    if (!dgvProductDetails.Rows[dgvProductDetails.CurrentRow.Index].IsNewRow)
                    {
                        if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (dgvProductDetails.RowCount > 1)
                            {
                                //if (dgvProductDetails.CurrentRow.Index < dgvProductDetails.RowCount - 1)
                                //{
                                    if (btnSave.Text == "Update")
                                    {
                                        if (dgvProductDetails.CurrentRow.Cells["dgvtxtPurchaseDetailsId"].Value != null)
                                        {
                                            if (dgvProductDetails.CurrentRow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString() != string.Empty)
                                            {
                                                arrlstRemove.Add(dgvProductDetails.CurrentRow.Cells["dgvtxtPurchaseDetailsId"].Value.ToString());
                                            }
                                        }
                                    }
                                    dgvProductDetails.Rows.Remove(dgvProductDetails.CurrentRow);
                                    Calculate();
                                    SerialNo();
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI89:" + ex.Message;
            }
        }
        /// <summary>
        /// make each and every changes of grid has to be commited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAdditionalCost_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                TotalAdditionalCostAmount();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI90:" + ex.Message;
            }
        }
        /// <summary>
        /// For additionalcost combo fill
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAdditionalCost_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvAdditionalCost.Columns["dgvcmbLedger"].Index)
                {
                    AdditionalCostComboFill(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI91:" + ex.Message;
            }
        }
        /// <summary>
        /// Calling the keypress event here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAdditionalCost_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBoxControl = e.Control as DataGridViewTextBoxEditingControl;
                if (TextBoxControl != null)
                {
                    TextBoxControl.KeyPress += dgvAdditionalCost_KeyPress;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI92:" + ex.Message;
            }
        }
        /// <summary>
        /// Decimal validation for dgvAdditionalcost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvAdditionalCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvAdditionalCost.CurrentCell != null)
                {
                    if (dgvAdditionalCost.CurrentCell.ColumnIndex == dgvAdditionalCost.Columns["dgvtxtAdditionalCostAmount"].Index)
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI93:" + ex.Message;
            }
        }
        /// <summary>
        /// To remove row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblAdditionalRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvAdditionalCost.RowCount > 1)
                    {
                        if (dgvAdditionalCost.CurrentRow.Index < dgvAdditionalCost.RowCount - 1)
                        {
                            if (btnSave.Text == "Update")
                            {
                                if (dgvAdditionalCost.CurrentRow.Cells["dgvtxtAdditionalCostId"].Value != null)
                                {
                                    if (dgvAdditionalCost.CurrentRow.Cells["dgvtxtAdditionalCostId"].Value.ToString() != string.Empty)
                                    {
                                        arrlstRemoveAdditionalCost.Add(dgvAdditionalCost.CurrentRow.Cells["dgvtxtAdditionalCostId"].Value.ToString());
                                    }
                                }
                            }
                            dgvAdditionalCost.Rows.Remove(dgvAdditionalCost.CurrentRow);
                            TotalAdditionalCostAmount();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI94:" + ex.Message;
            }
        }
        /// <summary>
        /// Add serialNo in dgvTax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTax_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                TaxSerialNo();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI95:" + ex.Message;
            }
        }
        /// <summary>
        /// On text change of txtTotalamount do grandtotal calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateGrandTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI96:" + ex.Message;
            }
        }
        /// <summary>
        /// On text change of lblTaxamount do calculate grand total
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTaxAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateGrandTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI97:" + ex.Message;
            }
        }
        /// <summary>
        /// On text change of lblAdditionalcostAmount do grandtotal calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblAdditionalCostAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateGrandTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI98:" + ex.Message;
            }
        }
        /// <summary>
        /// decimal validation in txtBillDiscount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI99:" + ex.Message;
            }
        }
        /// <summary>
        /// On leave from txtBillDiscount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillDiscount_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtBillDiscount.Text == string.Empty)
                {
                    txtBillDiscount.Text = Math.Round(0.00, PublicVariables._inNoOfDecimalPlaces).ToString();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI100:" + ex.Message;
            }
        }
        /// <summary>
        /// On enter of txtBillDiscount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillDiscount_Enter(object sender, EventArgs e)
        {
            try
            {
                txtBillDiscount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI101:" + ex.Message;
            }
        }
        /// <summary>
        /// On text change of txtBillDiscount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateGrandTotal();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI102:" + ex.Message;
            }
        }
        /// <summary>
        /// On save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnSave.Text))
                {
                    SaveOrEdit();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI103:" + ex.Message;
            }
        }
        /// <summary>
        /// On delete button click
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
                formMDI.infoError.ErrorString = "PI104:" + ex.Message;
            }
        }
        /// <summary>
        /// On close button click
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
                formMDI.infoError.ErrorString = "PI105:" + ex.Message;
            }
        }
        /// <summary>
        /// On clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                if (frmAgeingObj != null)
                {
                    frmAgeingObj.Close();
                    frmAgeingObj = null;
                }
                if (frmDayBookObj != null)
                {
                    frmDayBookObj.Close();
                    frmDayBookObj = null;
                }
                if (frmLedgerDetailsObj != null)
                {
                    frmLedgerDetailsObj.Close();
                    frmLedgerDetailsObj = null;
                }
                if (frmLedgerPopupObj != null)
                {
                    frmLedgerPopupObj.Close();
                    frmLedgerPopupObj = null;
                }
                if (frmProductSearchPopupObj != null)
                {
                    frmProductSearchPopupObj.Close();
                    frmProductSearchPopupObj = null;
                }
                if (frmPurchaseInvoiceRegisterObj != null)
                {
                    frmPurchaseInvoiceRegisterObj.Close();
                    frmPurchaseInvoiceRegisterObj = null;
                }
                if (frmPurchaseReportObj != null)
                {
                    frmPurchaseReportObj.Close();
                    frmPurchaseReportObj = null;
                }
                if (vatReturnReportobj != null)
                {
                    vatReturnReportobj.Close();
                    vatReturnReportobj = null;
                }
                if (objVoucherSearch != null)
                {
                    objVoucherSearch.Close();
                    objVoucherSearch = null;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI106:" + ex.Message;
            }
        }
        /// <summary>
        /// On cell enter of dgvAdditionalcost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAdditionalCost_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvAdditionalCost.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    dgvAdditionalCost.EditMode = DataGridViewEditMode.EditOnEnter;
                }
                else
                {
                    dgvAdditionalCost.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                }
               
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI107:" + ex.Message;
            }
        }
        
        /// <summary>
        /// Handling data error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
                {
                    object value = dgvProductDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (!((DataGridViewComboBoxColumn)dgvProductDetails.Columns[e.ColumnIndex]).Items.Contains(value))
                    {
                        e.ThrowException = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI108:" + ex.Message;
            }
        }
        /// <summary>
        /// Handling dataerror
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAdditionalCost_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
                {
                    object value = dgvAdditionalCost.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (!((DataGridViewComboBoxColumn)dgvAdditionalCost.Columns[e.ColumnIndex]).Items.Contains(value))
                    {
                        e.ThrowException = false;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI109:" + ex.Message;
            }
        }
        #endregion
        #region Navigation
        /// <summary>
        /// Enter key navigation of txtVoucherNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVoucherDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI110:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtVoucherDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVoucherDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVendorInvoiceNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVoucherDate.SelectionStart == 0)
                    {
                        if (!isAutomatic)
                        {
                            txtVoucherNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI111:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtVendorInvoiceNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVendorInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtInvoiceDate.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtVendorInvoiceNo.SelectionStart == 0)
                    {
                        txtVoucherDate.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI112:" + ex.Message;
            }
        }
        /// <summary>
        /// EnterKey and backspace navigation of txtInvoiceDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInvoiceDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCashOrParty.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtInvoiceDate.SelectionStart == 0)
                    {
                        txtVendorInvoiceNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI113:" + ex.Message;
            }
        }
        /// <summary>
        /// For shortcut keys
        /// Alt+c for account ledger creation 
        /// ctrl+f for popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCashOrParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCreditPeriod.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtInvoiceDate.Focus();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    AccountLedgerCreation();
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (cmbCashOrParty.SelectedIndex != -1)
                    {
                        frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromPurchaseInvoice(this, Convert.ToDecimal(cmbCashOrParty.SelectedValue.ToString()), "CashOrSundryCreditors");
                    }
                    else
                    {
                        Messages.InformationMessage("Select any cash or party");
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI114:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtCreditPeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreditPeriod_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbPurchaseMode.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtCreditPeriod.SelectionStart == 0)
                    {
                        cmbCashOrParty.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI115:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbPurchaseMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurchaseMode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbVoucherType.Visible)
                    {
                        cmbVoucherType.Focus();
                    }
                    else
                    {
                        cmbPurchaseAccount.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtCreditPeriod.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI116:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbVoucherType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbVoucherType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbOrderNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbPurchaseMode.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI117:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navgation of cmbOrderNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOrderNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbPurchaseAccount.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbVoucherType.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI118:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbPurchaseAccount
        /// alt+c for accountledger creation
        /// ctrl+f for popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurchaseAccount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cmbCurrency.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbOrderNo.Visible)
                    {
                        cmbOrderNo.Focus();
                    }
                    else
                    {
                        cmbPurchaseMode.Focus();
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    AccountLedgerCreation();
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Pop Up
                {
                    if (cmbCashOrParty.SelectedIndex != -1)
                    {
                        frmLedgerPopup frmLedgerPopupObj = new frmLedgerPopup();
                        frmLedgerPopupObj.MdiParent = formMDI.MDIObj;
                        frmLedgerPopupObj.CallFromPurchaseInvoice(this, Convert.ToDecimal(cmbPurchaseAccount.SelectedValue.ToString()), "PurchaseAccount");
                    }
                    else
                    {
                        Messages.InformationMessage("Select any purchase account");
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI119:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cmbCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dgvProductDetails.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    cmbPurchaseAccount.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI120:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of dgvProductDetails
        /// ctrl+f for productsearch popup
        /// alt+c for productcreation
        /// Esc for form close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductDetails_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int inRowCount = dgvProductDetails.Rows.Count;
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProductDetails.CurrentCell == dgvProductDetails.Rows[inRowCount - 1].Cells["dgvtxtAmount"])
                    {
                        dgvAdditionalCost.Focus();
                        dgvProductDetails.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvProductDetails.CurrentCell == dgvProductDetails.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        cmbCurrency.Focus();
                        dgvProductDetails.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.F && Control.ModifierKeys == Keys.Control) //Product Search Pop Up
                {
                    if (dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        frmProductSearchPopup frmProductSearchPopupObj = new frmProductSearchPopup();
                        frmProductSearchPopupObj.MdiParent = formMDI.MDIObj;
                        if (dgvProductDetails.CurrentRow.Cells["dgvtxtProductCode"].Value != null || dgvProductDetails.CurrentRow.Cells["dgvtxtProductName"].Value != null)
                        {
                            frmProductSearchPopupObj.CallFromPurchaseInvoice(this, dgvProductDetails.CurrentRow.Index, dgvProductDetails.CurrentRow.Cells["dgvtxtProductCode"].Value.ToString());
                        }
                        else
                        {
                            frmProductSearchPopupObj.CallFromPurchaseInvoice(this, dgvProductDetails.CurrentRow.Index, string.Empty);
                        }
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt) //Product Creation
                {
                    SendKeys.Send("{f10}");
                    if (dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductName" || dgvProductDetails.Columns[dgvProductDetails.CurrentCell.ColumnIndex].Name == "dgvtxtProductCode")
                    {
                        frmProductCreation frmProductCreationObj = new frmProductCreation();
                        frmProductCreationObj.MdiParent = formMDI.MDIObj;
                        frmProductCreationObj.CallFromPurchaseInvoice(this);
                    }
                }
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI121:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of dgvAdditionalCost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAdditionalCost_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int inRowCount = dgvAdditionalCost.RowCount;
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvAdditionalCost.CurrentCell == dgvAdditionalCost.Rows[inRowCount - 1].Cells["dgvtxtAdditionalCostAmount"])
                    {
                        txtNarration.Focus();
                        txtTransportationCompany.SelectionStart = txtTransportationCompany.TextLength;
                        dgvAdditionalCost.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvAdditionalCost.CurrentCell == dgvAdditionalCost.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        dgvProductDetails.Focus();
                        dgvAdditionalCost.ClearSelection();
                    }
                }
                if (e.KeyCode == Keys.Escape)
                {
                    btnClose_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI122:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtNarration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNarration_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    inNarrationCount++;
                    if (inNarrationCount >= 2)
                    {
                        txtTransportationCompany.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.SelectionStart == 0)
                    {
                        dgvAdditionalCost.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI123:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtTransportationCompany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTransportationCompany_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtLRNo.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtTransportationCompany.SelectionStart == 0)
                    {
                        txtNarration.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI124:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtLRNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLRNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBillDiscount.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtLRNo.SelectionStart == 0)
                    {
                        txtTransportationCompany.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI125:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of txtBillDiscount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBillDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxPrintAfterSave.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtBillDiscount.SelectionStart == 0)
                    {
                        txtLRNo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI126:" + ex.Message;
            }
        }
        /// <summary>
        /// Enterkey and backspace navigation of cbxPrintAfterSave
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
                    txtBillDiscount.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI127:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation of btnSave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyDown(object sender, KeyEventArgs e)
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
                formMDI.infoError.ErrorString = "PI128:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation of btnClear
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI129:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation of btnDelete
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
                formMDI.infoError.ErrorString = "PI130:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation of btnClose
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
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PI131:" + ex.Message;
            }
        }
        #endregion

       



    }
}
