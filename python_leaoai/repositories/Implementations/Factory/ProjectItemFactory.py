# repositories/project_item_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models.factory import ProjectItem

class ProjectItemFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[ProjectItem]:
        return self.db.query(ProjectItem).filter(ProjectItem.id == id).first()

    def save_obj(self, obj: ProjectItem) -> ProjectItem:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectItem) -> ProjectItem:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectItem]:
        return self.db.query(ProjectItem).all()

    def get_list(self) -> List[ProjectItem]:
        return self.db.query(ProjectItem).filter(ProjectItem.excluido == False).all()

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            obj.ativo = False
            obj.excluido = True
            self.db.merge(obj)
            self.db.commit()
            return True
        except:
            self.db.rollback()
            return False

    # --- Consultas customizadas ---
    def get_project_items_by_project(self, project_id: int) -> List[ProjectItem]:
        return self.db.query(ProjectItem).filter(
            ProjectItem.project_id == project_id,
            ProjectItem.excluido == False
        ).all()
