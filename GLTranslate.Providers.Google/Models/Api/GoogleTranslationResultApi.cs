using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GLTranslate.Providers.Google.Models.Api
{
    internal record struct GoogleTranslationResultModel
    {
        [JsonPropertyName("sentences")]
        public IReadOnlyList<GoogleSentence>? Sentences { get; set; }

        [JsonPropertyName("src")]
        public required string Source { get; set; }

        [JsonPropertyName("confidence")]
        public float? Confidence { get; set; }
    }
}
