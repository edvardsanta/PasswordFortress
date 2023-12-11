using Microsoft.AspNetCore.Identity;

namespace PasswordFortressFront.Configurations
{
    public static class IdentityConfiguration
    {
        public static Action<IdentityOptions> ConfigureIdentityOptions()
        {
            return options =>
            {
                // Password Options
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 1;

                // User Options
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            };
        }
    }
}
