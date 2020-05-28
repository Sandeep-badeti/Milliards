using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class StagingOrders
    {
        [Key]
        public DateTime StartDate { get; set; }
        public string JSON { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
    }
}
