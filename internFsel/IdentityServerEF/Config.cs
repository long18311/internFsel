using IdentityServer4.Models;
using IdentityServer4;
using IdentityModel;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServerEF
{
    public static class Config
    {
        public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "123",
                Username = "Gowtham",
                Password = "Test@123",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Gowtham K"),
                    new Claim(JwtClaimTypes.GivenName, "Gowtham"),
                    new Claim(JwtClaimTypes.FamilyName, "Kumar"),
                    new Claim(JwtClaimTypes.WebSite, "https://gowthamcbe.com/"),
                }
            }
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("api.read"),
                new ApiScope("api.write"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api.read"),new ApiResource("api.write")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api.read" }
                },
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                
                    // where to redirect to after login
                    //RedirectUris = { "http://localhost:7292/api/Order/GetAll" },

                    // where to redirect to after logout
                    //PostLogoutRedirectUris = { "http://localhost:7292/api/Order/GetAll" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.read"
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    //RedirectUris =           { "http://localhost:7168/callback.html" },
                    //PostLogoutRedirectUris = { "http://localhost:7168/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:7168" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.read"
                    }
                }
            };

    }
}
