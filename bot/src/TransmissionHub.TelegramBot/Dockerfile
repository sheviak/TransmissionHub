FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG VERSION=1.0.0
WORKDIR /src

# Copy build configuration from repo root
COPY Directory.Build.props Directory.Packages.props global.json ./

# Copy project file and restore
COPY src/TransmissionHub.TelegramBot/TransmissionHub.TelegramBot.csproj src/TransmissionHub.TelegramBot/
RUN dotnet restore src/TransmissionHub.TelegramBot/TransmissionHub.TelegramBot.csproj

# Copy source and publish
COPY src/TransmissionHub.TelegramBot/ src/TransmissionHub.TelegramBot/
RUN dotnet publish src/TransmissionHub.TelegramBot/TransmissionHub.TelegramBot.csproj \
    -c Release -o /app -p:Version=${VERSION} --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TransmissionHub.TelegramBot.dll"]
