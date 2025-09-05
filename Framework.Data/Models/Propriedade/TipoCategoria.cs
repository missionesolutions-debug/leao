
using System.Collections.Generic;

namespace Data.Models.Propriedade
{
    public class TipoCategoria : BaseConteudo
    {
        public TipoCategoria()
        {
            this.Categorias = new HashSet<Categoria>();
            this.SubCategorias = new HashSet<SubCategoria>();
        }
        public virtual ICollection<Categoria> Categorias { get; set; }
        public virtual ICollection<SubCategoria> SubCategorias { get; set; }
    }
}
