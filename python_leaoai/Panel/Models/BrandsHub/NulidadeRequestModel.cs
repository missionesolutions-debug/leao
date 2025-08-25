using Panel.Models.xCore;
using System.Collections.Generic;

namespace Panel.Models.BrandsHub
{
    public class NulidadeRequestModel
    {
        public string RegistroNumero { get; set; }
        public string RegistroMarca { get; set; }
        public string RegistroClasse { get; set; }
        public string RegistroEspecificacao { get; set; }
        public string RegistroTitular { get; set; }
        public DateTime? RegistroDataConcessao { get; set; }
        public string RegistroRpi { get; set; }
        public string AnteriorRegistro { get; set; }
        public string AnteriorMarca { get; set; }
        public string AnteriorClasse { get; set; }
        public string AnteriorEspecificacao { get; set; }
        public string AnteriorTitular { get; set; }
        public DateTime? AnteriorDataDeposito { get; set; }
        public string ComparacaoTipo { get; set; }
        public string ComparacaoElementos { get; set; }
        public string ComparacaoVisual { get; set; }
        public string ComparacaoFonetica { get; set; }
        public string Precedentes { get; set; }
        public string MaFe { get; set; }
        public string DanosMercado { get; set; }
        public string DecisoesAnteriores { get; set; }
        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();
    }
}
