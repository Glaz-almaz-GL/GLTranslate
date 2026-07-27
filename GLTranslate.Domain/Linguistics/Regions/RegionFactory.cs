using GLTranslate.Domain.Linguistics.Regions.Generated;

namespace GLTranslate.Domain.Linguistics.Regions;

/// <summary>
/// Looks up known regions by their ISO 3166-1 alpha-2 code.
/// </summary>
/// <remarks>
/// A convenience entry point for callers who identify regions by their
/// ISO code rather than by <see cref="RegionId"/>. All regions are already
/// preloaded by <see cref="RegionRegistry"/>; this type only adds an
/// alpha-2-keyed lookup on top of that data.
///
/// This type is immutable and thread-safe.
/// </remarks>
public static class RegionFactory
{
    /// <summary>
    /// Gets the region associated with the specified ISO 3166-1 alpha-2 code.
    /// </summary>
    /// <param name="alpha2">
    /// The ISO 3166-1 alpha-2 code of the region (e.g. <c>"US"</c>, <c>"RU"</c>).
    /// </param>
    /// <returns>
    /// The <see cref="Region"/> associated with the specified code.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="alpha2"/> is empty or consists only of
    /// white-space characters.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when <paramref name="alpha2"/> is not a known ISO 3166-1 region.
    /// </exception>
    public static Region Get(string alpha2)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alpha2);

        if (!TryGet(alpha2, out Region? region))
        {
            // Unknown region code
            throw new KeyNotFoundException($"'{alpha2}' is not a known ISO 3166-1 region.");
        }

        return region!;
    }

    /// <summary>
    /// Attempts to get the region associated with the specified ISO 3166-1
    /// alpha-2 code.
    /// </summary>
    /// <param name="alpha2">
    /// The ISO 3166-1 alpha-2 code of the region.
    /// </param>
    /// <param name="region">
    /// The region when found; otherwise <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the code is a known ISO 3166-1 region;
    /// otherwise <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="alpha2"/> is empty or consists only of
    /// white-space characters.
    /// </exception>
    public static bool TryGet(string alpha2, out Region? region)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alpha2);

        return RegionRegistryData.ByAlpha2.TryGetValue(alpha2.Trim().ToUpperInvariant(), out region);
    }
}
