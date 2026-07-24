using System.Diagnostics;

namespace GLTranslate.Abstractions.Languages;

/// <summary>
/// Represents the unique identifier of a language within GLTranslate.
/// </summary>
/// <remarks>
/// A <see cref="LanguageId"/> identifies a language independently from
/// any external language coding standard or translation provider.
/// </remarks>

[DebuggerDisplay("{Value}")]
public sealed class LanguageId : IEquatable<LanguageId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageId"/> class.
    /// </summary>
    /// <param name="value">
    /// The unique identifier value.
    /// </param>
    public LanguageId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }


    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public string Value { get; }


    /// <inheritdoc/>
    public bool Equals(LanguageId? other)
    {
        return ReferenceEquals(this, other) ||
            (other is not null &&
            Value.Equals(
                other.Value,
                StringComparison.OrdinalIgnoreCase));
    }


    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is LanguageId other && Equals(other);
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }


    /// <inheritdoc/>
    public override string ToString()
    {
        return Value;
    }
}