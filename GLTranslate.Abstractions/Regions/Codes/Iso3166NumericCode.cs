namespace GLTranslate.Abstractions.Regions.Codes;

/// <summary>
/// Represents an ISO 3166-1 numeric region code.
/// </summary>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="Iso3166NumericCode"/> class.
/// </remarks>
public sealed class Iso3166NumericCode(string value) : RegionCode(Normalize(value))
{
    private static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        return value.Length != 3 || !value.All(char.IsDigit)
            ? throw new ArgumentException("ISO 3166-1 numeric code must contain exactly three digits.", nameof(value))
            : value;
    }
}