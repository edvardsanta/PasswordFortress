using System.ComponentModel.DataAnnotations;

namespace PasswordFortress.Models
{
    public class PasswordManagerModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string PassPhrase { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

    }
}
