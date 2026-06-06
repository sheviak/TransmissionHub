using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using TransmissionHub.Client.Internal.Clients;
using TransmissionHub.Client.Internal.Dialects;
using TransmissionHub.Client.Internal.Http;
using TransmissionHub.Client.Internal.Validation;

namespace TransmissionHub.Client.Abstractions;

/// <summary>
/// Contains DI registration helpers for Transmission client.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    private const string SECTION_NAME = "TransmissionHubClient";

    /// <summary>
    /// Registers Transmission client from configuration section. Section name is <c>TransmissionHub</c>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <exception cref="InvalidOperationException">There is no section with key.</exception>
    public static IServiceCollection AddTransmissionHubClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var options = configuration.GetRequiredSection(SECTION_NAME).Get<TransmissionClientOptions>();

        return services.AddTransmissionHubClient(options);
    }

    private static IServiceCollection AddTransmissionHubClient(this IServiceCollection services,
        TransmissionClientOptions? options)
    {
        ValidateSettings(options);

        services.AddSingleton(options!);
        services.AddTransmissionValidation();

        return options!.RpcVersion switch
        {
            16 => RegisterClient<TransmissionClientV16, RpcRequestDialect>(services, options),
            17 => RegisterClient<TransmissionClientV17, RpcRequestDialect>(services, options),
            18 => RegisterClient<TransmissionClientV18, JsonRpcDialect>(services, options),
            _ => throw new ValidationException($"Unsupported RPC version: {options.RpcVersion}."),
        };
    }

    private static IServiceCollection RegisterClient<TImplementation, TDialect>(IServiceCollection services,
        TransmissionClientOptions options)
        where TImplementation : class, ITransmissionClient
        where TDialect : class, IRpcDialect
    {
        services.AddSingleton<IRpcDialect, TDialect>();
        services.AddTransient<SessionIdDelegatingHandler>();

        var httpClientBuilder = services
            .AddHttpClient<ITransmissionClient, TImplementation>(client => ConfigureHttpClient(client, options))
            .AddHttpMessageHandler<SessionIdDelegatingHandler>();

        httpClientBuilder.AddStandardResilienceHandler();
        httpClientBuilder.AddResilienceHandler("session-retry", configure =>
            configure.AddRetry(new HttpRetryStrategyOptions
            {
                UseJitter = false,
                Delay = TimeSpan.Zero,
                MaxRetryAttempts = options.MaxSessionRetries,
                ShouldHandle = args =>
                    ValueTask.FromResult(args.Outcome.Result?.StatusCode is HttpStatusCode.Conflict),
            }));

        return services;
    }

    private static void ConfigureHttpClient(HttpClient client, TransmissionClientOptions options)
    {
        client.BaseAddress = options.Url;
        client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        if (!options.RequiresAuthentication)
        {
            return;
        }

        var authBytes = Encoding.UTF8.GetBytes($"{options.Login}:{options.Password}");
        var base64 = Convert.ToBase64String(authBytes);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);
    }

    private static void ValidateSettings(TransmissionClientOptions? settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var validationContext = new ValidationContext(settings);

        Validator.ValidateObject(settings, validationContext, validateAllProperties: true);
    }

    /// <summary>
    /// Registers the validation provider and all validators from the client assembly.
    /// </summary>
    /// <param name="services">The service collection.</param>
    internal static IServiceCollection AddTransmissionValidation(this IServiceCollection services)
    {
        services.AddSingleton<IValidatorProvider, ValidatorProvider>();

        var validatorInterfaceType = typeof(IValidatableRequest<>);
        var validators = typeof(ServiceCollectionExtensions).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && t.GetInterfaces().Any(i =>
                            i.IsGenericType && i.GetGenericTypeDefinition() == validatorInterfaceType))
            .ToArray();

        foreach (var validatorType in validators)
        {
            var interfaceType = validatorType
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorInterfaceType);

            services.AddSingleton(interfaceType, validatorType);
        }

        return services;
    }
}