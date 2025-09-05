using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Factory
{
	// PROJECT
	public class Project
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[ForeignKey("ProjectStatus")]
		public int ProjectStatusId { get; set; }
		public virtual ProjectStatus ProjectStatus { get; set; }

		[ForeignKey("Client")]
		public int ClientId { get; set; }
		public virtual Client Client { get; set; }

		[ForeignKey("Supplier")]
		public int SupplierId { get; set; }
		public virtual Supplier Supplier { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? FinishedAt { get; set; }
		public DateTime? DatePrevisioned { get; set; }

		public string Observations { get; set; }
		public string PdfReportTotal { get; set; }
        public string Guid { get; set; }

        public string ProductName { get; set; }
        public string Acabamento { get; set; }
        public string Codigo { get; set; }

        public bool Ativo { get; set; } = true;
		public bool Excluido { get; set; } = false;
		public bool IsTemplate { get; set; } = false;

		// Navegação
		public virtual ICollection<ProjectItem> ProjectItems { get; set; } = new List<ProjectItem>();
		public virtual ICollection<ProjectUsuario> ProjectUsuarios { get; set; } = new List<ProjectUsuario>();
	}
}
