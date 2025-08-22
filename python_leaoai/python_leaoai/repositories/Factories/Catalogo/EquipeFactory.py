from typing import List, Optional
from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from models.catalogo import Equipe  # Importe seu modelo aqui

class EquipeFactory:
    def __init__(self, db: Session):
        self.db = db

    # -------------------------
    # CRUD Síncrono
    # -------------------------
    def get_obj(self, id: int) -> Optional[Equipe]:
        return self.db.query(Equipe).filter(Equipe.id == id).first()

    def save_obj(self, obj: Equipe) -> Equipe:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Equipe) -> Equipe:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Equipe]:
        return self.db.query(Equipe).all()

    def get_list(self) -> List[Equipe]:
        return self.db.query(Equipe).filter(Equipe.excluido != True).all()

    def get_all_ativo(self) -> List[Equipe]:
        return self.db.query(Equipe).filter(Equipe.ativo == True).all()

    def get_all_excluido(self) -> List[Equipe]:
        return self.db.query(Equipe).filter(Equipe.excluido == True).all()

    def remove_obj(self, obj: Equipe) -> bool:
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

    def get_by_id(self, equipe_id: int) -> Equipe:
        equipe = self.get_obj(equipe_id)
        if not equipe:
            raise Exception(f"Equipe com ID {equipe_id} não encontrada.")
        return equipe

    # -------------------------
    # Métodos específicos para imagens
    # -------------------------
    def get_all_to_blob(self) -> List[Equipe]:
        return self.db.query(Equipe).filter(
            ((Equipe.imagem != None) & (Equipe.imagem != "") & (~Equipe.imagem.contains("http"))) |
            ((Equipe.thumbnail != None) & (Equipe.thumbnail != "") & (~Equipe.thumbnail.contains("http")))
        ).all()

    def get_all_to_cdn(self) -> List[Equipe]:
        return self.db.query(Equipe).filter(
            ((Equipe.imagem != None) & (Equipe.imagem != "") & (Equipe.imagem.contains("http")) & (~Equipe.imagem.startswith("https://cdn.codiehost.com.br/"))) |
            ((Equipe.thumbnail != None) & (Equipe.thumbnail != "") & (Equipe.thumbnail.contains("http")) & (~Equipe.thumbnail.startswith("https://cdn.codiehost.com.br/")))
        ).all()

    # -------------------------
    # Async update (exemplo)
    # -------------------------
    async def update_obj_async(self, obj: Equipe, async_db):
        async_db.add(obj)
        await async_db.commit()
        await async_db.refresh(obj)
        return obj
