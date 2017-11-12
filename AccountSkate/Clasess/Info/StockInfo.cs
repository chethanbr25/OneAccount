using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSkate
{
    class StockInfo
    {
        private string productname;
        private string unit = "NA";
        public string ProductName { get { return productname; } set { productname = value; } }
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
        public string OpeningStock { get; set; }
        public string OpeningStockNumber { get; set; }
        public string ClosingRate { get; set; }
        
    }
}
