# repositories/project_group_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models.factory import ProjectGroup

class ProjectGroupFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[ProjectGroup]:
        return self.db.query(ProjectGroup).filter(ProjectGroup.id == id).first()

    def save_obj(self, obj: ProjectGroup) -> ProjectGroup:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectGroup) -> ProjectGroup:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectGroup]:
        return self.db.query(ProjectGroup).all()

    def get_list(self) -> List[ProjectGroup]:
        return self.db.query(ProjectGroup).filter(ProjectGroup.excluido == False).all()

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            obj.ativo = False
            obj.excluido = True
            self.db.commit()
            return True
        except:
            self.db.rollback()
            return False

    # --- Consultas customizadas ---
    def get_groups_by_project_phase(self, project_phase_id: int) -> List[ProjectGroup]:
        return self.db.query(ProjectGroup)\
            .filter(ProjectGroup.project_phase_id == project_phase_id, ProjectGroup.excluido == False)\
            .all()

    def get_groups_by_project(self, project_id: int) -> List[ProjectGroup]:
        return self.db.query(ProjectGroup)\
            .filter(ProjectGroup.project_id == project_id, ProjectGroup.excluido == False)\
            .all()
