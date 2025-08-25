using Framework.Data.Models.Factory;
using Painel.Models.Components;
using Painel.Models;
using System.Collections.Generic;
using Data.Models.PainelConfig;

namespace Panel.Models.Pages.Factory
{
    public class SupplierPage
    {
        public Page Page { get; set; }
        public ContentConfig ContentConfig { get; set; }
        public Supplier Supplier { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public Info Info { get; set; }
    }
}
