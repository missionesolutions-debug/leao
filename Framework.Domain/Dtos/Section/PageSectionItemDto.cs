namespace Framework.Domain.Dtos.Section
{
    public class PageSectionItemDto
    {
        public int Id { get; set; }
        public int PageSectionId { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }
    }

}
