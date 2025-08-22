# repositories/project_measure_item_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models.factory import ProjectMeasureItem

class ProjectMeasureItemFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[ProjectMeasureItem]:
        return self.db.query(ProjectMeasureItem).filter(ProjectMeasureItem.id == id).first()

    def save_obj(self, obj: ProjectMeasureItem) -> ProjectMeasureItem:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectMeasureItem) -> ProjectMeasureItem:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectMeasureItem]:
        return self.db.query(ProjectMeasureItem).all()

    def get_list(self) -> List[ProjectMeasureItem]:
        # Como o C# não aplicava filtro de excluído, mantemos todos
        return self.db.query(ProjectMeasureItem).all()

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except:
            self.db.rollback()
            return False

    # --- Consultas customizadas ---
    def get_measure_items_by_project_block(self, project_block_id: int) -> List[ProjectMeasureItem]:
        return self.db.query(ProjectMeasureItem).filter(
            ProjectMeasureItem.project_block_id == project_block_id
        ).all()
