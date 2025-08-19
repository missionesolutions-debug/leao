using Panel.Models.xCore;

namespace Panel.Models.BrandsHub
{
    public class RecursoIndeferimentoRequestModel
    {
        // Dados do Processo (registro indeferido)
        public string ProcessoNumero { get; set; }
        public string ProcessoMarca { get; set; }
        public string ProcessoClasse { get; set; }
        public string ProcessoEspecificacao { get; set; }
        public string ProcessoTitular { get; set; }
        public DateTime? ProcessoDataDeposito { get; set; } // Campo opcional

        // Dados do Indeferimento
        public string IndeferimentoRpi { get; set; }
        public DateTime? IndeferimentoDataRpi { get; set; }
        public string IndeferimentoFundamento { get; set; }
        public string IndeferimentoMotivo { get; set; }
        public string IndeferimentoAnterioridade { get; set; } // Opcional

        // Dados para a Defesa / Argumentação
        public string TipoConflitoApontado { get; set; }
        public string TipoReproducaoApontada { get; set; }
        public string AnaliseMercadologicaDefesa { get; set; }
        public string DefesaDistintividadeConjuntoMarcario { get; set; }
        public string DefesaDistintividadeElementosDistintivos { get; set; }
        public string DefesaDistintividadePrecedentes { get; set; }
        public string DefesaCoexistenciaRegistrosSimilares { get; set; }
        public string DefesaCoexistenciaSegmento { get; set; }
        public string DefesaCoexistenciaPublico { get; set; }
        public string DefesaOutros { get; set; }

        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();
    }

}
