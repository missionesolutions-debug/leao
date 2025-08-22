# repositories/client_factory.py
from sqlalchemy.orm import Session
from typing import List, Optional
from models.factory import Client

class ClientFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD ---
    def get_obj(self, id: int) -> Optional[Client]:
        return self.db.query(Client).filter(Client.id == id).first()

    def save_obj(self, obj: Client) -> Client:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Client) -> Client:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Client]:
        return self.db.query(Client).all()

    def get_list(self) -> List[Client]:
        return self.db.query(Client).filter(Client.excluido == False).all()

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
