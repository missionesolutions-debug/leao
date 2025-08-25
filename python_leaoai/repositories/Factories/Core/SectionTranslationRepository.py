from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from typing import List, Optional

from data.database import get_db  # sua função para obter sessão
from framework.data.models.core.section_translation import SectionTranslation


class SectionTranslationRepository:
    def __init__(self, db: Session):
        self.db = db

    # --------------------
    # CRUD
    # --------------------
    def get_obj(self, id: int) -> Optional[SectionTranslation]:
        return self.db.query(SectionTranslation).filter(SectionTranslation.id == id).first()

    def save_obj(self, obj: SectionTranslation) -> SectionTranslation:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: SectionTranslation) -> SectionTranslation:
        self.db.merge(obj)  # garante atualização
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[SectionTranslation]:
        return self.db.query(SectionTranslation).all()

    def remove_obj(self, obj: SectionTranslation) -> bool:
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
