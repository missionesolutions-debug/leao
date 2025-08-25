from abc import ABC, abstractmethod
from typing import List, Optional
from dataclasses import dataclass

# Definição equivalente do PageSectionItem
@dataclass
class PageSectionItem:
    id: int
    page_section_id: int
    title: str
    content: str
    # Adicione outros campos conforme necessário

class IPageSectionItemService(ABC):

    @abstractmethod
    def get_page_section_items(self, page_section_id: int) -> List[PageSectionItem]:
        """
        Retorna todos os PageSectionItems de uma determinada seção.
        """
        pass

    @abstractmethod
    def get_page_section_item_by_id(self, id: int) -> Optional[PageSectionItem]:
        """
        Retorna um PageSectionItem pelo seu ID.
        """
        pass

    @abstractmethod
    def add_page_section_item(self, page_section_item: PageSectionItem) -> None:
        """
        Adiciona um novo PageSectionItem.
        """
        pass

    @abstractmethod
    def update_page_section_item(self, page_section_item: PageSectionItem) -> None:
        """
        Atualiza um PageSectionItem existente.
        """
        pass

    @abstractmethod
    def delete_page_section_item(self, page_section_item_id: int) -> None:
        """
        Remove um PageSectionItem pelo seu ID.
        """
        pass
