using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Warehouse
    {
        [Key]
        public int? WarehouseId { get; set; }
        public string Name { get; set; }
        public int WHStatusId { get; set; }
        public int TimezoneId { get; set; }
        public int ShipFromAddressId { get; set; }
        public DateTime ShippingCutoffTime { get; set; }
    }
}
