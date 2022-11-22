using BuyU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuyU.Controllers.Api
{
    [ApiController]
    [Route("api/User/[action]")]
    public class ApiUser : ControllerBase
    {
        private readonly BuyUContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiUser(BuyUContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginData data)
        {
            string? userName = data.userName;
            string? password = data.password;
            bool? rememberMe = data.rememberMe;
            var user = await _userManager.FindByEmailAsync(userName);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userName);


            }
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var result = await _signInManager.PasswordSignInAsync(user, password, (bool)rememberMe, false);
                if (result.Succeeded)
                {

                    return Ok(user.Id);
                }
                return NotFound("Username or password is incorrect");
            }
            return NotFound("Username or password is incorrect2");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Check your inputs");
            }
            string? firstName = data.FirstName;
            string? lastName = data.LastName;
            string? userName = data.UserName;
            string? eMail = data.Email;
            string? password = data.Password;
            if (!password.Equals(data.ConfirmPassword))
                return BadRequest("Passwords don’t match");
            var checkUserName = await _userManager.FindByNameAsync(userName);
            var checkEmail = await _userManager.FindByEmailAsync(eMail);
            if (checkUserName != null)
                return BadRequest("Username is already exists");
            if (checkEmail != null)
                return BadRequest("Email is already exists");
            var user = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = eMail,
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(user);
            }
            return BadRequest(result.Errors.Select(e => e.Description).ToList());



        }
    }
}