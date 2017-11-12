using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace AccountSkate
{
    class OpenAccount : MasterValidations
    {
        /// <summary>
        /// Function for Connection State
        /// </summary>
        private void functionConnectionState()
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
        }
        /// <summary>
        /// Function to Add Account ledger
        /// </summary>
        /// <param name="infoAccountLedger"></param>
        /// <returns></returns>
        public void AddAccountLedger(List<AccountLedgerInfoforOpenAccount> lstinfoAccountLedger, ref string Mess, System.Windows.Controls.Label Message)
        {
            int Skated = 0;
            decimal decMainIdLed = 0;
            foreach (AccountLedgerInfoforOpenAccount infoAccountLedger in lstinfoAccountLedger)
            {
                try
                {
                    if (SavingValidation_AccountingLedger(infoAccountLedger))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("AccountLedgerAddWithIdentity", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@accountGroupId", SqlDbType.Decimal).Value = infoAccountLedger.AccountGroupId;
                        sqlcmd.Parameters.Add("@ledgerName", SqlDbType.VarChar).Value = infoAccountLedger.Name;
                        sqlcmd.Parameters.Add("@openingBalance", SqlDbType.Decimal).Value = infoAccountLedger.Opening_Balance;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoAccountLedger.Narration;
                        sqlcmd.Parameters.Add("@crOrDr", SqlDbType.VarChar).Value = infoAccountLedger.CrorDr;
                        sqlcmd.Parameters.Add("@mailingName", SqlDbType.VarChar).Value = infoAccountLedger.Mailing_Name;
                        sqlcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = infoAccountLedger.Address;
                        sqlcmd.Parameters.Add("@isDefault", SqlDbType.Bit).Value = false;
                        sqlcmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = infoAccountLedger.Phone;
                        sqlcmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = infoAccountLedger.Mobile;
                        sqlcmd.Parameters.Add("@email", SqlDbType.VarChar).Value = infoAccountLedger.Email;
                        sqlcmd.Parameters.Add("@creditPeriod", SqlDbType.Int).Value = infoAccountLedger.Credit_Period;
                        sqlcmd.Parameters.Add("@creditLimit", SqlDbType.Decimal).Value = infoAccountLedger.Credit_Limit;
                        sqlcmd.Parameters.Add("@pricinglevelId", SqlDbType.Decimal).Value = infoAccountLedger.Pricing_Level_ID;
                        sqlcmd.Parameters.Add("@billByBill", SqlDbType.Bit).Value = infoAccountLedger.Bill_By_Bill;
                        sqlcmd.Parameters.Add("@tin", SqlDbType.VarChar).Value = infoAccountLedger.TIN;
                        sqlcmd.Parameters.Add("@cst", SqlDbType.VarChar).Value = infoAccountLedger.CST;
                        sqlcmd.Parameters.Add("@pan", SqlDbType.VarChar).Value = infoAccountLedger.PAN;
                        sqlcmd.Parameters.Add("@routeId", SqlDbType.Decimal).Value = infoAccountLedger.Route_ID;
                        sqlcmd.Parameters.Add("@bankAccountNumber", SqlDbType.VarChar).Value = infoAccountLedger.Account_Number;
                        sqlcmd.Parameters.Add("@branchName", SqlDbType.VarChar).Value = infoAccountLedger.Branch_Name;
                        sqlcmd.Parameters.Add("@branchCode", SqlDbType.VarChar).Value = infoAccountLedger.Brach_Code;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@areaId", SqlDbType.Int).Value = infoAccountLedger.Area_ID;
                        sqlcmd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Today;

                        object obj = sqlcmd.ExecuteScalar();
                        if (obj != null && int.Parse(obj.ToString()) != 1)
                        {
                            decMainIdLed = Convert.ToDecimal(obj);
                        }
                        if (infoAccountLedger.Opening_Balance != 0)
                        {
                            SqlCommand SqlCmd = new SqlCommand("LedgerPostingAdd", sqlcon);
                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = functionGetFinancialYear();
                            SqlCmd.Parameters.Add("@voucherTypeId", SqlDbType.Decimal).Value = 1;
                            SqlCmd.Parameters.Add("@voucherNo", SqlDbType.VarChar).Value = decMainIdLed.ToString();
                            SqlCmd.Parameters.Add("@ledgerId", SqlDbType.Decimal).Value = decMainIdLed;
                            if (infoAccountLedger.CrorDr == "Dr")
                            {
                                SqlCmd.Parameters.Add("@debit", SqlDbType.Decimal).Value = infoAccountLedger.Opening_Balance;
                                SqlCmd.Parameters.Add("@credit", SqlDbType.Decimal).Value = 0;
                            }
                            else
                            {
                                SqlCmd.Parameters.Add("@debit", SqlDbType.Decimal).Value = 0;
                                SqlCmd.Parameters.Add("@credit", SqlDbType.Decimal).Value = infoAccountLedger.Opening_Balance;
                            }
                            SqlCmd.Parameters.Add("@detailsId", SqlDbType.Decimal).Value = 0;
                            SqlCmd.Parameters.Add("@yearId", SqlDbType.Decimal).Value = 1;
                            SqlCmd.Parameters.Add("@invoiceNo", SqlDbType.VarChar).Value = decMainIdLed;
                            SqlCmd.Parameters.Add("@chequeNo", SqlDbType.VarChar).Value = string.Empty;
                            SqlCmd.Parameters.Add("@chequeDate", SqlDbType.DateTime).Value = DateTime.Now;
                            SqlCmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                            SqlCmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                            SqlCmd.ExecuteScalar();
                        }
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }

            }
            Mess = Skated + " Datas succesfully skated.";
        }
        /// <summary>
        /// Function to Login
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public bool functionLogin(string strUserName, string strPassword)
        {
            object obj = null;
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("IF(SELECT COUNT (userId) FROM tbl_User WHERE userName = @strUserName AND password = @strPassword AND roleId = 1) > 0 SELECT 1 ELSE SELECT 0", sqlcon);
                sqlCmd.Parameters.AddWithValue("@strUserName", strUserName);
                sqlCmd.Parameters.AddWithValue("@strPassword", strPassword);
                obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) != 0)
                {
                    isTrue = true;
                }
            }
            catch (Exception)
            { }
            finally
            {
                sqlcon.Close();
            }
            return isTrue;
        }
        public void AddAccountGroup(List<AccountGroupInfoforOpenAccount> lstinfoAccountGroup, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (AccountGroupInfoforOpenAccount infoAccountGroup in lstinfoAccountGroup)
            {
                try
                {
                    if (SavingValidation_AccountingGroup(infoAccountGroup))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("AccountGroupAddWithIdentity", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@accountGroupName", SqlDbType.VarChar).Value = infoAccountGroup.Name;
                        sqlcmd.Parameters.Add("@groupUnder", SqlDbType.Decimal).Value = infoAccountGroup.Under;
                        sqlcmd.Parameters.Add("@nature", SqlDbType.VarChar).Value = infoAccountGroup.Nature;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoAccountGroup.Narration;
                        sqlcmd.Parameters.Add("@affectGrossProfit", SqlDbType.VarChar).Value = infoAccountGroup.Affect_Gross_Profit;
                        sqlcmd.Parameters.Add("@isDefault", SqlDbType.Bit).Value = false;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        /// <summary>
        /// Function to Add Product
        /// </summary>
        /// <param name="infoProduct"></param>
        /// <returns></returns>
        public void AddProduct(List<ProductInfofroOpenAccount> lstProduct, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            try
            {
                foreach (ProductInfofroOpenAccount infoProduct in lstProduct)
                {
                    if (SavingValidation_Product(infoProduct))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("productAdd", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@productCode", SqlDbType.VarChar).Value = (infoProduct.ProductCode == "Tally Demo Code") ? "MS00" + functionGenerateProductCode() : infoProduct.ProductCode;
                        sqlcmd.Parameters.Add("@productName", SqlDbType.VarChar).Value = infoProduct.ProductName;
                        sqlcmd.Parameters.Add("@groupId", SqlDbType.Decimal).Value = infoProduct.Group;
                        sqlcmd.Parameters.Add("@brandId", SqlDbType.Decimal).Value = 1;
                        sqlcmd.Parameters.Add("@unitId", SqlDbType.Decimal).Value = infoProduct.UnitId;
                        sqlcmd.Parameters.Add("@sizeId", SqlDbType.Decimal).Value = 1;
                        sqlcmd.Parameters.Add("@modelNoId", SqlDbType.Decimal).Value = 1;
                        sqlcmd.Parameters.Add("@taxId", SqlDbType.Decimal).Value = infoProduct.TaxId;
                        sqlcmd.Parameters.Add("@taxapplicableOn", SqlDbType.VarChar).Value = "MRP";
                        sqlcmd.Parameters.Add("@purchaseRate", SqlDbType.Decimal).Value = infoProduct.PurchaseRate;
                        sqlcmd.Parameters.Add("@salesRate", SqlDbType.Decimal).Value = infoProduct.SalesRate;
                        sqlcmd.Parameters.Add("@mrp", SqlDbType.Decimal).Value = infoProduct.MRP;
                        sqlcmd.Parameters.Add("@minimumStock", SqlDbType.Decimal).Value = infoProduct.MinimumStock;
                        sqlcmd.Parameters.Add("@maximumStock", SqlDbType.Decimal).Value = infoProduct.MaximumStock;
                        sqlcmd.Parameters.Add("@reorderLevel", SqlDbType.Decimal).Value = infoProduct.ReorderLevel;
                        sqlcmd.Parameters.Add("@godownId", SqlDbType.Decimal).Value = infoProduct.GodownId;
                        sqlcmd.Parameters.Add("@rackId", SqlDbType.Decimal).Value = infoProduct.RackId;
                        sqlcmd.Parameters.Add("@isallowBatch", SqlDbType.Bit).Value = infoProduct.AllowBatch;
                        sqlcmd.Parameters.Add("@ismultipleunit", SqlDbType.Bit).Value = infoProduct.MultipleUnit;
                        sqlcmd.Parameters.Add("@isBom", SqlDbType.Bit).Value = false;
                        sqlcmd.Parameters.Add("@isopeningstock", SqlDbType.Bit).Value = infoProduct.OpeningStock;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoProduct.Narration;
                        sqlcmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = true;
                        sqlcmd.Parameters.Add("@isshowRemember", SqlDbType.Bit).Value = false;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Today;
                        object obj = sqlcmd.ExecuteScalar();
                        decimal decProductId = Convert.ToDecimal(obj);
                        decimal decBatchId = 1;
                        int inBarcode = 1;
                        if (obj != null && int.Parse(obj.ToString()) != 0)
                        {
                            SqlCommand SqlCmd = new SqlCommand("AutomaticBarcodeGeneration", sqlcon);
                            SqlCmd.CommandType = CommandType.StoredProcedure;
                            inBarcode = Convert.ToInt32(SqlCmd.ExecuteScalar());
                            SqlCommand sqlCmdd = new SqlCommand("BatchAddWithBarCode", sqlcon);
                            sqlCmdd.CommandType = CommandType.StoredProcedure;
                            sqlCmdd.Parameters.Add("@batchNo", SqlDbType.VarChar).Value = "NA";
                            sqlCmdd.Parameters.Add("@productId", SqlDbType.Decimal).Value = decProductId;
                            sqlCmdd.Parameters.Add("@manufacturingDate", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlCmdd.Parameters.Add("@expiryDate", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlCmdd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                            sqlCmdd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                            sqlCmdd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlCmdd.Parameters.Add("@narration", SqlDbType.VarChar).Value = string.Empty;
                            sqlCmdd.Parameters.Add("@barcode", SqlDbType.VarChar).Value = inBarcode;
                            sqlCmdd.Parameters.Add("@partNo", SqlDbType.VarChar).Value = string.Empty;
                            object obj1 = sqlCmdd.ExecuteScalar();
                            decBatchId = Convert.ToDecimal(obj1);
                        }

                        if (infoProduct.MultipleUnit)
                        {
                            SqlCommand sqlCmdd = new SqlCommand("UnitConvertionAdd", sqlcon);
                            sqlCmdd.CommandType = CommandType.StoredProcedure;
                            sqlCmdd.Parameters.Add("@productId", SqlDbType.Decimal).Value = decProductId;
                            sqlCmdd.Parameters.Add("@unitId", SqlDbType.Decimal).Value = infoProduct.UnitId;
                            sqlCmdd.Parameters.Add("@conversionRate", SqlDbType.Decimal).Value = infoProduct.ConversionRate;
                            sqlCmdd.Parameters.Add("@quantities", SqlDbType.VarChar).Value = 0;
                            sqlCmdd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                            sqlCmdd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                            sqlCmdd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlCmdd.ExecuteScalar();
                        }
                        SqlCommand sqlCmd = new SqlCommand("UnitConvertionAdd", sqlcon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@conversionRate", SqlDbType.Decimal).Value = 1;
                        sqlCmd.Parameters.Add("@quantities", SqlDbType.Decimal).Value = DBNull.Value;
                        sqlCmd.Parameters.Add("@unitId", SqlDbType.Decimal).Value = infoProduct.UnitId;
                        sqlCmd.Parameters.Add("@productId", SqlDbType.Decimal).Value = decProductId;
                        sqlCmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Now;
                        sqlCmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            Mess = Skated + " Datas succesfully skated.";
        }
        internal void AddPriceLevel(List<PriceLevelInfofroOpenAccount> lstObjPriceLevel, ref string Mess, System.Windows.Controls.Label Message)
        {
            int Skated = 0;
            foreach (PriceLevelInfofroOpenAccount infoPricingLevel in lstObjPriceLevel)
            {
                try
                {
                    if (SavingValidation_PricingLevel(infoPricingLevel))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("PricingLevelAddWithoutSamePricingLevel", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@pricinglevelName", SqlDbType.VarChar).Value = infoPricingLevel.PricingLevelName;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoPricingLevel.Narration;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
             new Action(
             delegate()
             {
                 Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
             }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
                Mess = Skated + " Datas succesfully skated.";
            }
        }

        internal void AddProductGroup(List<ProductGroupinfoforOpenAccount> lstObjProductGroup, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (var ProductGroupinfoforOpenAccount in lstObjProductGroup)
            {
                try
                {
                    if (SavingValidation_ProductGroup(ProductGroupinfoforOpenAccount))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("ProductGroupAdd", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@groupName", SqlDbType.VarChar).Value = ProductGroupinfoforOpenAccount.GroupName;
                        sqlcmd.Parameters.Add("@groupUnder", SqlDbType.Decimal).Value = ProductGroupinfoforOpenAccount.GroupUnder;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = ProductGroupinfoforOpenAccount.Narration;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        internal void AddUnit(List<UnitinfoforOpenAccount> lstObjUnit, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (UnitinfoforOpenAccount infoUnit in lstObjUnit)
            {
                try
                {
                    if (SavingValidation_Unit(infoUnit))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("UnitAdd", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@unitName", SqlDbType.VarChar).Value = infoUnit.UnitName;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoUnit.Narration;
                        sqlcmd.Parameters.Add("@noOfDecimalplaces", SqlDbType.Decimal).Value = infoUnit.noOfDecimalPlaces;
                        sqlcmd.Parameters.Add("@formalName", SqlDbType.VarChar).Value = infoUnit.FormalName;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        internal void AddGodwn(List<GodowninfoforOpenAccount> lstObjGodowns, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (GodowninfoforOpenAccount item in lstObjGodowns)
            {
                try
                {
                    if (SavingValidation_Godown(item))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("GodownAddwithoutSameName", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@godownName", SqlDbType.VarChar).Value = item.Godown_Name;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = item.Narration;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        object obj = sqlcmd.ExecuteScalar();
                        SqlCommand sqlCmd = new SqlCommand("RackAdd", sqlcon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@rackName", SqlDbType.VarChar).Value = "NA";
                        sqlCmd.Parameters.Add("@godownId", SqlDbType.Decimal).Value = Convert.ToDecimal(obj);
                        sqlCmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Now;
                        sqlCmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        internal void AddStock(List<StockforOpenAccount> lstObjStock, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (StockforOpenAccount infoProduct in lstObjStock)
            {
                try
                {
                    if (SavingValidation_Stock(infoProduct))
                    {

                        functionConnectionState();
                        SqlCommand sqlCmmd = new SqlCommand("StockPostingAdd", sqlcon);
                        sqlCmmd.CommandType = CommandType.StoredProcedure;
                        sqlCmmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
                        sqlCmmd.Parameters.Add("@voucherTypeId", SqlDbType.Decimal).Value = 2;
                        sqlCmmd.Parameters.Add("@voucherNo", SqlDbType.VarChar).Value = infoProduct.ProductId;
                        sqlCmmd.Parameters.Add("@invoiceNo", SqlDbType.VarChar).Value = infoProduct.ProductId;
                        sqlCmmd.Parameters.Add("@productId", SqlDbType.Decimal).Value = infoProduct.ProductId;
                        sqlCmmd.Parameters.Add("@batchId", SqlDbType.Decimal).Value = infoProduct.BatchId;
                        sqlCmmd.Parameters.Add("@unitId", SqlDbType.Decimal).Value = infoProduct.UnitID;
                        sqlCmmd.Parameters.Add("@godownId", SqlDbType.Decimal).Value = 1;
                        sqlCmmd.Parameters.Add("@rackId", SqlDbType.Decimal).Value = 1;
                        sqlCmmd.Parameters.Add("@againstVoucherTypeId", SqlDbType.Decimal).Value = 0;
                        sqlCmmd.Parameters.Add("@againstInvoiceNo", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmmd.Parameters.Add("@againstVoucherNo", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmmd.Parameters.Add("@inwardQty", SqlDbType.Decimal).Value = infoProduct.OpeningStockNumber;
                        sqlCmmd.Parameters.Add("@outwardQty", SqlDbType.Decimal).Value = 0;
                        sqlCmmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = infoProduct.ClosingRate;
                        sqlCmmd.Parameters.Add("@financialYearId", SqlDbType.Decimal).Value = 1;
                        sqlCmmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlCmmd.ExecuteScalar();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        internal void AddCurrency(List<CurrencyInfoforOpenAccount> lstObjCurrency, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (CurrencyInfoforOpenAccount item in lstObjCurrency)
            {
                try
                {
                    if (SavingValidation_Currency(item))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("currencyAddwithIdentity", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@currencySymbol", SqlDbType.VarChar).Value = item.CurrencySymbol;
                        sqlcmd.Parameters.Add("@currencyName", SqlDbType.VarChar).Value = item.CurrencyName;
                        sqlcmd.Parameters.Add("@subunitName", SqlDbType.VarChar).Value = item.SubUnitName;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = item.Narration;
                        sqlcmd.Parameters.Add("@noOfdecimalPlaces", SqlDbType.Int).Value = item.NoOfDecimalPlaces;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@isDefault", SqlDbType.Bit).Value = false;
                        sqlcmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
                }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        internal void AddVoucherType(List<VoucherTypeinfoforOpenAccount> lstObjVoucherType, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            foreach (VoucherTypeinfoforOpenAccount infoVoucherType in lstObjVoucherType)
            {
                try
                {
                    if (SavingValidation_VoucherType(infoVoucherType))
                    {
                        functionConnectionState();
                        SqlCommand sqlcmd = new SqlCommand("VoucherTypeAddWithIdentity", sqlcon);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.Add("@voucherTypeName", SqlDbType.VarChar).Value = infoVoucherType.name;
                        sqlcmd.Parameters.Add("@typeOfVoucher", SqlDbType.VarChar).Value = infoVoucherType.typeOfVoucher;
                        sqlcmd.Parameters.Add("@methodOfVoucherNumbering", SqlDbType.VarChar).Value = infoVoucherType.methodOfVoucherNumbering;
                        sqlcmd.Parameters.Add("@isTaxApplicable", SqlDbType.Bit).Value = false;
                        sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoVoucherType.narration;
                        sqlcmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = infoVoucherType.active;
                        sqlcmd.Parameters.Add("@masterId", SqlDbType.Int).Value = functionCheckVoucherFormId(infoVoucherType.typeOfVoucher);
                        sqlcmd.Parameters.Add("@declaration", SqlDbType.VarChar).Value = infoVoucherType.declaration;
                        sqlcmd.Parameters.Add("@heading1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@heading2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@heading3", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@heading4", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                        sqlcmd.Parameters.Add("@isDefault", SqlDbType.Bit).Value = false;
                        sqlcmd.ExecuteScalar();
                        sqlcon.Close();
                        Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
                    }
                }
                catch (Exception ex)
                { System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog())); }
            }
            Mess = Skated + " Datas succesfully skated.";
        }

        public void AddSalesman(EmployeeInfo infoEmployee, ref string Mess, System.Windows.Controls.Label Message)
        {

            int Skated = 0;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("EmployeeForTakingEmployeeId", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@designationId", SqlDbType.Decimal).Value = 1;
                if (infoEmployee.employeeCode == string.Empty)
                {
                    sqlcmd.Parameters.Add("@employeeCode", SqlDbType.VarChar).Value = "MS00" + functionGenerateSalesmanCode();
                }
                else
                {
                    sqlcmd.Parameters.Add("@employeeCode", SqlDbType.VarChar).Value = infoEmployee.employeeCode;
                }
                sqlcmd.Parameters.Add("@employeeName", SqlDbType.VarChar).Value = infoEmployee.name;
                sqlcmd.Parameters.Add("@email", SqlDbType.VarChar).Value = infoEmployee.eMail;
                sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = infoEmployee.narration;
                sqlcmd.Parameters.Add("@phoneNumber", SqlDbType.VarChar).Value = infoEmployee.phone;
                sqlcmd.Parameters.Add("@mobileNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = infoEmployee.address;
                sqlcmd.Parameters.Add("@active", SqlDbType.Bit).Value = infoEmployee.isActive;
                sqlcmd.Parameters.Add("@dob", SqlDbType.DateTime).Value = DateTime.Now;
                sqlcmd.Parameters.Add("@maritalStatus", SqlDbType.VarChar).Value = "Single";
                sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = "Male";
                sqlcmd.Parameters.Add("@qualification", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@joiningDate", SqlDbType.DateTime).Value = DateTime.Now;
                sqlcmd.Parameters.Add("@terminationDate", SqlDbType.DateTime).Value = DateTime.Now;
                sqlcmd.Parameters.Add("@bloodGroup", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@passportNo", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@passportExpiryDate", SqlDbType.DateTime).Value = DateTime.Now;
                sqlcmd.Parameters.Add("@labourCardNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@labourCardExpiryDate", SqlDbType.DateTime).Value = DateTime.Now;
                sqlcmd.Parameters.Add("@visaNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@visaExpiryDate", SqlDbType.DateTime).Value = DateTime.Now;
                sqlcmd.Parameters.Add("@salaryType", SqlDbType.VarChar).Value = "Monthly";
                sqlcmd.Parameters.Add("@dailyWage", SqlDbType.Decimal).Value = 0;
                sqlcmd.Parameters.Add("@bankName", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@branchName", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@bankAccountNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@branchCode", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@panNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@pfNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@esiNumber", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@defaultPackageId", SqlDbType.Decimal).Value = 1;
                sqlcmd.ExecuteScalar();
                sqlcon.Close();
                Message.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(
            delegate()
            {
                Message.Content = (Skated++).ToString() + " Datas succesfully skated.";
            }));
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            Mess = Skated + " Datas succesfully skated.";
        }
    }
}
