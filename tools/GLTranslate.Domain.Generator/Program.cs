using System.Globalization;
using System.Text;

namespace GLTranslate.Domain.Generator;

/// <summary>
/// One-shot code generator that turns BCL globalization data (<see cref="CultureInfo"/>,
/// <see cref="RegionInfo"/>) plus a small curated script map into the generated
/// <c>*.g.cs</c> data files consumed by GLTranslate.Domain's registries and
/// <c>RegionFactory</c>.
/// </summary>
/// <remarks>
/// This is a dev-time tool, not shipped with the library. Re-run it whenever the
/// generated data needs to be refreshed (e.g. after a .NET globalization update).
/// </remarks>
internal static class Program
{
    private static readonly string DomainRoot = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "GLTranslate.Domain"));

    // Curated: ISO 639-1 language code -> ISO 15924 script codes the language is
    // written in. BCL does not expose this mapping, so it is hand-maintained.
    // Anything not listed here defaults to Latin.
    private static readonly Dictionary<string, string[]> LanguageScripts = new()
    {
        ["ru"] = ["Cyrl"], ["uk"] = ["Cyrl"], ["be"] = ["Cyrl"], ["bg"] = ["Cyrl"],
        ["mk"] = ["Cyrl"], ["sr"] = ["Cyrl", "Latn"], ["kk"] = ["Cyrl", "Latn"],
        ["ky"] = ["Cyrl"], ["tg"] = ["Cyrl"], ["mn"] = ["Cyrl"],
        ["ar"] = ["Arab"], ["fa"] = ["Arab"], ["ur"] = ["Arab"], ["ps"] = ["Arab"],
        ["sd"] = ["Arab"], ["ug"] = ["Arab"],
        ["he"] = ["Hebr"], ["yi"] = ["Hebr"],
        ["hi"] = ["Deva"], ["mr"] = ["Deva"], ["ne"] = ["Deva"], ["sa"] = ["Deva"],
        ["bn"] = ["Beng"], ["as"] = ["Beng"],
        ["pa"] = ["Guru", "Arab"],
        ["gu"] = ["Gujr"],
        ["or"] = ["Orya"],
        ["ta"] = ["Taml"],
        ["te"] = ["Telu"],
        ["kn"] = ["Knda"],
        ["ml"] = ["Mlym"],
        ["si"] = ["Sinh"],
        ["th"] = ["Thai"],
        ["lo"] = ["Laoo"],
        ["bo"] = ["Tibt"], ["dz"] = ["Tibt"],
        ["my"] = ["Mymr"],
        ["ka"] = ["Geor"],
        ["hy"] = ["Armn"],
        ["am"] = ["Ethi"], ["ti"] = ["Ethi"],
        ["zh"] = ["Hans", "Hant"],
        ["ja"] = ["Jpan"],
        ["ko"] = ["Hang"],
        ["el"] = ["Grek"],
    };

    // Curated: ISO 15924 script code -> (English name, native name).
    // Only scripts actually referenced by LanguageScripts need an entry here.
    private static readonly Dictionary<string, (string Name, string NativeName)> ScriptNames = new()
    {
        ["Latn"] = ("Latin", "Latin"),
        ["Cyrl"] = ("Cyrillic", "Кириллица"),
        ["Arab"] = ("Arabic", "العربية"),
        ["Hebr"] = ("Hebrew", "עברית"),
        ["Deva"] = ("Devanagari", "देवनागरी"),
        ["Beng"] = ("Bengali", "বাংলা"),
        ["Guru"] = ("Gurmukhi", "ਗੁਰਮੁਖੀ"),
        ["Gujr"] = ("Gujarati", "ગુજરાતી"),
        ["Orya"] = ("Oriya", "ଓଡ଼ିଆ"),
        ["Taml"] = ("Tamil", "தமிழ்"),
        ["Telu"] = ("Telugu", "తెలుగు"),
        ["Knda"] = ("Kannada", "ಕನ್ನಡ"),
        ["Mlym"] = ("Malayalam", "മലയാളം"),
        ["Sinh"] = ("Sinhala", "සිංහල"),
        ["Thai"] = ("Thai", "ไทย"),
        ["Laoo"] = ("Lao", "ລາວ"),
        ["Tibt"] = ("Tibetan", "བོད་ཡིག"),
        ["Mymr"] = ("Myanmar", "မြန်မာ"),
        ["Geor"] = ("Georgian", "ქართული"),
        ["Armn"] = ("Armenian", "Հայերեն"),
        ["Ethi"] = ("Ethiopic", "ግዕዝ"),
        ["Hans"] = ("Han (Simplified)", "简体"),
        ["Hant"] = ("Han (Traditional)", "繁體"),
        ["Jpan"] = ("Japanese", "日本語"),
        ["Hang"] = ("Hangul", "한글"),
        ["Grek"] = ("Greek", "Ελληνικά"),
    };

    private static void Main()
    {
        List<ScriptEntry> scripts = BuildScripts();
        List<LanguageEntry> languages = BuildLanguages(scripts);
        List<RegionEntry> regions = BuildRegions();
        List<CultureEntry> cultures = BuildCultures(languages, regions, scripts);

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Scripts", "Generated", "ScriptRegistryData.g.cs"),
            GenerateScriptData(scripts));

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Scripts", "Generated", "ScriptRegistry.Generated.cs"),
            GenerateScriptRegistryProperties(scripts));

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Languages", "Generated", "LanguageRegistryData.g.cs"),
            GenerateLanguageData(languages));

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Languages", "Generated", "LanguageRegistry.Generated.cs"),
            GenerateLanguageRegistryProperties(languages));

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Regions", "Generated", "RegionRegistryData.g.cs"),
            GenerateRegionRegistryData(regions));

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Regions", "Generated", "RegionRegistry.Generated.cs"),
            GenerateRegionRegistryProperties(regions));

        WriteFile(
            Path.Combine(DomainRoot, "Linguistics", "Cultures", "Generated", "CultureRegistryData.g.cs"),
            GenerateCultureData(cultures));

        Console.WriteLine($"Generated {scripts.Count} scripts, {languages.Count} languages, " +
                           $"{regions.Count} regions, {cultures.Count} cultures.");
    }

    private sealed record ScriptEntry(string Id, string Code, string Name, string NativeName);

    private sealed record LanguageEntry(string Id, string Iso1, string? Iso2, string Name, string NativeName, bool Rtl, string[] ScriptCodes);

    private sealed record RegionEntry(string Id, string Alpha2, string Alpha3, string Name);

    private sealed record CultureEntry(string Bcp47, string LanguageIso1, string? RegionAlpha2, string? ScriptCode);

    private static List<ScriptEntry> BuildScripts()
    {
        HashSet<string> usedIds = [];

        return [.. ScriptNames
            .Select(kv => new ScriptEntry(UniqueSlug(kv.Value.Name, kv.Key, usedIds), kv.Key, kv.Value.Name, kv.Value.NativeName))
            .OrderBy(x => x.Code, StringComparer.Ordinal)];
    }

    private static List<LanguageEntry> BuildLanguages(List<ScriptEntry> scripts)
    {
        HashSet<string> knownScriptCodes = [.. scripts.Select(s => s.Code)];
        Dictionary<string, LanguageEntry> byIso1 = [];
        HashSet<string> usedIds = [];

        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
        {
            if (culture.Name.Length == 0)
            {
                continue;
            }

            string iso1 = culture.TwoLetterISOLanguageName;

            if (iso1.Length != 2 || iso1 == "iv" || byIso1.ContainsKey(iso1))
            {
                continue;
            }

            string? iso2 = culture.ThreeLetterISOLanguageName is { Length: 3 } three ? three : null;

            string[] scriptCodes = LanguageScripts.TryGetValue(iso1, out string[]? mapped)
                ? [.. mapped.Where(knownScriptCodes.Contains)]
                : ["Latn"];

            if (scriptCodes.Length == 0)
            {
                scriptCodes = ["Latn"];
            }

            byIso1[iso1] = new LanguageEntry(
                UniqueSlug(culture.EnglishName, iso1, usedIds),
                iso1,
                iso2,
                culture.EnglishName,
                culture.NativeName,
                culture.TextInfo.IsRightToLeft,
                scriptCodes);
        }

        return [.. byIso1.Values.OrderBy(x => x.Iso1, StringComparer.Ordinal)];
    }

    private static List<RegionEntry> BuildRegions()
    {
        Dictionary<string, RegionEntry> byAlpha2 = [];
        HashSet<string> usedIds = [];

        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            RegionInfo region;

            try
            {
                region = new RegionInfo(culture.Name);
            }
            catch (ArgumentException)
            {
                continue;
            }

            string alpha2 = region.TwoLetterISORegionName;

            if (alpha2.Length != 2 || byAlpha2.ContainsKey(alpha2))
            {
                continue;
            }

            byAlpha2[alpha2] = new RegionEntry(
                UniqueSlug(region.EnglishName, alpha2, usedIds),
                alpha2,
                region.ThreeLetterISORegionName,
                region.EnglishName);
        }

        return [.. byAlpha2.Values.OrderBy(x => x.Alpha2, StringComparer.Ordinal)];
    }

    private static List<CultureEntry> BuildCultures(
        List<LanguageEntry> languages,
        List<RegionEntry> regions,
        List<ScriptEntry> scripts)
    {
        Dictionary<string, LanguageEntry> languagesByIso1 = languages.ToDictionary(l => l.Iso1);
        HashSet<string> knownRegions = [.. regions.Select(r => r.Alpha2)];
        HashSet<string> knownScripts = [.. scripts.Select(s => s.Code)];

        List<CultureEntry> result = [];
        HashSet<string> seen = [];

        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            if (!seen.Add(culture.Name))
            {
                continue;
            }

            string languageIso1 = culture.Parent.TwoLetterISOLanguageName;

            if (!languagesByIso1.TryGetValue(languageIso1, out LanguageEntry? language))
            {
                continue;
            }

            string? regionAlpha2 = null;

            try
            {
                string candidate = new RegionInfo(culture.Name).TwoLetterISORegionName;

                if (knownRegions.Contains(candidate))
                {
                    regionAlpha2 = candidate;
                }
            }
            catch (ArgumentException)
            {
                // Culture has no associated region (e.g. script-only culture).
            }

            string? scriptCode = culture.Name
                .Split('-')
                .FirstOrDefault(segment => segment.Length == 4 && knownScripts.Contains(
                    char.ToUpperInvariant(segment[0]) + segment[1..].ToLowerInvariant()));

            if (scriptCode is not null)
            {
                scriptCode = char.ToUpperInvariant(scriptCode[0]) + scriptCode[1..].ToLowerInvariant();

                // Only attach the script when the language actually declares it;
                // e.g. az-Cyrl-AZ exists in the BCL but Cyrillic is not (yet)
                // registered as one of Azerbaijani's scripts.
                if (!language.ScriptCodes.Contains(scriptCode))
                {
                    scriptCode = null;
                }
            }

            result.Add(new CultureEntry(culture.Name, languageIso1, regionAlpha2, scriptCode));
        }

        return [.. result.OrderBy(x => x.Bcp47, StringComparer.Ordinal)];
    }

    private static string GenerateScriptData(List<ScriptEntry> scripts)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Scripts;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Scripts.Code;");
        sb.AppendLine("using System.Collections.Immutable;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Scripts.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class ScriptRegistryData");
        sb.AppendLine("{");
        sb.AppendLine("    public static readonly ImmutableArray<Script> All =");
        sb.AppendLine("    [");

        foreach (ScriptEntry s in scripts)
        {
            sb.AppendLine($"        new Script(new ScriptId({Lit(s.Id)}), {Lit(s.Name)}, {Lit(s.NativeName)}, [new Iso15924Code({Lit(s.Code)})]),");
        }

        sb.AppendLine("    ];");
        sb.AppendLine();
        sb.AppendLine("    // Keyed by raw ISO 15924 code (e.g. \"Cyrl\"), used to wire up cross-references");
        sb.AppendLine("    // during generation. Not the same as the domain Id (see Script.Id).");
        sb.AppendLine("    public static readonly ImmutableDictionary<string, Script> ByCode =");
        sb.AppendLine("        All.ToImmutableDictionary(x => x.Codes.Get<Iso15924Code>().Value, x => x);");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateScriptRegistryProperties(List<ScriptEntry> scripts)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Scripts;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Scripts.Generated;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Scripts;");
        sb.AppendLine();
        sb.AppendLine("public sealed partial class ScriptRegistry");
        sb.AppendLine("{");

        HashSet<string> usedNames = [];

        foreach (ScriptEntry s in scripts)
        {
            string propertyName = UniquePascalIdentifier(s.Name, s.Code, usedNames);
            sb.AppendLine($"    /// <summary>Gets the {EscapeXmlDoc(s.Name)} script.</summary>");
            sb.AppendLine($"    public Script {propertyName} => Get(new ScriptId({Lit(s.Id)}));");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateLanguageData(List<LanguageEntry> languages)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Languages;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Languages.Codes;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Scripts.Generated;");
        sb.AppendLine("using System.Collections.Immutable;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Languages.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class LanguageRegistryData");
        sb.AppendLine("{");
        sb.AppendLine("    public static readonly ImmutableArray<Language> All =");
        sb.AppendLine("    [");

        foreach (LanguageEntry l in languages)
        {
            string direction = l.Rtl ? "LanguageDirection.RightToLeft" : "LanguageDirection.LeftToRight";
            string scriptRefs = string.Join(", ", l.ScriptCodes.Select(c => $"ScriptRegistryData.ByCode[{Lit(c)}]"));

            // ISO 639-3 is not exposed by the BCL. For languages that already have an
            // ISO 639-2 code, 639-3 preserves that same code, so it is safe to reuse it.
            string codes = l.Iso2 is null
                ? $"[new Iso6391Code({Lit(l.Iso1)})]"
                : $"[new Iso6391Code({Lit(l.Iso1)}), new Iso6392Code({Lit(l.Iso2)}), new Iso6393Code({Lit(l.Iso2)})]";

            sb.AppendLine($"        new Language(new LanguageId({Lit(l.Id)}), {Lit(l.Name)}, {Lit(l.NativeName)}, {direction}, [{scriptRefs}], {codes}),");
        }

        sb.AppendLine("    ];");
        sb.AppendLine();
        sb.AppendLine("    // Keyed by raw ISO 639-1 code (e.g. \"ru\"), used to wire up cross-references");
        sb.AppendLine("    // during generation. Not the same as the domain Id (see Language.Id).");
        sb.AppendLine("    public static readonly ImmutableDictionary<string, Language> ByIso1 =");
        sb.AppendLine("        All.ToImmutableDictionary(x => x.Сodes.Get<Iso6391Code>().Value, x => x);");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateLanguageRegistryProperties(List<LanguageEntry> languages)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Languages;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Languages.Generated;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Languages;");
        sb.AppendLine();
        sb.AppendLine("public sealed partial class LanguageRegistry");
        sb.AppendLine("{");

        HashSet<string> usedNames = [];

        foreach (LanguageEntry l in languages)
        {
            string propertyName = UniquePascalIdentifier(l.Name, l.Iso1, usedNames);
            sb.AppendLine($"    /// <summary>Gets the {EscapeXmlDoc(l.Name)} language.</summary>");
            sb.AppendLine($"    public Language {propertyName} => Get(new LanguageId({Lit(l.Id)}));");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateRegionRegistryData(List<RegionEntry> regions)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Regions;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Regions.Codes;");
        sb.AppendLine("using System.Collections.Immutable;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Regions.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class RegionRegistryData");
        sb.AppendLine("{");
        sb.AppendLine("    public static readonly ImmutableArray<Region> All =");
        sb.AppendLine("    [");

        foreach (RegionEntry r in regions)
        {
            sb.AppendLine($"        new Region(new RegionId({Lit(r.Id)}), {Lit(r.Name)}, [new Iso3166Alpha2Code({Lit(r.Alpha2)}), new Iso3166Alpha3Code({Lit(r.Alpha3)})]),");
        }

        sb.AppendLine("    ];");
        sb.AppendLine();
        sb.AppendLine("    // Keyed by raw ISO 3166-1 alpha-2 code (e.g. \"US\"), used by RegionFactory and to");
        sb.AppendLine("    // wire up cross-references during generation. Not the same as the domain Id (see Region.Id).");
        sb.AppendLine("    public static readonly ImmutableDictionary<string, Region> ByAlpha2 =");
        sb.AppendLine("        All.ToImmutableDictionary(x => x.Codes.Get<Iso3166Alpha2Code>().Value, x => x);");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateRegionRegistryProperties(List<RegionEntry> regions)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Regions;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Regions.Generated;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Regions;");
        sb.AppendLine();
        sb.AppendLine("public sealed partial class RegionRegistry");
        sb.AppendLine("{");

        HashSet<string> usedNames = [];

        foreach (RegionEntry r in regions)
        {
            string propertyName = UniquePascalIdentifier(r.Name, r.Alpha2, usedNames);
            sb.AppendLine($"    /// <summary>Gets the {EscapeXmlDoc(r.Name)} region.</summary>");
            sb.AppendLine($"    public Region {propertyName} => Get(new RegionId({Lit(r.Id)}));");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateCultureData(List<CultureEntry> cultures)
    {
        StringBuilder sb = new();

        WriteHeader(sb);
        sb.AppendLine("using GLTranslate.Abstractions.Linguistics.Cultures;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Cultures.Codes;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Languages.Generated;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Regions.Generated;");
        sb.AppendLine("using GLTranslate.Domain.Linguistics.Scripts.Generated;");
        sb.AppendLine("using System.Collections.Immutable;");
        sb.AppendLine();
        sb.AppendLine("namespace GLTranslate.Domain.Linguistics.Cultures.Generated;");
        sb.AppendLine();
        sb.AppendLine("internal static class CultureRegistryData");
        sb.AppendLine("{");
        sb.AppendLine("    public static readonly ImmutableArray<Culture> All =");
        sb.AppendLine("    [");

        foreach (CultureEntry c in cultures)
        {
            string region = c.RegionAlpha2 is null ? "null" : $"RegionRegistryData.ByAlpha2[{Lit(c.RegionAlpha2)}]";
            string script = c.ScriptCode is null ? "null" : $"ScriptRegistryData.ByCode[{Lit(c.ScriptCode)}]";

            sb.AppendLine($"        new Culture(new CultureId({Lit(c.Bcp47)}), LanguageRegistryData.ByIso1[{Lit(c.LanguageIso1)}], {region}, {script}, [new Bcp47Code({Lit(c.Bcp47)})]),");
        }

        sb.AppendLine("    ];");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void WriteHeader(StringBuilder sb)
    {
        sb.AppendLine("// <auto-generated>");
        sb.AppendLine("// Generated by tools/GLTranslate.Domain.Generator. Do not edit by hand.");
        sb.AppendLine("// </auto-generated>");
        sb.AppendLine();
    }

    // Converts an English name into a stable, standard-independent domain
    // identifier, e.g. "Chinese (Simplified)" -> "chinese_simplified".
    private static string Slug(string value)
    {
        StringBuilder sb = new();
        bool lastWasSeparator = true;

        foreach (char c in value)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToLowerInvariant(c));
                lastWasSeparator = false;
            }
            else if (!lastWasSeparator)
            {
                sb.Append('_');
                lastWasSeparator = true;
            }
        }

        return sb.ToString().TrimEnd('_');
    }

    // Same as Slug, but disambiguates collisions by appending the (already
    // unique) fallback key, since slugs are derived from human-readable names
    // that are not guaranteed unique across the whole data set.
    private static string UniqueSlug(string name, string fallbackKey, HashSet<string> used)
    {
        string slug = Slug(name);

        if (used.Add(slug))
        {
            return slug;
        }

        string disambiguated = $"{slug}_{Slug(fallbackKey)}";
        used.Add(disambiguated);
        return disambiguated;
    }

    // Converts an English name into a PascalCase C# identifier, e.g.
    // "Han (Simplified)" -> "HanSimplified".
    private static string PascalIdentifier(string value)
    {
        StringBuilder sb = new();

        foreach (string word in value.Split(
            [' ', '-', '\'', '(', ')', ',', '.', '/'],
            StringSplitOptions.RemoveEmptyEntries))
        {
            string cleaned = new([.. word.Where(char.IsLetterOrDigit)]);

            if (cleaned.Length == 0)
            {
                continue;
            }

            sb.Append(char.ToUpperInvariant(cleaned[0]));

            if (cleaned.Length > 1)
            {
                sb.Append(cleaned[1..]);
            }
        }

        string identifier = sb.ToString();

        if (identifier.Length == 0 || char.IsDigit(identifier[0]))
        {
            identifier = "_" + identifier;
        }

        return identifier;
    }

    private static string UniquePascalIdentifier(string name, string fallbackKey, HashSet<string> used)
    {
        string identifier = PascalIdentifier(name);

        if (used.Add(identifier))
        {
            return identifier;
        }

        string disambiguated = identifier + PascalIdentifier(fallbackKey);
        used.Add(disambiguated);
        return disambiguated;
    }

    private static string EscapeXmlDoc(string value)
    {
        return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    private static string Lit(string value)
    {
        StringBuilder sb = new();
        sb.Append('"');

        foreach (char c in value)
        {
            switch (c)
            {
                case '"': sb.Append("\\\""); break;
                case '\\': sb.Append("\\\\"); break;
                default: sb.Append(c); break;
            }
        }

        sb.Append('"');
        return sb.ToString();
    }

    private static void WriteFile(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }
}
