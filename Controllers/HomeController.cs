using BuyU.Classes;
using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Diagnostics;
using System.Drawing.Printing;
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

        public async Task<IActionResult> Index([FromQuery] HomeFilters? filters)
        {
            if (filters.pageSize == 0 || filters.pageSize == null)
            {
                filters.pageSize = 8;
            }
            ViewData["searchKey"] = filters.searchKey;
            ViewData["CurrentSort"] = filters.sortOrder;
            ViewData["category"] = filters.category;
            ViewData["min"] = filters.min;
            ViewData["max"] = filters.max;
            ViewData["pageSize"] = filters.pageSize;
            ViewData["brandsName"] = await _context.Brands.Select(b => b.BrandName).ToListAsync();
            if (filters.searchKey != null)
            {
                filters.pageNumber = 1;
            }
            else
            {
                filters.searchKey = filters.searchKey;
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                ViewData["user"] = string.Empty;
            else
            {
                var userId = (await _userManager.GetUserIdAsync(user));
                ViewData["user"] = userId;
            }
            ViewData["Skey"] = filters.searchKey;
            ViewData["category"] = filters.category;

            var result = Enumerable.Empty<Product>().AsQueryable();
            var buyUContext = _context.Products.Include(p => p.Brand).Include(c => c.Carts);
            result = buyUContext.Where(p => true);
            //ViewData["maxPrice"] = result.Max(p => p.Price);
            //ViewData["minPrice"] = result.Min(p => p.Price);
            if (filters.searchKey != null && filters.category == null)
                result = buyUContext.Where(s => s.Name.Contains(filters.searchKey));
            //return View(await buyUContext.Where(s => s.Name.Contains((string)searchKey)).ToListAsync());
            else if (filters.category != null && filters.searchKey == null)
                result = buyUContext.Where(s => s.Brand.BrandName == filters.category);
            else if (filters.category != null & filters.searchKey != null)
                result = buyUContext.Where(s => s.Brand.BrandName == filters.category && s.Name.Contains((string)filters.searchKey));

            switch (filters.sortOrder)
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
            if (filters.min != null && filters.max == null)
                result = result.Where(p => p.Price >= filters.min);
            else if (filters.max != null && filters.min == null)
                result = result.Where(p => p.Price <= filters.max);
            else if (filters.min != null && filters.max != null)
                result = result.Where(p => p.Price >= filters.min && p.Price <= filters.max);
            ViewData["countOfprod"] = result.Count();

            
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

            return View(await PaginatedList<Product>.CreateAsync(result.AsNoTracking(), filters.pageNumber ?? 1, (int)filters.pageSize ));

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
