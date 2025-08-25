from sqlalchemy.orm import Session, joinedload
from fastapi import HTTPException
from typing import List, Optional

from data.database import SessionLocal
from data.models.propriedade.categoria import Categoria


class CategoriaRepository:
    def __init__(self, db: Session = None):
        self.db = db or SessionLocal()

    # --------------------
    # CRUD
    # --------------------
    def get_obj(self, id: int) -> Optional[Categoria]:
        return self.db.query(Categoria).filter(Categoria.id == id).first()

    def get_obj_by_id(self, id: int) -> Optional[Categoria]:
        obj = self.db.query(Categoria).filter(Categoria.id == id).first()
        return self.encapsulate_categoria(obj) if obj else None

    def get_all_category(self) -> List[Categoria]:
        categorias = self.db.query(Categoria).filter(Categoria.ativo.is_(True)).all()
        return [self.encapsulate_categoria(c) for c in categorias]

    def encapsulate_categoria(self, categoria: Categoria) -> Categoria:
        if not categoria:
            return None
        model = Categoria(
            id=categoria.id,
            titulo=categoria.titulo,
            url=categoria.url
        )
        return model

    def save_obj(self, obj: Categoria) -> Categoria:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Categoria) -> Categoria:
        if not obj:
            raise HTTPException(status_code=400, detail="Objeto inválido")
        self.db.merge(obj)
        self.db.commit()
        return obj

    def get_all(self) -> List[Categoria]:
        return self.db.query(Categoria).all()

    def get_all_ativo(self) -> List[Categoria]:
        return (
            self.db.query(Categoria)
            .filter(Categoria.ativo.is_(True))
            .options(joinedload(Categoria.tipo_categoria))
            .all()
        )

    def get_ativos(self) -> List[Categoria]:
        return self.db.query(Categoria).filter(Categoria.ativo.is_(True)).all()

    def get_all_excluido(self) -> List[Categoria]:
        return self.db.query(Categoria).filter(Categoria.excluido.is_(True)).all()

    def remove_obj(self, obj: Categoria) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except Exception:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self.db.query(Categoria).filter(Categoria.id == id).first()
            if not obj:
                return False
            obj.ativo = False
            obj.excluido = True
            self.db.merge(obj)
            self.db.commit()
            return True
        except Exception:
            self.db.rollback()
            return False

    # --------------------
    # EXTRA
    # --------------------
    def get_obj_by_url(self, url: str) -> Optional[Categoria]:
        return (
            self.db.query(Categoria)
            .filter(Categoria.ativo.is_(True), Categoria.url == url)
            .first()
        )

    def get_obj_by_service(self, url: str) -> Optional[Categoria]:
        return (
            self.db.query(Categoria)
            .filter(Categoria.ativo.is_(True), Categoria.url == url)
            .first()
        )

    def get_all_to_blob(self) -> List[Categoria]:
        return (
            self.db.query(Categoria)
            .filter(
                ((Categoria.imagem.isnot(None)) & (Categoria.imagem != "") & (~Categoria.imagem.contains("http")))
                | ((Categoria.thumbnail.isnot(None)) & (Categoria.thumbnail != "") & (~Categoria.thumbnail.contains("http")))
            )
            .all()
        )

    def get_all_to_cdn(self) -> List[Categoria]:
        return (
            self.db.query(Categoria)
            .filter(
                ((Categoria.imagem != None) & (Categoria.imagem.contains("http")) & (~Categoria.imagem.startswith("https://cdn.codiehost.com.br/")))
                | ((Categoria.thumbnail != None) & (Categoria.thumbnail.contains("http")) & (~Categoria.thumbnail.startswith("https://cdn.codiehost.com.br/")))
            )
            .all()
        )
