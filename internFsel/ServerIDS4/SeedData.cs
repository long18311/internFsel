using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerIDS4.Data;
using System.Security.Claims;

namespace ServerIDS4
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AspNetIdentityDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<AspNetIdentityDbContext>();
            ctx.Database.Migrate();
            EnsureRoles(scope);
            EnsureUsers(scope);
        }
        private static async void EnsureRoles(IServiceScope scope)
        {
           
            var roleMgr = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var view_customer = roleMgr.FindByNameAsync("view_customer").Result;
            if (view_customer == null)
            {
                view_customer = new IdentityRole() { Name = "view_customer" };
                var result = roleMgr.CreateAsync(view_customer).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                /*result = roleMgr.AddClaimAsync(view_customer, new Claim("view_customer", "true")).Result;*/
            }
            var create_customer = roleMgr.FindByNameAsync("create_customer").Result;
            if (create_customer == null)
            {
                create_customer = new IdentityRole() { Name = "create_customer" };
                var result = roleMgr.CreateAsync(create_customer).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                /*result = roleMgr.AddClaimAsync(create_customer, new Claim("create_customer", "true")).Result;*/
            }
            var update_customer = roleMgr.FindByNameAsync("update_customer").Result;
            if (update_customer == null)
            {
                update_customer = new IdentityRole()
                {
                    Name = "update_customer"
                };
                var result = roleMgr.CreateAsync(update_customer).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                result = roleMgr.AddClaimAsync(update_customer, new Claim("update_customer", "true")).Result;
            }
            
            var delete_customer = roleMgr.FindByNameAsync("delete_customer").Result;
            if (delete_customer == null)
            {
                delete_customer = new IdentityRole() { Name = "delete_customer" };
                var result = roleMgr.CreateAsync(delete_customer).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                /*result = roleMgr.AddClaimAsync(delete_customer, new Claim("delete_customer", "true")).Result;*/
            }
        }
        private static async void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var angella = userMgr.FindByNameAsync("angella").Result;
            if (angella == null)
            {
                angella = new IdentityUser
                {
                    UserName = "angella",
                    Email = "angella.freeman@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(angella, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                result =
                    userMgr.AddClaimsAsync(
                        angella,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Angella Freeman"),
                            new Claim(JwtClaimTypes.GivenName, "Angella"),
                            new Claim(JwtClaimTypes.FamilyName, "Freeman"),
                            new Claim(JwtClaimTypes.WebSite, "http://angellafreeman.com"),
                            new Claim("location", "somewhere"),
                            new Claim("role","vui"),
                            new Claim("create_customer","true"),
                        }
                    ).Result;
                /*result = userMgr.AddToRolesAsync(angella, new List<string> { "create_customer", "view_customer", "delete_customer", "update_customer" }).Result;*/
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
