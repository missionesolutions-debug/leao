using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Data.Models.Factory
{
    public class Acabamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; } = "";
        public string Imagem { get; set; } = "";
        public string ImagemFormulacao { get; set; } = "";
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
