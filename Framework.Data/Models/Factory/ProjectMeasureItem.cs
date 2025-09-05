using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// PROJECT MEASURE ITEM
	public class ProjectMeasureItem
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("ProjectBlock")]
		public int ProjectBlockId { get; set; }
		public virtual ProjectBlock ProjectBlock { get; set; }

		[Required]
		public string Name { get; set; }

		public string Width { get; set; }
		public string Height { get; set; }
		public string Length { get; set; }
		public string Weight { get; set; }

		public bool NameEditable { get; set; } = false;
	}
}
