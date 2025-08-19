using Panel.Models.xCore;

namespace Panel.Models.BrandsHub
{
    public class ContraRazaoNulidadeRepostRequest
    {
        public string Cliente { get; set; }

        public string Classe { get; set; }

        public string Titular { get; set; }

        public string MarcaTerceiro { get; set; }

        public string ProcessoTerceiro { get; set; }

        public string MarcaRequerida { get; set; }

        public string ProcessoRequerido { get; set; }

        public string MarcaCliente { get; set; }

        public string ComentarSobreDiferencaEntreMarcas { get; set; }

        public string PlanilhaMarcasSimilares { get; set; }

        public string ComentarSobreDistincaoEntreProdutoServico { get; set; }

        public string Numero { get; set; }

        public DateTime? Data { get; set; }

        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();
    }
}
