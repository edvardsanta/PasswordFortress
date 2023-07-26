using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordFortress.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<UserAuthContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("UserAuthContext") ?? throw new InvalidOperationException("Connection string 'UserAuthContext' not found.")));

//builder.Services.AddDbContext<PasswordFortressContext>(options =>

//    options.UseSqlServer(builder.Configuration.GetConnectionString("PasswordFortressContext") ?? throw new InvalidOperationException("Connection string 'PasswordFortressContext' not found.")));
builder.Services.AddDbContext<PasswordFortressContext>(options =>
   options.UseInMemoryDatabase(databaseName: "UserAuthDatabase"));

builder.Services.AddDbContext<UserAuthContext>(options =>
    options.UseInMemoryDatabase(databaseName: "UserAuth"));

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true; // Marcar o cookie como essencial
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None; // Configurar o SameSite
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = System.TimeSpan.Zero;

                options.LoginPath = "/Auth/Index"; // Página de login
                
                //options.AccessDeniedPath = "/Auth/AccessDenied"; // Página de acesso negado (opcional)
            });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    //endpoints.MapFallbackToPage("/Auth/Index");
});
//app.MapWhen(
//       context => !context.User.Identity.IsAuthenticated,
//       builder =>
//       {
//           builder.Use(async (context, next) =>
//           {
//               context.Response.Redirect("/Auth/Index");
//               await next();
//           });
//       });

app.MapRazorPages();

app.Run();
