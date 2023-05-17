using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;
using System.Text.Json;

namespace ServerIden
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("OrderAPI.read"), new ApiScope("OrderAPI.write"), };
        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("OrderAPI")
                {
                    Scopes = new List<string> { "OrderAPI.read", "OrderAPI.write" },
                    ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
                    AllowedScopes = { "OrderAPI.read", "OrderAPI.write" },
                    AllowOfflineAccess = true,
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = true
                },
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedCorsOrigins ={"https://localhost/"},
                    RedirectUris = { "https://localhost:7101/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7101/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7101/signout-callback-oidc" },
                    
                    AllowedScopes = { "openid", "profile", "OrderAPI.read" },
                    AllowOfflineAccess = true,
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                },
            };
    }
}
