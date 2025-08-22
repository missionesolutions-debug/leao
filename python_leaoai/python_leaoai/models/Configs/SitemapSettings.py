# models/sitemap_settings.py
from dataclasses import dataclass, field
from typing import List
from .sitemap_entity import SitemapEntity

@dataclass
class SitemapSettings:
    domain: str
    entities: List[SitemapEntity] = field(default_factory=list)
