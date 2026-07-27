using GLTranslate.Abstractions.Linguistics.Scripts;
using GLTranslate.Domain.Registries;
using System.Collections.Immutable;

namespace GLTranslate.Domain.Linguistics.Scripts;

/// <summary>
/// Represents an immutable registry of script entities.
/// </summary>
/// <remarks>
/// The registry provides read-only access to known scripts
/// using their identifiers.
///
/// The registry is immutable and thread-safe.
/// </remarks>
public sealed class ScriptRegistry(IEnumerable<Script> entities) : ImmutableRegistry<Script, ScriptId>(entities)
{
    /// <summary>
    /// Gets the default registry containing the built-in scripts.
    /// </summary>
    public static readonly ScriptRegistry Default = new(CreateDefaultScripts());

    private static ImmutableArray<Script> CreateDefaultScripts()
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