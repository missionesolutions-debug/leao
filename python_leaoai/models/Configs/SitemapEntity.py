# models/sitemap_entity.py
from dataclasses import dataclass

@dataclass
class SitemapEntity:
    entity_name: str
    entity_route: str
    priority: float
    is_active: bool
