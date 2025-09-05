using System;

namespace Data.Models.PainelConfig
{
    public class Page : Base
    {
        public Nullable<int> MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public string SVGIcon { get; set; }
        public string RoleGate { get; set; }
        public string Nome { get; set; }
        public string Route { get; set; }
        public string DeleteRoute { get; set; }
        public string DetailRoute { get; set; }
        public string Post { get; set; }
        public string Tela { get; set; }
        public string Backplace { get; set; }
        public string Obs { get; set; }
        public string TableAction { get; set; }
        public bool NavTabs { get; set; }
        public bool acOrdem { get; set; }
        public bool acAtivo { get; set; }
        public bool acDestaque { get; set; }
        public bool acNome { get; set; }
        public bool acTitulo { get; set; }
        public bool acSubtitulo { get; set; }
        public bool acDescricao { get; set; }
        public bool acImagem { get; set; }
        public bool acThumbnail { get; set; }
        public bool acImagemAlt { get; set; }
        public bool acThumbnailAlt { get; set; }
        public bool acTags { get; set; }
        public bool acPageTitle { get; set; }
        public bool acMetaDescription { get; set; }
        public bool acMetaImage { get; set; }
        public bool acUrl { get; set; }
        public bool acSlug { get; set; }
        public bool acExclusivo { get; set; }
        public bool acTipoCategoria { get; set; }
        public bool acCategoria { get; set; }
        public bool acSubCategoria { get; set; }
        public bool ShowJobTaskItem { get; set; }
        public bool ShowUsersJobTask { get; set; }
        public bool ShowContentDetailItem { get; set; }
        public bool ShowContent { get; set; }
        public bool ShowFileItem { get; set; }
        public bool ShowImageItem { get; set; }
        public bool ShowPieces { get; set; }
        public bool? IsSiteUrlActive { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string EntityRoute { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Guid { get; set; }
    }
}
