using GLTranslate.Abstractions.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Generators
{
    /// <summary>
    /// Маппинг кодов языков для Google Translate API.
    /// Google использует нестандартные коды для некоторых языков.
    /// </summary>
    internal static class GoogleLanguageMapper
    {
        /// <summary>
        /// Преобразует стандартный код языка в код Google.
        /// </summary>
        public static string ToGoogleCode(ILanguage language)
        {
            ArgumentNullException.ThrowIfNull(language);
            return ToGoogleCode(language.Iso6391);
        }

        /// <summary>
        /// Преобразует стандартный код языка в код Google.
        /// </summary>
        public static string ToGoogleCode(string languageCode)
        {
            ArgumentNullException.ThrowIfNull(languageCode);

            return languageCode switch
            {
                // Google использует расширенные BCP47 теги для некоторых языков
                "mni" => "mni-Mtei",   // Манипури
                "he" => "iw",          // Google использует "iw" вместо "he"
                "jv" => "jw",          // Google использует "jw" вместо "jv"
                "prs" => "fa-AF",      // Дари (Афганистан)
                "sat" => "sat-Olck",   // Сантали
                "zh" => "zh-CN",       // Китайский упрощённый
                _ => languageCode
            };
        }

        /// <summary>
        /// Преобразует код Google обратно в стандартный код.
        /// </summary>
        public static string FromGoogleCode(string googleCode)
        {
            ArgumentNullException.ThrowIfNull(googleCode);

            return googleCode switch
            {
                "iw" => "he",
                "jw" => "jv",
                "mni-Mtei" => "mni",
                "fa-AF" => "prs",
                "sat-Olck" => "sat",
                "zh-CN" => "zh",
                _ => googleCode
            };
        }
    }
}
