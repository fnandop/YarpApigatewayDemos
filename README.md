# YARP API Gateway Demos + Azure Container Apps (ACA) + Microsoft Entra ID

This repository demonstrates the use of [YARP (Yet Another Reverse Proxy)](https://microsoft.github.io/reverse-proxy/) as an API Gateway deployed on Azure Container Apps (ACA), using Microsoft Entra ID as the Identity Provider for authentication and authorization.

It includes three distinct demos showcasing different gateway patterns:

1. **Simple Gateway Routing**
2. **Authentication and Authorization with OAuth Access Tokens**
3. **Multitenant Routing Based on TenantId Claim**

## Prerequisites

- [.NET 10](https://dotnet.microsoft.com/)
- An Azure account and subscription
- Azure Developer CLI (azd) installed and configured. Follow the instructions [here](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd) to set up the Azure Developer CLI.
- Bicep CLI installed. Follow the instructions [here](https://learn.microsoft.com/azure/azure-resource-manager/bicep/install) to install the Bicep CLI.
- Visual Studio or any IDE supporting .NET development

## How to Deploy the Demos to Azure

This solution is designed to be easily deployed to Azure. To deploy:

1. Run the following command to provision the ACA infrastructure and deploy each API to its own container app:
   ```bash
   azd up
   ```
   This command will automatically handle the deployment process.

   Or trigger  the github workflow defined in  [`azure-dev.yml](./.github/workflows/azure-dev.yml) 

2. Once the deployment is complete, the necessary infrastructure and APIs will be ready to use.

## Authentication and Authorization Using Microsoft Entra ID

1. **Add a Microsoft Identity Provider** to the reverse-proxy application. This will create a Microsoft Entra ID Enterprise Application (or you can choose an existing one). Refer to the [official documentation](https://learn.microsoft.com/en-us/azure/container-apps/authentication-entra) for detailed steps.

2. **Create App Roles**: Add the following app roles to the Enterprise Application:
   - `orders.read`
   - `orders.write`
   - `customers.read`
   - `customers.write`

   Assign these roles to Microsoft Entra ID users or groups as needed. Refer to the [official documentation](https://learn.microsoft.com/en-us/entra/identity-platform/howto-add-app-roles-in-apps) for detailed steps.

## Project Structure

The solution is organized into the following projects:

- **YarpApigatewayDemos.ReverseProxy**: Contains the YARP configuration and settings for the API Gateway.
- **YarpApigatewayDemos.Orders**: A sample backend service simulating order management.
- **YarpApigatewayDemos.Customers**: A sample backend service simulating customer management.
- **YarpApigatewayDemos.AppHost**: Hosts the application and integrates the gateway with backend services.

## Demos

### 1. Simple Gateway Routing
This demo showcases how YARP can forward requests to the appropriate backend service. It demonstrates:
- Routing requests to multiple backend services.

### 2. Authentication and Authorization with OAuth Access Tokens
This demo demonstrates securing the gateway using OAuth 2.0. It includes:
- Authorization based on access tokens.
- Protecting backend services with token validation.

### 3. Multitenant Routing Based on TenantId Claim
This demo highlights multitenancy support in YARP. It includes:
- Routing requests to different tenants based on the `TenantId` claim in the access token.
- Demonstrating tenant isolation in the gateway.

## Configuration

### Reverse Proxy Settings
The reverse proxy settings for each demo are defined in the `ReverseProxySettings` folder:
- `SimpleRoutingDemoSettings.json`
- `AuthDemoSettings.json`
- `MultitenantRoutingDemoSettings.json`

These files include pre-configured settings for interacting with the gateway and backend services.

## References

- [YARP Documentation](https://microsoft.github.io/reverse-proxy/)
- [OAuth 2.0](https://oauth.net/2/)
- [Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/overview)
- [Microsoft Entra ID](https://learn.microsoft.com/en-us/azure/active-directory/fundamentals/active-directory-whatis)