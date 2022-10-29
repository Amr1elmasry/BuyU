using BuyU.Classes;
using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Diagnostics;
using System.Linq;
namespace BuyU.Controllers
{

    public class HomeController : Controller
    {
        private readonly BuyUContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        public HomeController(ILogger<HomeController> logger, BuyUContext context , UserManager<ApplicationUser> userManager, IToastNotification toastNotification )
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _logger = logger;
            _context = context;
    
        }

        public async Task<IActionResult> Index(string sortOrder,
                int pageSize,
                string searchString,
                int? min, int? max,
                int? pageNumber, string? searchKey, string? category)
        {
            ViewData["searchKey"] = searchKey;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["category"] = category;
            ViewData["min"] = min;
            ViewData["max"] = max;
            ViewData["pageSize"] = pageSize != 0 ? pageSize : 8; ;
            ViewData["brandsName"] = await _context.Brands.Select(b => b.BrandName).ToListAsync();
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = searchKey;
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                ViewData["user"] = string.Empty;
            else
            {
                var userId = (await _userManager.GetUserIdAsync(user));
                ViewData["user"] = userId;
            }
            ViewData["Skey"] = searchKey;
            ViewData["category"] = category;

            var result = Enumerable.Empty<Product>().AsQueryable();
            var buyUContext = _context.Products.Include(p => p.Brand).Include(c => c.Carts);
            result = buyUContext.Where(p => true);
            //ViewData["maxPrice"] = result.Max(p => p.Price);
            //ViewData["minPrice"] = result.Min(p => p.Price);
            if (searchKey != null && category == null)
                result = buyUContext.Where(s => s.Name.Contains(searchKey));
            //return View(await buyUContext.Where(s => s.Name.Contains((string)searchKey)).ToListAsync());
            else if (category != null && searchKey == null)
                result = buyUContext.Where(s => s.Brand.BrandName == category);
            else if (category != null & searchKey != null)
                result = buyUContext.Where(s => s.Brand.BrandName == category && s.Name.Contains((string)searchKey));

            switch (sortOrder)
            {
                case "PriceHighTolow":
                    result = result.OrderByDescending(p => p.Price);
                    break;
                case "PricelowToHigh":
                    result = result.OrderBy(p => p.Price);
                    break;

                default:
                    break;
            }
            if (min != null && max == null)
                result = result.Where(p => p.Price >= min);
            else if (max != null && min == null)
                result = result.Where(p => p.Price <= max);
            else if (min != null && max != null)
                result = result.Where(p => p.Price >= min && p.Price <= max);
            ViewData["countOfprod"] = result.Count();

            pageSize = pageSize != 0 ? pageSize : 8;
            if (TempData["error"] as string == "true")
            {
                _toastNotification.AddAlertToastMessage("You already have this product");
            }
            else if (TempData["error"] as string == "add")
            {
                _toastNotification.AddSuccessToastMessage("Product added successfully");
            }
            else if (TempData["error"] as string != null)
            {
                _toastNotification.AddErrorToastMessage(TempData["error"] as string);
            }

            return View(await PaginatedList<Product>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));

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
