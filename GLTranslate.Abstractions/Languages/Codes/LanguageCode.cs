namespace GLTranslate.Abstractions.Languages.Codes;

/// <summary>
/// Represents the base class for all language code representations.
/// </summary>
/// <remarks>
/// <para>
/// A language code represents a language identifier in a specific format,
/// standard, or provider-specific system.
/// </para>
/// <para>
/// Different language codes may represent the same language using different
/// textual representations.
/// </para>
/// <para>
/// Implementations of this class are immutable and thread-safe.
/// </para>
/// </remarks>
public abstract class LanguageCode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageCode"/> class.
    /// </summary>
    /// <param name="value">
    /// The textual representation of the language code.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty or contains only whitespace characters.
    /// </exception>
    protected LanguageCode(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }

    /// <summary>
    /// Gets the textual representation of the language code.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the textual representation of the current language code.
    /// </summary>
    /// <returns>
    /// The value of the current language code.
    /// </returns>
    public override string ToString()
    {
        return Value;
    }
}