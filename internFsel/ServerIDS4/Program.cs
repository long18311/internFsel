using IdentityModel;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServerIDS4;
using ServerIDS4.Controllers;
using ServerIDS4.Data;
using ServerIDS4.repositories.IRepon;
using ServerIDS4.repositories.Repon;
using System.IdentityModel.Tokens.Jwt;

var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}
var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = @"Server=LONG\SQLEXPRESS;Database=IdenData;Trusted_Connection=True;Integrated Security= SSPI";
if (seed)
{
    SeedData.EnsureSeedData(defaultConnString);
}
/*builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:5443";
        options.ApiName = "api";
    });*/
builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString,
       b => b.MigrationsAssembly(assembly)));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IAccUserRepon, AccUserRepon>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityServer().AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    }).AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
/*app.UseAuthentication();*/
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.MapGet("/", () => "Hello World!");

app.Run();
