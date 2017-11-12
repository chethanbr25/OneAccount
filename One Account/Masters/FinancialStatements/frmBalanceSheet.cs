//This is a source code or part of Oneaccount project
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
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Globalization;
namespace One_Account
{
    public partial class frmBalanceSheet : Form
    {
        #region Public Variables
        /// <summary>
        /// public variable declaration part
        /// </summary>
        bool isFormLoad = false;
        int inCurrenRowIndex = 0;
        int inCurentcolIndex = 0;
        string calculationMethod = string.Empty;
        decimal decPrintOrNot = 0;
        decimal decPrintOrNot1 = 0;
        DateTime dtfromdate = PublicVariables._dtFromDate;
        #endregion
        #region FUNCTIONS
        /// <summary>
        /// Creates an instance of frmBalanceSheet  class
        /// </summary>
        public frmBalanceSheet()
        {
            InitializeComponent();
        }
      
        /// <summary>
        /// Function to fill Datagridview
        /// </summary>
        public void FillGrid()
        {
            try
            {
                if (!isFormLoad)
                {
                    DateValidation objValidation = new DateValidation();
                    objValidation.DateValidationFunction(txtToDate);
                    if (txtToDate.Text == string.Empty)
                        txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    Font newFont = new Font(dgvReport.Font, FontStyle.Bold);
                    CurrencyInfo InfoCurrency = new CurrencyInfo();
                    CurrencySP SpCurrency = new CurrencySP();
                    InfoCurrency = SpCurrency.CurrencyView(1);
                    int inDecimalPlaces = InfoCurrency.NoOfDecimalPlaces;
                    dgvReport.Rows.Clear();
                    FinancialStatementSP SpFinance = new FinancialStatementSP();
                    DataSet DsetBalanceSheet = new DataSet();
                    DataTable dtbl = new DataTable();
                    SettingsInfo InfoSettings = new SettingsInfo();
                    SettingsSP SpSettings = new SettingsSP();
                    //--------------- Selection Of Calculation Method According To Settings ------------------// 
                    if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "FIFO")
                    {
                        calculationMethod = "FIFO";
                    }
                    else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "Average Cost")
                    {
                        calculationMethod = "Average Cost";
                    }
                    else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "High Cost")
                    {
                        calculationMethod = "High Cost";
                    }
                    else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "Low Cost")
                    {
                        calculationMethod = "Low Cost";
                    }
                    else if (SpSettings.SettingsStatusCheck("StockValueCalculationMethod") == "Last Purchase Rate")
                    {
                        calculationMethod = "Last Purchase Rate";
                    }

                    int LiabilityRow = 0;
                    int AssetRow = 0;
                    decimal TotalAssets = 0;
                    decimal TotalLiabilities = 0;
                    decimal OpeningStock = SpFinance.StockValueGetOnDate(Convert.ToDateTime(txtToDate.Text), calculationMethod, true, true);
                    OpeningStock = Math.Round(OpeningStock, inDecimalPlaces);
                    DsetBalanceSheet = SpFinance.BalanceSheet(PublicVariables._dtFromDate, DateTime.Parse(txtToDate.Text));
                    //------------------- Asset -------------------------------// 
                    dtbl = DsetBalanceSheet.Tables[0];
                    foreach (DataRow rw in dtbl.Rows)
                    {
                        dgvReport.Rows.Add();
                        decimal balance = (rw["ID"].ToString() == "6") ? Convert.ToDecimal(rw["Balance"].ToString()) + OpeningStock : Convert.ToDecimal(rw["Balance"].ToString());
                        if (balance > 0)
                        {
                            dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = rw["Name"].ToString();
                            dgvReport.Rows[AssetRow].Cells["Amount1"].Value = balance.ToString("N2");
                            dgvReport.Rows[AssetRow].Cells["GroupId1"].Value = rw["ID"].ToString();
                            TotalAssets += balance;
                            AssetRow++;
                        }
                        else
                        {
                            balance = balance * -1;
                            dgvReport.Rows.Add();
                            dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = rw["Name"].ToString();
                            dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = balance.ToString("N2");
                            dgvReport.Rows[LiabilityRow].Cells["GroupId2"].Value = rw["ID"].ToString();
                            TotalLiabilities += balance;
                            LiabilityRow++;
                        }
                   
                    }

