using BuyU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyU.Controllers
{
    [Route("Brands/[action]")]
    public class UserBrandsController : Controller
    {
        private readonly BuyUContext _context;

        public UserBrandsController(BuyUContext buyUContext)
        {
            _context = buyUContext;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        public async Task<IActionResult> ProductsOfBrand(int id)
        {
            
            var brand = await _context.Brands.Include(p=> p.Products).SingleOrDefaultAsync(b => b.BrandId == id);
            
            //var products = await _context.Products.Where(p => p.BrandId == brand.BrandId).ToListAsync();
            ViewData["BrandName"] = brand.BrandName;
            var products = brand.Products.ToList();
            return View(products);
        }
    }
}
