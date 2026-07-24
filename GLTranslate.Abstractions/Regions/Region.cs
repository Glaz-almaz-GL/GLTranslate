using GLTranslate.Abstractions.Regions.Codes;

namespace GLTranslate.Abstractions.Regions;

/// <summary>
/// Represents a geographic region used for localization purposes.
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="Region"/> represents a geographic or cultural region
/// independently from any external coding standard.
/// </para>
/// <para>
/// Region-specific codes such as ISO 3166 codes are provided through
/// separate region code implementations.
/// </para>
/// <para>
/// Instances of <see cref="Region"/> are immutable and thread-safe.
/// </para>
/// </remarks>
public sealed class Region : IEquatable<Region>
{
    /// <summary>
    /// Gets the unique region identifier.
    /// </summary>
    public RegionId Id { get; }


    /// <summary>
    /// Gets the English name of the region.
    /// </summary>
    public string Name { get; }

    public IReadOnlyCollection<RegionCode> Codes { get; }


    /// <summary>
    /// Initializes a new instance of the <see cref="Region"/> class.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the region.
    /// </param>
    /// <param name="name">
    /// The English name of the region.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="id"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is empty or contains only whitespace characters.
    /// </exception>
    internal Region(
        RegionId id,
        string name,
        IReadOnlyCollection<RegionCode> codes)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(codes);

        if (codes.Count == 0)
        {
            throw new ArgumentException(
                "Region must contain at least one region code.",
                nameof(codes));
        }

        Id = id;
        Name = name;
        Codes = [.. codes];
    }


    /// <inheritdoc/>
    public bool Equals(Region? other)
    {
        return ReferenceEquals(this, other) ||
               (other is not null &&
                Id.Equals(other.Id));
    }


    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Region other &&
               Equals(other);
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }


    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}