                    //------------------------ Liability ---------------------//
                    dtbl = DsetBalanceSheet.Tables[1];
                    foreach (DataRow rw in dtbl.Rows)
                    {
                        decimal balance = Convert.ToDecimal(rw["Balance"].ToString());
                        if (balance < 0)
                        {
                            balance = balance * -1;
                            dgvReport.Rows.Add();
                            dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = rw["Name"].ToString();
                            dgvReport.Rows[AssetRow].Cells["Amount1"].Value = balance.ToString("N2");
                            dgvReport.Rows[AssetRow].Cells["GroupId1"].Value = rw["ID"].ToString();
                            TotalAssets += balance;
                            AssetRow++;
                        }
                        else
                        {
                            dgvReport.Rows.Add();
                            dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = rw["Name"].ToString();
                            dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = balance.ToString("N2");
                            dgvReport.Rows[LiabilityRow].Cells["GroupId2"].Value = rw["ID"].ToString();
                            TotalLiabilities += balance;
                            LiabilityRow++;
                        }
                    }

                    //---------------------Closing Stock---------------------------------------------------------------------------------------------------------------

                    decimal ClosingStock = SpFinance.StockValueGetOnDate(Convert.ToDateTime(txtToDate.Text), calculationMethod, false, false);
                    ClosingStock = Math.Round(ClosingStock, inDecimalPlaces) - OpeningStock;
                    if (ClosingStock != 0)
                    {
                        if (ClosingStock > 0)
                        {
                            dgvReport.Rows.Add();
                            dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "Closing Stock";
                            dgvReport.Rows[AssetRow].Cells["Amount1"].Value = ClosingStock.ToString("N2");
                            TotalAssets += ClosingStock;
                            AssetRow++;
                        }
                        else
                        {
                            ClosingStock = ClosingStock * -1;
                            dgvReport.Rows.Add();
                            dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = "Closing Stock";
                            dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = ClosingStock.ToString("N2");
                            TotalLiabilities += ClosingStock;
                            LiabilityRow++;
                        }
                    }

                    if (AssetRow > LiabilityRow)
                    {
                        LiabilityRow = AssetRow;
                    }
                    else
                    {
                        AssetRow = LiabilityRow;
                    }
                    //---------------------Profit And Loss---------------------------------------------------------------------------------------------------------------

                    DataSet dsetProfitAndLoss = SpFinance.ProfitAndLossAnalysisUpToaDateForBalansheet(PublicVariables._dtFromDate, DateTime.Parse(txtToDate.Text));
                    decimal dcProfit = 0;
                    for (int i = 0; i < dsetProfitAndLoss.Tables.Count; ++i)
                    {
                        dtbl = dsetProfitAndLoss.Tables[i];
                        decimal dcSum = 0;
                        if (i == 0 || (i % 2) == 0)
                        {
                            if (dtbl.Rows.Count > 0)
                            {
                                dcSum = decimal.Parse(dtbl.Compute("Sum(Debit)", string.Empty).ToString());
                                dcProfit = dcProfit - dcSum;
                            }
                        }
                        else
                        {
                            if (dtbl.Rows.Count > 0)
                            {
                                dcSum = decimal.Parse(dtbl.Compute("Sum(Credit)", string.Empty).ToString());
                                dcProfit = dcProfit + dcSum;
                            }
                        }
                    }

