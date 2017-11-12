using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AccountSkate
{
    class CommonSource
    {
        public MasterValidations mastervalidation = new MasterValidations();
        public Checkings checking = new Checkings();
        public List<pricingLevelInfo> lstPriceLevel = new List<pricingLevelInfo>();
        public List<CustomerSupplierInfo> lstCustomerSupplierInfo = new List<CustomerSupplierInfo>();
        public List<accountGroupInfo> lstAccountGroup = new List<accountGroupInfo>();
        public List<LedgerInfo> lstLedgerInfo = new List<LedgerInfo>();
        public List<productGroupInfo> lstproductgroupinfo = new List<productGroupInfo>();
        public List<unitsInfo> lstuniinfo = new List<unitsInfo>();
        public List<godownInfo> lstgodowninfo = new List<godownInfo>();
        public List<productInfo> lstproductinfo = new List<productInfo>();
        public List<StockInfo> lstStockInfo = new List<StockInfo>();
        public List<batchesInfo> lstbatches = new List<batchesInfo>();
        public List<currencyInfo> lstcurrencyinfo = new List<currencyInfo>();
        public List<voucherTypesInfo> lstvouchertypinfo = new List<voucherTypesInfo>();
        public List<EmployeeInfo> lstemplyeeinfo = new List<EmployeeInfo>();

        public void Clear()
        {
            lstAccountGroup.Clear();
            lstbatches.Clear();
            lstcurrencyinfo.Clear();
            lstCustomerSupplierInfo.Clear();
            lstemplyeeinfo.Clear();
            lstgodowninfo.Clear();
            lstLedgerInfo.Clear();
            lstPriceLevel.Clear();
            lstproductgroupinfo.Clear();
            lstproductinfo.Clear();
            lstStockInfo.Clear();
            lstuniinfo.Clear();
            lstvouchertypinfo.Clear();
        }
        /// <summary>
        /// Function to get printer
        /// </summary>
        /// <param name="strPrinter"></param>
        /// <returns></returns>
        public string GetPrinter(string strPrinter)
        {
            string strFormat = strPrinter;
            try
            {
                ArrayList alPrinters = new ArrayList();
                alPrinters.Add("Advance Payment");
                alPrinters.Add("Monthly Salary Voucher");
                alPrinters.Add("Daily Salary Voucher");
                alPrinters.Add("Stock Journal");
                alPrinters.Add("Memorandum");
                alPrinters.Add("Receipt Note");
                alPrinters.Add("Reversing Journal");
                alPrinters.Add("Memorandum");
                foreach (string strPrint in alPrinters)
                {
                    if (strPrint == strPrinter)
                    {
                        strFormat = "Not Applicable";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return strFormat;
        }
        /// <summary>
        /// Function to get nature
        /// </summary>
        /// <param name="StrGroupName"></param>
        /// <returns></returns>
        public string GetNature(string StrGroupName)
        {
            string strNature = "";
            try
            {
                String[] Liabilities = { "capital account", "loans (liability)", "current liabilities", "branch /divisions", "suspense a/c", "reservers &surplus", "bank od a/c", "secured loans", "unsecured loans", "duties& taxes", "provisions", "sundry creditors" };
                String[] Assets = { "fixed assets", "investments", "current assets", "misc.expenses (asset)", "stock-in-hand", "deposits(assets)", "loans & advances(asset)", "sundry debtors", "cash-in hand", "bank account" };
                String[] Income = { "sales account", "direct income", "indirect income" };
                String[] Expenses = { "purchase account", "direct expenses", "indirect expenses" };
                foreach (string item in Liabilities)
                {
                    if (item == StrGroupName)
                    {
                        strNature = "Liabilities";
                    }
                }
                foreach (string item in Assets)
                {
                    if (item == StrGroupName)
                    {
                        strNature = "Assets";
                    }
                }
                foreach (string item in Income)
                {
                    if (item == StrGroupName)
                    {
                        strNature = "Income";
                    }
                }
                foreach (string item in Expenses)
                {
                    if (item == StrGroupName)
                    {
                        strNature = "Expenses";
                    }
                }
                if (strNature == "")
                {
                    strNature = checking.functionGetNature(StrGroupName);
                }
            }
            catch (Exception)
            {
            }
            return strNature;
        }
        /// <summary>
        /// Function to CheckName
        /// </summary>
        /// <param name="StrGroupName"></param>
        /// <returns></returns>
        public string CheckName(string p)
        {
            switch (p)
            {
                case ("Bank Accounts"):
                    return "Bank Account";
                case ("Cash-in-hand"):
                    return "Cash-in Hand";
                case ("Branch / Divisions"):
                    return "Branch /Divisions";
                case ("Deposits (Asset)"):
                    return "Deposits(Assets)";
                case ("Direct Incomes"):
                    return "Direct Income";
                case ("Duties & Taxes"):
                    return "Duties& Taxes";
                case ("Indirect Incomes"):
                    return "Indirect Income";
                case ("Loans & Advances (Asset)"):
                    return "Loans & Advances(Asset)";
                case ("Misc. Expenses (ASSET)"):
                    return "Misc.Expenses (ASSET)";
                case ("Purchase Accounts"):
                    return "Purchase Account";
                case ("Reserves & Surplus"):
                    return "Reservers &Surplus";
                case ("Sales Accounts"):
                    return "Sales Account";
                case ("Profit & Loss A/c"):
                    return "Profit And Loss";
                case ("Sales"):
                    return "Sales Invoice";
                case ("Payroll"):
                    return "";
                case ("Rejections In"):
                    return "Rejection In";
                case ("Rejections Out"):
                    return "Rejection Out";
                case ("Journal"):
                    return "Journal Voucher";
                case ("Receipt"):
                    return "Receipt Voucher";
                case ("Payment"):
                    return "Payment Voucher";
                case ("Contra"):
                    return "Contra Voucher";
                case ("Purchase"):
                    return "Purchase Invoice";
                case (" Primary"):
                    return "Primary";
                default:
                    return p;
            }
        }
    }
}
