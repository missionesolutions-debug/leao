from typing import List, Optional
from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from Data.models.core import Imagem  # Ajuste o import conforme sua estrutura

class ImagemFactory:
    def __init__(self, db: Session):
        self.db = db

    # ----------------------------
    # CRUD
    # ----------------------------
    def get_obj(self, id: int) -> Optional[Imagem]:
        return self.db.query(Imagem).filter(Imagem.id == id).first()

    def save_obj(self, obj: Imagem) -> Imagem:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Imagem) -> Imagem:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Imagem]:
        return self.db.query(Imagem).all()

    def get_list(self) -> List[Imagem]:
        return self.db.query(Imagem).filter(Imagem.excluido != True).all()

    def get_all_ativo(self) -> List[Imagem]:
        return self.db.query(Imagem).filter(Imagem.ativo == True).all()

    def get_all_excluido(self) -> List[Imagem]:
        return self.db.query(Imagem).filter(Imagem.excluido == True).all()

    def remove_obj(self, obj: Imagem) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self.get_obj(id)
            if not obj:
                return False

            obj.ativo = False
            obj.excluido = True
            self.db.add(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    # ----------------------------
    # Custom Queries
    # ----------------------------
    def get_imagens_without_table_action_and_table_id(self) -> List[Imagem]:
        return self.db.query(Imagem).filter(
            Imagem.table_action == None,
            Imagem.table_id == 0,
            Imagem.excluido != True
        ).all()

    def get_all_to_blob(self) -> List[Imagem]:
        return self.db.query(Imagem).filter(Imagem.url == None).all()

    def get_all_to_cdn(self) -> List[Imagem]:
        return self.db.query(Imagem).filter(
            Imagem.url != None,
            Imagem.url.startswith("https://cdn.codiehost.com.br/")
        ).all()

    def get_imagens_by_table_action_and_table_id(self, table_action: str, table_id: int) -> List[Imagem]:
        imagens = self.db.query(Imagem).filter(
            Imagem.table_action == table_action,
            Imagem.table_id == table_id,
            Imagem.excluido != True
        ).all()
        return imagens or []

    def get_imagens_by_table_action(self, table_action: str) -> List[Imagem]:
        imagens = self.db.query(Imagem).filter(
            Imagem.table_action == table_action,
            Imagem.excluido != True
        ).all()
        return imagens or []

    def get_imagens_by_table_id(self, table_id: int) -> List[Imagem]:
        imagens = self.db.query(Imagem).filter(
            Imagem.table_id == table_id,
            Imagem.excluido != True
        ).all()
        return imagens or []
