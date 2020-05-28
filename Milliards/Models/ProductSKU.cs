using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class ProductSKU
    {
        [Key]
        public int ProductSKUId { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int SKUTypeId { get; set; }
        public int StatusId { get; set; }
        
        public virtual Product Product { get; set; }

        [ForeignKey("SKUTypeId")]
        public virtual ProductSKUType ProductSKUType { get; set; }

        [ForeignKey("StatusId")]
        public virtual ProductSKUStatus ProductSKUStatus { get; set; }
    }
}
