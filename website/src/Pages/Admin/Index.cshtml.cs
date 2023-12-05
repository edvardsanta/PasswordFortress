using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordFortressFront.Models;

namespace PasswordFortressFront.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public UserListViewModel UserList { get; set; }
        
        public IndexModel(UserListViewModel userList)
        {
            UserList = userList;
        }
        
        public void OnGet(UserListViewModel userList)
        {
            ViewData["users"] = userList;
        }
    }
}
