using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public int TagTypeId { get; set; }
    }
}
