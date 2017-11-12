using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountSkate
{
    class ProductInfofroOpenAccount
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Group { get; set; }
        public decimal TaxId { get; set; }
        public decimal UnitId { get; set; }
        public string TaxApplicableOn { get; set; }
        public decimal GodownId { get; set; }
        public decimal RackId { get; set; }
        public bool AllowBatch { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal ReorderLevel { get; set; }
        public bool MultipleUnit { get; set; }
        public bool OpeningStock { get; set; }
        public decimal OpeningStockNumber { get; set; }
        public string Narration { get; set; }
        public decimal Size { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal MaximumStock { get; set; }
        public decimal MRP { get; set; }
        public decimal SalesRate { get; set; }
        public decimal ClosingRate { get; set; }
    }
}
