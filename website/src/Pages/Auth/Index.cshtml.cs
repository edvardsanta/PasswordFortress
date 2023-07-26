using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PasswordFortress.Data;
using PasswordFortress.Models;

namespace PasswordFortress.Pages.Auth
{
    public class IndexModel : PageModel
    {
        private readonly PasswordFortress.Data.UserAuthContext _context;

        public IndexModel(PasswordFortress.Data.UserAuthContext context)
        {
            _context = context;
        }
        [BindProperty]
        public UserAuth UserAuth { get; set; } = default!;

        public IActionResult OnGet()
        {
            if (User is not null && User.Identity is not null && User.Identity.IsAuthenticated)
            {
                // User is already logged in, redirect to another page
                return RedirectToPage("/Password/Index");
            }

            // If the user is not logged in, proceed with the normal page rendering
            return Page();
        }

        public async Task<IActionResult>  OnPostAsync()
        {
            await Task.Delay(1);
            if (ModelState.IsValid)
            {
                // Perform authentication logic
                // Access the username/email and password using Login.UsernameOrEmail and Login.Password

                // Example: Check if the credentials are valid
                if (IsValidCredentials(UserAuth.UsernameOrEmail, UserAuth.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UserAuth.UsernameOrEmail),
                        // Adicione outras claims relevantes do usuário aqui, se necessário
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    //HttpContext.Session.SetString("user_authenticated", "true");

                    return RedirectToPage("/Index"); // Redirecionar para a página inicial após o login bem-sucedido
                    // Successful login, redirect to a protected page
                    //return RedirectToPage("/ProtectedPage");
                }
                else
                {
                    // Invalid credentials, display an error message
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }



            // Redisplay the login page with validation errors
            return Page();
        }

        private bool IsValidCredentials(string usernameOrEmail, string password)
        {
            // Perform the necessary validation/authentication logic
            // Return true if the credentials are valid; otherwise, return false
            // Replace this with your actual implementation
            return (usernameOrEmail == "admin" && password == "password");
        }
    }
}
