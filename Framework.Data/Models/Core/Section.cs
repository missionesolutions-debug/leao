using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Framework.Data.Models.Core
{
    public class Section
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string Ref { get; set; }

        public string LinkUrl { get; set; } 

        public string VideoUrl { get; set; } 

        public string JsonContent { get; set; } = string.Empty;

        [Required]
        public bool Enabled { get; set; } = true;
        public List<SectionTranslation> SectionTranslations { get; set; } = new List<SectionTranslation>();

        [JsonIgnore]
        public List<Metadata> Images { get; set; } = new List<Metadata>();

    }
}
