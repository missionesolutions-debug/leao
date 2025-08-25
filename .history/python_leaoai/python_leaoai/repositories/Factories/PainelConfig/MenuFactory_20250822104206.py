from sqlalchemy.orm import joinedload
from sqlalchemy.exc import SQLAlchemyError
from typing import List, Optional
from database import SessionLocal
from data.models.painel_config import Menu


class MenuFactory:
    def __init__(self, db: Optional[SessionLocal] = None):
        self.db = db or SessionLocal()

    # -----------------------------
    # CRUD
    # -----------------------------
    def get_obj(self, id: int) -> Optional[Menu]:
        return self.db.query(Menu).filter(Menu.id == id).first()

    def save_obj(self, obj: Menu) -> Menu:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Menu) -> Menu:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Menu]:
        return self.db.query(Menu).all()

    def get_list(self) -> List[Menu]:
        return self.db.query(Menu).filter(Menu.excluido != True).all()

    def get_all_ativo_with_pages(self) -> List[Menu]:
        return (
            self.db.query(Menu)
            .filter(Menu.excluido != True)
            .options(joinedload(Menu.page))
            .all()
        )

    def get_all_ativo(self) -> List[Menu]:
        return self.db.query(Menu).filter(Menu.ativo == True).all()

    def get_all_excluido(self) -> List[Menu]:
        return self.db.query(Menu).filter(Menu.excluido == True).all()

    def remove_obj(self, obj: Menu) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self.db.query(Menu).filter(Menu.id == id).first()
            if obj:
                obj.ativo = False
                obj.excluido = True
                self.db.add(obj)
                self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False
