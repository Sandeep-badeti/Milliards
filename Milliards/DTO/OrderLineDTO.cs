using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.DTO
{
    public class OrderLineDTO
    {
        [Key]
        public int? OrderLineId { set; get; }
        public int? ProductID { set; get; }
        public string ProductName { set; get; }
        public string SKU { set; get; }
        public string UPC { set; get; }
        public bool ShipsAlone { set;get;}
        public decimal? UnitPrice { set; get; }
        public int? Quantity { set; get; }
    }
}
