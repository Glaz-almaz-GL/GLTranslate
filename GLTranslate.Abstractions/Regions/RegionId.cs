namespace GLTranslate.Abstractions.Regions;

/// <summary>
/// Represents the unique identifier of a region within GLTranslate.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="RegionId"/> identifies a region independently from
/// any external coding standard.
/// </para>
/// <para>
/// External region codes such as ISO 3166 are represented separately
/// by <see cref="RegionCode"/> implementations.
/// </para>
/// </remarks>
public sealed class RegionId : IEquatable<RegionId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegionId"/> class.
    /// </summary>
    /// <param name="value">
    /// The unique identifier value of the region.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty or contains only whitespace characters.
    /// </exception>
    public RegionId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }

    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public string Value { get; }


    /// <inheritdoc/>
    public bool Equals(RegionId? other)
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
        return obj is RegionId other &&
               Equals(other);
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase
            .GetHashCode(Value);
    }


    /// <summary>
    /// Determines whether two <see cref="RegionId"/> instances represent
    /// the same region.
    /// </summary>
    public static bool operator ==(
        RegionId? left,
        RegionId? right)
    {
        return Equals(left, right);
    }


    /// <summary>
    /// Determines whether two <see cref="RegionId"/> instances represent
    /// different regions.
    /// </summary>
    public static bool operator !=(
        RegionId? left,
        RegionId? right)
    {
        return !Equals(left, right);
    }


    /// <inheritdoc/>
    public override string ToString()
    {
        return Value;
    }
}