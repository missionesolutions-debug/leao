# repositories/supplier_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models import Supplier

class SupplierFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[Supplier]:
        return self.db.query(Supplier).filter(Supplier.id == id).first()

    def save_obj(self, obj: Supplier) -> Supplier:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Supplier) -> Supplier:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Supplier]:
        return self.db.query(Supplier).all()

    def get_list(self) -> List[Supplier]:
        return self.db.query(Supplier).filter(Supplier.excluido == False).all()

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            obj.ativo = False
            obj.excluido = True
            self.db.merge(obj)
            self.db.commit()
            return True
        except:
            self.db.rollback()
            return False
