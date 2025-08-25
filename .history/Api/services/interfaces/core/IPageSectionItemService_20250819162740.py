from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel # Assuming services might use Pydantic models
from abc import ABC, abstractmethod # For interfaces

class IPageSectionItemService(ABC):
    @abstractmethod
    def get_page_section_item_by_id(self, id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def add_page_section_item(self, page_section_item): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def update_page_section_item(self, page_section_item): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def delete_page_section_item(self, page_section_item_id): object # TODO: Specify correct type hint):
    pass

