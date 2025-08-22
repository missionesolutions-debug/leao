from abc import ABC, abstractmethod
from typing import List, Optional
from dataclasses import dataclass

# Definição equivalente do PageSection
@dataclass
class PageSection:
    id: int
    owner_id: int
    owner_type: str
    title: str
    # Adicione outros campos conforme necessário

class IPageSectionService(ABC):

    @abstractmethod
    def get_page_sections_by_owner(self, owner_id: int, owner_type: str) -> List[PageSection]:
        """
        Retorna todas as PageSections de um determinado owner.
        """
        pass

    @abstractmethod
    def get_by_id(self, id: int) -> Optional[PageSection]:
        """
        Retorna uma PageSection pelo seu ID.
        """
        pass

    @abstractmethod
    def add_page_section(self, page_section: PageSection) -> None:
        """
        Adiciona uma nova PageSection.
        """
        pass

    @abstractmethod
    def update_page_section(self, page_section: PageSection) -> None:
        """
        Atualiza uma PageSection existente.
        """
        pass

    @abstractmethod
    def delete_page_section(self, page_section_id: int) -> None:
        """
        Remove uma PageSection pelo seu ID.
        """
        pass
