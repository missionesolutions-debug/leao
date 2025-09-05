using System.Collections.Generic;

namespace Framework.Domain.Dtos.Section
{
    public class PageSectionDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string OwnerType { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }
        public List<PageSectionItemDto> Items { get; set; }
    }
}
