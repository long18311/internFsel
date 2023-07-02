using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Hubs;
using ServerSignalData.Confing;
using ServerSignalData.repositories.IRepon;
using ServerSignalData.repositories.Repon;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ManageAppDbContext>(options => { 
    options.UseSqlServer(@"Server=LONG\SQLEXPRESS;Database=SignalData;Trusted_Connection=True; Connection Timeout=2");
}
    );
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:5500")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials();
        });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "InternFsel",
        ValidIssuer = "https://localhost:7194",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsTheSecurityKey12345678"))
};
});
builder.Services.AddTransient<IUserRepon, UserRepon>();
builder.Services.AddTransient<IMessageRepon, MessageRepon>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<SignalHub>("/signalHub");
app.MapControllers();

app.Run();
