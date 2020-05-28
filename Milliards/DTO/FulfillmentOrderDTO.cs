using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.DTO
{
    public class FulfillmentOrderDTO
    {
        [Key]
        public int FulOrderId { set; get; }
        public string Status { set; get; }
        public int? AssignedWarehouseId { set; get; }
        public int? AssignedCarrierId { set; get; }
        public DateTime? CarrierUpdateDate { set; get; }
        public int? BoxId { set; get; }
        public int? NoofFulfillmentItems { set; get; }
    }

    public class FulfillWarehouse
    {
        [Key]
        public int? warehouseId { get; set; }
        public string name { get; set; }
    }
    public class FulfillCarrier
    {
        public int? carrierId { get; set; }
        public string name { get; set; }
    }
    public class FulfillCarrierService
    {
        [Key]
        public int carrierServiceId { get; set; }
        public string name { get; set; }
    }
    public class AssignFulWarehouse
    {
        public int fulOrderId { get; set; }
        public int warehouseId { get; set; }
        public int carrierId { get; set; }
        public int carrierServiceId { get; set; }
    }
    public class Modes
    {
        [Key]
        public int modeId { get; set; }
        public string modeName { get; set; }
    }
    public class FulfillDTO
    {
        [Key]
        public int fullOrderId { set; get; }
        public int orderId { set; get; }
        public string? warehouse { set; get; }
        public string? carrier { set; get; }
        public string? carrierservice { set; get; }
        public string? fulorderstatus { set; get; }
        public int? pickingbatchID { set; get; }
        public int? labelbatchID { set; get; }
        public int? palletID { set; get; }
        public int? nooffulitems { set; get; }
        public bool? onhold { get; set; }
        public int TotalRecs { get; set; }
    }
    public class FulfillOrderViewDTO
    {
        public int fulOrderId { get; set; }
        public int orderId { get; set; }
        public string fulOrderStatus { get; set; }
        public string cancelReason { get; set; }
        public string assignedWareHouse { get; set; }
        public string assignedCarrier { get; set; }
        public DateTime? carrierUpdateDate { get; set; }
        public string box { get; set; }
        public string assignedCarrierService { get; set; }
        public int? pickingBatchId { get; set; }
        public int? labelBatchId { get; set; }
        public string palletName { get; set; }
        public int? originalFulOrderId { get; set; }
        public string onHoldFlag { get; set; }
        public string onHoldReason { get; set; }
        public string errorFlag { get; set; }
        public string errorReason { get; set; }
        public string isPrime { get; set; }
        public string carrierDescription { get; set; }
        public DateTime? shipByDate { get; set; }
        public DateTime? assignmentDate { get; set; }
        public List<FulItemViewDTO> fullItemDetails { get; set; }
    }
    public class FulItemViewDTO
    {
        public int fulItemId { get; set; }
        public int orderLineId { get; set; }
        public string sku { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public int? productVersion { get; set; }
    }

    public class UpdateTrackingDTO
    {
        public string trackingNumber { get; set; }
        [Key]
        public int fulOrderId { get; set; }
        public string productName { get; set; }
        public string fulOrderStatus { get; set; }
        public string sku { get; set; }
    }

    public class CarrierServiceDTO
    {
        public int carrierId { get; set; }
        public string carrierName { get; set; }
        public int carrierServiceTypeId { get; set; }
    }

    public class PalletDTO
    {
        public int palletId { get; set; }
        public string name { get; set; }
    }
    public class CreatePalletDTO
    {
        public int carrierServiceTypeId { get; set; }
        public int carrierId { get; set; }
    }
    public class PalletizeDTO
    {
        public string trackingNumber { get; set; }
        public int palletId { get; set; }
    }
    public class ExitScanRequestDTO
    {
        public List<int> fulOrderIdList { get; set; }
        public int palletId { get; set; }
    }
}
