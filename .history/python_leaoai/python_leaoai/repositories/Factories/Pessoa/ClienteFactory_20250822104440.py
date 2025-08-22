from sqlalchemy.orm import Session
from typing import List, Optional
from data.models.pessoa import Cliente


class ClienteFactory:
    def __init__(self, db: Session):
        self.db = db

    # -------------------- CRUD --------------------

    def get_obj(self, id: int) -> Optional[Cliente]:
        return self.db.query(Cliente).filter(Cliente.id == id).first()

    def save_obj(self, obj: Cliente) -> Cliente:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Cliente) -> Cliente:
        self.db.merge(obj)
        self.db.commit()
        return obj

    def get_all(self) -> List[Cliente]:
        return self.db.query(Cliente).all()

    def get_list(self) -> List[Cliente]:
        return self.db.query(Cliente).filter(Cliente.excluido != True).all()

    def get_all_ativo(self) -> List[Cliente]:
        return self.db.query(Cliente).filter(Cliente.ativo == True).all()

    def get_all_excluido(self) -> List[Cliente]:
        return self.db.query(Cliente).filter(Cliente.excluido == True).all()

    def remove_obj(self, obj: Cliente) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except Exception:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self.db.query(Cliente).filter(Cliente.id == id).first()
            if obj:
                obj.ativo = False
                obj.excluido = True
                self.db.commit()
                return True
            return False
        except Exception:
            self.db.rollback()
            return False
