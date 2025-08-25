# repositories/embalagem_factory.py
from sqlalchemy.orm import Session
from typing import List, Optional
from models.factory import Embalagem

class EmbalagemFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD ---
    def get_obj(self, id: int) -> Optional[Embalagem]:
        return self.db.query(Embalagem).filter(Embalagem.id == id).first()

    def save_obj(self, obj: Embalagem) -> Embalagem:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Embalagem) -> Embalagem:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Embalagem]:
        return self.db.query(Embalagem).all()

    def get_list_actives(self) -> List[Embalagem]:
        return self.db.query(Embalagem).filter(Embalagem.ativo == True).all()

    def get_list(self) -> List[Embalagem]:
        return self.db.query(Embalagem).filter(Embalagem.excluido == False).join(Embalagem.embalagem_type).all()

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
