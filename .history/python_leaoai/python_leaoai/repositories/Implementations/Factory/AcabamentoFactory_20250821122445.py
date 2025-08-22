# repositories/acabamento_factory.py
from sqlalchemy.orm import Session
from typing import List, Optional
from models.factory import Acabamento

class AcabamentoFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD ---
    def get_obj(self, id: int) -> Optional[Acabamento]:
        return self.db.query(Acabamento).filter(Acabamento.id == id).first()

    def save_obj(self, obj: Acabamento) -> Acabamento:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Acabamento) -> Acabamento:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Acabamento]:
        return self.db.query(Acabamento).all()

    def get_list_actives(self) -> List[Acabamento]:
        return self.db.query(Acabamento).filter(Acabamento.ativo == True).all()

    def get_list(self) -> List[Acabamento]:
        return self.db.query(Acabamento).filter(Acabamento.excluido == False).all()

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
