using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// PROJECT GROUP
	public class ProjectGroup
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

		[Required]
		public string Name { get; set; }

		public bool Ativo { get; set; } = true;
		public bool Excluido { get; set; } = false;

		// Navegação
		public virtual ICollection<ProjectBlock> ProjectBlocks { get; set; } = new List<ProjectBlock>();
	}
}
