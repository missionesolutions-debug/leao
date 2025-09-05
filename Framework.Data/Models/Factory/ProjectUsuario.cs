using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Data.Models.Factory
{
	// PROJECT USUÁRIO
	public class ProjectUsuario
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }

		// A tabela Usuario já existe; considere que UsuarioId referencie essa tabela.
		public int UsuarioId { get; set; }

		public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
	}
}
