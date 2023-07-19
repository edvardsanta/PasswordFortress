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
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("UsetAuthContext") ?? throw new InvalidOperationException("Connection string 'UsetAuthContext' not found.")));

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
});
app.MapWhen(
       context => !context.User.Identity.IsAuthenticated,
       builder =>
       {
           builder.Use(async (context, next) =>
           {
               context.Response.Redirect("/Auth/Index");
               await next();
           });
       });

app.MapRazorPages();

app.Run();
