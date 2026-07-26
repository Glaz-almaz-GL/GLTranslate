namespace GLTranslate.Abstractions.Linguistics.Languages.Codes
{
    public sealed class Iso6393Code(string value) :
        LanguageCode(value),
        IEquatable<Iso6393Code>
    {
        public bool Equals(Iso6393Code? other)
        {
            return other is not null &&
                   Value.Equals(
                       other.Value,
                       StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is Iso6393Code other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
