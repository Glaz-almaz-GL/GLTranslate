using System.Globalization;

namespace GLTranslate.Abstractions.Languages
{
    /// <summary>
    /// Методы расширения для ILanguage.
    /// </summary>
    public static class LanguageExtensions
    {
        /// <summary>
        /// Получает CultureInfo для данного языка.
        /// </summary>
        public static CultureInfo? GetCultureInfo(this ILanguage language)
        {
            ArgumentNullException.ThrowIfNull(language);

            try
            {
                return CultureInfo.GetCultureInfo(language.Bcp47Tag);
            }
            catch (CultureNotFoundException)
            {
                try
                {
                    return CultureInfo.GetCultureInfo(language.Iso6391);
                }
                catch (CultureNotFoundException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Проверяет валидность языка.
        /// </summary>
        public static bool IsValid(this ILanguage language)
        {
            return language != null &&
                   !string.IsNullOrWhiteSpace(language.Iso6391) &&
                   !string.IsNullOrWhiteSpace(language.Iso6393) &&
                   !string.IsNullOrWhiteSpace(language.Name) &&
                   !string.IsNullOrWhiteSpace(language.NativeName) &&
                   !string.IsNullOrWhiteSpace(language.Bcp47Tag) &&
                   !string.IsNullOrWhiteSpace(language.Iso15924);
        }
    }
}