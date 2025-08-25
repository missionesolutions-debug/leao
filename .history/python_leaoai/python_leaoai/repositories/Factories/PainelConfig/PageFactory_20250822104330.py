from typing import List, Optional
from sqlalchemy.orm import Session, joinedload
from sqlalchemy.exc import SQLAlchemyError
from sqlalchemy.future import select
from sqlalchemy.ext.asyncio import AsyncSession

from data.models.painel_config import Page


class PageFactory:
    def __init__(self, db: Session | AsyncSession):
        self._db = db

    # ---------------------------
    # Crud
    # ---------------------------
    def get_obj(self, id: int) -> Optional[Page]:
        return self._db.query(Page).filter(Page.id == id).first()

    def save_obj(self, obj: Page) -> Page:
        self._db.add(obj)
        self._db.commit()
        self._db.refresh(obj)
        return obj

    def update_obj(self, obj: Page) -> Page:
        self._db.merge(obj)
        self._db.commit()
        self._db.refresh(obj)
        return obj

    def get_all(self) -> List[Page]:
        return self._db.query(Page).all()

    def get_list(self) -> List[Page]:
        return (
            self._db.query(Page)
            .filter(Page.excluido != True)
            .options(joinedload(Page.menu))  # Equivalente ao Include(a => a.Menu)
            .all()
        )

    def get_all_ativo(self) -> List[Page]:
        return self._db.query(Page).filter(Page.ativo == True).all()

    def get_all_excluido(self) -> List[Page]:
        return self._db.query(Page).filter(Page.excluido == True).all()

    def remove_obj(self, obj: Page) -> bool:
        try:
            self._db.delete(obj)
            self._db.commit()
            return True
        except SQLAlchemyError:
            self._db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self._db.query(Page).filter(Page.id == id).first()
            if obj:
                obj.ativo = False
                obj.excluido = True
                self._db.add(obj)
                self._db.commit()
                return True
            return False
        except SQLAlchemyError:
            self._db.rollback()
            return False

    def get_by_table_action(self, tab: str) -> Optional[Page]:
        return self._db.query(Page).filter(Page.table_action == tab).first()

    # ---------------------------
    # Métodos assíncronos (AsyncSession)
    # ---------------------------
    async def get_pages_for_sitemap_async(self) -> List[Page]:
        stmt = select(Page).where(Page.ativo == True, Page.is_site_url_active == True)
        result = await self._db.execute(stmt)
        return result.scalars().all()
