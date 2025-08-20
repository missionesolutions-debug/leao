from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel # Assuming services might use Pydantic models
from abc import ABC, abstractmethod # For interfaces

class IService(ABC):
    @abstractmethod
    def get_item_by_id(self, id): object # TODO: Specify correct type hint):
    pass

    @abstractmethod
    def get_item_by_url(self, url): object # TODO: Specify correct type hint):
    pass

