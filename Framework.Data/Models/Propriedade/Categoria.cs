using System;

namespace Data.Models.Propriedade
{
	public class Categoria : BaseConteudo
    {
        #region Tables Filhos

        #endregion

        #region Tables Pais
        public Nullable<int> TipoCategoriaId { get; set; }
        public virtual TipoCategoria TipoCategoria { get; set; }
        #endregion
    }
}
