namespace GLTranslate.Abstractions.Regions.Codes;

public abstract class RegionCode
{
    protected RegionCode(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}

/// <summary>
/// Provides equality comparison for region codes based on type and value.
/// </summary>
public sealed class RegionCodeComparer : IEqualityComparer<RegionCode>
{
    public static readonly RegionCodeComparer Instance = new();

    private RegionCodeComparer() { }

    public bool Equals(RegionCode? x, RegionCode? y)
    {
        return ReferenceEquals(x, y) || (x is not null && y is not null && x.GetType() == y.GetType() &&
               string.Equals(x.Value, y.Value, StringComparison.OrdinalIgnoreCase));
    }

    public int GetHashCode(RegionCode obj)
    {
        return HashCode.Combine(
            obj.GetType(),
            StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Value));
    }
}