using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// PROJECT IMAGE
	public class ProjectImage
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }

		[ForeignKey("ProjectBlock")]
		public int ProjectBlockId { get; set; }
		public virtual ProjectBlock ProjectBlock { get; set; }

		[Required]
		public string UrlSource { get; set; }
		public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

		public bool enableOnReport { get; set; } = false;
		public string description { get; set; }
	}
}
