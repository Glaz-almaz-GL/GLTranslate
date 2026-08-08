using GLTranslate.Abstractions.Common;

namespace GLTranslate.Abstractions.Translation;

/// <summary>
/// Represents a piece of text involved in a translation operation, either
/// as the source text to translate or as the translated output.
/// </summary>
/// <remarks>
/// Instances of this class are immutable and thread-safe.
/// </remarks>
public sealed class TranslationText(string value) : StringValueObject(value);
