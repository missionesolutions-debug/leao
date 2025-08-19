using Framework.Data.Models.Factory;
using Painel.Models.Components;
using Painel.Models;
using System.Collections.Generic;
using Data.Models.PainelConfig;

namespace Panel.Models.Pages.Factory
{
    public class AcabamentoPage
    {
        public Page Page { get; set; }
        public ContentConfig ContentConfig { get; set; }
        public Acabamento Acabamento { get; set; }
        public List<Acabamento> Acabamentos { get; set; }
        public List<Client> Clients { get; set; }
        public Info Info { get; set; }
    }
}
