using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GLTranslate.Providers.Google.Models.Api
{
    internal record struct GoogleSentence
    {
        [JsonPropertyName("trans")]
        public required string Translation { get; set; }

        [JsonPropertyName("translit")]
        public string? ToTransliteration { get; set; }

        [JsonPropertyName("src_translit")]
        public string? FromTransliteration { get; set; }
    }
}
