using GLTranslate.Abstractions.Linguistics.Languages;

namespace GLTranslate.Abstractions.Translation;

/// <summary>
/// Represents the immutable parameters of a single text-to-speech
/// operation.
/// </summary>
/// <remarks>
/// Instances of this class are immutable and thread-safe.
/// </remarks>
public sealed class TextToSpeechRequest : ProviderRequest
{
    /// <summary>
    /// Gets the text to synthesize.
    /// </summary>
    public TranslationText Text { get; }

    /// <summary>
    /// Gets the identifier of the language the text is written in, which
    /// determines the voice used for synthesis.
    /// </summary>
    public LanguageId LanguageId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextToSpeechRequest"/> class.
    /// </summary>
    /// <param name="text">
    /// The text to synthesize.
    /// </param>
    /// <param name="languageId">
    /// The identifier of the language the text is written in.
    /// </param>
    /// <param name="id">
    /// The request identifier, or <see langword="null"/> to generate a new one.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="text"/> or <paramref name="languageId"/>
    /// is <see langword="null"/>.
    /// </exception>
    public TextToSpeechRequest(TranslationText text, LanguageId languageId, RequestId? id = null)
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(languageId);

        Text = text;
        LanguageId = languageId;
    }
}
