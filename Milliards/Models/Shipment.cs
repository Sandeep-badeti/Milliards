using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }
        public int FulOrderId { get; set; }
        public int ShipmentStatusId { get; set; }
        public DateTime StatusUpdatedDT { get; set; }
        public Guid? ShippingLabel { get; set; }
        public Guid? TrackingNumber { get; set; }
        public int CarrierId { get; set; }
        public DateTime? EstimatedArrivalDT { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public int? CarrierServiceId { get; set; }
    }
}