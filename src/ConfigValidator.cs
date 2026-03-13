using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Philiprehberger.ConfigValidator;

/// <summary>
/// Provides methods to validate configuration sections against attribute-based rules.
/// </summary>
public static class ConfigValidator
{
    /// <summary>
    /// Validates a configuration section against the attribute rules defined on <typeparamref name="T"/>.
    /// Throws <see cref="ConfigValidationException"/> if any validation errors are found.
    /// </summary>
    /// <typeparam name="T">The configuration type decorated with validation attributes.</typeparam>
    /// <param name="config">The configuration root.</param>
    /// <param name="section">The configuration section name to bind and validate.</param>
    /// <exception cref="ConfigValidationException">Thrown when one or more validation rules fail.</exception>
    public static void Validate<T>(IConfiguration config, string section) where T : class, new()
    {
        var errors = Check<T>(config, section);
        if (errors.Count > 0)
        {
            throw new ConfigValidationException(errors);
        }
    }

    /// <summary>
    /// Checks a configuration section against the attribute rules defined on <typeparamref name="T"/>
    /// and returns a list of error messages without throwing.
    /// </summary>
    /// <typeparam name="T">The configuration type decorated with validation attributes.</typeparam>
    /// <param name="config">The configuration root.</param>
    /// <param name="section">The configuration section name to bind and validate.</param>
    /// <returns>A list of validation error messages. Empty if validation passes.</returns>
    public static IReadOnlyList<string> Check<T>(IConfiguration config, string section) where T : class, new()
    {
        var instance = new T();
        config.GetSection(section).Bind(instance);

        var errors = new List<string>();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(instance);
            var propertyName = $"{section}:{property.Name}";

            ValidateRequired(property, value, propertyName, errors);
            ValidatePattern(property, value, propertyName, errors);
            ValidateRange(property, value, propertyName, errors);
            ValidateMinLength(property, value, propertyName, errors);
            ValidateMaxLength(property, value, propertyName, errors);
        }

        return errors;
    }

    private static void ValidateRequired(PropertyInfo property, object? value, string propertyName, List<string> errors)
    {
        var attr = property.GetCustomAttribute<RequiredAttribute>();
        if (attr is null) return;

        if (value is null)
        {
            errors.Add($"{propertyName} is required but was not provided.");
            return;
        }

        if (value is string str && string.IsNullOrWhiteSpace(str))
        {
            errors.Add($"{propertyName} is required but was empty.");
        }
    }

    private static void ValidatePattern(PropertyInfo property, object? value, string propertyName, List<string> errors)
    {
        var attr = property.GetCustomAttribute<PatternAttribute>();
        if (attr is null || value is not string str) return;

        if (!Regex.IsMatch(str, attr.RegexPattern))
        {
            errors.Add($"{propertyName} value '{str}' does not match pattern '{attr.RegexPattern}'.");
        }
    }

    private static void ValidateRange(PropertyInfo property, object? value, string propertyName, List<string> errors)
    {
        var attr = property.GetCustomAttribute<RangeAttribute>();
        if (attr is null || value is null) return;

        var numericValue = Convert.ToDouble(value);
        if (numericValue < attr.Min || numericValue > attr.Max)
        {
            errors.Add($"{propertyName} value {numericValue} is outside the allowed range [{attr.Min}, {attr.Max}].");
        }
    }

    private static void ValidateMinLength(PropertyInfo property, object? value, string propertyName, List<string> errors)
    {
        var attr = property.GetCustomAttribute<MinLengthAttribute>();
        if (attr is null || value is null) return;

        var length = GetLength(value);
        if (length is not null && length < attr.Length)
        {
            errors.Add($"{propertyName} length {length} is less than the minimum of {attr.Length}.");
        }
    }

    private static void ValidateMaxLength(PropertyInfo property, object? value, string propertyName, List<string> errors)
    {
        var attr = property.GetCustomAttribute<MaxLengthAttribute>();
        if (attr is null || value is null) return;

        var length = GetLength(value);
        if (length is not null && length > attr.Length)
        {
            errors.Add($"{propertyName} length {length} exceeds the maximum of {attr.Length}.");
        }
    }

    private static int? GetLength(object value)
    {
        if (value is string str) return str.Length;
        if (value is ICollection collection) return collection.Count;
        if (value is IEnumerable enumerable) return enumerable.Cast<object>().Count();
        return null;
    }
}
