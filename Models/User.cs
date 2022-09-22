using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuyU.Models
{
    public partial class User
    {


        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string? LastName { get; set; }
        [Required, MaxLength(50)]
        public decimal? Phone { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        [Required, PasswordPropertyText, MaxLength(50)]
        public string? Password { get; set; }
        public string? Image { get; set; }

        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
