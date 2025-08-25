from typing import TypeVar, Generic, List
from sqlalchemy.orm import Session
from services import ItemService
from repositories import Repository
from exceptions import NotFoundException, InvalidArgumentException

T = TypeVar("T")  # Tipo de entidade

class GenericService(Generic[T]):
    def __init__(self, db: Session, item_service: ItemService, repository: Repository[T]):
        self._db = db
        self._srvItem = item_service
        self._repository = repository

    def list_item(self) -> List[dict]:
        try:
            items = self._repository.get_all_ativo()
            return [self._srvItem.build(item) for item in items]
        except Exception as ex:
            raise Exception("An error occurred while retrieving items") from ex

    def get_item_by_id(self, id: int) -> dict:
        if id <= 0:
            raise InvalidArgumentException("ID must be greater than zero")

        try:
            item = self._repository.get_obj(id)
            if not item:
                raise NotFoundException(f"Item with ID {id} not found")
            return self._srvItem.build(item)
        except Exception as ex:
            raise Exception("An error occurred while retrieving the item by ID") from ex

    def get_item_by_url(self, url: str) -> dict:
        if not url:
            raise InvalidArgumentException("URL cannot be null or empty")

        try:
            item = self._repository.get_by_url(url)
            if not item:
                raise NotFoundException(f"Item with URL '{url}' not found")
            return self._srvItem.build(item)
        except Exception as ex:
            raise Exception("An error occurred while retrieving the item by URL") from ex
