namespace GLTranslate.Providers.Google.Constants
{
    /// <summary>
    /// Константы для Google Translate API.
    /// </summary>
    internal static class GoogleConstants
    {
        /// <summary>
        /// Основной эндпоинт для перевода.
        /// </summary>
        public const string ApiEndpoint = "https://translate.googleapis.com/translate_a/single";

        /// <summary>
        /// Эндпоинт для Text-to-Speech.
        /// </summary>
        public const string TtsApiEndpoint = "https://translate.google.com/translate_tts";

        /// <summary>
        /// Эндпоинт для получения списка языков.
        /// </summary>
        public const string LanguagesEndpoint = "https://translate.google.com/translate_a/l?client=t";

        /// <summary>
        /// User-Agent для имитации браузера.
        /// </summary>
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";

        /// <summary>
        /// Максимальная длина текста для одного запроса.
        /// </summary>
        public const int MaxTextLength = 5000;

        /// <summary>
        /// Название сервиса.
        /// </summary>
        public const string ServiceName = "Google Translate";
    }
}
