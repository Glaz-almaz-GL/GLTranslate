using GLTranslate.Abstractions.Translation;
using System.Net.Http.Json;
using System.Text.Json;

namespace GLTranslate.Providers.Google.Internal;

/// <summary>
/// Performs the HTTP call and response parsing required to translate text
/// through the free Google Translate web endpoint.
/// </summary>
/// <remarks>
/// <para>
/// This is the provider's engine: it contains the business logic of the
/// operation and is not part of the public API. Consumers depend on
/// <see cref="GoogleTranslationProvider"/> instead.
/// </para>
/// <para>
/// This type is thread-safe as long as the supplied <see cref="HttpClient"/>
/// is thread-safe, which is the case for any <see cref="HttpClient"/> not
/// otherwise mutated after construction.
/// </para>
/// </remarks>
internal sealed class GoogleTranslationEngine
{
    private const string ApiEndpoint = "https://translate.googleapis.com/translate_a/single";
    private const string ProviderName = "Google";

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleTranslationEngine"/> class.
    /// </summary>
    /// <param name="httpClient">
    /// The HTTP client used to send requests to the Google Translate web
    /// endpoint. The engine does not own its lifetime.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="httpClient"/> is <see langword="null"/>.
    /// </exception>
    public GoogleTranslationEngine(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
    }

    /// <summary>
    /// Translates text from one ISO 639-1 language code to another.
    /// </summary>
    /// <param name="text">
    /// The text to translate.
    /// </param>
    /// <param name="sourceLanguageCode">
    /// The ISO 639-1 code of the source language, or <c>"auto"</c> to
    /// request automatic detection.
    /// </param>
    /// <param name="targetLanguageCode">
    /// The ISO 639-1 code of the target language.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that completes with the translated text and the ISO 639-1
    /// code of the source language that was actually used.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="text"/>, <paramref name="sourceLanguageCode"/>
    /// or <paramref name="targetLanguageCode"/> is empty or consists only
    /// of white-space characters.
    /// </exception>
    /// <exception cref="TranslationProviderException">
    /// Thrown when the request fails, or when the response cannot be
    /// understood.
    /// </exception>
    public async Task<(string TranslatedText, string SourceLanguageCode)> TranslateAsync(
        string text,
        string sourceLanguageCode,
        string targetLanguageCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceLanguageCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetLanguageCode);

        string token = GoogleTokenGenerator.Generate(text);

        string url = $"{ApiEndpoint}?client=gtx" +
                     $"&sl={Uri.EscapeDataString(sourceLanguageCode)}" +
                     $"&tl={Uri.EscapeDataString(targetLanguageCode)}" +
                     "&dt=t&dt=bd&dj=1&source=input" +
                     $"&tk={Uri.EscapeDataString(token)}";

        using FormUrlEncodedContent content = new([new KeyValuePair<string, string>("q", text)]);

        GoogleTranslationResponse response;

        try
        {
            using HttpResponseMessage httpResponse = await _httpClient
                .PostAsync(new Uri(url), content, cancellationToken)
                .ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            response = await httpResponse.Content
                .ReadFromJsonAsync(GoogleTranslationJsonContext.Default.GoogleTranslationResponse, cancellationToken)
                .ConfigureAwait(false)
                ?? throw new TranslationProviderException(ProviderName, "Google Translate returned an empty response.");
        }
        catch (HttpRequestException exception)
        {
            throw new TranslationProviderException(ProviderName, "The request to Google Translate failed.", exception);
        }
        catch (JsonException exception)
        {
            throw new TranslationProviderException(ProviderName, "Google Translate returned an unexpected response format.", exception);
        }

        string translation = response.Sentences is null
            ? string.Empty
            : string.Concat(response.Sentences.Select(sentence => sentence.Translation));

        return (translation, response.Source);
    }
}
