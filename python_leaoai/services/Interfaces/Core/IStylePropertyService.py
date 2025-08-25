from abc import ABC, abstractmethod
from typing import List, Optional
from dataclasses import dataclass

# Definição equivalente do StyleProperty
@dataclass
class StyleProperty:
    id: int
    owner_id: int
    owner_type: str
    name: str
    value: str
    # Adicione outros campos conforme necessário

class IStylePropertyService(ABC):

    @abstractmethod
    def get_style_properties(self, owner_id: int, owner_type: str) -> List[StyleProperty]:
        """
        Retorna todas as StyleProperties de um determinado owner.
        """
        pass

    @abstractmethod
    def add_style_property(self, style_property: StyleProperty) -> None:
        """
        Adiciona uma nova StyleProperty.
        """
        pass

    @abstractmethod
    def update_style_property(self, style_property: StyleProperty) -> None:
        """
        Atualiza uma StyleProperty existente.
        """
        pass

    @abstractmethod
    def delete_style_property(self, id: int) -> None:
        """
        Remove uma StyleProperty pelo seu ID.
        """
        pass
