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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace AccountSkate
{
    /// <summary>
    /// Interaction logic for DataGrid.xaml
    /// </summary>
    public partial class DataGrid : UserControl
    {
        public DataGrid(object dtbl, string Header, string Message)
        {
            InitializeComponent();
            if (dtbl != null)
            {
                dgDatas.ToolTip = Header;
                lblEventMessage.Content = Message;
                lblBorder.Visibility = (Message != null) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

                switch (Header)
                {
                    case ("Pricing Level"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                            dgDatas.ItemsSource = ((dtbl as List<pricingLevelInfo>).Count != 0) ? (dtbl as List<pricingLevelInfo>) : null;
                        break;
                    case ("Customer"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<CustomerSupplierInfo>).Count != 0) ? (dtbl as List<CustomerSupplierInfo>) : null;
                        break;
                    case ("Supplier"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<CustomerSupplierInfo>).Count != 0) ? (dtbl as List<CustomerSupplierInfo>) : null;
                        break;
                    case ("Account Groups"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<accountGroupInfo>).Count != 0) ? (dtbl as List<accountGroupInfo>) : null;
                        break;
                    case ("Account Ledgers"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<LedgerInfo>).Count != 0) ? (dtbl as List<LedgerInfo>) : null;
                        break;
                    case ("Product Groups"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<productGroupInfo>).Count != 0) ? (dtbl as List<productGroupInfo>) : null;
                        break;
                    case ("Units"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<unitsInfo>).Count != 0) ? (dtbl as List<unitsInfo>) : null;
                        break;
                    case ("Godowns"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<godownInfo>).Count != 0) ? (dtbl as List<godownInfo>) : null;
                        break;
                    case ("Products"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<productInfo>).Count != 0) ? (dtbl as List<productInfo>) : null;
                        break;
                    case ("Batches"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<batchesInfo>).Count != 0) ? (dtbl as List<batchesInfo>) : null;
                        break;
                    case ("Currency"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<currencyInfo>).Count != 0) ? (dtbl as List<currencyInfo>) : null;
                        break;
                    case ("Voucher Type"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<voucherTypesInfo>).Count != 0) ? (dtbl as List<voucherTypesInfo>) : null;
                        break;
                    case ("SalesMan"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<EmployeeInfo>).Count != 0) ? (dtbl as List<EmployeeInfo>) : null;
                        break;
                    case ("Stock"):
                        if (dtbl is DataTable)
                            dgDatas.ItemsSource = ((dtbl as DataTable).Rows.Count != 0) ? (dtbl as DataTable).DefaultView : null;
                        else
                        dgDatas.ItemsSource = ((dtbl as List<StockInfo>).Count != 0) ? (dtbl as List<StockInfo>) : null;
                        break;
                    default:
                        break;
                }
            }
            txtNoRecords.Visibility = (dgDatas.ItemsSource == null) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        MasterValidations mastervalidation = new MasterValidations();
        pricingLevelInfo pricelevelinfo;
        CustomerSupplierInfo ledgerinfoforcustomer;
        accountGroupInfo accountgroupinfo;
        LedgerInfo accountledgerinfo;
        productGroupInfo productgroupinfo;
        unitsInfo unitinfo;
        godownInfo godowninfo;
        productInfo productinfo;
        StockInfo Stockinfo;
        batchesInfo batchinfo;
        currencyInfo currencyinfo;
        voucherTypesInfo vouchertype;
        EmployeeInfo employeeinfo;
        string toolTip;
        private void dgDatas_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            DataLoading(e);
        }

        private void DataLoading(DataGridRowEventArgs e)
        {
            switch (dgDatas.ToolTip.ToString())
            {
                case ("Pricing Level"):
                    pricelevelinfo = e.Row.Item as pricingLevelInfo;
                    if (pricelevelinfo != null)
                    {
                        e.Row.Background = mastervalidation.PriceLevelValidation(pricelevelinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Customer"):
                    ledgerinfoforcustomer = e.Row.Item as CustomerSupplierInfo;
                    if (ledgerinfoforcustomer != null)
                    {
                        e.Row.Background = mastervalidation.CustomerValidations(ledgerinfoforcustomer, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Supplier"):
                    ledgerinfoforcustomer = e.Row.Item as CustomerSupplierInfo;
                    if (ledgerinfoforcustomer != null)
                    {
                        e.Row.Background = mastervalidation.SupplierValidations(ledgerinfoforcustomer, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Account Groups"):
                    accountgroupinfo = e.Row.Item as accountGroupInfo;
                    if (accountgroupinfo != null)
                    {
                        e.Row.Background = mastervalidation.AccountGroupValidation(accountgroupinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Account Ledgers"):
                    accountledgerinfo = e.Row.Item as LedgerInfo;
                    if (accountledgerinfo != null)
                    {
                        e.Row.Background = mastervalidation.LedgerValidations(accountledgerinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Product Groups"):
                    productgroupinfo = e.Row.Item as productGroupInfo;
                    if (productgroupinfo != null)
                    {
                        e.Row.Background = mastervalidation.ProductGroupValidation(productgroupinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Units"):
                    unitinfo = e.Row.Item as unitsInfo;
                    if (unitinfo != null)
                    {
                        e.Row.Background = mastervalidation.UnitValidation(unitinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Godowns"):
                    godowninfo = e.Row.Item as godownInfo;
                    if (godowninfo != null && godowninfo.Godown_Name != null)
                    {
                        e.Row.Background = mastervalidation.GodownValidation(godowninfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Products"):
                    productinfo = e.Row.Item as productInfo;
                    if (productinfo != null)
                    {
                        e.Row.Background = mastervalidation.ProductValidation(productinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;


                case ("Stock"):
                    Stockinfo = e.Row.Item as StockInfo;
                    if (Stockinfo != null)
                    {
                        e.Row.Background = mastervalidation.StockValidation(Stockinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;


                case ("Batches"):
                    batchinfo = e.Row.Item as batchesInfo;
                    if (batchinfo != null)
                    {
                        e.Row.Background = mastervalidation.BatchValidation(batchinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Currency"):
                    currencyinfo = e.Row.Item as currencyInfo;
                    if (currencyinfo != null)
                    {
                        if (mastervalidation.CurrencyValidation(currencyinfo))
                        {
                            e.Row.Background = Brushes.LightGreen;
                        }
                        else
                        {
                            e.Row.Background = Brushes.LightCoral;
                            e.Row.ToolTip = "Currency exists";
                        }
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("Voucher Type"):
                    vouchertype = e.Row.Item as voucherTypesInfo;
                    if (vouchertype != null)
                    {
                        e.Row.Background = mastervalidation.VoucherTypeValidation(vouchertype, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                case ("SalesMan"):
                    employeeinfo = e.Row.Item as EmployeeInfo;
                    if (employeeinfo != null && employeeinfo.name != null)
                    {
                        e.Row.Background = mastervalidation.SalesmanValidation(employeeinfo, out toolTip);
                        e.Row.ToolTip = toolTip;
                    }
                    else
                    {
                        e.Row.Background = Brushes.Wheat;
                        e.Row.ToolTip = null;
                    }
                    break;
                default:
                    break;
            }
        }
        private void dgDatas_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key)
            {
                foreach (var row in dgDatas.SelectedItems)
                {
                    dgDatas.Items.Remove(row);
                }

            }
        }

        private void dgDatas_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.IsReadOnly = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            lblBorder.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
