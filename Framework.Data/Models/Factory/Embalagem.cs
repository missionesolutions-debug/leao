namespace Framework.Data.Models.Factory
{
    public class Embalagem
    {
        public int Id { get; set; }
        public int EmbalagemTypeId { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; } = "";
        public string ImagemFormulacao { get; set; } = "";
        public string Codigo { get; set; } = "";
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }

        public virtual EmbalagemType EmbalagemType { get; set; }
    }
}
