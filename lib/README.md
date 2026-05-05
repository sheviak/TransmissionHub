# TransmissionHub.Client

.NET Client library for Transmission RPC integration

## Configuration Example

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
        "MaxSessionRetries": 1
    }
}
```

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
