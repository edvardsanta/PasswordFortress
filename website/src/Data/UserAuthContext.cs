using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasswordFortress.Models;

namespace PasswordFortress.Data
{
    public class UserAuthContext : DbContext
    {
        public UserAuthContext (DbContextOptions<UserAuthContext> options)
            : base(options)
        {
        }

        public DbSet<PasswordFortress.Models.UserAuth> UserAuth { get; set; } = default!;
    }
}
