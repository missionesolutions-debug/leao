using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// PROJECT BLOCK
	public class ProjectBlock
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }

		[ForeignKey("ProjectPhase")]
		public int ProjectPhaseId { get; set; }
		public virtual ProjectPhase ProjectPhase { get; set; }

		[ForeignKey("ProjectItem")]
		public int ProjectItemId { get; set; }
		public virtual ProjectItem ProjectItem { get; set; }

		[ForeignKey("ProjectGroup")]
		public int ProjectGroupId { get; set; }
		public virtual ProjectGroup ProjectGroup { get; set; }

		// Se necessário, referenciar a tabela Usuario
		public int? UsuarioId { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }
		public int? Position { get; set; }

		[Required]
		public string Code { get; set; }  // Gerado como Guid.ToString()

        public string Name { get; set; }  // Gerado como Guid.ToString()

        public string Type { get; set; }
		public int MinImagesAmount { get; set; } = 0;
		public int MaxImagesAmount { get; set; } = 0;
		public string ImagesLabel { get; set; }
		public bool ObservationsEnabled { get; set; } = false;
		public bool IsCompleted { get; set; } = false;
		public string Instructions { get; set; }
		public string BoxType { get; set; }
		public string CardBoardType { get; set; }
		public string ClosureType { get; set; }
		public string ActionText { get; set; }
		public bool IsMeasurableIitems { get; set; } = false;
		public string Version { get; set; }
		public string Observation { get; set; }

		// Campos adicionais conforme solicitado
		public bool BoxTypeActive { get; set; } = false;
		public bool CardBoardTypeActive { get; set; } = false;
		public bool ClosureTypeActive { get; set; } = false;
		public bool VersionActive { get; set; } = false;
		public bool BlockForConclude { get; set; } = false;

		public bool Ativo { get; set; } = true;
		public bool Excluido { get; set; } = false;

		// Navegação
		public virtual ICollection<ProjectMeasureItem> ProjectMeasureItems { get; set; } = new List<ProjectMeasureItem>();
		public virtual ICollection<ProjectImage> ProjectImages { get; set; } = new List<ProjectImage>();
	}
}
