using GLTranslate.Abstractions.Languages;
using GLTranslate.Abstractions.Results;
using GLTranslate.Providers.Google.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Models.Results
{
    /// <summary>
    /// Представляет результат перевода от Google Translate.
    /// </summary>
    /// <typeparam name="T">Тип контента, который был переведен (например, string, byte[], и т.д.).</typeparam>
    public readonly record struct GoogleTranslationResult<T> : ITranslationResult<T>
    {
        #region Language Properties

        /// <inheritdoc/>
        public ILanguage FromLanguage { get; init; }

        /// <inheritdoc/>
        public ILanguage ToLanguage { get; init; }

        /// <inheritdoc/>
        public ILanguage? DetectedLanguage { get; init; }

        #endregion

        #region Content Properties

        /// <inheritdoc/>
        public T Original { get; init; }

        /// <inheritdoc/>
        public T Translated { get; init; }

        /// <inheritdoc/>
        public IReadOnlyList<T>? Alternatives { get; init; }

        #endregion

        #region Additional Properties

        /// <inheritdoc/>
        public readonly string ServiceName => GoogleConstants.ServiceName;

        /// <inheritdoc/>
        public float? Confidence { get; init; }

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; init; }

        /// <inheritdoc/>
        public TimeSpan Duration { get; init; }

        #endregion

        public override readonly string ToString()
        {
            string detected = DetectedLanguage != null
                ? $" [detected: {DetectedLanguage.Iso6391}]"
                : string.Empty;

            return $"\"{Original}\" -> \"{Translated}\" " +
                   $"({FromLanguage.Iso6391} -> {ToLanguage.Iso6391}){detected}";
        }
    }
}
