using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Framework.Domain.Dtos.Section
{
    public class SectionListDto
    {
        public int Id { get; set; }
        public string Ref { get; set; }
        public string LinkUrl { get; set; }
        public string VideoUrl { get; set; }
        public string JsonContent { get; set; }
        public bool Enabled { get; set; }
        [JsonPropertyName("i18n")]
        public Dictionary<string, TranslationDto> translations { get; set; }
        public List<MetaDataDto> Images { get; set; }

        public class TranslationDto
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Description { get; set; }
            public string LinkText { get; set; }
        }

        public class MetaDataDto
        {
            public int Position { get; set; }
            public string Id { get; set; }
            public string FileType { get; set; }
            public string Url { get; set; }
            public string Title { get; set; }
            public string Length { get; set; }
            public List<MetaDataChildDto> mobile { get; set; }
        }

        public class MetaDataChildDto
        {
            public string EntityId { get; set; } // Correspondente ao "entityId"
            public string EntityType { get; set; } // Correspondente ao "entityType"
            public int Position { get; set; } // Correspondente ao "position"
            public string Id { get; set; } // Correspondente ao "id"
            public string FileType { get; set; } // Correspondente ao "fileType"
            public string Url { get; set; } // Correspondente ao "url"
            public string Title { get; set; } // Correspondente ao "title"
            public string Length { get; set; } // Correspondente ao "length"
        }

    }
}
