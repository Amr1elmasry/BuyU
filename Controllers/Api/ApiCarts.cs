using BuyU.Classes;
using BuyU.Models;
using BuyU.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyU.Controllers.Api
{
    [Route("api/Carts/[action]")]
    [ApiController]
    public class ApiCarts : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiCarts(BuyUContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] ParaID? data)
        {
            if (data == null)
                return NotFound("No data are found");
            var id = data.UserId;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id of user must be not null !");
            }
            var cart = await _context.Carts.Include(p => p.Products).Include(p => p.CartProduct).SingleOrDefaultAsync(c => c.UserId == id);
            if (cart == null)
                return Ok(new CartsViewModel());
            int totalprice = (int)cart.CartProduct.Sum(p => p.Product.Price * p.Quantity);
            var cartsViewModel = new CartsViewModel();
            cartsViewModel.UserId = id;
            cartsViewModel.cartProducts = cart.CartProduct;
            cartsViewModel.TotalPrice = totalprice;
            return Ok(cartsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromForm] ParaID? data)
        {
            var UserId = data?.UserId;
            var ProductId = data?.ProductId;
            var userExists = await CommonFunctions.UserIsExists(_userManager, UserId);
            if (!userExists)
                return NotFound("No User Found");
            var product = _context.Products.Include(c => c.CartProduct).FirstOrDefault(p => p.ProductId == ProductId);
            var user = _context.Users.Include(c => c.Cart).SingleOrDefault(u => u.Id == UserId);
            if (user.CartId == null)
            {
                var Newcart = new Cart { UserId = user.Id, User = user };
                await _context.Carts.AddAsync(new Cart { UserId = user.Id, User = user });
                await _context.SaveChangesAsync();
            }
            var check = CommonFunctions.AddToCartValidation(_context, (int)ProductId, 1);
            if (check == "success")
            {
                if (product.CartProduct.Any(p => p.CartId == user.CartId))
                {
                    return NotFound("User Already has this product");
                }
                else
                {
                    _context.CartProduct.Add(new CartProduct { CartId = (int)user.CartId, ProductId = (int)ProductId, Quantity = 1, dateTime = DateTime.Now, Cart = user.Cart });
                    user.CartId = user.Cart.CartId;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                }
            }
            else
            {
                return NotFound(check);
            }





            return Ok(product);


        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromCart([FromForm] ParaID? data)
        {
            var UserId = data?.UserId;
            var ProductId = data?.ProductId;
            var userExists = await CommonFunctions.UserIsExists(_userManager, UserId);
            if (!userExists)
                return NotFound("No User Found");
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == UserId);
            var cartProduct = await _context.CartProduct.SingleOrDefaultAsync(c => c.ProductId == ProductId && c.CartId == user.CartId);
            _context.CartProduct.Remove(cartProduct);
            await _context.SaveChangesAsync();
            return Ok("Product removed successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> EmptyCart([FromForm] ParaID? data)
        {
            var UserId = data?.UserId;
            var userExists = await CommonFunctions.UserIsExists(_userManager, UserId);
            if (!userExists)
                return NotFound("No User Found");
            var cart = await _context.Carts.Include(c => c.CartProduct).SingleOrDefaultAsync(c => c.UserId == UserId);
            if (cart == null)
                return NotFound("User has no cart!");
            cart.CartProduct.Clear();
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return Ok("Now your cart is empty");
        }
    }
}
