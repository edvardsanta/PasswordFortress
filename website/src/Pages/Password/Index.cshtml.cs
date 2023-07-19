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
    public class IndexModel : PageModel
    {
        private readonly PasswordFortress.Data.PasswordFortressContext _context;

        public IndexModel(PasswordFortress.Data.PasswordFortressContext context)
        {
            _context = context;
        }

        public IList<PasswordManagerModel> PasswordManagerModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PasswordManagerModel != null)
            {
                PasswordManagerModel = await _context.PasswordManagerModel.ToListAsync();
            }
        }
    }
}
