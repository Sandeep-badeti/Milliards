using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class FulItem
    {
        [Key]
        public int FulItemId { get; set; }
        public int FulOrderId { get; set; }
        public int OrderLineId { get; set; }
        public string SKU { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? ProductVersionId { get; set; }
    }
}
