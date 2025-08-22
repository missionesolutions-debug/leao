# framework/services/configs_service.py

from typing import Any
from framework.repositories.repository import Repository
from framework.services.generic_service import GenericService
from data.models.core import HeadConfig
from framework.services.item_service import ItemService
from sqlalchemy.ext.asyncio import AsyncSession

class ConfigsService(GenericService[HeadConfig]):
    def __init__(
        self,
        session: AsyncSession,
        item_service: ItemService,
        repository: Repository[HeadConfig]
    ):
        super().__init__(session, item_service, repository)
        self._session = session
        self._srv_item = item_service
        self._repository = repository
