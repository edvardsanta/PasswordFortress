using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace PasswordFortress.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            //// Limpeza de cookies
            //await HttpContext.SignOutAsync();


            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Auth/Index");
            }

            return Page();
        }
    }
}