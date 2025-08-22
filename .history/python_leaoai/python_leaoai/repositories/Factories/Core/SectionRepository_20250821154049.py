from sqlalchemy.orm import Session, joinedload
from sqlalchemy.future import select
from sqlalchemy.exc import SQLAlchemyError
from typing import List, Optional
import asyncio

from data.database import get_db
from framework.data.models.core import Section, SectionTranslation, Metadata


class SectionRepository:
    def __init__(self, db: Session):
        self.db = db

    # -----------------------------
    # CRUD
    # -----------------------------
    def get_obj(self, id: int) -> Optional[Section]:
        return self.db.query(Section).filter(Section.id == id).first()

    async def get_obj_async(self, id: int) -> Optional[Section]:
        return await asyncio.to_thread(self.get_obj, id)

    def get_obj_with_translation(self, id: int) -> Optional[Section]:
        return (
            self.db.query(Section)
            .options(joinedload(Section.section_translations))
            .filter(Section.id == id)
            .first()
        )

    def save_obj(self, obj: Section) -> Section:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Section) -> Section:
        self.db.merge(obj)
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
            if obj:
                self.db.delete(obj)
                self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def get_obj_by_reference(self, reference: str) -> Optional[Section]:
        return self.db.query(Section).filter(Section.ref == reference).first()

    def get_objs_by_page_ref(self, page: str) -> List[Section]:
        sections = (
            self.db.query(Section)
            .filter((Section.ref.contains(page)) | (Section.ref.contains("global")))
            .all()
        )

        for section in sections:
            section.section_translations = (
                self.db.query(SectionTranslation)
                .filter(SectionTranslation.section_id == section.id)
                .all()
            )
            section.images = (
                self.db.query(Metadata)
                .filter(Metadata.section_id == section.id)
                .all()
            )

        return sections
