using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebFrontToBack.Models.Auth;
using WebFrontToBack.ViewModel.Auth;

namespace WebFrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            AppUser NewUser = new AppUser()
            {
                Fullname = registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };
            IdentityResult registerResult = await _userManager.CreateAsync(NewUser, registerVM.Password);
            if (!registerResult.Succeeded)
            {
                foreach (var error in registerResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            IdentityResult roleResult = await _userManager.AddToRoleAsync(NewUser, UserRoles.Admin.ToString());
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(login);
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult =await  _signInManager.CheckPasswordSignInAsync(user, login.Password,false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(login);
            }
           await _signInManager.SignInAsync(user, login.RememberMe);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        //public async Task<IActionResult> AddRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole {Name=role.ToString() }); 
        //        }
        //    }
        //    return Json("Ok");
        //}
        public IActionResult AccessDenied()
        {
            return View();
        }

        enum UserRoles
        {
            Admin,
            User,
            Moderator
        }

    }
}
