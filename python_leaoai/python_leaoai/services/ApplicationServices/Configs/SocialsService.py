from sqlalchemy.orm import Session
from typing import List, Optional
from models import RedesContato  # Supondo que você tenha a classe de modelo definida
from repositories import GenericRepository  # Repositório genérico equivalente

class SocialsService:
    def __init__(self, db: Session, repository: GenericRepository):
        self._db = db
        self._repository = repository

    # CRUD básico usando o repositório genérico
    def get(self, id: int) -> Optional[RedesContato]:
        return self._repository.get(id)

    def get_all(self) -> List[RedesContato]:
        return self._repository.get_all()

    def save(self, obj: RedesContato) -> RedesContato:
        return self._repository.save(obj)

    def update(self, obj: RedesContato) -> RedesContato:
        return self._repository.update(obj)

    def remove(self, obj: RedesContato) -> bool:
        return self._repository.remove(obj)

    def delete(self, id: int) -> bool:
        obj = self.get(id)
        if not obj:
            return False
        obj.ativo = False
        obj.excluido = True
        self._db.commit()
        return True

    # Aqui você pode adicionar métodos específicos de RedesContato
    def get_active(self) -> List[RedesContato]:
        return self._repository.get_by_condition(lambda x: x.ativo is True)

