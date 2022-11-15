using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BuyU.Controllers
{
    public static class CommonFunctions
    {
        public static async Task<ApplicationUser> UserIdAsync(UserManager<ApplicationUser> _userManager , ClaimsPrincipal User)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user!=null)
                return (user);
            return (null);
        }
        public static bool CheckSession(ClaimsPrincipal User)
        {
            return User.Identity.IsAuthenticated;
        }

        public static async Task<bool> UserIsExists (UserManager<ApplicationUser> _userManager , string id)
        {
            if(string.IsNullOrEmpty(id))
                return false;
            return (await _userManager.FindByIdAsync(id)).UserName.Any();
        }


        public static string AddToCartValidation(BuyUContext _context , int Productid, int ProductQty)
        {
            string massage = "success";
            var product = _context.Products.Include(c => c.CartProduct).FirstOrDefault(p => p.ProductId == Productid);

            if (product != null)
            {
                if (ProductQty == 0) massage = "Quantity of product cannot be zero!";
                else if (ProductQty < 0) massage = "Not eligible to add negative qty";
                else if (product.Quantity == 0) massage = "This product is out of stock!";
                else if (product.Quantity < ProductQty) massage = "The quantity of this product less than your request!";
            }
            else
            {
                massage = "Product is not found";
            }
            return massage;
        }

    }
    
}
