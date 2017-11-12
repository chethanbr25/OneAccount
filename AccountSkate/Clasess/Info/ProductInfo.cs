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
    class productInfo
    {
        private string productname;
        private string group = "Primary";
        private string tax = "NA";
        private string unit = "NA";
        private string taxapplicableon = "MRP";
        private string defaultgodown = "NA";
        private string rack = "NA";

        public string ProductCode { get; set; }
        public string ProductName { get { return productname; } set { productname = value; } }
        public string Group { get { return group; } set { group = value; } }
        public string Tax { get { return tax; } set { tax = value; } }
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                if (value == " Not Applicable")
                {
                    unit = "NA";
                }
                else
                {
                    unit = value;
                }
            }
        }
        public string TaxApplicableOn
        {
            get
            {
                return taxapplicableon;
            }
            set
            {
                if (value == " Not Applicable")
                {
                    taxapplicableon = "NA";
                }
                else
                {
                    taxapplicableon = value;
                }
            }
        }
        public string DefaultGodown
        {
            get
            {
                return defaultgodown;
            }
            set
            {
                if (value == " Not Applicable")
                {
                    defaultgodown = "NA";
                }
                else
                {
                    defaultgodown = value;
                }
            }
        }
        public string Rack
        {
            get
            {
                return rack;
            }
            set
            {
                if (value == " Not Applicable")
                {
                    rack = "NA";
                }
                else
                {
                    rack = value;
                }
            }
        }
        public string AllowBatch { get; set; }
        public string MinimumStock { get; set; }
        public string ConversionRate { get; set; }
        public string ReorderLevel { get; set; }
        public string MultipleUnit { get; set; }
        public string OpeningStock { get; set; }
        public string Narration { get; set; }
        public string Size { get; set; }
        public string PurchaseRate { get; set; }
        public string MaximumStock { get; set; }
        public string MRP { get; set; }
        public string SalesRate { get; set; }
        
    }
}
