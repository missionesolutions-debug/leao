using Framework.Data.Models.Core;
using Framework.Infrastructure;
using Framework.Repositories.Factories.Core;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Core
{
    public class MetadataService
    {
        private readonly MetadataRepository _repository;
        private readonly BlobService _blobService; // Adicionamos o BlobService aqui
        private readonly SectionRepository _sectionRepository;

        public MetadataService( MetadataRepository repository, BlobService blobService, SectionRepository sectionRepository)
        {
            _repository = repository;
            _blobService = blobService; // Injeção do BlobService
            _sectionRepository = sectionRepository;
        }

        // Este método processa o arquivo e o carrega usando o BlobService
        // Método para processar o arquivo e o carrega usando o BlobService e gera um GUID
        public async Task<Metadata> ProcessFileAsync(IFormFile file, string entityId, string entityType)
        {
            // Faz o upload do arquivo para o BlobStorage
            Uri fileUri = await _blobService.UploadBlobAsync(file);
            if (fileUri == null)
            {
                throw new Exception("Erro ao fazer upload do arquivo.");
            }

            // Inicializa variáveis
            int? sectionId = null;
            string metadataRef = null;

            // Verifica se o EntityId está associado a uma Section
            if (int.TryParse(entityId, out var sectionIdInt))
            {
                var section =  _sectionRepository.GetObj(sectionIdInt);
                if (section != null)
                {
                    sectionId = section.Id; // Associa o EntityId como SectionId
                }
            }

            // Se não encontrar Section, tenta buscar Metadata pelo Guid
            if (sectionId == null)
            {
                var parentMetadata = _repository.GetObj(entityId);
                if (parentMetadata != null)
                {
                    metadataRef = parentMetadata.Id.ToString().ToUpper(); // Se encontrar, define o MetadataRef
                }
            }

            // Gera o GUID para o novo Metadata
            string newGuid = Guid.NewGuid().ToString();

            // Cria o objeto Metadata
            var metadata = new Metadata
            {
                Url = fileUri.ToString(),
                FileName = file.FileName,
                FileLength = Convert.ToInt32(file.Length),
                FileType = file.ContentType,
                EntityType = entityType,
                SectionId = sectionId != null ? sectionId.Value : null, // Se não encontrar Section, será nulo
                MetadataRef = metadataRef, // Se não encontrar Metadata pai, será nulo
                Guid = newGuid, // Gera um novo GUID
                IsMain = false,
                Position = 0,
                CreatedAt = DateTime.UtcNow
            };

            // Salva o Metadata no banco de dados
            _repository.SaveObj(metadata);

            metadata.Section = null;

            // Retorna o Id do Metadata criado
            return metadata;
        }

        // Método para deletar o metadata e o arquivo no blob
        public bool DeleteMetadata(string id)
        {
            // Busca o Metadata no banco de dados
            var metadata = _repository.GetObj(id);


            if (metadata == null)
            {
                return false; // Retorna false se o Metadata não for encontrado
            }

            // Deleta o arquivo do blob storage
            //var deletedFromBlob = await _blobService.DeleteBlobAsync(metadata.Url);
            //if (!deletedFromBlob)
            //{
            //    throw new Exception("Falha ao deletar o arquivo no Blob Storage.");
            //}

            // Remove o Metadata do banco de dados
            _repository.RemoveObj(metadata);

            return true; // Retorna true se a operação foi bem-sucedida
        }
    }
}