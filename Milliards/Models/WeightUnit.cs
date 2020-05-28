using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class WeightUnit
    {
        public int WeightUnitId { get; set; }
        public string Name { get; set; }
        public decimal CoversionRate { get; set; }
    }
}
