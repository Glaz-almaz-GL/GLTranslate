using GLTranslate.Abstractions.Languages;

namespace GLTranslate.Abstractions.Results
{
    /// <summary>
    /// Результат транслитерации текста.
    /// </summary>
    public interface ITransliterationResult
    {
        #region Language Properties

        /// <summary>
        /// Язык текста (не меняется при транслиретации).
        /// </summary>
        ILanguage Language { get; }

        /// <summary>
        /// Исходный скрипт (ISO 15924). Например: "Latn", "Cyrl".
        /// </summary>
        string SourceScript { get; }

        /// <summary>
        /// Целевой скрипт (ISO 15924). Например: "Latn", "Cyrl".
        /// </summary>
        string TargetScript { get; }

        #endregion

        #region Content Properties

        /// <summary>
        /// Исходный текст.
        /// </summary>
        string Original { get; }

        /// <summary>
        /// Транслитерированный текст.
        /// </summary>
        string Transliterated { get; }

        #endregion

        #region Additional Properties

        /// <summary>
        /// Имя сервиса транслитерации, который был использован для выполнения операции.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Время выполнения операции.
        /// </summary>
        DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Длительность операции.
        /// </summary>
        TimeSpan Duration { get; }

        #endregion
    }
}
