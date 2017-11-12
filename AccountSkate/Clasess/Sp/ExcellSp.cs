using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace AccountSkate
{
    class ExcellSp : DBSourceConnectionExcell
    {
        public bool CheckExcellConnection()
        {
            bool isTrue = false;
            try
            {
                if (oledbcon.State == ConnectionState.Closed)
                {
                    oledbcon.Open();
                    isTrue = true;
                   if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
                }
                else
                {
                    isTrue = true;
                }
            }
            catch (Exception)
            { }
            return isTrue;
        }

        private bool CheckColumnsMandatory(List<string> lstColumnName)
        {
            DataTable myColumns = oledbcon.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, "Sheet1$", null });
            List<string> items = (from a in myColumns.AsEnumerable()
                                  from b in lstColumnName
                                  where a.Field<string>("COLUMN_NAME").ToString() == b.ToString()
                                  select a.Field<string>("COLUMN_NAME")).Distinct().ToList();

            return (items.Count == lstColumnName.Count);

        }

        public List<pricingLevelInfo> GetPriceLevel()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Pricing Level Name");
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    pricingLevelInfo infopricelevel;
                    while (oledbReader.Read())
                    {
                        try
                        {
                            infopricelevel = new pricingLevelInfo();
                            infopricelevel.PricingLevelName = (oledbReader["Pricing Level Name"] != DBNull.Value) ? oledbReader["Pricing Level Name"].ToString() : string.Empty;
                            infopricelevel.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            lstPriceLevel.Add(infopricelevel);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Pricing Level Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Pricing Level Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
                if (oledbcon!=null && oledbcon.State == ConnectionState.Open)
                oledbcon.Close();
            }
            return lstPriceLevel;
        }

        public List<CustomerSupplierInfo> GetCustomer()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Customer Name");
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    CustomerSupplierInfo infoCustomerSupplier;
                    string Exceptions = null;
                    while (oledbReader.Read())
                    {
                        try
                        {

                            infoCustomerSupplier = new CustomerSupplierInfo();
                            infoCustomerSupplier.Name = (oledbReader["Customer Name"] != DBNull.Value) ? oledbReader["Customer Name"].ToString() : string.Empty;
                            infoCustomerSupplier.Opening_Balance = (oledbReader["Opening balance"] != DBNull.Value) ? oledbReader["Opening balance"].ToString() : "0";
                            infoCustomerSupplier.Address = (oledbReader["Address"] != DBNull.Value) ? oledbReader["Address"].ToString() : string.Empty;
                            infoCustomerSupplier.Bill_By_Bill = (oledbReader["Bill by Bill"] != DBNull.Value) ? oledbReader["Bill by Bill"].ToString() : "false";
                            infoCustomerSupplier.Credit_Limit = (oledbReader["Credit Limit"] != DBNull.Value) ? oledbReader["Credit Limit"].ToString() : "0";
                            infoCustomerSupplier.CST = (oledbReader["CST"] != DBNull.Value) ? oledbReader["CST"].ToString() : string.Empty;
                            infoCustomerSupplier.Mailing_Name = (oledbReader["Mailing Name"] != DBNull.Value) ? oledbReader["Mailing Name"].ToString() : infoCustomerSupplier.Name;
                            infoCustomerSupplier.Account_Number = (oledbReader["Account Number"] != DBNull.Value) ? oledbReader["Account Number"].ToString() : string.Empty;
                            infoCustomerSupplier.Phone = (oledbReader["Phone"] != DBNull.Value) ? oledbReader["Phone"].ToString() : string.Empty;
                            infoCustomerSupplier.Email = (oledbReader["E-mail"] != DBNull.Value) ? oledbReader["E-mail"].ToString() : string.Empty;
                            infoCustomerSupplier.PAN = (oledbReader["PAN"] != DBNull.Value) ? oledbReader["PAN"].ToString() : string.Empty;
                            infoCustomerSupplier.Pricing_Level = (oledbReader["Pricing level"] != DBNull.Value) ? oledbReader["Pricing level"].ToString() : "NA";
                            infoCustomerSupplier.Credit_Period = (oledbReader["Credit Period"] != DBNull.Value) ? oledbReader["Credit Period"].ToString() : "0";
                            infoCustomerSupplier.TIN = (oledbReader["TIN"] != DBNull.Value) ? oledbReader["TIN"].ToString() : string.Empty;
                            infoCustomerSupplier.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            infoCustomerSupplier.Mobile = (oledbReader["Mobile"] != DBNull.Value) ? oledbReader["Mobile"].ToString() : string.Empty;
                            infoCustomerSupplier.CrorDr = (oledbReader["CrorDr"] != DBNull.Value) ? oledbReader["CrorDr"].ToString() : string.Empty;
                            infoCustomerSupplier.Branch_Name = (oledbReader["Branch Name"] != DBNull.Value) ? oledbReader["Branch Name"].ToString() : string.Empty;
                            infoCustomerSupplier.Area = (oledbReader["Area"] != DBNull.Value) ? oledbReader["Area"].ToString() : string.Empty;
                            infoCustomerSupplier.Route = (oledbReader["Route"] != DBNull.Value) ? oledbReader["Route"].ToString() : string.Empty;
                            infoCustomerSupplier.Brach_Code = (oledbReader["Branch code"] != DBNull.Value) ? oledbReader["Branch code"].ToString() : string.Empty;
                            lstCustomerSupplierInfo.Add(infoCustomerSupplier);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Customer Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Customer Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstCustomerSupplierInfo;
        }

        public List<accountGroupInfo> GetAccountGroup()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Account Group Name");
                ff.Add("Under");
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    accountGroupInfo infoAccountGroup;
                    while (oledbReader.Read())
                    {
                        try
                        {
                            infoAccountGroup = new accountGroupInfo();
                            infoAccountGroup.Name = (oledbReader["Account Group Name"] != DBNull.Value) ? (oledbReader["Account Group Name"].ToString()) : string.Empty;
                            infoAccountGroup.Affect_Gross_Profit = (oledbReader["Affect Gross Profit"] != DBNull.Value) ? oledbReader["Affect Gross Profit"].ToString() : "false";
                            infoAccountGroup.Under = (oledbReader["Under"] != DBNull.Value) ? oledbReader["Under"].ToString() : string.Empty;
                            infoAccountGroup.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            infoAccountGroup.Nature = GetNature(infoAccountGroup.Under.ToLower());
                            lstAccountGroup.Add(infoAccountGroup);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Account Group Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Account Group Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstAccountGroup;
        }

        public List<LedgerInfo> GetAccountLedger()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Account Ledger Name");
                ff.Add("Account Group");
                Clear();
                LedgerInfo ledgerinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    int i = 0;
                    while (oledbReader.Read())
                    {
                        try
                        {
                            ledgerinfo = new LedgerInfo();
                            ledgerinfo.Name = (oledbReader["Account Ledger Name"] != DBNull.Value) ? (oledbReader["Account Ledger Name"].ToString()) : string.Empty;
                            ledgerinfo.Account_Group = (oledbReader["Account Group"] != DBNull.Value) ? (oledbReader["Account Group"].ToString()) : string.Empty;
                            ledgerinfo.Opening_Balance = (oledbReader["Opening Balance"] != DBNull.Value) ? oledbReader["Opening Balance"].ToString() : "0";
                            ledgerinfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            ledgerinfo.CrorDr = (oledbReader["CrorDr"] != DBNull.Value) ? oledbReader["CrorDr"].ToString() : string.Empty;
                            lstLedgerInfo.Add(ledgerinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? ++i + " ." + oledbReader["Account Ledger Name"].ToString() + " row - " + Ex.Message : "\n" + ++i + " ." + oledbReader["Account Ledger Name"].ToString() + " row - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstLedgerInfo;
        }

        public List<productGroupInfo> GetProductGroup()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Group Name");
                Clear();
                productGroupInfo productgroupinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            productgroupinfo = new productGroupInfo();
                            productgroupinfo.GroupName = (oledbReader["Group Name"] != DBNull.Value) ? (oledbReader["Group Name"].ToString()) : string.Empty;
                            productgroupinfo.Group_Under = (oledbReader["Group Under"] != DBNull.Value) ? (oledbReader["Group Under"].ToString()) : "Primary";
                            productgroupinfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            lstproductgroupinfo.Add(productgroupinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Group Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Group Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }

            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstproductgroupinfo;
        }

        public List<unitsInfo> GetUnit()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Unit Name");
                ff.Add("Formal Name");
                ff.Add("No Of Decimal Places");
                Clear();
                unitsInfo unitinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();

                    while (oledbReader.Read())
                    {
                        try
                        {
                            unitinfo = new unitsInfo();
                            unitinfo.UnitName = (oledbReader["Unit Name"] != DBNull.Value) ? (oledbReader["Unit Name"].ToString()) : string.Empty;
                            unitinfo.FormalName = (oledbReader["Formal Name"] != DBNull.Value) ? oledbReader["Formal Name"].ToString() : string.Empty; ;
                            unitinfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            unitinfo.noOfDecimalPlaces = (oledbReader["No Of Decimal Places"] != DBNull.Value) ? oledbReader["No Of Decimal Places"].ToString() : "0";
                            lstuniinfo.Add(unitinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Unit Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Unit Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstuniinfo;
        }

        public List<godownInfo> GetGodown()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Goddown Name");
                Clear();
                godownInfo godowninfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            godowninfo = new godownInfo();
                            godowninfo.Godown_Name = (oledbReader["Goddown Name"] != DBNull.Value) ? (oledbReader["Goddown Name"].ToString()) : string.Empty;
                            godowninfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            lstgodowninfo.Add(godowninfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Goddown Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Goddown Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstgodowninfo;
        }

        public List<productInfo> GetProduct()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Product Code");
                ff.Add("Product Name");
                ff.Add("Group");
                ff.Add("Unit");
                Clear();
                productInfo productinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            productinfo = new productInfo();
                            productinfo.ProductName = (oledbReader["Product Name"] != DBNull.Value) ? (oledbReader["Product Name"].ToString()) : string.Empty;
                            productinfo.Group = (oledbReader["Group"] != DBNull.Value) ? (oledbReader["Group"].ToString()) : string.Empty;
                            productinfo.Tax = (oledbReader["Tax"] != DBNull.Value) ? (oledbReader["Tax"].ToString()) : "NA";
                            productinfo.AllowBatch = (oledbReader["Allow Batch"] != DBNull.Value) ? oledbReader["Allow Batch"].ToString() : "false";
                            productinfo.ConversionRate = (oledbReader["Conversion Rate"] != DBNull.Value) ? oledbReader["Conversion Rate"].ToString() : "0";
                            productinfo.DefaultGodown = (oledbReader["Default Godown"] != DBNull.Value) ? oledbReader["Default Godown"].ToString() : "NA";
                            productinfo.Rack = (oledbReader["Rack"] != DBNull.Value) ? (oledbReader["Rack"].ToString()) : "NA";
                            productinfo.MaximumStock = (oledbReader["Maximum Stock"] != DBNull.Value) ? (oledbReader["Maximum Stock"].ToString()) : "0";
                            productinfo.MinimumStock = (oledbReader["Minimum Stock"] != DBNull.Value) ? (oledbReader["Minimum Stock"].ToString()) : "0";
                            productinfo.MRP = (oledbReader["MRP"] != DBNull.Value) ? (oledbReader["MRP"].ToString()) : "0";
                            productinfo.MultipleUnit = (oledbReader["Multiple Unit"] != DBNull.Value) ? (oledbReader["Multiple Unit"]).ToString() : "false";
                            productinfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            productinfo.OpeningStock = (oledbReader["Opening Stock"] != DBNull.Value) ? (oledbReader["Opening Stock"].ToString()) : "false";
                            productinfo.PurchaseRate = (oledbReader["Purchase Rate"] != DBNull.Value) ? (oledbReader["Purchase Rate"].ToString()) : "0";
                            productinfo.ReorderLevel = (oledbReader["Reorder Level"] != DBNull.Value) ? (oledbReader["Reorder Level"].ToString()) : "0";
                            productinfo.SalesRate = (oledbReader["Sales Rate"] != DBNull.Value) ? (oledbReader["Sales Rate"].ToString()) : "0";
                            productinfo.TaxApplicableOn = (oledbReader["Tax Applicable On"] != DBNull.Value) ? (oledbReader["Tax Applicable On"].ToString()) : "NA";
                            productinfo.Unit = (oledbReader["Unit"] != DBNull.Value) ? oledbReader["Unit"].ToString() : "NA";
                            productinfo.ProductCode = (oledbReader["Product Code"] != DBNull.Value) ? oledbReader["Product Code"].ToString() : string.Empty;
                            productinfo.Size = "1";
                            lstproductinfo.Add(productinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Product Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Product Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstproductinfo;
        }

        public List<StockInfo> GetStock()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Name");
                Clear();
                StockInfo productinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            productinfo = new StockInfo();
                            productinfo.ProductName = (oledbReader["Name"] != DBNull.Value) ? (oledbReader["Name"].ToString()) : string.Empty;
                            productinfo.OpeningStock = "true";
                            productinfo.ClosingRate = (oledbReader["Closing Values"] != DBNull.Value) ? oledbReader["Closing Values"].ToString() : "0";
                            productinfo.OpeningStockNumber = (oledbReader["Opening Stock Number"] != DBNull.Value) ? oledbReader["Opening Stock Number"].ToString() : "0";
                            productinfo.Unit = (oledbReader["Base Units"] != DBNull.Value) ? oledbReader["Base Units"].ToString() : "NA";
                            lstStockInfo.Add(productinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstStockInfo;
        }

        public List<batchesInfo> GetBatch()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Product Name");
                ff.Add("Batch Name");
                Clear();
                batchesInfo batchinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            batchinfo = new batchesInfo();
                            batchinfo.ProductName = (oledbReader["Product Name"] != DBNull.Value) ? (oledbReader["Product Name"].ToString()) : string.Empty;
                            batchinfo.MfgDate = (oledbReader["MFG Date"] != DBNull.Value) ? Convert.ToDateTime(oledbReader["MFG Date"]).ToString() : Convert.ToDateTime(DateTime.Now.ToString()).ToString();
                            batchinfo.ExpiryDate = (oledbReader["Expiry Date"] != DBNull.Value) ? Convert.ToDateTime(oledbReader["Expiry Date"].ToString()).ToString() : Convert.ToDateTime(DateTime.Now.ToString()).ToString();
                            batchinfo.BatchName = (oledbReader["Batch Name"] != DBNull.Value) ? oledbReader["Batch Name"].ToString() : string.Empty;
                            batchinfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            lstbatches.Add(batchinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Batch Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Batch Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstbatches;
        }

        public List<currencyInfo> GetCurrency()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Currency Name");
                ff.Add("Currency Symbol");
                ff.Add("No of Decimal Place");
                Clear();
                currencyInfo currencyinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            currencyinfo = new currencyInfo();
                            currencyinfo.CurrencyName = (oledbReader["Currency Name"] != DBNull.Value) ? (oledbReader["Currency Name"].ToString()) : string.Empty;
                            currencyinfo.CurrencySymbol = (oledbReader["Currency Symbol"] != DBNull.Value) ? (oledbReader["Currency Symbol"].ToString()) : string.Empty;
                            currencyinfo.NoOfDecimalPlaces = (oledbReader["No of Decimal Place"] != DBNull.Value) ? oledbReader["No of Decimal Place"].ToString() : "0";
                            currencyinfo.Narration = (oledbReader["Narration"] != DBNull.Value) ? (oledbReader["Narration"].ToString()) : string.Empty;
                            lstcurrencyinfo.Add(currencyinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Currency Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Currency Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstcurrencyinfo;
        }

        public List<voucherTypesInfo> GetVoucherType()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Name");
                ff.Add("Type of Voucher");
                ff.Add("Method Of Voucher Numbering");
                ff.Add("Dot Matrix Print Format");
                Clear();
                voucherTypesInfo vouchertypeinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            vouchertypeinfo = new voucherTypesInfo();
                            vouchertypeinfo.name = (oledbReader["Name"] != DBNull.Value) ? (oledbReader["Name"].ToString()) : string.Empty;
                            //vouchertypeinfo.dotMatrixPrintFormat = vouchertypeinfo.name;
                            vouchertypeinfo.dotMatrixPrintFormat = (oledbReader["Dot Matrix Print Format"] != DBNull.Value) ? (oledbReader["Dot Matrix Print Format"].ToString()) : string.Empty;
                            //vouchertypeinfo.active = (oledbReader["Active"] != DBNull.Value) ? (Convert.ToDateTime(oledbReader["Active"]) > DateTime.Today) : true;
                            vouchertypeinfo.active = (oledbReader["Active"] != DBNull.Value) ? ((oledbReader["Active"]).ToString() == "True").ToString() : "false";
                            vouchertypeinfo.declaration = (oledbReader["Declaration"] != DBNull.Value) ? (oledbReader["Declaration"].ToString()) : string.Empty;
                            vouchertypeinfo.methodOfVoucherNumbering = (oledbReader["Method Of Voucher Numbering"] != DBNull.Value) ? ((oledbReader["Method Of Voucher Numbering"].ToString() == "None") ? "Automatic" : oledbReader["Method Of Voucher Numbering"].ToString()) : string.Empty;
                            vouchertypeinfo.narration = (oledbReader["Narration"] != DBNull.Value) ? (oledbReader["Narration"].ToString()) : string.Empty;
                            vouchertypeinfo.typeOfVoucher = (oledbReader["Type of Voucher"] != DBNull.Value) ? (oledbReader["Type of Voucher"].ToString()) : string.Empty;
                            lstvouchertypinfo.Add(vouchertypeinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstvouchertypinfo;
        }

        public List<EmployeeInfo> GetSalesMen()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Salesman code");
                ff.Add("Name");
                Clear();
                EmployeeInfo employeeinfo;
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    while (oledbReader.Read())
                    {
                        try
                        {
                            employeeinfo = new EmployeeInfo();
                            employeeinfo.name = (oledbReader["Name"] != DBNull.Value) ? (oledbReader["Name"].ToString()) : string.Empty;
                            employeeinfo.address = (oledbReader["Address"] != DBNull.Value) ? (oledbReader["Address"].ToString()) : string.Empty;
                            employeeinfo.eMail = (oledbReader["E-mail"] != DBNull.Value) ? (oledbReader["E-mail"].ToString()) : string.Empty;
                            employeeinfo.employeeCode = (oledbReader["Salesman code"] != DBNull.Value) ? (oledbReader["Salesman code"].ToString()) : string.Empty;
                            //employeeinfo.isActive = (oledbReader["`$DeactivationDate`"] != DBNull.Value) ? (Convert.ToDateTime(oledbReader["`$DeactivationDate`"]) < DateTime.Today) : false;
                            employeeinfo.isActive = (oledbReader["Active"] != DBNull.Value) ? ((oledbReader["Active"]).ToString() == "True").ToString() : "false";
                            employeeinfo.mobile = (oledbReader["Mobile"] != DBNull.Value) ? (oledbReader["Mobile"].ToString()) : string.Empty;
                            employeeinfo.narration = (oledbReader["Narration"] != DBNull.Value) ? (oledbReader["Narration"].ToString()) : string.Empty;
                            employeeinfo.phone = (oledbReader["Phone"] != DBNull.Value) ? (oledbReader["Phone"].ToString()) : string.Empty;
                            lstemplyeeinfo.Add(employeeinfo);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstemplyeeinfo;
        }

        public List<CustomerSupplierInfo> GetSupplier()
        {
            try
            {
                oledbcon.Open();
                List<string> ff = new List<string>();
                ff.Add("Supplier Name");
                string Exceptions = null;
                if (CheckColumnsMandatory(ff))
                {
                    OleDbCommand oledbcmd = new OleDbCommand("select * from [Sheet1$]", oledbcon);
                    OleDbDataReader oledbReader = oledbcmd.ExecuteReader();
                    Clear();
                    CustomerSupplierInfo infoCustomerSupplier;
                    while (oledbReader.Read())
                    {
                        try
                        {
                            infoCustomerSupplier = new CustomerSupplierInfo();
                            infoCustomerSupplier.Name = (oledbReader["Supplier Name"] != DBNull.Value) ? oledbReader["Supplier Name"].ToString() : string.Empty;
                            infoCustomerSupplier.Opening_Balance = (oledbReader["Opening balance"] != DBNull.Value) ? oledbReader["Opening balance"].ToString() : "0";
                            infoCustomerSupplier.Address = (oledbReader["Address"] != DBNull.Value) ? oledbReader["Address"].ToString() : string.Empty;
                            infoCustomerSupplier.Bill_By_Bill = (oledbReader["Bill by Bill"] != DBNull.Value) ? oledbReader["Bill by Bill"].ToString() : "false";
                            infoCustomerSupplier.Credit_Limit = (oledbReader["Credit Limit"] != DBNull.Value) ? oledbReader["Credit Limit"].ToString() : "0";
                            infoCustomerSupplier.CST = (oledbReader["CST"] != DBNull.Value) ? oledbReader["CST"].ToString() : string.Empty;
                            infoCustomerSupplier.Mailing_Name = (oledbReader["Mailing Name"] != DBNull.Value) ? oledbReader["Mailing Name"].ToString() : infoCustomerSupplier.Name;
                            infoCustomerSupplier.Account_Number = (oledbReader["Account Number"] != DBNull.Value) ? oledbReader["Account Number"].ToString() : string.Empty;
                            infoCustomerSupplier.Phone = (oledbReader["Phone"] != DBNull.Value) ? oledbReader["Phone"].ToString() : string.Empty;
                            infoCustomerSupplier.Email = (oledbReader["E-mail"] != DBNull.Value) ? oledbReader["E-mail"].ToString() : string.Empty;
                            infoCustomerSupplier.PAN = (oledbReader["PAN"] != DBNull.Value) ? oledbReader["PAN"].ToString() : string.Empty;
                            infoCustomerSupplier.Pricing_Level = (oledbReader["Pricing level"] != DBNull.Value) ? oledbReader["Pricing level"].ToString() : "NA";
                            infoCustomerSupplier.Credit_Period = (oledbReader["Credit Period"] != DBNull.Value) ? oledbReader["Credit Period"].ToString() : "0";
                            infoCustomerSupplier.TIN = (oledbReader["TIN"] != DBNull.Value) ? oledbReader["TIN"].ToString() : string.Empty;
                            infoCustomerSupplier.Narration = (oledbReader["Narration"] != DBNull.Value) ? oledbReader["Narration"].ToString() : string.Empty;
                            infoCustomerSupplier.Mobile = (oledbReader["Mobile"] != DBNull.Value) ? oledbReader["Mobile"].ToString() : string.Empty;
                            infoCustomerSupplier.CrorDr = (oledbReader["CrorDr"] != DBNull.Value) ? oledbReader["CrorDr"].ToString() : string.Empty;
                            infoCustomerSupplier.Branch_Name = (oledbReader["Branch Name"] != DBNull.Value) ? oledbReader["Branch Name"].ToString() : string.Empty;
                            infoCustomerSupplier.Area = (oledbReader["Area"] != DBNull.Value) ? oledbReader["Area"].ToString() : string.Empty;
                            infoCustomerSupplier.Route = (oledbReader["Route"] != DBNull.Value) ? oledbReader["Route"].ToString() : string.Empty;
                            infoCustomerSupplier.Brach_Code = (oledbReader["Branch code"] != DBNull.Value) ? oledbReader["Branch code"].ToString() : string.Empty;
                            lstCustomerSupplierInfo.Add(infoCustomerSupplier);
                        }
                        catch (FormatException Ex)
                        {
                            Exceptions += (Exceptions == null) ? oledbReader["Supplier Name"].ToString() + " - " + Ex.Message : "\n" + oledbReader["Supplier Name"].ToString() + " - " + Ex.Message;
                        }
                    }
                    oledbReader.Close();
                    if (Exceptions != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(Exceptions).ShowDialog()));
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Mandratory field are Miss matching...").ShowDialog()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox(ex.Message).ShowDialog()));
            }
            finally
            {
               if (oledbcon!=null && oledbcon.State == ConnectionState.Open) oledbcon.Close();
            }
            return lstCustomerSupplierInfo;
        }

    }
}



