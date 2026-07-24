using System.Diagnostics;

namespace GLTranslate.Abstractions.Languages;

/// <summary>
/// Represents a human language supported by GLTranslate.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Language"/> is the canonical representation of a natural language
/// within the GLTranslate ecosystem.
/// </para>
/// <para>
/// A language is independent from any specific translation provider.
/// Provider-specific identifiers are resolved internally by provider
/// implementations and are intentionally not exposed by this type.
/// </para>
/// <para>
/// Instances of <see cref="Language"/> are immutable and thread-safe.
/// </para>
/// </remarks>

[DebuggerDisplay("{Id,nq} ({Name})")]
public sealed class Language : IEquatable<Language>
{
    /// <summary>
    /// The unique identifier that represents the language
    /// within the GLTranslate ecosystem.
    /// </summary>
    public LanguageId Id { get; }

    /// <summary>
    /// Gets the English name of the language.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the native name of the language.
    /// </summary>
    public string NativeName { get; }

    /// <summary>
    /// Gets the primary writing system of the language.
    /// </summary>
    public LanguageScript Script { get; }

    /// <summary>
    /// Gets the writing direction of the language.
    /// </summary>
    public LanguageDirection Direction { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Language"/> class.
    /// </summary>
    /// <param name="id">
    /// The provider-independent language code that uniquely identifies the language.
    /// </param>
    /// <param name="name">
    /// The English name of the language.
    /// </param>
    /// <param name="nativeName">
    /// The native name of the language written in its own writing system.
    /// </param>
    /// <param name="script">
    /// The primary writing system used by the language.
    /// </param>
    /// <param name="direction">
    /// The writing direction of the language.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when one of the required arguments is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> or
    /// <paramref name="nativeName"/> is empty or consists only of white-space characters.
    /// </exception>
    internal Language(
        LanguageId id,
        string name,
        string nativeName,
        LanguageScript script,
        LanguageDirection direction)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(script);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(nativeName);

        Id = id;

        Name = name;
        NativeName = nativeName;
        Script = script;
        Direction = direction;
    }

    /// <inheritdoc />
    public bool Equals(Language? other)
    {
        return ReferenceEquals(this, other) || (other is not null && Id.Equals(other.Id));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Language other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Determines whether two <see cref="Language"/> instances represent the same language.
    /// </summary>
    public static bool operator ==(Language? left, Language? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two <see cref="Language"/> instances represent different languages.
    /// </summary>
    public static bool operator !=(Language? left, Language? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Returns a string representation of the current language.
    /// </summary>
    /// <returns>
    /// The value of <see cref="Name"/>.
    /// </returns>
    public override string ToString()
    {
        return Name;
    }
}