using GLTranslate.Abstractions.Common;

namespace GLTranslate.Abstractions.Translation;

/// <summary>
/// Represents the MIME content type of synthesized audio data (for
/// example <c>"audio/mpeg"</c>).
/// </summary>
/// <remarks>
/// Instances of this class are immutable and thread-safe.
/// </remarks>
public sealed class AudioContentType(string value) : StringValueObject(value);
