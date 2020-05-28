using System.ComponentModel.DataAnnotations;

namespace Milliards.Models
{
    public class CarrierServiceType
    {
        [Key]
        public int CarrierServiceTypeId { get; set; }
        public string Name { get; set; }
    }
}
