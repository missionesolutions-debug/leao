using System;
using System.Collections.Generic;

namespace Framework.Domain.Site.Component
{
    public class Item
    {
        public int? Id { get; set; } = null;
        public int? Ordem { get; set; } = null;
		public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Thumbnail { get; set; }
        public string Imagem { get; set; }
        public string Ref { get; set; }
		public string PageTitle { get; set; }
		public string MetaDescription { get; set; }
		public string ImageOpenGraph { get; set; }
		public string HeadScripts { get; set; }
		public string BodyScripts { get; set; }
        public string GroupPagina { get; set; }
		public string ImagemAlt { get; set; }
        public string ThumbnailAlt { get; set; }
        public string ImagemMobile { get; set; }
        public string Arquivo { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
		public bool? Destaque { get; set; }
        public string Datas { get; set; }
		public bool? Menu { get; set; }
		public Nullable<System.DateTime> Data { get; set; }
        public Nullable<System.DateTime> DataCriacao { get; set; }
		public string Link { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataCadastro { get; set; }
        // Dicionário para campos dinâmicos
        public Dictionary<string, object> fields { get; set; } = new Dictionary<string, object>();
        public List<Item> imagens { get; set; } = new List<Item>();
        public List<Item> Items { get; set; } = new List<Item>();

        public string Chave { get; set; }

    }
    
}
