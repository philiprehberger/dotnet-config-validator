namespace Philiprehberger.ConfigValidator;

/// <summary>
/// Exception thrown when configuration validation fails. Contains all validation errors.
/// </summary>
public sealed class ConfigValidationException : Exception
{
    /// <summary>
    /// Gets the list of validation error messages.
    /// </summary>
    public IReadOnlyList<string> Errors { get; }

    /// <summary>
    /// Creates a new <see cref="ConfigValidationException"/> with the specified validation errors.
    /// </summary>
    /// <param name="errors">The list of validation error messages.</param>
    public ConfigValidationException(IReadOnlyList<string> errors)
        : base(FormatMessage(errors))
    {
        Errors = errors;
    }

    private static string FormatMessage(IReadOnlyList<string> errors)
    {
        return $"Configuration validation failed with {errors.Count} error(s):{Environment.NewLine}" +
               string.Join(Environment.NewLine, errors.Select(e => $"  - {e}"));
    }
}
