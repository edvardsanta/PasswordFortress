namespace PasswordFortressFront.Configurations
{
    public static class CookieConfiguration
    {
        public static void ConfigureCookie(this WebApplicationBuilder app)
        {
            app.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Identity.Application";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });
        }
    }
}
