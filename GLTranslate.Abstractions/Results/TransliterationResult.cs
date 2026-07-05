using GLTranslate.Abstractions.Languages;
using System;

namespace GLTranslate.Abstractions.Results
{
    /// <inheritdoc/>
    public readonly record struct TransliterationResult : ITransliterationResult
    {
        #region Language Properties

        /// <inheritdoc/>
        public ILanguage Language { get; init; }

        /// <inheritdoc/>
        public string SourceScript { get; init; }

        /// <inheritdoc/>
        public string TargetScript { get; init; }

        #endregion

        #region Content Properties

        /// <inheritdoc/>
        public string Original { get; init; }

        /// <inheritdoc/>
        public string Transliterated { get; init; }

        #endregion

        #region Additional Properties

        /// <inheritdoc/>
        public string ServiceName { get; init; }

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; init; }

        /// <inheritdoc/>
        public TimeSpan Duration { get; init; }

        #endregion

        public readonly override string ToString()
        {
            return $"[{ServiceName}] \"{Original}\" -> \"{Transliterated}\" " +
                   $"({Language.Iso6391}: {SourceScript} -> {TargetScript})";
        }
    }
}