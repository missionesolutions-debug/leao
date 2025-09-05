using Data.Models.Propriedade;
using System;

namespace Data.Models.Conteudo
{
    public class Pagina : BaseConteudo
    {
        public string GroupPagina { get; set; }
        public Nullable<int> CategoriaId { get; set; }
        public Nullable<int> AutorId { get; set; }
        public virtual Categoria Categoria { get; set; }
      
	}
}
