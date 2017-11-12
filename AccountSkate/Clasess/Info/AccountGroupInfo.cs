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
    class accountGroupInfo
    {
        private string name = "";
        private string under = "";
        private string narration = "";
        private string nature = "";
        private string affect_gross_profit = "";

        public string Name { get { return name; } set { name = value; } }
        public string Under { get { return under; } set { under = value; } }
        public string Narration { get { return narration; } set { narration = value; } }
        public string Nature { get { return nature; } set { nature = value; } }
        public string Affect_Gross_Profit { get { return affect_gross_profit; } set { affect_gross_profit = value; } }
        
    }
}
