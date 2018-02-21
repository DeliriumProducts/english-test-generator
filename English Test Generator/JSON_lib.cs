
namespace JSON_lib
{

    using Newtonsoft.Json;

    public partial class GetResponse
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Metadata
    {
    }

    public partial class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("lexicalEntries")]
        public LexicalEntry[] LexicalEntries { get; set; }

        [JsonProperty("matchType")]
        public string MatchType { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("pronunciations")]
        public Pronunciation[] Pronunciations { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }
    }

    public partial class LexicalEntry
    {
        [JsonProperty("derivativeOf")]
        public Derivative[] DerivativeOf { get; set; }

        [JsonProperty("derivatives")]
        public Derivative[] Derivatives { get; set; }

        [JsonProperty("entries")]
        public Entry[] Entries { get; set; }

        [JsonProperty("grammaticalFeatures")]
        public GrammaticalFeature[] GrammaticalFeatures { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("lexicalCategory")]
        public string LexicalCategory { get; set; }

        [JsonProperty("notes")]
        public Note[] Notes { get; set; }

        [JsonProperty("pronunciations")]
        public Pronunciation[] Pronunciations { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("variantForms")]
        public VariantForm[] VariantForms { get; set; }
    }

    public partial class Derivative
    {
        [JsonProperty("domains")]
        public string[] Domains { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("regions")]
        public string[] Regions { get; set; }

        [JsonProperty("registers")]
        public string[] Registers { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Entry
    {
        [JsonProperty("etymologies")]
        public string[] Etymologies { get; set; }

        [JsonProperty("grammaticalFeatures")]
        public GrammaticalFeature[] GrammaticalFeatures { get; set; }

        [JsonProperty("homographNumber")]
        public string HomographNumber { get; set; }

        [JsonProperty("notes")]
        public Note[] Notes { get; set; }

        [JsonProperty("pronunciations")]
        public Pronunciation[] Pronunciations { get; set; }

        [JsonProperty("senses")]
        public Sense[] Senses { get; set; }

        [JsonProperty("variantForms")]
        public VariantForm[] VariantForms { get; set; }
    }

    public partial class GrammaticalFeature
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Note
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Pronunciation
    {
        [JsonProperty("audioFile")]
        public string AudioFile { get; set; }

        [JsonProperty("dialects")]
        public string[] Dialects { get; set; }

        [JsonProperty("phoneticNotation")]
        public string PhoneticNotation { get; set; }

        [JsonProperty("phoneticSpelling")]
        public string PhoneticSpelling { get; set; }

        [JsonProperty("regions")]
        public string[] Regions { get; set; }
    }

    public partial class Sense
    {
        [JsonProperty("crossReferenceMarkers")]
        public string[] CrossReferenceMarkers { get; set; }

        [JsonProperty("crossReferences")]
        public Note[] CrossReferences { get; set; }

        [JsonProperty("definitions")]
        public string[] Definitions { get; set; }

        [JsonProperty("domains")]
        public string[] Domains { get; set; }

        [JsonProperty("examples")]
        public Example[] Examples { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("notes")]
        public Note[] Notes { get; set; }

        [JsonProperty("pronunciations")]
        public Pronunciation[] Pronunciations { get; set; }

        [JsonProperty("regions")]
        public string[] Regions { get; set; }

        [JsonProperty("registers")]
        public string[] Registers { get; set; }

        [JsonProperty("subsenses")]
        public Subsense[] Subsenses { get; set; }

        [JsonProperty("translations")]
        public Translation[] Translations { get; set; }

        [JsonProperty("variantForms")]
        public VariantForm[] VariantForms { get; set; }
    }

    public partial class Subsense
    {
        [JsonProperty("definitions")]
        public string[] Definitions { get; set; }

        [JsonProperty("examples")]
        public Example[] Examples { get; set; }
    }

    public partial class Example
    {
        [JsonProperty("definitions")]
        public string[] Definitions { get; set; }

        [JsonProperty("domains")]
        public string[] Domains { get; set; }

        [JsonProperty("notes")]
        public Note[] Notes { get; set; }

        [JsonProperty("regions")]
        public string[] Regions { get; set; }

        [JsonProperty("registers")]
        public string[] Registers { get; set; }

        [JsonProperty("senseIds")]
        public string[] SenseIds { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("translations")]
        public Translation[] Translations { get; set; }
    }

    public partial class Translation
    {
        [JsonProperty("domains")]
        public string[] Domains { get; set; }

        [JsonProperty("grammaticalFeatures")]
        public GrammaticalFeature[] GrammaticalFeatures { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("notes")]
        public Note[] Notes { get; set; }

        [JsonProperty("regions")]
        public string[] Regions { get; set; }

        [JsonProperty("registers")]
        public string[] Registers { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class VariantForm
    {
        [JsonProperty("regions")]
        public string[] Regions { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, JSON_lib.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, JSON_lib.Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
