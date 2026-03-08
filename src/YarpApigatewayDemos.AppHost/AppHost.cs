var builder = DistributedApplication.CreateBuilder(args);

var ordersApi = builder.AddProject<Projects.YarpApigatewayDemos_OrdersService>("orders-api")
    .WithHttpHealthCheck("/health");

var customersApi = builder.AddProject<Projects.YarpApigatewayDemos_CustomerService>("customers-api")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.YarpApigatewayDemos_ReverseProxy>("reverse-proxy")
    .WithReference(ordersApi)
    .WithReference(customersApi)
    .WithExternalHttpEndpoints();


builder.Build().Run();