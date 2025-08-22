# repositories/embalagem_type_factory.py
from sqlalchemy.orm import Session
from typing import List, Optional
from models.factory import EmbalagemType

class EmbalagemTypeFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD ---
    def get_obj(self, id: int) -> Optional[EmbalagemType]:
        return self.db.query(EmbalagemType).filter(EmbalagemType.id == id).first()

    def save_obj(self, obj: EmbalagemType) -> EmbalagemType:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: EmbalagemType) -> EmbalagemType:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[EmbalagemType]:
        return self.db.query(EmbalagemType).all()

    def get_list_actives(self) -> List[EmbalagemType]:
        return self.db.query(EmbalagemType).filter(EmbalagemType.ativo == True).all()

    def get_list(self) -> List[EmbalagemType]:
        return self.db.query(EmbalagemType).filter(EmbalagemType.excluido == False).all()

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            obj.ativo = False
            obj.excluido = True
            self.db.commit()
            return True
        except:
            self.db.rollback()
            return False
