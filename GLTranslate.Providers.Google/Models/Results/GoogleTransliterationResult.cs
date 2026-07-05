using GLTranslate.Abstractions.Languages;
using GLTranslate.Abstractions.Results;
using GLTranslate.Providers.Google.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Models.Results
{
    /// <summary>
    /// Представляет результат транслитерации от Google Translate.
    /// </summary>
    public readonly record struct GoogleTransliterationResult : ITransliterationResult
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
        public string ServiceName => GoogleConstants.ServiceName;

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
