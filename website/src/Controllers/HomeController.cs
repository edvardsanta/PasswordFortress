using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordFortressFront.Models;
using System.Diagnostics;

namespace PasswordFortressFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
                              UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.GetUserAsync(HttpContext.User);
            string message = "Hello " + appUser.UserName;
            return View((object) message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}