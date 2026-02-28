var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/customers", async (HttpContext ctx) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "customers" });
});

app.MapPost("/customers", async (HttpContext ctx) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "customers" });
});

app.MapGet("/customers/{id}", (HttpContext ctx, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "customers", Id = id }));

app.MapPut("/customers/{id}", (HttpContext ctx, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "customers", Id = id }));


// Also multi-tenant paths

app.MapGet("tetants/{tenantId}/customers", async (HttpContext ctx, string tenantId) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "customers", TenantId = tenantId });
});

app.MapPost("tetants/{tenantId}/customers", async (HttpContext ctx, string tenantId) =>
{
    return await ctx.DumpRequestAsync(new { Endpoint = "customers", TenantId = tenantId });
});


app.MapGet("tetants/{tenantId}/customers/{id}", (HttpContext ctx, string tenantId, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "customers", TenantId = tenantId, Id = id }));

app.MapPut("tetants/{tenantId}/customers/{id}", (HttpContext ctx, string tenantId, int id) =>
    ctx.DumpRequestAsync(new { Endpoint = "customers", TenantId = tenantId, Id = id }));

app.MapOpenApi();
app.MapDefaultEndpoints();
app.Run();