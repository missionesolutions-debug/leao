from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel # Assuming services might use Pydantic models
from abc import ABC, abstractmethod # For interfaces

class ISitemapService(ABC):
    @abstractmethod
    def convert_sitemap_to_xml(self, sitemap_urls: object # TODO: Specify correct type hint):
        pass

