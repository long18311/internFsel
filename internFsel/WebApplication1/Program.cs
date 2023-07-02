using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using WebApplication1.repositories.Repon;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DDBC>( options => { options.UseSqlServer(@"Server=LONG\SQLEXPRESS;Database=InternEl;Trusted_Connection=True; Connection Timeout=2"); }
    );

//Integrated Security = True
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("view_customer_Only", policy => {
        policy.RequireClaim("role", "view_customer");
        policy.RequireClaim("location");
    });
    options.AddPolicy("create_customer_Only", policy => {
        policy.RequireClaim("role", "create_customer");
    });
    options.AddPolicy("update_customer_Only", policy => {
        policy.RequireClaim("role", "update_customer");
    });
    options.AddPolicy("delete_customer_Only", policy => {
        policy.RequireClaim("role", "delete_customer");
    });
}).AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:5443";
        options.ApiName = "api";
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
//Where registering services
builder.Services.AddCors(policy => {
    policy.AddPolicy("OpenCorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//app configurations

builder.Services.AddTransient<IUserRepon, UserRepon>();
builder.Services.AddTransient<ICustomerRepon, CustomerRepon>();
builder.Services.AddMediatR(typeof(CustomerRepon).Assembly);
var app = builder.Build();
                
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("OpenCorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
