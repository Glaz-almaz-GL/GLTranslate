namespace GLTranslate.Providers.Google.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при ошибках Google Translate API.
    /// </summary>
    public class GoogleTranslateException : Exception
    {
        public GoogleTranslateException() { }

        public GoogleTranslateException(string message)
            : base(message) { }

        public GoogleTranslateException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Создаёт исключение для ошибки 503 (IP заблокирован).
        /// </summary>
        public static GoogleTranslateException IPBanned()
        {
            return new GoogleTranslateException(
                "Google Translate API returned 503. Your IP may be banned due to too many requests. " +
                "Try again later or use a different IP address.");
        }

        /// <summary>
        /// Создаёт исключение для ошибки 429 (превышен лимит запросов).
        /// </summary>
        public static GoogleTranslateException RateLimitExceeded()
        {
            return new GoogleTranslateException(
                "Google Translate API rate limit exceeded. Please slow down your requests.");
        }
    }
}
