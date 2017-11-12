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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace One_Account
{
    public partial class frmProductCreation : Form
    {
        #region PublicVariables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        int inNarrationCount = 0;
        int inBatchIdWithPartNoNA;
        int inArrOfRemoveIndex = 0;//Index to remov row from tbl_stockposting
        decimal decId;                    //Unit Id of product
        decimal decCheck;                 //CheckWeather Data Added to Stockposting
        decimal decUnitIdSelectedWhenMulUntCalled;//UnitId When Selecting Multiple Unit
        decimal decSaveProduct;                  // Id Of Last Saved Product
        decimal decProductIdForEdit;             //Id of Product for Updation
        decimal decUnitIdForUpdate;              //Produt UnitId For Updation
        decimal decProductId;
        string strBrandName;
        string strSizeName;
        string strTaxName;
        string strGodownName;
        string strUnitName;
        string strModelName;
        string strRackName;
        string strGroupName;
        string strProductName;
        bool isBatchRemoved = false;                      //Batch removed while updating
        bool isUpdated;                                //Check weather product updated Or not
        bool isBomUpdated;                            //Check weather bom updated Or not
        bool isDeletedConfirmedForMulUnit = false;   //Check weather unitConvertion Deleted
        bool isSaveBomCheck;                        //Check weather bom Created
        bool isSaveMulUnitCheck;                   //Check weather UnitConvertion Created
        bool isstockPostingGridFil;
        bool isOpeningStockChanged;
        bool isCallFromGrid = false;
        bool isValueChanged = false;
        bool isGridHasToClear = false;
        bool IsdgvConsumption = false;
        bool IsdgvProduction = false;
        bool isCheck = false;               //Ckeck weather unit has changed
        bool isAuto = false;               //Check weatherProductCode is automatic or not
        bool isBomFromRegister;
        bool isMulUnitFromRgister = false;  //Check weather th product has multiple unit
        bool isDeletedConfirmed;           //If bom deleted from frmProductBom
        bool isStpUpdated;               //Check weather StockPosting is updated
        bool isOpeningStockForUpdate;   //Check weather the product has  opening stock
        bool isBatchForUpdate;         //Check weather the product has  batch
        bool isBatchUpdated;          //Check weather the batch table updated
        bool isRowRemoved = false;   //Data raw removed fron stockposting
        bool isMulUnitUpdated;      //Check weather multiple unit is updated
        bool isBatchCheck = false;//Check weather batch no is repeting
        bool isGodownCheck = false; //Check weather Godown And Rack  repeting
        bool isFromPOSItemCombo = false;
        bool isRackFill = false;//check whether godown combobox is filled before filling rack
        decimal decBatchId = 0;
        decimal decGowdownId;
        decimal decRackId;
        decimal decGodownIdForRack;
        string strUnitNameForGrid;
        ArrayList lstArrOfRemove = new ArrayList();
        ArrayList lstArrOfRemoveFromBatchTable = new ArrayList();
        ArrayList lstArrOfRemoveFromStockPosting = new ArrayList();
        ArrayList lstArrOfRemoveFromBatch = new ArrayList();
        ArrayList lststrArrBatchRemove = new ArrayList();
        ArrayList lststrArrOfRemoveForBom = new ArrayList();
        string[] strArrOfRemoveForBom = new string[100];
        string[] strArrOfRemoveForMulUnit = new string[100];
        frmProductMultipleUnit frmMultipleUnitObj = new frmProductMultipleUnit();
        DataTable dtblBom;                   //Data table from frmBom
        DataTable dtblMulUnit;               //Data table from frmProductMultipleUnit
        DataTable dtblBomForEdit;            //Data table from frmBom after updation
        DataTable dtblMulUnitForEdit;        //Data table from frmProductMultipleUnit
        DataTable dtblFromBomForUpdate;
        DataTable dtblFromMulUnitForUpdate;
        DataTable dtblForUnitIdInOpeNingStock;
        AutoCompleteStringCollection Batch = new AutoCompleteStringCollection();
        DataGridViewTextBoxEditingControl TextBoxControl;
        frmProductRegister frmProductRegisterObj;
        frmPOS frmPosObj;
        frmSalesInvoice frmSalesInvoiceObj = null;
        frmPhysicalStock frmPhysicalStockObj = null;
        frmPurchaseInvoice frmPurchaseInvoiceObj = null;
        frmMaterialReceipt frmMaterialReceiptObj = null;
        frmPurchaseReturn frmPurchaseReturnObj = null;
        frmSalesReturn frmSalesReturnObj = null;
        frmPurchaseOrder frmpurchaseOrderObj = null;
        frmSalesQuotation frmSalesQuotationObj = null;
        frmSalesOrder frmSalesOrderObj = null;
        frmProductBom frmBomObj = null;
        frmDeliveryNote frmDeliveryNoteObj = null;
        frmStockJournal frmStockJournalObj = null;
        #endregion
        #region Functions
        /// <summary>
        /// Creates an instance of frmProductCreation class
        /// </summary>
        public frmProductCreation()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function to add datatable from productBom
        /// </summary>
        /// <param name="dtblRecived"></param>
        /// <param name="isSaveBom"></param>
        public void DataTableReturn(DataTable dtblRecived, bool isSaveBom)
        {
            try
            {
                isSaveBomCheck = isSaveBom;
                dtblBom = dtblRecived;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:1" + ex.Message;
              
            }
        }
        /// <summary>
        /// Function to add data to tbl_Bom
        /// </summary>
        public void BomTableFill()
        {
            try
            {
                BomInfo infoBom = new BomInfo();
                BomSP spBom = new BomSP();
                decimal decProductId;
                if (btnSave.Text == "Update")
                {
                    decProductId = decProductIdForEdit;
                }
                else
                {
                    decProductId = decSaveProduct;
                }
                for (int i = 0; i < dtblBom.Rows.Count; i++)
                {
                    infoBom.RowmaterialId = Convert.ToDecimal(dtblBom.Rows[i]["dgvcmbRawMaterial"]);
                    infoBom.UnitId = Convert.ToDecimal(dtblBom.Rows[i]["dgvtxtUnitId"]);
                    infoBom.Quantity = Convert.ToDecimal(dtblBom.Rows[i]["dgvtxtQty"]);
                    infoBom.Extra1 = dtblBom.Rows[i]["extra1"].ToString();
                    infoBom.Extra2 = dtblBom.Rows[i]["extra2"].ToString();
                    infoBom.ExtraDate = Convert.ToDateTime(dtblBom.Rows[i]["extraDate"]);
                    spBom.BomFromDatatable(infoBom, decProductId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:2" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the datagridview while updating
        /// </summary>
        public void OpeningStockGridFill()
        {
            try
            {
                StockPostingSP spStockposting = new StockPostingSP();
                ProductSP spProduct = new ProductSP();
                DataTable dtbl = new DataTable();
                dtbl = spProduct.ProductViewGridFillFromStockPosting(decProductIdForEdit);
                UnitComboInsideGridFill();
                dgvProductCreation.Rows.Clear();
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    dgvProductCreation.Rows.Add();
                    dgvProductCreation.Rows[i].Cells["dgvtxtstockpostId"].Value = dtbl.Rows[i]["stockPostingId"];
                    dgvProductCreation.Rows[i].Cells["dgvtxtqty"].Value = dtbl.Rows[i]["inwardQty"];
                    dgvProductCreation.Rows[i].Cells["dgvtxtrate"].Value = dtbl.Rows[i]["rate"];
                    dgvProductCreation.Rows[i].Cells["dgvcmbtgodown"].Value = dtbl.Rows[i]["godownId"];
                    dgvProductCreation.Rows[i].Cells["dgvcmbrack"].Value = dtbl.Rows[i]["rackId"];
                    dgvProductCreation.Rows[i].Cells["dgvcmbUnit"].Value = dtbl.Rows[i]["unitId"];
                }
                foreach (DataGridViewRow dgvRowObj in dgvProductCreation.Rows)
                {
                    if (!dgvRowObj.IsNewRow)
                    {
                        DataGridViewCellEventArgs dgvArg = new DataGridViewCellEventArgs(0, dgvRowObj.Index);
                        CheckingForIncompleteRowInGrid(dgvArg);
                    }
                }
                dgvProductCreation.Focus();
                dgvProductCreation.CurrentCell = dgvProductCreation.Rows[0].Cells[0];
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:3" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill the DtaGridview With Batch While Updating
        /// </summary>
        public void OpeningStockGridWithBathFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ProductSP spProduct = new ProductSP();
                BatchSP spBatch = new BatchSP();
                cmbAllowBatch.SelectedIndex = 1;
                isBatchForUpdate = true;
                dtbl = spProduct.ProductViewGridFillFromBatch(decProductIdForEdit);
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    dgvProductCreation.Rows[i].Cells["dgvtxtbatchId"].Value = Convert.ToDecimal(dtbl.Rows[i]["batchId"]);
                    dgvProductCreation.Rows[i].Cells["dgvtxtExpDate"].Value = dtbl.Rows[i]["expiryDate"];
                    dgvProductCreation.Rows[i].Cells["dgvtxManfDate"].Value = dtbl.Rows[i]["manufacturingDate"];
                    dgvProductCreation.Rows[i].Cells["dgvtxtbatch"].Value = dtbl.Rows[i]["batchNo"];
                }
                foreach (DataGridViewRow dgvRowObj in dgvProductCreation.Rows)
                {
                    if (!dgvRowObj.IsNewRow)
                    {
                        DataGridViewCellEventArgs dgvArg = new DataGridViewCellEventArgs(0, dgvRowObj.Index);
                        CheckingForIncompleteRowInGrid(dgvArg);
                    }
                }
                dgvProductCreation.Focus();
                dgvProductCreation.CurrentCell = dgvProductCreation.Rows[0].Cells[1];
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:4" + ex.Message;
            }
        }
        /// <summary>
        /// Function to DeleteRowsFrom tbl_Stockposting While Updating
        /// </summary>
        public void DeleteOpeningStock()
        {
            try
            {
                StockPostingSP spStockposting = new StockPostingSP();
                spStockposting.StpDeleteForProductUpdation(decProductIdForEdit);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:5" + ex.Message;
            }
        }
        /// <summary>
        /// IF AllowBatch Selected No While Updating
        /// </summary>
        public void DeleteBatchFromOpeningStock()
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                spBatch.DeleteBatchForProductUpdate(decProductIdForEdit);
                isBatchRemoved = true;
                BatchWithBarCode();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:6" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check existence of batch no
        /// </summary>
        /// <param name="e"></param>
        public void BatchNoExistenceChecking(DataGridViewCellEventArgs e)
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                string strBatchNo = Convert.ToString(dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatch"].Value);
                decBatchId = Convert.ToDecimal(dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatchId"].Value);
                bool isOk = spBatch.BatchNameExistenceChecking(strBatchNo, decBatchId);
                if (!isOk)
                {
                    Messages.InformationMessage("Batch No already exist");
                    dgvProductCreation.CurrentCell = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatch"];
                    dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatch"].Value = "";
                    dgvProductCreation.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:7" + ex.Message;
            }
        }
        /// <summary>
        /// Function to confirm delete
        /// </summary>
        /// <param name="isDeleted"></param>
        public void ReciveDeleteConfirmation(bool isDeleted)
        {
            try
            {
                if (isDeleted)
                {
                    isDeletedConfirmed = isDeleted;
                    isBomFromRegister = false;//If bom deleted
                    cmbBom.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:8" + ex.Message;
            }
        }
        /// <summary>
        /// Function to confirm multiple unit delete
        /// </summary>
        /// <param name="isDeleted"></param>
        public void ReciveDeleteConfirmationFromMulUnit(bool isDeleted)
        {
            try
            {
                if (isDeleted)
                {
                    isDeletedConfirmedForMulUnit = isDeleted;
                    isMulUnitFromRgister = false;//if Multipleunite deleted
                    cmbMultipleUnit.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:9" + ex.Message;
            }
        }
        /// <summary>
        ///Function to Checking Bom Or MultipleUnit updated
        /// </summary>
        public void CheckWetherSaveOrEdit()
        {
            try
            {
                if (!isBomFromRegister && isSaveBomCheck)//if bom is adding while updating
                {
                    BomTableFill();
                }
                else
                {
                    if (cmbBom.SelectedIndex == 0 && isBomFromRegister)
                    {
                        BomDeleteFunction();
                    }
                }
                if (isBomUpdated)// if bom made any changes
                {
                    RemoveBom();
                    NewRowAddedForBom();//New row added while updating
                    BomTableEditFill();
                }
                if (isDeletedConfirmed)// If bom deleted
                {
                    BomDeleteFunction();
                }
                if (isMulUnitFromRgister && isSaveMulUnitCheck)
                {
                    MulUnitDeleteFunction();
                    UnitConvertionTableFill();
                }
                if (!isSaveMulUnitCheck && isCheck)
                {
                    MulUnitDeleteFunction();
                }
                if (!isMulUnitFromRgister && isSaveMulUnitCheck)
                {
                    UnitConvertionTableFill();
                }
                else
                {
                    if (cmbMultipleUnit.SelectedIndex == 0 && isMulUnitFromRgister)
                    {
                        MulUnitDeleteFunction();
                    }
                }
                if (isMulUnitUpdated)
                {
                    RemoveMulUnit();
                    NewRowAddedForMulUnit();//New Row added for multiple unit
                    UnitConvertionEditTableFill(); ;
                }
                if (isDeletedConfirmedForMulUnit)
                {
                    MulUnitDeleteFunction();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:10" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Edit tbl_Bom
        /// </summary>
        public void BomTableEditFill()
        {
            try
            {
                BomInfo infoBom = new BomInfo();
                BomSP spBom = new BomSP();
                for (int i = 0; i < dtblFromBomForUpdate.Rows.Count; i++)
                {
                    infoBom.RowmaterialId = Convert.ToDecimal(dtblFromBomForUpdate.Rows[i]["dgvcmbRawMaterial"]);
                    infoBom.UnitId = Convert.ToDecimal(dtblFromBomForUpdate.Rows[i]["dgvtxtUnitId"]);
                    infoBom.Quantity = Convert.ToDecimal(dtblFromBomForUpdate.Rows[i]["dgvtxtQty"]);
                    infoBom.Extra1 = dtblFromBomForUpdate.Rows[i]["extra1"].ToString();
                    infoBom.Extra2 = dtblFromBomForUpdate.Rows[i]["extra2"].ToString();
                    infoBom.ExtraDate = Convert.ToDateTime(dtblFromBomForUpdate.Rows[i]["extraDate"]);
                    infoBom.BomId = Convert.ToDecimal(dtblFromBomForUpdate.Rows[i]["bomId"]);
                    infoBom.ProductId = decProductIdForEdit;
                    spBom.UpdateBom(infoBom);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:11" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check data in Multiple Unit table
        /// </summary>
        public void BomDeleteFunction()
        {
            try
            {
                BomSP spBom = new BomSP();
                spBom.BomDeleteForUpdation(decProductIdForEdit);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:12" + ex.Message;
            }
        }
        /// <summary>
        /// Function to delete Multiple Unit
        /// </summary>
        public void MulUnitDeleteFunction()
        {
            try
            {
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                spUnitConvertion.MulUnitDeleteForUpdation(decProductIdForEdit);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:13" + ex.Message;
            }
        }
        /// <summary>
        /// Function to reset the form
        /// </summary>
        public void ClearFunction()
        {
            try
            {
                txtMaximumStock.Text = "0";
                txtMinimumStock.Text = "0";
                if (isAuto)
                {
                    txtProductCode.Text = string.Empty;
                    AutomaticCodeGenaration();
                }
                else
                {
                    txtProductCode.Text = string.Empty;
                }
                txtNarration.Text = string.Empty;
                txtName.Text = string.Empty;
                txtReorderLevel.Text = "0";
                txtSalesRate.Text = "0";
                txtPurchaseRate.Text = "0";
                txtMrp.Text = "0";
                txtPartNo.Text = string.Empty;
                cmbUnit.SelectedIndex = -1;
                cmbUnit.Enabled = true;
                cmbAllowBatch.SelectedIndex = 0;
                cmbBom.SelectedIndex = 0;
                cmbModalNo.SelectedIndex = 0;
                cmbDefaultGodown.SelectedIndex = 0;
                cmbMultipleUnit.SelectedIndex = 0;
                cmbDefaultRack.SelectedIndex = 0;
                cmbSize.SelectedIndex = 0;
                cmbGroup.SelectedIndex = -1;
                cmbBrand.SelectedIndex = 0;
                cmbTax.SelectedIndex = 0;
                cmbTaxApplicableOn.SelectedIndex = 0;
                cmbOpeningStock.SelectedIndex = 0;
                cbxReminder.Checked = false;
                dgvProductCreation.Rows.Clear();
                btnSave.Text = "Save";
                btnDelete.Enabled = false;
                txtName.Select();
                AutomaticCodeGenaration();
                InitialSettings();
                dtblBom = new DataTable();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:14" + ex.Message;
            }
        }
        /// <summary>
        /// Function to generate Product code automatically
        /// </summary>
        public void AutomaticCodeGenaration()
        {
            try
            {
                if (AutomaticProductCode())
                {
                    ProductSP spProduct = new ProductSP();
                    string strPcode = spProduct.ProductMax();
                    //int inCode = strPcode.Length;
                    //if (inCode == 3)
                    //{
                    //    strPcode = (strPcode.ToString()).PadLeft(4, '0');
                    //}
                    //else if (inCode == 2)
                    //{
                    //    strPcode = (strPcode.ToString()).PadLeft(4, '0');
                    //}
                    //else
                    //{
                    //    strPcode = (strPcode.ToString());
                    //}
                    txtProductCode.Text = strPcode;
                    txtProductCode.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:15" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check settings of ProductCode generation
        /// </summary>
        /// <returns></returns>
        public bool AutomaticProductCode()
        {
            try
            {
                SettingsSP spSetting = new SettingsSP();
                isAuto = spSetting.AutomaticProductCodeGeneration();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:16" + ex.Message;
            }
            return isAuto;
        }
        /// <summary>
        ///Function to  CheckInvalidEntries in DataGridView
        /// </summary>
        /// <param name="e"></param>
        public void CheckInvalidEntries(DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvProductCreation.CurrentRow != null)
                {
                    if (!isValueChanged)
                    {
                        if (((cmbAllowBatch.SelectedIndex == 1 && !dgvProductCreation.CurrentRow.IsNewRow) && (dgvProductCreation.CurrentRow.Cells["dgvtxManfDate"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvtxManfDate"].Value.ToString().Trim() == "")))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                            if (dgvProductCreation.Rows[0].Cells["dgvtxtrate"].Value == null || dgvProductCreation.Rows[0].Cells["dgvtxtrate"].Value.ToString().Trim() == "")
                            {
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                                dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                                dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                                dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                            }
                        }
                        else if (((cmbAllowBatch.SelectedIndex == 1 && !dgvProductCreation.CurrentRow.IsNewRow) && (dgvProductCreation.CurrentRow.Cells["dgvtxtExpDate"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvtxtExpDate"].Value.ToString().Trim() == "")))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (!dgvProductCreation.CurrentRow.IsNewRow && (dgvProductCreation.CurrentRow.Cells["dgvtxtqty"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvtxtqty"].Value.ToString().Trim() == ""))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (!dgvProductCreation.CurrentRow.IsNewRow && (dgvProductCreation.CurrentRow.Cells["dgvtxtrate"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvtxtrate"].Value.ToString().Trim() == ""))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                            if (dgvProductCreation.Rows[0].Cells["dgvtxtrate"].Value == null || dgvProductCreation.Rows[0].Cells["dgvtxtrate"].Value.ToString().Trim() == "")
                            {
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                                dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                                dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                                dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                            }
                        }
                        else if ((cmbAllowBatch.SelectedIndex == 1 && !dgvProductCreation.CurrentRow.IsNewRow && (dgvProductCreation.CurrentRow.Cells["dgvtxtbatch"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvtxtbatch"].Value.ToString().Trim() == "")))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if ((dgvProductCreation.Columns["dgvcmbtgodown"].Visible && !dgvProductCreation.CurrentRow.IsNewRow) && (Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value) == 0 || dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value.ToString().Trim() == ""))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if ((dgvProductCreation.Columns["dgvcmbrack"].Visible && !dgvProductCreation.CurrentRow.IsNewRow) && (Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbrack"].Value) == 0 || dgvProductCreation.CurrentRow.Cells["dgvcmbrack"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvcmbrack"].Value.ToString().Trim() == ""))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if ((!dgvProductCreation.CurrentRow.IsNewRow) && (Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbUnit"].Value) == 0 || dgvProductCreation.CurrentRow.Cells["dgvcmbUnit"].Value == null || dgvProductCreation.CurrentRow.Cells["dgvcmbUnit"].Value.ToString().Trim() == string.Empty))
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.CurrentRow.HeaderCell.Value = "X";
                            dgvProductCreation.CurrentRow.HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            isValueChanged = true;
                            dgvProductCreation.CurrentRow.Cells["dgvtxtCheck"].Value = "";
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "";
                        }
                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:17" + ex.Message;
            }
        }
        /// <summary>
        /// Function to add data to StockTable With Batch
        /// </summary>
        public void BatchTableWithStockAndProductBatchFill()
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                BatchInfo infoBatch = new BatchInfo();
                StockPostingSP spStockPosting = new StockPostingSP();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                {
                    infoBatch.ManufacturingDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxManfDate"].Value);
                    infoBatch.ExpiryDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxtExpDate"].Value);
                    infoBatch.BatchNo = dgvProductCreation.Rows[inI].Cells["dgvtxtbatch"].Value.ToString();
                    if (btnSave.Text == "Update")
                    {
                        infoBatch.ProductId = decProductIdForEdit;
                    }
                    else
                    {
                        infoBatch.ProductId = decSaveProduct;
                    }
                    infoBatch.Extra1 = string.Empty;
                    infoBatch.Extra2 = string.Empty;
                    infoBatch.ExtraDate = DateTime.Now;
                    infoBatch.narration = string.Empty;
                    infoBatch.barcode = Convert.ToString(spBatch.AutomaticBarcodeGeneration());
                    decBatchId = spBatch.BatchAddReturnIdentity(infoBatch);
                    infoStockPosting.AgainstInvoiceNo = string.Empty;
                    infoStockPosting.AgainstVoucherNo = string.Empty;
                    infoStockPosting.Date = PublicVariables._dtCurrentDate;
                    infoStockPosting.AgainstVoucherTypeId = 0;
                    infoStockPosting.InvoiceNo = Convert.ToString(decSaveProduct);
                    infoStockPosting.VoucherNo = Convert.ToString(decSaveProduct);
                    infoStockPosting.VoucherTypeId = 2;
                    infoStockPosting.UnitId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbUnit"].Value);
                    infoStockPosting.InwardQty = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtqty"].Value);
                    infoStockPosting.OutwardQty = 0;
                    infoStockPosting.Rate = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].Value);
                    infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                    infoStockPosting.Extra1 = string.Empty;
                    infoStockPosting.Extra2 = string.Empty;
                    infoStockPosting.ExtraDate = DateTime.Now;
                    if ((dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Visible) == false)
                    {
                        infoStockPosting.RackId = 1;
                    }
                    else
                    {
                        infoStockPosting.RackId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Value);
                    }
                    if ((dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Visible) == false)
                    {
                        infoStockPosting.GodownId = 1;
                    }
                    else
                    {
                        infoStockPosting.GodownId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Value);
                    }
                    if (btnSave.Text == "Update")
                    {
                        infoStockPosting.ProductId = decProductIdForEdit;
                    }
                    else
                    {
                        infoStockPosting.ProductId = decSaveProduct;
                    }
                    if (cmbAllowBatch.SelectedIndex == 0)
                    {
                        infoStockPosting.BatchId = 0;
                    }
                    else
                    {
                        infoStockPosting.BatchId = decBatchId;
                    }
                    decCheck = spStockPosting.StockPostingAdd(infoStockPosting);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:18" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save data to BatchTable While Updating
        /// </summary>
        public void BatchTableEditFill()
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                BatchInfo infoBatch = new BatchInfo();
                for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                {
                    infoBatch.BatchId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtbatchId"].Value);
                    infoBatch.ManufacturingDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxManfDate"].Value);
                    infoBatch.ExpiryDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxtExpDate"].Value);
                    infoBatch.BatchNo = dgvProductCreation.Rows[inI].Cells["dgvtxtbatch"].Value.ToString();
                    infoBatch.ProductId = decProductIdForEdit;
                    infoBatch.Extra1 = string.Empty;
                    infoBatch.Extra2 = string.Empty;
                    infoBatch.ExtraDate = DateTime.Now;
                    infoBatch.narration = string.Empty;
                    isBatchUpdated = spBatch.BatchEditForProductEdit(infoBatch);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:19" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from Salesreturn form
        /// </summary>
        /// <param name="frmSalesReturn"></param>
        public void CallFromSalesReturn(frmSalesReturn frmSalesReturn)
        {
            try
            {
                base.Show();
                this.frmSalesReturnObj = frmSalesReturn;
                frmSalesReturn.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:20" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from Stock Journal
        /// </summary>
        /// <param name="frmSalesReturn"></param>
        public void CallFromStockJournal(frmStockJournal frmStockJournal)
        {
            try
            {
                if (IsdgvConsumption)
                {
                    frmStockJournal.Enabled = false;
                    this.frmStockJournalObj = frmStockJournal;
                    base.Show();
                }
                else
                {
                    frmStockJournal.Enabled = false;
                    this.frmStockJournalObj = frmStockJournal;
                    base.Show();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:21" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Edit tbl_Bom
        /// </summary>
        public void BomTableForUpdate()
        {
            try
            {
                BomSP spBom = new BomSP();
                DataTable dtblBom = new DataTable();
                dtblBom = spBom.ProduBomForEdit(decProductIdForEdit);
                dtblBomForEdit = dtblBom;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:22" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Edit tbl_unitconvertion
        /// </summary>
        public void MultipleUnitTableForUpdate()
        {
            try
            {
                UnitConvertionSP spUnitConverstions = new UnitConvertionSP();
                DataTable dtblMulUnit = new DataTable();
                dtblMulUnit = spUnitConverstions.UnitConverstionTableForEdit(decProductIdForEdit);
                dtblMulUnitForEdit = dtblMulUnit;
                if (cmbOpeningStock.SelectedIndex == 1 && dtblForUnitIdInOpeNingStock != null && dtblForUnitIdInOpeNingStock.Rows.Count > 0)
                {
                    dgvcmbUnit.DataSource = dtblForUnitIdInOpeNingStock;
                    dgvcmbUnit.DisplayMember = "unitName";
                    dgvcmbUnit.ValueMember = "dgvtxtunitId";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:23" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save data to StockPosting Table
        /// </summary>
        public void StockPostingTableFill()
        {
            try
            {
                StockPostingSP spStockPosting = new StockPostingSP();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                {
                    infoStockPosting.AgainstInvoiceNo = string.Empty;
                    infoStockPosting.AgainstVoucherNo = string.Empty;
                    infoStockPosting.Date = PublicVariables._dtCurrentDate;
                    infoStockPosting.AgainstVoucherTypeId = 0;
                    infoStockPosting.InvoiceNo = Convert.ToString(decSaveProduct);
                    infoStockPosting.VoucherNo = Convert.ToString(decSaveProduct);
                    infoStockPosting.VoucherTypeId = 2;
                    infoStockPosting.UnitId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbUnit"].Value);
                    infoStockPosting.InwardQty = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtqty"].Value);
                    infoStockPosting.OutwardQty = 0;
                    infoStockPosting.Rate = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].Value);
                    infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                    infoStockPosting.Extra1 = string.Empty;
                    infoStockPosting.Extra2 = string.Empty;
                    infoStockPosting.ExtraDate = DateTime.Now;
                    if ((dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Visible) == false)
                    {
                        infoStockPosting.RackId = 0;
                    }
                    else
                    {
                        infoStockPosting.RackId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Value);
                    }
                    if ((dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Visible) == false)
                    {
                        infoStockPosting.GodownId = 1;
                    }
                    else
                    {
                        infoStockPosting.GodownId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Value);
                    }
                    if (btnSave.Text == "Update")
                    {
                        infoStockPosting.ProductId = decProductIdForEdit;
                    }
                    else
                    {
                        infoStockPosting.ProductId = decSaveProduct;
                    }
                    if (cmbAllowBatch.SelectedIndex == 0)
                    {
                        infoStockPosting.BatchId = inBatchIdWithPartNoNA;
                    }
                    else
                    {
                        infoStockPosting.BatchId = decBatchId;
                    }
                    decCheck = spStockPosting.StockPostingAdd(infoStockPosting);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:24" + ex.Message;
            }
        }
        /// <summary>
        /// Function to Edit stockposting table
        /// </summary>
        public void StockPostingTableEditFill()
        {
            try
            {
                StockPostingSP spStockPosting = new StockPostingSP();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                BatchSP spBatch = new BatchSP();
                for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                {
                    infoStockPosting.StockPostingId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtstockpostId"].Value);
                    infoStockPosting.AgainstInvoiceNo = string.Empty;
                    infoStockPosting.AgainstVoucherNo = string.Empty;
                    infoStockPosting.Date = PublicVariables._dtCurrentDate;
                    infoStockPosting.AgainstVoucherTypeId = 0;
                    infoStockPosting.InvoiceNo = Convert.ToString(decProductIdForEdit);
                    infoStockPosting.VoucherNo = Convert.ToString(decProductIdForEdit);
                    infoStockPosting.VoucherTypeId = 2;
                    infoStockPosting.UnitId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbUnit"].Value);
                    infoStockPosting.InwardQty = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtqty"].Value);
                    infoStockPosting.OutwardQty = 0;
                    infoStockPosting.ProductId = decProductIdForEdit;
                    infoStockPosting.Rate = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].Value);
                    infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                    infoStockPosting.Extra1 = string.Empty;
                    infoStockPosting.Extra2 = string.Empty;
                    infoStockPosting.ExtraDate = DateTime.Now;
                    if (!dgvProductCreation.Columns["dgvcmbrack"].Visible)
                    {
                        infoStockPosting.RackId = 1;
                    }
                    else
                    {
                        infoStockPosting.RackId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Value);
                    }
                    if (!dgvProductCreation.Columns["dgvcmbtgodown"].Visible)
                    {
                        infoStockPosting.GodownId = 1;
                    }
                    else
                    {
                        infoStockPosting.GodownId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Value);
                    }
                    if (cmbAllowBatch.SelectedIndex == 0)
                    {
                        if (isBatchRemoved)
                        {
                            infoStockPosting.BatchId = spBatch.BatchIdForStockPosting(decProductIdForEdit);
                        }
                        else
                        {
                            int inId = spStockPosting.ReturnBatchIdFromStockPosting(decProductIdForEdit);
                            infoStockPosting.BatchId = inId;
                        }
                    }
                    else
                    {
                        infoStockPosting.BatchId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtbatchId"].Value);
                    }
                    isStpUpdated = spStockPosting.StockPostingEdit(infoStockPosting);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:25" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save data to product table
        /// </summary>
        public void ProductTableFill()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                ProductInfo infoProduct = new ProductInfo();
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                UnitConvertionInfo infoUnitConvertion = new UnitConvertionInfo();
                infoProduct.ProductName = txtName.Text.Trim();
                infoProduct.ProductCode = txtProductCode.Text.Trim();
                if (txtPurchaseRate.Text != string.Empty)
                {
                    infoProduct.PurchaseRate = Convert.ToDecimal(txtPurchaseRate.Text.Trim());
                }
                else
                {
                    infoProduct.PurchaseRate = 0;
                }
                if (txtSalesRate.Text != string.Empty)
                {
                    infoProduct.SalesRate = Convert.ToDecimal(txtSalesRate.Text.Trim());
                }
                else
                {
                    infoProduct.SalesRate = 0;
                }
                if (txtMrp.Text != string.Empty)
                {
                    infoProduct.Mrp = Convert.ToDecimal(txtMrp.Text.Trim());
                }
                else
                {
                    infoProduct.Mrp = 0;
                }
                infoProduct.MaximumStock = Convert.ToDecimal(txtMaximumStock.Text.Trim());
                infoProduct.MinimumStock = Convert.ToDecimal(txtMinimumStock.Text.Trim());
                infoProduct.ReorderLevel = Convert.ToDecimal(txtReorderLevel.Text.Trim());
                infoProduct.Extra1 = string.Empty;
                infoProduct.Extra2 = string.Empty;
                infoProduct.ExtraDate = PublicVariables._dtCurrentDate;
                infoProduct.TaxId = Convert.ToDecimal(cmbTax.SelectedValue);
                infoProduct.UnitId = Convert.ToDecimal(cmbUnit.SelectedValue);
                infoProduct.GroupId = Convert.ToDecimal(cmbGroup.SelectedValue);
                infoProduct.Narration = txtNarration.Text;
                infoUnitConvertion.UnitId = Convert.ToDecimal(cmbUnit.SelectedValue);
                infoUnitConvertion.ConversionRate = 1;
                infoUnitConvertion.Quantities = string.Empty;
                infoUnitConvertion.Extra1 = string.Empty;
                infoUnitConvertion.Extra2 = string.Empty;
                infoUnitConvertion.ExtraDate = DateTime.Now;
                if (cmbTax.SelectedIndex == 0)
                {
                    infoProduct.TaxapplicableOn = "Rate";
                }
                else
                {
                    infoProduct.TaxapplicableOn = Convert.ToString(cmbTaxApplicableOn.SelectedItem);
                }
                if (cmbBrand.SelectedIndex != -1)
                {
                    infoProduct.BrandId = Convert.ToDecimal(cmbBrand.SelectedValue);
                }
                else
                {
                    infoProduct.BrandId = 1;
                }
                if (cmbSize.SelectedIndex != -1)
                {
                    infoProduct.SizeId = Convert.ToDecimal(cmbSize.SelectedValue);
                }
                else
                {
                    infoProduct.SizeId = 1;
                }
                if (cmbModalNo.SelectedIndex != -1)
                {
                    infoProduct.ModelNoId = Convert.ToDecimal(cmbModalNo.SelectedValue);
                }
                else
                {
                    infoProduct.ModelNoId = 1;
                }
                if (cmbDefaultGodown.SelectedIndex != -1)
                {
                    infoProduct.GodownId = Convert.ToDecimal(cmbDefaultGodown.SelectedValue);
                }
                else
                {
                    infoProduct.GodownId = 1;
                }
                if (cmbDefaultRack.SelectedIndex != -1)
                {
                    infoProduct.RackId = Convert.ToDecimal(cmbDefaultRack.SelectedValue);
                }
                else
                {
                    infoProduct.RackId = 1;
                }
                if (cmbAllowBatch.SelectedIndex == 0)
                {
                    infoProduct.IsallowBatch = false;
                }
                else
                {
                    infoProduct.IsallowBatch = true;
                }
                if (cmbBom.SelectedIndex == 0)
                {
                    infoProduct.IsBom = false;
                }
                else
                {
                    if (isSaveBomCheck)
                    {
                        infoProduct.IsBom = true;
                    }
                    else
                    {
                        infoProduct.IsBom = false;
                    }
                }
                if (cmbOpeningStock.SelectedIndex == 0)
                {
                    infoProduct.Isopeningstock = false;
                }
                else
                {
                    infoProduct.Isopeningstock = true;
                    infoProduct.PurchaseRate = Convert.ToDecimal(dgvProductCreation.Rows[0].Cells["dgvtxtrate"].Value);
                }
                if (cmbMultipleUnit.SelectedIndex == 0)
                {
                    infoProduct.Ismultipleunit = false;
                }
                else
                {
                    if (isSaveMulUnitCheck)
                    {
                        infoProduct.Ismultipleunit = true;
                    }
                    else
                    {
                        infoProduct.Ismultipleunit = false;
                    }
                }
                if (cbxActive.Checked)
                {
                    infoProduct.IsActive = true;
                }
                else
                {
                    infoProduct.IsActive = false;
                }
                if (cbxReminder.Checked)
                {
                    infoProduct.IsshowRemember = true;
                }
                else
                {
                    infoProduct.IsshowRemember = false;
                }
                decSaveProduct = spProduct.ProductAdd(infoProduct);
                infoUnitConvertion.ProductId = decSaveProduct;
                spUnitConvertion.UnitConvertionAdd(infoUnitConvertion);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:26" + ex.Message;
            }
        }
        /// <summary>
        /// Function to edit Product table
        /// </summary>
        public void ProductEditFill()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                ProductInfo infoProduct = new ProductInfo();
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                UnitConvertionInfo infoUnitConvertion = new UnitConvertionInfo();
                BatchSP spBatch = new BatchSP();
                infoProduct.ProductName = txtName.Text.Trim();
                infoProduct.ProductCode = txtProductCode.Text.Trim();
                if (txtPurchaseRate.Text.Trim() == string.Empty)
                {
                    infoProduct.PurchaseRate = 0;
                }
                else
                {
                    infoProduct.PurchaseRate = Convert.ToDecimal(txtPurchaseRate.Text.Trim());
                }
                if (txtSalesRate.Text.Trim() == string.Empty)
                {
                    infoProduct.SalesRate = 0;
                }
                else
                {
                    infoProduct.SalesRate = Convert.ToDecimal(txtSalesRate.Text.Trim());
                }
                if (txtMrp.Text.Trim() == string.Empty)
                {
                    infoProduct.Mrp = 0;
                }
                else
                {
                    infoProduct.Mrp = Convert.ToDecimal(txtMrp.Text);
                }
                infoProduct.MaximumStock = Convert.ToDecimal(txtMaximumStock.Text.Trim());
                infoProduct.MinimumStock = Convert.ToDecimal(txtMinimumStock.Text.Trim());
                infoProduct.ReorderLevel = Convert.ToDecimal(txtReorderLevel.Text.Trim());
                infoProduct.Extra1 = string.Empty;
                infoProduct.Extra2 = string.Empty;
                infoProduct.ExtraDate = PublicVariables._dtCurrentDate;
                infoProduct.TaxId = Convert.ToDecimal(cmbTax.SelectedValue);
                infoProduct.UnitId = Convert.ToDecimal(cmbUnit.SelectedValue);
                infoProduct.GroupId = Convert.ToDecimal(cmbGroup.SelectedValue);
                infoProduct.Narration = txtNarration.Text;
                infoProduct.ProductId = decProductIdForEdit;
                infoUnitConvertion.ProductId = decProductIdForEdit;
                infoUnitConvertion.UnitId = Convert.ToDecimal(cmbUnit.SelectedValue);
                if (cmbTax.SelectedIndex == 0)
                {
                    infoProduct.TaxapplicableOn = string.Empty;
                }
                else
                {
                    infoProduct.TaxapplicableOn = Convert.ToString(cmbTaxApplicableOn.SelectedItem);
                }
                if (cmbBrand.SelectedIndex != -1)
                {
                    infoProduct.BrandId = Convert.ToDecimal(cmbBrand.SelectedValue);
                }
                else
                {
                    infoProduct.BrandId = 1;
                }
                if (cmbSize.SelectedIndex != -1)
                {
                    infoProduct.SizeId = Convert.ToDecimal(cmbSize.SelectedValue);
                }
                else
                {
                    infoProduct.SizeId = 1;
                }
                if (cmbModalNo.SelectedIndex != -1)
                {
                    infoProduct.ModelNoId = Convert.ToDecimal(cmbModalNo.SelectedValue);
                }
                else
                {
                    infoProduct.ModelNoId = 1;
                }
                if (cmbDefaultGodown.SelectedIndex != -1)
                {
                    infoProduct.GodownId = Convert.ToDecimal(cmbDefaultGodown.SelectedValue);
                }
                else
                {
                    infoProduct.GodownId = 1;
                }
                if (cmbDefaultRack.SelectedIndex != -1)
                {
                    infoProduct.RackId = Convert.ToDecimal(cmbDefaultRack.SelectedValue);
                }
                else
                {
                    infoProduct.RackId = 1;
                }
                if (cmbAllowBatch.SelectedIndex == 0)
                {
                    infoProduct.IsallowBatch = false;
                    spBatch.PartNoUpdate(decProductIdForEdit, txtPartNo.Text.Trim());
                }
                else
                {
                    infoProduct.IsallowBatch = true;
                }
                if (cmbBom.SelectedIndex == 0)
                {
                    infoProduct.IsBom = false;
                }
                else
                {
                    if (isSaveBomCheck || isBomFromRegister)
                    {
                        infoProduct.IsBom = true;
                    }
                    else
                    {
                        infoProduct.IsBom = false;
                    }
                }
                if (cmbMultipleUnit.SelectedIndex == 0)
                {
                    infoProduct.Ismultipleunit = false;
                }
                else
                {
                    if ((isMulUnitFromRgister || isSaveMulUnitCheck))
                    {
                        infoProduct.Ismultipleunit = true;
                    }
                    else
                    {
                        infoProduct.Ismultipleunit = false;
                    }
                }
                if (cbxActive.Checked)
                {
                    infoProduct.IsActive = true;
                }
                else
                {
                    infoProduct.IsActive = false;
                }
                if (cbxReminder.Checked)
                {
                    infoProduct.IsshowRemember = true;
                }
                else
                {
                    infoProduct.IsshowRemember = false;
                }
                if (cmbOpeningStock.SelectedIndex == 0)
                {
                    infoProduct.Isopeningstock = false;
                }
                else
                {
                    infoProduct.Isopeningstock = true;
                }
                if (PublicVariables.isMessageEdit)
                {
                    if (Messages.UpdateMessage())
                    {
                        isUpdated = spProduct.ProductEdit(infoProduct);
                        spUnitConvertion.UnitConverstionEditWhenProductUpdating(infoUnitConvertion);
                    }
                }
                else
                {
                    isUpdated = spProduct.ProductEdit(infoProduct);
                    spUnitConvertion.UnitConverstionEditWhenProductUpdating(infoUnitConvertion);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:27" + ex.Message;
            }
        }
        /// <summary>
        /// Function to get multiple unit datatable from frmProductMultipleUnit
        /// </summary>
        /// <param name="dtblFromMulUnit"></param>
        /// <param name="isSaveMulUnit"></param>
        /// <param name="decUnitIdSelected"></param>
        public void DataTableReturnFromMultipleUnit(DataTable dtblFromMulUnit, bool isSaveMulUnit, decimal decUnitIdSelected)
        {
            try
            {
                isSaveMulUnitCheck = isSaveMulUnit;
                dtblMulUnit = dtblFromMulUnit;
                decUnitIdSelectedWhenMulUntCalled = decUnitIdSelected;
                dtblForUnitIdInOpeNingStock = dtblMulUnit;
                if (cmbOpeningStock.SelectedIndex == 1 && dtblForUnitIdInOpeNingStock != null && dtblForUnitIdInOpeNingStock.Rows.Count > 0)
                {
                    dgvcmbUnit.DataSource = dtblForUnitIdInOpeNingStock;
                    dgvcmbUnit.DisplayMember = "unitName";
                    dgvcmbUnit.ValueMember = "dgvtxtunitId";
                    for (int i = 0; i < dgvProductCreation.Rows.Count; i++)
                    {
                        if (!dgvProductCreation.Rows[i].IsNewRow)
                        {
                            dgvProductCreation.Rows[i].Cells["dgvcmbUnit"].Value = dtblForUnitIdInOpeNingStock.Rows[0][1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:28" + ex.Message;
            }
        }
        /// <summary>
        /// Function to add multiple unit details to tbl_UnitConversion table
        /// </summary>
        public void UnitConvertionTableFill()
        {
            try
            {
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                UnitConvertionInfo infoUnitConversion = new UnitConvertionInfo();
                decimal decProductId = decSaveProduct;
                if (btnSave.Text == "Update")
                {
                    decProductId = decProductIdForEdit;
                }
                else
                {
                    decProductId = decSaveProduct;
                }
                for (int i = 0; i < dtblMulUnit.Rows.Count; i++)
                {
                    decimal decConversionRate = Convert.ToDecimal(dtblMulUnit.Rows[i]["CnvertionRate"]);
                    if (decConversionRate != 0)
                    {
                        infoUnitConversion.ConversionRate = Convert.ToDecimal(dtblMulUnit.Rows[i]["CnvertionRate"]);
                        infoUnitConversion.UnitId = Convert.ToDecimal(dtblMulUnit.Rows[i]["dgvtxtUnitId"]);
                        infoUnitConversion.Quantities = Convert.ToString(dtblMulUnit.Rows[i]["quantities"]);
                        infoUnitConversion.Extra1 = dtblMulUnit.Rows[i]["extra1"].ToString();
                        infoUnitConversion.Extra2 = dtblMulUnit.Rows[i]["extra2"].ToString();
                        infoUnitConversion.ExtraDate = Convert.ToDateTime(dtblMulUnit.Rows[i]["extraDate"]);
                        infoUnitConversion.ProductId = decProductId;
                        spUnitConvertion.UnitConvertionAdd(infoUnitConversion);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:29" + ex.Message;
            }
        }
        /// <summary>
        /// Function to update multiple unit details to tbl_UnitConversion table
        /// </summary>
        public void UnitConvertionEditTableFill()
        {
            try
            {
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                UnitConvertionInfo infoUnitConversion = new UnitConvertionInfo();
                for (int i = 0; i < dtblFromMulUnitForUpdate.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[i]["unitconvertionId"]) != 0)
                    {
                        infoUnitConversion.ConversionRate = Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[i]["CnvertionRate"]);
                        infoUnitConversion.UnitId = Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[i]["dgvtxtUnitId"]);
                        infoUnitConversion.Extra1 = dtblFromMulUnitForUpdate.Rows[i]["extra1"].ToString();
                        infoUnitConversion.Extra2 = dtblFromMulUnitForUpdate.Rows[i]["extra2"].ToString();
                        infoUnitConversion.ExtraDate = Convert.ToDateTime(dtblFromMulUnitForUpdate.Rows[i]["extraDate"]);
                        infoUnitConversion.UnitconvertionId = Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[i]["unitconvertionId"]);
                        infoUnitConversion.ProductId = decProductIdForEdit;
                        spUnitConvertion.UnitConvertionEdit(infoUnitConversion);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:30" + ex.Message;
            }
        }
        /// <summary>
        /// Function to remove incomplete row from grid
        /// </summary>
        /// <returns></returns>
        public bool RemoveIncompleteRowsFromGrid()
        {
            bool isOk = true;
            try
            {
                string strMessage = "Rows";
                int inC = 0, inForFirst = 0;
                int inRowcount = dgvProductCreation.RowCount;
                int inLastRow = 1;//To eliminate last row from checking
                foreach (DataGridViewRow dgvrowCur in dgvProductCreation.Rows)
                {
                    if (inLastRow < inRowcount)
                    {
                        if (dgvrowCur.Cells["dgvtxtCheck"].Value != null && dgvrowCur.Cells["dgvtxtCheck"].Value.ToString() == "X")
                        {
                            isOk = false;
                            if (inC == 0)
                            {
                                strMessage = strMessage + Convert.ToString(dgvrowCur.Index + 1);
                                inForFirst = dgvrowCur.Index;
                                inC++;
                            }
                            else
                            {
                                strMessage = strMessage + ", " + Convert.ToString(dgvrowCur.Index + 1);
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
                        for (int inK = 0; inK < dgvProductCreation.Rows.Count; inK++)
                        {
                            if (!dgvProductCreation.Rows[inK].IsNewRow)
                            {
                                if (dgvProductCreation.Rows[inK].Cells["dgvtxtCheck"].Value != null && dgvProductCreation.Rows[inK].Cells["dgvtxtCheck"].Value.ToString() == "X")// && dgvProductBOM.Rows[inK].Cells["dgvtxtCheck"].Value.ToString() == "x")
                                {
                                    if (!dgvProductCreation.Rows[inK].IsNewRow)
                                    {
                                        if (dgvProductCreation.Rows[inK].Cells["dgvtxtstockpostId"].Value != null && Convert.ToDecimal(dgvProductCreation.Rows[inK].Cells["dgvtxtstockpostId"].Value) != 0)
                                        {
                                            lstArrOfRemoveFromStockPosting.Add(dgvProductCreation.Rows[inK].Cells["dgvtxtstockpostId"].Value.ToString());
                                            if (dgvProductCreation.Rows[inK].Cells["dgvtxtbatchId"].Value != null && Convert.ToDecimal(dgvProductCreation.Rows[inK].Cells["dgvtxtbatchId"].Value) != 0)
                                            {
                                                lstArrOfRemoveFromBatchTable.Add(dgvProductCreation.Rows[inK].Cells["dgvtxtbatchId"].Value.ToString());
                                            }
                                            dgvProductCreation.Rows.RemoveAt(inK);
                                        }
                                        else
                                        {
                                            dgvProductCreation.Rows.RemoveAt(inK);
                                        }
                                        inK--;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        dgvProductCreation.Select();
                        dgvProductCreation.CurrentCell = dgvProductCreation.Rows[inForFirst].Cells["dgvtxtSlNo"];
                    }
                }
                if (dgvProductCreation.Rows.Count == 1)
                {
                    isOk = false;
                    Messages.InformationMessage("There is no row to save in opening stock");
                    dgvProductCreation.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:31" + ex.Message;
            }
            return isOk;
        }
        /// <summary>
        /// Function to edit stock posting
        /// </summary>
        public void StockPostingTableFillWhileUpdating()
        {
            try
            {
                StockPostingSP spStockPosting = new StockPostingSP();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                {
                    infoStockPosting.AgainstInvoiceNo = string.Empty;
                    infoStockPosting.AgainstVoucherNo = string.Empty;
                    infoStockPosting.Date = PublicVariables._dtCurrentDate;
                    infoStockPosting.AgainstVoucherTypeId = 0;
                    infoStockPosting.InvoiceNo = Convert.ToString(decProductIdForEdit);
                    infoStockPosting.VoucherNo = Convert.ToString(decProductIdForEdit);
                    infoStockPosting.VoucherTypeId = 2;
                    infoStockPosting.UnitId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbUnit"].Value);
                    infoStockPosting.InwardQty = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtqty"].Value);
                    infoStockPosting.OutwardQty = 0;
                    infoStockPosting.Rate = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].Value);
                    infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                    infoStockPosting.Extra1 = string.Empty;
                    infoStockPosting.Extra2 = string.Empty;
                    infoStockPosting.ExtraDate = DateTime.Now;
                    infoStockPosting.ProductId = decProductIdForEdit;
                    if ((dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Visible) == false)
                    {
                        infoStockPosting.RackId = 0;
                    }
                    else
                    {
                        infoStockPosting.RackId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbrack"].Value);
                    }
                    if ((dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Visible) == false)
                    {
                        infoStockPosting.GodownId = 0;
                    }
                    else
                    {
                        infoStockPosting.GodownId = Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvcmbtgodown"].Value);
                    }
                    if (cmbAllowBatch.SelectedIndex == 0)
                    {
                        infoStockPosting.BatchId = 0;
                    }
                    else
                    {
                        infoStockPosting.BatchId = decBatchId;
                    }
                    decCheck = spStockPosting.StockPostingAdd(infoStockPosting);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:32" + ex.Message;
            }
        }
        /// <summary>
        /// Function for serial number generation
        /// </summary>
        public void SlNo()
        {
            try
            {
                int inRowNo = 1;
                foreach (DataGridViewRow dr in dgvProductCreation.Rows)
                {
                    dr.Cells["dgvtxtSlNo"].Value = inRowNo;
                    inRowNo++;
                    if (dr.Index == dgvProductCreation.Rows.Count - 2)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:33" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill godown combobox in grid
        /// </summary>
        public void GodownGrigComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                GodownSP spGodown = new GodownSP();
                dtbl = spGodown.GodownViewAll();
                DataRow drow = dtbl.NewRow();
                drow["godownName"] = string.Empty;
                drow["godownId"] = 0;
                dtbl.Rows.InsertAt(drow, 0);
                dgvcmbtgodown.DataSource = dtbl;
                dgvcmbtgodown.DisplayMember = "godownName";
                dgvcmbtgodown.ValueMember = "godownId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:34" + ex.Message;
            }
        }
        /// <summary>
        /// Function fill the new created brand from brand form
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromBrandForm(decimal decId)
        {
            try
            {
                BrandComboFill();
                if (decId.ToString() != "0")
                {
                    cmbBrand.SelectedValue = decId;
                }
                else if (strBrandName != string.Empty)
                {
                    cmbBrand.SelectedValue = strBrandName;
                }
                else
                {
                    cmbBrand.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbBrand.Focus();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:35" + ex.Message;
            }
        }
        /// <summary>
        /// Function to set initial values
        /// </summary>
        public void InitialValues()
        {
            try
            {
                txtMinimumStock.Text = "0";
                txtMrp.Text = "0";
                txtReorderLevel.Text = "0";
                txtSalesRate.Text = "0";
                txtPurchaseRate.Text = "0";
                txtMaximumStock.Text = "0";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:36" + ex.Message;
            }
        }
        //--------------------------------------------------------------ReturnFromFormSize---------------------------- 
        /// <summary>
        /// Function to fill size combobox while return from size when creating new size 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromSizeForm(decimal decId)
        {
            try
            {
                SizeComboFill();
                if (decId.ToString() != "0")
                {
                    cmbSize.SelectedValue = decId;
                }
                else if (strSizeName != string.Empty)
                {
                    cmbSize.SelectedValue = strSizeName;
                }
                else
                {
                    cmbSize.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbSize.Focus();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:37" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill tax combobox while return from tax when creating new tax 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromTaxForm(decimal decId)//Form Tax
        {
            try
            {
                TaxComboFill();
                if (decId.ToString() != "0")
                {
                    cmbTax.SelectedValue = decId;
                }
                else if (strTaxName != string.Empty)
                {
                    cmbTax.SelectedValue = strTaxName;
                }
                else
                {
                    cmbTax.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbTax.Focus();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:38" + ex.Message;
            }
        }
        //--------------------------------------------------------------ReturnFromGodownForm---------------------------- 
        /// <summary>
        /// Function to fill godown combobox while return from godown when creating new godown 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromGodownForm(decimal decId)
        {
            try
            {
                if (!isCallFromGrid)
                {
                    GodownComboFill();
                    if (decId.ToString() != "0")
                    {
                        cmbDefaultGodown.SelectedValue = decId;
                    }
                    else if (strGodownName != string.Empty)
                    {
                        cmbDefaultGodown.SelectedValue = strGodownName;
                    }
                    else
                    {
                        cmbDefaultGodown.SelectedIndex = -1;
                    }
                    this.Enabled = true;
                    cmbDefaultGodown.Focus();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
                else
                {
                    DataGridViewComboBoxCell dgvGodownCell = (DataGridViewComboBoxCell)dgvProductCreation[dgvProductCreation.Columns["dgvcmbtgodown"].Index, dgvProductCreation.CurrentRow.Index];
                    DataGridViewComboBoxCell dgvRackCell = (DataGridViewComboBoxCell)dgvProductCreation[dgvProductCreation.Columns["dgvcmbrack"].Index, dgvProductCreation.CurrentRow.Index];
                    DataTable dtbl = new DataTable();
                    GodownSP spGodown = new GodownSP();
                    dtbl = spGodown.GodownViewAll();
                    dgvcmbtgodown.DataSource = dtbl;
                    if (decId.ToString() != "0")
                    {
                        dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value = decId;
                        dgvRackCell.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:39" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Unit combobox while return from Unit when creating new Unit 
        /// </summary>
        /// <param name="decId"></param>
        public void ReturnFromUnitForm(decimal decId)//Form Unit
        {
            try
            {
                UnitComboFill();
                if (decId.ToString() != "0")
                {
                    cmbUnit.SelectedValue = decId;
                }
                else if (strUnitName != string.Empty)
                {
                    cmbUnit.SelectedValue = strUnitName;
                }
                else
                {
                    cmbUnit.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbUnit.Focus();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:40" + ex.Message;
            }
        }
        //-------------------------------------------------------------ReturnFromModelNoForm--------------------------- 
        public void ReturnFromModelNoForm(decimal decId)//Form ModelNo
        {
            try
            {
                ModelNoComboFill();
                if (decId.ToString() != "0")
                {
                    cmbModalNo.SelectedValue = decId;
                }
                else if (strModelName != string.Empty)
                {
                    cmbModalNo.SelectedValue = strModelName;
                }
                else
                {
                    cmbModalNo.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbModalNo.Focus();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:41" + ex.Message;
            }
        }
        //-------------------------------------------------------------ReturnFromRackForm--------------------------- 
        public void ReturnFromRackForm(decimal decId)//Form Rack
        {
            try
            {
                if (!isCallFromGrid)
                {
                    RackComboFill();
                    if (decId.ToString() != "0")
                    {
                        cmbDefaultRack.SelectedValue = decId;
                    }
                    else if (strRackName != string.Empty)
                    {
                        cmbDefaultRack.SelectedValue = strRackName;
                    }
                    else
                    {
                        cmbDefaultRack.SelectedIndex = -1;
                    }
                    this.Enabled = true;
                    cmbDefaultRack.Focus();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
                else
                {
                    DataGridViewComboBoxCell dgvRackCell = (DataGridViewComboBoxCell)dgvProductCreation[dgvProductCreation.Columns["dgvcmbrack"].Index, dgvProductCreation.CurrentRow.Index];
                    DataTable dtbl = new DataTable();
                    RackSP spRack = new RackSP();
                    dtbl = spRack.RackNamesCorrespondingToGodownId(decGodownIdForRack);
                    if (decId.ToString() != "0")
                    {
                        dgvRackCell.DataSource = dtbl;
                        dgvRackCell.ValueMember = "rackId";
                        dgvRackCell.DisplayMember = "rackName";
                        dgvProductCreation.CurrentRow.Cells["dgvcmbrack"].Value = decId;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:42" + ex.Message;
            }
        }
        //-------------------------------------------------------------ReturnFromProductGroupForm--------------------------- 
        public void ReturnFromProductGroupForm(decimal decId)//Form ProductGroup
        {
            try
            {
                GroupComboFill();
                if (decId.ToString() != "0")
                {
                    cmbGroup.SelectedValue = decId;
                }
                else if (strGroupName != string.Empty)
                {
                    cmbGroup.SelectedValue = strGroupName;
                }
                else
                {
                    cmbGroup.SelectedIndex = -1;
                }
                this.Enabled = true;
                cmbGroup.Focus();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:43" + ex.Message;
            }
        }
        //-------------------------------------------------------------ModelNoComboFill--------------------------- 
        public void ModelNoComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ModelNoSP spModelNo = new ModelNoSP();
                dtbl = spModelNo.ModelNoViewAll();
                cmbModalNo.DataSource = dtbl;
                cmbModalNo.DisplayMember = "modelNo";
                cmbModalNo.ValueMember = "modelNoId";
                cmbModalNo.SelectedValue = 1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:44" + ex.Message;
            }
        }
        //-------------------------------------------------------------SizeComboFill---------------------------
        public void SizeComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                SizeSP spSize = new SizeSP();
                dtbl = spSize.SizeViewAll();
                cmbSize.DataSource = dtbl;
                cmbSize.DisplayMember = "size";
                cmbSize.ValueMember = "sizeId";
                cmbSize.SelectedValue = 1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:45" + ex.Message;
            }
        }
        //-------------------------------------------------------------BrandComboFill---------------------------
        public void BrandComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                BrandSP spBrand = new BrandSP();
                dtbl = spBrand.BrandViewAll();
                cmbBrand.DataSource = dtbl;
                cmbBrand.DisplayMember = "brandName";
                cmbBrand.ValueMember = "brandId";
                cmbBrand.SelectedValue = 1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:46" + ex.Message;
            }
        }
        //-------------------------------------------------------------TaxComboFill---------------------------
        public void TaxComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                TaxSP spTax = new TaxSP();
                dtbl = spTax.TaxViewAllForProduct();
                cmbTax.DataSource = dtbl;
                cmbTax.DisplayMember = "taxName";
                cmbTax.ValueMember = "taxId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:47" + ex.Message;
            }
        }
        //-------------------------------------------------------------TaxBomComboFill--------------------------
        public void BomComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                BomSP spBom = new BomSP();
                dtbl = spBom.BomViewAll();
                cmbBom.DataSource = dtbl;
                cmbBom.DisplayMember = "taxName";
                cmbBom.ValueMember = "taxId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:48" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill godown combobox in grid
        /// </summary>
        public void GodownComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                GodownSP spGodown = new GodownSP();
                dtbl = spGodown.GodownViewAll();
                cmbDefaultGodown.DataSource = dtbl;
                cmbDefaultGodown.DisplayMember = "godownName";
                cmbDefaultGodown.ValueMember = "godownId";
                cmbDefaultGodown.SelectedValue = 1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:49" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill batch combobox in grid
        /// </summary>
        public void BatchComboFill()
        {
            try
            {
                cmbAllowBatch.Items.Add("No");
                cmbAllowBatch.Items.Add("Yes");
                cmbAllowBatch.SelectedItem = "No";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:50" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill BOM
        /// </summary>
        public void BomFill()
        {
            try
            {
                cmbBom.Items.Add("No");
                cmbBom.Items.Add("Yes");
                cmbBom.SelectedItem = "No";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:51" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill unit combobox in grid
        /// </summary>
        public void UnitComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                UnitSP spUnit = new UnitSP();
                dtbl = spUnit.UnitViewAll();
                cmbUnit.DataSource = dtbl;
                cmbUnit.DisplayMember = "unitName";
                cmbUnit.ValueMember = "unitId";
                cmbUnit.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:52" + ex.Message;
            }
        }
        /// <summary>
        /// Function of applicable for tax
        /// </summary>
        public void TaxApplicableOn()
        {
            try
            {
                cmbTaxApplicableOn.Items.Add("MRP");
                cmbTaxApplicableOn.Items.Add("Rate");
                //cmbTaxApplicableOn.Items.Add("PurchaseRate");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:53" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill rack combobox in grid
        /// </summary>
        public void RackComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                RackSP spRack = new RackSP();
                decimal decGodownId = Convert.ToDecimal(cmbDefaultGodown.SelectedValue.ToString());
                dtbl = spRack.RackNamesCorrespondingToGodownId(decGodownId);
                cmbDefaultRack.DataSource = dtbl;
                cmbDefaultRack.DisplayMember = "rackName";
                cmbDefaultRack.ValueMember = "rackId";
                if (dtbl.Rows.Count > 0)
                {
                    cmbDefaultRack.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:54" + ex.Message;
            }
        }
        /// <summary>
        /// Function to view all rack combobox in grid
        /// </summary>
        public void RackGridComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                RackSP spRack = new RackSP();
                dtbl = spRack.RackViewAll();
                DataRow drow = dtbl.NewRow();
                drow["rackName"] = string.Empty;
                drow["rackId"] = 0;
                dtbl.Rows.InsertAt(drow, 0);
                dgvcmbrack.DataSource = dtbl;
                dgvcmbrack.DisplayMember = "rackName";
                dgvcmbrack.ValueMember = "rackId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:55" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill opening stock
        /// </summary>
        public void OpeningStockComboFill()
        {
            try
            {
                cmbOpeningStock.Items.Add("No");
                cmbOpeningStock.Items.Add("Yes");
                cmbOpeningStock.SelectedItem = "No";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:56" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill multiple unit combobox
        /// </summary>
        public void MultipleUnitComboFill()
        {
            try
            {
                cmbMultipleUnit.Items.Add("No");
                cmbMultipleUnit.Items.Add("Yes");
                cmbMultipleUnit.SelectedItem = "No";
                dgvProductCreation.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:57" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill zero
        /// </summary>
        public void ZeroFilling()
        {
            try
            {
                txtPurchaseRate.Text = "0";
                txtMaximumStock.Text = "0";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:58" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill group combobox 
        /// </summary>
        public void GroupComboFill()
        {
            try
            {
                DataTable dtbl = new DataTable();
                ProductGroupSP spProductGroup = new ProductGroupSP();
                dtbl = spProductGroup.ProductGroupViewAll();
                cmbGroup.DataSource = dtbl;
                cmbGroup.DisplayMember = "groupName";
                cmbGroup.ValueMember = "groupId";
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:59" + ex.Message;
            }
        }
        /// <summary>
        /// Function for save details
        /// </summary>
        public void SaveFunction()
        {
            try
            {
                if (cmbOpeningStock.SelectedIndex == 1)
                {
                    if (cmbAllowBatch.SelectedIndex == 0)
                    {
                        if (RemoveIncompleteRowsFromGrid())
                        {
                            if (!GridCheckWeatherSameRackAndGodownExisting())
                            {
                                ProductTableFill();
                                BatchWithBarCode();
                                if (decSaveProduct >= 0)
                                {
                                    if (isSaveBomCheck)
                                    {
                                        BomTableFill();
                                    }
                                    if (isSaveMulUnitCheck)
                                    {
                                        UnitConvertionTableFill();
                                    }
                                    StockPostingTableFill();
                                    if (decCheck > 0)
                                    {
                                        Messages.SavedMessage();
                                        ClearFunction();
                                        if (isAuto)
                                        {
                                            AutomaticCodeGenaration();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dgvProductCreation.Rows.Count == 1)
                                {
                                    Messages.InformationMessage("Cant save without atleast one row");
                                    dgvProductCreation.Focus();
                                    dgvProductCreation.CurrentCell = dgvProductCreation.Rows[0].Cells["dgvcmbtgodown"];
                                }
                            }
                        }
                    }
                    else
                    {
                        if (RemoveIncompleteRowsFromGrid())
                        {
                            if (!GridCheckWeatherSameRackAndGodownExisting())
                            {
                                ProductTableFill();
                                if (decSaveProduct > 0)
                                {
                                    if (isSaveBomCheck)
                                    {
                                        BomTableFill();
                                    }
                                    if (isSaveMulUnitCheck)
                                    {
                                        UnitConvertionTableFill();
                                    }
                                    BatchTableWithStockAndProductBatchFill();
                                    BatchWithBarCode();
                                    if (decCheck > 0)
                                    {
                                        Messages.SavedMessage();
                                        ClearFunction();
                                        if (isAuto)
                                        {
                                            AutomaticCodeGenaration();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dgvProductCreation.Rows.Count == 1)
                            {
                                Messages.InformationMessage("Cant save without atleast one row");
                                dgvProductCreation.Focus();
                                dgvProductCreation.CurrentCell = dgvProductCreation.Rows[0].Cells["dgvcmbtgodown"];
                            }
                        }
                    }
                }
                else
                {
                    ProductTableFill();
                    BatchWithBarCode();
                    if (isSaveBomCheck)
                    {
                        BomTableFill();
                    }
                    if (isSaveMulUnitCheck)
                    {
                        UnitConvertionTableFill();
                    }
                    if (decSaveProduct > 0)
                    {
                        Messages.SavedMessage();
                        ClearFunction();
                        if (isAuto)
                        {
                            AutomaticCodeGenaration();
                        }
                    }
                }
                if (frmPosObj != null)
                {
                    this.Close();
                }
                else if (frmSalesInvoiceObj != null)
                {
                    this.Close();
                }
                else if (frmPurchaseReturnObj != null)
                {
                    this.Close();
                }
                else if (frmSalesReturnObj != null)
                {
                    this.Close();
                }
                else if (frmpurchaseOrderObj != null)
                {
                    this.Close();
                }
                else if (frmSalesQuotationObj != null)
                {
                    this.Close();
                }
                else if (frmSalesOrderObj != null)
                {
                    this.Close();
                }
                else if (frmDeliveryNoteObj != null)
                {
                    this.Close();
                }
                else if (frmStockJournalObj != null)
                {
                    this.Close();
                }
                else if (frmPhysicalStockObj != null)
                {
                    this.Close();
                }
                else if (frmPurchaseInvoiceObj != null)
                {
                    this.Close();
                }
                else if (frmMaterialReceiptObj != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:60" + ex.Message;
            }
        }
        /// <summary>
        /// Function for update details in table
        /// </summary>
        public void EditFunction()
        {
            ProductSP spProduct = new ProductSP();
            try
            {
                decimal decStatus = spProduct.BatchReferenceCheckForProductEdit(decProductIdForEdit);
                if (decStatus != 0)
                {
                    if (cmbOpeningStock.SelectedIndex == 1)
                    {
                        if (isOpeningStockChanged)
                        {
                            DeleteOpeningStock();
                            DeleteBatchFromOpeningStock();
                        }
                        if (cmbAllowBatch.SelectedIndex == 0)
                        {
                            if (RemoveIncompleteRowsFromGrid())
                            {
                                if (!GridCheckWeatherSameRackAndGodownExisting())
                                {
                                    ProductEditFill();
                                    if (isUpdated)
                                    {
                                        CheckWetherSaveOrEdit();
                                        RemoveRows();
                                        if (isBatchForUpdate)
                                        {
                                            DeleteBatchFromOpeningStock();
                                        }
                                        if (!isOpeningStockForUpdate)
                                        {
                                            StockPostingTableFillWhileUpdating();
                                        }
                                        else
                                        {
                                            NewRowAdded();
                                            StockPostingTableEditFill();
                                        }
                                        if (isStpUpdated || decCheck > 0 || isRowRemoved)
                                        {
                                            Messages.UpdatedMessage();
                                            this.Close();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dgvProductCreation.Rows.Count == 1)
                                {
                                    Messages.InformationMessage("Cant save without atleast one row");
                                    dgvProductCreation.Focus();
                                }
                            }
                        }
                        else
                        {
                            if (RemoveIncompleteRowsFromGrid())
                            {
                                if (!GridCheckWeatherSameRackAndGodownExisting())
                                {
                                    ProductEditFill();
                                    if (isUpdated)
                                    {
                                        CheckWetherSaveOrEdit();
                                        RemoveRows();
                                        if (!isBatchForUpdate)
                                        {
                                            BatchTableFill();
                                        }
                                        else
                                        {
                                            RowsAddInBatchTable();
                                            BatchTableEditFill();
                                        }
                                        if (!isOpeningStockForUpdate)
                                        {
                                            StockPostingTableFillWhileUpdating();
                                        }
                                        else
                                        {
                                            NewRowAdded();
                                            StockPostingTableEditFill();
                                        }
                                        if (isStpUpdated || isBatchUpdated || decBatchId > 0 || decCheck > 0)
                                        {
                                            Messages.UpdatedMessage();
                                            this.Close();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dgvProductCreation.Rows.Count == 1)
                                {
                                    Messages.InformationMessage("Cant save without atleast one row");
                                    dgvProductCreation.Focus();
                                }
                            }
                        }
                    }
                    else
                    {
                        ProductEditFill();
                        CheckWetherSaveOrEdit();
                        if (cmbOpeningStock.SelectedIndex == 0 && isOpeningStockForUpdate == true)
                        {
                            DeleteOpeningStock();
                            if (isBatchForUpdate)
                            {
                                isBatchForUpdate = false;
                                DeleteBatchFromOpeningStock();
                            }
                        }
                        if (cmbAllowBatch.SelectedIndex == 1 && (isBatchForUpdate == true))
                        {
                            DeleteBatchFromOpeningStock();
                        }
                        if (isUpdated)
                        {
                            Messages.UpdatedMessage();
                            this.Close();
                        }
                    }
                }
                else
                {
                    Messages.ReferenceExistsMessageForUpdate();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:61" + ex.Message;
            }
        }
        /// <summary>
        /// Function for save or update
        /// </summary>
        public void SaveOrEdit()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                if (txtName.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter product name");
                    txtName.Focus();
                }
                else if (txtProductCode.Text.Trim() == string.Empty)
                {
                    Messages.InformationMessage("Enter prouct code");
                    txtProductCode.Focus();
                }
                else if (cmbGroup.SelectedIndex == -1)
                {
                    Messages.InformationMessage("Select group");
                    cmbGroup.Focus();
                }
                else if (cmbUnit.SelectedIndex == -1)
                {
                    Messages.InformationMessage(" Select unit");
                    cmbUnit.Focus();
                }
                else if (cmbTaxApplicableOn.SelectedIndex == -1 && cmbTax.SelectedIndex != 0)
                {
                    Messages.InformationMessage("Select tax applicable on");
                    cmbTaxApplicableOn.Focus();
                }
                else if (Convert.ToDecimal(txtMaximumStock.Text.ToString()) <= Convert.ToDecimal(txtMinimumStock.Text.ToString()))
                {
                    Messages.InformationMessage("maximum stock should be greater than Minimum  stock");
                    txtMinimumStock.Focus();
                }
                //else if (txtPartNo.Text.Trim() != string.Empty && spProduct.PartNoCheckExistence(txtPartNo.Text.Trim()) == true)
                //{
                //    Messages.InformationMessage("Part No Already Exist");
                //    txtPartNo.Focus();
                //}
                else
                {
                    if (btnSave.Text == "Save")
                    {
                        if (spProduct.ProductCodeCheckExistence(txtProductCode.Text.Trim(), 0) == false)
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
                        else
                        {
                            Messages.InformationMessage("Product code already exist");
                            txtProductCode.Focus();
                        }
                    }
                    else
                    {
                        if (spProduct.ProductCodeCheckExistence(txtProductCode.Text.Trim(), decProductIdForEdit) == false)
                        {
                            EditFunction();
                        }
                        else
                        {
                            Messages.InformationMessage("Product code already exist");
                            txtProductCode.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:62" + ex.Message;
            }
        }
        /// <summary>
        /// Function for call from productRegister
        /// </summary>
        public void CallFromProductRegister(decimal decId, frmProductRegister frmProRegister)
        {
            try
            {
                base.Show();
                this.frmProductRegisterObj = frmProRegister;
                frmProductRegisterObj.Enabled = false;
                ProductSP spProduct = new ProductSP();
                ProductInfo infoProduct = new ProductInfo();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                BatchInfo infoBatch = new BatchInfo();
                StockPostingSP spStockposting = new StockPostingSP();
                BatchSP spBatch = new BatchSP();
                DataTable dtbl = new DataTable();
                UnitSP spUnit = new UnitSP();
                decProductIdForEdit = decId;
                infoProduct = spProduct.ProductView(decId);
                strUnitNameForGrid = spUnit.UnitName(infoProduct.UnitId);
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                //cmbUnit.Enabled = false;
                txtName.Text = infoProduct.ProductName;
                txtProductCode.Text = infoProduct.ProductCode;
                cmbGroup.SelectedValue = infoProduct.GroupId;
                cmbBrand.SelectedValue = infoProduct.BrandId;
                cmbUnit.SelectedValue = infoProduct.UnitId;
                if (spProduct.ProductReferenceCheck(decId))
                {
                    cmbUnit.Enabled = false;
                }
                decUnitIdForUpdate = infoProduct.UnitId;
                cmbSize.SelectedValue = infoProduct.SizeId;
                cmbModalNo.SelectedValue = infoProduct.ModelNoId;
                cmbTax.SelectedValue = infoProduct.TaxId;
                cmbTaxApplicableOn.SelectedItem = infoProduct.TaxapplicableOn;
                txtPurchaseRate.Text = infoProduct.PurchaseRate.ToString();
                txtSalesRate.Text = infoProduct.SalesRate.ToString();
                txtMrp.Text = infoProduct.Mrp.ToString();
                txtMinimumStock.Text = infoProduct.MinimumStock.ToString();
                txtMaximumStock.Text = infoProduct.MaximumStock.ToString();
                txtReorderLevel.Text = infoProduct.ReorderLevel.ToString();
                txtPartNo.Text = infoProduct.PartNo;
                cmbDefaultGodown.SelectedValue = infoProduct.GodownId;
                cmbDefaultRack.SelectedValue = infoProduct.RackId;
                if (infoProduct.IsBom)
                {
                    cmbBom.SelectedIndex = 1;
                    isBomFromRegister = true;
                }
                if (infoProduct.Ismultipleunit)
                {
                    cmbMultipleUnit.SelectedIndex = 1;
                    isMulUnitFromRgister = true;
                }
                if (infoProduct.Isopeningstock)
                {
                    isOpeningStockForUpdate = true;
                    OpeningStockGridFill();
                }
                if (infoProduct.IsallowBatch)
                {
                    OpeningStockGridWithBathFill();
                }
                else
                {
                    cmbAllowBatch.SelectedIndex = 0;
                    txtPartNo.Text = spBatch.PartNoReturn(decProductIdForEdit);
                }
                if (infoProduct.Ismultipleunit)
                {
                    cmbMultipleUnit.SelectedIndex = 1;
                }
                else
                {
                    cmbMultipleUnit.SelectedIndex = 0;
                }
                if (infoProduct.IsBom)
                {
                    cmbBom.SelectedIndex = 1;
                }
                else
                {
                    cmbBom.SelectedIndex = 0;
                }
                if (infoProduct.Isopeningstock)
                {
                    cmbOpeningStock.SelectedIndex = 1;
                }
                else
                {
                    cmbOpeningStock.SelectedIndex = 0;
                }
                if (infoProduct.IsActive)
                {
                    cbxActive.Checked = true;
                }
                else
                {
                    cbxActive.Checked = false;
                }
                if (infoProduct.IsshowRemember)
                {
                    cbxReminder.Checked = true;
                }
                else
                {
                    cbxReminder.Checked = false;
                }
                txtNarration.Text = infoProduct.Narration;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:63" + ex.Message;
            }
        }
        /// <summary>
        /// Function for delete details from table
        /// </summary>
        public void DeleteFuntion()
        {
            try
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
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:64" + ex.Message;
            }
        }
        /// <summary>
        /// Function for delete data from tbl_BOM
        /// </summary>
        public void RemoveBom()
        {
            try
            {
                BomInfo infoBom = new BomInfo();
                BomSP spBom = new BomSP();
                int inC = 0;
                while (strArrOfRemoveForBom[inC] != null)
                {
                    decimal decId = Convert.ToDecimal(strArrOfRemoveForBom[inC]);
                    spBom.BomRemoveRows(decId);
                    inC++;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:65" + ex.Message;
            }
        }
        /// <summary>
        /// Function for delete data from tbl_Product
        /// </summary>
        public void Delete()
        {
            try
            {
                ProductSP spProduct = new ProductSP();
                decimal decResult = spProduct.DeleteProductWithReferenceCheck(decProductIdForEdit);
                if (decResult > 0)
                {
                    Messages.DeletedMessage();
                    ClearFunction();
                    this.Close();
                }
                else
                {
                    Messages.ReferenceExistsMessage();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:66" + ex.Message;
            }
        }
        /// <summary>
        /// Function for NewRow Added to tbl_StockPosting
        /// </summary>
        public void NewRowAdded()
        {
            try
            {
                for (int inI = 0; inI < dgvProductCreation.Rows.Count - 1; inI++)
                {
                    if (Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtstockpostId"].Value) == 0)
                    {
                        StockPostingNewRows(inI);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:67" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Delete Rows From tbl_StockPosting Or tbl_Batch
        /// </summary>
        public void RemoveRows()
        {
            try
            {
                foreach (var strId in lstArrOfRemove)
                {
                    decimal decDeleteId = Convert.ToDecimal(strId);
                    StockPostingSP spStockPosting = new StockPostingSP();
                    isRowRemoved = spStockPosting.StpDeleteForRowRemove(decDeleteId);
                }
                foreach (var strBatchId in lstArrOfRemoveFromBatchTable)
                {
                    decimal decDeleteId = Convert.ToDecimal(strBatchId);
                    BatchSP spBatch = new BatchSP();
                    spBatch.BatchDelete(decDeleteId);
                }
                foreach (var strStockPostingId in lstArrOfRemoveFromStockPosting)
                {
                    decimal decDeleteId = Convert.ToDecimal(strStockPostingId);
                    StockPostingSP spStockPosting = new StockPostingSP();
                    isRowRemoved = spStockPosting.StpDeleteForRowRemove(decDeleteId);
                }
                foreach (var strBatchRemoveId in lststrArrBatchRemove)
                {
                    decimal decDeleteId = Convert.ToDecimal(strBatchRemoveId);
                    BatchSP spBatch = new BatchSP();
                    spBatch.BatchDelete(decDeleteId);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:68" + ex.Message;
            }
        }
        /// <summary>
        /// Function for New Row Added to tbl_Batch
        /// </summary>
        public void RowsAddInBatchTable()
        {
            try
            {
                for (int inI = 0; inI < dgvProductCreation.Rows.Count - 1; inI++)
                {
                    if (Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtbatchId"].Value) == 0 || Convert.ToDecimal(dgvProductCreation.Rows[inI].Cells["dgvtxtstockpostId"].Value) == 0)
                    {
                        BatchTableNewRows(inI);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:69" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Data Row To Delete From tbl_Bom
        /// </summary>
        /// <param name="strArr"></param>
        public void RomovedIndexFromBom(string[] strArr)
        {
            try
            {
                strArrOfRemoveForBom = strArr;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:70" + ex.Message;
            }
        }
        /// <summary>
        /// Funvtion for Data Row To Delete From tbl_UnitConvertion
        /// </summary>
        /// <param name="strArr"></param>
        public void RomovedIndexFromMulUnit(string[] strArr)
        {
            try
            {
                strArrOfRemoveForMulUnit = strArr;
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:71" + ex.Message;
            }
        }
        /// <summary>
        /// Function for New Row Added To Delete From tbl_StockPosting
        /// </summary>
        /// <param name="inI"></param>
        public void StockPostingNewRows(int inI)
        {
            try
            {
                StockPostingSP spStockPosting = new StockPostingSP();
                StockPostingInfo infoStockPosting = new StockPostingInfo();
                int inRow = inI;
                infoStockPosting.AgainstInvoiceNo = string.Empty;
                infoStockPosting.AgainstVoucherNo = string.Empty;
                infoStockPosting.Date = PublicVariables._dtCurrentDate;
                infoStockPosting.AgainstVoucherTypeId = 0;
                infoStockPosting.InvoiceNo = Convert.ToString(decProductIdForEdit);
                infoStockPosting.VoucherNo = Convert.ToString(decProductIdForEdit);
                infoStockPosting.VoucherTypeId = 2;
                // infoStockPosting.UnitId = decUnitIdForUpdate;
                infoStockPosting.UnitId = Convert.ToDecimal(dgvProductCreation.Rows[inRow].Cells["dgvcmbUnit"].Value);
                infoStockPosting.GodownId = Convert.ToDecimal(dgvProductCreation.Rows[inRow].Cells["dgvcmbtgodown"].Value);
                infoStockPosting.InwardQty = Convert.ToDecimal(dgvProductCreation.Rows[inRow].Cells["dgvtxtqty"].Value);
                infoStockPosting.OutwardQty = 0;
                infoStockPosting.RackId = Convert.ToDecimal(dgvProductCreation.Rows[inRow].Cells["dgvcmbrack"].Value);
                infoStockPosting.Rate = Convert.ToDecimal(dgvProductCreation.Rows[inRow].Cells["dgvtxtrate"].Value);
                infoStockPosting.FinancialYearId = PublicVariables._decCurrentFinancialYearId;
                infoStockPosting.Extra1 = string.Empty;
                infoStockPosting.Extra2 = string.Empty;
                infoStockPosting.ExtraDate = DateTime.Now;
                infoStockPosting.ProductId = decProductIdForEdit;
                if (cmbAllowBatch.SelectedIndex == 0)
                {
                    int inId = spStockPosting.ReturnBatchIdFromStockPosting(decProductIdForEdit);
                    infoStockPosting.BatchId = inId;
                }
                else
                {
                    // infoStockPosting.BatchId = decBatchId;
                    infoStockPosting.BatchId = Convert.ToDecimal(dgvProductCreation.Rows[inRow].Cells["dgvtxtbatchId"].Value);
                }
                decimal decId = spStockPosting.StockPostingAdd(infoStockPosting);
                dgvProductCreation.Rows[inRow].Cells["dgvtxtstockpostId"].Value = decId;
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:72" + ex.Message;
            }
        }
        /// <summary>
        /// Function for New Row Added To tbl_Batch
        /// </summary>
        /// <param name="inI"></param>
        public void BatchTableNewRows(int inI)
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                BatchInfo infoBatch = new BatchInfo();
                infoBatch.ManufacturingDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxManfDate"].Value);
                infoBatch.ExpiryDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxtExpDate"].Value);
                infoBatch.BatchNo = dgvProductCreation.Rows[inI].Cells["dgvtxtbatch"].Value.ToString();
                infoBatch.ProductId = decProductIdForEdit;
                infoBatch.Extra1 = string.Empty;
                infoBatch.Extra2 = string.Empty;
                infoBatch.ExtraDate = DateTime.Now;
                infoBatch.narration = string.Empty;
                infoBatch.barcode = Convert.ToString(spBatch.AutomaticBarcodeGeneration());
                decimal decId = spBatch.BatchAddReturnIdentity(infoBatch);
                //decimal decId = spBatch.StockPostingAdd(infoBatch);
                dgvProductCreation.Rows[inI].Cells["dgvtxtbatchId"].Value = decId;
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:73" + ex.Message;
            }
        }
        /// <summary>
        /// Function for check settings
        /// </summary>
        public void InitialSettings()
        {
            try
            {
                SettingsSP spSettings = new SettingsSP();
                SettingsInfo info = new SettingsInfo();
                DataTable dtbl = new DataTable();
                dtbl = spSettings.SettingsViewAll();
                if (dtbl.Rows.Count > 0)
                {
                    foreach (DataRow item in dtbl.Rows)
                    {
                        info.SettingsName = item["settingsName"].ToString();
                        info.Status = item["status"].ToString();
                        if (info.SettingsName == "AllowBatch" && info.Status == "Yes")
                        {
                            cmbAllowBatch.Enabled = true;
                        }
                        if (info.SettingsName == "AllowBatch" && info.Status == "No")
                        {
                            cmbAllowBatch.Enabled = false;
                        }
                        if (info.SettingsName == "AllowSize" && info.Status == "Yes")
                        {
                            cmbSize.Enabled = true;
                            btnSizeAdd.Enabled = true;
                        }
                        if (info.SettingsName == "AllowSize" && info.Status == "No")
                        {
                            cmbSize.Enabled = false;
                            btnSizeAdd.Enabled = false;
                        }
                        if (info.SettingsName == "AllowModelNo" && info.Status == "Yes")
                        {
                            cmbModalNo.Enabled = true;
                            btnModalNo.Enabled = true;
                        }
                        if (info.SettingsName == "AllowModelNo" && info.Status == "No")
                        {
                            cmbModalNo.Enabled = false;
                            btnModalNo.Enabled = false;
                        }
                        if (info.SettingsName == "AllowGodown" && info.Status == "Yes")
                        {
                            cmbDefaultGodown.Enabled = true;
                            btnDefaultGodownAdd.Enabled = true;
                            dgvProductCreation.Columns["dgvcmbtgodown"].Visible = true;
                        }
                        if (info.SettingsName == "AllowGodown" && info.Status == "No")
                        {
                            cmbDefaultGodown.Enabled = false;
                            btnDefaultGodownAdd.Enabled = false;
                            dgvProductCreation.Columns["dgvcmbtgodown"].Visible = false;
                        }
                        if (info.SettingsName == "AllowRack" && info.Status == "Yes")
                        {
                            cmbDefaultRack.Enabled = true;
                            btnDefaultAdd.Enabled = true;
                            dgvProductCreation.Columns["dgvcmbrack"].Visible = true;
                        }
                        if (info.SettingsName == "AllowRack" && info.Status == "No")
                        {
                            cmbDefaultRack.Enabled = false;
                            btnDefaultAdd.Enabled = false;
                            dgvProductCreation.Columns["dgvcmbrack"].Visible = false;
                        }
                        if (info.SettingsName == "Tax" && info.Status == "No")
                        {
                            cmbTax.Enabled = false;
                            btnTaxAdd.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:74" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Delete Rows From tbl_unitConvertion
        /// </summary>
        public void RemoveMulUnit()
        {
            try
            {
                UnitConvertionSP spunitConvertion = new UnitConvertionSP();
                int inC = 0;
                while (strArrOfRemoveForMulUnit[inC] != null)
                {
                    decimal decId = Convert.ToDecimal(strArrOfRemoveForMulUnit[inC]);
                    spunitConvertion.MulUnitRemoveRows(decId);
                    inC++;
                }
            }
            catch (Exception ex)
            {
           formMDI.infoError.ErrorString = "PC:75" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Update dataTable for Bom 
        /// </summary>
        /// <param name="dtblBomUpdate"></param>
        /// <param name="isUpdated"></param>
        public void DataTableForBomUpdate(DataTable dtblBomUpdate, bool isUpdated)
        {
            try
            {
                dtblFromBomForUpdate = dtblBomUpdate;
                isBomUpdated = isUpdated;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:76" + ex.Message;
            }
        }
        /// <summary>
        /// Function for Update dataTable for multiple unit
        /// </summary>
        /// <param name="dtblMulUnitUpdate"></param>
        /// <param name="isUpdated"></param>
        public void DataTableForMulUnitUpdate(DataTable dtblMulUnitUpdate, bool isUpdated)
        {
            try
            {
                dtblFromMulUnitForUpdate = dtblMulUnitUpdate;
                isMulUnitUpdated = isUpdated;
                dtblForUnitIdInOpeNingStock = dtblFromMulUnitForUpdate;
                if (cmbOpeningStock.SelectedIndex == 1 && dtblForUnitIdInOpeNingStock != null && dtblForUnitIdInOpeNingStock.Rows.Count > 0)
                {
                    dgvcmbUnit.DataSource = dtblForUnitIdInOpeNingStock;
                    dgvcmbUnit.DisplayMember = "unitName";
                    dgvcmbUnit.ValueMember = "dgvtxtunitId";
                    for (int i = 0; i < dgvProductCreation.Rows.Count; i++)
                    {
                        if (!dgvProductCreation.Rows[i].IsNewRow)
                        {
                            dgvProductCreation.Rows[i].Cells["dgvcmbUnit"].Value = dtblForUnitIdInOpeNingStock.Rows[0][1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:77" + ex.Message;
            }
        }
        /// <summary>
        /// Function for new row added for Bom
        /// </summary>
        public void NewRowAddedForBom()
        {
            try
            {
                BomInfo infoBom = new BomInfo();
                BomSP spBom = new BomSP();
                for (int inI = 0; inI < dtblFromBomForUpdate.Rows.Count; inI++)
                {
                    if (Convert.ToDecimal(dtblFromBomForUpdate.Rows[inI]["bomId"]) == 0)
                    {
                        infoBom.RowmaterialId = Convert.ToDecimal(dtblFromBomForUpdate.Rows[inI]["dgvcmbRawMaterial"]);
                        infoBom.UnitId = Convert.ToDecimal(dtblFromBomForUpdate.Rows[inI]["dgvtxtUnitId"]);
                        infoBom.Quantity = Convert.ToDecimal(dtblFromBomForUpdate.Rows[inI]["dgvtxtQty"]);
                        infoBom.Extra1 = dtblFromBomForUpdate.Rows[inI]["extra1"].ToString();
                        infoBom.Extra2 = dtblFromBomForUpdate.Rows[inI]["extra2"].ToString();
                        infoBom.ExtraDate = Convert.ToDateTime(dtblFromBomForUpdate.Rows[inI]["extraDate"]);
                        spBom.BomFromDatatable(infoBom, decProductIdForEdit);
                    }
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:78" + ex.Message;
            }
        }
        /// <summary>
        /// Function for new row added for multiple unit
        /// </summary>
        public void NewRowAddedForMulUnit()
        {
            try
            {
                UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
                UnitConvertionInfo infoUnitConversion = new UnitConvertionInfo();
                for (int inI = 0; inI < dtblFromMulUnitForUpdate.Rows.Count; inI++)
                {
                    if (Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[inI]["unitconvertionId"]) == 0)
                    {
                        infoUnitConversion.ConversionRate = Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[inI]["CnvertionRate"]);
                        infoUnitConversion.UnitId = Convert.ToDecimal(dtblFromMulUnitForUpdate.Rows[inI]["dgvtxtUnitId"]);
                        infoUnitConversion.Quantities = Convert.ToString(dtblFromMulUnitForUpdate.Rows[inI]["quantities"]);
                        infoUnitConversion.Extra1 = dtblFromMulUnitForUpdate.Rows[inI]["extra1"].ToString();
                        infoUnitConversion.Extra2 = dtblFromMulUnitForUpdate.Rows[inI]["extra2"].ToString();
                        infoUnitConversion.ExtraDate = Convert.ToDateTime(dtblFromMulUnitForUpdate.Rows[inI]["extraDate"]);
                        infoUnitConversion.ProductId = decProductIdForEdit;
                        spUnitConvertion.UnitConvertionAdd(infoUnitConversion);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:79" + ex.Message;
            }
        }
        /// <summary>
        /// Function for check same batch in grid
        /// </summary>
        /// <returns></returns>
        public bool GridCheckOfSameBatch()
        {
            isBatchCheck = false;
            try
            {
                foreach (DataGridViewRow dgvrowOne in dgvProductCreation.Rows)
                {
                    foreach (DataGridViewRow dgvrowTwo in dgvProductCreation.Rows)
                    {
                        if (dgvrowOne.Index != dgvrowTwo.Index)
                        {
                            if (dgvrowOne.Cells["dgvtxtbatch"].Value != null && dgvrowTwo.Cells["dgvtxtbatch"].Value != null)
                                if (dgvrowOne.Cells["dgvtxtbatch"].Value.ToString() == dgvrowTwo.Cells["dgvtxtbatch"].Value.ToString())
                                {
                                    isBatchCheck = true;
                                    Messages.InformationMessage("Repeatation of same batch No");
                                    dgvProductCreation.Focus();
                                    dgvProductCreation.CurrentCell = dgvProductCreation.Rows[dgvrowTwo.Index].Cells["dgvtxtbatch"];
                                    dgvProductCreation.BeginEdit(true);
                                    break;
                                }
                        }
                    }
                    if (isBatchCheck == true)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:80" + ex.Message;
            }
            return isBatchCheck;
        }
        /// <summary>
        /// Function for fill productBatch table
        /// </summary>
        public void ProductBatchTableFill()
        {
            try
            {
            ProductBatchSP spProductBatch = new ProductBatchSP();
            ProductBatchInfo infoProductBatch = new ProductBatchInfo();
            if (true)
            {
                infoProductBatch.BatchId = 0;
            }
            infoProductBatch.Extra1 = string.Empty;
            infoProductBatch.Extra2 = string.Empty;
            infoProductBatch.ExtraDate = DateTime.Now;
            infoProductBatch.PartNo = string.Empty;
            infoProductBatch.ProductId = decSaveProduct;
            spProductBatch.ProductBatchAdd(infoProductBatch);
            } 
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:81" + ex.Message;
            }

        }
        /// <summary>
        /// Function to fill product,product code autocompletion 
        /// </summary>
        /// <param name="isBatchName"></param>
        /// <param name="editControl"></param>
        public void FillBatch(bool isBatchName, DataGridViewTextBoxEditingControl editControl)
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                DataTable dtblBatch = new DataTable();
                Batch = spBatch.BatchViewAllWithoutNA();
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "PC:82" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save batch
        /// </summary>
        public void BatchTableFill()
        {
            try
            {
                BatchSP spBatch = new BatchSP();
                BatchInfo infoBatch = new BatchInfo();
                for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                {
                    infoBatch.ManufacturingDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxManfDate"].Value);
                    infoBatch.ExpiryDate = Convert.ToDateTime(dgvProductCreation.Rows[inI].Cells["dgvtxtExpDate"].Value);
                    infoBatch.BatchNo = dgvProductCreation.Rows[inI].Cells["dgvtxtbatch"].Value.ToString();
                    infoBatch.barcode = Convert.ToString(spBatch.AutomaticBarcodeGeneration());
                    if (btnSave.Text == "Update")
                    {
                        infoBatch.ProductId = decProductIdForEdit;
                    }
                    else
                    {
                        infoBatch.ProductId = decSaveProduct;
                    }
                    infoBatch.Extra1 = string.Empty;
                    infoBatch.Extra2 = string.Empty;
                    infoBatch.ExtraDate = DateTime.Now;
                    infoBatch.narration = string.Empty;
                    decBatchId = spBatch.BatchAddAndDelete(infoBatch, decProductIdForEdit);
                    dgvProductCreation.Rows[inI].Cells["dgvtxtbatchId"].Value = decBatchId;
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:83" + ex.Message;
            }
        }
        /// <summary>
        /// Function to save batch
        /// </summary>
        public void BatchWithBarCode()
        {
            try
            {
            BatchSP spBatch = new BatchSP();
            BatchInfo infoBatch = new BatchInfo();
            Int32 inBarcode = spBatch.AutomaticBarcodeGeneration();
            infoBatch.BatchNo = "NA";
            infoBatch.ExpiryDate = DateTime.Now;
            infoBatch.ManufacturingDate = DateTime.Now;
            infoBatch.partNo = txtPartNo.Text.Trim();
            if (btnSave.Text == "Update")
            {
                infoBatch.ProductId = decProductIdForEdit;
            }
            else
            {
                infoBatch.ProductId = decSaveProduct;
            }
            infoBatch.narration = string.Empty;
            infoBatch.ExtraDate = DateTime.Now;
            infoBatch.barcode = Convert.ToString(inBarcode);
            infoBatch.Extra1 = string.Empty;
            infoBatch.Extra2 = string.Empty;
            inBatchIdWithPartNoNA = spBatch.BatchAddWithBarCode(infoBatch);
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:84" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill default Unit
        /// </summary>
        public void DefaultUnitFillingInProductGrid()
        {
            try
            {
            UnitSP spUnit = new UnitSP();
            string strName = spUnit.UnitName(decId);
            DataTable dtblUnit = new DataTable();
            dtblUnit.Columns.Add("UnitName", typeof(string));
            dtblUnit.Columns.Add("UnitId", typeof(decimal));
            if (btnSave.Text == "Update")
            {
                strName = spUnit.UnitName(Convert.ToDecimal(cmbUnit.SelectedValue));
                decId = Convert.ToDecimal(cmbUnit.SelectedValue);
            }
            DataRow dr = dtblUnit.NewRow();
            dr["UnitName"] = strName;
            dr["UnitId"] = decId;
            dtblUnit.Rows.Add(dr);
            DataRow drow = dtblUnit.NewRow();
            drow["UnitName"] = string.Empty;
            drow["UnitId"] = 0;
            dtblUnit.Rows.InsertAt(drow, 0);
            dgvcmbUnit.DataSource = dtblUnit;
            dgvcmbUnit.DisplayMember = "UnitName";
            dgvcmbUnit.ValueMember = "UnitId";
            for (int i = 0; i < dgvProductCreation.Rows.Count; i++)
            {
                if (!dgvProductCreation.Rows[i].IsNewRow)
                {
                    dgvProductCreation.Rows[i].Cells["dgvcmbUnit"].Value = decId;
                }
            }
            }
              catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:85" + ex.Message;
            }
        }
        /// <summary>
        /// Function to fill Unit combobox
        /// </summary>
        public void UnitComboInsideGridFill()
        {
            try
            {
            UnitConvertionSP spUnitConvertion = new UnitConvertionSP();
            DataTable dtbl = new DataTable();
            dtbl = spUnitConvertion.UnitsOfPerticularProduct(decProductIdForEdit);
            dgvcmbUnit.DataSource = dtbl;
            DataRow drow = dtbl.NewRow();
            drow["unitName"] = string.Empty;
            drow["unitId"] = 0;
            dtbl.Rows.InsertAt(drow, 0);
            dgvcmbUnit.ValueMember = "unitId";
            dgvcmbUnit.DisplayMember = "unitName";
            }
              catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:86" + ex.Message;
            }
        }
        /// <summary>
        /// Function to check existence of Rack And Godown
        /// </summary>
        /// <returns></returns>
        public bool GridCheckWeatherSameRackAndGodownExisting()
        {
            isGodownCheck = false;
            try
            {
                if (cmbDefaultGodown.Enabled && cmbDefaultRack.Enabled && cmbAllowBatch.SelectedIndex == 0)
                {
                    foreach (DataGridViewRow dgvrowOne in dgvProductCreation.Rows)
                    {
                        foreach (DataGridViewRow dgvrowTwo in dgvProductCreation.Rows)
                        {
                            if (dgvrowOne.Index != dgvrowTwo.Index)
                            {
                                if (dgvrowOne.Cells["dgvcmbtgodown"].Value != null && dgvrowOne.Cells["dgvcmbrack"].Value != null && dgvrowTwo.Cells["dgvcmbtgodown"].Value != null
                                    && dgvrowTwo.Cells["dgvcmbrack"].Value != null)
                                    if (dgvrowOne.Cells["dgvcmbtgodown"].Value.ToString() == dgvrowTwo.Cells["dgvcmbtgodown"].Value.ToString() &&
                                         dgvrowOne.Cells["dgvcmbrack"].Value.ToString() == dgvrowTwo.Cells["dgvcmbrack"].Value.ToString())
                                    {
                                        isGodownCheck = true;
                                        Messages.InformationMessage("Repeatation of same Godown and Rack");
                                        dgvProductCreation.Focus();
                                        dgvProductCreation.CurrentCell = dgvProductCreation.Rows[dgvrowTwo.Index].Cells["dgvcmbrack"];
                                        dgvProductCreation.BeginEdit(true);
                                        break;
                                    }
                            }
                        }
                        if (isGodownCheck == true)
                        {
                            break;
                        }
                    }
                }
                if (cmbDefaultGodown.Enabled && !cmbDefaultRack.Enabled && cmbAllowBatch.SelectedIndex == 0)
                {
                    foreach (DataGridViewRow dgvrowOne in dgvProductCreation.Rows)
                    {
                        foreach (DataGridViewRow dgvrowTwo in dgvProductCreation.Rows)
                        {
                            if (dgvrowOne.Index != dgvrowTwo.Index)
                            {
                                if (dgvrowOne.Cells["dgvcmbtgodown"].Value != null && dgvrowTwo.Cells["dgvcmbtgodown"].Value != null)
                                    if (dgvrowOne.Cells["dgvcmbtgodown"].Value.ToString() == dgvrowTwo.Cells["dgvcmbtgodown"].Value.ToString())
                                    {
                                        isGodownCheck = true;
                                        Messages.InformationMessage("Repeatation of same Godown");
                                        dgvProductCreation.Focus();
                                        dgvProductCreation.CurrentCell = dgvProductCreation.Rows[dgvrowTwo.Index].Cells["dgvcmbtgodown"];
                                        dgvProductCreation.BeginEdit(true);
                                        break;
                                    }
                            }
                        }
                        if (isGodownCheck == true)
                        {
                            break;
                        }
                    }
                }
                if (cmbDefaultGodown.Enabled && cmbDefaultRack.Enabled && cmbAllowBatch.SelectedIndex == 1)
                {
                    foreach (DataGridViewRow dgvrowOne in dgvProductCreation.Rows)
                    {
                        foreach (DataGridViewRow dgvrowTwo in dgvProductCreation.Rows)
                        {
                            if (dgvrowOne.Index != dgvrowTwo.Index)
                            {
                                if (dgvrowOne.Cells["dgvcmbtgodown"].Value != null && dgvrowOne.Cells["dgvcmbrack"].Value != null && dgvrowOne.Cells["dgvtxtbatch"].Value != null && dgvrowTwo.Cells["dgvcmbtgodown"].Value != null
                                    && dgvrowTwo.Cells["dgvcmbrack"].Value != null && dgvrowTwo.Cells["dgvtxtbatch"].Value != null)
                                    if (dgvrowOne.Cells["dgvcmbtgodown"].Value.ToString() == dgvrowTwo.Cells["dgvcmbtgodown"].Value.ToString() &&
                                         dgvrowOne.Cells["dgvcmbrack"].Value.ToString() == dgvrowTwo.Cells["dgvcmbrack"].Value.ToString() &&
                                        dgvrowOne.Cells["dgvtxtbatch"].Value.ToString() == dgvrowTwo.Cells["dgvtxtbatch"].Value.ToString())
                                    {
                                        isGodownCheck = true;
                                        Messages.InformationMessage("Repeatation of same Godown Rack and Batch ");
                                        dgvProductCreation.Focus();
                                        dgvProductCreation.CurrentCell = dgvProductCreation.Rows[dgvrowTwo.Index].Cells["dgvcmbrack"];
                                        dgvProductCreation.BeginEdit(true);
                                        break;
                                    }
                            }
                        }
                        if (isGodownCheck == true)
                        {
                            break;
                        }
                    }
                }
                if (!cmbDefaultGodown.Enabled && !cmbDefaultRack.Enabled && cmbAllowBatch.SelectedIndex == 1)
                {
                    foreach (DataGridViewRow dgvrowOne in dgvProductCreation.Rows)
                    {
                        foreach (DataGridViewRow dgvrowTwo in dgvProductCreation.Rows)
                        {
                            if (dgvrowOne.Index != dgvrowTwo.Index)
                            {
                                if (dgvrowOne.Cells["dgvtxtbatch"].Value != null && dgvrowTwo.Cells["dgvtxtbatch"].Value != null)
                                    if (dgvrowOne.Cells["dgvtxtbatch"].Value.ToString() == dgvrowTwo.Cells["dgvtxtbatch"].Value.ToString())
                                    {
                                        isGodownCheck = true;
                                        Messages.InformationMessage("Repeatation of same batch No");
                                        dgvProductCreation.Focus();
                                        dgvProductCreation.CurrentCell = dgvProductCreation.Rows[dgvrowTwo.Index].Cells["dgvtxtbatch"];
                                        dgvProductCreation.BeginEdit(true);
                                        break;
                                    }
                            }
                        }
                        if (isGodownCheck == true)
                        {
                            break;
                        }
                    }
                }
                if (cmbDefaultGodown.Enabled && !cmbDefaultRack.Enabled && cmbAllowBatch.SelectedIndex == 1)
                {
                    foreach (DataGridViewRow dgvrowOne in dgvProductCreation.Rows)
                    {
                        foreach (DataGridViewRow dgvrowTwo in dgvProductCreation.Rows)
                        {
                            if (dgvrowOne.Index != dgvrowTwo.Index)
                            {
                                if (dgvrowOne.Cells["dgvcmbtgodown"].Value != null && dgvrowOne.Cells["dgvtxtbatch"].Value != null && dgvrowTwo.Cells["dgvcmbtgodown"].Value != null
                                    && dgvrowTwo.Cells["dgvtxtbatch"].Value != null)
                                    if (dgvrowOne.Cells["dgvcmbtgodown"].Value.ToString() == dgvrowTwo.Cells["dgvcmbtgodown"].Value.ToString() &&
                                        dgvrowOne.Cells["dgvtxtbatch"].Value.ToString() == dgvrowTwo.Cells["dgvtxtbatch"].Value.ToString())
                                    {
                                        isGodownCheck = true;
                                        Messages.InformationMessage("Repeatation of same Godown and Batch ");
                                        dgvProductCreation.Focus();
                                        dgvProductCreation.CurrentCell = dgvProductCreation.Rows[dgvrowTwo.Index].Cells["dgvcmbtgodown"];
                                        dgvProductCreation.BeginEdit(true);
                                        break;
                                    }
                            }
                        }
                        if (isGodownCheck == true)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:87" + ex.Message;
            }
            return isGodownCheck;
        }
        /// <summary>
        /// Function to call godown form to create a new godown
        /// </summary>
        public void CreateGodownFromGrid()
        {
            try
            {
                isCallFromGrid = true;
                if (dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value != null)
                {
                    decGowdownId = Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value);
                }
                else
                {
                    decGowdownId = -1;
                }
                frmGodown frmGodownObj = new frmGodown();
                frmGodownObj.MdiParent = formMDI.MDIObj;
                frmGodownObj.CallFromProdutCreation(this);
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:88" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call Rack form to create a new rack
        /// </summary>
        /// <param name="decId"></param>
        public void CreateRackFromGrid(decimal decId)
        {
            try
            {
                isCallFromGrid = true;
                if (dgvProductCreation.CurrentRow.Cells["dgvcmbrack"].Value != null)
                {
                    decRackId = Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbrack"].Value);
                    decGodownIdForRack = Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value);
                }
                frmRack frmRackObj = new frmRack();
                frmRackObj.MdiParent = formMDI.MDIObj;
                frmRackObj.CallFromProdutCreationForRackCreation(this, decId);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:89" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmPOS to create a new product
        /// </summary>
        /// <param name="frmPOS"></param>
        /// <param name="isFromProduct"></param>
        public void CallFromPOSForProductCreation(frmPOS frmPOS, bool isFromProduct)
        {
            try
            {
                isFromPOSItemCombo = isFromProduct;
                this.frmPosObj = frmPOS;
                base.Show();
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:90" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmPurchaseInvoice to create a new product
        /// </summary>
        /// <param name="frmPurchaseInvoiceObj"></param>
        public void CallFromPurchaseInvoice(frmPurchaseInvoice frmPurchaseInvoiceObj)
        {
            try
            {
                frmPurchaseInvoiceObj.Enabled = false;
                this.frmPurchaseInvoiceObj = frmPurchaseInvoiceObj;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:91" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmMaterialReceipt to create a new product
        /// </summary>
        /// <param name="frmMaterialReceiptObj"></param>
        public void CallFromMaterialReceipt(frmMaterialReceipt frmMaterialReceiptObj)
        {
            try
            {
                frmMaterialReceiptObj.Enabled = false;
                this.frmMaterialReceiptObj = frmMaterialReceiptObj;
                base.Show();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:92" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmPurchaseReturn to create a new product
        /// </summary>
        /// <param name="frmPurchaseReturnObj"></param>
        public void CallFromPurcahseReturn(frmPurchaseReturn frmPurchaseReturnObj)
        {
            try
            {
                frmPurchaseReturnObj.Enabled = false;
                this.frmPurchaseReturnObj = frmPurchaseReturnObj;
                base.Show();
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:93" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmSalesInvoice to create a new product
        /// </summary>
        /// <param name="frmSalesInvoiceObj"></param>
        public void CallFromDSalesInvoice(frmSalesInvoice frmSalesInvoiceObj)
        {
            try
            {
                frmSalesInvoiceObj.Enabled = false;
                this.frmSalesInvoiceObj = frmSalesInvoiceObj;
                base.Show();
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:94" + ex.Message;
            }
        }
        /// <summary>
        ///  Function to call this form from frmSalesInvoice to create a new product
        /// </summary>
        /// <param name="frmSalesInvoiceObj"></param>
        public void CallFromPhysicalStock(frmPhysicalStock frmPhysicalStockObj)
        {
            try
            {
                frmPhysicalStockObj.Enabled = false;
                this.frmPhysicalStockObj = frmPhysicalStockObj;
                base.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:95" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmDeliveryNote to create a new product
        /// </summary>
        /// <param name="frmDeliveryNote"></param>
        public void CallFromDeliveryNote(frmDeliveryNote frmDeliveryNote)
        {
            try
            {
                frmDeliveryNote.Enabled = false;
                this.frmDeliveryNoteObj = frmDeliveryNote;
                base.Show();
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:96" + ex.Message;
              
            }
        }
        /// <summary>
        ///  Function to call this form from frmPurchaseOrder to create a new product
        /// </summary>
        /// <param name="purchaseOrderobj"></param>
        /// <param name="strLedgerName"></param>
        public void CallFromPurchaseOrder(frmPurchaseOrder purchaseOrderobj)
        {
            try
            {
                base.Show();
                this.frmpurchaseOrderObj = purchaseOrderobj;
                frmpurchaseOrderObj.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:97" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmSalesOrder to create a new product
        /// </summary>
        /// <param name="frmSalesOrder"></param>
        public void CallFromSalesOrder(frmSalesOrder frmSalesOrder)
        {
            try
            {
                base.Show();
                frmSalesOrder.Enabled = false;
                this.frmSalesOrderObj = frmSalesOrder;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:98" + ex.Message;
            }
        }
        /// <summary>
        /// Function to call this form from frmSalesQuotation to create a new product
        /// </summary>
        /// <param name="frmSalesQuotation"></param>
        public void CallFromSalesQuotation(frmSalesQuotation frmSalesQuotation)
        {
            try
            {
                frmSalesQuotation.Enabled = false;
                this.frmSalesQuotationObj = frmSalesQuotation;
                base.Show();
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:99" + ex.Message;
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Form Button close click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrmClose_Click(object sender, EventArgs e)
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
              formMDI.infoError.ErrorString = "PC:100" + ex.Message;
            }
        }
        /// <summary>
        /// Button close click
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
                formMDI.infoError.ErrorString = "PC:101" + ex.Message;
            }
        }
        /// <summary>
        /// Button brandAdd click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrandAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBrand.SelectedValue != null)
                {
                    strBrandName = cmbBrand.SelectedValue.ToString();
                }
                else
                {
                    strBrandName = string.Empty;
                }
                frmBrand frmBrand = new frmBrand();
                frmBrand.MdiParent = formMDI.MDIObj;
                frmBrand open = Application.OpenForms["frmBrand"] as frmBrand;
                if (open == null)
                {
                    frmBrand.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmBrand.MdiParent = formMDI.MDIObj;
                    frmBrand.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
             formMDI.infoError.ErrorString = "PC:102" + ex.Message;
            }
        }
        /// <summary>
        /// When form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductCreation_Load(object sender, EventArgs e)
        {
            try
            {
                BrandComboFill();
                SizeComboFill();
                TaxComboFill();
                isRackFill = false;
                GodownComboFill();
                isRackFill = true;
                BatchComboFill();
                BomFill();
                UnitComboFill();
                ModelNoComboFill();
                TaxApplicableOn();
                RackComboFill();
                OpeningStockComboFill();
                MultipleUnitComboFill();
                GroupComboFill();
                InitialValues();
                btnDelete.Enabled = false;
                AutomaticCodeGenaration();
                InitialSettings();
                FillBatch(false, null);
                ClearFunction();
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:103" + ex.Message;
            }
        }
        /// <summary>
        /// Button size add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSizeAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSize.SelectedValue != null)
                {
                    strSizeName = cmbSize.SelectedValue.ToString();
                }
                else
                {
                    strSizeName = string.Empty;
                }
                frmSize frmSize = new frmSize();
                frmSize.MdiParent = formMDI.MDIObj;
                frmSize open = Application.OpenForms["frmSize"] as frmSize;
                if (open == null)
                {
                    frmSize.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmSize.MdiParent = formMDI.MDIObj;
                    frmSize.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:104" + ex.Message;
            }
        }
        /// <summary>
        /// Button tax add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTaxAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTax.SelectedValue != null)
                {
                    strTaxName = cmbTax.SelectedValue.ToString();
                }
                else
                {
                    strTaxName = string.Empty;
                }
                frmTax frmTax = new frmTax();
                frmTax.MdiParent = formMDI.MDIObj;
                frmTax open = Application.OpenForms["frmTax"] as frmTax;
                if (open == null)
                {
                    frmTax.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmTax.MdiParent = formMDI.MDIObj;
                    frmTax.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:105" + ex.Message;
            }
        }
        /// <summary>
        /// Button default godown add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaultGodownAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDefaultGodown.SelectedValue != null)
                {
                    strGodownName = cmbDefaultGodown.SelectedValue.ToString();
                }
                else
                {
                    strGodownName = string.Empty;
                }
                frmGodown frmGodown = new frmGodown();
                frmGodown.MdiParent = formMDI.MDIObj;
                frmGodown open = Application.OpenForms["frmGodown"] as frmGodown;
                if (open == null)
                {
                    frmGodown.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmGodown.MdiParent = formMDI.MDIObj;
                    frmGodown.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:106" + ex.Message;
            }
        }
        /// <summary>
        /// Button unit add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnitAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUnit.SelectedValue != null)
                {
                    strUnitName = cmbUnit.SelectedValue.ToString();
                }
                else
                {
                    strUnitName = string.Empty;
                }
                frmUnit frmUnit = new frmUnit();
                frmUnit.MdiParent = formMDI.MDIObj;
                frmUnit open = Application.OpenForms["frmUnit"] as frmUnit;
                if (open == null)
                {
                    frmUnit.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmUnit.MdiParent = formMDI.MDIObj;
                    frmUnit.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:107" + ex.Message;
            }
        }
        /// <summary>
        /// Button model number add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModalNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbModalNo.SelectedValue != null)
                {
                    strModelName = cmbModalNo.SelectedValue.ToString();
                }
                else
                {
                    strModelName = string.Empty;
                }
                frmModalNo frmModelObj = new frmModalNo();
                frmModelObj.MdiParent = formMDI.MDIObj;
                frmModelObj.CallFromProdutCreation(this);
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:108" + ex.Message;
            }
        }
        /// <summary>
        /// Button default add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaultAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDefaultRack.SelectedValue != null)
                {
                    strRackName = cmbDefaultRack.SelectedValue.ToString();
                }
                else
                {
                    strRackName = string.Empty;
                }
                frmRack frmRack = new frmRack();
                frmRack.MdiParent = formMDI.MDIObj;
                frmRack open = Application.OpenForms["frmRack"] as frmRack;
                if (open == null)
                {
                    frmRack.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmRack.MdiParent = formMDI.MDIObj;
                    frmRack.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:109" + ex.Message;
            }
        }
        /// <summary>
        /// PurchaseRate textBox mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseRate_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (txtPurchaseRate.Text.Trim() == "0")
                {
                    txtPurchaseRate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:110" + ex.Message;
            }
        }
        /// <summary>
        /// Button group add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbGroup.SelectedValue != null)
                {
                    strGroupName = cmbGroup.SelectedValue.ToString();
                }
                else
                {
                    strGroupName = string.Empty;
                }
                frmProductGroup frmProductGroup = new frmProductGroup();
                frmProductGroup.MdiParent = formMDI.MDIObj;
                frmProductGroup open = Application.OpenForms["frmProductGroup"] as frmProductGroup;
                if (open == null)
                {
                    frmProductGroup.WindowState = FormWindowState.Normal;//Edited by Najma
                    frmProductGroup.MdiParent = formMDI.MDIObj;
                    frmProductGroup.CallFromProdutCreation(this);
                }
                else
                {
                    open.MdiParent = formMDI.MDIObj;
                    open.CallFromProdutCreation(this);
                    open.BringToFront();
                    if (open.WindowState == FormWindowState.Minimized)
                    {
                        open.WindowState = FormWindowState.Normal;
                    }
                }
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:111" + ex.Message;
            }
        }
        /// <summary>
        /// To fill tax applicationNo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTax.SelectedIndex == 0)
                {
                    cmbTaxApplicableOn.Enabled = false;
                }
                else
                {
                    cmbTaxApplicableOn.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:112" + ex.Message;
            }
        }
        /// <summary>
        /// to work on cmbBom cell leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBom_Leave(object sender, EventArgs e)
        {
            try
            {
                // if (k.KeyCode != Keys.Escape)
                if (cmbBom.SelectedIndex == 1)
                {
                    if (txtName.Text.Trim() != string.Empty)
                    {
                        strProductName = txtName.Text.Trim();
                        frmBomObj = new frmProductBom();
                        frmBomObj.MdiParent = formMDI.MDIObj;
                        frmProductBom open = Application.OpenForms["frmProductBom"] as frmProductBom;
                        if (open == null)
                        {
                            if (isBomFromRegister && btnSave.Text == "Update")
                            {
                                BomTableForUpdate();
                                frmBomObj.CallFromProdutCreationForUpadte(this, strProductName, decProductIdForEdit, dtblBomForEdit);
                            }
                            else
                            {
                                if (strProductName != string.Empty)
                                {
                                    if (!isSaveBomCheck && btnSave.Text == "Update")
                                    {
                                        frmBomObj.CallFromProdutCreation(this, strProductName, decProductIdForEdit);
                                    }
                                    //------------------------------------CheckingBomSavedOnce-----------------------------------------------------------------------------------
                                    else if (isSaveBomCheck)
                                    {
                                        frmBomObj.CallFromProdutCreationAgain(this, strProductName, dtblBom);
                                    }
                                    else
                                    {
                                        frmBomObj.CallFromProdutCreation(this, strProductName);
                                    }
                                }
                            }
                            this.Enabled = false;
                        }
                    }
                    else
                    {
                        Messages.InformationMessage("Enter product name");
                        cmbBom.SelectedIndex = 0;
                        txtName.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:113" + ex.Message;
            }
        }
        /// <summary>
        /// to change the corresponding values when txtPurchaserate leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseRate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtPurchaseRate.Text == string.Empty)
                {
                    txtPurchaseRate.Text = "0";
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:114" + ex.Message;
            }
        }
        /// <summary>
        /// to change the corresponding values when txtMrp Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMrp_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtMrp.Text == string.Empty)
                {
                    txtMrp.Text = "0";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:115" + ex.Message;
            }
        }
        /// <summary>
        /// to change the corresponding values when txtMaximumStock Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaximumStock_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtMaximumStock.Text == string.Empty)
                {
                    txtMaximumStock.Text = "0";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:116" + ex.Message;
            }
        }
        /// <summary>
        /// to change the corresponding values when txtSalesRate Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtSalesRate.Text == string.Empty)
                {
                    txtSalesRate.Text = "0";
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:117" + ex.Message;
            }
        }
        /// <summary>
        /// to change the corresponding values when txtMinimumStock Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinimumStock_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtMinimumStock.Text == string.Empty)
                {
                    txtMinimumStock.Text = "0";
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:118" + ex.Message;
            }
        }
        /// <summary>
        /// to change the corresponding values when txtMrp mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMrp_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (txtMrp.Text == "0")
                {
                    txtMrp.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:119" + ex.Message;
            }
        }
        /// <summary>
        /// To work on txtMaximumStock mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaximumStock_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (txtMaximumStock.Text == "0")
                {
                    txtMaximumStock.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:120" + ex.Message;
            }
        }
        /// <summary>
        /// To work on txtSalesRate mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRate_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (txtSalesRate.Text == "0")
                {
                    txtSalesRate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:121" + ex.Message;
            }
        }
        /// <summary>
        /// To work on txtMinimumStock mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinimumStock_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (txtMinimumStock.Text == "0")
                {
                    txtMinimumStock.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:122" + ex.Message;
            }
        }
        /// <summary>
        /// To woek on cmbMultipleUnit cell leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMultipleUnit_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (k.KeyCode != Keys.Escape)
                if (cmbMultipleUnit.SelectedIndex == 1)
                {
                    UnitSP spUnit = new UnitSP();
                    strProductName = txtName.Text.Trim();
                    decimal decUnitId = Convert.ToDecimal(cmbUnit.SelectedValue);
                    string strUnit = spUnit.UnitName(Convert.ToDecimal(cmbUnit.SelectedValue));
                    frmProductMultipleUnit frmMultipleUnitObj = new frmProductMultipleUnit();
                    frmMultipleUnitObj.MdiParent = formMDI.MDIObj;
                    frmProductMultipleUnit open = Application.OpenForms["frmProductMultipleUnit"] as frmProductMultipleUnit;
                    if (open == null)
                    {
                        if (isMulUnitFromRgister && btnSave.Text == "Update")
                        {
                            if (!isCheck)
                            {
                                MultipleUnitTableForUpdate();
                                frmMultipleUnitObj.CallFromProdutCreationForUpadte(this, strProductName, decProductIdForEdit, dtblMulUnitForEdit, strUnitNameForGrid, decUnitIdForUpdate);
                                isGridHasToClear = true;
                            }
                            else
                            {
                                if (strProductName != string.Empty)
                                {
                                    if (cmbUnit.SelectedIndex != -1)
                                    {
                                        if (isSaveMulUnitCheck && (decUnitId == decUnitIdSelectedWhenMulUntCalled))
                                        {
                                            frmMultipleUnitObj.CallFromProdutCreationAgain(this, strProductName, decUnitId, strUnit, dtblMulUnit);
                                            //txtPartNo.Focus();
                                        }
                                        else
                                        {
                                            frmMultipleUnitObj.CallFromProdutCreation(this, strProductName, decUnitId, strUnit);
                                            //txtPartNo.Focus();
                                        }
                                    }
                                    else
                                    {
                                        Messages.InformationMessage("Select default unit");
                                        cmbMultipleUnit.SelectedIndex = 0;
                                        cmbUnit.Focus();
                                    }
                                }
                                else
                                {
                                    Messages.InformationMessage("Enter product name");
                                    cmbMultipleUnit.SelectedIndex = 0;
                                    txtName.Focus();
                                }
                            }
                        }
                        else
                        {
                            if (strProductName != string.Empty)
                            {
                                if (cmbUnit.SelectedIndex != -1)
                                {
                                    if (isSaveMulUnitCheck && (decUnitId == decUnitIdSelectedWhenMulUntCalled))
                                    {
                                        frmMultipleUnitObj.CallFromProdutCreationAgain(this, strProductName, decUnitId, strUnit, dtblMulUnit);
                                        //txtPartNo.Focus();
                                    }
                                    else
                                    {
                                        frmMultipleUnitObj.CallFromProdutCreation(this, strProductName, decUnitId, strUnit);
                                        //txtPartNo.Focus();
                                    }
                                }
                                else
                                {
                                    Messages.InformationMessage("Select default unit");
                                    cmbMultipleUnit.SelectedIndex = 0;
                                    cmbUnit.Focus();
                                }
                            }
                            else
                            {
                                Messages.InformationMessage("Enter product name");
                                cmbMultipleUnit.SelectedIndex = 0;
                                txtName.Focus();
                            }
                        }
                    }
                    else if (open != null)
                    {
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                    }
                    if (dtblForUnitIdInOpeNingStock != null && dtblForUnitIdInOpeNingStock.Rows.Count > 0)
                    {
                        dgvcmbUnit.DataSource = dtblForUnitIdInOpeNingStock;
                        dgvcmbUnit.DisplayMember = "unitName";
                        dgvcmbUnit.ValueMember = "dgvtxtunitId";
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:123" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbOpeningStock SelectedValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOpeningStock_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbOpeningStock.SelectedIndex == 1)
                {
                    dgvProductCreation.Enabled = true;
                    GodownGrigComboFill();
                    RackGridComboFill();
                    DefaultUnitFillingInProductGrid();
                    if (decProductIdForEdit != 0)
                    {
                        isstockPostingGridFil = true;
                    }
                }
                else
                {
                    dgvProductCreation.Enabled = false;
                    dgvProductCreation.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
            formMDI.infoError.ErrorString = "PC:124" + ex.Message;
            }
        }
        /// <summary>
        /// Woek on cmbAllowBatch SelectedValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAllowBatch_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAllowBatch.Text == "Yes")
                {
                    dgvProductCreation.Columns["dgvtxManfDate"].Visible = true;
                    dgvProductCreation.Columns["dgvtxtExpDate"].Visible = true;
                    dgvProductCreation.Columns["dgvtxtbatch"].Visible = true;
                    txtPartNo.Enabled = false;
                }
                else
                {
                    dgvProductCreation.Columns["dgvtxManfDate"].Visible = false;
                    dgvProductCreation.Columns["dgvtxtExpDate"].Visible = false;
                    dgvProductCreation.Columns["dgvtxtbatch"].Visible = false;
                    txtPartNo.Enabled = true;
                }
                if (dgvProductCreation.Enabled)
                {
                    foreach (DataGridViewRow dgvRowObj in dgvProductCreation.Rows)
                    {
                        if (!dgvRowObj.IsNewRow)
                        {
                            DataGridViewCellEventArgs dgvArg = new DataGridViewCellEventArgs(0, dgvRowObj.Index);
                            CheckingForIncompleteRowInGrid(dgvArg);
                        }
                    }
                    dgvProductCreation.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:125" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbUnit Cell Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUnit_Leave(object sender, EventArgs e)
        {
            try
            {
                UnitSP spUnit = new UnitSP();
                DataTable dtbl = new DataTable();
                decId = Convert.ToDecimal(cmbUnit.SelectedValue);
                strUnitNameForGrid = spUnit.UnitName(decId);
                isSaveMulUnitCheck = false;
                if (dtblMulUnit != null)
                {
                    dtblMulUnit.Clear();
                }//if unit changed
                if (dtblForUnitIdInOpeNingStock != null)
                {
                    dtblForUnitIdInOpeNingStock.Clear();//if unit changed
                }
                if (dtblFromMulUnitForUpdate != null)
                {
                    dtblFromMulUnitForUpdate.Clear();//if unit changed
                }
                if (btnSave.Text == "Update")
                {
                    cmbMultipleUnit.SelectedIndex = 0;
                    isMulUnitFromRgister = false;
                    isCheck = true;
                }
                if (dgvProductCreation.Rows.Count != 0)
                {
                    for (int inI = 0; inI <= dgvProductCreation.Rows.Count - 1; inI++)
                    {
                        dgvProductCreation.Rows[inI].Cells["dgvtxtunit"].Value = strUnitNameForGrid;
                        DefaultUnitFillingInProductGrid();
                    }
                }
                else
                {
                    dgvProductCreation.Rows[0].Cells["dgvtxtunit"].Value = strUnitNameForGrid;
                    DefaultUnitFillingInProductGrid();
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "PC:126" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvProductCreation RowsAdded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (dgvProductCreation.Rows.Count != 1)
                {
                    SlNo();
                }
                if (e.RowIndex != 0)
                {
                    dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtrate"].ReadOnly = true;
                }
                // dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtunit"].Value = strUnitNameForGrid;
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:127" + ex.Message;
            }
        }
        /// <summary>
        /// Work on button Save click
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
                  formMDI.infoError.ErrorString = "PC:128" + ex.Message;
            }
        }
        /// <summary>
        /// Work on clear button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFunction();
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:129" + ex.Message;
            }
        }
        /// <summary>
        /// work on dgvProductCreation CellValueChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                CheckInvalidEntries(e);
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtqty"].Value != null && dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtqty"].Value.ToString() != "")
                    {
                        if (dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtrate"].Value != null && dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtrate"].Value.ToString() != "")
                        {
                            int inDecimalPlace = PublicVariables._inNoOfDecimalPlaces;
                            decimal decQuantity = Convert.ToDecimal(dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtqty"].Value.ToString());
                            decimal decRate = Convert.ToDecimal(dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtrate"].Value.ToString());
                            decimal decTotal = Math.Round(decQuantity * decRate, inDecimalPlace);
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtamount"].Value = decTotal;
                        }
                        else
                        {
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtamount"].Value = 0;
                        }
                    }
                    TextBox txtMFD = new TextBox();
                    TextBox txtEXPD = new TextBox();
                    if ((dgvProductCreation.Columns[e.ColumnIndex].Name == "dgvtxManfDate" || dgvProductCreation.Columns[e.ColumnIndex].Name == "dgvtxtExpDate") && (cmbAllowBatch.SelectedIndex == 1))
                    {
                        DateValidation objDateValidation = new DateValidation();
                        TextBox txt = new TextBox();
                        if (dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value != null)
                        {
                            txt.Text = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value.ToString();
                            bool isDate = objDateValidation.DateValidationFunctionMFDEXPD(txt);
                            if (isDate)
                            {
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value = txt.Text;
                                txtMFD.Text = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value.ToString();
                            }
                            else
                            {
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value = string.Empty;
                                txtMFD.Text = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value.ToString();
                            }
                        }
                        if (dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value != null)
                        {
                            txt.Text = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value.ToString();
                            bool isDate = objDateValidation.DateValidationFunctionMFDEXPD(txt);
                            if (isDate)
                            {
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value = txt.Text;
                                txtEXPD.Text = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value.ToString();
                            }
                            else
                            {
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value = string.Empty;
                                txtEXPD.Text = dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value.ToString();
                            }
                        }
                        if (txtEXPD.Text != String.Empty && txtMFD.Text != string.Empty)
                        {
                            if (Convert.ToDateTime(txtEXPD.Text) < Convert.ToDateTime(txtMFD.Text))
                            {
                                MessageBox.Show("EXPD should be greater than or equal to MFD", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value = string.Empty;
                            }
                        }
                    }
                    for (int inI = 0; inI < dgvProductCreation.RowCount - 1; inI++)
                    {
                        if (inI == 0)
                        {
                            dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].ReadOnly = false;
                            dgvProductCreation.Rows[inI].Cells["dgvtxtunit"].Value = strUnitNameForGrid;
                        }
                        else
                        {
                            dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].Value = dgvProductCreation.Rows[0].Cells["dgvtxtrate"].Value;
                            dgvProductCreation.Rows[inI].Cells["dgvtxtrate"].ReadOnly = true;
                            dgvProductCreation.Rows[inI].Cells["dgvtxtunit"].Value = strUnitNameForGrid;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:130" + ex.Message;
            }
        }
        /// <summary>
        /// Work on Delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUserPrivilege.PrivilegeCheck(PublicVariables._decCurrentUserId, this.Name, btnDelete.Text))
                {
                    DeleteFuntion();
                }
                else
                {
                    Messages.NoPrivillageMessage();
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:131" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvtxtrate KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvtxtrate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvProductCreation.CurrentCell != null)
                {
                    if (dgvProductCreation.Columns[dgvProductCreation.CurrentCell.ColumnIndex].Name == "dgvtxtrate")
                    {
                        Common.DecimalValidation(sender, e, false);
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:132" + ex.Message;
            }
        }
        /// <summary>
        /// Work on gridCombo KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCombo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DataGridViewCell cell = dgvProductCreation.CurrentCell;
                dgvProductCreation.CurrentCell = cell;
                dgvProductCreation.BeginEdit(true);
                if (dgvProductCreation.CurrentCell != null)
                {
                    dgvProductCreation.BeginEdit(true);
                    if (e.KeyChar == (char)Keys.Down)
                    {
                        dgvProductCreation.BeginEdit(true);
                    }
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        dgvProductCreation.BeginEdit(true);
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:133" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvProductCreation EditingControlShowing event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridViewTextBoxEditingControl dgvtxtqty = e.Control as DataGridViewTextBoxEditingControl;
                DataGridViewTextBoxEditingControl dgvtxtrate = e.Control as DataGridViewTextBoxEditingControl;
                if (dgvtxtqty != null)
                {
                    dgvtxtqty.KeyPress += dgvtxtqty_KeyPress;
                }
                if (dgvtxtrate != null)
                {
                    dgvtxtrate.KeyPress += dgvtxtrate_KeyPress;
                }
                if (e.Control is ComboBox)
                {
                    //SendKeys.Send("{F4}");
                    //e.Control.KeyPress += gridCombo_KeyPress;
                }
                TextBoxControl = e.Control as DataGridViewTextBoxEditingControl;
                if (TextBoxControl != null)
                {
                    if (dgvProductCreation.CurrentCell != null && dgvProductCreation.Columns[dgvProductCreation.CurrentCell.ColumnIndex].Name == "dgvtxtbatch")
                    {
                        TextBoxControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TextBoxControl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        TextBoxControl.AutoCompleteCustomSource = Batch;
                    }
                }
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "PC:134" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbOpeningStock Leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOpeningStock_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbOpeningStock.SelectedIndex == 1)
                {
                    if (cmbUnit.SelectedIndex == -1)
                    {
                        Messages.InformationMessage("Select default unit");
                        cmbOpeningStock.SelectedIndex = 0;
                        cmbUnit.Focus();
                    }
                    if (!isstockPostingGridFil)//Checking during updation
                    {
                        if (isOpeningStockForUpdate)
                        {
                            OpeningStockGridFill();
                        }
                        if (isBatchForUpdate)
                        {
                            OpeningStockGridWithBathFill();
                        }
                    }
                    else
                    {
                        if (!isGridHasToClear)
                        {
                            dgvProductCreation.Rows.Clear();
                            isOpeningStockChanged = true;
                        }
                    }
                    if (cmbMultipleUnit.SelectedIndex == 1 && dtblForUnitIdInOpeNingStock != null && dtblForUnitIdInOpeNingStock.Rows.Count > 0)
                    {
                        UnitSP spUnit = new UnitSP();
                        DataTable dtbl = new DataTable();
                        decId = Convert.ToDecimal(cmbUnit.SelectedValue);
                        strUnitNameForGrid = spUnit.UnitName(decId);
                        DataRow dr = dtblForUnitIdInOpeNingStock.NewRow();
                        dr["dgvtxtUnitId"] = decId;
                        dr["unitName"] = strUnitNameForGrid;
                        dr["CnvertionRate"] = 0;
                        dr["quantities"] = 0;
                        dr["extra1"] = string.Empty;
                        dr["extra2"] = string.Empty;
                        dr["extraDate"] = DateTime.Now;
                        dtblForUnitIdInOpeNingStock.Rows.InsertAt(dr, (dtblForUnitIdInOpeNingStock.Rows.Count + 1));
                        dgvcmbUnit.DataSource = dtblForUnitIdInOpeNingStock;
                        dgvcmbUnit.DisplayMember = "unitName";
                        dgvcmbUnit.ValueMember = "dgvtxtunitId";
                        for (int i = 0; i < dgvProductCreation.Rows.Count; i++)
                        {
                            if (!dgvProductCreation.Rows[i].IsNewRow)
                            {
                                dgvProductCreation.Rows[i].Cells["dgvcmbUnit"].Value = dtblForUnitIdInOpeNingStock.Rows[0][1];
                            }
                        }
                    }
                    else if (decId != 0)
                    {
                        DefaultUnitFillingInProductGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:135" + ex.Message;
            }
        }
        /// <summary>
        /// Work on linkbutton Remove click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dgvProductCreation.RowCount > 1)
                {
                    if (dgvProductCreation.CurrentRow.Index + 1 != dgvProductCreation.RowCount)
                    {
                        //SlNo();
                        if (btnSave.Text == "Update")
                        {
                            if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (dgvProductCreation.CurrentRow.Cells["dgvtxtstockpostId"].Value != null && dgvProductCreation.CurrentRow.Cells["dgvtxtstockpostId"].Value.ToString() != "")
                                {
                                    // strArrOfRemove[inArrOfRemoveIndex] = (dgvProductCreation.CurrentRow.Cells["dgvtxtstockpostId"].Value.ToString());
                                    //inArrOfRemoveIndex++;
                                    lstArrOfRemove.Add(dgvProductCreation.CurrentRow.Cells["dgvtxtstockpostId"].Value.ToString());
                                    if (dgvProductCreation.CurrentRow.Cells["dgvtxtbatchId"].Value != null && dgvProductCreation.CurrentRow.Cells["dgvtxtbatchId"].Value.ToString() != "")
                                    {
                                        lststrArrBatchRemove.Add(dgvProductCreation.CurrentRow.Cells["dgvtxtbatchId"].Value.ToString());
                                        //strArrBatchremove[inArrOfBatchIndex] = (dgvProductCreation.CurrentRow.Cells["dgvtxtbatchId"].Value.ToString());
                                        //inArrOfBatchIndex++;
                                    }
                                    dgvProductCreation.Rows.RemoveAt(dgvProductCreation.CurrentRow.Index);
                                }
                                else
                                {
                                    dgvProductCreation.Rows.RemoveAt(dgvProductCreation.CurrentRow.Index);
                                }
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Do you want to remove current row ?", "Oneaccount", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                dgvProductCreation.Rows.RemoveAt(dgvProductCreation.CurrentRow.Index);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:136" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvProductCreation DataError event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                e.ThrowException = false;
            }
            catch (Exception ex)
            {
                   formMDI.infoError.ErrorString = "PC:137" + ex.Message;
            }
        }
        private void dgvtxtqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgvProductCreation.CurrentCell != null)
                {
                    if (dgvProductCreation.Columns[dgvProductCreation.CurrentCell.ColumnIndex].Name == "dgvtxtqty")
                    {
                        Common.NumberOnly(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:138" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvProductCreation CellEndEdit event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvProductCreation.Columns[e.ColumnIndex].Name == "dgvtxtbatch")
                {
                    string strBatch = dgvProductCreation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? "" : dgvProductCreation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (strBatch != "")
                    {
                        BatchSP spBatch = new BatchSP();
                        DataTable dtbl = new DataTable();
                        dtbl = spBatch.BatchViewByName(strBatch, decId);
                        if (dtbl.Rows.Count != 0)
                        {
                            //dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatchId"].Value = Convert.ToDecimal(dtbl.Rows[0]["batchId"]);
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value = dtbl.Rows[0]["expiryDate"];
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value = dtbl.Rows[0]["manufacturingDate"];
                        }
                    }
                    else
                    {
                        dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value = string.Empty;
                        dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value = string.Empty;
                    }
                }
                if (dgvProductCreation.Columns[e.ColumnIndex].Name == "dgvcmbtgodown")
                {
                    RackSP spRack = new RackSP();
                    RackInfo infoRack = new RackInfo();
                    DataTable dtblRack = new DataTable();
                    decimal decGodownId = Convert.ToDecimal(dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbtgodown"].Value);
                    dtblRack = spRack.RackNamesCorrespondingToGodownId(decGodownId);
                    DataRow drow = dtblRack.NewRow();
                    drow["rackName"] = string.Empty;
                    drow["rackId"] = 0;
                    dtblRack.Rows.InsertAt(drow, 0);
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbrack"]);
                    cell.DataSource = dtblRack;
                    cell.ValueMember = "rackId";
                    cell.DisplayMember = "rackName";
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:139" + ex.Message;
            }
        }
        /// <summary>
        /// Work on frmProductCreation form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductCreation_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (frmProductRegisterObj != null)
                {
                    frmProductRegisterObj.Enabled = true;
                    frmProductRegisterObj.GridFill();
                }
                if (frmPosObj != null)
                {
                    frmPosObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmPurchaseInvoiceObj != null)
                {
                    frmPurchaseInvoiceObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmMaterialReceiptObj != null)
                {
                    frmMaterialReceiptObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmPurchaseReturnObj != null)
                {
                    frmPurchaseReturnObj.Enabled = true;
                    frmPurchaseReturnObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmSalesInvoiceObj != null)
                {
                    frmSalesInvoiceObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmPhysicalStockObj != null)
                {
                    frmPhysicalStockObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmSalesReturnObj != null)
                {
                    frmSalesReturnObj.Enabled = true;
                    frmSalesReturnObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmpurchaseOrderObj != null)
                {
                    frmpurchaseOrderObj.Enabled = true;
                    frmpurchaseOrderObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmSalesQuotationObj != null)
                {
                    frmSalesQuotationObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmSalesOrderObj != null)
                {
                    frmSalesOrderObj.Enabled = true;
                    frmSalesOrderObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmDeliveryNoteObj != null)
                {
                    frmDeliveryNoteObj.Enabled = true;
                    frmDeliveryNoteObj.ReturnFromProductCreation(decSaveProduct);
                }
                if (frmStockJournalObj != null)
                {
                    frmStockJournalObj.Enabled = true;
                    frmStockJournalObj.ReturnFromProductCreation(decSaveProduct);
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:140" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbMultipleUnit SelectedIndexChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMultipleUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMultipleUnit.SelectedIndex == 0)
                {
                    DefaultUnitFillingInProductGrid();
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:141" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbDefaultGodown SelectedValueChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDefaultGodown_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDefaultGodown.SelectedIndex > -1 && isRackFill)
                {
                    if (cmbDefaultGodown.SelectedValue != null)
                    {
                        RackComboFill();
                    }
                }
            }
            catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:142" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvProductCreation CellEnter event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
            if (dgvProductCreation.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
            {
                dgvProductCreation.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            else
            {
                dgvProductCreation.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            }
            }
             catch (Exception ex)
            {
                  formMDI.infoError.ErrorString = "PC:143" + ex.Message;
            }

        }
        #endregion
        #region Navigation
        /// <summary>
        /// Work on txtProductCode KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtProductCode.Text == string.Empty || txtProductCode.SelectionStart == 0)
                    {
                        txtName.Focus();
                        txtName.SelectionLength = 0;
                        txtName.SelectionStart = 0;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbGroup.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:144" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbBrand KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBrand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbBrand.Text == string.Empty || cmbBrand.SelectionStart == 0)
                    {
                        cmbUnit.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbModalNo.Enabled)
                    {
                        cmbModalNo.Focus();
                    }
                    else if (cmbSize.Enabled)
                    {
                        cmbSize.Focus();
                    }
                    else
                    {
                        txtMrp.Focus();
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnBrandAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:145" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbSize KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbSize.Text == string.Empty || cmbSize.SelectionStart == 0)
                    {
                        //cmbSize.SelectionStart = 0;
                        //cmbSize.SelectionLength = 0;
                        cmbModalNo.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    txtMrp.Focus();
                    //txtMrp.Clear();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnSizeAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:146" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbGroup KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGroup_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbGroup.Text == string.Empty || cmbGroup.SelectionStart == 0)
                    {
                        if (txtProductCode.Enabled == true)
                        {
                            txtProductCode.SelectionStart = 0;
                            txtProductCode.SelectionLength = 0;
                            txtProductCode.Focus();
                        }
                        else
                        {
                            txtName.SelectionStart = 0;
                            txtName.SelectionLength = 0;
                            txtName.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    // SendKeys.Send("{TAB}");
                    cmbUnit.Focus();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnGroupAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:147" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbUnit KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUnit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbUnit.Text == "" || cmbUnit.SelectionStart == 0)
                    {
                        //cmbUnit.SelectionStart = 0;
                        //cmbUnit.SelectionLength = 0;
                        cmbGroup.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                    cmbBrand.Focus();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnUnitAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:148" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbModalNo KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModalNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbModalNo.Text == string.Empty || cmbModalNo.SelectionStart == 0)
                    {
                        //cmbSize.SelectionStart = 0;
                        //cmbSize.SelectionLength = 0;
                        cmbBrand.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    // SendKeys.Send("{TAB}");
                    cmbSize.Focus();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnModalNo_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:149" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbTax KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbTax.Text == string.Empty || cmbTax.SelectionStart == 0)
                    {
                        //txtSalesRate.SelectionStart = 0;
                        //txtSalesRate.SelectionLength = 0;
                        ////txtSalesRate.Text = string.Empty;
                        txtSalesRate.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    // SendKeys.Send("{TAB}");
                    if (cmbTaxApplicableOn.Enabled == true)
                    {
                        cmbTaxApplicableOn.Focus();
                    }
                    else
                    {
                        cmbBom.Focus();
                    }
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnTaxAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:150" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbTaxApplicableOn KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTaxApplicableOn_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbTaxApplicableOn.Text == string.Empty || cmbTaxApplicableOn.SelectionStart == 0)
                    {
                        cmbTax.SelectionStart = 0;
                        cmbTax.SelectionLength = 0;
                        cmbTax.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:151" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtPurchaseRate KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtPurchaseRate.Text == string.Empty || txtPurchaseRate.SelectionStart == 0)
                    {
                        txtMrp.SelectionStart = 0;
                        txtMrp.SelectionLength = 0;
                        if (txtMrp.Text == "0")
                        {
                            txtMrp.Text = string.Empty;
                        }
                        txtMrp.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    txtSalesRate.Focus();
                    //txtSalesRate.Clear();
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:152" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtSalesRate KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtSalesRate.Text == string.Empty || txtSalesRate.SelectionStart == 0)
                    {
                        txtPurchaseRate.SelectionStart = 0;
                        txtPurchaseRate.SelectionLength = 0;
                        //txtPurchaseRate.Text = string.Empty;
                        txtPurchaseRate.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbTax.Enabled)
                    {
                        cmbTax.Focus();
                    }
                    else
                    {
                        cmbBom.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:153" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtMrp KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMrp_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtMrp.Text == string.Empty || txtMrp.SelectionStart == 0)
                    {
                        if (cmbSize.Enabled)
                        {
                            cmbSize.Focus();
                        }
                        else
                        {
                            cmbModalNo.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    txtPurchaseRate.Focus();
                    //txtPurchaseRate.Clear();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:154" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtMinimumStock KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinimumStock_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtMinimumStock.Text == string.Empty || txtMinimumStock.SelectionStart == 0)
                    {
                        cmbBom.SelectionStart = 0;
                        cmbBom.SelectionLength = 0;
                        cmbBom.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:155" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtMaximumStock KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaximumStock_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtMaximumStock.Text == string.Empty || txtMaximumStock.SelectionStart == 0)
                    {
                        txtMinimumStock.SelectionStart = 0;
                        txtMinimumStock.SelectionLength = 0;
                        //txtMinimumStock.Text = string.Empty;
                        txtMinimumStock.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    txtReorderLevel.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:156" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtReorderLevel KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReorderLevel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtReorderLevel.Text == string.Empty || txtReorderLevel.SelectionStart == 0)
                    {
                        txtMaximumStock.SelectionStart = 0;
                        txtMaximumStock.SelectionLength = 0;
                        //txtMaximumStock.Text = string.Empty;
                        txtMaximumStock.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:157" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbDefaultGodown KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDefaultGodown_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbDefaultGodown.Text == string.Empty || cmbDefaultGodown.SelectionStart == 0)
                    {
                        txtReorderLevel.SelectionStart = 0;
                        txtReorderLevel.SelectionLength = 0;
                        txtReorderLevel.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                    cmbDefaultRack.Focus();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnDefaultGodownAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:158" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbDefaultRack KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDefaultRack_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbDefaultRack.Text == string.Empty || cmbDefaultRack.SelectionStart == 0)
                    {
                        cmbDefaultGodown.SelectionStart = 0;
                        cmbDefaultGodown.SelectionLength = 0;
                        cmbDefaultGodown.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                    cmbAllowBatch.Focus();
                }
                if (e.KeyCode == Keys.C && Control.ModifierKeys == Keys.Alt)
                {
                    SendKeys.Send("{F10}");
                    btnDefaultAdd_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:159" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbAllowBatch KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAllowBatch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbAllowBatch.Text == string.Empty || cmbAllowBatch.SelectionStart == 0)
                    {
                        if (cmbDefaultRack.Enabled)
                        {
                            cmbDefaultRack.SelectionStart = 0;
                            cmbDefaultRack.SelectionLength = 0;
                            cmbDefaultRack.Focus();
                        }
                        else if (cmbDefaultGodown.Enabled)
                        {
                            cmbDefaultGodown.SelectionStart = 0;
                            cmbDefaultGodown.SelectionLength = 0;
                            cmbDefaultGodown.Focus();
                        }
                        else
                        {
                            txtReorderLevel.SelectionStart = 0;
                            txtReorderLevel.SelectionLength = 0;
                            txtReorderLevel.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:160" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbMultipleUnit KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMultipleUnit_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbMultipleUnit.Text == string.Empty || cmbMultipleUnit.SelectionStart == 0)
                    {
                        if (cmbMultipleUnit.SelectedIndex == 0)
                        {
                            cmbAllowBatch.SelectionStart = 0;
                            cmbAllowBatch.SelectionLength = 0;
                            cmbAllowBatch.Focus();
                        }
                        else
                        {
                            if (cmbDefaultRack.Enabled)
                            {
                                cmbDefaultRack.Focus();//Dont change focus
                            }
                            else if (cmbDefaultGodown.Enabled)
                            {
                                cmbDefaultGodown.Focus();
                            }
                            else
                            {
                                txtReorderLevel.Focus();
                            }
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    cmbOpeningStock.Focus();
                }
                //if (e.KeyCode == Keys.Enter)
                //{
                //    txtPartNo.Focus();
                //    txtPartNo.SelectionStart = 0;
                //    txtPartNo.SelectionLength = 0;
                //}
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:161" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbBom KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbBom.Text == string.Empty || cmbBom.SelectionStart == 0)
                    {
                        if (cmbTaxApplicableOn.Enabled)
                        {
                            cmbTaxApplicableOn.SelectionStart = 0;
                            cmbTaxApplicableOn.SelectionLength = 0;
                            cmbTaxApplicableOn.Focus();
                        }
                        else
                        {
                            cmbTax.SelectionStart = 0;
                            cmbTax.SelectionLength = 0;
                            cmbTax.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    if (cmbBom.SelectedIndex == 0)
                    {
                        SendKeys.Send("{TAB}");
                    }
                    else
                    {
                        if (txtMinimumStock.Enabled)
                        {
                            txtMinimumStock.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
             formMDI.infoError.ErrorString = "PC:162" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cmbOpeningStock KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOpeningStock_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (cmbOpeningStock.Text == string.Empty || cmbOpeningStock.SelectionStart == 0)
                    {
                        txtPartNo.Enabled = false;
                        if (txtPartNo.Enabled)
                        {
                            txtPartNo.SelectionStart = 0;
                            txtPartNo.SelectionLength = 0;
                            txtPartNo.Focus();
                        }
                        else
                        {
                            cmbMultipleUnit.Focus();
                        }
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    // SendKeys.Send("{TAB}");
                    if (cmbOpeningStock.SelectedIndex == 1)
                    {
                        SendKeys.Send("{TAB}");
                    }
                    else
                    {
                        txtNarration.Focus();
                        txtNarration.SelectionLength = 0;
                        txtNarration.SelectionStart = 0;
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:163" + ex.Message;
            }
        }
        /// <summary>
        /// Work on frmProductCreation KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmProductCreation_KeyDown(object sender, KeyEventArgs e)
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
                if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control) //Save Ctrl + S
                {
                    if (cmbGroup.Focused || cmbModalNo.Focused || cmbSize.Focused || cmbTax.Focused || cmbBrand.Focused)
                    {
                        cmbGroup.DropDownStyle = ComboBoxStyle.DropDown;
                        cmbModalNo.DropDownStyle = ComboBoxStyle.DropDown;
                        cmbSize.DropDownStyle = ComboBoxStyle.DropDown;
                        cmbTax.DropDownStyle = ComboBoxStyle.DropDown;
                        cmbBrand.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                    else
                    {
                        cmbGroup.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbModalNo.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbSize.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbTax.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    cmbGroup.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbModalNo.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbSize.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbTax.DropDownStyle = ComboBoxStyle.DropDownList;
                    if (dgvProductCreation.Columns["dgvcmbtgodown"].Selected || dgvProductCreation.Columns["dgvcmbrack"].Selected)
                    {
                        btnSave.Select();
                    }
                    btnSave_Click(sender, e);
                }
                if (e.KeyCode == Keys.D && Control.ModifierKeys == Keys.Control)//Delete Ctrl + D
                {
                    if (btnDelete.Enabled == true)
                    {
                        btnDelete_Click(sender, e);
                    }
                }
                if (dgvProductCreation.CurrentRow != null)
                {
                    if (dgvProductCreation.CurrentCell == dgvProductCreation.CurrentRow.Cells["dgvcmbrack"] || dgvProductCreation.CurrentCell == dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"])
                    {
                        if (Control.ModifierKeys == Keys.Alt && e.KeyCode == Keys.C)
                        {
                            if (dgvProductCreation.CurrentCell == dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"])
                            {
                                SendKeys.Send("{F10}");
                                CreateGodownFromGrid();
                            }
                            if (dgvProductCreation.CurrentCell == dgvProductCreation.CurrentRow.Cells["dgvcmbrack"]
                                && dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value != null)
                            {
                                SendKeys.Send("{F10}");
                                CreateRackFromGrid(Convert.ToDecimal(dgvProductCreation.CurrentRow.Cells["dgvcmbtgodown"].Value));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:164" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtPurchaseRate KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPurchaseRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:165" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtSalesRate KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSalesRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:166" + ex.Message;
            }
        }
        /// <summary>
        ///  Work on txtMrp KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMrp_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
             formMDI.infoError.ErrorString = "PC:167" + ex.Message;
            }
        }
        /// <summary>
        ///  Work on txtMinimumStock KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinimumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:168" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtMaximumStock KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaximumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:169" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtReorderLevel KeyPress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReorderLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Common.DecimalValidation(sender, e, false);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:170" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cbxActive KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxActive_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cbxReminder.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = txtNarration.Text.Length;
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:171" + ex.Message;
            }
        }
        /// <summary>
        /// Work on cbxReminder KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxReminder_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    cbxActive.Focus();
                }
                if (e.KeyCode == Keys.Enter)
                {
                    txtNarration.Focus();
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:172" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtNarration KeyDown event
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
                    if (inNarrationCount == 2)
                    {
                        inNarrationCount = 0;
                        btnSave.Focus();
                    }
                }
                else
                {
                    inNarrationCount = 0;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (txtNarration.Text == string.Empty || txtNarration.SelectionStart == 0)
                    {
                        if (dgvProductCreation.Enabled)
                        {
                            dgvProductCreation.Focus();
                            dgvProductCreation.CurrentCell = dgvProductCreation.Rows[0].Cells["dgvtxtslno"];
                        }
                        else
                        {
                            cmbOpeningStock.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:173" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtReorderLevel MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReorderLevel_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (txtReorderLevel.Text == "0")
                {
                    txtReorderLevel.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
              formMDI.infoError.ErrorString = "PC:174" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtReorderLevel CellLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReorderLevel_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtReorderLevel.Text == string.Empty)
                {
                    txtReorderLevel.Text = "0";
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:175" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtPartNo KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPartNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (txtPartNo.Text == string.Empty || txtPartNo.SelectionStart == 0)
                    {
                        cmbMultipleUnit.SelectionStart = 0;
                        cmbMultipleUnit.SelectionLength = 0;
                        cmbMultipleUnit.Focus();
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                    cmbOpeningStock.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "PC:176" + ex.Message;
            }
        }
        /// <summary>
        /// Work on btnSave KeyUp event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    txtNarration.Focus();
                    txtNarration.SelectionStart = 0;
                    txtNarration.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:177" + ex.Message;
            }
        }
        /// <summary>
        /// Function for CheckInvalidRowsInGrid
        /// </summary>
        /// <param name="e"></param>
        public void CheckingForIncompleteRowInGrid(DataGridViewCellEventArgs e)
        {
            try
            {
                if ((dgvProductCreation.Rows[e.RowIndex]) != null)
                {
                    if (!isValueChanged)
                    {
                        if ((dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxManfDate"].Value.ToString().Trim() == "") && cmbAllowBatch.SelectedIndex == 1)
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if ((dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtExpDate"].Value.ToString().Trim() == "") && cmbAllowBatch.SelectedIndex == 1)
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtqty"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtqty"].Value.ToString().Trim() == "")
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtrate"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtrate"].Value.ToString().Trim() == "")
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if ((dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatch"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtbatch"].Value.ToString().Trim() == "") && cmbAllowBatch.SelectedIndex == 1)
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductCreation.Columns["dgvcmbtgodown"].Visible && (dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbtgodown"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbtgodown"].Value.ToString().Trim() == "" || Convert.ToDecimal(dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbtgodown"].Value) == 0))
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else if (dgvProductCreation.Columns["dgvcmbrack"].Visible && (dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbrack"].Value == null || dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbrack"].Value.ToString().Trim() == "" || Convert.ToDecimal(dgvProductCreation.Rows[e.RowIndex].Cells["dgvcmbrack"].Value) == 0))
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "X";
                            dgvProductCreation["dgvtxtCheck", e.RowIndex].Style.ForeColor = Color.Red;
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "X";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            isValueChanged = true;
                            dgvProductCreation.Rows[e.RowIndex].Cells["dgvtxtCheck"].Value = "";
                            dgvProductCreation.Rows[e.RowIndex].HeaderCell.Value = "";
                        }
                    }
                    isValueChanged = false;
                }
            }
            catch (Exception ex)
            {
                 formMDI.infoError.ErrorString = "PC:178" + ex.Message;
            }
        }
        /// <summary>
        /// Work on dgvProductCreation KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductCreation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dgvProductCreation.CurrentCell == dgvProductCreation.Rows[dgvProductCreation.Rows.Count - 1].Cells["dgvtxtamount"])
                    {
                        txtNarration.Focus();
                    }
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (dgvProductCreation.CurrentCell == dgvProductCreation.Rows[0].Cells["dgvtxtSlNo"])
                    {
                        cmbOpeningStock.Select();
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:179" + ex.Message;
            }
        }
        /// <summary>
        /// Work on txtName KeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtProductCode.Enabled)
                    {
                        txtProductCode.Focus();
                    }
                    else
                    {
                        cmbGroup.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
               formMDI.infoError.ErrorString = "PC:180" + ex.Message;
            }
        }
        #endregion
    }
}