using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PasswordFortressFront.Models;

namespace PasswordFortressFront.Data
{
    public class PasswordFortressFrontDbContext : IdentityDbContext<AppUser>
    {
        public PasswordFortressFrontDbContext(DbContextOptions<PasswordFortressFrontDbContext> options) : base(options) { }
    }
}
