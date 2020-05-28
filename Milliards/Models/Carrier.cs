using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Carrier
    {
        [Key]
        public int CarrierId { get; set; }
        public string Name { get; set; }
    }
}
