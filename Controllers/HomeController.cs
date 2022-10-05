using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BuyU.Controllers
{
    public class HomeController : Controller
    {
        private readonly BuyUContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, BuyUContext context , UserManager<ApplicationUser> userManager )
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
    
        }


        // GET: UserProductsController
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                ViewData["user"] = string.Empty;
            else
            {
                var userId = (await _userManager.GetUserIdAsync(user));
                ViewData["user"] = userId;
            }

            var buyUContext = _context.Products.Include(p => p.Brand).Include(c=>c.Carts);
            return View(await buyUContext.OrderBy(p=>p.ProductId).ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> Index(string? searchKey )
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                ViewData["user"] = string.Empty;
            else
            {
                var userId = (await _userManager.GetUserIdAsync(user));
                ViewData["user"] = userId;
            }
            ViewData["Skey"] = searchKey;
            var buyUContext = _context.Products.Include(p => p.Brand);
            if (searchKey != null)
                return View(await buyUContext.Where(s => s.Name.Contains((string)searchKey)).ToListAsync());

            return View(await buyUContext.ToListAsync());

        }


        public ActionResult Details(int id)
        {
            return RedirectToAction("Details","UserProduct",new {id = id});
        }
        
        public ActionResult ChatWithAdmin()
        {
            return View();
        }


    }
}
