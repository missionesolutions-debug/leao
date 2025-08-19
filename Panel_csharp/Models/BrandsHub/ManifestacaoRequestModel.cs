using Panel.Models.xCore;
using System.Collections.Generic;

namespace Panel.Models.BrandsHub
{
    public class ManifestacaoRequestModel
    {
        // Dados do Processo (Registro Contestado)
        public string ProcessoNumero { get; set; }
        public string ProcessoMarca { get; set; }
        public string ProcessoClasse { get; set; }
        public string ProcessoEspecificacao { get; set; }
        public string ProcessoTitular { get; set; }
        public DateTime? ProcessoDataDeposito { get; set; } // Campo extra

        // Dados da Oposição
        public string OposicaoRpi { get; set; }
        public DateTime? OposicaoDataRpi { get; set; }
        public string OposicaoOpoente { get; set; }
        public string OposicaoFundamento { get; set; }
        public string OposicaoMarca { get; set; }       // Campo extra
        public string OposicaoProcesso { get; set; }     // Campo extra

        // Dados da Defesa / Análise
        public string TipoConflitoAlegado { get; set; }
        public string TipoReproducaoAlegada { get; set; }
        public string AnaliseMercadologicaDefesa { get; set; }
        public string DefesaDistintividadeFonetica { get; set; }
        public string DefesaDistintividadeIdeologica { get; set; }
        public string DefesaDistintividadeVisual { get; set; } // Campo extra
        public string DefesaEspecialidadeSegmento { get; set; }  // Campo extra
        public string DefesaEspecialidadePublico { get; set; }   // Campo extra
        public string DefesaEspecialidadeCanais { get; set; }    // Campo extra
        public string DefesaCoexistencia { get; set; }           // Campo extra
        public string DefesaDecisoesAnteriores { get; set; }      // Campo extra
        public string UsoAnterior { get; set; }
        public string OutrosRegistros { get; set; }
        public string MaFe { get; set; }                         // Campo extra
        public ChatIAListMessagesResponse ListMessages { get; set; } = new ChatIAListMessagesResponse();
    }
}
