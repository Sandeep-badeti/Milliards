using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Models
{
    public class Channel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Channel()
        {
            this.Orders = new HashSet<Order>();
        }
        [Key]
        public int ChannelId { get; set; }
        public int ChannelTypeId { get; set; }
        public string Name { get; set; }
        public string TimeZone { get; set; }
        public string Website { get; set; }

        public virtual ChannelType ChannelType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
