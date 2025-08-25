using System.Collections.Generic;
using Data.Models;
using Framework.Data.Models.Factory;
using Painel.Models.Components;
using Painel.Models;
using Data.Models.PainelConfig;

namespace Panel.Models.Pages
{
	public class ProjectPage
	{
		public Page Page { get; set; } // Supondo que exista uma classe Page para gerenciar as configurações de layout e rota
		public ContentConfig ContentConfig { get; set; } // Supondo que exista essa classe para configuração da interface
		public Project Project { get; set; }
		public List<Project> Projects { get; set; }
		public List<ProjectItem> Products { get; set; }

		public List<Client> Clients { get; set; } = new List<Client>();
        public List<Embalagem> Embalagems { get; set; } = new List<Embalagem>();
        public List<Acabamento> Acabamentos { get; set; } = new List<Acabamento>();
        public List<Supplier> Suppliers { get; set; }
		public List<ProjectStatus> ProjectStatuses { get; set; }
		public Info Info { get; set; } // Classe para mensagens e informações de feedback
	}
}
