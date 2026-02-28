# YARP API Gateway Demos

This repository demonstrates the use of [YARP (Yet Another Reverse Proxy)](https://microsoft.github.io/reverse-proxy/) as an API Gateway. It includes three distinct demos showcasing different gateway patterns:

1. **Simple Gateway Routing**
2. **Authentication and Authorization with OAuth Access Tokens**
3. **Multitenant Routing Based on TenantId Claim**

## Prerequisites

- [.NET 10](https://dotnet.microsoft.com/)
- A running Identity Provider for OAuth (e.g., IdentityServer4, Entra Id, etc.)
- Visual Studio or any IDE supporting .NET development

## Project Structure

The solution is organized into the following projects:

- **YarpApigatewayDemos.ReverseProxy**: Contains the YARP configuration and settings for the API Gateway.
- **YarpApigatewayDemos.Orders**: A sample backend service simulating order management.
- **YarpApigatewayDemos.Customerss**: A sample backend service simulating customer management.
- **YarpApigatewayDemos.AppHost**: Hosts the application and integrates the gateway with backend services.
- **YarpApigatewayDemos.IdentityApi**: An Identity API project for handling authentication and authorization using OAuth 2.0, but it could use another provider like Entra ID.
 
## Demos

### 1. Simple Gateway Routing
This demo showcases how YARP can forward the request to the right backend servicet. It demonstrates:
- Routing requests to multiple backend services.

### 2. Authentication and Authorization with OAuth Access Tokens
This demo demonstrates securing the gateway using OAuth 2.0. It includes:
- Authorization based on access tokens.
- Protecting backend services with token validation.

### 3. Multitenant Routing Based on TenantId Claim
This shows how you can use a transformation to implement multitenancy based on claims. It includes:
- Routing requests to different tenants based on the `TenantId` claim in the access token.


## Configuration

### Reverse Proxy Settings
The reverse proxy settings for each demo are defined in the `ReverseProxySettings` folder:
- `SimpleRoutingDemoSettings.json`
- `AuthDemoSettings.json`
- `MultitenantRoutingDemoSettings.json`

### HTTP Request Samples
HTTP request samples for testing the demos are provided in the `httpRequests` folder:
- `1-SimpleRoutingDemo.http`
- `2-AuthDemo-Unauthorized.http`
- `2-AuthDemo-client-1.http`
- `2-AuthDemo-client-2.http`
- `3-MultitenantRoutingDemo.http`

These files include pre-configured requests for interacting with the gateway and backend services.

## References

- [YARP Documentation](https://microsoft.github.io/reverse-proxy/)
- [OAuth 2.0](https://oauth.net/2/)