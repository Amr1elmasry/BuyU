using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BuyU.Models
{
    public class ApplicationUser : IdentityUser
    {


        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public byte[]? Image { get; set; }
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; } 

    }
}
