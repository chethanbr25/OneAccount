//This is a source code or part of OpenAccount project
//Copyright (C) 2013 OpenAccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program. If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountI.DAL;
using System.Data;
using Entity;

namespace AccountI.BLL
{
    public class CallSP
    {
              
        public List<DataTable> GetListDetails(CategoryInfo infoCategory)
        {
            ListBoxFillSP spListBoxFill = new ListBoxFillSP();
            return spListBoxFill.GetList(infoCategory);
        }

        public List<DataTable> GetPieChartTableDetails(CategoryInfo infoCategory)
        {
            PieChartFillSP spPieChartFill = new PieChartFillSP();
            return spPieChartFill.GetPieChartTable(infoCategory);
        }

        public List<DataTable> GetPieChartDetails(CategoryInfo infoCategory)
        {
            PieChartFillSP spPieChartFill = new PieChartFillSP();
            return spPieChartFill.GetPieChart(infoCategory);
        }

        public List<DataTable> GetTopBarChartDetails()
        {
            BarChartFillSP spBarChartFill = new BarChartFillSP();
            return spBarChartFill.GetBarTopChart();
        }

        public List<DataTable> GetAccountGroupBarChartDetails()
        {
            BarChartFillSP spBarChartFill = new BarChartFillSP();
            return spBarChartFill.GetBarAccountGroupChart();
        }

        public List<DataTable> GetBarChartColorDetails(CategoryInfo CategoryInfo)
        {
            BarChartFillSP spBarChartFill = new BarChartFillSP();
            return spBarChartFill.GetBarColor(CategoryInfo);
        }
        public String GetCompanyNameDetails()
        {
            CompanyNameSP spCompanyName = new CompanyNameSP();
            return spCompanyName.GetCompanyName();
        }

        public DateTime GetFromDate()
        {
            FinancialDate spFinancialDate = new FinancialDate();
            return spFinancialDate.GetFromDate();
        }

        public DateTime GetToDate()
        {
            FinancialDate spFinancialDate = new FinancialDate();
            return spFinancialDate.GetToDate();
        }

        public DateTime GetLastDate()
        {
            GetLastDate spFinancialDate = new GetLastDate();
            return spFinancialDate.GetLastDay();
        }

        public List<DataTable> GetPlotterFill(CategoryInfo CategoryInfo)
        {
            PlotterFillSP spPlotterFill = new PlotterFillSP();
            return spPlotterFill.GetPlotterData(CategoryInfo);
        }
         public List<DataTable> GetBudgetNameFill()
        {
            GetBudgetNames spGetBudgetName = new GetBudgetNames();
            return spGetBudgetName.GetBudgetName();
        }

         public List<DataTable> GetViewDetailsFill()
         {
             ViewDetailsFillSP spDetailsFill = new ViewDetailsFillSP();
             return spDetailsFill.GetViewDetailsData();
         }

         public List<DataTable> GetCompanyName()
         {
             SelectCompany spDetailsFill = new SelectCompany();
             return spDetailsFill.GetCompanyNames();
         }
         public bool GetLoginCheck(string strUseName, string strPassword)
         {
             LoginCheck spLoginChecking = new LoginCheck();
             return spLoginChecking.CheckUserAndPassord(strUseName, strPassword);
         }

    }
}
