from abc import ABC, abstractmethod
from typing import List
from dataclasses import dataclass

# Definição equivalente do SitemapUrl
@dataclass
class SitemapUrl:
    loc: str
    lastmod: str | None = None
    changefreq: str | None = None
    priority: float | None = None

class ISitemapService(ABC):
    @abstractmethod
    async def get_sitemap_async(self) -> List[SitemapUrl]:
        """
        Retorna a lista de URLs do sitemap de forma assíncrona.
        """
        pass

    @abstractmethod
    def convert_sitemap_to_xml(self, sitemap_urls: List[SitemapUrl]) -> str:
        """
        Converte a lista de SitemapUrl em XML.
        """
        pass
