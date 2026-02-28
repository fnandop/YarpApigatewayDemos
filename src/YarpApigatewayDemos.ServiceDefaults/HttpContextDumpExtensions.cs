using Microsoft.AspNetCore.Http;
using System.Text;

public static class HttpContextDumpExtensions
{
    public static async Task<IResult> DumpRequestAsync(
        this HttpContext ctx,
        object? extra = null,
        int maxBodyChars = 50_000)
    {
        var req = ctx.Request;

        var headers = req.Headers.ToDictionary(
            h => h.Key,
            h => string.Join(", ", h.Value.ToArray())
        );

        // Read body safely (so other middleware/endpoints can still read it)
        req.EnableBuffering();

        string? body = null;
        if (req.Body.CanRead)
        {
            if (req.Body.CanSeek)
                req.Body.Position = 0;

            using var reader = new StreamReader(
                req.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true
            );

            body = await reader.ReadToEndAsync();

            if (req.Body.CanSeek)
                req.Body.Position = 0;

            if (body.Length > maxBodyChars)
                body = body[..maxBodyChars] + $"… (truncated, {body.Length} chars total)";
        }

        var payload = new
        {
            TimestampUtc = DateTimeOffset.UtcNow,
            Method = req.Method,
            Scheme = req.Scheme,
            Host = req.Host.Value,
            PathBase = req.PathBase.Value,
            Path = req.Path.Value,
            QueryString = req.QueryString.Value,
            Protocol = req.Protocol,
            ContentType = req.ContentType,
            ContentLength = req.ContentLength,

            RemoteIp = ctx.Connection.RemoteIpAddress?.ToString(),
            RemotePort = ctx.Connection.RemotePort,
            LocalIp = ctx.Connection.LocalIpAddress?.ToString(),
            LocalPort = ctx.Connection.LocalPort,

            IsHttps = req.IsHttps,

            User = new
            {
                IsAuthenticated = ctx.User?.Identity?.IsAuthenticated ?? false,
                AuthenticationType = ctx.User?.Identity?.AuthenticationType,
                Name = ctx.User?.Identity?.Name,
            },

            Headers = headers,
            Body = body,
            Extra = extra
        };

        return Results.Json(payload);
    }
}