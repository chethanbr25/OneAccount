using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSkate
{
    class PriceLevelInfofroOpenAccount
    {
        public string PricingLevelName { get; set; }
        private string narration = "";

        public string Narration { get { return narration; } set { narration = value; } }
    }
}
