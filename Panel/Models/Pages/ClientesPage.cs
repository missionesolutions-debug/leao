using Data.Models.PainelConfig;
using Data.Models.Pessoa;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class ClientesPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<Cliente> Clientes { get; set; }
        public Cliente Cliente { get; set; }

    }
}
