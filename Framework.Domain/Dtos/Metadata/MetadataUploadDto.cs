using Microsoft.AspNetCore.Http;

namespace Framework.Domain.Dtos.Metadata
{
    public class MetadataUploadDto
    {
        public string EntityId { get; set; } // ID da entidade associada
        public string EntityType { get; set; } // Tipo da entidade (ex: 'imagessection')
        public IFormFile Files { get; set; } // Coleção de arquivos para upload
    }

}
