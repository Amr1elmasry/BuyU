using BuyU.Classes;
using BuyU.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyU.Controllers.Api
{
    [Route("api/Products/[action]")]
    [ApiController]
    public class ApiProducts : Controller
    {
        private readonly BuyUContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApiProducts(ILogger<HomeController> logger, BuyUContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] HomeFilters? filters)
        {
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
            if (filters.searchKey != null && filters.category == null)
                result = buyUContext.Where(s => s.Name.Contains(filters.searchKey));
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

            filters.pageSize = filters.pageSize != 0 ? filters.pageSize : 8;


            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> Details([FromQuery] int? id)
        {
            if (id == null)
                return BadRequest("No id found");
            var product = await _context.Products
                .Include(p => p.Brand).Include(c => c.Carts)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound("No Product found");
            }

            return Ok(product);
        }


        }
}
