using BuyU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuyU.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ApiUsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApiUsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception();
            return Ok();
        }


    }
}
