using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Box
    {
        public int BoxId { get; set; }
        public string Name { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public int DimensionUnitId { get; set; }
    }
}
