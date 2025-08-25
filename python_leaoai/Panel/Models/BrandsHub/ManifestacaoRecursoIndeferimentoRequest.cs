using Panel.Models.xCore;
using System.ComponentModel.DataAnnotations;

namespace Panel.Models.BrandsHub
{
    public class ManifestacaoRecursoIndeferimentoRequest
    {
        [Required] public string Cliente { get; set; }
        [Required] public string ProcessoNumero { get; set; }
        [Required] public string Marca { get; set; }
        [Required] public string Classe { get; set; }
        [Required] public string Titular { get; set; }
        [Required] public string RpiNumero { get; set; }
        [Required] public DateTime RpiData { get; set; }
        [Required] public string FundamentoRecorrente { get; set; }
        [Required] public string FundamentoLegal { get; set; }
        [Required] public string RazoesContra { get; set; }
        [Required] public string Infrações { get; set; }
        [Required] public string NomeCliente { get; set; }
        public ChatIAListMessagesResponse ListMessages { get; set; } = new();
    }

}
