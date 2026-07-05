using GLTranslate.Abstractions.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Abstractions.Services
{
    /// <summary>
    /// Базовый интерфейс для сервисов, работающих с языками.
    /// Предоставляет методы для проверки поддержки языков.
    /// </summary>
    public interface ILanguageSupport
    {
        /// <summary>
        /// Проверяет, поддерживает ли сервис указанный язык.
        /// </summary>
        /// <param name="language">Язык для проверки.</param>
        /// <returns>True если язык поддерживается.</returns>
        bool IsLanguageSupported(ILanguage language);

        /// <summary>
        /// Получает список всех языков, поддерживаемых сервисом.
        /// </summary>
        /// <returns>Список поддерживаемых языков.</returns>
        IReadOnlyList<ILanguage> GetSupportedLanguages();
    }
}
