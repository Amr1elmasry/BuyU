using BuyU.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BuyU.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route ("Products/[action]")]
    public class UserProductsController : Controller
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        

        public UserProductsController(BuyUContext buyUContext, UserManager<ApplicationUser> userManager , IToastNotification toastNotification)
        {
            _context = buyUContext;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        // GET: UserProductsController
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Home");

        }
        

        // GET: UserProductsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                ViewData["user"] = string.Empty;
            else
            {
                user = await CommonFunctions.UserIdAsync(_userManager, User);
                ViewData["user"] = user.Id;
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand).Include(c=>c.Carts)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            if (TempData["error"] as string == "add")
            {
                
                _toastNotification.AddSuccessToastMessage("Product added successfully");
            }
            else if (TempData["error"] as string == "true")
            {
                _toastNotification.AddAlertToastMessage("You already have this product");
            }
            return View(product);
        }

    }
}