                    DataSet dsetProfitAndLossOpening = SpFinance.ProfitAndLossAnalysisUpToaDateForPreviousYears(PublicVariables._dtFromDate);
                    decimal dcProfitOpening = 0;
                    for (int i = 0; i < dsetProfitAndLossOpening.Tables.Count; ++i)
                    {
                        dtbl = dsetProfitAndLossOpening.Tables[i];
                        decimal dcSum = 0;
                        if (i == 0 || (i % 2) == 0)
                        {
                            if (dtbl.Rows.Count > 0)
                            {
                                dcSum = decimal.Parse(dtbl.Compute("Sum(Debit)", string.Empty).ToString());
                                dcProfitOpening = dcProfitOpening - dcSum;
                            }
                        }
                        else
                        {
                            if (dtbl.Rows.Count > 0)
                            {
                                dcSum = decimal.Parse(dtbl.Compute("Sum(Credit)", string.Empty).ToString());
                                dcProfitOpening = dcProfitOpening + dcSum;
                            }
                        }
                    }

                    decimal decProfitLedgerOpening = decimal.Parse(DsetBalanceSheet.Tables[3].Compute("Sum(Balance)", string.Empty).ToString());

                    decimal decTotalProfitAndLoss = decimal.Parse(DsetBalanceSheet.Tables[2].Compute("Sum(Balance)", string.Empty).ToString());

                    decimal decCurrentProfitLoss = dcProfit + ClosingStock;
                    decimal decOpeningOfProfitAndLoss = decProfitLedgerOpening + dcProfitOpening;
                    decimal decTotalProfitAndLossOverAll = decTotalProfitAndLoss + decOpeningOfProfitAndLoss + decCurrentProfitLoss;
                    decTotalProfitAndLoss = (decTotalProfitAndLoss < 0) ? decTotalProfitAndLoss * -1 : decTotalProfitAndLoss;
                    decOpeningOfProfitAndLoss = (decOpeningOfProfitAndLoss < 0) ? decOpeningOfProfitAndLoss * -1 : decOpeningOfProfitAndLoss;
                    decCurrentProfitLoss = (decCurrentProfitLoss < 0) ? decCurrentProfitLoss * -1 : decCurrentProfitLoss;

