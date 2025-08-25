from typing import List, Optional
from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from models.core import Arquivo  # Importe seu modelo Arquivo

class ArquivoFactory:
    def __init__(self, db: Session):
        self.db = db

    # -------------------------
    # CRUD
    # -------------------------
    def get_obj(self, id: int) -> Optional[Arquivo]:
        return self.db.query(Arquivo).filter(Arquivo.id == id).first()

    def save_obj(self, obj: Arquivo) -> Arquivo:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Arquivo) -> Arquivo:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Arquivo]:
        return self.db.query(Arquivo).all()

    def get_list(self) -> List[Arquivo]:
        return self.db.query(Arquivo).filter(Arquivo.excluido != True).all()

    def get_all_ativo(self) -> List[Arquivo]:
        return self.db.query(Arquivo).filter(Arquivo.ativo == True).all()

    def get_all_excluido(self) -> List[Arquivo]:
        return self.db.query(Arquivo).filter(Arquivo.excluido == True).all()

    def remove_obj(self, obj: Arquivo) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            obj.ativo = False
            obj.excluido = True
            self.db.add(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    # -------------------------
    # Métodos customizados
    # -------------------------
    def get_arquivos_without_table_action_and_table_id(self) -> List[Arquivo]:
        return self.db.query(Arquivo).filter(
            Arquivo.table_action == None,
            Arquivo.table_id == 0,
            Arquivo.excluido != True
        ).all()

    def get_arquivos_by_table_action_and_table_id(self, table_action: str, table_id: int) -> List[Arquivo]:
        arquivos = self.db.query(Arquivo).filter(
            Arquivo.table_action == table_action,
            Arquivo.table_id == table_id,
            Arquivo.excluido != True
        ).all()
        return arquivos if arquivos else []
