using System.Text.RegularExpressions;

namespace Philiprehberger.ConfigValidator;

/// <summary>
/// Marks a configuration property as required. The value must be present and non-null/non-empty.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredAttribute : Attribute
{
}

/// <summary>
/// Validates that a string configuration property matches the specified regular expression pattern.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class PatternAttribute : Attribute
{
    /// <summary>
    /// Gets the regular expression pattern to match against.
    /// </summary>
    public string RegexPattern { get; }

    /// <summary>
    /// Creates a new <see cref="PatternAttribute"/> with the specified regex pattern.
    /// </summary>
    /// <param name="regexPattern">The regular expression pattern the value must match.</param>
    public PatternAttribute(string regexPattern)
    {
        RegexPattern = regexPattern;
    }
}

/// <summary>
/// Validates that a numeric configuration property falls within the specified range (inclusive).
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RangeAttribute : Attribute
{
    /// <summary>
    /// Gets the minimum allowed value (inclusive).
    /// </summary>
    public double Min { get; }

    /// <summary>
    /// Gets the maximum allowed value (inclusive).
    /// </summary>
    public double Max { get; }

    /// <summary>
    /// Creates a new <see cref="RangeAttribute"/> with the specified minimum and maximum values.
    /// </summary>
    /// <param name="min">The minimum allowed value (inclusive).</param>
    /// <param name="max">The maximum allowed value (inclusive).</param>
    public RangeAttribute(double min, double max)
    {
        Min = min;
        Max = max;
    }
}

/// <summary>
/// Validates that a string or collection property has at least the specified length/count.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MinLengthAttribute : Attribute
{
    /// <summary>
    /// Gets the minimum required length.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Creates a new <see cref="MinLengthAttribute"/> with the specified minimum length.
    /// </summary>
    /// <param name="length">The minimum required length.</param>
    public MinLengthAttribute(int length)
    {
        Length = length;
    }
}

/// <summary>
/// Validates that a string or collection property does not exceed the specified length/count.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MaxLengthAttribute : Attribute
{
    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Creates a new <see cref="MaxLengthAttribute"/> with the specified maximum length.
    /// </summary>
    /// <param name="length">The maximum allowed length.</param>
    public MaxLengthAttribute(int length)
    {
        Length = length;
    }
}
