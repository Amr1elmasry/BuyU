using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        
    }
    
}
