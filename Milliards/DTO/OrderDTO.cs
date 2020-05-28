using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.DTO
{
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; set; }
        public string Channel { get; set; }
        public string SKU { get; set; }
        public int? Quantity { set; get; }
        public string OrderType { set; get; }
        public string OrderNumber { set; get; }
        public DateTime OrderDate { set; get; }
        public DateTime ShipByDate { set; get; }
        public DateTime DeliverByDate { set; get; }
        public string OrderStatus { set; get; }
        public bool IsFulfilled { get; set; }
        public int TotalRecs { get; set; }
    }
}
