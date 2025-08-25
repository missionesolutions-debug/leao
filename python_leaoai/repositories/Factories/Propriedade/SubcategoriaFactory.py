from sqlalchemy.orm import Session, joinedload
from typing import List, Optional
from datetime import datetime
from data.models.propriedade.sub_categoria import SubCategoria
from data.application_db_context import ApplicationDbContext


class SubCategoriaFactory:
    def __init__(self, context: ApplicationDbContext):
        self._context = context

    # ---------------------------
    # CRUD
    # ---------------------------
    def get_obj(self, id: int) -> Optional[SubCategoria]:
        return self._context.session.query(SubCategoria).filter(SubCategoria.id == id).first()

    def save_obj(self, obj: SubCategoria) -> SubCategoria:
        self._context.session.add(obj)
        self._context.session.commit()
        return obj

    def update_obj(self, obj: SubCategoria) -> SubCategoria:
        self._context.session.merge(obj)
        self._context.session.commit()
        return obj

    def get_all(self) -> List[SubCategoria]:
        return self._context.session.query(SubCategoria).all()

    def get_all_ativo(self) -> List[SubCategoria]:
        return (
            self._context.session.query(SubCategoria)
            .options(joinedload(SubCategoria.categoria), joinedload(SubCategoria.tipo_categoria))
            .filter(SubCategoria.ativo == True)
            .all()
        )

    def get_all_excluido(self) -> List[SubCategoria]:
        return self._context.session.query(SubCategoria).filter(SubCategoria.excluido == True).all()

    def remove_obj(self, obj: SubCategoria) -> bool:
        try:
            self._context.session.delete(obj)
            self._context.session.commit()
            return True
        except Exception:
            self._context.session.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self._context.session.query(SubCategoria).filter(SubCategoria.id == id).first()
            if obj:
                obj.ativo = False
                obj.excluido = True
                self._context.session.merge(obj)
                self._context.session.commit()
                return True
            return False
        except Exception:
            self._context.session.rollback()
            return False

    # ---------------------------
    # Custom
    # ---------------------------
    def get_obj_by_url(self, url: str) -> Optional[SubCategoria]:
        return (
            self._context.session.query(SubCategoria)
            .filter(SubCategoria.ativo == True, SubCategoria.url == url)
            .first()
        )
