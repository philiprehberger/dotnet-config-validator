using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Philiprehberger.ConfigValidator;

/// <summary>
/// Extension methods for registering and validating configuration sections at startup.
/// </summary>
public static class ConfigValidatorExtensions
{
    /// <summary>
    /// Validates a configuration section against the attribute rules defined on <typeparamref name="T"/>
    /// and registers the bound instance as a singleton. Throws <see cref="ConfigValidationException"/>
    /// at registration time if validation fails (fail-fast).
    /// </summary>
    /// <typeparam name="T">The configuration type decorated with validation attributes.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The configuration root.</param>
    /// <param name="sectionName">The configuration section name to bind and validate.</param>
    /// <returns>The service collection for chaining.</returns>
    /// <exception cref="ConfigValidationException">Thrown when one or more validation rules fail.</exception>
    public static IServiceCollection ValidateConfiguration<T>(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName) where T : class, new()
    {
        ConfigValidator.Validate<T>(config, sectionName);

        var instance = new T();
        config.GetSection(sectionName).Bind(instance);
        services.AddSingleton(instance);

        return services;
    }
}
