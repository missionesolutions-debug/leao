from abc import ABC, abstractmethod
from typing import List, Optional, Any
from datetime import datetime


class PageSectionItem:
    """
    Classe de exemplo só para tipagem.
    Você pode expandir conforme precisar.
    """
    def __init__(self, id: int, content: str, created_at: Optional[datetime] = None):
        self.id = id
        self.content = content
        self.created_at = created_at or datetime.now()


class IPageSectionItemRepository(ABC):
    """
    Python interface equivalente ao C# IPageSectionItemRepository.
    Define o contrato para operações do repositório.
    """

    @abstractmethod
    def get_page_section_items_by_page_section_id(self, page_section_id: int) -> List[PageSectionItem]:
        """
        Retorna uma lista de PageSectionItem com base no ID da seção.
        """
        pass
