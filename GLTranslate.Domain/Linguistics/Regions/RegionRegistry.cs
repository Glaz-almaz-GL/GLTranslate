using GLTranslate.Abstractions.Linguistics.Regions;
using GLTranslate.Domain.Registries;
using System.Collections.Immutable;

namespace GLTranslate.Domain.Linguistics.Regions;

/// <summary>
/// Represents an immutable registry of region entities.
/// </summary>
public sealed class RegionRegistry(IEnumerable<Region> regions) : ImmutableRegistry<Region, RegionId>(regions)
{
    /// <summary>
    /// Gets the default registry containing the built-in scripts.
    /// </summary>
    public static readonly RegionRegistry Default = new(CreateDefaultScripts());

    private static ImmutableArray<Region> CreateDefaultScripts()
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