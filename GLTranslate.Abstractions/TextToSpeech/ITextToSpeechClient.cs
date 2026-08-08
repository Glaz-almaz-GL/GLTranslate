using GLTranslate.Abstractions.Providers;

namespace GLTranslate.Abstractions.TextToSpeech;

/// <summary>
/// Represents the public entry point for performing text-to-speech
/// synthesis.
/// </summary>
/// <remarks>
/// Implementations are expected to be immutable and thread-safe.
/// </remarks>
public interface ITextToSpeechClient : IOperationClient<TextToSpeechRequest, TextToSpeechResult>;
