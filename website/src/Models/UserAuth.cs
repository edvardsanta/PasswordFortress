using System.ComponentModel.DataAnnotations;

namespace PasswordFortress.Models
{
    public class UserAuth
    {
        public int Id { get; set; }
        [Required]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string .Empty;

        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
