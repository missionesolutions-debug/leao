using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ProjectTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string TemplateJson { get; set; }

        public bool Ativo { get; set; } = true;
        public bool Excluido { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
