using System;

namespace Data.Models.Propriedade
{
    public class SubCategoria : BaseConteudo
    {
        public Nullable<int> CategoriaId { get; set; }
        public Nullable<int> TipoCategoriaId { get; set; }
       
        public virtual Categoria Categoria { get; set; }
        public virtual TipoCategoria TipoCategoria { get; set; }
    }
}
