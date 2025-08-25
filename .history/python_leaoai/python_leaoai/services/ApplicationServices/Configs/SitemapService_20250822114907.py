# framework/services/sitemap_service.py

from typing import List, Optional
from datetime import datetime
from framework.data.models.common import SitemapUrl, SitemapItem
from framework.factories.system.page_factory import PageFactory
from framework.services.application_services.configs.sitemap_unit_of_work_service import SitemapUnitOfWorkService

class SitemapSettings:
    def __init__(self, domain: str):
        self.domain = domain


class SitemapService:
    def __init__(
        self,
        page_repository: PageFactory,
        sitemap_settings: SitemapSettings,
        sitemap_unit_of_work_service: SitemapUnitOfWorkService
    ):
        self._page_repository = page_repository
        self._sitemap_settings = sitemap_settings
        self._sitemap_uow = sitemap_unit_of_work_service

    async def get_sitemap_async(self) -> List[SitemapUrl]:
        sitemap_urls: List[SitemapUrl] = []

        active_pages = await self._page_repository.get_pages_for_sitemap_async()

        for page in active_pages:
            entity_name = page.entity_name
            entity_route = page.entity_route
            priority = page.priority

            items = await self.get_items_for_entity_async(entity_name)
            if not items:
                continue

            sitemap_urls.extend(self.build_urls(entity_route, items, priority))

        return sorted(sitemap_urls, key=lambda x: x.priority or 0, reverse=True)

    async def get_items_for_entity_async(self, entity_name: str) -> Optional[List[SitemapItem]]:
        if entity_name == "Servico":
            return await self._sitemap_uow.get_servicos_async()
        elif entity_name == "Blog":
            return await self._sitemap_uow.get_blogs_async()
        elif entity_name == "Pagina":
            return await self._sitemap_uow.get_paginas_async()
        else:
            return None

    def build_urls(self, entity_route: str, items: List[SitemapItem], priority: Optional[str]) -> List[SitemapUrl]:
        urls: List[SitemapUrl] = []
        for item in items:
            url = SitemapUrl(
                loc=self.get_location(entity_route, item.url),
                lastmod=item.last_modified,
                changefreq="weekly",
                priority=priority
            )
            urls.append(url)
        return urls

    def get_location(self, entity_route: str, url: str) -> str:
        if entity_route:
            return f"{self._sitemap_settings.domain}/{entity_route}/{url}"
        return f"{self._sitemap_settings.domain}/{url}"

    def convert_sitemap_to_xml(self, sitemap_urls: List[SitemapUrl]) -> str:
        xml = ['<?xml version="1.0" encoding="UTF-8"?>']
        xml.append('<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">')

        for url in sitemap_urls:
            xml.append("  <url>")
            xml.append(f"    <loc>{url.loc}</loc>")
            if url.lastmod:
                xml.append(f"    <lastmod>{url.lastmod.strftime('%Y-%m-%d')}</lastmod>")
            if url.changefreq:
                xml.append(f"    <changefreq>{url.changefreq}</changefreq>")
            if url.priority:
                xml.append(f"    <priority>{url.priority}</priority>")
            xml.append("  </url>")

        xml.append("</urlset>")
        return "\n".join(xml)
