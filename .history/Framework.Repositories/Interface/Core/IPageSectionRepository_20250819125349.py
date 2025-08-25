from abc import ABC, abstractmethod
from typing import List, Optional, Any
from datetime import datetime
class IPageSectionRepository(ABC):
    """
    Python interface equivalent to the C# IPageSectionRepository.
    Defines the contract for repository operations.
    """
    @abstractmethod
    def get_page_sections_by_owner(self, owner_id: int, owner_type): string) -> IEnumerable<PageSection>: # Basic return type mapping
        ...
