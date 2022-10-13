using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

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
        //public async Task<IActionResult> Index(string sortOrder,
        //        string currentFilter,
        //        string searchString,
        //        int? pageNumber)
        //{
        //    ViewData["CurrentSort"] = sortOrder;
        //    ViewData["brandsName"] = await _context.Brands.Select(b => b.BrandName).ToListAsync();
        //    if (searchString != null)
        //    {
        //        pageNumber = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //        ViewData["user"] = string.Empty;
        //    else
        //    {
        //        var userId = (await _userManager.GetUserIdAsync(user));
        //        ViewData["user"] = userId;
        //    }
        //    var result = Enumerable.Empty<Product>().AsQueryable();
        //    var buyUContext = _context.Products.Include(p => p.Brand).Include(c=>c.Carts);
        //    result = buyUContext.Where(p => true);    
        //    switch (sortOrder)
        //    {
        //        case "PriceHighTolow":
        //            result = result.OrderByDescending(p => p.Price);
        //            break;
        //        case "PricelowToHigh":
        //            result = result.OrderBy(p => p.Price);
        //            break;

        //        default:
        //            result = result;
        //            break;
        //    }
        //    int pageSize = 8;
        //    return View(await PaginatedList<Product>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));

        //}

        public async Task<IActionResult> Index(string sortOrder,
                int pageSize,
                string searchString,
                int? pageNumber,string? searchKey , string? category )
        {
            ViewData["searchKey"] = searchKey;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["category"] = category;
            ViewData["pageSize"] = pageSize != 0 ? pageSize : 8; ;
            ViewData["countOfprod"] = _context.Products.AsQueryable().Count();
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
            var buyUContext = _context.Products.Include(p => p.Brand).Include(c=>c.Carts);
            result = buyUContext.Where(p => true);
            if (searchKey != null && category == null)
                 result =  buyUContext.Where(s => s.Name.Contains(searchKey));
                //return View(await buyUContext.Where(s => s.Name.Contains((string)searchKey)).ToListAsync());
            else if (category!= null && searchKey==null)
                result =  buyUContext.Where(s => s.Brand.BrandName == category);
            else if (category!= null & searchKey!=null)
                result =  buyUContext.Where(s => s.Brand.BrandName == category && s.Name.Contains((string)searchKey));

            switch (sortOrder)
            {
                case "PriceHighTolow":
                    result = result.OrderByDescending(p => p.Price);
                    break;
                case "PricelowToHigh":
                    result = result.OrderBy(p => p.Price);
                    break;
                
                default:
                    result = result;
                    break;
            }
            pageSize = pageSize != 0 ? pageSize : 8;
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
