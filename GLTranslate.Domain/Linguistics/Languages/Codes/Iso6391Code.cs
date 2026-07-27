using GLTranslate.Abstractions.Linguistics.Languages;

namespace GLTranslate.Domain.Linguistics.Languages.Codes
{
    /// <summary>
    /// Represents an ISO 639-1 language code.
    /// </summary>
    public sealed class Iso6391Code(string value) : LanguageCode(Normalize(value))
    {
        /// <summary>
        /// Normalizes an ISO 639-1 code value.
        /// </summary>
        /// <param name="value">
        /// The original code value.
        /// </param>
        /// <returns>
        /// A normalized lowercase ISO 639-1 code.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the value is empty or invalid.
        /// </exception>
        public static string Normalize(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            string normalized = value
                .Trim()
                .ToLowerInvariant();


            if (normalized.Length != 2)
            {
                // ISO 639-1 language code must contain exactly two characters. (ru, us)
                throw new ArgumentException("ISO 639-1 language code must contain exactly two characters.", nameof(value));
            }

            return normalized;
        }
    }
}