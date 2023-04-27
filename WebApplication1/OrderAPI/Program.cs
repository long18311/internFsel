using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;
using OrderAPI.Service;
using Refit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DDBC>(options => { options.UseSqlServer(@"Data Source=LONG\SQLEXPRESS;Initial Catalog=OrderData; Integrated Security = True;TrustServerCertificate=True "); });
//builder.Services.AddControllers().AddJsonOptions(options => {
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//});
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = "InternFsel",
//        ValidIssuer = "https://localhost:7283",
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsTheSecurityKey12345678"))
//    };
//});
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd")
//    .EnableTokenAcquisitionToCallDownstreamApi()
//    .AddInMemoryTokenCaches();
builder.Services.AddTransient<IOrderRepon, OrderRepon>();
builder.Services.AddTransient<IOrderDetailRepon, OrderDetailRepon>();
builder.Services.AddRefitClient<IApiCustomerService>().ConfigureHttpClient(x =>
{
    x.BaseAddress = new Uri("https://localhost:7186/gateway");
});
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

app.Run();
