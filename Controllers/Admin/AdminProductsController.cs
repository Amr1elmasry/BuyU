using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuyU.Models;
using System.Collections;
using BuyU.ViewModel;
using Microsoft.AspNetCore.Authorization;
using BuyU.Classes;
using Microsoft.AspNetCore.Identity;

namespace BuyU.Controllers.Admin
{
    [Authorize(Roles = "Admin")]

    public class AdminProductsController : Controller
    {
        private readonly BuyUContext _context;
        public List<string> Colors = new List<string>
        {
            "Black" , "White"  ,  "Red", "Blue" , "Gray" , "Selver" , "Gold"
        };


        public AdminProductsController(BuyUContext context)
        {
            _context = context;
        }

        // GET: AdminProducts
        
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


            return View(await PaginatedList<Product>.CreateAsync(result.AsNoTracking(), filters.pageNumber ?? 1, (int)filters.pageSize));

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
        // GET: AdminProducts/Details/5
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

        // GET: AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["BrandName"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["Color"] = new SelectList(Colors, "Color");

            return View();
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            //var braid = await _context.Brands.FirstOrDefaultAsync(b => b.BrandName == productmodel.BrandName);
            //var product = new Product
            //{
            //    Name = productmodel.Name,
            //    Description = productmodel.Description,
            //    Price = productmodel.Price,
            //    Photo = productmodel.Photo,
            //    Color = productmodel.Color,
            //    BrandId = braid.BrandId,
            //    Quantity = productmodel.Quantity,
            //    DiscountId = productmodel.DiscountId
            //};

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }


            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandName"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["Color"] = new SelectList(Colors, "Color");
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Photo,Color,BrandId,Quantity,DiscountId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            //ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatId", product.CatId);
            //return View(product);
        }

        // GET: AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'BuyUContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
