using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PasswordFortress.Data;
using PasswordFortress.Models;

namespace PasswordFortress.Pages.Password
{
    public class CreateModel : PageModel
    {
        private readonly PasswordFortress.Data.PasswordFortressContext _context;

        public CreateModel(PasswordFortress.Data.PasswordFortressContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PasswordManagerModel PasswordManagerModel { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.PasswordManagerModel == null || PasswordManagerModel == null)
            {
                return Page();
            }

            _context.PasswordManagerModel.Add(PasswordManagerModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
