using GLTranslate.Abstractions.Linguistics.Languages;
using GLTranslate.Domain.Registries;
using System.Collections.Immutable;

namespace GLTranslate.Domain.Linguistics.Languages;

/// <summary>
/// Represents an immutable registry of language entities.
/// </summary>
/// <remarks>
/// Provides read-only lookup of languages by identifier.
/// </remarks>
public sealed class LanguageRegistry(IEnumerable<Language> languages) : ImmutableRegistry<Language, LanguageId>(languages)
{
    /// <summary>
    /// Gets the default registry containing the built-in scripts.
    /// </summary>
    public static readonly LanguageRegistry Default = new(CreateDefaultScripts());

    private static ImmutableArray<Language> CreateDefaultScripts()
    {
        return
        [
            // Latin,
            // Cyrillic,
            // Arabic,
            // ...
        ];
    }
}