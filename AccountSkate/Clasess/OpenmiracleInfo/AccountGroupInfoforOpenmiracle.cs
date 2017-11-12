using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSkate
{
    class AccountGroupInfoforOpenAccount
    {
        private string name;
        private string narration = "";
        private string nature = "";
        private bool affect_gross_profit = true;

        public string Name { get { return name; } set { name = value; } }
        public decimal Under { get; set; }
        public string Narration { get { return narration; } set { narration = value; } }
        public string Nature { get { return nature; } set { nature = value; } }
        public bool Affect_Gross_Profit { get { return affect_gross_profit; } set { affect_gross_profit = value; } }
    }
}
