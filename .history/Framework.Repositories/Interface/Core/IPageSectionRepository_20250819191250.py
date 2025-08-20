from abc import ABC, abstractmethod
from typing import List, Optional
from datetime import datetime


class PageSection:
    """
    Classe de exemplo só para tipagem.
    """
    def __init__(self, id: int, owner_id: int, owner_type: str, created_at: Optional[datetime] = None):
        self.id = id
        self.owner_id = owner_id
        self.owner_type = owner_type
        self.created_at = created_at or datetime.now()


class IPageSectionRepository(ABC):
    """
    Python interface equivalente ao C# IPageSectionRepository.
    Define o contrato para operações do repositório.
    """

    @abstractmethod
    def get_page_sections_by_owner(self, owner_id: int, owner_type: str) -> List[PageSection]:
        """
        Retorna uma lista de PageSection com base no owner_id e owner_type.
        """
        pass
