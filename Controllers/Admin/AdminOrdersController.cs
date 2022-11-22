using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BuyU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuyU.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    [Route("[controller]")]
    public class AdminOrdersController : Controller
    {
        private readonly BuyUContext _context;
        public AdminOrdersController(BuyUContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o=>o.OrderDetails)
                .ThenInclude(o=>o.Product).ToListAsync();

            return View(orders);
        }

        
    }
}