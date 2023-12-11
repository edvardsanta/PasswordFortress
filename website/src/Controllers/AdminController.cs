using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordFortressFront.Models;

namespace PasswordFortressFront.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IUserValidator<AppUser> _userValidator;

        public AdminController(UserManager<AppUser> userMngr,
                               IPasswordHasher<AppUser> passwordHasher,
                               IPasswordValidator<AppUser> passwordValidator,
                               IUserValidator<AppUser> userValidator)
        {
            _userManager = userMngr;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _userValidator = userValidator;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        #region [Create]
        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = user.Name,
                    Email = user.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    Errors(result);
                }
            }
            
            return View();
        }
        #endregion

        #region [Update]
        public async Task<IActionResult> Update(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
                return View(appUser);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                IdentityResult? validEmail = null;
                if (!string.IsNullOrEmpty(email))
                {
                    validEmail = await _userValidator.ValidateAsync(_userManager, appUser);
                    if (validEmail.Succeeded)
                        appUser.Email = email;
                    else
                        Errors(validEmail);
                }   
                else
                    ModelState.AddModelError("", "Email cannot be empty.");

                IdentityResult? validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, appUser, password);
                    if (validPass.Succeeded)
                        appUser.PasswordHash = _passwordHasher.HashPassword(appUser, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty.");

                if (validEmail != null && validPass != null && validEmail.Succeeded && validPass.Succeeded)
                {
                    IdentityResult result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User not found.");

            return View(User);
        }
        #endregion

        #region [Delete]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User not found.");

            return View("Index", _userManager.Users);
        }
        #endregion

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
