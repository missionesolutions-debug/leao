using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// STATUS DO PROJECT
	public class ProjectStatus
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Nome { get; set; }
	}
}
