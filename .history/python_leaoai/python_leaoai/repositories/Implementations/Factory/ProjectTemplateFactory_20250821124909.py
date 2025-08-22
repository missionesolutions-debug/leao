# repositories/project_template_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models import ProjectTemplate

class ProjectTemplateFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[ProjectTemplate]:
        return self.db.query(ProjectTemplate).filter(
            ProjectTemplate.id == id,
            ProjectTemplate.excluido == False
        ).first()

    def save_obj(self, obj: ProjectTemplate) -> ProjectTemplate:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectTemplate) -> ProjectTemplate:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectTemplate]:
        return self.db.query(ProjectTemplate).all()

    def get_list(self) -> List[ProjectTemplate]:
        return self.db.query(ProjectTemplate).filter(ProjectTemplate.excluido == False).all()

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
