using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class ErrorReason
    {
        [Key]
        public int ErrorReasonId { get; set; }
        public string Name { get; set; }
    }
}
