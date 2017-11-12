using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AccountSkate
{
    class Checkings:DBConnection
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
            }
        }
        /// <summary>
        /// Function to check whether price level exist.
        /// </summary>
        /// <param name="strPriceLevel"></param>
        /// <returns></returns>
        public bool CheckPriceLevelExist(string strPriceLevel)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("PricingLevelCheckIfExist", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@pricinglevelName", SqlDbType.VarChar).Value = strPriceLevel;
                sqlCmd.Parameters.Add("@pricinglevelId", SqlDbType.Int).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Customer exist.
        /// </summary>
        /// <param name="strCustomer"></param>
        /// <returns></returns>
        public bool CheckCustomerExist(string strCustomer)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("AccountLedgerCheckExistenceForCustomer", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@ledgerName ", SqlDbType.VarChar).Value = strCustomer;
                sqlCmd.Parameters.Add("@ledgerId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();

                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Supplier exist.
        /// </summary>
        /// <param name="strSupplierName"></param>
        /// <returns></returns>
        public bool CheckSupplierExist(string strSupplierName)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("AccountLedgerCheckExistenceForSalesman", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@ledgerName", SqlDbType.VarChar).Value = strSupplierName;
                sqlCmd.Parameters.Add("@ledgerId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Account Group Exist.
        /// </summary>
        /// <param name="strAccountGroupName"></param>
        /// <returns></returns>
        public bool CheckAccountGroupExist(string strAccountGroupName)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("AccountGroupCheckExistence", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@accountGroupName", SqlDbType.VarChar).Value = strAccountGroupName;
                sqlCmd.Parameters.Add("@accountGroupId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Account Ledger exist.
        /// </summary>
        /// <param name="strAccountLedger"></param>
        /// <returns></returns>
        public bool CheckAccountLedgerExist(string strAccountLedger)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("AccountLedgerCheckExistence", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@ledgerName", SqlDbType.VarChar).Value = strAccountLedger;
                sqlCmd.Parameters.Add("@ledgerId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Product group exist.
        /// </summary>
        /// <param name="strProductGroup"></param>
        /// <returns></returns>
        public bool CheckProductGroupExist(string strProductGroup)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("ProductGroupCheckExistence", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@groupName", SqlDbType.VarChar).Value = strProductGroup;
                sqlCmd.Parameters.Add("@groupId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Unit exist.
        /// </summary>
        /// <param name="strUnit"></param>
        /// <returns></returns>
        public bool CheckUnitExist(string strUnit)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("UnitCheckExistence", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@unitName", SqlDbType.VarChar).Value = strUnit;
                sqlCmd.Parameters.Add("@unitId", SqlDbType.VarChar).Value = 0;
                object obj = sqlCmd.ExecuteScalar();

                if (obj != null && int.Parse(obj.ToString()) == 0)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Godown exist.
        /// </summary>
        /// <param name="strGodown"></param>
        /// <returns></returns>
        public bool CheckGodownExist(string strGodown)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("GodownCheckIfExist", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@godownName", SqlDbType.VarChar).Value = strGodown;
                sqlCmd.Parameters.Add("@godownId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Product exist.
        /// </summary>
        /// <param name="strProductName"></param>
        /// <returns></returns>
        public bool CheckProductExist(string strProductName)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("IF(SELECT COUNT (productId)	FROM tbl_Product WHERE ProductName= @strProductName AND ProductId <> 0 ) >0 SELECT 1 ELSE SELECT 0", sqlcon);
                sqlCmd.Parameters.AddWithValue("@strProductName", strProductName);
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check Batch exist.
        /// </summary>
        /// <param name="strProductName"></param>
        /// <param name="strBatchName"></param>
        /// <returns></returns>
        public bool CheckBatchExist(string strProductName, string strBatchName)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("IF (SELECT COUNT(batchId) FROM tbl_Batch WHERE productId = (SELECT productId FROM tbl_Product WHERE productName = @strProductName) AND tbl_Batch.batchNo = @strBatchName)>0 BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END", sqlcon);
                sqlCmd.Parameters.AddWithValue("@strProductName", strProductName);
                sqlCmd.Parameters.AddWithValue("@strBatchName", strBatchName);
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Currency exist.
        /// </summary>
        /// <param name="strCurrency"></param>
        /// <param name="strSymbol"></param>
        /// <returns></returns>
        public bool CheckCurrencyExist(string strCurrency, string strSymbol)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("IF(SELECT COUNT(currencyId)FROM  tbl_Currency where currencyName=@strCurrency  AND currencySymbol=@strSymbol AND currencyId <> '0')>'0'BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END", sqlcon);
                sqlCmd.Parameters.AddWithValue("@strCurrency", strCurrency);
                sqlCmd.Parameters.AddWithValue("@strSymbol", strSymbol);
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Voucher type exist.
        /// </summary>
        /// <param name="strVoucherType"></param>
        /// <returns></returns>
        public bool CheckVoucherTypeExist(string strVoucherType)
        {
            bool isTrue = true;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("VoucherTypeCheckExistence", sqlcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@voucherTypeName", SqlDbType.VarChar).Value = strVoucherType;
                sqlCmd.Parameters.Add("@voucherTypeId", SqlDbType.Decimal).Value = 0;
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = false;
                }
                sqlcon.Close();
            }
            catch (Exception)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to check whether Salesman exist.
        /// </summary>
        /// <param name="strSalesmanCode"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public bool CheckSalesmanExist(string strSalesmanCode, string strName)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("IF(SELECT COUNT(employeeId)FROM  tbl_Employee where employeeName=@strName OR employeeCode=@strSalesmanCode AND employeeId<>0)>0 BEGIN	SELECT 1 END ELSE BEGIN	SELECT 0 END", sqlcon);
                sqlCmd.Parameters.AddWithValue("@strName", strName);
                sqlCmd.Parameters.AddWithValue("@strSalesmanCode", strSalesmanCode);
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to get Financial Year
        /// </summary>
        /// <returns></returns>
        public DateTime functionGetFinancialYear()
        {
            DateTime datFinYear = DateTime.Now;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("SELECT fromDate FROM tbl_FinancialYear WHERE financialYearId ='1'", sqlcon);
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null)
                {
                    datFinYear = Convert.ToDateTime(obj);
                }
            }
            catch (Exception ex)
            {
            }
            return datFinYear;
        }
        /// <summary>
        /// Function to Check Account Group under ID
        /// </summary>
        /// <param name="strAccountGroup"></param>
        /// <returns></returns>
        public decimal functionCheckAccountGroupUnderId(string strAccountGroup)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT accountGroupId FROM tbl_AccountGroup WHERE accountGroupName='" + strAccountGroup + "'", sqlcon);
                object test = sqlcmd.ExecuteScalar();
                if (test != null)
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        /// <summary>
        /// Function to check Voucher Form ID
        /// </summary>
        /// <param name="strVoucherType"></param>
        /// <returns></returns>
        public decimal functionCheckVoucherFormId(string strVoucherType)
        {
            decimal decId = 1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT masterId FROM tbl_Master WHERE formName=(SELECT formId FROM tbl_Form WHERE formName='" + strVoucherType + "')", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        public string functionCheckNature(string strAccountGroup)
        {
            string strNature = "";
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT nature FROM tbl_AccountGroup WHERE accountGroupName='" + strAccountGroup + "'", sqlcon);
                object test = sqlcmd.ExecuteScalar();
                if (test != null)
                {
                    strNature = test.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return strNature;
        }
        /// <summary>
        /// Function to Add Account ledger
        /// </summary>
        /// <param name="infoAccountLedger"></param>
        /// <returns></returns>
        public decimal functionCheckAreaIdId(string strArea)
        {
            decimal decId = 0;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("AreaAdd", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@areaName", SqlDbType.VarChar).Value = strArea;
                sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                object test = sqlcmd.ExecuteScalar();
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        internal decimal functionPricingLevelId(string p)
        {
            decimal decId = 0;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("PricingLevelAddWithoutSamePricingLevel", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@pricinglevelName", SqlDbType.VarChar).Value = p;
                sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                object test = sqlcmd.ExecuteScalar();
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }

        internal decimal functionRoutId(string p, string p1)
        {
            decimal decId = 0;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("RouteAddforSkate", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@routeName", SqlDbType.VarChar).Value = p;
                sqlcmd.Parameters.Add("@areaName", SqlDbType.VarChar).Value = p1;
                sqlcmd.Parameters.Add("@narration", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extraDate", SqlDbType.DateTime).Value = DateTime.Today;
                sqlcmd.Parameters.Add("@extra1", SqlDbType.VarChar).Value = string.Empty;
                sqlcmd.Parameters.Add("@extra2", SqlDbType.VarChar).Value = string.Empty;
                object test = sqlcmd.ExecuteScalar();
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        /// <summary>
        ///  Function to Generate Product Code
        /// </summary>
        /// <returns></returns>
        public int functionGenerateProductCode()
        {
            object obj;
            int inProductCode = 1;
            try
            {
                do
                {
                    functionConnectionState();
                    SqlCommand sqlcmd = new SqlCommand("ProductCodeCheckExistence", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@productCode", SqlDbType.VarChar).Value = "MS00" + inProductCode;
                    sqlcmd.Parameters.Add("@productId", SqlDbType.Decimal).Value = 0;
                    obj = sqlcmd.ExecuteScalar();
                    if (obj != null && int.Parse(obj.ToString()) == 1)
                    {
                        inProductCode++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                } while (int.Parse(obj.ToString()) == 1);
            }
            catch (Exception ex)
            {
            }
            return inProductCode;
        }
        /// <summary>
        /// Function to check Group under ID
        /// </summary>
        /// <param name="strGroup"></param>
        /// <returns></returns>
        public decimal functionCheckProductGroupUnderId(string strGroup)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT groupId FROM tbl_ProductGroup WHERE groupName='" + strGroup + "'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());

                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        /// <summary>
        /// Function to Check Unit ID
        /// </summary>
        /// <param name="strUnit"></param>
        /// <returns></returns>
        public decimal functionCheckUnitId(string strUnit)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT unitId FROM tbl_Unit WHERE unitName='" + strUnit + "'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        public decimal CheckStockExists(string strProduct)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT productId from tbl_StockPosting where productId = (Select productId from tbl_Product where productName= '" + strProduct + "')", sqlcon);
                object test = sqlcmd.ExecuteScalar();

                if (test != null)
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception)
            {
            }
            return decId;
        }
        public decimal functionCheckTaxId(string p)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT taxId FROM tbl_Tax WHERE taxName='" + p + "'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }

        public string functionGetNature(string p)
        {
            string decId = "";
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT nature FROM tbl_AccountGroup WHERE accountGroupName='" + p + "'", sqlcon);
                object test = sqlcmd.ExecuteScalar();
                if (test != null)
                {
                    decId = test.ToString();
                }
            }
            catch (Exception)
            {
            }
            return decId;
        }

        public decimal functionCheckGodownId(string p)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT godownId FROM tbl_Godown WHERE godownName='" + p + "'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }

        public decimal functionCheckRackId(string p)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT rackId FROM tbl_Rack WHERE rackName='" + p + "'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        internal bool functionCheckProductCodeExists(string p)
        {
            bool isTrue = true;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("ProductCodeCheckExistence", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                object test = sqlcmd.ExecuteScalar();
                if (test != null)
                {
                    isTrue = Convert.ToBoolean(test);
                }
            }
            catch (Exception)
            {
            }
            return isTrue;
        }
        public decimal functionBatchId(string p)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("SELECT batchId FROM tbl_Batch WHERE productId=(SELECT productId FROM tbl_Product WHERE productName = '" + p + "') AND batchNo='NA'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());
                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }

        public decimal functionProductId(string p)
        {
            decimal decId = -1;
            try
            {
                functionConnectionState();
                SqlCommand sqlcmd = new SqlCommand("Select productId from tbl_Product where productName= '" + p + "'", sqlcon);
                object test = Convert.ToDecimal(sqlcmd.ExecuteScalar());

                if (test != null && test.ToString() != "0")
                {
                    decId = Convert.ToDecimal(test);
                }
            }
            catch (Exception ex)
            {
            }
            return decId;
        }
        internal bool CheckTypeofVoucherExists(string p)
        {
            bool isTrue = false;
            try
            {
                functionConnectionState();
                SqlCommand sqlCmd = new SqlCommand("IF(SELECT COUNT(typeOfVoucher)FROM  tbl_VoucherType where typeOfVoucher=@strName)>0 BEGIN	SELECT 1 END ELSE BEGIN	SELECT 0 END", sqlcon);
                sqlCmd.Parameters.AddWithValue("@strName", p);
                object obj = sqlCmd.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) == 1)
                {
                    isTrue = true;
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {
            }
            return isTrue;
        }
        /// <summary>
        /// Function to Generate Salesman Code
        /// </summary>
        /// <returns></returns>
        public int functionGenerateSalesmanCode()
        {
            int insalemanCode = 0;
            object obj;
            try
            {
                do
                {
                   
                    SqlCommand sqlcmd = new SqlCommand("EmployeeCodeCheckExistance", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.Add("@employeeCode", SqlDbType.VarChar).Value = "MS00" + insalemanCode;
                    sqlcmd.Parameters.Add("@employeeId", SqlDbType.Decimal).Value = 0;
                    obj = sqlcmd.ExecuteScalar();
                    if (obj != null && int.Parse(obj.ToString()) == 1)
                    {
                        insalemanCode++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                } while (int.Parse(obj.ToString()) == 1);
            }
            catch (Exception ex)
            {
            }
            return insalemanCode;
        }
    }
}
