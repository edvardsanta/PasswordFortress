using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasswordFortress.Models;

namespace PasswordFortress.Data
{
    public class PasswordFortressContext : DbContext
    {
        public PasswordFortressContext (DbContextOptions<PasswordFortressContext> options)
            : base(options)
        {
        }

        public DbSet<PasswordFortress.Models.PasswordManagerModel> PasswordManagerModel { get; set; } = default!;
    }
}
