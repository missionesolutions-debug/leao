using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Conteudo
{
    public class PageSectionItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }

        public string Subtitulo { get; set; }
        public string Descricao { get; set; }

        // Campos de Controle
        public bool Ativo { get; set; } = true;
        public bool Excluido { get; set; } = false;

        public string Imagem { get; set; }
        public string Thumbnail { get; set; }
        public string Arquivo { get; set; }
        public string ImagemAlt { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }

        public string Ref { get; set; }
        public bool? IsBlocked { get; set; } = false;
        public bool? IsBlockedNewItem { get; set; } = false;
        public bool? IsBlockedRemoveItem { get; set; } = false;
        public bool? IsBlockedDelete { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataEdicao { get; set; }
        public int Ordem { get; set; }

        // Relação com PageSection
        public int PageSectionId { get; set; }
        public virtual PageSection PageSection { get; set; }

        // Coleções
        //public virtual ICollection<StyleProperty> StyleProperties { get; set; } = new List<StyleProperty>();
        //public virtual ICollection<Metadata> Metadatas { get; set; } = new List<Metadata>();
    }

}
