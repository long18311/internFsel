using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerIDS.Data;
using ServerIDS.repositories.IRepon;
using ServerIDS.repositories.Repon;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var defaultConnString = @"Server=LONG\SQLEXPRESS;Database=IdenData;Trusted_Connection=True;Integrated Security= SSPI";
var assembly = typeof(Program).Assembly.GetName().Name;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:5443";
        options.ApiName = "api";
    });
builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString,
       b => b.MigrationsAssembly(assembly)));
builder.Services.AddTransient<IAccUserRepon, AccUserRepon>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>().AddDefaultTokenProviders();
/*builder.Services.AddDbContext<ConfigurationDbContext>(options =>
    options.UseSqlServer(defaultConnString,
       b => b.MigrationsAssembly(assembly)));
builder.Services.AddDbContext<PersistedGrantDbContext>(options =>
    options.UseSqlServer(defaultConnString,
       b => b.MigrationsAssembly(assembly)));*/

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//.AddEntityFrameworkStores<AspNetIdentityDbContext>().AddDefaultTokenProviders();
/*builder.Services.AddIdentityServer().AddAspNetIdentity<IdentityUser>().AddConfigurationStore(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
}).AddOperationalStore(options =>
{
    options.ConfigureDbContext = b =>
    b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
});*/
builder.Services.AddAutoMapper(typeof(Program));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
/*app.UseIdentityServer();*/

app.Run();
