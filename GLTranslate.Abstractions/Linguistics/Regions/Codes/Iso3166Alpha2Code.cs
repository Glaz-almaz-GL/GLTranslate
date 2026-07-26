namespace GLTranslate.Abstractions.Linguistics.Regions.Codes;

/// <summary>
/// Represents an ISO 3166-1 alpha-2 region code.
/// </summary>
/// <remarks>
/// ISO 3166-1 alpha-2 codes consist of two uppercase Latin letters.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="Iso3166Alpha2Code"/> class.
/// </remarks>
/// <param name="value">
/// The two-letter ISO 3166-1 alpha-2 code.
/// </param>
/// <exception cref="ArgumentException">
/// Thrown when the value is not a valid ISO 3166-1 alpha-2 code.
/// </exception>
public sealed class Iso3166Alpha2Code(string value) : RegionCode(Normalize(value))
{
    private static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim().ToUpperInvariant();

        if (value.Length != 2 || !value.All(char.IsLetter))
        {
            // ISO 3166-1 alpha-3 code must contain exactly three letters. (RU, US)
            throw new ArgumentException("ISO 3166-1 alpha-2 code must contain exactly three letters.", nameof(value));
        }

        return value;
    }
}