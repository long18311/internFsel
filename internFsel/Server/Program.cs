using IdentityServer4;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityConfiguration.Clients)
    .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
    .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddTestUsers(IdentityConfiguration.TestUsers)
    .AddDeveloperSigningCredential();
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseRouting();
app.UseIdentityServer();


app.Run();