using Data.Models.Pessoa;
using Panel.Models.xCore;
using System.ComponentModel.DataAnnotations;
using static MudBlazor.Defaults;

namespace Panel.Models.BrandsHub
{
    public class ContraRazaoNulidadeRequestModel
    {
        [Required(ErrorMessage = "O campo Cliente é obrigatório.")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "O campo Classe é obrigatório.")]
        public string Classe { get; set; }

        [Required(ErrorMessage = "O campo Titular é obrigatório.")]
        public string Titular { get; set; }

        [Required(ErrorMessage = "O campo Especificações é obrigatório.")]
        public string Especificacoes { get; set; }

        [Required(ErrorMessage = "O campo Marca do Terceiro é obrigatório.")]
        public string MarcaTerceiro { get; set; }

        [Required(ErrorMessage = "O campo Processo do Terceiro é obrigatório.")]
        public string ProcessoTerceiro { get; set; }

        [Required(ErrorMessage = "O campo Marca Requerida é obrigatório.")]
        public string MarcaRequerida { get; set; }

        [Required(ErrorMessage = "O campo Processo Requerido é obrigatório.")]
        public string ProcessoRequerido { get; set; }

        [Required(ErrorMessage = "O campo Marca do Cliente é obrigatório.")]
        public string MarcaCliente { get; set; }

        [Required(ErrorMessage = "O campo Comentário sobre Diferença entre Marcas é obrigatório.")]
        public string ComentarSobreDiferencaEntreMarcas { get; set; }

        [Required(ErrorMessage = "O campo Planilha de Marcas Similares é obrigatório.")]
        public string PlanilhaMarcasSimilares { get; set; }

        [Required(ErrorMessage = "O campo Comentário sobre Distinção entre Produto e Serviço é obrigatório.")]
        public string ComentarSobreDistincaoEntreProdutoServico { get; set; }

        [Required(ErrorMessage = "O campo Numero é obrigatório.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "O campo Data é obrigatório.")]
        public DateTime? Data { get; set; }

        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();
    }
}
