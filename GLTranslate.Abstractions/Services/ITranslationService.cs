using GLTranslate.Abstractions.Languages;
using GLTranslate.Abstractions.Results;

namespace GLTranslate.Abstractions.Services
{
    /// <summary>
    /// Сервис перевода контента между языками.
    /// </summary>
    /// <typeparam name="T">Тип контента (например, string, byte[], и т.д.).</typeparam>
    public interface ITranslationService<T> : ILanguageSupport, IDisposable
    {
        #region Translate Methods

        /// <summary>
        /// Переводит указанный контент на целевой язык.
        /// </summary>
        /// <param name="content">Контент для перевода.</param>
        /// <param name="toLanguage">Целевой язык.</param>
        /// <param name="fromLanguage">Исходный язык (необязательный параметр).</param>
        /// <returns>Результат перевода с метаинформацией.</returns>
        ITranslationResult<T> Translate(T content, ILanguage toLanguage, ILanguage? fromLanguage = null);

        /// <summary>
        /// Асинхронно переводит указанный контент на целевой язык.
        /// </summary>
        /// <param name="content">Контент для перевода.</param>
        /// <param name="toLanguage">Целевой язык.</param>
        /// <param name="fromLanguage">Исходный язык (необязательный параметр).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат перевода с метаинформацией.</returns>
        Task<ITranslationResult<T>> TranslateAsync(T content, ILanguage toLanguage, ILanguage? fromLanguage = null, CancellationToken cancellationToken = default);

        #endregion

        #region Batch Translate Methods

        /// <summary>
        /// Переводит пакет контента на целевой язык.
        /// </summary>
        /// <param name="contents">Коллекция контента для перевода.</param>
        /// <param name="toLanguage">Целевой язык.</param>
        /// <param name="fromLanguage">Исходный язык (необязательный параметр).</param>
        /// <returns>Список результатов перевода с метаинформацией.</returns>
        IReadOnlyList<ITranslationResult<T>> TranslateBatch(IEnumerable<T> contents, ILanguage toLanguage, ILanguage? fromLanguage = null);

        /// <summary>
        /// Асинхронно переводит пакет контента на целевой язык.
        /// </summary>
        /// <param name="contents">Коллекция контента для перевода.</param>
        /// <param name="toLanguage">Целевой язык.</param>
        /// <param name="fromLanguage">Исходный язык (необязательный параметр).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список результатов перевода с метаинформацией.</returns>
        Task<IReadOnlyList<ITranslationResult<T>>> TranslateBatchAsync(IEnumerable<T> contents, ILanguage toLanguage, ILanguage? fromLanguage = null, CancellationToken cancellationToken = default);

        #endregion

        #region Try Translate Methods

        /// <summary>
        /// Пытается перевести указанный контент на целевой язык.
        /// </summary>
        /// <param name="content">Контент для перевода.</param>
        /// <param name="toLanguage">Целевой язык.</param>
        /// <param name="fromLanguage">Исходный язык.</param>
        /// <param name="result">Результат перевода, если операция успешна; иначе null.</param>
        /// <returns>Флаг успешности операции.</returns>
        bool TryTranslate(T content, ILanguage toLanguage, ILanguage? fromLanguage, out ITranslationResult<T>? result);

        /// <summary>
        /// Асинхронно пытается перевести указанный контент на целевой язык.
        /// </summary>
        /// <param name="content">Контент для перевода.</param>
        /// <param name="toLanguage">Целевой язык.</param>
        /// <param name="fromLanguage">Исходный язык (необязательный параметр).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Кортеж с флагом успешности и результатом перевода.</returns>
        Task<(bool Success, ITranslationResult<T>? Result)> TryTranslateAsync(T content, ILanguage toLanguage, ILanguage? fromLanguage = null, CancellationToken cancellationToken = default);

        #endregion
    }
}