using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Framework.Data.Models.Conteudo
{

    public class PageSection
    {
        [Key]
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Subtitulo { get; set; }
        public string Descricao { get; set; }

        // Campos de Controle
        public bool Ativo { get; set; } = true;
        public bool Excluido { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataEdicao { get; set; }
        public string Ref { get; set; }
        public bool? IsBlocked { get; set; } = false;
        public bool? IsBlockedNewItem { get; set; } = false;
        public bool? IsBlockedRemoveItem { get; set; } = false;
        public bool? IsBlockedDelete { get; set; } = false;
        public int Ordem { get; set; }

        public string Imagem { get; set; }
        public string Thumbnail { get; set;}
        public string Arquivo { get; set; }
        public string ImagemAlt { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }

        // Referências à entidade pai
        public int OwnerId { get; set; }
        public string OwnerType { get; set; }

        // Coleções
        public virtual ICollection<PageSectionItem> PageSectionItems { get; set; } = new List<PageSectionItem>();
        //public virtual ICollection<StyleProperty> StyleProperties { get; set; } = new List<StyleProperty>();
        //public virtual ICollection<Metadata> Metadatas { get; set; } = new List<Metadata>();
    }
}

