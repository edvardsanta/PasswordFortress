using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PasswordFortressFront.Models
{
    public class UserListViewModel
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<AppUser>? Users { get; set; }
    }
}
