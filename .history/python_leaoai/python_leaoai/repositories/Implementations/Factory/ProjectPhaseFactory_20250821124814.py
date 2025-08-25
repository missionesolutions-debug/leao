# repositories/project_phase_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models.factory import ProjectPhase

class ProjectPhaseFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[ProjectPhase]:
        return self.db.query(ProjectPhase).filter(ProjectPhase.id == id).first()

    def save_obj(self, obj: ProjectPhase) -> ProjectPhase:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectPhase) -> ProjectPhase:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectPhase]:
        return self.db.query(ProjectPhase).all()

    def get_list(self) -> List[ProjectPhase]:
        return self.db.query(ProjectPhase).filter(ProjectPhase.excluido == False).all()

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
    def get_phases_by_project(self, project_id: int) -> List[ProjectPhase]:
        return self.db.query(ProjectPhase).filter(
            ProjectPhase.project_id == project_id,
            ProjectPhase.excluido == False
        ).all()

    def get_phases_by_project_item(self, project_item_id: int) -> List[ProjectPhase]:
        return self.db.query(ProjectPhase).filter(
            ProjectPhase.project_item_id == project_item_id,
            ProjectPhase.excluido == False
        ).all()
