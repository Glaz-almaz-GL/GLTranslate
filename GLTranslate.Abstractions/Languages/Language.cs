namespace GLTranslate.Abstractions.Languages
{
    /// <inheritdoc/>
    public record struct Language(
        string Name,
        string NativeName,
        string Iso6391,
        string Iso6393,
        string Bcp47Tag,
        string Iso15924,
        bool IsRightToLeft) : ILanguage
    {
        /// <summary>
        /// Возвращает строковое представление языка.
        /// </summary>
        public override readonly string ToString()
        {
            return $"{Name} ({NativeName}) [{Iso6391}]";
        }
    }
}