using Framework.Data.Models.Factory;
using Painel.Models.Components;
using Painel.Models;
using System.Collections.Generic;
using Data.Models.PainelConfig;

namespace Panel.Models.Pages.Factory
{
    public class EmbalagemTypePage
    {
        public Page Page { get; set; }
        public ContentConfig ContentConfig { get; set; }
        public EmbalagemType EmbalagemType { get; set; }
        public List<EmbalagemType> EmbalagemTypes { get; set; }
        public Info Info { get; set; }
    }
}
