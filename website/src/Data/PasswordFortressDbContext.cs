using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PasswordFortressFront.Models;

namespace PasswordFortressFront.Data
{
    public class PasswordFortressDbContext : IdentityDbContext<AppUser>
    {
        public PasswordFortressDbContext(DbContextOptions<PasswordFortressDbContext> options) : base(options) { }
    }
}
