# TransmissionHub

Hub repository with independent projects:

- [TransmissionHub.Client](lib/README.md) - a Transmission RPC library with support for RPC versions 16, 17, and 18.
- [TransmissionHub.Client](bot/README.md) - an ASP.NET Core Telegram bot application on `.NET 8` that uses the `TransmissionHub.Client` package. **Coming soon, stay tuned!**

## Principles

- Projects evolve independently.
- Bot consume the client library via `NuGet`.
- Build settings and package versions are managed centrally.
- The client library is stateless and version-aware for Transmission RPC 16/17/18.
