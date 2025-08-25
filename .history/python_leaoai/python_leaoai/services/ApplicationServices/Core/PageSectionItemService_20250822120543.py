from typing import List, Optional

class PageSectionItemService:
    def __init__(self, page_section_item_repository):
        self._page_section_item_repository = page_section_item_repository

    def get_page_section_items(self, page_section_id: int) -> List:
        """Retorna todos os PageSectionItems de uma seção específica"""
        return self._page_section_item_repository.get_by_page_section_id(page_section_id)

    def get_page_section_item_by_id(self, id: int):
        """Retorna um PageSectionItem pelo ID"""
        return self._page_section_item_repository.get_by_id(id)

    def add_page_section_item(self, page_section_item):
        """Adiciona um novo PageSectionItem"""
        self._page_section_item_repository.add(page_section_item)

    def update_page_section_item(self, page_section_item):
        """Atualiza um PageSectionItem existente"""
        self._page_section_item_repository.update(page_section_item)

    def delete_page_section_item(self, page_section_item_id: int):
        """Marca um PageSectionItem como excluído e inativo"""
        page_section_item = self._page_section_item_repository.get_by_id(page_section_item_id)
        if page_section_item:
            page_section_item.excluido = True
            page_section_item.ativo = False
            self._page_section_item_repository.update(page_section_item)
