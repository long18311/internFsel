using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;

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
builder.Services.AddTransient<IOrderRepon, OrderRepon>();
builder.Services.AddTransient<IOrderDetailRepon, OrderDetailRepon>();

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
