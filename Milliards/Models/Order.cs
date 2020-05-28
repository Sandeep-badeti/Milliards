using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderLines = new HashSet<OrderLine>();
        }
        [Key]
        public int OrderId { get; set; }
        public int ChannelId { get; set; }
        public int OrderTypeId { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderNumber { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public System.DateTime OrderDT { get; set; }
        public System.DateTime ShipByDT { get; set; }
        public string RequiredShippingService { get; set; }
        public System.DateTime DeliverByDate { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<decimal> ShippingPaid { get; set; }
        public Nullable<decimal> TaxPaid { get; set; }
        public int BillToAddressId { get; set; }
        public Nullable<int> ShipToAddressId { get; set; }
        public string CustomerNotes { get; set; }
        public string InternalNotes { get; set; }
        public Nullable<int> BillToCarrierId { get; set; }
        public string BillToAccountNumber { get; set; }
        public string BillToAccountZip { get; set; }
        public string Warehouse { get; set; }
        public bool IsFulfilled { get; set; }
        public bool IsPrime_FLG { get; set; }
        public string CarrierDescription { get; set; }
        public virtual OrderType OrderType { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
