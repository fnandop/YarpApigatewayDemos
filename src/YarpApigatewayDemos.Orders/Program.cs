var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/orders", async (HttpContext ctx) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "orders" });
});

app.MapPost("/orders", async (HttpContext ctx) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "orders" });
});

app.MapGet("/orders/{id}", (HttpContext ctx, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "orders", Id = id }));

app.MapPut("/orders/{id}", (HttpContext ctx, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "orders", Id = id }));


// Also multi-tenant paths

app.MapGet("tetants/{tenantId}/orders", async (HttpContext ctx, string tenantId) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "orders", TenantId = tenantId });
});

app.MapPost("tetants/{tenantId}/orders", async (HttpContext ctx, string tenantId) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "orders", TenantId = tenantId });
});


app.MapGet("tetants/{tenantId}/orders/{id}", (HttpContext ctx, string tenantId, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "orders", TenantId = tenantId, Id = id }));

app.MapPut("tetants/{tenantId}/orders/{id}", (HttpContext ctx, string tenantId, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "orders", TenantId = tenantId, Id = id }));

app.MapOpenApi();
app.MapDefaultEndpoints();
app.Run();