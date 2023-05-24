using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerIDS4;
using ServerIDS4.Data;
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
builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();
builder.Services.AddIdentityServer().AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    }).AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    /*.AddDeveloperSigningCredential()*/;
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
//app.UseAuthorization();
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});*/
app.MapGet("/", () => "Hello World!");

app.Run();
