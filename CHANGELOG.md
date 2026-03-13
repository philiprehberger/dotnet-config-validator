# Changelog

## 0.1.0 (2026-03-13)

- Initial release
- Attribute-based configuration validation: `[Required]`, `[Pattern]`, `[Range]`, `[MinLength]`, `[MaxLength]`
- `ConfigValidator.Validate<T>()` throws on failure, `ConfigValidator.Check<T>()` returns error list
- `IServiceCollection.ValidateConfiguration<T>()` extension for fail-fast startup validation
- `ConfigValidationException` with structured error list
