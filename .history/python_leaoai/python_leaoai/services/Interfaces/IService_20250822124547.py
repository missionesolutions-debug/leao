from abc import ABC, abstractmethod
from typing import List, Optional

# Supondo que exista uma classe Item equivalente à do C#
class Item:
    def __init__(self, id: int, url: str):
        self.id = id
        self.url = url

class IService(ABC):
    """
    Interface para serviços que manipulam itens do site.
    """

    @abstractmethod
    def list_item(self) -> List[Item]:
        pass

    @abstractmethod
    def get_item_by_id(self, id: int) -> Optional[Item]:
        pass

    @abstractmethod
    def get_item_by_url(self, url: str) -> Optional[Item]:
        pass
