using Framework.Domain.Site.Interfaces;

namespace Framework.Domain.Site.Core
{
    public class Body : IBody
    {
        public string BodyScripts { get; set; }
        public string TituloBanner { get; set; }
        public string DescricaoBanner { get; set; }
        public string Subtitulo { get; set; }
		public string Descricao { get; set; }
		public string NossaHistoria { get; set; }
		public string SolucoesPersonalizadas { get; set; }
	}
}
