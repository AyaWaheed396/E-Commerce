using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.UserService.Dto;

namespace Admin_Dashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                    if (!result.Succeeded || !await _userManager.IsInRoleAsync(user, "admin"))
                        return View(loginDto);

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginDto);
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");

        }
    }
}
