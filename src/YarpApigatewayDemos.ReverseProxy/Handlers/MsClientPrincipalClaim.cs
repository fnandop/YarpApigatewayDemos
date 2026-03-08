using System.Text.Json.Serialization;

namespace YarpApigatewayDemos.ReverseProxy.Handlers;

public class MsClientPrincipalClaim
{
    [JsonPropertyName("typ")]
    public string? Type { get; set; }

    [JsonPropertyName("val")]
    public string? Value { get; set; }
}
