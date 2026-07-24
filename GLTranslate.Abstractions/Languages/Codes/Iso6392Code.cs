namespace GLTranslate.Abstractions.Languages.Codes
{
    public sealed class Iso6392Code(string value) :
         LanguageCode(value),
         IEquatable<Iso6392Code>
    {
        public bool Equals(Iso6392Code? other)
        {
            return other is not null &&
                   Value.Equals(
                       other.Value,
                       StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is Iso6392Code other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
