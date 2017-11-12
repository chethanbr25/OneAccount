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
using System.Text;    
//<summary>    
//Summary description for MaterialReceiptMasterInfo    
//</summary>    
namespace One_Account    
{    
class MaterialReceiptMasterInfo    
{    
    private decimal _materialReceiptMasterId;    
    private string _voucherNo;    
    private string _invoiceNo;    
    private decimal _suffixPrefixId;    
    private decimal _voucherTypeId;    
    private DateTime _date;    
    private decimal _ledgerId;    
    private decimal _orderMasterId;    
    private string _narration;
    private  decimal  _exchangeRateId;
    private decimal _totalAmount;    
    private decimal _userId;    
    private string _lrNo;    
    private string _transportationCompany;    
    private decimal _financialYearId;    
    private DateTime _extraDate;    
    private string _extra1;    
    private string _extra2;    
    
    public decimal MaterialReceiptMasterId    
    {    
        get { return _materialReceiptMasterId; }    
        set { _materialReceiptMasterId = value; }    
    }    
    public string VoucherNo    
    {    
        get { return _voucherNo; }    
        set { _voucherNo = value; }    
    }    
    public string InvoiceNo    
    {    
        get { return _invoiceNo; }    
        set { _invoiceNo = value; }    
    }    
    public decimal SuffixPrefixId    
    {    
        get { return _suffixPrefixId; }    
        set { _suffixPrefixId = value; }    
    }    
    public decimal VoucherTypeId    
    {    
        get { return _voucherTypeId; }    
        set { _voucherTypeId = value; }    
    }    
    public DateTime Date    
    {    
        get { return _date; }    
        set { _date = value; }    
    }    
    public decimal LedgerId    
    {    
        get { return _ledgerId; }    
        set { _ledgerId = value; }    
    }    
    public decimal OrderMasterId    
    {    
        get { return _orderMasterId; }    
        set { _orderMasterId = value; }    
    }    
    public string Narration    
    {    
        get { return _narration; }    
        set { _narration = value; }    
    }    
    public decimal exchangeRateId
    {
        get{ return  _exchangeRateId;}
        set
        {
            _exchangeRateId = value;
        }
    }
    public decimal TotalAmount    
    {    
        get { return _totalAmount; }    
        set { _totalAmount = value; }    
    }    
    public decimal UserId    
    {    
        get { return _userId; }    
        set { _userId = value; }    
    }    
    public string LrNo    
    {    
        get { return _lrNo; }    
        set { _lrNo = value; }    
    }    
    public string TransportationCompany    
    {    
        get { return _transportationCompany; }    
        set { _transportationCompany = value; }    
    }    
    public decimal FinancialYearId    
    {    
        get { return _financialYearId; }    
        set { _financialYearId = value; }    
    }    
    public DateTime ExtraDate    
    {    
        get { return _extraDate; }    
        set { _extraDate = value; }    
    }    
    public string Extra1    
    {    
        get { return _extra1; }    
        set { _extra1 = value; }    
    }    
    public string Extra2    
    {    
        get { return _extra2; }    
        set { _extra2 = value; }    
    }    
    
}    
}
