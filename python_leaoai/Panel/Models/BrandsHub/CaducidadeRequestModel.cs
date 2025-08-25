using Panel.Models.xCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Panel.Models.BrandsHub
{
    public class CaducidadeRequestModel
    {
        [Required(ErrorMessage = "O campo Registro da Marca é obrigatório.")]
        public string RegistroRequerida { get; set; }

        [Required(ErrorMessage = "O campo Marca Requerida é obrigatório.")]
        public string MarcaRequerida { get; set; }

        [Required(ErrorMessage = "O campo Classe da Marca é obrigatório.")]
        public string ClasseRequerida { get; set; }

        [Required(ErrorMessage = "O campo Especificações é obrigatório.")]
        public string EspecificacoesRequerida { get; set; }

        [Required(ErrorMessage = "O campo Titular da Marca é obrigatório.")]
        public string TitularRequerida { get; set; }

        [Required(ErrorMessage = "O campo Número do Processo é obrigatório.")]
        public string ProcessoRequerida { get; set; }

        [Required(ErrorMessage = "O campo Nome do Cliente é obrigatório.")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "O campo Marca do Cliente é obrigatório.")]
        public string MarcaCliente { get; set; }

        [Required(ErrorMessage = "O campo Data do Depósito é obrigatório.")]
        public DateTime? DataDepositoCliente { get; set; }

        [Required(ErrorMessage = "O campo Classe da Marca do Cliente é obrigatório.")]
        public string ClasseCliente { get; set; }

        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();

    }
}
