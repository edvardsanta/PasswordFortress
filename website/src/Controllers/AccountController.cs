using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordFortressFront.Models;

namespace PasswordFortressFront.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userMngr, SignInManager<AppUser> signInMngr)
        {
            _userManager = userMngr;
            _signInManager = signInMngr;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            Login login = new();
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user: await _userManager.FindByEmailAsync(login.Email), password: login.Password, isPersistent: false, lockoutOnFailure: true);
                    if (result.Succeeded)
                        return Redirect("/");

                    //if (result.Succeeded)
                    //    return Redirect(login.ReturnUrl ?? "/");

                    //if (result.IsLockedOut)
                    //    ModelState.AddModelError("", "Your account is locked out. Kindly wait for 10 minutes and try again.");

                    //bool emailStatus = await userManager.IsEmailConfirmedAsync(appUser);
                    //if (emailStatus == false)
                    //{
                    //    ModelState.AddModelError(nameof(login.Email), "Email is unconfirmed, please confirm it first");
                    //}
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or Password");
            }
            return View(login);
        }
    }
}
