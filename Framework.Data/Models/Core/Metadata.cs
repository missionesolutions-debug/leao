using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Data.Models.Core
{
    public class Metadata
    {
        [Key]
        public Guid Id { get; set; } // Agora o Id é do tipo GUID

        public string Url { get; set; } // URL do arquivo

        public string FileName { get; set; } // Nome do arquivo

        public int FileLength { get; set; } // Tamanho do arquivo

        public string FileType { get; set; } // Tipo do arquivo
        public string MetadataRef { get; set; }
        public string Guid { get; set; }

        public string EntityType { get; set; } // Tipo de entidade associada

        public int? SectionId { get; set; } // Chave estrangeira para a Section

        // Relação com a entidade Section
        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        public bool? IsMain { get; set; } = true; // Arquivo principal

        public int Position { get; set; } = 0; // Posição do arquivo

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow; // Data de criação

    }

}
