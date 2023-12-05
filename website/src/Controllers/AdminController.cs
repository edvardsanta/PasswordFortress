using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordFortressFront.Models;

namespace PasswordFortressFront.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;
        private IUserValidator<AppUser> _userValidator;
        private ILogger<AdminController> _logger;

        public AdminController(UserManager<AppUser> userManager,
                               IPasswordHasher<AppUser> passwordHasher,
                               IPasswordValidator<AppUser> passwordValidator,
                               IUserValidator<AppUser> userValidator,
                               ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _userValidator = userValidator;
            _logger = logger;
        }

        // GET: /Admin
        public IActionResult Index()
        {
            //var viewModel = "teste";
            var viewModel = new UserListViewModel
            {
                Users = _userManager.Users.ToList()
            };
            ViewData["Title"] = "Registered Users";
            return View("Pages/Admin/Index.cshtml", viewModel);
        }

        public ViewResult Create() => View();

        [HttpPost]
        [AllowAnonymous]
        [Route("Admin/Create")]
        public async Task<IActionResult> Create(User user)
        {
            _logger.LogInformation("Create checkpoint");
            
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = user.Name,
                    Email = user.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
            }

            return View(user);
        }

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
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult? validEmail = null;
                if (!string.IsNullOrEmpty(email))
                {
                    validEmail = await _userValidator.ValidateAsync(_userManager, user);
                    if (validEmail.Succeeded)
                        user.Email = email;
                    else
                        Errors(validEmail);
                }
                else
                    ModelState.AddModelError(string.Empty, "Email cannot be empty.");

                IdentityResult? validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError(string.Empty, "Password cannot be empty.");

                if (validEmail != null && validPass != null)
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError(string.Empty, "User not found");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError(string.Empty, "User not found;");

            return View("Index", _userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        
    }
}
