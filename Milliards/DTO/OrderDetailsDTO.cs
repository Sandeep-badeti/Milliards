using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.DTO
{
    public class OrderDetailsDTO
    {
        [Key]
        public int? OrderId { get; set; }
        public string Channel { get; set; }

        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNo { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipByDate { get; set; }
        public string ReqShipService { get; set; }
        public DateTime DeliverByDate { get; set; }

        public decimal? AmountPaid { get; set; }
        public decimal? ShippingPaid { get; set; }
        public decimal? TaxPaid { get; set; }
        public string BilltoAddress { get; set; }
        public string ShiptoAddress { get; set; }
        public string BilltoAcctNo { get; set; }
        public string InternalNotes { get; set; }
        public string BilltoCarrier { get; set; }
        public string CustomerNotes { get; set; }
        public string BilltoAcctZip { get; set; }
        public string Warehouse { get; set; }
    }
}
