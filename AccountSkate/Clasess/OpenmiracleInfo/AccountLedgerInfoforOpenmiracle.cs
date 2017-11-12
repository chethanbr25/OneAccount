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
//along with this program. If not, see <http://www.gnu.org/licenses/>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSkate 
{
    class AccountLedgerInfoforOpenAccount
    {
        private decimal area = 1;
        private decimal rout = 1;
        private decimal pricinglevelId = 1;
        public decimal AccountGroupId { get; set; }
        public string Name { get; set; }
        public decimal Opening_Balance { get; set; }
        public string Branch_Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public bool Bill_By_Bill { get; set; }
        public decimal Credit_Limit { get; set; }
        public string CST { get; set; }
        public decimal Area_ID
        {
            get
            {
                return area;
            }
            set
            {
                area = value;
            }
        }
        public decimal Route_ID
        {
            get
            {
                return rout;
            }
            set
            {
                rout = value;
            }
        }
        public string Mailing_Name { get; set; }
        public string Account_Number { get; set; }
        public string Brach_Code { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Pricing_Level_ID
        {
            get
            {
                return pricinglevelId;
            }
            set
            {
                pricinglevelId = value;
            }
        }
        public decimal Credit_Period { get; set; }
        public string TIN { get; set; }
        public string PAN { get; set; }
        public string Narration { get; set; }
        public string CrorDr { get; set; }
    }
}
