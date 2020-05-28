using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Pallet
    {
        [Key]
        public int PalletId { get; set; }
        public string Name { get; set; }
        public DateTime? PickedUpDT { get; set; }
        public int? CarrierId { get; set; }
        public int? CarrierServiceTypeId { get; set; }
        public int? PalletStatusId { get; set; }
    }
}
