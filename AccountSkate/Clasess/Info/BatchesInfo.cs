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
    class batchesInfo
    {
        private string productname;
        //private DateTime mfgdate = DateTime.Today;
        //private DateTime expirydate = DateTime.Today.AddDays(10);.
        private string mfgdate;
        private string expirydate;
        private string batchname = "NA";
        private string narration = "";

        public string ProductName { get { return productname; } set { productname = value; } }
        //public DateTime MfgDate { get { return mfgdate; } set { mfgdate = value; } }
        //public DateTime ExpiryDate { get { return expirydate; } set { expirydate = value; } }

        public string MfgDate { get { return mfgdate; } set { mfgdate = value; } }
        public string ExpiryDate { get { return expirydate; } set { expirydate = value; } }

        public string BatchName { get { return batchname; } set { batchname = value; } }
        public string Narration { get { return narration; } set { narration = value; } }
    }
}
