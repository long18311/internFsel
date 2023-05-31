using IdentityServer4.Models;

namespace ServerIDS4
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
            new[] { new ApiScope("api.read"), new ApiScope("api.write"), };
        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("api")
                {
                    Scopes = new List<string> { "api.read", "api.write" },
                    ApiSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                    UserClaims = new List<string> { "role" },
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "api.read", "api.write" }
                },
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "newclient",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,                    
                    AllowedScopes = { "openid", "profile", "api.read","api.write" },                    
                },
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    RedirectUris = { "https://localhost:7292/api/Order/GetAll" },
                    FrontChannelLogoutUri = "https://localhost:7292/api/Order/GetAll",
                    PostLogoutRedirectUris = { "https://localhost:7292/api/Order/GetAll" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api.read","api.write" },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                },
                new Client
                {
                    ClientId = "interactivecl",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5444/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api.read" },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                },
            };
    }
}
