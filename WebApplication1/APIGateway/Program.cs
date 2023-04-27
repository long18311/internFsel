using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("configuration.json",optional:false,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);
//Where registering services
builder.Services.AddCors(policy => {
    policy.AddPolicy("OpenCorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//builder.Services.AddOcelot(new ConfigurationBuilder().AddJsonFile("configuration.json").Build());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("OpenCorsPolicy");
app.UseAuthorization();
await app.UseOcelot();

app.MapControllers();

app.Run();
