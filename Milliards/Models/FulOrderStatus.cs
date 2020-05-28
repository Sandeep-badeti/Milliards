using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class FulOrderStatus
    {
        [Key]
        public int FulOrderStatusId { get; set; }
        public string Name { get; set; }
    }
}
