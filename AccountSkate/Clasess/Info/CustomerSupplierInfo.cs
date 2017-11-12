using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSkate
{
    class CustomerSupplierInfo
    {
        private string customer_name = "";
        private string opening_balance = "";
        private string area = "NA";
        private string rout = "NA";
        private string pricinglevel = "NA";
        private string branch_name = "";
        private string mobile = "";
        private string address = "";
        private string bill_by_bill = "";
        private string credit_limit = "";
        private string cst = "";
        private string mailing_name = "";
        private string account_number = "";
        private string brach_code = "";
        private string phone = "";
        private string email = "";
        private string credit_period = "";
        private string tin = "";
        private string pan = "";
        private string narration = "";
        private string crordr = "Dr";

        public string Credit_Period { get { return credit_period; } set { credit_period = value; } }
        public string Credit_Limit { get { return credit_limit; } set { credit_limit = value; } }
        public string Bill_By_Bill { get { return bill_by_bill; } set { bill_by_bill = value; } }
        public string Opening_Balance { get { return opening_balance; } set { opening_balance = value; } }
        public string Name { get { return customer_name; } set { customer_name = value; } }
        public string Branch_Name { get { return branch_name; } set { branch_name = value; } }
        public string Mobile { get { return mobile; } set { mobile = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string CST { get { return cst; } set { cst = value; } }
        public string Area { get { return area; } set { area = value; } }
        public string Route { get { return rout; } set { rout = value; } }
        public string Mailing_Name { get { return mailing_name; } set { mailing_name = value; } }
        public string Account_Number { get { return account_number; } set { account_number = value; } }
        public string Brach_Code { get { return brach_code; } set { brach_code = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Pricing_Level { get { return pricinglevel; } set { pricinglevel = value; } }
        public string TIN { get { return tin; } set { tin = value; } }
        public string PAN { get { return pan; } set { pan = value; } }
        public string Narration { get { return narration; } set { narration = value; } }
        public string CrorDr { get { return crordr; } set { crordr = value; } }
    }
}
