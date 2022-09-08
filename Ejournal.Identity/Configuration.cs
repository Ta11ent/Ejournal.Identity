using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4;
using System.Collections.Generic;

namespace Ejournal.Identity
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> 
            {
                new ApiScope(name: "ejournal_web_api", displayName:"Web API") 
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("Levels", "User level(s)", new List<string> { "level" })
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource(name: "ejournal_web_api", displayName:"Web API",
                    new[] {"level"}) //JwtClaimTypes.Name
                {
                    Scopes = { "ejournal_web_api" }
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "ejournal_web_id",
                    ClientName = "Web API",
                    AllowedGrantTypes = GrantTypes.Code,
                    //RequireClientSecret = false,
                    ClientSecrets = new List<Secret> { new Secret("wREdeWsH4E".Sha256()) },
                    RequirePkce = true,
                    /// <summary>
                    /// nedd client app
                    /// RedirectUris = {"Https://.../signin-oidc"}, 
                    /// AllowedCorsOrigins ={"Https://..."},
                    /// PostLogoutRedirectUris = {"Https://...signuot-oidc"}
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ejournal_web_api"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId ="postman_id_postman",
                    ClientName = "Postman",
                    ClientSecrets = new List<Secret>{ new Secret("sEDOqTdOLB".Sha256()) },
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ejournal_web_api"
                    },
                    RequireConsent = true
                }
            };

    }
}
