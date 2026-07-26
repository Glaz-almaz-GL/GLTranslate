namespace GLTranslate.Abstractions.Linguistics.Regions.Codes;

/// <summary>
/// Represents an ISO 3166-1 alpha-3 region code.
/// </summary>
/// <remarks>
/// ISO 3166-1 alpha-3 codes consist of three uppercase Latin letters.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="Iso3166Alpha3Code"/> class.
/// </remarks>
public sealed class Iso3166Alpha3Code(string value) : RegionCode(Normalize(value))
{
    private static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim().ToUpperInvariant();

        if (value.Length != 3 || !value.All(char.IsLetter))
        {
            // ISO 3166-1 alpha-3 code must contain exactly three letters. (RUS, USA)
            throw new ArgumentException("ISO 3166-1 alpha-3 code must contain exactly three letters.", nameof(value));
        }

        return value;
    }
}