using System.ComponentModel.DataAnnotations;

namespace BuyU.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int Qty { get; set; }

        public Product? Product { get; set; }
        public Order? Order { get; set; }
    }
}