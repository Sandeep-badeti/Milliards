using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class ProductVersion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductVersion()
        {
            this.FulItems = new HashSet<FulItem>();
        }
        [Key]
        public int ProductVersionId { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FulItem> FulItems { get; set; }
        public virtual Product Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
        [ForeignKey("StatusId")]
        public virtual ProductVersionStatus ProductVersionStatus { get; set; }
    }
}
