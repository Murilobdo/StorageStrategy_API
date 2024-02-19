using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Models
{
    public class StockHistoryItemEntity
    {
        public int StockHistoryItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Taxing { get; set; }
    }
}
