using BuyU.Models;
using System.ComponentModel.DataAnnotations;

namespace BuyU.ViewModels
{
    public class OrderFormViewModel
    {
        
        public List<CartProduct> cartProducts { get; set; }
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
        public double TotalPrice { get; set; }
        public string? UserId { get; set; }


    }
}
