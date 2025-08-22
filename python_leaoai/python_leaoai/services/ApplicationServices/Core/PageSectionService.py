from typing import List, Optional

class PageSectionService:
    def __init__(self, page_section_repository):
        self._page_section_repository = page_section_repository

    def get_page_sections_by_owner(self, owner_id: int, owner_type: str) -> List:
        """Retorna todas as PageSections de um dono específico"""
        return self._page_section_repository.get_by_owner(owner_id, owner_type)

    def get_by_id(self, id: int):
        """Retorna uma PageSection pelo ID"""
        return self._page_section_repository.get_by_id(id)

    def add_page_section(self, page_section):
        """Adiciona uma nova PageSection"""
        self._page_section_repository.add(page_section)

    def update_page_section(self, page_section):
        """Atualiza uma PageSection existente"""
        self._page_section_repository.update(page_section)

    def delete_page_section(self, page_section_id: int):
        """Marca uma PageSection como excluída e inativa"""
        page_section = self._page_section_repository.get_by_id(page_section_id)
        if page_section:
            page_section.excluido = True
            page_section.ativo = False
            self._page_section_repository.update(page_section)
