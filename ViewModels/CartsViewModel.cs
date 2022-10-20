using BuyU.Models;

namespace BuyU.ViewModels
{
    public class CartsViewModel
    {
        public string UserId { get; set; }
        public List<CartProduct> cartProducts { get; set; }
    }
}
