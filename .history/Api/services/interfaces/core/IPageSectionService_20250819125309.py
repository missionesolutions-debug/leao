from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel # Assuming services might use Pydantic models
from abc import ABC, abstractmethod # For interfaces

class IPageSectionService(ABC):
    @abstractmethod
    def get_by_id(self, id: object # TODO: Specify correct type hint):
        pass

    @abstractmethod
    def add_page_section(self, page_section: object # TODO: Specify correct type hint):
        pass

    @abstractmethod
    def update_page_section(self, page_section: object # TODO: Specify correct type hint):
        pass

    @abstractmethod
    def delete_page_section(self, page_section_id: object # TODO: Specify correct type hint):
        pass

