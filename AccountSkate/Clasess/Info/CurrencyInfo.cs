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
    class currencyInfo
    {
        private string currencysymbol = "";
        private string narration = "";
        private string subunitname = "";

        public string CurrencyName { get; set; }
        public string CurrencySymbol { get { return currencysymbol; } set { currencysymbol = value; } }
        public string NoOfDecimalPlaces { get; set; }
        public string Narration { get { return narration; } set { narration = value; } }
        public string SubUnitName { get { return subunitname; } set { subunitname = value; } }
        
    }
}
