using Data.Models.Propriedade;
using System;

namespace Data.Models.Catalogo
{
    public class Equipe : BaseCatalogo
    {
        public Nullable<int> PaginaId { get; set; }
        public string Chave { get; set; } = string.Empty;
        #region External Tables
        public Nullable<int> CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
        #endregion
    }
}
