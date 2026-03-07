using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("ReverseProxySettings/SimpleRoutingDemoSettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ReverseProxySettings/AuthDemoSettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ReverseProxySettings/MultitenantRoutingDemoSettings.json", optional: true, reloadOnChange: true);

builder.AddServiceDefaults();              // Aspire defaults (incl. service discovery)

var configuration = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.Authority = configuration["services:demo-identityserver:https:0"];
    jwtOptions.TokenValidationParameters.ValidateAudience = false;
});


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("requireAuthenticatedUser", p => p.RequireAuthenticatedUser())
    .AddPolicy("ordersReadPolicy", p => p.RequireAuthenticatedUser().RequireClaim("scope", "orders.read"))
    .AddPolicy("ordersWritePolicy", p => p.RequireAuthenticatedUser().RequireClaim("scope", "orders.write"))
    .AddPolicy("customersReadPolicy", p => p.RequireAuthenticatedUser().RequireClaim("scope", "customers.read"))
    .AddPolicy("customersWritePolicy", p => p.RequireAuthenticatedUser().RequireClaim("scope", "customers.write"));



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
               var tenantId = transformContext.HttpContext.User?.FindFirst("client_tenantId")?.Value;
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