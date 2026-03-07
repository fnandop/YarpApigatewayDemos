var builder = DistributedApplication.CreateBuilder(args);
var k8s = builder.AddKubernetesEnvironment("k8s");

var ordersApi = builder.AddProject<Projects.YarpApigatewayDemos_OrdersService>("orders-api")
    .WithHttpHealthCheck("/health");

var customersApi = builder.AddProject<Projects.YarpApigatewayDemos_CustomerService>("customers-api")
    .WithHttpHealthCheck("/health");

var identityserver = builder.AddProject<Projects.YarpApigatewayDemos_IdentityApi>("demo-identityserver");

builder.AddProject<Projects.YarpApigatewayDemos_ReverseProxy>("reverse-proxy")
    .WithReference(ordersApi)
    .WithReference(customersApi)
    .WithReference(identityserver);


builder.Build().Run();