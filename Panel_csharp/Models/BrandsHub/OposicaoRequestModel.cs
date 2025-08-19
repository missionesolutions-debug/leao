using Panel.Models.xCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Panel.Models.BrandsHub
{
    public class OposicaoRequestModel
    {
        [Required(ErrorMessage = "O campo Processo Contestado é obrigatório.")]
        public string ProcessoContestado { get; set; }

        [Required(ErrorMessage = "O campo Marca Contestada é obrigatório.")]
        public string MarcaContestada { get; set; }

        [Required(ErrorMessage = "O campo Classe Contestada é obrigatório.")]
        public string ClasseContestada { get; set; }

        [Required(ErrorMessage = "O campo Especificação Contestada é obrigatório.")]
        public string EspecificacaoContestada { get; set; }

        [Required(ErrorMessage = "O campo Titular Contestado é obrigatório.")]
        public string TitularContestado { get; set; }

        [Required(ErrorMessage = "O campo Número RPI é obrigatório.")]
        public string NumeroRpi { get; set; }

        [Required(ErrorMessage = "O campo Data RPI é obrigatório.")]
        public DateTime? DataRpi { get; set; }

        [Required(ErrorMessage = "O campo Nome do Cliente é obrigatório.")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Conflito é obrigatório.")]
        public string TipoConflito { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Reprodução é obrigatório.")]
        public string TipoReproducao { get; set; }

        [Required(ErrorMessage = "O campo Análise Mercadológica é obrigatório.")]
        public string AnaliseMercadologica { get; set; }

        [Required(ErrorMessage = "O campo Precedentes é obrigatório.")]
        public string Precedentes { get; set; }

        // Campos opcionais (não marcados como [Required])
        public string MarcaAnterior { get; set; }
        public string ProcessoAnterior { get; set; }
        public string ClasseAnterior { get; set; }
        public string ProdutosAnterior { get; set; }
        public string Coexistencia { get; set; }

        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();
    }
}
