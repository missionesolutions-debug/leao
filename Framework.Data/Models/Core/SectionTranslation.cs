using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Core
{
    public class SectionTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        [Required]
        public string Language { get; set; } // pt-BR, en-USA, etc.

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }
        public string LinkText { get; set; }
    }
}
