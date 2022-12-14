using BuyU.Controllers.Api;
using BuyU.Models;
using BuyU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using System.Xml.Serialization;

namespace BuyU.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Cart/[action]")]
    [Authorize]
    public class UserCartController : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        public UserCartController(BuyUContext buyUContext, UserManager<ApplicationUser> userManager, IToastNotification toastNotification , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = buyUContext;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> CartProducts()
        {
            var user = await CommonFunctions.UserIdAsync(_userManager, User);
            ViewData["userName"] = user.UserName;
            var cart = await _context.Carts.Include(p=>p.Products).Include(p=>p.CartProduct).SingleOrDefaultAsync(c => c.UserId == user.Id);
            if (cart == null)
                return View(new CartsViewModel());
            int totalprice = (int)cart.CartProduct.Sum(p => p.Product.Price * p.Quantity);
            ViewData["TotalPrice"] = totalprice;
            var cartsViewModel = new CartsViewModel();
            cartsViewModel.UserId = user.Id;
            cartsViewModel.cartProducts = cart.CartProduct;
            return View(cartsViewModel);
        }
        


        [HttpPost]
        public async Task<IActionResult> CartProductsAsync(CartsViewModel model)
        {
            var user = await CommonFunctions.UserIdAsync(_userManager, User);
            ViewData["userName"] = user.UserName;
            var cart = await _context.Carts.Include(p => p.Products).SingleOrDefaultAsync(c => c.UserId == user.Id);
            foreach (var item in cart.CartProduct)
            {
                item.Quantity = model.cartProducts.FirstOrDefault(i => i.ProductId==item.ProductId).Quantity;
                _context.Carts.Update(cart);
            }
            //cart.Products.SingleOrDefault(p => p.ProductId == product.ProductId).Quantity = product.Quantity;
            _context.SaveChanges();
            return RedirectToAction("Order", "UserOrder");
        }

        public async Task<IActionResult> AddToCart(string? ok , int id)
        {
            
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            var user = await CommonFunctions.UserIdAsync(_userManager, User);
            var product = _context.Products.Include(c=>c.CartProduct).FirstOrDefault(p => p.ProductId == id);
            user = _context.Users.Include(c => c.Cart).SingleOrDefault(u=>u.Id==user.Id);
            if (user.Cart == null)
            {
                var Newcart = new Cart { UserId = user.Id, User = user };
                await _context.Carts.AddAsync(new Cart { UserId = user.Id, User = user});
                await _context.SaveChangesAsync();
            }
            var check = CommonFunctions.AddToCartValidation(_context , id, 1);
            if (check == "success")
            {
                if (product.CartProduct.Any(p => p.CartId == user.CartId))
                {
                    TempData["error"] = "true";
                    
                    if (ok == "ok")
                    {
                        TempData["error"] = "true";
                        return RedirectToAction("Index", "Home");

                    }
                    else if (ok == "det")
                    {
                        TempData["error"] = "true";
                        return RedirectToAction("Details", "UserProducts", new { id = id });
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _context.CartProduct.Add(new CartProduct { CartId = user.Cart.CartId, ProductId = id, Quantity = 1, dateTime = DateTime.Now, Cart = user.Cart });
                    user.CartId = user.Cart.CartId;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    ViewData["userName"] = user.UserName;
                    TempData["error"] = "add";
                }
            }
            else
            {
                
                TempData["error"] = check;
                

            }
            
            

            //_toastNotification.AddSuccessToastMessage("Product added successfully");
            if (ok==null)
            {
                return Ok(check);
            }
            else if (ok=="ok")
            {
                TempData["error"] = "add";
                return RedirectToAction("Index", "Home");
                
            }
            else if(ok=="det")
            {
                TempData["error"] = "add";
                return RedirectToAction("Details", "UserProduct", new { id = id });
            }

            return RedirectToAction("Index" , "Home");

        }
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cartProduct = await _context.CartProduct.SingleOrDefaultAsync(c => c.ProductId == id && c.CartId == user.CartId);
            _context.CartProduct.Remove(cartProduct);
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Product removed successfully");
            return RedirectToAction("CartProducts");
        }


        public async Task<IActionResult> EmptyCart()
        {
            var user = await CommonFunctions.UserIdAsync(_userManager, User);
            var cart = await _context.Carts.Include(c=>c.CartProduct).SingleOrDefaultAsync(c =>c.UserId==user.Id);
            cart.CartProduct.Clear();
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            //_toastNotification.AddSuccessToastMessage("All Products are removed successfully");
            return RedirectToAction("CartProducts");
        }
    }
}