                    if (decTotalProfitAndLossOverAll >= 0)
                    {
                        dgvReport.Rows.Add();
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "----------------------------------------";
                        dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                        AssetRow++;
                        foreach (DataRow dRow in DsetBalanceSheet.Tables[2].Rows)
                        {
                            if (dRow["Name"].ToString() == "Profit And Loss Account")
                            {
                                dgvReport.Rows.Add();
                                dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                                dgvReport.Rows[AssetRow].DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                                dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = dRow["Name"].ToString();
                                dgvReport.Rows[AssetRow].Cells["Amount1"].Value = Math.Round(decTotalProfitAndLoss + decCurrentProfitLoss, PublicVariables._inNoOfDecimalPlaces).ToString("N2");
                                dgvReport.Rows[AssetRow].Cells["GroupId1"].Value = dRow["ID"].ToString();
                                AssetRow++;
                            }
                        }
                        //-------------- Asset ---------------//
                        dgvReport.Rows.Add();
                        dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "Profit And Loss (Opening)";
                        dgvReport.Rows[AssetRow].Cells["Amount1"].Value = Math.Round(decTotalProfitAndLoss, PublicVariables._inNoOfDecimalPlaces).ToString("N2");
                        dgvReport.Rows[AssetRow].Cells["Amount1"].Style.ForeColor = Color.DarkSlateGray;
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Style.ForeColor = Color.DarkSlateGray;
                        AssetRow++;
                        //-------------- Asset ---------------//
                        dgvReport.Rows.Add();
                        dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "Current Period";
                        dgvReport.Rows[AssetRow].Cells["Amount1"].Value = Math.Round(decCurrentProfitLoss, PublicVariables._inNoOfDecimalPlaces).ToString("N2");
                        dgvReport.Rows[AssetRow].Cells["Amount1"].Style.ForeColor = Color.DarkSlateGray;
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Style.ForeColor = Color.DarkSlateGray;
                        AssetRow++;
                        TotalAssets = TotalAssets + (decCurrentProfitLoss + decTotalProfitAndLoss);
                        dgvReport.Rows.Add();
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "----------------------------------------";
                        dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                        AssetRow++;
                    }
                    else
                    {
                        dgvReport.Rows.Add();
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = "----------------------------------------";
                        dgvReport.Rows[LiabilityRow].DefaultCellStyle.Font = newFont;
                        LiabilityRow++;
                        foreach (DataRow dRow in DsetBalanceSheet.Tables[2].Rows)
                        {
                            if (dRow["Name"].ToString() == "Profit And Loss Account")
                            {
                                dgvReport.Rows.Add();
                                dgvReport.Rows[LiabilityRow].DefaultCellStyle.Font = newFont;
                                dgvReport.Rows[LiabilityRow].DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                                dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = dRow[1].ToString();
                                dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = Math.Round(decTotalProfitAndLoss + decCurrentProfitLoss, PublicVariables._inNoOfDecimalPlaces).ToString("N2");
                                dgvReport.Rows[LiabilityRow].Cells["GroupId2"].Value = dRow[0].ToString();
                                LiabilityRow++;
                            }
                        }
                        //------------ Liability ------------//
                        dgvReport.Rows.Add();
                        dgvReport.Rows[LiabilityRow].DefaultCellStyle.Font = newFont;
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = "Profit And Loss (Opening)";
                        dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = Math.Round(decTotalProfitAndLoss, inDecimalPlaces).ToString("N2");
                        dgvReport.Rows[LiabilityRow].Cells["Amount2"].Style.ForeColor = Color.DarkSlateGray;
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Style.ForeColor = Color.DarkSlateGray;
                        TotalLiabilities += decOpeningOfProfitAndLoss;
                        LiabilityRow++;
                        //------------ Liability ------------//
                        dgvReport.Rows.Add();
                        dgvReport.Rows[LiabilityRow].DefaultCellStyle.Font = newFont;
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = "Current Period";
                        dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = Math.Round(decCurrentProfitLoss, inDecimalPlaces).ToString("N2");
                        dgvReport.Rows[LiabilityRow].Cells["Amount2"].Style.ForeColor = Color.DarkSlateGray;
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Style.ForeColor = Color.DarkSlateGray;
                        TotalLiabilities = TotalLiabilities + (decCurrentProfitLoss + decTotalProfitAndLoss); //dcProfit;
                        LiabilityRow++;
                        dgvReport.Rows.Add();
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = "----------------------------------------";
                        dgvReport.Rows[LiabilityRow].DefaultCellStyle.Font = newFont;
                        LiabilityRow++;
                    }
                    if (AssetRow > LiabilityRow)
                    {
                        LiabilityRow = AssetRow;
                    }
                    else
                    {
                        AssetRow = LiabilityRow;
                    }
                    dgvReport.Rows.Add();
                    decimal dcDiffAsset = 0;
                    decimal dcDiffLiability = 0;

                    LiabilityRow++;
                    AssetRow++;

