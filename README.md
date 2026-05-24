<div align="center">
  <img src="assets/icon.svg" alt="TransmissionHub Logo" width="128" />
  <h1>TransmissionHub</h1>
</div>

**TransmissionHub** is a comprehensive ecosystem for managing and interacting with your Transmission torrent servers. Built on modern .NET technologies, it centralizes powerful tools to simplify torrent management for both developers and end-users.

## 🎯 What problems does it solve?

- **Simplifies API Integration:** Abstracts the complexities of the Transmission RPC protocol, automatically handling session management, authentication, and different API versions.
- **Convenient Management:** Enables remote, on-the-go management of your torrent client via a Telegram Bot, bypassing the need for complex web interfaces or VPN setups.
- **Unified Ecosystem:** Provides a centralized, modular approach where tools evolve independently but share a common, reliable foundation and build setup.

## 📁 Projects in this Hub

This repository serves as a central hub for the following independent projects:

### 📦 [TransmissionHub.Client](lib/README.md)
A modern, strongly-typed .NET library designed for interacting with the Transmission torrent client's RPC API.
- **Highlights:** Transparent adaptation to different RPC versions (snake_case vs kebab-case), automatic session handling (409 Conflict retries), resilient HTTP connections, and safe logging.
- **Status:** Available on [NuGet](https://www.nuget.org/packages/TransmissionHub.Client).

### 🤖 [TransmissionHub.TelegramBot](bot/README.md)
An ASP.NET Core Telegram bot application (built on `.NET 8`) that utilizes the `TransmissionHub.Client` to allow you to manage your Transmission server directly from Telegram.
- **Highlights:** Remote torrent management, status updates, and easy configuration.
- **Status:** 🚧 *Coming soon, stay tuned!*

## 💡 Architecture & Principles

- **Independent Evolution:** Projects within the hub evolve and can be released independently.
- **Modularity:** The bot and any future applications consume the core client library via standard `NuGet` packages.
- **Centralized Management:** Build settings and package versions (`Directory.Build.props`, `Directory.Packages.props`) are managed centrally for consistency across the repository.
- **Stateless Foundation:** The core client library is designed to be stateless, ensuring high reliability and easy integration in various application lifecycles.

## 🙌 Contributing

I welcome any help and suggestions for improving the project! \
Feel free to create **Pull Requests** with new features and fixes, or open **Issues** to report bugs and suggest new ideas. \
Your contributions are greatly appreciated!

## 📞 Contacts

- **Email:** [sheviak.k@gmail.com](mailto:sheviak.k@gmail.com)
- **Telegram:** [@sheviak](https://t.me/sheviak)
