from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel # Assuming services might use Pydantic models
from abc import ABC, abstractmethod # For interfaces

class IStylePropertyService(ABC):
    @abstractmethod
    def add_style_property(self, style_property: object # TODO: Specify correct type hint):
        pass

    @abstractmethod
    def update_style_property(self, style_property: object # TODO: Specify correct type hint):
        pass

    @abstractmethod
    def delete_style_property(self, id: object # TODO: Specify correct type hint):
        pass

