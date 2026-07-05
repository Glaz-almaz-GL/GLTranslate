using GLTranslate.Abstractions.Languages;
using GLTranslate.Abstractions.Results;

namespace GLTranslate.Abstractions.Services
{
    /// <summary>
    /// Сервис транслитерации текста, предоставляющий методы для преобразования текста
    /// из одной письменной системы в другую в рамках одного языка.
    /// </summary>
    public interface ITransliterationService : ILanguageSupport, IDisposable
    {
        #region Transliterate Methods

        /// <summary>
        /// Транслитерирует указанный текст, преобразуя его из одного скрипта в другой.
        /// </summary>
        /// <param name="content">Текст для транслитерации.</param>
        /// <param name="language">Язык текста (не меняется при транслитерации).</param>
        /// <param name="targetScript">Целевой скрипт (ISO 15924). Например: "Latn", "Cyrl".</param>
        /// <param name="sourceScript">
        /// Исходный скрипт (ISO 15924).
        /// Если null, используется значение из <see cref="ILanguage.Iso15924"/>.
        /// </param>
        /// <returns>Результат транслитерации.</returns>
        ITransliterationResult Transliterate(
            string content,
            ILanguage language,
            string targetScript,
            string? sourceScript = null);

        /// <summary>
        /// Асинхронно транслитерирует указанный текст.
        /// </summary>
        Task<ITransliterationResult> TransliterateAsync(
            string content,
            ILanguage language,
            string targetScript,
            string? sourceScript = null,
            CancellationToken cancellationToken = default);

        #endregion

        #region Batch Transliterate Methods

        /// <summary>
        /// Транслитерирует пакет текстов.
        /// </summary>
        IReadOnlyList<ITransliterationResult> TransliterateBatch(
            IEnumerable<string> contents,
            ILanguage language,
            string targetScript,
            string? sourceScript = null);

        /// <summary>
        /// Асинхронно транслитерирует пакет текстов.
        /// </summary>
        Task<IReadOnlyList<ITransliterationResult>> TransliterateBatchAsync(
            IEnumerable<string> contents,
            ILanguage language,
            string targetScript,
            string? sourceScript = null,
            CancellationToken cancellationToken = default);

        #endregion

        #region Try Transliterate Methods

        /// <summary>
        /// Пытается транслитерировать указанный текст.
        /// </summary>
        bool TryTransliterate(
            string content,
            ILanguage language,
            string targetScript,
            out ITransliterationResult? result,
            string? sourceScript = null);

        /// <summary>
        /// Асинхронно пытается транслитерировать указанный текст.
        /// </summary>
        Task<(bool Success, ITransliterationResult? Result)> TryTransliterateAsync(
            string content,
            ILanguage language,
            string targetScript,
            string? sourceScript = null,
            CancellationToken cancellationToken = default);

        #endregion
    }
}