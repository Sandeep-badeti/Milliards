using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.FulItems = new HashSet<FulItem>();
            this.OrderLines = new HashSet<OrderLine>();
            this.ProductSKUs = new HashSet<ProductSKU>();
            this.ProductTags = new HashSet<ProductTag>();
            this.ProductVersions = new HashSet<ProductVersion>();
        }
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UPC { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public bool ShipsAlone_FLG { get; set; }
        public int StatusId { get; set; }
        public int WeightUnitId { get; set; }
        public int DimensionUnitId { get; set; }
        public Nullable<int> ColorId { get; set; }
        public int ConditionId { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public Nullable<int> BoxId { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FulItem> FulItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<ProductStatus> ProductStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSKU> ProductSKUs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
