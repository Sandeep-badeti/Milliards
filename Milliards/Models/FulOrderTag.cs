using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class FulOrderTag
    {
        [Key]
        public int FulOrderId { get; set; }
        public int TagId { get; set; }
        public virtual FulOrder FulOrder { get; set; }
    }
}
