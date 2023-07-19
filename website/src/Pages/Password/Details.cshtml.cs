using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PasswordFortress.Data;
using PasswordFortress.Models;

namespace PasswordFortress.Pages.Password
{
    public class DetailsModel : PageModel
    {
        private readonly PasswordFortress.Data.PasswordFortressContext _context;

        public DetailsModel(PasswordFortress.Data.PasswordFortressContext context)
        {
            _context = context;
        }

      public PasswordManagerModel PasswordManagerModel { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.PasswordManagerModel == null)
            {
                return NotFound();
            }

            var passwordmanagermodel = await _context.PasswordManagerModel.FirstOrDefaultAsync(m => m.Id == id);
            if (passwordmanagermodel == null)
            {
                return NotFound();
            }
            else 
            {
                PasswordManagerModel = passwordmanagermodel;
            }
            return Page();
        }
    }
}
