using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class FulOrder
    {
        [Key]
        public int FulOrderId { set; get; }
        public int OrderId { get; set; }
        public int FulOrderStatusId { get; set; }
        public int? CancelReasonId { get; set; }
        public int? AssignedWarehouseId { get; set; }
        public int? AssignedCarrierId { get; set; }
        public DateTime? CarrierUpdateDT { get; set; }
        public int? BoxId { get; set; }
        public int? AssignedCarrierServiceId { get; set; }
        public int? PickingBatchId { get; set; }
        public int? LabelBatchId { get; set; }
        public int? PalletId { get; set; }
        public int? OriginalFulOrderId { get; set; }
        public bool? OnHold_FLG { get; set; }
        public int? OnHoldReasonId { get; set; }
        public bool? Error_FLG { get; set; }
        public int? ErrorReasonId { get; set; }
        public bool? IsPrime_FLG { get; set; }
        public string CarrierDescription { get; set; }
        public DateTime? ShipByDT { get; set; }
        public DateTime? Assignment_DT { get; set; }
    }
}