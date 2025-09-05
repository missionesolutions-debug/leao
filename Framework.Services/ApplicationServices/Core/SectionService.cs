using Data;
using Framework.Data.Models.Core;
using Framework.Domain.Dtos.Section;
using System.Collections.Generic;
using System.Linq;
using Framework.Repositories.Factories.Core;

namespace Framework.Services.ApplicationServices.Core
{
    public class SectionService
    {
        private readonly ApplicationDbContext _context;
        private readonly SectionRepository _repository;
        private readonly MetadataRepository _metadataRepository;

        public SectionService(ApplicationDbContext context, SectionRepository repository, MetadataRepository metadataRepository)
        {
            _context = context;
            _repository = repository;
            _metadataRepository = metadataRepository;
        }

        public int CreateSection(SectionDto sectionDto)
        {
            // Verificar se já existe uma seção com a mesma Ref
            var existingSection = _repository.GetObj(sectionDto.Ref);

            if (existingSection != null)
            {
                // Lançar uma exceção ou retornar um valor que indique que a Ref já existe
                throw new InvalidOperationException($"A seção com a referência '{sectionDto.Ref}' já existe.");
            }

            // Mapeia o DTO para a entidade Section
            var section = new Section
            {
                Ref = sectionDto.Ref,
                LinkUrl = sectionDto.LinkUrl,
                VideoUrl = sectionDto.VideoUrl,
                Enabled = sectionDto.Enabled.Value,
                JsonContent = sectionDto.JsonContent,
                SectionTranslations = new List<SectionTranslation>()
            };

            // Mapeia as traduções (i18n) do DTO para a entidade SectionTranslation
            foreach (var translation in sectionDto.Translations)
            {
                var sectionTranslation = new SectionTranslation
                {
                    Language = translation.Key, // pt-BR, en-USA, etc.
                    Title = translation.Value.Title,
                    Subtitle = translation.Value.Subtitle,
                    Description = translation.Value.Description,
                    LinkText = translation.Value.LinkText
                };

                section.SectionTranslations.Add(sectionTranslation);
            }

            // Adiciona a nova seção no banco de dados usando o repositório
            _repository.SaveObj(section);

            return section.Id;
        }

        public List<SectionListDto> GetSectionsByPage(string page)
        {
            var sections = _repository.GetObjsByPageRef(page);

            // Mapeamento para o DTO SectionListDto
            var sectionDtos = sections.Select(s => new SectionListDto
            {
                Id = s.Id,
                Ref = s.Ref,
                LinkUrl = s.LinkUrl,
                VideoUrl = s.VideoUrl,
                JsonContent = s.JsonContent,
                Enabled = s.Enabled,

                // Mapeamento das traduções
                translations = s.SectionTranslations.ToDictionary(
                    t => t.Language, // A chave do Dictionary é o código da linguagem (ex: pt-BR, en-US)
                    t => new SectionListDto.TranslationDto
                    {
                        Title = t.Title,
                        Subtitle = t.Subtitle,
                        Description = t.Description,
                        LinkText = t.LinkText
                    }
                ),

                // Mapeamento das imagens
                Images = s.Images.Select(img => new SectionListDto.MetaDataDto
                {
                    Position = img.Position,
                    Id = img.Id.ToString(),
                    FileType = img.FileType,
                    Url = img.Url,
                    Title = img.FileName ?? "",
                    Length = img.FileLength.ToString(),

                    // Verifica se há um "filho" com base no campo MetadataRef
                    mobile = GetMobileImage(img.Id.ToString()) // Chama o método para buscar a imagem "filha"
                }).ToList()

            }).ToList();

            return sectionDtos;
        }

        private List<SectionListDto.MetaDataChildDto> GetMobileImage(string parentGuid)
        {
            List<SectionListDto.MetaDataChildDto> metadataItems = new List<SectionListDto.MetaDataChildDto>();
            // Busca o metadado "filho" que tem o MetadataRef igual ao Guid do pai
            List<Metadata> childMetadata = _metadataRepository.GetObjsByRefGuid(parentGuid);

            // Se encontrar um "filho", mapeia para o MetaDataDto
            if (childMetadata.Any())
            {
                foreach (var item in childMetadata)
                {
                    metadataItems.Add(new SectionListDto.MetaDataChildDto
                    {
                        Position = item.Position,
                        Id = item.Id.ToString(),
                        FileType = item.FileType,
                        Url = item.Url,
                        Title = item.FileName ?? "",
                        Length = item.FileLength.ToString()
                    });
                }
            }

            return metadataItems; // Retorna null se não houver um filho
        }

        public int UpdateSection(SectionDto sectionDto)
        {
            // Verifica se a seção existe
            var existingSection = _repository.GetObjWithTranslation(sectionDto.Id.Value);
            if (existingSection == null)
            {
                throw new KeyNotFoundException($"Seção com Id {sectionDto.Id} não encontrada.");
            }

            // Verifica se a Ref já existe e não pertence à seção atual
            var duplicateSection = _repository.GetAll().FirstOrDefault(s => s.Ref == sectionDto.Ref && s.Id != sectionDto.Id);
            if (duplicateSection != null)
            {
                throw new InvalidOperationException($"A seção com a referência '{sectionDto.Ref}' já existe.");
            }

            // Atualiza os campos da seção existente
            existingSection.LinkUrl = sectionDto.LinkUrl;
            existingSection.VideoUrl = sectionDto.VideoUrl;
            existingSection.Enabled = sectionDto.Enabled.Value;
            existingSection.JsonContent = sectionDto.JsonContent;

            // Atualiza ou cria novas traduções (i18n)
            foreach (var translationDto in sectionDto.Translations)
            {
                var existingTranslation = existingSection.SectionTranslations
                    .FirstOrDefault(t => t.Language == translationDto.Key);

                if (existingTranslation != null)
                {
                    // Atualiza a tradução existente
                    existingTranslation.Title = translationDto.Value.Title;
                    existingTranslation.Subtitle = translationDto.Value.Subtitle;
                    existingTranslation.Description = translationDto.Value.Description;
                    existingTranslation.LinkText = translationDto.Value.LinkText;
                }
                else
                {
                    // Adiciona uma nova tradução
                    var newTranslation = new SectionTranslation
                    {
                        SectionId = existingSection.Id,
                        Language = translationDto.Key,
                        Title = translationDto.Value.Title,
                        Subtitle = translationDto.Value.Subtitle,
                        Description = translationDto.Value.Description,
                        LinkText = translationDto.Value.LinkText
                    };
                    existingSection.SectionTranslations.Add(newTranslation);
                }
            }

            _repository.UpdateObj(existingSection);

            return sectionDto.Id.Value;
        }

    }
}
