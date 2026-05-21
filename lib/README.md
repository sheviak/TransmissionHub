# TransmissionHub.Client

Transmission RPC client library for RPC versions 16, 17, and 18.

## Documentation

The purpose of this README file is to provide a quick overview of some of the library's features, including everything you need to get started making calls to the Transmission API.

[Official Transmission RPC specs](https://github.com/transmission/transmission/blob/4.1.0/docs/rpc-spec.md)

## Implemented Features

- Stateless `ITransmissionClient` implementation with Result pattern for all public operations.
- Field and method notation mapping by version:
    - RPC 16/17: kebab-case
    - RPC 18: snake_case
- Session id challenge handling (`X-Transmission-Session-Id`) with automatic retry.
- Safe request logging without sensitive data (`login`, `password`, host, auth header are not logged).
- DI registration with validation
- HTTP client resilience via `Microsoft.Extensions.Http.Resilience` default policy.

## Quick start

To take full advantage of the library, first add the [TransmissionHub.Client](https://www.nuget.org/packages/TransmissionHub.Client)
package to your project by running the following command:

```shell
dotnet add package TransmissionHub.Client
```

Next, you need to add the connection configuration to your Transmission API. \
A detailed description of all parameters and default values is provided in the table below.

```json
{
    "TransmissionHub": {
        "Url": "http://127.0.0.1:9091/transmission/rpc",
        "DownloadDirectory": "/downloads",
        "RequiresAuthentication": true,
        "Login": "user",
        "Password": "password",
        "RpcVersion": 16,
        "TimeoutSeconds": 30,
        "MaxSessionRetries": 1,
        "LogRequests": true,
        "LogResponses": true
    }
}
```
<details>
  <summary>Detailed description of parameters</summary>

    | Parameter              | Description                                                                 | Required                                 | Default |
    |------------------------|-----------------------------------------------------------------------------|------------------------------------------|---------|
    | Url                    | Gets or sets the Transmission RPC endpoint URL                              | true                                     |         |
    | DownloadDirectory      | Gets or sets a default download directory used by consumers                 | true                                     |         |
    | RequiresAuthentication | Gets or sets a value indicating whether basic authentication should be used | false                                    | true    |
    | Login                  | Gets or sets the basic authentication login                                 | true if `RequiresAuthentication` is true |         |
    | Password               | Gets or sets the basic authentication password                              | true if `RequiresAuthentication` is true |         |
    | RpcVersion             | Gets or sets the target Transmission RPC version                            | true                                     |         |
    | TimeoutSeconds         | Gets or sets the HTTP timeout in seconds                                    | false                                    | 10 sec  |
    | MaxSessionRetries      | Gets or sets the maximum number of retries for a Session ID (409 Conflict)  | false                                    | 1 retry |
    | LogRequests            | Gets or sets a value indicating whether to log RPC requests.                | false                                    | false   |
    | LogResponses           | Gets or sets a value indicating whether to log RPC responses.               | false                                    | false   |

</details>

Next you need to register the required client in IServiceCollection, it's very simple, just one line.

```csharp
services.AddTransmissionHubClient(configuration);
```

You can now use `ITransmissionClient` regardless of the specified RPC version.

## Example

> [!IMPORTANT]
> A simple example of connecting and making a request to the Transmission API. \
> To improve the quality of your application, add: CORS, logging, metrics, etc.

```csharp
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransmissionHubClient(builder.Configuration);

var app = builder.Build();

app.UseRouting();

app.MapGet("/", async ([FromServices] ITransmissionClient client) =>
    {
        var request = new TorrentGetRequest
        {
            Fields =
            [
                TorrentGetRequest.TorrentFields.Id,
                TorrentGetRequest.TorrentFields.Name,
                TorrentGetRequest.TorrentFields.Status,
            ]
        };

        var result = await client.TorrentGetAsync(request);

        return Results.Ok(result.Value);
    }
);

app.Run();
```

