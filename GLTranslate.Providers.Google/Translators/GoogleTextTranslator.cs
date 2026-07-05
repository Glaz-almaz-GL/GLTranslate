using GLTranslate.Abstractions.Languages;
using GLTranslate.Abstractions.Results;
using GLTranslate.Abstractions.Services;
using GLTranslate.Abstractions.Translators;
using GLTranslate.Providers.Google.Constants;
using GLTranslate.Providers.Google.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Translators
{
    public sealed class GoogleTextTranslator(GoogleTextTranslationService translationService) : ITranslator<string>, ITransliterationService
    {
        private readonly GoogleTextTranslationService _translationService = translationService;
        private bool disposedValue;

        public string Name => GoogleConstants.ServiceName;

        public IReadOnlyList<ILanguage> GetSupportedLanguages() => _translationService.GetSupportedLanguages();
        public bool IsLanguageSupported(ILanguage language) => _translationService.IsLanguageSupported(language);

        #region Translation Methods

        public ITranslationResult<string> Translate(string content, ILanguage toLanguage, ILanguage? fromLanguage = null)
            => _translationService.Translate(content, toLanguage, fromLanguage);

        public Task<ITranslationResult<string>> TranslateAsync(string content, ILanguage toLanguage, ILanguage? fromLanguage = null, CancellationToken cancellationToken = default)
            => _translationService.TranslateAsync(content, toLanguage, fromLanguage, cancellationToken);

        public IReadOnlyList<ITranslationResult<string>> TranslateBatch(IEnumerable<string> contents, ILanguage toLanguage, ILanguage? fromLanguage = null)
            => _translationService.TranslateBatch(contents, toLanguage, fromLanguage);

        public Task<IReadOnlyList<ITranslationResult<string>>> TranslateBatchAsync(IEnumerable<string> contents, ILanguage toLanguage, ILanguage? fromLanguage = null, CancellationToken cancellationToken = default)
            => _translationService.TranslateBatchAsync(contents, toLanguage, fromLanguage, cancellationToken);

        public bool TryTranslate(string content, ILanguage toLanguage, ILanguage? fromLanguage, out ITranslationResult<string>? result)
            => _translationService.TryTranslate(content, toLanguage, fromLanguage, out result);

        public Task<(bool Success, ITranslationResult<string>? Result)> TryTranslateAsync(string content, ILanguage toLanguage, ILanguage? fromLanguage = null, CancellationToken cancellationToken = default)
            => _translationService.TryTranslateAsync(content, toLanguage, fromLanguage, cancellationToken);

        #endregion

        #region Transliteration Methods

        public ITransliterationResult Transliterate(string content, ILanguage language, string targetScript, string? sourceScript = null)
            => _translationService.Transliterate(content, language, targetScript, sourceScript);

        public Task<ITransliterationResult> TransliterateAsync(string content, ILanguage language, string targetScript, string? sourceScript = null, CancellationToken cancellationToken = default)
            => _translationService.TransliterateAsync(content, language, targetScript, sourceScript, cancellationToken);

        public IReadOnlyList<ITransliterationResult> TransliterateBatch(IEnumerable<string> contents, ILanguage language, string targetScript, string? sourceScript = null)
            => _translationService.TransliterateBatch(contents, language, targetScript, sourceScript);

        public Task<IReadOnlyList<ITransliterationResult>> TransliterateBatchAsync(IEnumerable<string> contents, ILanguage language, string targetScript, string? sourceScript = null, CancellationToken cancellationToken = default)
            => _translationService.TransliterateBatchAsync(contents, language, targetScript, sourceScript, cancellationToken);

        public bool TryTransliterate(string content, ILanguage language, string targetScript, out ITransliterationResult? result, string? sourceScript = null)
            => _translationService.TryTransliterate(content, language, targetScript, out result, sourceScript);

        public Task<(bool Success, ITransliterationResult? Result)> TryTransliterateAsync(string content, ILanguage language, string targetScript, string? sourceScript = null, CancellationToken cancellationToken = default)
            => _translationService.TryTransliterateAsync(content, language, targetScript, sourceScript, cancellationToken);

        #endregion

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _translationService.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
