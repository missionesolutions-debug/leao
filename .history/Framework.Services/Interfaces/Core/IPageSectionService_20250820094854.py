from abc import ABC, abstractmethod
from typing import List, Optional, Any
from datetime import datetime
class IPageSectionService(ABC):
    """
    Python interface equivalent to the C# IPageSectionService.
    """

    @abstractmethod
    def get_page_sections_by_owner(self, owner_id: int, owner_type: string) -> IEnumerable_PageSection: # Basic return type mapping
        pass # TODO: Translate logic from C# method 'GetPageSectionsByOwner'
