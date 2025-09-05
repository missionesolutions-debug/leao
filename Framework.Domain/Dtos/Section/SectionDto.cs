using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Framework.Domain.Dtos.Section
{
    public class SectionDto
    {
        public int? Id { get; set; }
        public string Ref { get; set; }
        public string LinkUrl { get; set; }
        public string VideoUrl { get; set; }
        public string JsonContent { get; set; }
        public bool? Enabled { get; set; } = true;

        [JsonPropertyName("i18n")]
        public Dictionary<string, TranslationDto> Translations { get; set; }

        public class TranslationDto
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Description { get; set; }
            public string LinkText { get; set; }
        }
     
    }
}
