using Framework.Data.Models.Factory;
using Painel.Models.Components;
using Painel.Models;
using System.Collections.Generic;
using Data.Models.PainelConfig;

namespace Panel.Models.Pages.Factory
{
    public class EmbalagemPage
    {
        public Page Page { get; set; }
        public ContentConfig ContentConfig { get; set; }
        public Embalagem Embalagem { get; set; }
        public List<Embalagem> Embalagems { get; set; }
        public List<EmbalagemType> EmbalagemTypes { get; set; } = new List<EmbalagemType>();
        public Info Info { get; set; }
    }
}
