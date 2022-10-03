using BuyU.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BuyU.Controllers
{
    public class HomeController : Controller
    {
        private readonly BuyUContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, BuyUContext context)
        {
            _logger = logger;
            _context = context;
    
        }


        // GET: UserProductsController
        public async Task<IActionResult> Index()
        {
            var buyUContext = _context.Products.Include(p => p.Brand);
            return View(await buyUContext.OrderBy(p=>p.ProductId).ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> Index(string? searchKey )
        {
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

    }
}
