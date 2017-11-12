using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AccountSkate
{
    class MasterValidations : Checkings
    {
        bool isT;
        decimal isD;
        int isI;
        #region Ledgers
        internal System.Windows.Media.Brush CustomerValidations(CustomerSupplierInfo ledgerinfoforcustomer, out string toolTip)
        {
            if (ledgerinfoforcustomer.Name != string.Empty && ledgerinfoforcustomer.Name != null)
            {
                if (!CheckCustomerExist(ledgerinfoforcustomer.Name))
                {
                    if (decimal.TryParse(ledgerinfoforcustomer.Credit_Period, out isD))
                    {
                        if (decimal.TryParse(ledgerinfoforcustomer.Credit_Limit, out isD))
                        {
                            if (decimal.TryParse(ledgerinfoforcustomer.Opening_Balance, out isD))
                            {
                                if (bool.TryParse(ledgerinfoforcustomer.Bill_By_Bill, out isT))
                                {
                                    if (ledgerinfoforcustomer.CrorDr == "Cr" || ledgerinfoforcustomer.CrorDr == "Dr")
                                    {
                                        if (Convert.ToDecimal(ledgerinfoforcustomer.Credit_Period) >= 0)
                                        {
                                            if (Convert.ToDecimal(ledgerinfoforcustomer.Credit_Limit) >= 0)
                                            {
                                                if (Convert.ToDecimal(ledgerinfoforcustomer.Opening_Balance) >= 0)
                                                {
                                                    toolTip = "Customer";
                                                    return System.Windows.Media.Brushes.LightGreen;
                                                }
                                                else
                                                {
                                                    toolTip = "Opening Balance not contain negative values.";
                                                    return System.Windows.Media.Brushes.LightGray;
                                                }
                                            }
                                            else
                                            {
                                                toolTip = "Credit Limit not contain negative values.";
                                                return System.Windows.Media.Brushes.LightGray;
                                            }
                                        }
                                        else
                                        {
                                            toolTip = "Credit Period not contain negative values.";
                                            return System.Windows.Media.Brushes.LightGray;
                                        }
                                    }
                                    else
                                    {
                                        toolTip = "Credit or Debit not matching with value, it contain only Cr/Dr .";
                                        return System.Windows.Media.Brushes.LightSteelBlue;
                                    }
                                }
                                else
                                {
                                    toolTip = "Bill By Bill type not matching, it contain only True/False .";
                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                }
                            }
                            else
                            {
                                toolTip = "Opening Balance type not matching, it contain only numeric values.";
                                return System.Windows.Media.Brushes.LightSteelBlue;
                            }
                        }
                        else
                        {
                            toolTip = "Credit Limit value type not matching, it contain only numeric values.";
                            return System.Windows.Media.Brushes.LightSteelBlue;
                        }
                    }
                    else
                    {
                        toolTip = "Credit Period value type not matching, it contain only numeric values.";
                        return System.Windows.Media.Brushes.LightSteelBlue;
                    }
                }
                else
                {
                    toolTip = "Customer name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Customer name is empty.";
                return System.Windows.Media.Brushes.Red;
            }
        }

        public bool CustomerValidations(CustomerSupplierInfo ledgerinfoforcustomer)
        {
            return (ledgerinfoforcustomer.Name != string.Empty && ledgerinfoforcustomer.Name != null && !CheckCustomerExist(ledgerinfoforcustomer.Name) && 
            (decimal.TryParse(ledgerinfoforcustomer.Credit_Period, out isD)) && (decimal.TryParse(ledgerinfoforcustomer.Credit_Limit, out isD)) &&
            (decimal.TryParse(ledgerinfoforcustomer.Opening_Balance, out isD)) && (bool.TryParse(ledgerinfoforcustomer.Bill_By_Bill, out isT)) && (ledgerinfoforcustomer.CrorDr == "Cr" || ledgerinfoforcustomer.CrorDr == "Dr") && 
            (Convert.ToDecimal(ledgerinfoforcustomer.Credit_Period) >= 0) && (Convert.ToDecimal(ledgerinfoforcustomer.Credit_Limit) >= 0)  &&  (Convert.ToDecimal(ledgerinfoforcustomer.Opening_Balance) >= 0));
        }

        internal System.Windows.Media.Brush SupplierValidations(CustomerSupplierInfo ledgerinfoforcustomer, out string toolTip)
        {
            if (ledgerinfoforcustomer.Name != string.Empty && ledgerinfoforcustomer.Name != null)
            {
                if (!CheckCustomerExist(ledgerinfoforcustomer.Name))
                {
                    if (decimal.TryParse(ledgerinfoforcustomer.Credit_Period, out isD))
                    {
                        if (decimal.TryParse(ledgerinfoforcustomer.Credit_Limit, out isD))
                        {
                            if (decimal.TryParse(ledgerinfoforcustomer.Opening_Balance, out isD))
                            {
                                if (bool.TryParse(ledgerinfoforcustomer.Bill_By_Bill, out isT))
                                {
                                    if (ledgerinfoforcustomer.CrorDr == "Cr" || ledgerinfoforcustomer.CrorDr == "Dr")
                                    {
                                        if (Convert.ToDecimal(ledgerinfoforcustomer.Credit_Period) >= 0)
                                        {
                                            if (Convert.ToDecimal(ledgerinfoforcustomer.Credit_Limit) >= 0)
                                            {
                                                if (Convert.ToDecimal(ledgerinfoforcustomer.Opening_Balance) >= 0)
                                                {
                                                    toolTip = "Supplier";
                                                    return System.Windows.Media.Brushes.LightGreen;
                                                }
                                                else
                                                {
                                                    toolTip = "Opening Balance not contain negative values.";
                                                    return System.Windows.Media.Brushes.LightGray;
                                                }
                                            }
                                            else
                                            {
                                                toolTip = "Credit Limit not contain negative values.";
                                                return System.Windows.Media.Brushes.LightGray;
                                            }

                                        }
                                        {
                                            toolTip = "Credit Period not contain negative values.";
                                            return System.Windows.Media.Brushes.LightGray;
                                        }
                                    }
                                    else
                                    {
                                        toolTip = "Credit or Debit not matching with value, it contain only Cr/Dr .";
                                        return System.Windows.Media.Brushes.LightSteelBlue;
                                    }
                                }
                                else
                                {
                                    toolTip = "Bill By Bill type not matching, it contain only True/False .";
                                    return System.Windows.Media.Brushes.LightSteelBlue;

                                }
                            }
                            else
                            {
                                toolTip = "Opening Balance type not matching, it contain only numeric values.";
                                return System.Windows.Media.Brushes.LightSteelBlue;
                            }
                        }
                        else
                        {
                            toolTip = "Credit Limit value type not matching, it contain only numeric values.";
                            return System.Windows.Media.Brushes.LightSteelBlue;
                        }
                    }
                    else
                    {
                        toolTip = "Credit Period value type not matching, it contain only numeric values.";
                        return System.Windows.Media.Brushes.LightSteelBlue;
                    }
                }
                else
                {
                    toolTip = "Supplier name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Supplier name is empty.";
                return System.Windows.Media.Brushes.Red;
            }
        }

        public bool SupplierValidations(CustomerSupplierInfo Supplier)
        {
            return (Supplier.Name != string.Empty && Supplier.Name != null && (!CheckCustomerExist(Supplier.Name)) && (decimal.TryParse(Supplier.Credit_Period, out isD))
            && (decimal.TryParse(Supplier.Credit_Limit, out isD)) && (decimal.TryParse(Supplier.Opening_Balance, out isD)) && (bool.TryParse(Supplier.Bill_By_Bill, out isT))
            &&  (Supplier.CrorDr == "Cr" || Supplier.CrorDr == "Dr") && (Convert.ToDecimal(Supplier.Credit_Period) >= 0) &&  (Convert.ToDecimal(Supplier.Credit_Limit) >= 0)
            &&  (Convert.ToDecimal(Supplier.Opening_Balance) >= 0));
        }

        internal System.Windows.Media.Brush LedgerValidations(LedgerInfo accountLedgerinfo, out string toolTip)
        {
            if (accountLedgerinfo.Name != string.Empty && accountLedgerinfo.Name != null)
            {
                if (!CheckAccountLedgerExist(accountLedgerinfo.Name))
                {
                    if (functionCheckAccountGroupUnderId(accountLedgerinfo.Account_Group) != -1)
                    {
                        if (decimal.TryParse(accountLedgerinfo.Opening_Balance, out isD))
                        {
                            if (Convert.ToDecimal(accountLedgerinfo.Opening_Balance) >= 0)
                            {
                                if (accountLedgerinfo.CrorDr == "Cr" || accountLedgerinfo.CrorDr == "Dr")
                                {
                                    toolTip = "Account Ledger";
                                    return System.Windows.Media.Brushes.LightGreen;
                                }
                                else
                                {
                                    toolTip = "Credit or Debit not matching with value, it contain only Cr/Dr .";
                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                }
                            }
                            else
                            {
                                toolTip = "Opening Balance not contain negative values.";
                                return System.Windows.Media.Brushes.LightGray;
                            }
                        }
                        else
                        {
                            toolTip = "Opening Balance type not matching, it contain only numeric values.";
                            return System.Windows.Media.Brushes.LightSteelBlue;
                        }
                    }
                    else
                    {
                        toolTip = "Account group does not belong to OpenAccount, Add Account group first.";
                        return System.Windows.Media.Brushes.Orange;
                    }
                }
                else
                {
                    toolTip = "Account Ledger name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Ledger name is empty.";
                return System.Windows.Media.Brushes.Red;
            }
        }

        public bool LedgerValidations(LedgerInfo Ledger)
        {
            return (Ledger.Name != string.Empty && Ledger.Name != null && !CheckAccountLedgerExist(Ledger.Name) && functionCheckAccountGroupUnderId(Ledger.Account_Group) != -1 && decimal.TryParse(Ledger.Opening_Balance, out isD) && Convert.ToDecimal(Ledger.Opening_Balance) >= 0 &&
            (Ledger.CrorDr == "Cr" || Ledger.CrorDr == "Dr"));
        }

        internal bool SavingValidation_AccountingLedger(AccountLedgerInfoforOpenAccount infoAccountLedger)
        {
            return (infoAccountLedger.AccountGroupId != -1 && infoAccountLedger.Name != string.Empty && infoAccountLedger.Name != null && !CheckAccountLedgerExist(infoAccountLedger.Name) && !CheckAccountLedgerExist(infoAccountLedger.Name));
        }
        #endregion
        #region AccountGroup
        internal System.Windows.Media.Brush AccountGroupValidation(accountGroupInfo accountgroupinfo, out string toolTip)
        {
            if (accountgroupinfo.Name != string.Empty && accountgroupinfo.Name != null)
            {
                if (accountgroupinfo.Under != string.Empty && accountgroupinfo.Under != null)
                {
                    if (!CheckAccountGroupExist(accountgroupinfo.Name))
                    {
                        if (accountgroupinfo.Nature != string.Empty)
                        {
                            if (bool.TryParse(accountgroupinfo.Affect_Gross_Profit, out isT))
                            {
                                toolTip = "Account Group";
                                return System.Windows.Media.Brushes.LightGreen;
                            }
                            else
                            {
                                toolTip = "Affect Gross Profit Column Value does not match, Here only contain True of False";
                                return System.Windows.Media.Brushes.LightSteelBlue;
                            }
                        }
                        else
                        {
                            toolTip = "Nature does not exist because Account group under does not belong to OpenAccount,Repeated clicking on the skate button,may catches the Nature.";
                            return System.Windows.Media.Brushes.Orange;
                        }
                    }
                    else
                    {
                        toolTip = "Account Group Name exist.";
                        return System.Windows.Media.Brushes.LightCoral;
                    }
                }
                else
                {
                    toolTip = "Account Group Under is empty.";
                    return System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                toolTip = "Account Group Name is empty.";
                return System.Windows.Media.Brushes.Red;
            }

        }

        internal bool AccountGroupValidation(accountGroupInfo accountgroupinfo)
        {
            return (accountgroupinfo.Name != "" && accountgroupinfo.Name != null && accountgroupinfo.Under != "" && accountgroupinfo.Under != null && !CheckAccountGroupExist(accountgroupinfo.Name) && accountgroupinfo.Nature != string.Empty && bool.TryParse(accountgroupinfo.Affect_Gross_Profit, out isT));
        }

        internal bool SavingValidation_AccountingGroup(AccountGroupInfoforOpenAccount infoAccountGroup)
        {
            return (!CheckAccountGroupExist(infoAccountGroup.Name) && infoAccountGroup.Name != "" && infoAccountGroup.Nature != "" && infoAccountGroup.Name != null && infoAccountGroup.Nature != null && infoAccountGroup.Under != -1);
        }
        #endregion
        #region Product
        internal System.Windows.Media.Brush ProductValidation(productInfo productinfo, out string toolTip)
        {
            if (productinfo.ProductName != null && productinfo.ProductName != string.Empty)
            {
                if (productinfo.ProductCode != null && productinfo.ProductCode != string.Empty)
                {
                    if (productinfo.Group != null && productinfo.Group != string.Empty)
                    {
                        if (productinfo.Unit != null && productinfo.Unit != string.Empty)
                        {
                            if (!CheckProductExist(productinfo.ProductName))
                            {
                                if (CheckUnitExist(productinfo.Unit))
                                {
                                    if (CheckProductGroupExist(productinfo.Group))
                                    {
                                        if (functionCheckProductCodeExists(productinfo.ProductCode))
                                        {
                                            if (functionCheckProductGroupUnderId(productinfo.Group) != -1)
                                            {
                                                if (functionCheckUnitId(productinfo.Unit) != -1)
                                                {
                                                    if (functionCheckTaxId(productinfo.Tax) != -1)
                                                    {
                                                        if (functionCheckGodownId(productinfo.DefaultGodown) != -1)
                                                        {
                                                            if (functionCheckRackId(productinfo.Rack) != -1)
                                                            {
                                                                if (Decimal.TryParse(productinfo.MRP, out isD))
                                                                {
                                                                    if (Decimal.TryParse(productinfo.ConversionRate, out isD))
                                                                    {
                                                                        if (Decimal.TryParse(productinfo.PurchaseRate, out isD))
                                                                        {
                                                                            if (Decimal.TryParse(productinfo.SalesRate, out isD))
                                                                            {
                                                                                if (Decimal.TryParse(productinfo.MinimumStock, out isD))
                                                                                {
                                                                                    if (Decimal.TryParse(productinfo.MaximumStock, out isD))
                                                                                    {
                                                                                        if (Decimal.TryParse(productinfo.ReorderLevel, out isD))
                                                                                        {
                                                                                            if (Convert.ToDecimal(productinfo.MRP) >= 0)
                                                                                            {
                                                                                                if (Convert.ToDecimal(productinfo.ConversionRate) >= 0)
                                                                                                {
                                                                                                    if (Convert.ToDecimal(productinfo.PurchaseRate) >= 0)
                                                                                                    {
                                                                                                        if (Convert.ToDecimal(productinfo.SalesRate) >= 0)
                                                                                                        {
                                                                                                            if (Convert.ToDecimal(productinfo.MinimumStock) >= 0)
                                                                                                            {
                                                                                                                if (Convert.ToDecimal(productinfo.MaximumStock) >= 0)
                                                                                                                {
                                                                                                                    if (Convert.ToDecimal(productinfo.ReorderLevel) >= 0)
                                                                                                                    {
                                                                                                                        if (bool.TryParse(productinfo.AllowBatch, out isT))
                                                                                                                        {
                                                                                                                            if (bool.TryParse(productinfo.MultipleUnit, out isT))
                                                                                                                            {
                                                                                                                                if (bool.TryParse(productinfo.OpeningStock, out isT))
                                                                                                                                {
                                                                                                                                    toolTip = "Product";
                                                                                                                                    return System.Windows.Media.Brushes.LightGreen;
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    toolTip = "Opening stock type not matching, it contain only True/False";
                                                                                                                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                toolTip = "Multiple unit type not matching, it contain only True/False.";
                                                                                                                                return System.Windows.Media.Brushes.LightSteelBlue;

                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            toolTip = "Allow Batch type not matching, it contain only True/False.";
                                                                                                                            return System.Windows.Media.Brushes.LightSteelBlue;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        toolTip = "Reorder level not contain negative values";
                                                                                                                        return System.Windows.Media.Brushes.LightGray;
                                                                                                                    }
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    toolTip = "Maximum stock not contain negative values";
                                                                                                                    return System.Windows.Media.Brushes.LightGray;
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                toolTip = "Minimum stock not contain negative values";
                                                                                                                return System.Windows.Media.Brushes.LightGray;
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            toolTip = "Sales rate not contain negative values";
                                                                                                            return System.Windows.Media.Brushes.LightGray;
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        toolTip = "Purchase rate not contain negativevalue.";
                                                                                                        return System.Windows.Media.Brushes.LightGray;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toolTip = "Conversion rate not contain negative values.";
                                                                                                    return System.Windows.Media.Brushes.LightGray;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toolTip = "MRP not contain negative values.";
                                                                                                return System.Windows.Media.Brushes.LightGray;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            toolTip = "Reorderlevel not matching, it contain only numeric values.";
                                                                                            return System.Windows.Media.Brushes.LightSteelBlue;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        toolTip = "Maximum stock not matching, it contain only numeric values.";
                                                                                        return System.Windows.Media.Brushes.LightSteelBlue;
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    toolTip = "Minimum stock not matching, it contain only numeric values.";
                                                                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                toolTip = "Sale Rate not matching, it contain only numeric values.";
                                                                                return System.Windows.Media.Brushes.LightSteelBlue;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            toolTip = "Purchase Rate not matcing, it contain only numeric values.";
                                                                            return System.Windows.Media.Brushes.LightSteelBlue;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        toolTip = "Conversion Rate type not matching, it contain only numeric values.";
                                                                        return System.Windows.Media.Brushes.LightSteelBlue;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    toolTip = "MRP type not matching, it contain only numeric values.";
                                                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                toolTip = "Rack does not exist, Add Rack first.";
                                                                return System.Windows.Media.Brushes.Orange;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            toolTip = "Godown does not exist, Add Godown first.";
                                                            return System.Windows.Media.Brushes.Orange;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        toolTip = "Product Tax does not exist, Add Tax first.";
                                                        return System.Windows.Media.Brushes.Orange;
                                                    }
                                                }
                                                {
                                                    toolTip = "Product Unit does not exist, Add unit first.";
                                                    return System.Windows.Media.Brushes.Orange;
                                                }
                                            }
                                            {
                                                toolTip = "Product Group does not exist, Add Product group first.";
                                                return System.Windows.Media.Brushes.Orange;
                                            }
                                        }
                                        else
                                        {
                                            toolTip = "Product Code Exist.";
                                            return System.Windows.Media.Brushes.LightCoral;
                                        }
                                    }
                                    else
                                    {
                                        toolTip = "Product Group not exist,Add Product Group first";
                                        return System.Windows.Media.Brushes.Orange;
                                    }
                                }
                                else
                                {
                                    toolTip = "Unit not Exist,Add Unit First.";
                                    return System.Windows.Media.Brushes.Orange;
                                }
                            }
                            else
                            {
                                toolTip = "Product name Exist.";
                                return System.Windows.Media.Brushes.LightCoral;
                            }
                        }
                        else
                        {
                            toolTip = "Unit is empty";
                            return System.Windows.Media.Brushes.Red;
                        }
                    }
                    else
                    {
                        toolTip = "Product group is empty";
                        return System.Windows.Media.Brushes.Red;
                    }
                }
                else
                {
                    toolTip = "Product code is empty";
                    return System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                toolTip = "Product Name is empty.";
                return System.Windows.Media.Brushes.Red;
            }

        }

        internal bool ProductValidation(productInfo productinfo)
        {
            return (productinfo.ProductName != null &&
                    productinfo.ProductName != string.Empty &&
  productinfo.ProductCode != null &&
  productinfo.ProductCode != string.Empty &&
 productinfo.Group != null &&
  productinfo.Group != string.Empty &&
  productinfo.Unit != null &&
  productinfo.Unit != string.Empty &&
 (!CheckProductExist(productinfo.ProductName)) &&
 (CheckUnitExist(productinfo.Unit)) &&
 (CheckProductGroupExist(productinfo.Group)) &&
 (functionCheckProductCodeExists(productinfo.ProductCode)) &&
  (functionCheckProductGroupUnderId(productinfo.Group) != -1) &&
  (functionCheckUnitId(productinfo.Unit) != -1) &&
   (functionCheckTaxId(productinfo.Tax) != -1) &&
  (functionCheckGodownId(productinfo.DefaultGodown) != -1) &&
   (functionCheckRackId(productinfo.Rack) != -1) &&
   (Decimal.TryParse(productinfo.MRP, out isD)) &&
   (Decimal.TryParse(productinfo.ConversionRate, out isD)) &&
   (Decimal.TryParse(productinfo.PurchaseRate, out isD)) &&
   (Decimal.TryParse(productinfo.SalesRate, out isD)) &&
    (Decimal.TryParse(productinfo.MinimumStock, out isD)) &&
  (Decimal.TryParse(productinfo.MaximumStock, out isD)) &&
  (Decimal.TryParse(productinfo.ReorderLevel, out isD)) &&
 Convert.ToDecimal(productinfo.MRP) >= 0 &&
 (Convert.ToDecimal(productinfo.ConversionRate) >= 0) &&
 (Convert.ToDecimal(productinfo.PurchaseRate) >= 0) &&
  (Convert.ToDecimal(productinfo.SalesRate) >= 0) &&
  (Convert.ToDecimal(productinfo.MinimumStock) >= 0) &&
  (Convert.ToDecimal(productinfo.MaximumStock) >= 0) &&
  (Convert.ToDecimal(productinfo.ReorderLevel) >= 0) &&
   (bool.TryParse(productinfo.AllowBatch, out isT)) &&
  (bool.TryParse(productinfo.MultipleUnit, out isT)) &&
  (bool.TryParse(productinfo.OpeningStock, out isT)));
         
           

        }

        internal bool SavingValidation_Product(ProductInfofroOpenAccount infoProduct)
        {
            return (!CheckProductExist(infoProduct.ProductName) && infoProduct.GodownId != -1 && infoProduct.Group != -1 && functionCheckProductCodeExists(infoProduct.ProductCode) && infoProduct.RackId != -1 && infoProduct.TaxId != -1 && infoProduct.UnitId != -1 && infoProduct.ProductName != "" && infoProduct.ProductName != null && infoProduct.ProductCode != "" && infoProduct.ProductCode != null);
        }
        #endregion
        #region Pricing Level
        internal System.Windows.Media.Brush PriceLevelValidation(pricingLevelInfo pricelevelinfo, out string toolTip)
        {
            if (pricelevelinfo.PricingLevelName != "" && pricelevelinfo.PricingLevelName != null)
            {
                if (!CheckPriceLevelExist(pricelevelinfo.PricingLevelName))
                {
                    toolTip = "Pricing Level";
                    return System.Windows.Media.Brushes.LightGreen;
                }
                else
                {
                    toolTip = "Pricing Level name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Pricing Level name is empty";
                return System.Windows.Media.Brushes.Red;
            }
        }

        internal bool PriceLevelValidation(pricingLevelInfo pricelevelinfo)
        {
            return (pricelevelinfo.PricingLevelName != string.Empty && pricelevelinfo.PricingLevelName != null && !CheckPriceLevelExist((pricelevelinfo.PricingLevelName)));

        }

        internal bool SavingValidation_PricingLevel(PriceLevelInfofroOpenAccount infoPricingLevel)
        {
            return (infoPricingLevel.PricingLevelName != null && infoPricingLevel.PricingLevelName != "" && !CheckPriceLevelExist(infoPricingLevel.PricingLevelName));
        }
        #endregion
        #region ProductGroup
        internal System.Windows.Media.Brush ProductGroupValidation(productGroupInfo productgroupinfo, out string toolTip)
        {
            if (productgroupinfo.GroupName != string.Empty && productgroupinfo.GroupName != null)
            {
                if (!CheckProductGroupExist(productgroupinfo.GroupName))
                {
                    if (functionCheckProductGroupUnderId(productgroupinfo.Group_Under) != -1)
                    {
                        toolTip = "Product Group";
                        return System.Windows.Media.Brushes.LightGreen;
                    }
                    else
                    {
                        toolTip = "Base group does not exist,Repeated clicking on the skate button,get base group.";
                        return System.Windows.Media.Brushes.Orange;
                    }
                }
                else
                {
                    toolTip = "Product Group name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Product Group name is empty";
                return System.Windows.Media.Brushes.Red;
            }
        }

        internal bool ProductGroupValidation(productGroupInfo productgroupinfo)
        {
            return (productgroupinfo.GroupName != string.Empty &&
                  productgroupinfo.GroupName != null &&
                  !CheckProductGroupExist(productgroupinfo.GroupName) &&
                  functionCheckProductGroupUnderId(productgroupinfo.Group_Under) != -1);
        }

        internal bool SavingValidation_ProductGroup(ProductGroupinfoforOpenAccount ProductGroupinfoforOpenAccount)
        {
            return (ProductGroupinfoforOpenAccount.GroupName != "" && ProductGroupinfoforOpenAccount.GroupUnder != -1 && ProductGroupinfoforOpenAccount.GroupName != null && !CheckProductGroupExist((ProductGroupinfoforOpenAccount.GroupName)));
        }

        #endregion
        #region Unit

        internal System.Windows.Media.Brush UnitValidation(unitsInfo unitinfo, out string toolTip)
        {
            if (unitinfo.UnitName != null && unitinfo.UnitName != string.Empty)
            {
                if (unitinfo.FormalName != null && unitinfo.FormalName != string.Empty)
                {
                    if (!CheckUnitExist(unitinfo.UnitName))
                    {
                        if (int.TryParse(unitinfo.noOfDecimalPlaces, out isI))
                        {
                            if (Convert.ToInt32(unitinfo.noOfDecimalPlaces) >= 0)
                            {
                                toolTip = "Unit";
                                return System.Windows.Media.Brushes.LightGreen;
                            }
                            else
                            {
                                toolTip = "No Of Decimal Places not contain negative values.";
                                return System.Windows.Media.Brushes.LightGray;
                            }
                        }
                        else
                        {
                            toolTip = "Number of decimal places type not matching, it contain only numeric values.";
                            return System.Windows.Media.Brushes.LightSteelBlue;
                        }
                    }
                    else
                    {
                        toolTip = "Unit name exist.";
                        return System.Windows.Media.Brushes.LightCoral;
                    }
                }
                else
                {
                    toolTip = "Formal name is empty";
                    return System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                toolTip = "Unit name is empty";
                return System.Windows.Media.Brushes.Red;
            }
        }

        internal bool UnitValidation(unitsInfo Unit)
        {
            return (Unit.FormalName != "" && Unit.UnitName != "" && Unit.FormalName != null && Unit.UnitName != null && int.TryParse(Unit.noOfDecimalPlaces, out isI) && (Convert.ToInt32(Unit.noOfDecimalPlaces) >= 0) && /*Convert.ToDecimal(Unit.noOfDecimalPlaces) >= 0 */  !CheckUnitExist(Unit.UnitName));
        }

        internal bool SavingValidation_Unit(UnitinfoforOpenAccount infoUnit)
        {
            return (infoUnit.FormalName != "" && infoUnit.UnitName != "" && infoUnit.FormalName != null && infoUnit.UnitName != null && !CheckUnitExist(infoUnit.UnitName));
        }
        #endregion
        #region Godown
        internal System.Windows.Media.Brush GodownValidation(godownInfo godowninfo, out string toolTip)
        {
            if (godowninfo.Godown_Name != string.Empty && godowninfo.Godown_Name != null)
            {
                if (!CheckGodownExist(godowninfo.Godown_Name))
                {
                    toolTip = "Godown";
                    return System.Windows.Media.Brushes.LightGreen;
                }
                else
                {
                    toolTip = "Godown name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Godown name is empty.";
                return System.Windows.Media.Brushes.Red;
            }

        }
        internal bool GodownValidation(godownInfo Godown)
        {
            return (Godown.Godown_Name != "" && Godown.Godown_Name != null && !CheckGodownExist(Godown.Godown_Name));

        }

        internal bool SavingValidation_Godown(GodowninfoforOpenAccount item)
        {
            return (item.Godown_Name != "" && item.Godown_Name != null && !CheckGodownExist(item.Godown_Name));
        }
        #endregion
        #region Stock
        internal System.Windows.Media.Brush StockValidation(StockInfo Stockinfo, out string toolTip)
        {
            if (Stockinfo.ProductName != null && Stockinfo.ProductName != string.Empty)
            {
                if (Stockinfo.ClosingRate != "0" || Stockinfo.OpeningStockNumber != "0")
                {
                    if (CheckStockExists(Stockinfo.ProductName) == -1)
                    {
                        if (CheckProductExist(Stockinfo.ProductName))
                        {
                            if (functionCheckUnitId(Stockinfo.Unit) != -1)
                            {
                                if (bool.TryParse(Stockinfo.OpeningStock, out isT))
                                {
                                    if (bool.TryParse(Stockinfo.OpeningStock, out isT))
                                    {
                                        if (Decimal.TryParse(Stockinfo.OpeningStockNumber, out isD))
                                        {
                                            if (Convert.ToDecimal(Stockinfo.OpeningStockNumber) >= 0)
                                            {
                                                if (Decimal.TryParse(Stockinfo.ClosingRate, out isD))
                                                {
                                                    if (Convert.ToDecimal(Stockinfo.ClosingRate) >= 0)
                                                    {
                                                        toolTip = "Stock";
                                                        return System.Windows.Media.Brushes.LightGreen;
                                                    }
                                                    else
                                                    {
                                                        toolTip = "Closing Rate Places not contain negative values.";
                                                        return System.Windows.Media.Brushes.LightGray;
                                                    }
                                                }
                                                else
                                                {
                                                    toolTip = "Closing rate not matching, it contain only numeric values.";
                                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                                }
                                            }
                                            else
                                            {
                                                toolTip = "Opening Stock Number Places not contain negative values.";
                                                return System.Windows.Media.Brushes.LightGray;
                                            }
                                        }
                                        else
                                        {
                                            toolTip = "Opening stock number not matching, it contain only numeric values.";
                                            return System.Windows.Media.Brushes.LightSteelBlue;
                                        }
                                    }
                                    else
                                    {
                                        toolTip = "Opening stock type not matching, it contain only True/False.";
                                        return System.Windows.Media.Brushes.LightSteelBlue;
                                    }
                                }
                                else
                                {
                                    toolTip = "Opening stock type not matching, it contain only True/False.";
                                    return System.Windows.Media.Brushes.LightSteelBlue;
                                }
                            }
                            else
                            {
                                toolTip = "Product Unit does not exist, Add unit first.";
                                return System.Windows.Media.Brushes.Orange;
                            }
                        }
                        else
                        {
                            toolTip = "Product does not exist, Add product first.";
                            return System.Windows.Media.Brushes.Orange;
                        }
                    }
                    else
                    {
                        toolTip = "Stock Exist.";
                        return System.Windows.Media.Brushes.LightCoral;
                    }
                }
                else
                {
                    toolTip = "Opening Stock And Rate are zero";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Product name is empty";
                return System.Windows.Media.Brushes.Red;
            }
        }

        internal bool StockValidation(StockInfo Stock)
        {
            return (CheckStockExists(Stock.ProductName) == -1 && CheckProductExist(Stock.ProductName) && (Stock.OpeningStockNumber != "0" || Stock.ClosingRate != "0") && Stock.ProductName != null && Stock.ProductName != string.Empty && functionCheckUnitId(Stock.Unit) != -1 && bool.TryParse(Stock.OpeningStock, out isT) && Decimal.TryParse(Stock.OpeningStockNumber, out isD)
&& decimal.TryParse(Stock.ClosingRate, out isD));
        }

        internal bool SavingValidation_Stock(StockforOpenAccount infoProduct)
        {
            return (infoProduct.ProductId != -1 && (infoProduct.OpeningStockNumber != 0 || infoProduct.ClosingRate != 0));
        }
        #endregion
        #region Currency

        internal System.Windows.Media.Brush CurrencyValidation(currencyInfo currencyinfo, out string toolTip)
        {
            if (currencyinfo.CurrencyName != null && currencyinfo.CurrencyName != string.Empty)
            {
                if (currencyinfo.CurrencySymbol != null && currencyinfo.CurrencySymbol != string.Empty)
                {
                    if (currencyinfo.NoOfDecimalPlaces != null)//|| currencyinfo.NoOfDecimalPlaces == "0")
                    {
                        if (!CheckCurrencyExist(currencyinfo.CurrencyName, currencyinfo.CurrencySymbol))
                        {
                            if (int.TryParse(currencyinfo.NoOfDecimalPlaces, out isI))
                            {
                                if (Convert.ToDecimal(currencyinfo.NoOfDecimalPlaces) >= 0)
                                {
                                    toolTip = "Currency";
                                    return System.Windows.Media.Brushes.LightGreen;
                                }
                                else
                                {
                                    toolTip = "NoOfDecimalPlaces not contain negative values.";
                                    return System.Windows.Media.Brushes.LightGray;
                                }
                            }
                            else
                            {
                                toolTip = "No of decimal places not matcing, it contain only numeric values.";
                                return System.Windows.Media.Brushes.LightSteelBlue;
                            }
                        }
                        else
                        {
                            toolTip = "Currency is exists.";
                            return System.Windows.Media.Brushes.LightCoral;
                        }
                    }
                    else
                    {
                        toolTip = "No of decimal places is empty.";
                        return System.Windows.Media.Brushes.Red;
                    }
                }
                else
                {
                    toolTip = "Currency symbol is empty.";
                    return System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                toolTip = "Currency name is empty.";
                return System.Windows.Media.Brushes.Red;
            }
        }

        internal bool SavingValidation_Currency(CurrencyInfoforOpenAccount item)
        {
            return (item.CurrencyName != "" && item.CurrencySymbol != "" && item.CurrencyName != null && item.CurrencySymbol != null && !CheckCurrencyExist(item.CurrencyName, item.CurrencySymbol));
        }

        internal bool CurrencyValidation(currencyInfo Currency)
        {
            return (Currency.CurrencyName != "" && Currency.CurrencySymbol != "" && Currency.CurrencyName != null && Currency.CurrencySymbol != null && !CheckCurrencyExist(Currency.CurrencyName, Currency.CurrencySymbol) &&
            Currency.NoOfDecimalPlaces != null && int.TryParse(Currency.NoOfDecimalPlaces, out isI) && Convert.ToDecimal(Currency.NoOfDecimalPlaces) >= 0);
        }
        #endregion
        #region Batch
        internal System.Windows.Media.Brush BatchValidation(batchesInfo batchinfo, out string toolTip)
        {
            DateTime ExactDate;
            if (batchinfo.BatchName != null && batchinfo.BatchName != string.Empty)
            {
                if (batchinfo.ProductName != null && batchinfo.ProductName != string.Empty)
                {
                    if (!CheckBatchExist(batchinfo.ProductName, batchinfo.BatchName))
                    {
                        if (DateTime.TryParse(batchinfo.MfgDate, out ExactDate))
                        {
                            if (DateTime.TryParse(batchinfo.ExpiryDate, out ExactDate))
                            {
                                toolTip = "Batch";
                                return System.Windows.Media.Brushes.LightGreen;
                            }
                            else
                            {
                                toolTip = "Expiry date is not matcing.";
                                return System.Windows.Media.Brushes.LightSteelBlue;
                            }
                        }
                        else
                        {
                            toolTip = "MFG date is not matching.";
                            return System.Windows.Media.Brushes.LightSteelBlue;
                        }
                    }
                    {
                        toolTip = "Batch name exists.";
                        return System.Windows.Media.Brushes.LightCoral;
                    }
                }
                {
                    toolTip = "Product name is empty";
                    return System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                toolTip = "Batch name is empty";
                return System.Windows.Media.Brushes.Red;
            }

        }
        internal bool BatchValidation(batchesInfo batchinfo)
        {
            return (batchinfo.ProductName != string.Empty && !CheckBatchExist(batchinfo.ProductName, batchinfo.BatchName) && batchinfo.ProductName != null && batchinfo.BatchName != null && batchinfo.BatchName != "");
        }
        #endregion
        #region VoucherType

        internal System.Windows.Media.Brush VoucherTypeValidation(voucherTypesInfo vouchertype, out string toolTip)
        {
            if (vouchertype.name != null && vouchertype.name != string.Empty)
            {
                if (!CheckVoucherTypeExist(vouchertype.name))
                {
                    if (vouchertype.typeOfVoucher != null && vouchertype.typeOfVoucher != string.Empty)
                    {
                        if (vouchertype.methodOfVoucherNumbering != null && vouchertype.methodOfVoucherNumbering != string.Empty)
                        {
                            if (vouchertype.dotMatrixPrintFormat != null && vouchertype.dotMatrixPrintFormat != string.Empty)
                            {
                                if (CheckTypeofVoucherExists(vouchertype.typeOfVoucher))
                                {
                                    if (CheckTypeofVoucherExists(vouchertype.dotMatrixPrintFormat))
                                    {
                                        if (bool.TryParse(vouchertype.active, out isT))
                                        {
                                            toolTip = "Voucher type";
                                            return System.Windows.Media.Brushes.LightGreen;
                                        }
                                        else
                                        {
                                            toolTip = "Voucher Active type not matching, it contain only True/False.";
                                            return System.Windows.Media.Brushes.LightSteelBlue;
                                        }
                                    }
                                    else
                                    {
                                        toolTip = "Sorry!, Type of printing format not match with OpenAccount.";
                                        return System.Windows.Media.Brushes.Orange;
                                    }
                                }
                                else
                                {
                                    toolTip = "Sorry!, Type of voucher not match with OpenAccount";
                                    return System.Windows.Media.Brushes.Orange;
                                }
                            }
                            else
                            {
                                toolTip = "Dot matrix print format";
                                return System.Windows.Media.Brushes.Red;
                            }
                        }
                        else
                        {
                            toolTip = "Method of number is empty";
                            return System.Windows.Media.Brushes.Red;
                        }
                    }
                    else
                    {
                        toolTip = "Type of Voucher is empty";
                        return System.Windows.Media.Brushes.Red;
                    }
                }
                else
                {
                    toolTip = "Voucher type name exist.";
                    return System.Windows.Media.Brushes.LightCoral;
                }
            }
            else
            {
                toolTip = "Voucher type name is empty";
                return System.Windows.Media.Brushes.Red;
            }

        }
        internal bool VoucherTypeValidation(voucherTypesInfo vouchertype)
        {
            return (vouchertype.name != string.Empty && vouchertype.name != null &&
            vouchertype.methodOfVoucherNumbering != string.Empty && vouchertype.methodOfVoucherNumbering != null &&
            vouchertype.typeOfVoucher != string.Empty && vouchertype.typeOfVoucher != null &&
            vouchertype.dotMatrixPrintFormat != string.Empty && vouchertype.dotMatrixPrintFormat != null &&
            !CheckVoucherTypeExist(vouchertype.name) && CheckTypeofVoucherExists(vouchertype.typeOfVoucher) && CheckTypeofVoucherExists(vouchertype.dotMatrixPrintFormat)
            && bool.TryParse(vouchertype.active, out isT));
        }

        internal bool SavingValidation_VoucherType(VoucherTypeinfoforOpenAccount infoVoucherType)
        {
            return (infoVoucherType.name != string.Empty && infoVoucherType.name != null && infoVoucherType.methodOfVoucherNumbering != string.Empty && infoVoucherType.methodOfVoucherNumbering != null && infoVoucherType.typeOfVoucher != string.Empty && infoVoucherType.typeOfVoucher != null && infoVoucherType.dotMatrixPrintFormat != string.Empty && infoVoucherType.dotMatrixPrintFormat != null && !CheckVoucherTypeExist(infoVoucherType.name) && CheckTypeofVoucherExists(infoVoucherType.typeOfVoucher) && CheckTypeofVoucherExists(infoVoucherType.dotMatrixPrintFormat));
        }
        #endregion
        #region Salesman
        internal System.Windows.Media.Brush SalesmanValidation(EmployeeInfo infoemployee, out string toolTip)
        {
            if (infoemployee.name != null && infoemployee.name != string.Empty)
            {
                if (infoemployee.employeeCode != null && infoemployee.employeeCode != string.Empty)
                {
                    if (!CheckSalesmanExist(infoemployee.employeeCode, infoemployee.name))
                    {
                        if (bool.TryParse(infoemployee.isActive, out isT))
                        {
                            toolTip = "Salesman";
                            return System.Windows.Media.Brushes.LightGreen;
                        }
                        else
                        {
                            toolTip = "Active type not matching, it contain only True/False.";
                            return System.Windows.Media.Brushes.LightSteelBlue;
                        }
                    }
                    else
                    {
                        toolTip = "Salesman name exist.";
                        return System.Windows.Media.Brushes.Orange;
                    }
                }
                else
                {
                    toolTip = "Salesman Code is empty.";
                    return System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                toolTip = "Salesman name is empty.";
                return System.Windows.Media.Brushes.Red;
            }

        }
        #endregion
    }
}
