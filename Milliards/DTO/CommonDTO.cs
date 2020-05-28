using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.DTO
{
    public class RecordCount
    {
        [Key]
        public int Count { get; set; }
    }
}
