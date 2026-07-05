using GLTranslate.Abstractions.Languages;

namespace GLTranslate.Abstractions.Results
{
    /// <inheritdoc/>
    public readonly record struct TranslationResult<T> : ITranslationResult<T>
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
        public string ServiceName { get; init; }

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; init; }

        /// <inheritdoc/>
        public TimeSpan Duration { get; init; }

        /// <inheritdoc/>
        public float? Confidence { get; init; }

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
