namespace GLTranslate.Abstractions.Linguistics.Scripts.Code;

/// <summary>
/// Initializes a new instance of the <see cref="Iso15924Code"/> class.
/// </summary>
/// <param name="value">
/// The ISO 15924 script code.
/// </param>
/// <exception cref="ArgumentException">
/// Thrown when <paramref name="value"/> is not a valid ISO 15924 code.
/// </exception>
public sealed class Iso15924Code(string value) : ScriptCode(Normalize(value))
{
    private static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        if (value.Length != 4)
        {
            // ISO 15924 codes must consist of exactly four letters. (Latn, Cyrl)
            throw new ArgumentException("ISO 15924 codes must consist of exactly four letters.", nameof(value));
        }

        if (!value.All(char.IsLetter))
        {
            // ISO 15924 codes must consist of exactly four letters. (Latn and Cyrl, not Латин and Кирил)
            throw new ArgumentException("ISO 15924 codes may contain only Latin letters.", nameof(value));
        }

        return char.ToUpperInvariant(value[0]) + value[1..].ToLowerInvariant();
    }
}