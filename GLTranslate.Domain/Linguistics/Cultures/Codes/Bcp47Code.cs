using GLTranslate.Abstractions.Linguistics.Cultures;

namespace GLTranslate.Domain.Linguistics.Cultures.Codes;

/// <summary>
/// Represents a culture code defined by the
/// <c>BCP 47</c> (Best Current Practice 47) specification.
/// </summary>
/// <remarks>
/// <para>
/// BCP 47 is the most widely used standard for identifying languages,
/// scripts, regions and their combinations.
/// </para>
/// <para>
/// Examples of valid BCP 47 codes include:
/// <list type="bullet">
/// <item><c>en</c></item>
/// <item><c>en-US</c></item>
/// <item><c>ru</c></item>
/// <item><c>ru-KZ</c></item>
/// <item><c>zh-Hans</c></item>
/// <item><c>zh-Hant-TW</c></item>
/// </list>
/// </para>
/// <para>
/// Instances of <see cref="Bcp47Code"/> are immutable and thread-safe.
/// </para>
/// </remarks>
public sealed class Bcp47Code : CultureCode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Bcp47Code"/> class.
    /// </summary>
    /// <param name="value">
    /// The textual representation of the BCP 47 culture code.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty or contains only
    /// white-space characters.
    /// </exception>
    internal Bcp47Code(string value)
        : base(Normalize(value))
    {
    }

    /// <summary>
    /// Normalizes the specified BCP 47 code.
    /// </summary>
    /// <param name="value">
    /// The code to normalize.
    /// </param>
    /// <returns>
    /// A normalized representation of the specified BCP 47 code.
    /// </returns>
    private static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return value.Trim();
    }
}