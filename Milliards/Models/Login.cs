using System;
using System.ComponentModel.DataAnnotations;

namespace Milliards.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}
