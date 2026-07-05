namespace GLTranslate.Abstractions.Languages
{
    /// <summary>
    /// Представляет язык с полной метаинформацией для систем перевода.
    /// </summary>
    public interface ILanguage
    {
        /// <summary>
        /// Название языка на английском языке. Например: "Russian", "English"
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Название языка на родном языке. Например: "Русский", "English"
        /// </summary>
        string NativeName { get; }

        /// <summary>
        /// Двухбуквенный код языка (ISO 639-1). Например: "en", "ru", "zh"
        /// </summary>
        string Iso6391 { get; }

        /// <summary>
        /// Трёхбуквенный код языка (ISO 639-3). Например: "eng", "rus", "zho"
        /// </summary>
        string Iso6393 { get; }

        /// <summary>
        /// BCP 47 тег языка. Например: "en", "zh-Hans", "pt-BR"
        /// </summary>
        string Bcp47Tag { get; }

        /// <summary>
        /// Код скрипта письменности (ISO 15924). Например: "Latn", "Cyrl", "Arab"
        /// </summary>
        string Iso15924 { get; }

        /// <summary>
        /// Направление письма: true для RTL (арабский, иврит), false для LTR
        /// </summary>
        bool IsRightToLeft { get; }
    }
}