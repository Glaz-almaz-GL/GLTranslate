namespace GLTranslate.Abstractions.Translation;

/// <summary>
/// Represents the public entry point for performing text translation.
/// </summary>
/// <remarks>
/// Implementations are expected to be immutable and thread-safe.
/// </remarks>
public interface ITextTranslationClient : ITranslationClient<TextTranslationRequest, TextTranslationResult>;
