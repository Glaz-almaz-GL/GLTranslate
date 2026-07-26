namespace GLTranslate.Abstractions.Linguistics.Languages.Codes
{
    /// <summary>
    /// Represents an ISO 639-1 language code.
    /// </summary>
    /// <remarks>
    /// ISO 639-1 codes consist of two lowercase letters and are used
    /// to identify languages in a compact format.
    /// </remarks>
    public sealed class Iso6391Code(string value) :
        LanguageCode(value),
        IEquatable<Iso6391Code>
    {
        public bool Equals(Iso6391Code? other)
        {
            return other is not null &&
                   Value.Equals(
                       other.Value,
                       StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is Iso6391Code other &&
                   Equals(other);
        }

        public static bool operator ==(
            Iso6391Code? left,
            Iso6391Code? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(
            Iso6391Code? left,
            Iso6391Code? right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
