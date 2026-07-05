using GLTranslate.Abstractions.Languages;

namespace GLTranslate.Abstractions.Results
{
    /// <summary>
    /// Результат перевода, содержащий исходный и переведённый контент, языки и дополнительную информацию.
    /// </summary>
    /// <typeparam name="T">Тип содержимого перевода (например, string, byte[], и т.д.).</typeparam>
    public interface ITranslationResult<out T>
    {
        #region Language Properties

        /// <summary>
        /// Исходный язык перевода.
        /// </summary>
        ILanguage FromLanguage { get; }

        /// <summary>
        /// Целевой язык перевода.
        /// </summary>
        ILanguage ToLanguage { get; }

        /// <summary>
        /// Автоматически определённый исходный язык перевода.
        /// Null если определение языка не поддерживается или не удалось.
        /// </summary>
        ILanguage? DetectedLanguage { get; }

        #endregion

        #region Content Properties

        /// <summary>
        /// Оригинальное содержимое перевода.
        /// </summary>
        T Original { get; }

        /// <summary>
        /// Переведённое содержимое перевода.
        /// </summary>
        T Translated { get; }

        /// <summary>
        /// Альтернативные варианты перевода.
        /// Null если альтернативы отсутствуют или не поддерживаются.
        /// </summary>
        IReadOnlyList<T>? Alternatives { get; }

        #endregion

        #region Additional Properties

        /// <summary>
        /// Имя сервиса перевода, который был использован для выполнения перевода.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Время выполнения перевода.
        /// </summary>
        DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Длительность выполнения перевода.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Уровень уверенности в точности перевода (от 0.0 до 1.0).
        /// </summary>
        float? Confidence { get; }

        #endregion
    }
}
