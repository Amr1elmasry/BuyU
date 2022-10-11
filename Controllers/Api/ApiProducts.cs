//using BuyU.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace BuyU.Controllers.Api
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ApiProducts : Controller
//    {
//        private readonly BuyUContext _context;
//        private readonly ILogger<HomeController> _logger;
//        private readonly UserManager<ApplicationUser> _userManager;
//        public ApiProducts(ILogger<HomeController> logger, BuyUContext context, UserManager<ApplicationUser> userManager)
//        {
//            _userManager = userManager;
//            _logger = logger;
//            _context = context;

//        }

//        [HttpPost]
//        public async Task<IActionResult> GetProducts()
//        {
//            var pageSize = int.Parse(Request.Form["length"]);
//            var skip = int.Parse(Request.Form["start"]);

//            var searchValue = Request.Form["search[value]"];

//            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
//            var sortColumnDirection = Request.Form["order[0][dir]"];

//            IQueryable<Product> products = _context.Products.Where(m => string.IsNullOrEmpty(searchValue)
//                ? true
//                : (m.Name.Contains(searchValue) || m.Brand.BrandName.Contains(searchValue) ));

//            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
//            {
//                products = products.OrderBy((string.Concat(sortColumn , " " , sortColumnDirection)));
//            }
//            var data = products.Skip(skip).Take(pageSize).ToList();

//            var recordsTotal = products.Count();

//            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };






//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//                ViewData["user"] = string.Empty;
//            else
//            {
//                var userId = await _userManager.GetUserIdAsync(user);
//                ViewData["user"] = userId;
//            }
//            var pageSize = int.Parse(Request.Form["length"]);
//            var skip = int.Parse(Request.Form["start"]);

//            var searchValue = Request.Form["search[value]"];

//            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
//            var sortColumnDirection = Request.Form["order[0][dir]"];
//            IQueryable<Product> customers = _context.Products.Where(m => string.IsNullOrEmpty(searchValue)
//               ? true
//               : (m.Name.Contains(searchValue) || m.Brand.BrandName.Contains(searchValue)));

//            var buyUContext = _context.Products.Include(p => p.Brand).Include(c => c.Carts);
//            var products = await buyUContext.OrderBy(p => p.ProductId).ToListAsync();

//            var recordsTotal = products.Count();
//            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = products };

//            return Ok(jsonData);
//        }
//    }
//}
