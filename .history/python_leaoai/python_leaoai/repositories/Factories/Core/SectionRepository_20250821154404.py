from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from typing import List, Optional

from data.database import get_db  # função de sessão (injete ela no FastAPI com Depends se quiser)
from framework.data.models.core.section import Section


class SectionRepository:
    def __init__(self, db: Session):
        self.db = db

    # --------------------
    # CRUD
    # --------------------
    def get_obj(self, id: int) -> Optional[Section]:
        return self.db.query(Section).filter(Section.id == id).first()

    def save_obj(self, obj: Section) -> Section:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Section) -> Section:
        self.db.merge(obj)  # garante update mesmo vindo de fora da sessão
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Section]:
        return self.db.query(Section).all()

    def remove_obj(self, obj: Section) -> bool:
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
            self.db.delete(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False
