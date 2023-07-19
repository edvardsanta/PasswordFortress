using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PasswordFortress.Data;
using PasswordFortress.Models;

namespace PasswordFortress.Pages.Password
{
    public class EditModel : PageModel
    {
        private readonly PasswordFortress.Data.PasswordFortressContext _context;

        public EditModel(PasswordFortress.Data.PasswordFortressContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PasswordManagerModel PasswordManagerModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.PasswordManagerModel == null)
            {
                return NotFound();
            }

            var passwordmanagermodel =  await _context.PasswordManagerModel.FirstOrDefaultAsync(m => m.Id == id);
            if (passwordmanagermodel == null)
            {
                return NotFound();
            }
            PasswordManagerModel = passwordmanagermodel;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PasswordManagerModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PasswordManagerModelExists(PasswordManagerModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PasswordManagerModelExists(string id)
        {
          return (_context.PasswordManagerModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
