using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class CarrierService
    {
        [Key]
        public int CarrierServiceId { get; set; }
        public string Name { get; set; }
        public bool FulOrderAutoHold_FLG { get; set; }
        public int CarrierServiceTypeId { get; set; }
        public int CarrierId { get; set; }
    }
}