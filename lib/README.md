# 📦 TransmissionHub.Client

A modern .NET library for the Transmission RPC API.

## 📋 Table of Contents
- [What is this library?](#what-is-this-library)
- [What problems does it solve?](#what-problems-does-it-solve)
- [What is its main highlight?](#what-is-its-main-highlight)
- [Documentation](#documentation)
- [Quick start](#quick-start)
- [Example](#example)

## 🤔 What is this library?

**TransmissionHub.Client** is a modern .NET library designed for interacting with the Transmission torrent client's RPC API. It provides a convenient, strongly-typed, and resilient way to manage your Transmission server from any .NET application.

## 🎯 What problems does it solve?

This library abstracts the complexities of direct communication with the Transmission RPC, solving the following problems:

- **Protocol complexity:** No more manual JSON construction. The library provides ready-to-use C# models for all operations.
- **Session management:** Automatically handles the `X-Transmission-Session-Id`, including transparent retries on 409 Conflict errors.
- **Safe logging:** Built-in mechanisms automatically redact sensitive data (login, password, host) from logs.
- **Resilience:** Utilizes `Microsoft.Extensions.Http.Resilience` for connection stability and automatic retries.
- **Easy integration:** Seamlessly integrates with `Microsoft.Extensions.DependencyInjection` and configuration binding.

## 🌟 What is its main highlight?

The key feature of **TransmissionHub.Client** is its **transparent adaptation to different RPC protocol versions (16, 17, and 18)**.

Different Transmission versions use different JSON field naming conventions (`kebab-case` vs. `snake_case`). This library **automatically maps** field names based on your configuration. You write your code once, and it works across different Transmission server versions.

## 📚 Documentation

This README provides a quick overview. For complete details, refer to the official specifications.

[Official Transmission RPC specs](https://github.com/transmission/transmission/blob/4.1.0/docs/rpc-spec.md)

## 🚀 Quick start

### 1. Add the Package

Add the [TransmissionHub.Client](https://www.nuget.org/packages/TransmissionHub.Client) package to your project:

```shell
dotnet add package TransmissionHub.Client
```

### 2. Configure Your `appsettings.json`

Add the connection settings for your Transmission API:

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
  <summary>View detailed parameter descriptions</summary>

| Parameter              | Description                                                    | Required                                   | Default |
|------------------------|----------------------------------------------------------------|--------------------------------------------|---------|
| Url                    | The Transmission RPC endpoint URL.                             | **true**                                   |         |
| DownloadDirectory      | The default download directory used by consumers.              | **true**                                   |         |
| RequiresAuthentication | Indicates whether basic authentication should be used.         | false                                      | `true`  |
| Login                  | The basic authentication login.                                | `true` if `RequiresAuthentication` is true |         |
| Password               | The basic authentication password.                             | `true` if `RequiresAuthentication` is true |         |
| RpcVersion             | The target Transmission RPC version.                           | **true**                                   |         |
| TimeoutSeconds         | The HTTP timeout in seconds.                                   | false                                      | `10`    |
| MaxSessionRetries      | The maximum number of retries for a Session ID (409 Conflict). | false                                      | `1`     |
| LogRequests            | Indicates whether to log RPC requests.                         | false                                      | `false` |
| LogResponses           | Indicates whether to log RPC responses.                        | false                                      | `false` |

</details>

### 3. Register the Client

In your `Program.cs` or startup configuration, register the client in the `IServiceCollection`:

```csharp
services.AddTransmissionHubClient(configuration);
```

You can now inject and use `ITransmissionClient` anywhere in your application.

## 💻 Example

> [!IMPORTANT]
> This is a minimal example. For a production application, consider adding proper error handling, logging, CORS, and other middleware.

```csharp
using TransmissionHub.Client.Abstractions;
using TransmissionHub.Client.Models.Requests;

var builder = WebApplication.CreateBuilder(args);

// Register the client
builder.Services.AddTransmissionHubClient(builder.Configuration);

var app = builder.Build();

app.MapGet("/torrents", async (ITransmissionClient client) =>
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
});

app.Run();
```
