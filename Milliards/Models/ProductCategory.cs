using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class ProductCategory
    {


        [Key]
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }

        [ForeignKey("Product")]
        public virtual int ProductId { get; set; }
        public virtual Product Products { get; set; }
    }
}
