using BuyU.ViewModel;
using BuyU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BuyU.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
           
            return View(await _roleManager.Roles.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", await _roleManager.Roles.ToListAsync());

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role Is Exists !");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            return RedirectToAction(nameof(Index));

        }
    }
}
