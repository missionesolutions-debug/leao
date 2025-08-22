from typing import List, Optional
from sqlalchemy.orm import Session, joinedload
from sqlalchemy.exc import SQLAlchemyError
from models.conteudo import Pagina  # Importe seu modelo aqui

class PaginaFactory:
    def __init__(self, db: Session):
        self.db = db

    # -------------------------
    # CRUD
    # -------------------------
    def get_obj(self, id: int) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.id == id).first()

    def get_obj_by_urlhome(self) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.url == "home").first()

    def get_obj_by_url_sobre(self) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.url == "quem-somos").first()

    def get_obj_by_url_espiritualidade(self) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.url == "inteligencia-comercial").first()

    def get_obj_by_url_mkt(self) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.url == "marketing").first()

    def get_obj_by_url_atividade(self) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.url == "para-empresas").first()

    def save_obj(self, obj: Pagina) -> Pagina:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Pagina) -> Pagina:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    async def update_obj_async(self, obj: Pagina, async_db):
        async_db.add(obj)
        await async_db.commit()
        await async_db.refresh(obj)
        return obj

    def get_all(self) -> List[Pagina]:
        return self.db.query(Pagina).all()

    def get_all_ativo(self) -> List[Pagina]:
        return self.db.query(Pagina).options(joinedload(Pagina.categoria)).filter(Pagina.ativo == True).all()

    def get_all_excluido(self) -> List[Pagina]:
        return self.db.query(Pagina).filter(Pagina.excluido == True).all()

    def remove_obj(self, obj: Pagina) -> bool:
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
    # Filtros e URLs
    # -------------------------
    def get_obj_by_url(self, url: str) -> Optional[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.url == url).first()

    def get_paginas_by_categoria_id(self, categoria_id: int) -> List[Pagina]:
        return self.db.query(Pagina).filter(Pagina.ativo == True, Pagina.categoria_id == categoria_id).all()

    # -------------------------
    # Métodos específicos para imagens
    # -------------------------
    def get_all_to_blob(self) -> List[Pagina]:
        return self.db.query(Pagina).filter(
            ((Pagina.imagem != None) & (Pagina.imagem != "") & (~Pagina.imagem.contains("http"))) |
            ((Pagina.thumbnail != None) & (Pagina.thumbnail != "") & (~Pagina.thumbnail.contains("http")))
        ).all()

    def get_all_to_cdn(self) -> List[Pagina]:
        return self.db.query(Pagina).filter(
            ((Pagina.imagem != None) & (Pagina.imagem != "") & (Pagina.imagem.contains("http")) & (~Pagina.imagem.startswith("https://cdn.codiehost.com.br/"))) |
            ((Pagina.thumbnail != None) & (Pagina.thumbnail != "") & (Pagina.thumbnail.contains("http")) & (~Pagina.thumbnail.startswith("https://cdn.codiehost.com.br/")))
        ).all()
