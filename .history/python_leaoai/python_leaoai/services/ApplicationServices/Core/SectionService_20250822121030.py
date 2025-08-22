from typing import List, Dict

class SectionService:
    def __init__(self, context, repository, metadata_repository):
        self._context = context
        self._repository = repository
        self._metadata_repository = metadata_repository

    def create_section(self, section_dto):
        # Verifica se já existe uma seção com a mesma Ref
        existing_section = self._repository.get_obj_by_ref(section_dto.ref)
        if existing_section:
            raise ValueError(f"A seção com a referência '{section_dto.ref}' já existe.")

        # Cria a seção
        section = Section(
            ref=section_dto.ref,
            link_url=section_dto.link_url,
            video_url=section_dto.video_url,
            enabled=section_dto.enabled,
            json_content=section_dto.json_content,
            section_translations=[]
        )

        # Traduções
        for lang, trans in section_dto.translations.items():
            section_translation = SectionTranslation(
                language=lang,
                title=trans.title,
                subtitle=trans.subtitle,
                description=trans.description,
                link_text=trans.link_text
            )
            section.section_translations.append(section_translation)

        self._repository.save_obj(section)
        return section.id

    def get_sections_by_page(self, page: str) -> List:
        sections = self._repository.get_objs_by_page_ref(page)
        section_dtos = []

        for s in sections:
            translations = {t.language: SectionListDto.TranslationDto(
                title=t.title,
                subtitle=t.subtitle,
                description=t.description,
                link_text=t.link_text
            ) for t in s.section_translations}

            images = []
            for img in s.images:
                images.append(SectionListDto.MetaDataDto(
                    position=img.position,
                    id=str(img.id),
                    file_type=img.file_type,
                    url=img.url,
                    title=img.file_name or "",
                    length=str(img.file_length),
                    mobile=self.get_mobile_image(str(img.id))
                ))

            section_dtos.append(SectionListDto(
                id=s.id,
                ref=s.ref,
                link_url=s.link_url,
                video_url=s.video_url,
                json_content=s.json_content,
                enabled=s.enabled,
                translations=translations,
                images=images
            ))

        return section_dtos

    def get_mobile_image(self, parent_guid: str) -> List:
        metadata_items = []
        child_metadata = self._metadata_repository.get_objs_by_ref_guid(parent_guid)
        for item in child_metadata:
            metadata_items.append(SectionListDto.MetaDataChildDto(
                position=item.position,
                id=str(item.id),
                file_type=item.file_type,
                url=item.url,
                title=item.file_name or "",
                length=str(item.file_length)
            ))
        return metadata_items

    def update_section(self, section_dto):
        existing_section = self._repository.get_obj_with_translation(section_dto.id)
        if not existing_section:
            raise KeyError(f"Seção com Id {section_dto.id} não encontrada.")

        # Verifica duplicidade de Ref
        duplicate = next((s for s in self._repository.get_all() if s.ref == section_dto.ref and s.id != section_dto.id), None)
        if duplicate:
            raise ValueError(f"A seção com a referência '{section_dto.ref}' já existe.")

        # Atualiza campos
        existing_section.link_url = section_dto.link_url
        existing_section.video_url = section_dto.video_url
        existing_section.enabled = section_dto.enabled
        existing_section.json_content = section_dto.json_content

        # Atualiza ou adiciona traduções
        for lang, trans in section_dto.translations.items():
            existing_translation = next((t for t in existing_section.section_translations if t.language == lang), None)
            if existing_translation:
                existing_translation.title = trans.title
                existing_translation.subtitle = trans.subtitle
                existing_translation.description = trans.description
                existing_translation.link_text = trans.link_text
            else:
                new_translation = SectionTranslation(
                    section_id=existing_section.id,
                    language=lang,
                    title=trans.title,
                    subtitle=trans.subtitle,
                    description=trans.description,
                    link_text=trans.link_text
                )
                existing_section.section_translations.append(new_translation)

        self._repository.update_obj(existing_section)
        return section_dto.id
