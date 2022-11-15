using BuyU.Models;
using BuyU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using System.Collections.Generic;
using System.Linq;
using static NuGet.Packaging.PackagingConstants;

namespace BuyU.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("MyOrders/[action]")]
    [Authorize]
    public class UserOrder : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        public UserOrder(BuyUContext context, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }


        [HttpGet]
        public async Task<IActionResult> Order()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["userName"] = user.UserName;
            var userId = user.Id;
            var cart = await _context.Carts.Include(p => p.Products).Include(p=>p.CartProduct).SingleOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || cart.CartProduct == null || cart.CartProduct.Count == 0)
            {
                _toastNotification.AddAlertToastMessage("You don’t have any products in you cart");
                return RedirectToAction("CartProducts", "UserCart");
            }
                
            double totalprice = (double)cart.CartProduct.Sum(p => p.Product.Price * p.Quantity);
            ViewData["TotalPrice"] = totalprice;
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
            if (User.IsInRole("Admin"))
            {
                _toastNotification.AddAlertToastMessage("There Is New Order");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Order(OrderFormViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["userName"] = user.UserName;
            var userId = user.Id;
            var cart = await _context.Carts.Include(p => p.Products).Include(p=>p.CartProduct).SingleOrDefaultAsync(c => c.UserId == userId);
            double totalprice = model.TotalPrice;
            var order = new Order { dateTime = DateTime.Now, TotalPrice = totalprice, User = user, UserId = userId, Address= model.Address , Email = model.Email , Name = model.Name , PhoneNumber = model.PhoneNumber , products = cart.Products , Status = "Under review"};
            _context.Orders.Add(order);
            _context.SaveChanges();
            var UserOrder = await _context.Orders.Include(p=>p.OrderDetails).SingleOrDefaultAsync(u => u.OrderId == order.OrderId);
            foreach (var item in cart.CartProduct)
            {
                var orderDetails =  new OrderDetail{ Product = item.Product, ProductId = item.ProductId, Order = order, OrderId = order.OrderId, Qty = item.Quantity };
                UserOrder.OrderDetails.Add(orderDetails);
            }
            _context.SaveChanges();
            var temporders = await _context.Orders.Include(p => p.products).Where(u => u.UserId == userId).ToListAsync();
            //cart.CartProduct.RemoveAll(u=>u.CartId==cart.CartId);
            ViewData["TotalPrice"] = totalprice;
            var userCart = await _context.Carts.Include(c => c.Products).Where(u=>u.UserId==userId).SingleOrDefaultAsync();
            userCart.Products.Clear();
            _context.Carts.Update(userCart);
            _context.SaveChanges();
            _toastNotification.AddAlertToastMessage("Your Order Is Done");
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["userName"] = user.UserName;
            var userId = user.Id;
            user = await _context.Users.Include(o => o.Orders).SingleOrDefaultAsync(u => u.Id == userId);
            List<Order> orders = new List<Order> ();
            if(user.Orders is not null || user.Orders.Any())
            {
                orders = await _context.Orders.Include(p=>p.products).Where(u=>u.UserId==userId).ToListAsync();
                //orders = user.Orders.OrderByDescending(d => d.dateTime).ToList();
            }
            else
            {
                return View(new MyOrdersViewModel());
            }

            var order = _context.Orders.Include(d => d.products).Where(u => u.UserId == userId).Select(p => new MyOrdersViewModel
            {
                UserId = userId,
                Name = p.Name,
                Address = p.Address,
                Email = p.Email,
                Products = p.products.ToList(),
                PhoneNumber = p.PhoneNumber,
                TotalPrice = (double)p.TotalPrice,
                
                Status = p.Status,

            }).ToList();
            //orders
            return View(order);

        }
    }
}
