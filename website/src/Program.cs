using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PasswordFortressFront.Configurations;
using PasswordFortressFront.Configurations.IdentityPolicy;
using PasswordFortressFront.Data;
using PasswordFortressFront.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PasswordFortressFrontDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<PasswordFortressFrontDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure(IdentityConfiguration.ConfigureIdentityOptions());

// Authentication Cookie
builder.ConfigureCookie();

// Change login URL
builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Account/Login");

builder.Services.AddControllersWithViews();

// Validators
builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