                    if (TotalAssets > TotalLiabilities)
                    {
                        //--------------- Liability exceeds so in asset side ----------------//
                        dgvReport.Rows.Add();
                        dgvReport.Rows[LiabilityRow].Cells["dgvtxtLiability"].Value = "Difference";
                        dgvReport.Rows[LiabilityRow].Cells["Amount2"].Value = Math.Round((TotalAssets - TotalLiabilities), inDecimalPlaces).ToString("N2");
                        dgvReport.Rows[LiabilityRow].DefaultCellStyle.Font = newFont;
                        dgvReport.Rows[LiabilityRow].DefaultCellStyle.ForeColor = Color.DarkRed;
                        dcDiffLiability = TotalAssets - TotalLiabilities;
                        LiabilityRow++;
                    }
                    else
                    {
                        //--------------- Asset exceeds so in liability side ----------------//
                        dgvReport.Rows.Add();
                        dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "Difference";
                        dgvReport.Rows[AssetRow].Cells["Amount1"].Value = Math.Round((TotalLiabilities - TotalAssets), inDecimalPlaces).ToString("N2");
                        dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                        dgvReport.Rows[AssetRow].DefaultCellStyle.ForeColor = Color.DarkRed;
                        dcDiffAsset = TotalLiabilities - TotalAssets;
                        AssetRow++;
                    }
                    if (AssetRow > LiabilityRow)
                    {
                        LiabilityRow = AssetRow;
                    }
                    else
                    {
                        AssetRow = LiabilityRow;
                    }
                    dgvReport.Rows.Add();
                    dgvReport.Rows.Add();
                    AssetRow++;
                    dgvReport.Rows[AssetRow].Cells["Amount1"].Value = "________________________";
                    dgvReport.Rows[AssetRow].Cells["Amount2"].Value = "________________________";
                    dgvReport.Rows.Add();
                    AssetRow++;
                    dgvReport.Rows[AssetRow].DefaultCellStyle.Font = newFont;
                    dgvReport.Rows[AssetRow].Cells["dgvtxtLiability"].Value = "Total";
                    dgvReport.Rows[AssetRow].Cells["dgvtxtAsset"].Value = "Total";
                    dgvReport.Rows[AssetRow].Cells["Amount1"].Value = Math.Round((TotalAssets + dcDiffAsset), inDecimalPlaces).ToString("N2");
                    dgvReport.Rows[AssetRow].Cells["Amount2"].Value = Math.Round((TotalLiabilities + dcDiffLiability), inDecimalPlaces).ToString("N2");
                    LiabilityRow++;
                    if (dgvReport.Columns.Count > 0)
                    {
                        dgvReport.Columns["Amount1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReport.Columns["Amount2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    decPrintOrNot = TotalAssets + dcDiffAsset;
                    decPrintOrNot1 = TotalLiabilities + dcDiffLiability;
                    if (inCurrenRowIndex >= 0 && dgvReport.Rows.Count > 0 && inCurrenRowIndex < dgvReport.Rows.Count)
                    {
                        if (dgvReport.Rows[inCurrenRowIndex].Cells[inCurentcolIndex].Visible)
                        {
                            dgvReport.CurrentCell = dgvReport.Rows[inCurrenRowIndex].Cells[inCurentcolIndex];
                        }
                        if (dgvReport.CurrentCell != null && dgvReport.CurrentCell.Visible)
                            dgvReport.CurrentCell.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS1:" + ex.Message;
            }
        }
        /// <summary>
        /// Function to create Datatables
        /// </summary>
        /// <returns></returns>
        public DataTable dtblBalanceSheet()
        {
            DataTable dtLocalC = new DataTable();
            try
            {
                dtLocalC.Columns.Add("Liability");
                dtLocalC.Columns.Add("Amount2");
                dtLocalC.Columns.Add("Asset");
                dtLocalC.Columns.Add("Amount1");
                DataRow drLocal = null;
                foreach (DataGridViewRow dr in dgvReport.Rows)
                {
                    drLocal = dtLocalC.NewRow();
                    drLocal["Liability"] = dr.Cells["dgvtxtLiability"].Value;
                    drLocal["Amount2"] = dr.Cells["Amount2"].Value;
                    drLocal["Asset"] = dr.Cells["dgvtxtAsset"].Value;
                    drLocal["Amount1"] = dr.Cells["Amount1"].Value;
                    dtLocalC.Rows.Add(drLocal);
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS2:" + ex.Message;
            }
            return dtLocalC;
        }
        /// <summary>
        /// Function to get dataset
        /// </summary>
        /// <returns></returns>
        public DataSet getdataset()
        {
            DataSet dsFundFlow = new DataSet();
            try
            {
                FinancialStatementSP spfinancial = new FinancialStatementSP();
                DataTable dtblFund = dtblBalanceSheet();
                DataTable dtblCompany = new DataTable();
                dtblCompany = spfinancial.FundFlowReportPrintCompany(1);
                dsFundFlow.Tables.Add(dtblFund);
                dsFundFlow.Tables.Add(dtblCompany);
                return dsFundFlow;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS3:" + ex.Message;
            }
            return dsFundFlow;
        }
        /// <summary>
        /// Function for Print
        /// </summary>
        /// <param name="toDate"></param>
        public void Print(DateTime toDate)
        {
            try
            {
                FinancialStatementSP spFinancial = new FinancialStatementSP();
                DataSet destBalance = getdataset();
                frmReport frmReport = new frmReport();
                frmReport.MdiParent = formMDI.MDIObj;
                frmReport.BalanceSheetReportPrint(destBalance);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS4:" + ex.Message;
            }
        }
        #endregion
        #region EVENTS
        /// <summary>
        /// On 'Search' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtToDate.Text != string.Empty)
                {
                    FillGrid();
                }
                else if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtCurrentDate.ToString();
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS5:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Clear' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString();
                dtpdate.MinDate = PublicVariables._dtFromDate;
                dtpdate.MaxDate = PublicVariables._dtToDate;
                dtpdate.Value = PublicVariables._dtToDate;
                txtToDate.Text = dtpdate.Value.ToString("dd-MMM-yyyy");
                FillGrid();
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS6:" + ex.Message;
            }
        }
        /// <summary>
        /// Date Validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpdate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateValidation objValidation = new DateValidation();
                objValidation.DateValidationFunction(txtToDate);
                if (txtToDate.Text == string.Empty)
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                DateTime dt;
                DateTime.TryParse(txtToDate.Text, out dt);
                dtpdate.Value = dt;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS7:" + ex.Message;
            }
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBalanceSheet_Load(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
                txtToDate.Text = PublicVariables._dtCurrentDate.ToString();
                isFormLoad = true;
                dtpdate.MinDate = PublicVariables._dtFromDate;
                dtpdate.MaxDate = PublicVariables._dtToDate;
                dtpdate.Value = PublicVariables._dtToDate;
                txtToDate.Text = dtpdate.Value.ToString("dd-MMM-yyyy");
                isFormLoad = false;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS8:" + ex.Message;
            }
        }
        /// <summary>
        /// Shows details of corresponding Ledgers on Cell double click in Datagirdview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvReport.CurrentRow.Index == e.RowIndex)
                {
                    if (dgvReport.CurrentCell != null)
                    {
                        if (dgvReport.CurrentCell.Value != null && dgvReport.CurrentCell.Value.ToString().Trim() != string.Empty)
                        {
                            int inRowIndex = dgvReport.CurrentCell.RowIndex;
                            int inColIndex = dgvReport.CurrentCell.ColumnIndex;
                            string strGroupId = string.Empty;
                            string strLedgerId = string.Empty;
                            if (dgvReport.Columns[inColIndex].Name == "dgvtxtAsset" || dgvReport.Columns[inColIndex].Name == "Amount1")
                            {
                                try
                                {
                                    if (Convert.ToDecimal(dgvReport.Rows[e.RowIndex].Cells["Amount1"].Value.ToString()) != 0)
                                        if (dgvReport.Rows[inRowIndex].Cells["ID1"].Value != null && dgvReport.Rows[inRowIndex].Cells["ID1"].Value.ToString() != string.Empty)
                                        {
                                            strLedgerId = dgvReport.Rows[inRowIndex].Cells["ID1"].Value.ToString();
                                            strGroupId = string.Empty;
                                        }
                                        else if (dgvReport.Rows[inRowIndex].Cells["GroupId1"].Value != null && dgvReport.Rows[inRowIndex].Cells["GroupId1"].Value.ToString() != string.Empty)
                                        {
                                            strGroupId = dgvReport.Rows[inRowIndex].Cells["GroupId1"].Value.ToString();
                                            strLedgerId = string.Empty;
                                        }
                                }
                                catch
                                {
                                    strGroupId = string.Empty;
                                    strLedgerId = string.Empty;
                                }
                            }
                            else if (dgvReport.Columns[inColIndex].Name == "dgvtxtLiability" || dgvReport.Columns[inColIndex].Name == "Amount2")
                            {
                                try
                                {
                                    if (Convert.ToDecimal(dgvReport.Rows[e.RowIndex].Cells["Amount2"].Value.ToString()) != 0)
                                        if (dgvReport.Rows[inRowIndex].Cells["ID2"].Value != null && dgvReport.Rows[inRowIndex].Cells["ID2"].Value.ToString() != string.Empty)
                                        {
                                            strLedgerId = dgvReport.Rows[inRowIndex].Cells["ID2"].Value.ToString();
                                            strGroupId = string.Empty;
                                        }
                                        else if (dgvReport.Rows[inRowIndex].Cells["GroupId2"].Value != null && dgvReport.Rows[inRowIndex].Cells["GroupId2"].Value.ToString() != string.Empty)
                                        {
                                            strGroupId = dgvReport.Rows[inRowIndex].Cells["GroupId2"].Value.ToString();
                                            strLedgerId = string.Empty;
                                        }
                                }
                                catch
                                {
                                    strGroupId = string.Empty;
                                    strLedgerId = string.Empty;
                                }
                            }
                            if (strLedgerId != string.Empty || strGroupId != string.Empty)
                            {
                                inCurrenRowIndex = inRowIndex;
                                frmAccountGroupwiseReport objForm1 = new frmAccountGroupwiseReport();
                                objForm1.WindowState = FormWindowState.Normal;
                                objForm1.MdiParent = formMDI.MDIObj;
                                objForm1.CallFromBalancesheet(dtfromdate.ToString(), txtToDate.Text, strGroupId, this);
                                this.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Enabled = true;

                formMDI.infoError.ErrorString = "BS9:" + ex.Message;
            }
        }
        /// <summary>
        /// Checks on Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBalanceSheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                isFormLoad = true;
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS10:" + ex.Message;
            }
        }
        /// <summary>
        /// On 'Print' button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (decPrintOrNot == 0 && decPrintOrNot1 == 0)
                {
                    MessageBox.Show("No Row To Print", "Oneaccount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    Print(PublicVariables._dtToDate);
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS11:" + ex.Message;
            }
        }
        /// <summary>
        /// Date validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtToDate.Text == string.Empty)
                {
                    txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    DateValidation objValidation = new DateValidation();
                    objValidation.DateValidationFunction(txtToDate);
                    if (txtToDate.Text == string.Empty)
                        txtToDate.Text = PublicVariables._dtToDate.ToString("dd-MMM-yyyy");
                    DateTime dt;
                    DateTime.TryParse(txtToDate.Text, out dt);
                    dtpdate.Value = dt;
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS12:" + ex.Message;
            }
        }
        /// <summary>
        /// Fills txtToDate textbox on dtpdate datetimepicker ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpdate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = this.dtpdate.Value;
                this.txtToDate.Text = date.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS13:" + ex.Message;
            }
        }
        #endregion
        #region NAVIGATION
        /// <summary>
        /// Escape key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBalanceSheet_KeyDown(object sender, KeyEventArgs e)
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
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS14:" + ex.Message;
            }
        }
        /// <summary>
        /// Back space navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    dgvReport.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS15:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnsearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS16:" + ex.Message;
            }
        }
        /// <summary>
        ///  Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnsearch.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    txtToDate.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS17:" + ex.Message;
            }
        }
        /// <summary>
        /// Enter key and backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnclear_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnPrint.Focus();
                }
                if (e.KeyCode == Keys.Back)
                {
                    btnsearch.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS18:" + ex.Message;
            }
        }
        /// <summary>
        /// Backspace navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    btnclear.Focus();
                }
            }
            catch (Exception ex)
            {
                formMDI.infoError.ErrorString = "BS19:" + ex.Message;
            }
        }
        #endregion
    }
}
