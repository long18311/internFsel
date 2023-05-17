using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;
using OrderAPI.Service;
using OrderAPI.ViewModel.JWT_Token;
using Refit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DDBC>(options => { options.UseSqlServer(@"Data Source=LONG\SQLEXPRESS;Initial Catalog=OrderData; Integrated Security = True;TrustServerCertificate=True "); });
/*builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});*/
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:7101";
        options.ApiName = "CoffeeAPI";
    });
/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "InternFsel",
        ValidIssuer = "https://localhost:7283",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsTheSecurityKey12345678"))
    };
});*/


builder.Services.AddTransient<IOrderRepon, OrderRepon>();
builder.Services.AddMediatR(typeof(OrderRepon).Assembly);
builder.Services.AddTransient<IOrderDetailRepon, OrderDetailRepon>();
builder.Services.AddMediatR(typeof(OrderDetailRepon).Assembly);
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddRefitClient<IApiCustomerService>()
    .ConfigureHttpClient(x => x.BaseAddress = new Uri("https://localhost:7186/gateway")).AddHttpMessageHandler<AuthorizationMessageHandler>();

//builder.Services.AddRefitClient<IApiCustomerService>().ConfigureHttpClient(x => x.BaseAddress = new Uri("https://localhost:7186/gateway")).AddHttpMessageHandler<AuthorizationMessageHandler>();

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
