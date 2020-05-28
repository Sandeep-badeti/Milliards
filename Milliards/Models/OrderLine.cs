using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class OrderLine
    {
        [Key]
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string UPC { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
