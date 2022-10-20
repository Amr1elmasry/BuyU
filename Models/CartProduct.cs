using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuyU.Models
{
    public class CartProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [Required]
        public DateTime dateTime { get; set; }
        [DefaultValue(1)]
        public int Quantity { get; set; }
    }
}
