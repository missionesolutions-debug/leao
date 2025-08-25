from abc import ABC, abstractmethod
from typing import List, Optional, Any
from datetime import datetime
class IService(ABC):
    """
    Python interface equivalent to the C# IService.
    """

    @abstractmethod
    def list_item(self, ) -> List<Item>: # Basic return type mapping
        pass # TODO: Translate logic from C# method 'ListItem'

    @abstractmethod
    def get_item_by_id(self, id: int) -> Item: # Basic return type mapping
        pass # TODO: Translate logic from C# method 'GetItemById'

    @abstractmethod
    def get_item_by_url(self, url: string) -> Item: # Basic return type mapping
        pass # TODO: Translate logic from C# method 'GetItemByUrl'
