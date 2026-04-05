FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG VERSION=1.0.0
WORKDIR /src

# Copy build configuration and assets from repo root
COPY Directory.Build.props Directory.Packages.props global.json ./
COPY assets/ assets/

# Copy project's README
COPY bot/README.md bot/README.md

# Copy project file and restore
COPY bot/src/TransmissionHub.TelegramBot/TransmissionHub.TelegramBot.csproj bot/src/TransmissionHub.TelegramBot/
RUN dotnet restore bot/src/TransmissionHub.TelegramBot/TransmissionHub.TelegramBot.csproj

# Copy source and publish
COPY bot/src/TransmissionHub.TelegramBot/ bot/src/TransmissionHub.TelegramBot/
RUN dotnet publish bot/src/TransmissionHub.TelegramBot/TransmissionHub.TelegramBot.csproj \
    -c Release -o /app -p:Version=${VERSION} --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TransmissionHub.TelegramBot.dll"]
