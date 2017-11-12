using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Windows;
using System.Collections;

namespace AccountSkate
{
    class Tally : DBSourceConnectionTally
    {
        /// <summary>
        /// Function to Check Tally Connection
        /// </summary>
        /// <returns></returns>
        public bool CheckTallyConnection()
        {
            bool isTrue = false;
            try
            {
                if (odbcCon != null)
                {
                    if (odbcCon.State == ConnectionState.Closed)
                    {
                        odbcCon.Open();
                        isTrue = true;
                        odbcCon.Close();
                    }
                    else
                    {
                        isTrue = true;
                    }
                }
                else
                {
                    isTrue = false;
                }


            }
            catch
            { }
            return isTrue;
        }
        public List<pricingLevelInfo> GetPriceLevel()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$PriceLevel` FROM PriceLevels", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                pricingLevelInfo infopricelevel;
                while (odbcReader.Read())
                {
                    infopricelevel = new pricingLevelInfo();
                    infopricelevel.PricingLevelName = (odbcReader["`$PriceLevel`"] != DBNull.Value) ? odbcReader["`$PriceLevel`"].ToString() : string.Empty;
                    lstPriceLevel.Add(infopricelevel);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstPriceLevel;
        }
        /// <summary>
        /// Function to get Customer.
        /// </summary>
        /// <returns></returns>
        public List<CustomerSupplierInfo> GetCustomer()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$LedgerMobile`, `$OnAccountValue`,`$MailingName`, `$ClosingBalance`,`$InterStateSTNumber`, `$_Address1`+' '+`$_Address2`+' '+`$_Address3`+' '+`$StateName` +' '+`$PINCode`, `$IsBillWiseOn`, `$CreditLimit`, `$SalesTaxNumber`, `$Name`, `$BankDetails`, `$LedgerPhone`,`$LedgerContact`, `$EMail`, `$PriceLevel`, `$BillCreditPeriod`, `$VATTINNumber`, `$IncomeTaxNumber`, `$Narration` FROM Ledger WHERE `$Parent`= Sundry Debtors", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                CustomerSupplierInfo ledgerinfo;
                while (odbcReader.Read())
                {
                    ledgerinfo = new CustomerSupplierInfo();
                    ledgerinfo.Name = (odbcReader["`$Name`"] != DBNull.Value) ? odbcReader["`$Name`"].ToString() : string.Empty;
                    ledgerinfo.Opening_Balance = (odbcReader["`$ClosingBalance`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString()) < 0) ? (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString()) * -1).ToString() : (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString()).ToString())) : "0";
                    ledgerinfo.Address = (odbcReader["`$_Address1`+' '+`$_Address2`+' '+`$_Address3`+' '+`$StateName` +' '+`$PINCode`"] != DBNull.Value) ? odbcReader["`$_Address1`+' '+`$_Address2`+' '+`$_Address3`+' '+`$StateName` +' '+`$PINCode`"].ToString() : string.Empty;
                    ledgerinfo.Bill_By_Bill = (odbcReader["`$IsBillWiseOn`"] != DBNull.Value) ? Convert.ToBoolean(odbcReader["`$IsBillWiseOn`"]).ToString() : "false";
                    ledgerinfo.Credit_Limit = (odbcReader["`$CreditLimit`"] != DBNull.Value) ? Convert.ToDecimal(odbcReader["`$CreditLimit`"].ToString()).ToString() : "0";
                    ledgerinfo.CST = (odbcReader["`$InterStateSTNumber`"] != DBNull.Value) ? odbcReader["`$InterStateSTNumber`"].ToString() : string.Empty;
                    ledgerinfo.Mailing_Name = (odbcReader["`$MailingName`"] != DBNull.Value) ? odbcReader["`$MailingName`"].ToString() : ledgerinfo.Name;
                    ledgerinfo.Account_Number = (odbcReader["`$BankDetails`"] != DBNull.Value) ? odbcReader["`$BankDetails`"].ToString() : string.Empty;
                    ledgerinfo.Phone = (odbcReader["`$LedgerPhone`"] != DBNull.Value) ? odbcReader["`$LedgerPhone`"].ToString() : string.Empty;
                    ledgerinfo.Email = (odbcReader["`$EMail`"] != DBNull.Value) ? odbcReader["`$EMail`"].ToString() : string.Empty;
                    ledgerinfo.PAN = (odbcReader["`$IncomeTaxNumber`"] != DBNull.Value) ? odbcReader["`$IncomeTaxNumber`"].ToString() : string.Empty;
                    ledgerinfo.Pricing_Level = (odbcReader["`$PriceLevel`"] != DBNull.Value) ? odbcReader["`$PriceLevel`"].ToString() : "NA";
                    ledgerinfo.Credit_Period = (odbcReader["`$BillCreditPeriod`"] != DBNull.Value) ? (int.Parse(new string(odbcReader["`$BillCreditPeriod`"].ToString().Where(char.IsDigit).ToArray()))).ToString() : "0";
                    ledgerinfo.TIN = (odbcReader["`$VATTINNumber`"] != DBNull.Value) ? odbcReader["`$VATTINNumber`"].ToString() : string.Empty;
                    ledgerinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    ledgerinfo.Mobile = (odbcReader["`$LedgerMobile`"] != DBNull.Value) ? odbcReader["`$LedgerMobile`"].ToString() : string.Empty;
                    decimal decClosing = (odbcReader["`$ClosingBalance`"] != DBNull.Value) ? (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString())) : 0;
                    ledgerinfo.CrorDr = (decClosing <= 0) ? "Dr" : "Cr";
                    lstCustomerSupplierInfo.Add(ledgerinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstCustomerSupplierInfo;
        }
        /// <summary>
        /// Function to get Supplier.
        /// </summary>
        /// <returns></returns>
        public List<CustomerSupplierInfo> GetSupplier()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`, `$OnAccountValue`, `$ClosingBalance`, `$_Address1`+' '+`$_Address2`+' '+`$_Address3`+' '+`$StateName` +' '+`$PINCode`, `$IsBillWiseOn`, `$CreditLimit`, `$SalesTaxNumber`, `$Name`, `$BankDetails`, `$LedgerPhone`, `$EMail`, `$PriceLevel`, `$BillCreditPeriod`, `$VATTINNumber`, `$IncomeTaxNumber`, `$Narration` FROM Ledger WHERE `$Parent`= Sundry Creditors", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                CustomerSupplierInfo ledgerinfo;
                while (odbcReader.Read())
                {
                    ledgerinfo = new CustomerSupplierInfo();
                    ledgerinfo.Name = (odbcReader["`$Name`"] != DBNull.Value) ? odbcReader["`$Name`"].ToString() : string.Empty;
                    ledgerinfo.Opening_Balance = (odbcReader["`$ClosingBalance`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString()) < 0) ? (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString()) * -1).ToString() : (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString()).ToString())) : "0";
                    ledgerinfo.Address = (odbcReader["`$_Address1`+' '+`$_Address2`+' '+`$_Address3`+' '+`$StateName` +' '+`$PINCode`"] != DBNull.Value) ? odbcReader["`$_Address1`+' '+`$_Address2`+' '+`$_Address3`+' '+`$StateName` +' '+`$PINCode`"].ToString() : string.Empty;
                    ledgerinfo.Bill_By_Bill = (odbcReader["`$IsBillWiseOn`"] != DBNull.Value) ? Convert.ToBoolean(odbcReader["`$IsBillWiseOn`"]).ToString() : "false";
                    ledgerinfo.Credit_Limit = (odbcReader["`$CreditLimit`"] != DBNull.Value) ? Convert.ToDecimal(odbcReader["`$CreditLimit`"].ToString()).ToString() : "0";
                    ledgerinfo.CST = (odbcReader["`$SalesTaxNumber`"] != DBNull.Value) ? odbcReader["`$SalesTaxNumber`"].ToString() : string.Empty;
                    ledgerinfo.Mailing_Name = (odbcReader["`$Name`"] != DBNull.Value) ? odbcReader["`$Name`"].ToString() : string.Empty;
                    ledgerinfo.Account_Number = (odbcReader["`$BankDetails`"] != DBNull.Value) ? odbcReader["`$BankDetails`"].ToString() : string.Empty;
                    ledgerinfo.Phone = (odbcReader["`$LedgerPhone`"] != DBNull.Value) ? odbcReader["`$LedgerPhone`"].ToString() : string.Empty;
                    ledgerinfo.Email = (odbcReader["`$EMail`"] != DBNull.Value) ? odbcReader["`$EMail`"].ToString() : string.Empty;
                    ledgerinfo.Pricing_Level = (odbcReader["`$PriceLevel`"] != DBNull.Value) ? odbcReader["`$PriceLevel`"].ToString() : "NA";
                    ledgerinfo.Credit_Period = (odbcReader["`$BillCreditPeriod`"] != DBNull.Value) ? (int.Parse(new string(odbcReader["`$BillCreditPeriod`"].ToString().Where(char.IsDigit).ToArray()))).ToString() : "0";
                    ledgerinfo.TIN = (odbcReader["`$VATTINNumber`"] != DBNull.Value) ? odbcReader["`$VATTINNumber`"].ToString() : string.Empty;
                    ledgerinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    decimal decClosing = (odbcReader["`$ClosingBalance`"] != DBNull.Value) ? (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString())) : 0;
                    ledgerinfo.CrorDr = (decClosing < 0) ? "Dr" : "Cr";
                    lstCustomerSupplierInfo.Add(ledgerinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstCustomerSupplierInfo;
        }
        /// <summary>
        /// Function to get Account group.
        /// </summary>
        /// <returns></returns>
        public List<accountGroupInfo> GetAccountGroup()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$Parent`,`$AffectsGrossProfit`,`$Narration`,`$_PrimaryGroup` FROM Groups", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                accountGroupInfo infoaccountgroup;
                while (odbcReader.Read())
                {
                    infoaccountgroup = new accountGroupInfo();
                    infoaccountgroup.Name = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    infoaccountgroup.Affect_Gross_Profit = (odbcReader["`$AffectsGrossProfit`"] != DBNull.Value) ? (odbcReader["`$AffectsGrossProfit`"].ToString() == "1").ToString() : "false";
                    infoaccountgroup.Under = (odbcReader["`$Parent`"] != DBNull.Value) ? CheckName(odbcReader["`$Parent`"].ToString()) : string.Empty;
                    infoaccountgroup.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    infoaccountgroup.Nature = (odbcReader["`$Name`"] != DBNull.Value) ? ((GetNature(infoaccountgroup.Name.ToLower()) == string.Empty) ? GetNature(infoaccountgroup.Under.ToLower()) : GetNature(infoaccountgroup.Name.ToLower())) : "Primary";
                    lstAccountGroup.Add(infoaccountgroup);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstAccountGroup;
        }
        /// <summary>
        /// Function to get Account Ledger.
        /// </summary>
        /// <returns></returns>
        public List<LedgerInfo> GetAccountLedger()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$_PrimaryGroup`,`$OnAccountValue`,`$Parent`, `$ClosingBalance`,`$Narration` FROM Ledger WHERE `$Parent` NOT IN (Sundry Creditors,Sundry Debtors)", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                LedgerInfo ledgerinfo;
                while (odbcReader.Read())
                {
                    decimal Closing = (odbcReader["`$ClosingBalance`"] != DBNull.Value) ? (Convert.ToDecimal(odbcReader["`$ClosingBalance`"].ToString())) : 0;
                    ledgerinfo = new LedgerInfo();
                    ledgerinfo.Name = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    ledgerinfo.Account_Group = (odbcReader["`$Parent`"] != DBNull.Value) ? CheckName(odbcReader["`$Parent`"].ToString()) : "Primary";
                    string ss = checking.functionCheckNature(ledgerinfo.Account_Group);
                    ledgerinfo.Opening_Balance = ((ss == "Assets" || ss == "Liabilities" || ss == "NA") ? ((Closing < 0) ? (Closing * -1).ToString() : (Closing).ToString()) : "0");
                    ledgerinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    ledgerinfo.CrorDr = (Closing < 0) ? "Dr" : "Cr";
                    lstLedgerInfo.Add(ledgerinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstLedgerInfo;
        }
        /// <summary>
        /// Function to get Product Group.
        /// </summary>
        /// <returns></returns>
        public List<productGroupInfo> GetProductGroup()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$Parent`,`$Narration` FROM MultiStockGroup", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                productGroupInfo productgroupinfo;
                while (odbcReader.Read())
                {
                    productgroupinfo = new productGroupInfo();
                    productgroupinfo.GroupName = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    productgroupinfo.Group_Under = (odbcReader["`$Parent`"] != DBNull.Value) ? CheckName(odbcReader["`$Parent`"].ToString()) : "Primary";
                    productgroupinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    lstproductgroupinfo.Add(productgroupinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstproductgroupinfo;
        }
        /// <summary>
        /// Function to get Unit.
        /// </summary>
        /// <returns></returns>
        public List<unitsInfo> GetUnit()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$OriginalSymbol`,`$Name`,`$DecimalPlaces`,`$Narration` FROM Unit WHERE `$IsSimpleUnit`='Yes'", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                unitsInfo unitinfo;
                while (odbcReader.Read())
                {

                    unitinfo = new unitsInfo();
                    unitinfo.UnitName = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    unitinfo.FormalName = (odbcReader["`$OriginalSymbol`"] != DBNull.Value) ? odbcReader["`$OriginalSymbol`"].ToString() : string.Empty; ;
                    unitinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    unitinfo.noOfDecimalPlaces = (odbcReader["`$DecimalPlaces`"] != DBNull.Value) ? Convert.ToDecimal(odbcReader["`$DecimalPlaces`"].ToString()).ToString() : "0";
                    lstuniinfo.Add(unitinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstuniinfo;
        }
        /// <summary>
        /// Function to get Godown.
        /// </summary>
        /// <returns></returns>
        public List<godownInfo> GetGodown()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$Narration` FROM Godown", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                godownInfo godowninfo;
                while (odbcReader.Read())
                {

                    godowninfo = new godownInfo();
                    godowninfo.Godown_Name = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    godowninfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    lstgodowninfo.Add(godowninfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstgodowninfo;
        }
        /// <summary>
        /// Function to get Product.
        /// </summary>
        /// <returns></returns>
        public List<productInfo> GetProduct()
        {
            try
            {
                odbcCon.Open();
                int i=0;
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$Parent`,`$MinOrderRoundLimit`,`$_LastPurcCost`,`$_LastSalePrice`,`$RateofVAT`,`$IsBatchWiseOn`,`$BaseUnits`,`$_ClosingRate`,`$RateofMRP`,`$TaxClassificationName`,`$ReorderBase`,`$BaseUnits`,`$ClosingBalance`,`$Narration`,`$Conversion`,`$_ClosingBalance`,`$Denominator` FROM StockItem", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                productInfo productinfo;
                while (odbcReader.Read())
                {
                    productinfo = new productInfo();
                    productinfo.ProductName = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    productinfo.Group = (odbcReader["`$Parent`"] != DBNull.Value) ? CheckName(odbcReader["`$Parent`"].ToString()) : "Primary";
                    productinfo.Tax = "NA";
                    productinfo.AllowBatch = (odbcReader["`$IsBatchWiseOn`"] != DBNull.Value) ? Convert.ToBoolean(odbcReader["`$IsBatchWiseOn`"]).ToString() : "false";
                    productinfo.ConversionRate = (odbcReader["`$Conversion`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$Conversion`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$Conversion`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$Conversion`"].ToString())).ToString() : "0";
                    productinfo.DefaultGodown = "NA";
                    productinfo.MaximumStock = (odbcReader["`$ReorderBase`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$ReorderBase`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$ReorderBase`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$ReorderBase`"].ToString())).ToString() : "0";
                    productinfo.MinimumStock = (odbcReader["`$MinOrderRoundLimit`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$MinOrderRoundLimit`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$MinOrderRoundLimit`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$MinOrderRoundLimit`"].ToString())).ToString() : "0";
                    productinfo.MRP = (odbcReader["`$RateofMRP`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$RateofMRP`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$RateofMRP`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$RateofMRP`"].ToString())).ToString() : "0";
                    productinfo.MultipleUnit = "false";
                    productinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? odbcReader["`$Narration`"].ToString() : string.Empty;
                    productinfo.OpeningStock = (odbcReader["`$_ClosingBalance`"] != DBNull.Value) ? (Convert.ToDecimal(odbcReader["`$_ClosingBalance`"].ToString()) > 0).ToString() : "false";
                    productinfo.PurchaseRate = (odbcReader["`$_LastPurcCost`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$_LastPurcCost`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$_LastPurcCost`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$_LastPurcCost`"].ToString())).ToString() : "0";
                    productinfo.ReorderLevel = (odbcReader["`$ReorderBase`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$ReorderBase`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$ReorderBase`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$ReorderBase`"].ToString())).ToString() : "0";
                    productinfo.SalesRate = (odbcReader["`$_LastSalePrice`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$_LastSalePrice`"].ToString()) < 0) ? Convert.ToDecimal(odbcReader["`$_LastSalePrice`"].ToString()) * -1 : Convert.ToDecimal(odbcReader["`$_LastSalePrice`"].ToString())).ToString() : "0";
                    productinfo.TaxApplicableOn = (odbcReader["`$TaxClassificationName`"] != DBNull.Value) ? CheckName(odbcReader["`$TaxClassificationName`"].ToString()) : "NA";
                    productinfo.Unit = (odbcReader["`$BaseUnits`"] != DBNull.Value) ? odbcReader["`$BaseUnits`"].ToString() : "NA";
                    productinfo.ProductCode = "TC"+ ++i;
                    lstproductinfo.Add(productinfo);
                }
                odbcCon.Close();
                odbcReader.Close();


            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstproductinfo;
        }
        /// <summary>
        /// Function to get Product.
        /// </summary>
        /// <returns></returns>
        public List<StockInfo> GetStock()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$Parent`,`$MinOrderRoundLimit`,`$_ClosingValue`,`$_LastPurcCost`,`$_LastSalePrice`,`$RateofVAT`,`$IsBatchWiseOn`,`$BaseUnits`,`$_ClosingRate`,`$RateofMRP`,`$TaxClassificationName`,`$ReorderBase`,`$BaseUnits`,`$ClosingBalance`,`$Narration`,`$Conversion`,`$_ClosingBalance`,`$Denominator` FROM StockItem", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                StockInfo productinfo;
                while (odbcReader.Read())
                {

                    productinfo = new StockInfo();
                    productinfo.ProductName = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    productinfo.OpeningStock = (odbcReader["`$_ClosingBalance`"] != DBNull.Value) ? (Convert.ToDecimal(odbcReader["`$_ClosingBalance`"].ToString()) > 0).ToString() : "false";
                    productinfo.ClosingRate = (odbcReader["`$_ClosingValue`"] != DBNull.Value && odbcReader["`$_ClosingBalance`"] != DBNull.Value) ? ((Convert.ToDecimal(odbcReader["`$_ClosingValue`"].ToString()) / Convert.ToDecimal(odbcReader["`$_ClosingBalance`"].ToString())) * -1).ToString() : ((odbcReader["`$_ClosingValue`"] != DBNull.Value) ? Convert.ToDecimal(odbcReader["`$_ClosingValue`"].ToString()) * -1 : 0).ToString();
                    productinfo.OpeningStockNumber = (odbcReader["`$_ClosingBalance`"] != DBNull.Value) ? Convert.ToDecimal(odbcReader["`$_ClosingBalance`"].ToString()).ToString() : ((productinfo.ClosingRate == "0") ? 0 : 1).ToString();
                    productinfo.Unit = (odbcReader["`$BaseUnits`"] != DBNull.Value) ? odbcReader["`$BaseUnits`"].ToString() : "NA";
                    lstStockInfo.Add(productinfo);
                }
                odbcCon.Close();
                odbcReader.Close();


            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstStockInfo;
        }
        /// <summary>
        /// Function to get Batch.
        /// </summary>
        /// <returns></returns>
        public List<batchesInfo> GetBatch()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name` FROM StockItem WHERE `$IsBatchWiseOn`='Yes'", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                batchesInfo batchinfo;
                while (odbcReader.Read())
                {

                    batchinfo = new batchesInfo();
                    batchinfo.ProductName = (odbcReader["`$Name`"] != DBNull.Value) ? CheckName(odbcReader["`$Name`"].ToString()) : string.Empty;
                    lstbatches.Add(batchinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstbatches;
        }
        /// <summary>
        /// Function to get Currency.
        /// </summary>
        /// <returns></returns>
        public List<currencyInfo> GetCurrency()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$AdditionalName`,`$OriginalSymbol`,`$DecimalPlaces`,`$Narration` FROM Currency", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                currencyInfo currencyinfo;
                while (odbcReader.Read())
                {

                    currencyinfo = new currencyInfo();
                    currencyinfo.CurrencyName = (odbcReader["`$AdditionalName`"] != DBNull.Value) ? CheckName(odbcReader["`$AdditionalName`"].ToString()) : string.Empty;
                    currencyinfo.CurrencySymbol = (odbcReader["`$OriginalSymbol`"] != DBNull.Value) ? CheckName(odbcReader["`$OriginalSymbol`"].ToString()) : string.Empty;
                    currencyinfo.NoOfDecimalPlaces = (odbcReader["`$DecimalPlaces`"] != DBNull.Value) ? odbcReader["`$DecimalPlaces`"].ToString() : "0";
                    currencyinfo.Narration = (odbcReader["`$Narration`"] != DBNull.Value) ? CheckName(odbcReader["`$Narration`"].ToString()) : string.Empty;
                    lstcurrencyinfo.Add(currencyinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstcurrencyinfo;
        }
        /// <summary>
        /// Function to get Voucher Type.
        /// </summary>
        /// <returns></returns>
        public List<voucherTypesInfo> GetVoucherType()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$Name`,`$Parent`,`$NumberingMethod`,`$Narration`,`$VchPrintDecl`, `$ActiveTo` FROM VoucherType", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                voucherTypesInfo vouchertypeinfo;
                while (odbcReader.Read())
                {

                    vouchertypeinfo = new voucherTypesInfo();
                    vouchertypeinfo.name = (odbcReader["`$Name`"] != DBNull.Value) ? (odbcReader["`$Name`"].ToString()) : string.Empty;
                    vouchertypeinfo.active = (odbcReader["`$ActiveTo`"] != DBNull.Value) ? (Convert.ToDateTime(odbcReader["`$ActiveTo`"]) > DateTime.Today).ToString() : "true";
                    vouchertypeinfo.declaration = (odbcReader["`$VchPrintDecl`"] != DBNull.Value) ? CheckName(odbcReader["`$VchPrintDecl`"].ToString()) : string.Empty;
                    vouchertypeinfo.methodOfVoucherNumbering = (odbcReader["`$NumberingMethod`"] != DBNull.Value) ? ((odbcReader["`$NumberingMethod`"].ToString() == "None") ? "Automatic" : odbcReader["`$NumberingMethod`"].ToString()) : string.Empty;
                    vouchertypeinfo.narration = (odbcReader["`$Narration`"] != DBNull.Value) ? CheckName(odbcReader["`$Narration`"].ToString()) : string.Empty;
                    vouchertypeinfo.typeOfVoucher = (odbcReader["`$Parent`"] != DBNull.Value) ? CheckName(odbcReader["`$Parent`"].ToString()) : string.Empty;
                    vouchertypeinfo.dotMatrixPrintFormat = vouchertypeinfo.typeOfVoucher;
                    lstvouchertypinfo.Add(vouchertypeinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstvouchertypinfo;
        }
        /// <summary>
        /// Function to get Salesmen.
        /// </summary>
        /// <returns></returns>
        public List<EmployeeInfo> GetSalesMen()
        {
            try
            {
                odbcCon.Open();
                OdbcCommand odbcCom = new OdbcCommand("SELECT `$AdditionalName`,`$Name`,`$EmailID`,`$ContactNumbers`,`$ContactNumbers`,`$Address`,`$Narration`,`$DeactivationDate` FROM Employees", odbcCon);
                OdbcDataReader odbcReader = odbcCom.ExecuteReader();
                Clear();
                EmployeeInfo employeeinfo;
                while (odbcReader.Read())
                {

                    employeeinfo = new EmployeeInfo();
                    employeeinfo.name = (odbcReader["`$AdditionalName`"] != DBNull.Value) ? (odbcReader["`$AdditionalName`"].ToString()) : string.Empty;
                    employeeinfo.address = (odbcReader["`$Name`"] != DBNull.Value) ? (odbcReader["`$Name`"].ToString()) : string.Empty;
                    employeeinfo.eMail = (odbcReader["`$EmailID`"] != DBNull.Value) ? (odbcReader["`$EmailID`"].ToString()) : string.Empty;
                    employeeinfo.employeeCode = (odbcReader["`$Name`"] != DBNull.Value) ? (odbcReader["`$Name`"].ToString()) : string.Empty;
                    employeeinfo.isActive = (odbcReader["`$DeactivationDate`"] != DBNull.Value) ? (Convert.ToDateTime(odbcReader["`$DeactivationDate`"]) < DateTime.Today).ToString() : "false";
                    employeeinfo.mobile = (odbcReader["`$ContactNumbers`"] != DBNull.Value) ? (odbcReader["`$ContactNumbers`"].ToString()) : string.Empty;
                    employeeinfo.narration = (odbcReader["`$Narration`"] != DBNull.Value) ? (odbcReader["`$Narration`"].ToString()) : string.Empty;
                    employeeinfo.phone = (odbcReader["`$ContactNumbers`"] != DBNull.Value) ? CheckName(odbcReader["`$ContactNumbers`"].ToString()) : string.Empty;
                    lstemplyeeinfo.Add(employeeinfo);
                }
                odbcCon.Close();
                odbcReader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            return lstemplyeeinfo;
        }
    }

}
