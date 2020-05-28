using Microsoft.EntityFrameworkCore;
using Milliards.DTO;

namespace Milliards.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductTag>()
                .HasKey(e => new { e.TagId, e.ProductId });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Login> Login { get; set; }
        public DbSet<StagingOrders> StagingOrders { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<OrderDTO> OrderList { get; set; }
        public DbSet<CancelReason> CancelReason { get; set; }
        public DbSet<ChannelType> ChannelType { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<FulItem> FulItem { get; set; }
        public DbSet<OrderLineDTO> OrderLinesList { set; get; }
        public DbSet<RecordCount> RecordCount { get; set; }
        public DbSet<ErrorReason> ErrorReason { get; set; }
        public DbSet<FulfillmentOrderDTO> FulfillmentOrderList { set; get; }
        public DbSet<FulOrder> FulOrder { get; set; }
        public DbSet<FulOrderStatus> FulOrderStatus { get; set; }
        public DbSet<FulOrderTag> FulOrderTag { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<Pallet> Pallet { get; set; }
        public DbSet<PalletStatus> PalletStatus { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductInventory> ProductInventory { get; set; }
        public DbSet<ProductInventoryStatus> ProductInventoryStatus { get; set; }
        public DbSet<ProductSKU> ProductSKU { get; set; }
        public DbSet<ProductSKUStatus> ProductSKUStatus { get; set; }
        public DbSet<ProductSKUType> ProductSKUType { get; set; }
        public DbSet<ProductStatus> ProductStatus { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<ProductVersion> ProductVersion { get; set; }
        public DbSet<ProductDTO> ProductList { get; set; }
        public DbSet<ProductVersionStatus> ProductVersionStatus { get; set; }
        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatus { get; set; }
        public DbSet<Carrier> Carrier { get; set; }
        public DbSet<CarrierService> CarrierService { get; set; }
        public DbSet<CarrierServiceType> CarrierServiceType { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Box> Box { set; get; }
        public DbSet<DimensionUnit> DimensionUnit { set; get; }
        public DbSet<WeightUnit> WeightUnit { set; get; }
        public DbSet<Manufacturer> Manufacturer { set; get; }
        public DbSet<Condition> Condition { set; get; }
        public DbSet<Color> Color { set; get; }
        public DbSet<Tag> Tag { set; get; }
        public DbSet<FulfillDTO> fulfillList { get; set; }
        public DbSet<FulfillMode> FulfillMode { get; set; }
        public DbSet<OnHoldReason> OnHoldReason { get; set; }
        public DbSet<UpdateTrackingDTO> UpdateTrackingDTO { get; set; }
    }
}