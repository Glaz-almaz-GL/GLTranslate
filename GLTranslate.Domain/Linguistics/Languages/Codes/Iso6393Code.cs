using GLTranslate.Abstractions.Linguistics.Languages;

namespace GLTranslate.Domain.Linguistics.Languages.Codes
{
    /// <summary>
    /// Represents an ISO 639-3 language code.
    /// </summary>
    public sealed class Iso6393Code(string value) : LanguageCode(Normalize(value))
    {
        /// <summary>
        /// Normalizes an ISO 639-3 language code.
        /// </summary>
        /// <param name="value">
        /// The original code value.
        /// </param>
        /// <returns>
        /// A normalized lowercase ISO 639-3 code.
        /// </returns>
        public static string Normalize(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            string normalized = value
                .Trim()
                .ToLowerInvariant();


            if (normalized.Length != 3)
            {
                // ISO 639-3 language code must contain exactly three characters. (rus, eng)
                throw new ArgumentException("ISO 639-3 language code must contain exactly three characters.", nameof(value));
            }

            return normalized;
        }
    }
}
