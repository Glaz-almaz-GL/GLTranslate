using GLTranslate.Abstractions.Results;
using GLTranslate.Providers.Google.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Extensions
{
    /// <summary>
    /// Extension methods для результатов Google Translate.
    /// </summary>
    public static class GoogleTranslationResultExtensions
    {
        /// <summary>
        /// Помечает результат как полученный от Google Translate.
        /// </summary>
        public static TranslationResult<T> AsGoogle<T>(this TranslationResult<T> result)
        {
            return result with { ServiceName = GoogleConstants.ServiceName };
        }
    }
}
