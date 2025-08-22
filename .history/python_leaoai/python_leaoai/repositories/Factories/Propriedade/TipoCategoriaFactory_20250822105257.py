from sqlalchemy.orm import Session
from typing import List, Optional
from data.models.propriedade import TipoCategoria


class TipoCategoriaFactory:
    def __init__(self, db: Session):
        self.db = db

    # -------------------------
    # CRUD
    # -------------------------

    def get_obj(self, id: int) -> Optional[TipoCategoria]:
        return self.db.query(TipoCategoria).filter(TipoCategoria.id == id).first()

    def save_obj(self, obj: TipoCategoria) -> TipoCategoria:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: TipoCategoria) -> TipoCategoria:
        self.db.merge(obj)
        self.db.commit()
        return obj

    def get_all(self) -> List[TipoCategoria]:
        return self.db.query(TipoCategoria).all()

    def get_all_ativo(self) -> List[TipoCategoria]:
        return self.db.query(TipoCategoria).filter(TipoCategoria.ativo.is_(True)).all()

    def get_all_excluido(self) -> List[TipoCategoria]:
        return self.db.query(TipoCategoria).filter(TipoCategoria.excluido.is_(True)).all()

    def remove_obj(self, obj: TipoCategoria) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except Exception:
            self.db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self.get_obj(id)
            if obj is None:
                return False

            obj.ativo = False
            obj.excluido = True
            self.db.merge(obj)
            self.db.commit()
            return True
        except Exception:
            self.db.rollback()
            return False

    # -------------------------
    # Extra
    # -------------------------

    def get_obj_by_url(self, url: str) -> Optional[TipoCategoria]:
        return (
            self.db.query(TipoCategoria)
            .filter(TipoCategoria.ativo.is_(True), TipoCategoria.url == url)
            .first()
        )
