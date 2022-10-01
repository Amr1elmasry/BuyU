using BuyU.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyU.Controllers
{
    [Route ("Products/[action]")]
    public class UserProductsController : Controller
    {
        private readonly BuyUContext _context;

        public UserProductsController(BuyUContext buyUContext)
        {
            _context = buyUContext;
        }

        // GET: UserProductsController
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> Index(string? searchKey)
        {
            ViewData["Skey"] = searchKey;
            var buyUContext = _context.Products.Include(p => p.Brand);

            if (searchKey == null)
                return View(await buyUContext.ToListAsync());
            if (searchKey != null)
                return View(await buyUContext.Where(s => s.Name.Contains((string)searchKey)).ToListAsync());

            return View(await buyUContext.ToListAsync());

        }

        // GET: UserProductsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
