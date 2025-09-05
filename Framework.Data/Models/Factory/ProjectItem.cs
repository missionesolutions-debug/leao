using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// PROJECT ITEM
	public class ProjectItem
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }

		[Required]
		public string Name { get; set; }

		public string PdfReport { get; set; }
        public string ProductName { get; set; }
        public string Acabamento { get; set; }
        public string Codigo { get; set; }
        public int? Amount { get; set; }
		public bool IsApproved { get; set; } = false;
		public bool IsReproved { get; set; } = false;
		public DateTime? ConcludedAt { get; set; }
		public DateTime? StartedAt { get; set; }

		public bool Ativo { get; set; } = true;
		public bool Excluido { get; set; } = false;
		public bool IsTemplate { get; set; } = false;
        public int? AcabamentoId { get; set; }
        // FK para Embalagem (tipo Caixa)
        public int? EmbalagemCaixaId { get; set; }

        // FK para Embalagem (tipo Papelão)
        public int? EmbalagemPapelaoId { get; set; }

        // FK para Embalagem (tipo Fechamento)
        public int? EmbalagemFechamentoId { get; set; }

        // Navegação
        public virtual ICollection<ProjectPhase> ProjectPhases { get; set; } = new List<ProjectPhase>();
	}
}
