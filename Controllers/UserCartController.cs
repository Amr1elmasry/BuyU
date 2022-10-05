using BuyU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BuyU.Controllers
{
    [Route("Cart/[action]")]
    [Authorize]
    public class UserCartController : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        public UserCartController(BuyUContext buyUContext , UserManager<ApplicationUser> userManager , IToastNotification toastNotification )
        {
            _userManager = userManager;
            _context = buyUContext;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> CartProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["userName"] = user.UserName;
            var userId = (await _userManager.GetUserIdAsync(user));
            //var cart = _context.Users.Carts.Where(c => c.UserId == userId);
            //var products = _context.Products.Where(p => cart.Any(c => c.ProductId == p.ProductId));
            var currUser = _context.Users.Include(u => u.Cart).FirstOrDefault(u => u.Id == userId);
            var cart = await _context.Carts.Include(p => p.Products).SingleOrDefaultAsync(c => c.UserId == currUser.Id);
            return View(cart.Products.ToList());
        }


        public async Task<IActionResult> AddToCart(int? id)
        {

            var user = await _userManager.GetUserAsync(User);

            var userId = (await _userManager.GetUserIdAsync(user));
            //_context.Carts.Add(new Cart { CartId = (int)user.CartId, ProductId = id, UserId = userId });
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            var currUser = _context.Users.Include(u=>u.Cart).FirstOrDefault(u=>u.Id==userId);
            if (currUser.Cart == null)
            {
                await _context.Carts.AddAsync(new Cart { UserId = currUser.Id, User = currUser });
                currUser.CartId = currUser.Cart.CartId;
                await _context.SaveChangesAsync();
            }
            //currUser.Cart.Include(c=>c.p).Products.Add(new Product { ProductId = product.ProductId, Brand = product.Brand, BrandId = product.BrandId, Description = product.Description, Name = product.Name, Price = product.Price });
            var cart = await _context.Carts.Include(p=>p.Products).SingleOrDefaultAsync(c => c.UserId==currUser.Id);
            cart.Products.Add(product);

            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Product added successfully");

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> RemoveProduct(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var userId = (await _userManager.GetUserIdAsync(user));
            //_context.Carts.Add(new Cart { CartId = (int)user.CartId, ProductId = id, UserId = userId });
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            var currUser = _context.Users.Include(u => u.Cart).FirstOrDefault(u => u.Id == userId);
            var cart = await _context.Carts.Include(p => p.Products).SingleOrDefaultAsync(c => c.UserId == currUser.Id);
            cart.Products.Remove(product);

            await _context.SaveChangesAsync();

            _toastNotification.AddSuccessToastMessage("Product removed successfully");

            return RedirectToAction("CartProducts");
        }
    }
}
