namespace YarpApigatewayDemos.IdentityApi;

using Duende.IdentityServer.Models;

public class Config
{
    // ApiResources define the apis in your system
    public static IEnumerable<ApiResource> GetApis() =>
            [
                new ("protected-api", "Protected Api")
            ];

    // ApiScope is used to protect the API 
    //The effect is the same as that of API resources in IdentityServer 3.x
    public static IEnumerable<ApiScope> GetApiScopes() => [
                new("orders.read", "Orders Read"),
                new ("orders.write", "Orders Write"),
                new ("customers.read", "Customers Read"),
                new ("customers.write", "Customers Write")
            ];

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public static IEnumerable<IdentityResource> GetResources() => [
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            ];

    // client want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients(IConfiguration _) => [
                new Client
                {
                    ClientId = "protected-api-client-1",
                    ClientName = "Protected Api Client 1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = new List<string>
                    {
                        "orders.read",
                        "customers.read"
                    },
                    Claims =
                    {
                        new ClientClaim("tenantId", "tenant-1")
                    },
                     AccessTokenLifetime = 60*60*24*365*10, // a lot of time, for testing
                    
                },
                new Client
                {
                    ClientId = "protected-api-client-2",
                    ClientName = "Protected Api Client 2",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = new List<string>
                    {
                        "orders.read",
                        "customers.read",
                        "orders.write",
                        "customers.write"
                    },
                    Claims =
                    {
                        new ClientClaim("tenantId", "tenant-2")
                    },
                     AccessTokenLifetime = 60*60*24*365*10, // a lot of time, for testing
                    
                }

        ];
}