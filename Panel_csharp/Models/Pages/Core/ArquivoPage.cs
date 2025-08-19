using Data.Models.Core;
using Data.Models.PainelConfig;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages.Core
{
    public class ArquivoPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }

        public string Tab { get; set; }
        public int Id { get; set; }
        public List<Arquivo> Arquivos { get; set; }
        public Arquivo Arquivo { get; set; }
    }
}
