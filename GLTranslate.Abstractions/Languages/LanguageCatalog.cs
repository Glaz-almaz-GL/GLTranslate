namespace GLTranslate.Abstractions.Languages
{
    /// <summary>
    /// Статический каталог языков с предопределённым списком из 146 языков.
    /// Предоставляет методы для быстрого поиска по различным кодам.
    /// </summary>
    public static class LanguageCatalog
    {
        private static readonly Lazy<IReadOnlyList<ILanguage>> _allLanguages = new(LoadLanguages);
        private static readonly Lazy<Dictionary<string, ILanguage>> _lookup = new(BuildLookup);

        /// <summary>
        /// Получает все языки из каталога.
        /// </summary>
        public static IReadOnlyList<ILanguage> All => _allLanguages.Value;

        /// <summary>
        /// Ищет язык по любому коду (ISO 639-1, ISO 639-3, BCP 47, или API-коду).
        /// </summary>
        /// <param name="code">Код языка. Регистр не учитывается.</param>
        /// <returns>Язык, если найден; иначе null.</returns>
        public static ILanguage? Find(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && _lookup.Value.TryGetValue(code.ToLowerInvariant(), out var language))
            {
                return language;
            }

            return null;
        }


        /// <summary>
        /// Пытается найти язык по коду.
        /// </summary>
        public static bool TryFind(string code, out ILanguage? language)
        {
            language = Find(code);
            return language is not null;
        }

        /// <summary>
        /// Проверяет, содержится ли язык с указанным кодом в каталоге.
        /// </summary>
        public static bool Contains(string code)
        {
            return Find(code) is not null;
        }

        /// <summary>
        /// Ищет язык по коду, используемому конкретным API.
        /// Автоматически пробует различные варианты кодирования.
        /// </summary>
        /// <param name="apiCode">Код языка от API (например, "zh-CN", "zh-Hans", "mni-Mtei").</param>
        /// <returns>Язык, если найден; иначе null.</returns>
        public static ILanguage? FindByApiCode(string apiCode)
        {
            if (string.IsNullOrWhiteSpace(apiCode))
            {
                return null;
            }

            string normalized = apiCode.ToLowerInvariant();

            // Прямой поиск
            if (_lookup.Value.TryGetValue(normalized, out ILanguage? language))
            {
                return language;
            }

            // Пробуем различные варианты нормализации
            IEnumerable<string> candidates = GetApiCodeCandidates(apiCode);
            foreach (string candidate in candidates)
            {
                if (_lookup.Value.TryGetValue(candidate, out language))
                {
                    return language;
                }
            }

            return null;
        }

        /// <summary>
        /// Получает все варианты кода для поиска по API-коду.
        /// </summary>
        private static IEnumerable<string> GetApiCodeCandidates(string apiCode)
        {
            string lower = apiCode.ToLowerInvariant();

            yield return lower;

            // Для кодов типа "zh-CN" пробуем "zh"
            int dashIndex = lower.IndexOf('-');
            if (dashIndex > 0)
            {
                yield return lower[..dashIndex];
            }

            // Маппинг известных API-кодов
            string mapped = MapApiCode(lower);
            if (mapped != lower)
            {
                yield return mapped;
            }
        }

        /// <summary>
        /// Маппинг специфичных API-кодов на стандартные.
        /// </summary>
        private static string MapApiCode(string apiCode)
        {
            return apiCode switch
            {
                // Google API коды
                "zh-cn" => "zh",
                "zh-tw" => "zh-tw",
                "mni-mtei" => "mni",
                "fa-af" or "fa-fa" => "prs",
                "sat-olck" => "sat",

                // Microsoft API коды (BCP47)
                "zh-hans" => "zh",
                "zh-hant" => "zh-tw",
                "sr-cyrl" => "sr",
                "sr-latn" => "sr",
                "mn-cyrl" => "mn",
                "tlh-latn" => "tlh",

                // Yandex API коды
                "pt-br" => "pt",
                "pt-pt" => "pt",

                _ => apiCode
            };
        }

        /// <summary>
        /// Строит словарь для быстрого поиска по всем кодам.
        /// </summary>
        private static Dictionary<string, ILanguage> BuildLookup()
        {
            Dictionary<string, ILanguage> lookup = new(StringComparer.OrdinalIgnoreCase);

            foreach (ILanguage language in _allLanguages.Value)
            {
                // Индексируем по всем кодам
                AddToLookup(lookup, language.Iso6391, language);
                AddToLookup(lookup, language.Iso6393, language);
                AddToLookup(lookup, language.Bcp47Tag, language);

                // Специальные случаи для BCP47 с регионами
                if (language.Bcp47Tag.Contains('-'))
                {
                    string baseCode = language.Bcp47Tag.Split('-')[0];
                    // Не перезаписываем, если уже есть более специфичный
                    if (!lookup.ContainsKey(baseCode))
                    {
                        lookup[baseCode] = language;
                    }
                }
            }

            return lookup;
        }

        private static void AddToLookup(Dictionary<string, ILanguage> lookup, string code, ILanguage language)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                lookup[code] = language;
            }
        }

        /// <summary>
        /// Загружает список всех языков.
        /// </summary>
        private static List<ILanguage> LoadLanguages()
        {
            return
            [
                new Language("English", "English", "en", "eng", "en", "Latn", false),
                new Language("Spanish", "Español", "es", "spa", "es", "Latn", false),
                new Language("French", "Français", "fr", "fra", "fr", "Latn", false),
                new Language("German", "Deutsch", "de", "deu", "de", "Latn", false),
                new Language("Italian", "Italiano", "it", "ita", "it", "Latn", false),
                new Language("Portuguese", "Português", "pt", "por", "pt", "Latn", false),
                new Language("Russian", "Русский", "ru", "rus", "ru", "Cyrl", false),
                new Language("Ukrainian", "Українська", "uk", "ukr", "uk", "Cyrl", false),
                new Language("Polish", "Polski", "pl", "pol", "pl", "Latn", false),
                new Language("Czech", "Čeština", "cs", "ces", "cs", "Latn", false),
                new Language("Slovak", "Slovenčina", "sk", "slk", "sk", "Latn", false),
                new Language("Slovenian", "Slovenščina", "sl", "slv", "sl", "Latn", false),
                new Language("Croatian", "Hrvatski", "hr", "hrv", "hr", "Latn", false),
                new Language("Serbian", "Српски", "sr", "srp", "sr", "Cyrl", false),
                new Language("Bosnian", "Bosanski", "bs", "bos", "bs", "Latn", false),
                new Language("Bulgarian", "Български", "bg", "bul", "bg", "Cyrl", false),
                new Language("Romanian", "Română", "ro", "ron", "ro", "Latn", false),
                new Language("Hungarian", "Magyar", "hu", "hun", "hu", "Latn", false),
                new Language("Greek", "Ελληνικά", "el", "ell", "el", "Grek", false),
                new Language("Finnish", "Suomi", "fi", "fin", "fi", "Latn", false),
                new Language("Swedish", "Svenska", "sv", "swe", "sv", "Latn", false),
                new Language("Norwegian", "Norsk", "no", "nor", "no", "Latn", false),
                new Language("Norwegian Bokmål", "Norsk bokmål", "nb", "nob", "nb", "Latn", false),
                new Language("Norwegian Nynorsk", "Norsk nynorsk", "nn", "nno", "nn", "Latn", false),
                new Language("Danish", "Dansk", "da", "dan", "da", "Latn", false),
                new Language("Icelandic", "Íslenska", "is", "isl", "is", "Latn", false),
                new Language("Estonian", "Eesti", "et", "est", "et", "Latn", false),
                new Language("Latvian", "Latviešu", "lv", "lav", "lv", "Latn", false),
                new Language("Lithuanian", "Lietuvių", "lt", "lit", "lt", "Latn", false),
                new Language("Dutch", "Nederlands", "nl", "nld", "nl", "Latn", false),
                new Language("Afrikaans", "Afrikaans", "af", "afr", "af", "Latn", false),
                new Language("Irish", "Gaeilge", "ga", "gle", "ga", "Latn", false),
                new Language("Scottish Gaelic", "Gàidhlig", "gd", "gla", "gd", "Latn", false),
                new Language("Welsh", "Cymraeg", "cy", "cym", "cy", "Latn", false),
                new Language("Basque", "Euskara", "eu", "eus", "eu", "Latn", false),
                new Language("Catalan", "Català", "ca", "cat", "ca", "Latn", false),
                new Language("Galician", "Galego", "gl", "glg", "gl", "Latn", false),
                new Language("Albanian", "Shqip", "sq", "sqi", "sq", "Latn", false),
                new Language("Macedonian", "Македонски", "mk", "mkd", "mk", "Cyrl", false),
                new Language("Maltese", "Malti", "mt", "mlt", "mt", "Latn", false),
                new Language("Luxembourgish", "Lëtzebuergesch", "lb", "ltz", "lb", "Latn", false),
                new Language("Belarusian", "Беларуская", "be", "bel", "be", "Cyrl", false),
                new Language("Armenian", "Հայերեն", "hy", "hye", "hy", "Armn", false),
                new Language("Chinese", "中文", "zh", "zho", "zh-Hans", "Hans", false),
                new Language("Chinese (Traditional)", "繁體中文", "zh-TW", "zho", "zh-Hant", "Hant", false),
                new Language("Japanese", "日本語", "ja", "jpn", "ja", "Jpan", false),
                new Language("Korean", "한국어", "ko", "kor", "ko", "Kore", false),
                new Language("Vietnamese", "Tiếng Việt", "vi", "vie", "vi", "Latn", false),
                new Language("Thai", "ไทย", "th", "tha", "th", "Thai", false),
                new Language("Indonesian", "Bahasa Indonesia", "id", "ind", "id", "Latn", false),
                new Language("Malay", "Bahasa Melayu", "ms", "msa", "ms", "Latn", false),
                new Language("Filipino", "Filipino", "tl", "tgl", "tl", "Latn", false),
                new Language("Hindi", "हिन्दी", "hi", "hin", "hi", "Deva", false),
                new Language("Bengali", "বাংলা", "bn", "ben", "bn", "Beng", false),
                new Language("Tamil", "தமிழ்", "ta", "tam", "ta", "Taml", false),
                new Language("Telugu", "తెలుగు", "te", "tel", "te", "Telu", false),
                new Language("Kannada", "ಕನ್ನಡ", "kn", "kan", "kn", "Knda", false),
                new Language("Malayalam", "മലയാളം", "ml", "mal", "ml", "Mlym", false),
                new Language("Gujarati", "ગુજરાતી", "gu", "guj", "gu", "Gujr", false),
                new Language("Marathi", "मराठी", "mr", "mar", "mr", "Deva", false),
                new Language("Punjabi", "ਪੰਜਾਬੀ", "pa", "pan", "pa", "Guru", false),
                new Language("Urdu", "اردو", "ur", "urd", "ur", "Arab", true),
                new Language("Nepali", "नेपाली", "ne", "nep", "ne", "Deva", false),
                new Language("Sinhala", "සිංහල", "si", "sin", "si", "Sinh", false),
                new Language("Burmese", "မြန်မာ", "my", "mya", "my", "Mymr", false),
                new Language("Khmer", "ខ្មែរ", "km", "khm", "km", "Khmr", false),
                new Language("Lao", "ລາວ", "lo", "lao", "lo", "Laoo", false),
                new Language("Mongolian", "Монгол", "mn", "mon", "mn", "Cyrl", false),
                new Language("Kazakh", "Қазақ", "kk", "kaz", "kk", "Cyrl", false),
                new Language("Uzbek", "Oʻzbek", "uz", "uzb", "uz", "Latn", false),
                new Language("Kyrgyz", "Кыргызча", "ky", "kir", "ky", "Cyrl", false),
                new Language("Tajik", "Тоҷикӣ", "tg", "tgk", "tg", "Cyrl", false),
                new Language("Turkmen", "Türkmen", "tk", "tuk", "tk", "Latn", false),
                new Language("Azerbaijani", "Azərbaycan", "az", "aze", "az", "Latn", false),
                new Language("Georgian", "ქართული", "ka", "kat", "ka", "Geor", false),
                new Language("Hebrew", "עברית", "he", "heb", "he", "Hebr", true),
                new Language("Arabic", "العربية", "ar", "ara", "ar", "Arab", true),
                new Language("Persian", "فارسی", "fa", "fas", "fa", "Arab", true),
                new Language("Pashto", "پښتو", "ps", "pus", "ps", "Arab", true),
                new Language("Kurdish", "Kurdî", "ku", "kur", "ku", "Latn", false),
                new Language("Turkish", "Türkçe", "tr", "tur", "tr", "Latn", false),
                new Language("Uyghur", "ئۇيغۇرچە", "ug", "uig", "ug", "Arab", true),
                new Language("Javanese", "Basa Jawa", "jv", "jav", "jv", "Latn", false),
                new Language("Swahili", "Kiswahili", "sw", "swa", "sw", "Latn", false),
                new Language("Amharic", "አማርኛ", "am", "amh", "am", "Ethi", false),
                new Language("Tigrinya", "ትግርኛ", "ti", "tir", "ti", "Ethi", false),
                new Language("Hausa", "Hausa", "ha", "hau", "ha", "Latn", false),
                new Language("Yoruba", "Yorùbá", "yo", "yor", "yo", "Latn", false),
                new Language("Igbo", "Igbo", "ig", "ibo", "ig", "Latn", false),
                new Language("Zulu", "isiZulu", "zu", "zul", "zu", "Latn", false),
                new Language("Xhosa", "isiXhosa", "xh", "xho", "xh", "Latn", false),
                new Language("Southern Sotho", "Sesotho", "st", "sot", "st", "Latn", false),
                new Language("Tswana", "Setswana", "tn", "tsn", "tn", "Latn", false),
                new Language("Shona", "chiShona", "sn", "sna", "sn", "Latn", false),
                new Language("Malagasy", "Malagasy", "mg", "mlg", "mg", "Latn", false),
                new Language("Somali", "Soomaali", "so", "som", "so", "Latn", false),
                new Language("Wolof", "Wolof", "wo", "wol", "wo", "Latn", false),
                new Language("Lingala", "Lingála", "ln", "lin", "ln", "Latn", false),
                new Language("Kinyarwanda", "Ikinyarwanda", "rw", "kin", "rw", "Latn", false),
                new Language("Kirundi", "Ikirundi", "rn", "run", "rn", "Latn", false),
                new Language("Oromo", "Afaan Oromoo", "om", "orm", "om", "Latn", false),
                new Language("Bambara", "Bamanankan", "bm", "bam", "bm", "Latn", false),
                new Language("Chichewa", "Chichewa", "ny", "nya", "ny", "Latn", false),
                new Language("Akan", "Akan", "ak", "aka", "ak", "Latn", false),
                new Language("Ganda", "Luganda", "lg", "lug", "lg", "Latn", false),
                new Language("Luba-Kasai", "Tshiluba", "lu", "lub", "lu", "Latn", false),
                new Language("Kongo", "Kikongo", "kg", "kon", "kg", "Latn", false),
                new Language("Ewe", "Eʋegbe", "ee", "ewe", "ee", "Latn", false),
                new Language("Maori", "Te Reo Māori", "mi", "mri", "mi", "Latn", false),
                new Language("Samoan", "Gagana Sāmoa", "sm", "smo", "sm", "Latn", false),
                new Language("Tongan", "Lea Faka-Tonga", "to", "ton", "to", "Latn", false),
                new Language("Fijian", "Na Vosa Vakaviti", "fj", "fij", "fj", "Latn", false),
                new Language("Haitian Creole", "Kreyòl ayisyen", "ht", "hat", "ht", "Latn", false),
                new Language("Cebuano", "Binisaya", "ceb", "ceb", "ceb", "Latn", false),
                new Language("Ilocano", "Ilokano", "ilo", "ilo", "ilo", "Latn", false),
                new Language("Hmong", "Hmoob", "hmn", "hmn", "hmn", "Latn", false),
                new Language("Esperanto", "Esperanto", "eo", "epo", "eo", "Latn", false),
                new Language("Latin", "Latina", "la", "lat", "la", "Latn", false),
                new Language("Yiddish", "ייִדיש", "yi", "yid", "yi", "Hebr", true),
                new Language("Sanskrit", "संस्कृतम्", "sa", "san", "sa", "Deva", false),
                new Language("Dzongkha", "རྫོང་ཁ", "dz", "dzo", "dz", "Tibt", false),
                new Language("Tibetan", "བོད་སྐད", "bo", "bod", "bo", "Tibt", false),
                new Language("Sindhi", "سنڌي", "sd", "snd", "sd", "Arab", true),
                new Language("Odia", "ଓଡ଼ିଆ", "or", "ori", "or", "Orya", false),
                new Language("Assamese", "অসমীয়া", "as", "asm", "as", "Beng", false),
                new Language("Bihari", "भोजपुरी", "bh", "bih", "bh", "Deva", false),
                new Language("Corsican", "Corsu", "co", "cos", "co", "Latn", false),
                new Language("Western Frisian", "Frysk", "fy", "fry", "fy", "Latn", false),
                new Language("Sundanese", "Basa Sunda", "su", "sun", "su", "Latn", false),
                new Language("Hawaiian", "ʻŌlelo Hawaiʻi", "haw", "haw", "haw", "Latn", false),
                new Language("Bashkir", "Башҡорт", "ba", "bak", "ba", "Cyrl", false),
                new Language("Chuvash", "Чӑваш", "cv", "chv", "cv", "Cyrl", false),
                new Language("Tatar", "Татар", "tt", "tat", "tt", "Cyrl", false),
                new Language("Chechen", "Нохчийн", "ce", "che", "ce", "Cyrl", false),
                new Language("Adyghe", "Адыгэбзэ", "ady", "ady", "ady", "Cyrl", false),
                new Language("Twi", "Twi", "tw", "twi", "tw", "Latn", false),
                new Language("Tsonga", "Xitsonga", "ts", "tso", "ts", "Latn", false),
                new Language("Venda", "Tshivenḓa", "ve", "ven", "ve", "Latn", false),
                new Language("South Ndebele", "isiNdebele", "nr", "nbl", "nr", "Latn", false),
                new Language("Swati", "siSwati", "ss", "ssw", "ss", "Latn", false),
                new Language("Navajo", "Diné bizaad", "nv", "nav", "nv", "Latn", false),
                new Language("Cherokee", "ᏣᎳᎩ", "chr", "chr", "chr", "Cher", false),
                new Language("Manx", "Gaelg", "gv", "glv", "gv", "Latn", false),
                new Language("Cornish", "Kernewek", "kw", "cor", "kw", "Latn", false),
                new Language("Breton", "Brezhoneg", "br", "bre", "br", "Latn", false),
                new Language("Occitan", "Occitan", "oc", "oci", "oc", "Latn", false)
            ];
        }
    }
}