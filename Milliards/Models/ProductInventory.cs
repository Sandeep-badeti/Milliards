using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class ProductInventory
    {
        [Key]
        public int ProductInventoryId { get; set; }
        [ForeignKey("ProductVersionId")]
        public int ProductVersionId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public int StatusId { get; set; }
    }
}
