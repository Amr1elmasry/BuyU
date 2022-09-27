using BuyU.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BuyU.ViewModel
{
    public class ProductForm
    {
        public int ProductId { get; set; }

        [Required, MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public int? Price { get; set; }
        public string? Photo { get; set; }
        public string? Color { get; set; }
        public string? BrandName { get; set; }
        public int? Quantity { get; set; }
        public int? DiscountId { get; set; }

        public IEnumerable<Brand> Brands { get; set; }
    }
}
