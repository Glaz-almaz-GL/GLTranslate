using GLTranslate.Abstractions.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Models.Results
{
    /// <summary>
    /// Результат Google Translate для текста с транслитерацией.
    /// Содержит перевод и опциональную транслитерацию.
    /// </summary>
    public readonly record struct GoogleTextTranslationResult
    {
        /// <summary>
        /// Результат перевода.
        /// </summary>
        public GoogleTranslationResult<string> Translation { get; init; }

        /// <summary>
        /// Транслитерация исходного текста (если доступна).
        /// </summary>
        public GoogleTransliterationResult? SourceTransliteration { get; init; }

        /// <summary>
        /// Транслитерация переведённого текста (если доступна).
        /// </summary>
        public GoogleTransliterationResult? TargetTransliteration { get; init; }

        /// <summary>
        /// Возвращает true, если транслитерация доступна.
        /// </summary>
        public bool HasTransliteration => SourceTransliteration.HasValue || TargetTransliteration.HasValue;

        public readonly override string ToString()
        {
            var parts = new List<string> { Translation.ToString() };

            if (SourceTransliteration.HasValue)
                parts.Add($"[src: {SourceTransliteration.Value.Transliterated}]");

            if (TargetTransliteration.HasValue)
                parts.Add($"[tgt: {TargetTransliteration.Value.Transliterated}]");

            return string.Join(" ", parts);
        }
    }
}
