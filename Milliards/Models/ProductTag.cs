using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class ProductTag
    {

        [Key]
        public int TagId { get; set; }

        [Key]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
