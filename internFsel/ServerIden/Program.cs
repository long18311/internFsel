using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerIden;
using ServerIden.Models;

var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AspNetIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AspNetIdentityDbContextConnection' not found.");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = @"Server=LONG\SQLEXPRESS;Database=IdenData;Trusted_Connection=True";
if (seed)
{
    SeedData.EnsureSeedData(defaultConnString);
}
builder.Services.AddDbContext<AspNetIdentityDbContext>(options => { options.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly)); });

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AspNetIdentityDbContext>();
builder.Services.AddIdentityServer().AddAspNetIdentity<IdentityUser>().AddConfigurationStore(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, b => b.MigrationsAssembly(assembly));
}).AddOperationalStore(options => options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly)))
.AddDeveloperSigningCredential();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseHttpsRedirection();
app.UseAuthentication();;
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.MapControllers();

app.Run();
