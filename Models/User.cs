using System;
using System.Collections.Generic;

namespace BuyU.Models
{
    public partial class User
    {

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public decimal? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
        public byte[]? CreatedAt { get; set; }

        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
