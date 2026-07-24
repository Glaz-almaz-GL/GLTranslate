namespace GLTranslate.Abstractions.Languages.Codes
{
    public sealed class Bcp47Code(string value) :
        LanguageCode(value),
        IEquatable<Bcp47Code>
    {
        public bool Equals(Bcp47Code? other)
        {
            return other is not null &&
                   Value.Equals(
                       other.Value,
                       StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is Bcp47Code other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
