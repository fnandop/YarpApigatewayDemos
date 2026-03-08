using Yarp.ReverseProxy.Transforms;
using YarpApigatewayDemos.ReverseProxy.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("ReverseProxySettings/SimpleRoutingDemoSettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ReverseProxySettings/AuthDemoSettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ReverseProxySettings/MultitenantRoutingDemoSettings.json", optional: true, reloadOnChange: true);

builder.AddServiceDefaults();              // Aspire defaults (incl. service discovery)

builder.Services.AddAuthentication(EasyAuthAuthenticationHandler.EASY_AUTH_SCHEME_NAME)
                .AddAzureEasyAuthHandler();
builder.Services.AddAuthorization();


// Based on entra id app roles https://learn.microsoft.com/en-us/entra/identity-platform/howto-add-app-roles-in-apps
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("requireAuthenticatedUser", p => p.RequireAuthenticatedUser()) 
    .AddPolicy("ordersReadPolicy", p => p.RequireAuthenticatedUser().RequireClaim("roles",     "orders.read"))
    .AddPolicy("ordersWritePolicy", p => p.RequireAuthenticatedUser().RequireClaim("roles",    "orders.write"))
    .AddPolicy("customersReadPolicy", p => p.RequireAuthenticatedUser().RequireClaim("roles",  "customers.read"))
    .AddPolicy("customersWritePolicy", p => p.RequireAuthenticatedUser().RequireClaim("roles", "customers.write"));



builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("SimpleRoutingDemoReverseProxy"))
    .LoadFromConfig(builder.Configuration.GetSection("AuthDemoReverseProxy"))
    .LoadFromConfig(builder.Configuration.GetSection("MultitenantRoutingDemoReverseProxy"))
    .AddTransforms(builderContext =>
       {
           // ✅ Only apply to a specific route by RouteId
           if (!builderContext.Route.RouteId.StartsWith("multitenant"))
               return;

           builderContext.AddRequestTransform(transformContext =>
           {
               var tenantId = transformContext.HttpContext.User?.FindFirst("tid")?.Value;
               if (!string.IsNullOrEmpty(tenantId))
               {
                   var originalPath = (transformContext.Path.Value ?? string.Empty).TrimStart('/');
                   transformContext.Path = $"/tetants/{tenantId}/{originalPath}";
               }
               return ValueTask.CompletedTask;
           });
       }).AddServiceDiscoveryDestinationResolver();
var app = builder.Build();
app.MapReverseProxy();
app.Run();