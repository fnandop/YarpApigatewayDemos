using Microsoft.AspNetCore.Authentication;

namespace YarpApigatewayDemos.ReverseProxy.Handlers;

public class EasyAuthAuthenticationOptions : AuthenticationSchemeOptions
{
    public EasyAuthAuthenticationOptions()
    {
        Events = new object();
    }
}
