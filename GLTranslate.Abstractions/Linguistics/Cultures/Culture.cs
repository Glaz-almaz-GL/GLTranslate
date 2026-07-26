using GLTranslate.Abstractions.Common;
using GLTranslate.Abstractions.Interfaces;
using GLTranslate.Abstractions.Linguistics.Cultures.Codes;
using GLTranslate.Abstractions.Linguistics.Languages;
using GLTranslate.Abstractions.Linguistics.Regions;
using GLTranslate.Abstractions.Linguistics.Scripts;
using System.Diagnostics;

namespace GLTranslate.Abstractions.Linguistics.Cultures;

/// <summary>
/// Represents a linguistic culture supported by GLTranslate.
/// </summary>
[DebuggerDisplay("{Id,nq}")]
public sealed class Culture :
    IIdentifiable<CultureId>,
    IEquatable<Culture>
{
    #region Properties

    /// <summary>
    /// Gets the unique identifier of the culture within GLTranslate.
    /// </summary>
    public CultureId Id { get; }

    /// <summary>
    /// Gets the language represented by the culture.
    /// </summary>
    public Language Language { get; }

    /// <summary>
    /// Gets the region associated with the culture.
    /// </summary>
    public Region? Region { get; }

    /// <summary>
    /// Gets the writing system associated with the culture.
    /// </summary>
    public Script? Script { get; }

    /// <summary>
    /// Gets the set of all code representations associated with this culture.
    /// </summary>
    public CodeSet<CultureCode> Codes { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Culture"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="id"/>, <paramref name="language"/>, or <paramref name="codes"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="codes"/> contains null or duplicate elements.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the specified <paramref name="script"/> is not supported by the specified <paramref name="language"/>.
    /// </exception>
    internal Culture(
        CultureId id,
        Language language,
        Region? region,
        Script? script,
        IEnumerable<CultureCode> codes)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(language);
        ArgumentNullException.ThrowIfNull(codes);

        if (script is not null && !language.Scripts.Contains(script))
        {
            throw new InvalidOperationException("The specified writing system is not supported by the language.");
        }

        Id = id;
        Language = language;
        Region = region;
        Script = script;

        Codes = new CodeSet<CultureCode>(codes);
    }

    #endregion

    #region Equality Members

    /// <inheritdoc />
    public bool Equals(Culture? other)
    {
        return ReferenceEquals(this, other) || (other is not null && Id.Equals(other.Id));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Culture other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Determines whether two <see cref="Culture"/> instances represent the same culture.
    /// </summary>
    public static bool operator ==(Culture? left, Culture? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two <see cref="Culture"/> instances represent different cultures.
    /// </summary>
    public static bool operator !=(Culture? left, Culture? right)
    {
        return !Equals(left, right);
    }

    #endregion

    #region Object Overrides

    /// <inheritdoc />
    public override string ToString()
    {
        return Id.ToString();
    }

    #endregion
}