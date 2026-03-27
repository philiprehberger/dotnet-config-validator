# Philiprehberger.ConfigValidator

[![CI](https://github.com/philiprehberger/dotnet-config-validator/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-config-validator/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.ConfigValidator.svg)](https://www.nuget.org/packages/Philiprehberger.ConfigValidator)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-config-validator)](LICENSE)
[![Sponsor](https://img.shields.io/badge/sponsor-GitHub%20Sponsors-ec6cb9)](https://github.com/sponsors/philiprehberger)

Validate appsettings.json configuration sections at startup with attribute-based rules.

## Installation

```bash
dotnet add package Philiprehberger.ConfigValidator
```

## Usage

### Define your options class with validation attributes

```csharp
using Philiprehberger.ConfigValidator;

public class SmtpOptions
{
    [Required]
    public string Host { get; set; } = "";

    [Range(1, 65535)]
    public int Port { get; set; }

    [Required]
    [Pattern(@"^[^@]+@[^@]+\.[^@]+$")]
    public string FromAddress { get; set; } = "";

    [MinLength(1)]
    [MaxLength(100)]
    public string Username { get; set; } = "";
}
```

### Register and validate at startup (fail-fast)

```csharp
using Philiprehberger.ConfigValidator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ValidateConfiguration<SmtpOptions>(
    builder.Configuration,
    "Smtp");

var app = builder.Build();
app.Run();
```

If any validation rule fails, a `ConfigValidationException` is thrown immediately at startup with all errors listed.

### Check without throwing

```csharp
var errors = ConfigValidator.Check<SmtpOptions>(configuration, "Smtp");

foreach (var error in errors)
{
    Console.WriteLine(error);
}
```

## API

### Attributes

| Attribute | Target | Description |
|-----------|--------|-------------|
| `[Required]` | Property | Value must be present and non-null/non-empty |
| `[Pattern(regex)]` | String property | Value must match the regular expression |
| `[Range(min, max)]` | Numeric property | Value must be within range (inclusive) |
| `[MinLength(n)]` | String/collection | Length/count must be at least n |
| `[MaxLength(n)]` | String/collection | Length/count must be at most n |

### `ConfigValidator`

| Method | Description |
|--------|-------------|
| `Validate<T>(IConfiguration, string)` | Binds section to T, validates, throws `ConfigValidationException` on failure |
| `Check<T>(IConfiguration, string)` | Binds section to T, validates, returns list of error strings |

### `ConfigValidatorExtensions`

| Method | Description |
|--------|-------------|
| `ValidateConfiguration<T>(IServiceCollection, IConfiguration, string)` | Validates at registration time and registers the bound instance as a singleton |

### `ConfigValidationException`

| Member | Description |
|--------|-------------|
| `Errors` | `IReadOnlyList<string>` of all validation error messages |
| `Message` | Formatted string listing all errors |

## Development

```bash
dotnet build src/Philiprehberger.ConfigValidator.csproj --configuration Release
```

## License

MIT
