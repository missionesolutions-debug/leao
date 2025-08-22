from typing import TypeVar, Generic
from sqlalchemy.orm import Session
from services import ItemService
from repositories import Repository
from models import Produto  # Modelo SQLAlchemy do Produto
from services.generic_service import GenericService  # Serviço genérico base

T = TypeVar('T')

class ProdutosService(GenericService[Produto, Repository]):
    def __init__(self, db: Session, item_service: ItemService, repository: Repository):
        super().__init__(db, item_service, repository)
        self._db = db
        self._srvItem = item_service
        self._repository = repository

    # Aqui você pode adicionar métodos específicos de Produto, se necessário
