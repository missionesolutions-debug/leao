using Data.Models.Catalogo;
using Data.Models.PainelConfig;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class EquipesPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<Equipe> Equipes { get; set; }
        public Equipe Equipe { get; set; }
    }
}
