# framework/services/sitemap_unit_of_work_service.py

from typing import List
from framework.data.models.common import SitemapItem
from framework.repositories.interface.servico_repository import IServicoRepository
from framework.repositories.interface.blog_repository import IBlogRepository
from framework.repositories.interface.pagina_repository import IPaginaRepository

class SitemapUnitOfWorkService:
    def __init__(
        self,
        servico_repository: IServicoRepository,
        blog_repository: IBlogRepository,
        pagina_repository: IPaginaRepository
    ):
        self._servico_repository = servico_repository
        self._blog_repository = blog_repository
        self._pagina_repository = pagina_repository

    async def get_servicos_async(self) -> List[SitemapItem]:
        servicos = await self._servico_repository.get_active_pages_async()
        return [SitemapItem(url=s.url, last_modified=s.data_edicao) for s in servicos]

    async def get_blogs_async(self) -> List[SitemapItem]:
        blogs = await self._blog_repository.get_active_pages_async()
        return [SitemapItem(url=b.url, last_modified=b.data_edicao) for b in blogs]

    async def get_paginas_async(self) -> List[SitemapItem]:
        paginas = await self._pagina_repository.get_active_pages_async()
        return [SitemapItem(url=p.url, last_modified=p.data_edicao) for p in paginas]

    # Você pode adicionar métodos futuros como get_produtos_async(), get_artigos_async(), etc.
