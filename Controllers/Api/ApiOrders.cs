using BuyU.Classes;
using BuyU.Models;
using BuyU.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyU.Controllers.Api
{
    [Route("api/Orders/[action]")]
    [ApiController]
    public class ApiOrders : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiOrders(BuyUContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders([FromQuery] ParaID data)
        {
            var userId = data?.UserId;
            var user = await _context.Users.Include(o => o.Orders).SingleOrDefaultAsync(u => u.Id == userId);
            List<Order> orders = new List<Order>();
            if (user.Orders is not null || user.Orders.Any())
            {
                orders = await _context.Orders.Include(p => p.Products).Where(u => u.UserId == userId).ToListAsync();
            }
            else
            {
                return Ok("No orders are found");
            }

            var order = _context.Orders.Include(d => d.Products).Where(u => u.UserId == userId).Select(p => new MyOrdersViewModel
            {
                UserId = userId,
                Name = p.Name,
                Address = p.Address,
                Email = p.Email,
                Products = p.Products.ToList(),
                PhoneNumber = p.PhoneNumber,
                TotalPrice = (double)p.TotalPrice,

                Status = p.Status,

            }).ToList();
            //orders
            return Ok(order);

        }

        [HttpGet]
        public async Task<IActionResult> MakeOrder([FromQuery] ParaID data)
        {
            var userId = data?.UserId;
            var cart = await _context.Carts.Include(p => p.Products).Include(p => p.CartProduct).SingleOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || cart.CartProduct == null || cart.CartProduct.Count == 0 )
            {
                return NotFound("You don’t have any product in your cart");
            }

            double totalprice = (double)cart.CartProduct.Sum(p => p.Product.Price * p.Quantity);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var order = new OrderFormViewModel
            {
                cartProducts = cart.CartProduct,
                Name = user.FirstName + " " + user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                TotalPrice = totalprice,
                Status = "Under review"

            };

            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody]OrderFormViewModel model)
        {

            var userId = model.UserId;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var cart = await _context.Carts.Include(p => p.Products).Include(p => p.CartProduct).SingleOrDefaultAsync(c => c.UserId == userId);
            double totalprice = model.TotalPrice;
            var order = new Order { dateTime = DateTime.Now, TotalPrice = totalprice, User = user, UserId = userId, Address = model.Address, Email = model.Email, Name = model.Name, PhoneNumber = model.PhoneNumber, Products = cart.Products, Status = "Under review" };
            _context.Orders.Add(order);
            _context.SaveChanges();
            var UserOrder = await _context.Orders.Include(p => p.OrderDetails).SingleOrDefaultAsync(u => u.OrderId == order.OrderId);
            foreach (var item in cart.CartProduct)
            {
                var orderDetails = new OrderDetail { Product = item.Product, ProductId = item.ProductId, Order = order, OrderId = order.OrderId, Qty = item.Quantity };
                UserOrder.OrderDetails.Add(orderDetails);
            }
            _context.SaveChanges();
            var userCart = await _context.Carts.Include(c => c.Products).Where(u => u.UserId == userId).SingleOrDefaultAsync();
            userCart.Products.Clear();
            _context.Carts.Update(userCart);
            _context.SaveChanges();
            return Ok("Your Order Is Done");
        }

        
    }
}
