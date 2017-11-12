using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.Data;

namespace AccountSkate
{
    /// <summary>
    /// Interaction logic for Skate.xaml
    /// </summary>
    public partial class Skate : Window
    {
        Propertiess propertiess = new Propertiess();
        public Skate()
        {
            InitializeComponent();
            propertiess.PropertyChanged += new PropertyChangedEventHandler(CategoryInfo_PropertyChanged);
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);

            bgSave.DoWork += new DoWorkEventHandler(bgSave_DoWork);
            bgSave.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgSave_RunWorkerCompleted);
        }

        Tally spTally;
        ExcellSp spExcell;
        OpenAccountExport spOpenAccountExport;
        GenerateExcell spGenerateExcel;
        OpenAccount openAccount = new OpenAccount();
        void bgSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (((spTally != null) ? spTally.CheckTallyConnection() : false) || ((spExcell != null) ? spExcell.CheckExcellConnection() : false))
            {
                if (!bg.IsBusy)
                {
                    bg.RunWorkerAsync(Header);
                }
            }
            else
            {
                ShowMessage(Process);
                grdMain.Children.OfType<DataGrid>().ToList().ForEach(b => grdMain.Children.Remove(b));
                pbProgressBar.IsIndeterminate = false;
                mainmaenu.IsEnabled = true;
                grdLeftMenu.IsEnabled = true;
                grdMain.IsEnabled = true;
                Messages = null;
            }
        }
        Microsoft.Office.Interop.Excel.Range fontRange;
        void bgSave_DoWork(object sender, DoWorkEventArgs e)
        {
            DataGrid datagrid = e.Argument as DataGrid;

            mainmaenu.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
              new Action(
              delegate()
              {
                  mainmaenu.IsEnabled = false;
                  grdLeftMenu.IsEnabled = false;
                  grdMain.IsEnabled = false;
              }));
            if (Process == "btnImportFromExcel" || Process == "btnImportFromTally")
            {
                switch (Header)
                {
                    case ("Pricing Level"):
                        #region PriceLevel
                        List<PriceLevelInfofroOpenAccount> lstObjPriceLevel = new List<PriceLevelInfofroOpenAccount>();
                        foreach (pricingLevelInfo PriceLevel in datagrid.dgDatas.Items)
                        {
                            if (openAccount.PriceLevelValidation(PriceLevel))
                            {
                                PriceLevelInfofroOpenAccount obj = new PriceLevelInfofroOpenAccount();
                                obj.PricingLevelName = PriceLevel.PricingLevelName;
                                obj.Narration = PriceLevel.Narration;
                                lstObjPriceLevel.Add(obj);
                            }
                        }

                        if (lstObjPriceLevel.Count != 0)
                        {
                            openAccount.AddPriceLevel(lstObjPriceLevel, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Pricing Level names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Customer"):
                        #region Customer
                        List<AccountLedgerInfoforOpenAccount> lstObj = new List<AccountLedgerInfoforOpenAccount>();
                        foreach (CustomerSupplierInfo Customer in datagrid.dgDatas.Items)
                        {
                            if (openAccount.CustomerValidations(Customer))
                            {
                                AccountLedgerInfoforOpenAccount obj = new AccountLedgerInfoforOpenAccount();
                                obj.AccountGroupId = 26;
                                obj.Account_Number = Customer.Account_Number;
                                obj.Address = Customer.Address;
                                obj.Area_ID = openAccount.functionCheckAreaIdId(Customer.Area);
                                obj.Bill_By_Bill = Convert.ToBoolean(Customer.Bill_By_Bill);
                                obj.Brach_Code = Customer.Brach_Code;
                                obj.Branch_Name = Customer.Branch_Name;
                                obj.Credit_Limit = Convert.ToDecimal(Customer.Credit_Limit);
                                obj.Credit_Period = Convert.ToDecimal(Customer.Credit_Period);
                                obj.CrorDr = Customer.CrorDr;
                                obj.CST = Customer.CST;
                                obj.Email = Customer.Email;
                                obj.Mailing_Name = Customer.Mailing_Name;
                                obj.Mobile = Customer.Mobile;
                                obj.Name = Customer.Name;
                                obj.Narration = Customer.Narration;
                                obj.Opening_Balance = Convert.ToDecimal(Customer.Opening_Balance);
                                obj.PAN = Customer.PAN;
                                obj.Phone = Customer.Phone;
                                obj.Pricing_Level_ID = openAccount.functionPricingLevelId(Customer.Pricing_Level);
                                obj.Route_ID = openAccount.functionRoutId(Customer.Route, Customer.Area);
                                obj.TIN = Customer.TIN;
                                lstObj.Add(obj);
                            }

                        }
                        if (lstObj.Count != 0)
                        {
                            openAccount.AddAccountLedger(lstObj, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Customers names existed.!";
                        }
                        #endregion
                        break;
                    case ("Supplier"):
                        #region Supplier
                        List<AccountLedgerInfoforOpenAccount> lstObj1 = new List<AccountLedgerInfoforOpenAccount>();
                        foreach (CustomerSupplierInfo Supplier in datagrid.dgDatas.Items)
                        {
                            if (openAccount.SupplierValidations(Supplier))
                            {
                                AccountLedgerInfoforOpenAccount obj = new AccountLedgerInfoforOpenAccount();
                                obj.AccountGroupId = 22;
                                obj.Account_Number = Supplier.Account_Number;
                                obj.Address = Supplier.Address;
                                obj.Area_ID = openAccount.functionCheckAreaIdId(Supplier.Area);
                                obj.Bill_By_Bill = Convert.ToBoolean(Supplier.Bill_By_Bill);
                                obj.Brach_Code = Supplier.Brach_Code;
                                obj.Branch_Name = Supplier.Branch_Name;
                                obj.Credit_Limit = Convert.ToDecimal(Supplier.Credit_Limit);
                                obj.Credit_Period = Convert.ToDecimal(Supplier.Credit_Period);
                                obj.CrorDr = Supplier.CrorDr;
                                obj.CST = Supplier.CST;
                                obj.Email = Supplier.Email;
                                obj.Mailing_Name = Supplier.Mailing_Name;
                                obj.Mobile = Supplier.Mobile;
                                obj.Name = Supplier.Name;
                                obj.Narration = Supplier.Narration;
                                obj.Opening_Balance = Convert.ToDecimal(Supplier.Opening_Balance);
                                obj.PAN = Supplier.PAN;
                                obj.Phone = Supplier.Phone;
                                obj.Pricing_Level_ID = openAccount.functionPricingLevelId(Supplier.Pricing_Level);
                                obj.Route_ID = openAccount.functionRoutId(Supplier.Route, Supplier.Area);
                                obj.TIN = Supplier.TIN;
                                lstObj1.Add(obj);
                            }

                        }
                        if (lstObj1.Count != 0)
                        {
                            openAccount.AddAccountLedger(lstObj1, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Suppliers names existed.!";
                        }
                        #endregion
                        break;
                    case ("Account Ledgers"):
                        #region AccountLedger
                        List<AccountLedgerInfoforOpenAccount> lstObj2 = new List<AccountLedgerInfoforOpenAccount>();
                        foreach (LedgerInfo ledger in datagrid.dgDatas.Items)
                        {
                            if (openAccount.LedgerValidations(ledger))
                            {
                                decimal AccountGroupId = openAccount.functionCheckAccountGroupUnderId(ledger.Account_Group);
                                if (AccountGroupId != -1)
                                {
                                    AccountLedgerInfoforOpenAccount obj = new AccountLedgerInfoforOpenAccount();
                                    obj.AccountGroupId = AccountGroupId;
                                    obj.Account_Number = "";
                                    obj.Address = "";
                                    obj.Area_ID = 1;
                                    obj.Bill_By_Bill = false;
                                    obj.Brach_Code = "";
                                    obj.Branch_Name = "";
                                    obj.Credit_Limit = 0;
                                    obj.Credit_Period = 0;
                                    obj.CrorDr = ledger.CrorDr;
                                    obj.CST = "";
                                    obj.Email = "";
                                    obj.Mailing_Name = "";
                                    obj.Mobile = "";
                                    obj.Name = ledger.Name;
                                    obj.Narration = ledger.Narration;
                                    obj.Opening_Balance = Convert.ToDecimal(ledger.Opening_Balance);
                                    obj.PAN = "";
                                    obj.Phone = "";
                                    obj.Pricing_Level_ID = 1;
                                    obj.Route_ID = 1;
                                    obj.TIN = "";
                                    lstObj2.Add(obj);
                                }
                            }

                        }
                        if (lstObj2.Count != 0)
                        {
                            openAccount.AddAccountLedger(lstObj2, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Account Ledger names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Account Groups"):
                        #region AccountGroup
                        List<AccountGroupInfoforOpenAccount> lstObj3 = new List<AccountGroupInfoforOpenAccount>();
                        foreach (accountGroupInfo AccountGroup in datagrid.dgDatas.Items)
                        {
                            if (openAccount.AccountGroupValidation(AccountGroup))
                            {
                                AccountGroupInfoforOpenAccount infoaccountGroup = new AccountGroupInfoforOpenAccount();
                                infoaccountGroup.Name = AccountGroup.Name;
                                infoaccountGroup.Affect_Gross_Profit = Convert.ToBoolean(AccountGroup.Affect_Gross_Profit);
                                infoaccountGroup.Narration = AccountGroup.Narration;
                                infoaccountGroup.Under = openAccount.functionCheckAccountGroupUnderId(AccountGroup.Under);
                                infoaccountGroup.Nature = AccountGroup.Nature;
                                lstObj3.Add(infoaccountGroup);
                            }

                        }
                        if (lstObj3.Count != 0)
                        {
                            openAccount.AddAccountGroup(lstObj3, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Account Group names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Units"):
                        #region Unit
                        List<UnitinfoforOpenAccount> lstObjUnit = new List<UnitinfoforOpenAccount>();
                        foreach (unitsInfo Unit in datagrid.dgDatas.Items)
                        {
                            if (openAccount.UnitValidation(Unit))
                            {
                                UnitinfoforOpenAccount infoUnit = new UnitinfoforOpenAccount();
                                infoUnit.FormalName = Unit.FormalName;
                                infoUnit.Narration = Unit.Narration;
                                infoUnit.Narration = Unit.Narration;
                                infoUnit.noOfDecimalPlaces = Convert.ToDecimal(Unit.noOfDecimalPlaces);
                                infoUnit.UnitName = Unit.UnitName;
                                lstObjUnit.Add(infoUnit);
                            }

                        }
                        if (lstObjUnit.Count != 0)
                        {
                            openAccount.AddUnit(lstObjUnit, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Units are existed.!";
                        }
                        #endregion
                        break;
                    case ("Product Groups"):
                        #region ProductGroup
                        List<ProductGroupinfoforOpenAccount> lstObjProductGroup = new List<ProductGroupinfoforOpenAccount>();
                        foreach (productGroupInfo ProductGroup in datagrid.dgDatas.Items)
                        {
                            if (openAccount.ProductGroupValidation(ProductGroup))
                            {
                                ProductGroupinfoforOpenAccount obj = new ProductGroupinfoforOpenAccount();
                                obj.GroupName = ProductGroup.GroupName;
                                obj.GroupUnder = openAccount.functionCheckProductGroupUnderId(ProductGroup.Group_Under);
                                obj.Narration = ProductGroup.Narration;
                                lstObjProductGroup.Add(obj);
                            }

                        }
                        if (lstObjProductGroup.Count != 0)
                        {
                            openAccount.AddProductGroup(lstObjProductGroup, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Product Group names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Godowns"):
                        #region Godowns
                        List<GodowninfoforOpenAccount> lstObjGodowns = new List<GodowninfoforOpenAccount>();
                        foreach (godownInfo Godown in datagrid.dgDatas.Items)
                        {
                            if (openAccount.GodownValidation(Godown))
                            {
                                GodowninfoforOpenAccount obj = new GodowninfoforOpenAccount();
                                obj.Godown_Name = Godown.Godown_Name;
                                obj.Narration = Godown.Narration;
                                lstObjGodowns.Add(obj);
                            }

                        }
                        if (lstObjGodowns.Count != 0)
                        {
                            openAccount.AddGodwn(lstObjGodowns, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Godown names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Products"):
                        #region Products
                        List<ProductInfofroOpenAccount> lstObj4 = new List<ProductInfofroOpenAccount>();
                        foreach (productInfo Product in datagrid.dgDatas.Items)
                        {
                            if (openAccount.ProductValidation(Product))
                            {
                                ProductInfofroOpenAccount productinfo = new ProductInfofroOpenAccount();
                                productinfo.AllowBatch = Convert.ToBoolean(Product.AllowBatch);
                                productinfo.ConversionRate = Convert.ToDecimal(Product.ConversionRate);
                                productinfo.GodownId = openAccount.functionCheckGodownId(Product.DefaultGodown);
                                productinfo.Group = openAccount.functionCheckProductGroupUnderId(Product.Group);
                                productinfo.MaximumStock = Convert.ToDecimal(Product.MaximumStock);
                                productinfo.MinimumStock = Convert.ToDecimal(Product.MinimumStock);
                                productinfo.MRP = Convert.ToDecimal(Product.MRP);
                                productinfo.MultipleUnit = Convert.ToBoolean(Product.MultipleUnit);
                                productinfo.Narration = Product.Narration;
                                productinfo.OpeningStock = Convert.ToBoolean(Product.OpeningStock);
                                productinfo.ProductCode = Product.ProductCode;
                                productinfo.ProductName = Product.ProductName;
                                productinfo.PurchaseRate = Convert.ToDecimal(Product.PurchaseRate);
                                productinfo.RackId = openAccount.functionCheckRackId(Product.Rack);
                                productinfo.ReorderLevel = Convert.ToDecimal(Product.ReorderLevel);
                                productinfo.SalesRate = Convert.ToDecimal(Product.SalesRate);
                                productinfo.Size = Convert.ToDecimal(Product.Size);
                                productinfo.TaxApplicableOn = Product.TaxApplicableOn;
                                productinfo.TaxId = openAccount.functionCheckTaxId(Product.Tax);
                                productinfo.UnitId = openAccount.functionCheckUnitId(Product.Unit);
                                lstObj4.Add(productinfo);
                            }
                        }
                        if (lstObj4.Count != 0)
                        {
                            openAccount.AddProduct(lstObj4, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Product names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Stock"):
                        #region Stock
                        List<StockforOpenAccount> lstObjStock = new List<StockforOpenAccount>();
                        foreach (StockInfo Stock in datagrid.dgDatas.Items)
                        {
                            if (openAccount.StockValidation(Stock))
                            {
                                decimal decProductId = openAccount.functionProductId(Stock.ProductName);
                                decimal decBatchId = openAccount.functionBatchId(Stock.ProductName);
                                if (decProductId != -1 && decBatchId != -1)
                                {
                                    StockforOpenAccount productinfo = new StockforOpenAccount();
                                    productinfo.OpeningStock = Convert.ToBoolean(Stock.OpeningStock);
                                    productinfo.OpeningStockNumber = Convert.ToDecimal(Stock.OpeningStockNumber);
                                    productinfo.ProductId = decProductId;
                                    productinfo.BatchId = decBatchId;
                                    productinfo.UnitID = openAccount.functionCheckUnitId(Stock.Unit);
                                    productinfo.ClosingRate = Convert.ToDecimal(Stock.ClosingRate);
                                    lstObjStock.Add(productinfo);
                                }
                            }
                        }
                        if (lstObjStock.Count != 0)
                        {
                            openAccount.AddStock(lstObjStock, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Stock existed.!";
                        }
                        #endregion
                        break;
                    case ("Currency"):
                        #region Currency
                        List<CurrencyInfoforOpenAccount> lstObjCurrency = new List<CurrencyInfoforOpenAccount>();
                        foreach (currencyInfo Currency in datagrid.dgDatas.Items)
                        {
                            if (openAccount.CurrencyValidation(Currency))
                            {
                                CurrencyInfoforOpenAccount currencyinfo = new CurrencyInfoforOpenAccount();
                                currencyinfo.CurrencyName = Currency.CurrencyName;
                                currencyinfo.CurrencySymbol = Currency.CurrencySymbol;
                                currencyinfo.Narration = Currency.Narration;
                                currencyinfo.NoOfDecimalPlaces = Convert.ToInt32(Currency.NoOfDecimalPlaces);
                                currencyinfo.SubUnitName = Currency.SubUnitName;
                                lstObjCurrency.Add(currencyinfo);
                            }
                        }
                        if (lstObjCurrency.Count != 0)
                        {
                            openAccount.AddCurrency(lstObjCurrency, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Currency names are existed.!";
                        }
                        #endregion
                        break;
                    case ("Voucher Type"):
                        #region VoucherType
                        List<VoucherTypeinfoforOpenAccount> lstObjVoucherType = new List<VoucherTypeinfoforOpenAccount>();
                        foreach (voucherTypesInfo Voucher in datagrid.dgDatas.Items)
                        {
                            if (openAccount.VoucherTypeValidation(Voucher))
                            {
                                VoucherTypeinfoforOpenAccount voucherinfo = new VoucherTypeinfoforOpenAccount();
                                voucherinfo.name = Voucher.name;
                                voucherinfo.active = Convert.ToBoolean(Voucher.active);
                                voucherinfo.declaration = Voucher.declaration;
                                voucherinfo.dotMatrixPrintFormat = Voucher.dotMatrixPrintFormat;
                                voucherinfo.methodOfVoucherNumbering = Voucher.methodOfVoucherNumbering;
                                voucherinfo.narration = Voucher.narration;
                                voucherinfo.typeOfVoucher = Voucher.typeOfVoucher;
                                lstObjVoucherType.Add(voucherinfo);
                            }
                        }
                        if (lstObjVoucherType.Count != 0)
                        {
                            openAccount.AddVoucherType(lstObjVoucherType, ref Messages, datagrid.lblEventMessage);
                        }
                        else
                        {
                            Messages = "All Voucher Type names are existed.!";
                        }
                        #endregion
                        break;
                }
            }
            else if (Process == "btnExportExcel")
            {
                spGenerateExcel.GenerateExcellWithData(datagrid.dgDatas, saveDlg.FileName, fontRange);
            }
        }
        Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog();


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginUser loginuser = new LoginUser(propertiess);
            string ss = loginuser.txtUserName.Text;
            grdMain.Children.Add(loginuser);
        }
        public void CategoryInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "logIn_True")
            {
                mainmaenu.Visibility = System.Windows.Visibility.Visible;
                mainmaenu.btnPricingLevel.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnCustomer.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnSupplier.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnAccountGroups.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnAccountLedgers.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnProductGroups.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnUnits.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnGodowns.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnProducts.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnBatches.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnCurrency.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnVoucherType.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnSalesMan.Click += new RoutedEventHandler(btnClick);
                mainmaenu.btnStock.Click += new RoutedEventHandler(btnClick);
            }
        }
        BackgroundWorker bg = new BackgroundWorker();
        BackgroundWorker bgSave = new BackgroundWorker();
        object dtbl;
        string Header;
        string Process;
        private void ShowMessage(string Process)
        {
            switch (Process)
            {
                case ("btnImportFromTally"):
                    new wdoMessageBox("Source connection not established! Make sure you have open Tally to proceed.").ShowDialog();
                    break;
                case ("btnImportFromExcel"):
                    new wdoMessageBox("Source connection not established!").ShowDialog();
                    break;
            }

        }
        #region TopButtonCliks
        void btnClick(object sender, RoutedEventArgs e)
        {
            if (Process == "btnImportFromTally")
            {
                if (spTally == null || !spTally.CheckTallyConnection())
                    spTally = new Tally();
            }
            else if (Process == "btnImportFromExcel")
            {
                    spExcell = new ExcellSp();
            }
            else if (Process == "btnExportExcel")
            {
                if (spOpenAccountExport == null)
                    spOpenAccountExport = new OpenAccountExport();
                if (spGenerateExcel == null)
                    spGenerateExcel = new GenerateExcell();
            }
            else if (Process == "btnExcellFormat")
            {
                if (spGenerateExcel == null)
                    spGenerateExcel = new GenerateExcell();
            }
            if (((spTally != null) ? spTally.CheckTallyConnection() : false) || ((spExcell != null) ? spExcell.CheckExcellConnection() : false) || spOpenAccountExport != null || spGenerateExcel != null)
            {
                if (!bg.IsBusy)
                {
                    pbProgressBar.IsIndeterminate = true;
                    bg.RunWorkerAsync((sender as Button).ToolTip);
                }
            }
            else
            {
                ShowMessage(Process);
            }
        }
        #endregion
        DataGrid datagrid;
        System.Windows.Forms.FolderBrowserDialog myFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
        string Messages = null;
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            grdMain.Children.OfType<DataGrid>().ToList().ForEach(b => grdMain.Children.Remove(b));
            if (Process != "btnExcellFormat")
            {
                datagrid = new DataGrid(dtbl, Header, Messages);
                grdMain.Children.Add(datagrid);
            }
            pbProgressBar.IsIndeterminate = false;
            mainmaenu.IsEnabled = true;
            grdLeftMenu.IsEnabled = true;
            grdMain.IsEnabled = true;
            Messages = null;

        }
        System.Windows.Forms.FolderBrowserDialog folderbrowser = new System.Windows.Forms.FolderBrowserDialog();
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            mainmaenu.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
               new Action(
               delegate()
               {
                   mainmaenu.IsEnabled = false;
                   grdLeftMenu.IsEnabled = false;
                   grdMain.IsEnabled = false;
               }));

            Header = e.Argument.ToString();
            if (Messages == null)
                Messages = e.Argument.ToString();
            switch (e.Argument.ToString())
            {
                case ("Pricing Level"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetPriceLevel();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetPriceLevel();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.PricingLevelToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.PricingLevel(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Customer"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetCustomer();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetCustomer();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.CustomerExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Customer(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Supplier"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetSupplier();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetSupplier();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.SupplierExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Supplier(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Account Groups"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetAccountGroup();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetAccountGroup();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.AccountGroupExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.AccountGroup(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Account Ledgers"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetAccountLedger();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetAccountLedger();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.AccountLegderExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.AccountLeger(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Product Groups"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetProductGroup();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetProductGroup();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.ProductGroupExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.ProductGroup(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Units"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetUnit();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetUnit();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.UnitExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Unit(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Godowns"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetGodown();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetGodown();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.GodownExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Godown(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Products"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetProduct();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetProduct();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.ProductExportTOExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Product(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Batches"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetBatch();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetBatch();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.BatchExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Batch(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Currency"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetCurrency();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetCurrency();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.CurrencyExportToexcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Currency(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Voucher Type"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetVoucherType();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetVoucherType();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.VoucherTypeExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Vochertype(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("SalesMan"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetSalesMen();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetSalesMen();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.SalesManExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.SalesMan(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
                case ("Stock"):
                    switch (Process)
                    {
                        case ("btnImportFromTally"):
                            dtbl = spTally.GetStock();
                            break;
                        case ("btnImportFromExcel"):
                            dtbl = spExcell.GetStock();
                            break;
                        case ("btnExportExcel"):
                            dtbl = spOpenAccountExport.StockExportToExcel();
                            break;
                        case ("btnExcellFormat"):
                            if (GetPath())
                                spGenerateExcel.Stock(myFolderBrowserDialog.SelectedPath);
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        private bool GetPath()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Suported Excel is not installed.").ShowDialog()));
                return false;
            }
            else
            {
                Thread t = new Thread(() => myFolderBrowserDialog.ShowDialog());
                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
                return true;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (Process == "btnExportExcel")
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Suported Excel is not installed.").ShowDialog()));
                }
                else
                {
                    saveDlg.Filter = "Excel files (*.xls)|*.xls";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export Excel File To";
                    saveDlg.ShowDialog();
                }
            }
            List<DataGrid> dd = grdMain.Children.OfType<DataGrid>().ToList();
            if (dd.Count != 0)
            {
                if (!bgSave.IsBusy)
                {
                    pbProgressBar.IsIndeterminate = true;
                    bgSave.RunWorkerAsync(dd.Last());
                }
            }

        }

        private void grdMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("VD 51 : " + ex.Message).ShowDialog()));
            }
        }
        ImageBrush images;
        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            grdMain.Children.OfType<DataGrid>().ToList().ForEach(b => grdMain.Children.Remove(b));
            grdMain.Children.OfType<AboutUs>().ToList().ForEach(b => grdMain.Children.Remove(b));
            grdMain.Children.OfType<ContactUs>().ToList().ForEach(b => grdMain.Children.Remove(b));
            switch (Process)
            {
                case ("btnImportFromTally"):
                    btnImportFromTally.Content = null;
                    btnImportFromTally.Background = images;
                    break;
                case ("btnImportFromExcel"):
                    btnImportFromExcel.Content = null;
                    btnImportFromExcel.Background = images;
                    break;
                case ("btnExcellFormat"):
                    btnExcellFormat.Content = null;
                    btnExcellFormat.Background = images;
                    break;
                case ("btnExportExcel"):
                    btnExportExcel.Content = null;
                    btnExportExcel.Background = images;
                    break;
                case ("btnAboutUs"):
                    btnAboutUs.Content = null;
                    btnAboutUs.Background = images;
                    break;
                case ("btnContactUs"):
                    btnContactUs.Content = null;
                    btnContactUs.Background = images;
                    break;
                default:
                    break;
            }
            if ((sender as Button).Content == null)
            {
                images = (ImageBrush)(sender as Button).Background;
                (sender as Button).Background = Brushes.Goldenrod;
                Process = (sender as Button).Name;
            }
            switch ((sender as Button).Name)
            {
                case ("btnImportFromTally"):
                    (sender as Button).Content = "Import\n Tally.";
                    spExcell = null;
                    spGenerateExcel = null;
                    spOpenAccountExport = null;
                    break;
                case ("btnImportFromExcel"):
                    (sender as Button).Content = "Import\n Excel.";
                    spTally = null;
                    spGenerateExcel = null;
                    spOpenAccountExport = null;
                    break;
                case ("btnExcellFormat"):
                    (sender as Button).Content = "Generate\n Format";
                    spTally = null;
                    spExcell = null;
                    spOpenAccountExport = null;
                    break;
                case ("btnExportExcel"):
                    (sender as Button).Content = "Export\n Excel";
                    spTally = null;
                    spExcell = null;
                    break;
                case ("btnAboutUs"):
                    (sender as Button).Content = "About\n   Us";
                    spTally = null;
                    spExcell = null;
                    spGenerateExcel = null;
                    spOpenAccountExport = null;
                    if (grdMain.Children.OfType<AboutUs>().ToList().Count == 0)
                    {
                        AboutUs obj = new AboutUs();
                        grdMain.Children.Add(obj);
                    }
                    break;
                case ("btnContactUs"):
                    (sender as Button).Content = "Contact\n    Us";
                    spTally = null;
                    spExcell = null;
                    spGenerateExcel = null;
                    spOpenAccountExport = null;
                    if (grdMain.Children.OfType<ContactUs>().ToList().Count == 0)
                    {
                        ContactUs obj = new ContactUs();
                        grdMain.Children.Add(obj);
                    }
                    break;
                default:
                    break;
            }

        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            grdMain.Children.OfType<DataGrid>().ToList().ForEach(b => grdMain.Children.Remove(b));
            spGenerateExcel = null;
            spExcell = null;
            spOpenAccountExport = null;
            spTally = null;
            mainmaenu.Visibility = Visibility.Hidden;
            LoginUser loginuser = new LoginUser(propertiess);
            string ss = loginuser.txtUserName.Text;
            grdMain.Children.Add(loginuser);
        }


    }
}

