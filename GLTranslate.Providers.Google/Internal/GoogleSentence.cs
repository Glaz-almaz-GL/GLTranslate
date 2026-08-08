using System.Text.Json.Serialization;

namespace GLTranslate.Providers.Google.Internal;

/// <summary>
/// Represents a single translated sentence fragment as returned by the
/// Google Translate web endpoint.
/// </summary>
/// <remarks>
/// A translated text is split into one fragment per sentence; the full
/// translation is the concatenation of every fragment's
/// <see cref="Translation"/>.
/// </remarks>
internal sealed class GoogleSentence
{
    /// <summary>
    /// Gets or sets the translated text of this fragment.
    /// </summary>
    [JsonPropertyName("trans")]
    public required string Translation { get; set; }
}
