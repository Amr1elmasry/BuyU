using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyU.Models
{
    public class Order
    {
        [Required]
        public int OrderId { get; set; }
        [Required]

         [MaxLength(265)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }

        [DefaultValue("Under review")]
        public string? Status { get; set; }
        public double? TotalPrice { get; set; }

        [Required]
        public DateTime dateTime { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<Product>? products { get; set; } 

    }
}
