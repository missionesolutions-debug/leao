using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Framework.Data.Models.Factory
{
	// PROJECT PHASE
	public class ProjectPhase
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }

		[ForeignKey("ProjectItem")]
		public int ProjectItemId { get; set; }
		public virtual ProjectItem ProjectItem { get; set; }

		[Required]
		public string Name { get; set; }

		public bool Ativo { get; set; } = true;
		public bool Excluido { get; set; } = false;
		public bool IsApproved { get; set; } 
		public bool IsReproved { get; set; }
		public int UsuarioId { get; set; }
		public bool IsTaken { get; set; } = false;
		public DateTime? UpdatedAt { get; set; }

        // Navegação
        public virtual ICollection<ProjectGroup> ProjectGroups { get; set; } = new List<ProjectGroup>();
		public virtual ICollection<ProjectBlock> ProjectBlocks { get; set; } = new List<ProjectBlock>();
	}

}
