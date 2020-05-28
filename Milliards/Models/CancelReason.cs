using System.ComponentModel.DataAnnotations;

namespace Milliards.Models
{
    public class CancelReason
    {
        [Key]
        public int CancelReasonId { get; set; }
        public string Name { get; set; }
    }
}
