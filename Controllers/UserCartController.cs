﻿using BuyU.Models;
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
    [Route("Cart/[action]")]
    [Authorize]
    public class UserCartController : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        public UserCartController(BuyUContext buyUContext, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _userManager = userManager;
            _context = buyUContext;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> CartProducts()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["userName"] = user.UserName;
            var userId = user.Id;
            var cart = await _context.Carts.Include(p=>p.Products).Include(p=>p.CartProduct).SingleOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return View(new CartsViewModel());
            int totalprice = (int)cart.CartProduct.Sum(p => p.Product.Price * p.Quantity);
            ViewData["TotalPrice"] = totalprice;
            var cartsViewModel = new CartsViewModel();
            cartsViewModel.UserId = userId;
            cartsViewModel.cartProducts = cart.CartProduct;
            return View(cartsViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CartProductsAsync(CartsViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
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


        public async Task<IActionResult> AddToCart(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            user = _context.Users.Include(c => c.Cart).SingleOrDefault(u=>u.Id==user.Id);
            if (user.Cart == null)
            {
                var Newcart = new Cart { UserId = user.Id, User = user };
                await _context.Carts.AddAsync(new Cart { UserId = user.Id, User = user });
                user.CartId = Newcart.CartId;
                await _context.SaveChangesAsync();
            }
            _context.CartProduct.Add(new CartProduct { CartId = (int)user.CartId, ProductId = id, Quantity = 1, dateTime = DateTime.Now, Cart = user.Cart });
            await _context.SaveChangesAsync();
            ViewData["userName"] = user.UserName;
            _toastNotification.AddSuccessToastMessage("Product added successfully");

            return RedirectToAction("Index", "Home");

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
    }
}
