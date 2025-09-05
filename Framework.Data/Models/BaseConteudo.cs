namespace Data.Models
{
    public abstract class BaseConteudo : Base
    {
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string Thumbnail { get; set; }
        public string ImagemAlt { get; set; }
        public string ThumbnailAlt { get; set; }
        public string Tags { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        public string Url { get; set; }
        public string Slug { get; set; }

    }
}
