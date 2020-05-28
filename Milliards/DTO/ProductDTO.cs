using Milliards.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.DTO
{
    public class ProductDTO
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string UPC { set; get; }
        public string Description { set; get; }
        public string Status { set; get; }
        public int NoofVersion { set; get; }
        public int Inventory { set; get; }
        public string Category { set; get; }
        public bool ShipAlone { set; get; }
        public string Manufacturer { get; set; }
        public int TotalRecs { get; set; }
    }
    public class ProductDetailsDTO
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainSKU { get; set; }
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
        public virtual ICollection<ProductSKU> ProductSku { get; set; }
        public virtual ICollection<ProductVersion> ProductVersion { get; set; }
        public virtual ICollection<ProductInventory> ProductInventory { get; set; }
        public virtual ICollection<ProductTag> ProductTag { get; set; }
        // public int TagId { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<FulItem> FulItems { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<OrderLine> OrderLines { get; set; }
        //public virtual ProductStatus ProductStatu { get; set; }
        //public virtual ProductCategory ProductCategory { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductSKU> ProductSKUs { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductTag> ProductTags { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
    public class ProductViewDTO
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainSKU { get; set; }
        public string UPC { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public bool ShipsAlone_FLG { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int WeightUnitId { get; set; }
        public string WeightUnitName { get; set; }
        public int DimensionUnitId { get; set; }
        public string DimensionName { get; set; }
        public Nullable<int> ColorId { get; set; }
        public string ColorName { get; set; }
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> BoxId { get; set; }
        public string BoxName { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public virtual ICollection<ProductSkuDTO> ProductSku { get; set; }
        public virtual ICollection<ProductVersionDTO> ProductVersion { get; set; }
        public virtual ICollection<ProductInventoryDTO> ProductInventory { get; set; }
        public virtual ICollection<ProductTagDTO> ProductTag { get; set; }
    }
    public class ProductSkuDTO
    {
        public int ProductSKUId { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int SKUTypeId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

    }
    public class ProductVersionDTO
    {
        public int ProductVersionId { get; set; }
        public int ProductId { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }

    }
    public class ProductInventoryDTO
    {
        [Key]
        public int ProductInventoryId { get; set; }
        public int ProductVersionId { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

    }
    public class ProductTagDTO
    {
        public int ProductId { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
