# repositories/project_block_factory.py
from sqlalchemy.orm import Session
from typing import List, Optional
from models.factory import ProjectBlock

class ProjectBlockFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD ---
    def get_obj(self, id: int) -> Optional[ProjectBlock]:
        return self.db.query(ProjectBlock).filter(ProjectBlock.id == id).first()

    def save_obj(self, obj: ProjectBlock) -> ProjectBlock:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectBlock) -> ProjectBlock:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectBlock]:
        return self.db.query(ProjectBlock).all()

    def get_list(self) -> List[ProjectBlock]:
        return self.db.query(ProjectBlock).filter(ProjectBlock.excluido == False).all()

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

    # --- Custom Queries ---
    def get_blocks_by_project(self, project_id: int) -> List[ProjectBlock]:
        return self.db.query(ProjectBlock)\
                      .filter(ProjectBlock.project_id == project_id, ProjectBlock.excluido == False)\
                      .all()

    def get_blocks_by_project_phase(self, project_phase_id: int) -> List[ProjectBlock]:
        return self.db.query(ProjectBlock)\
                      .filter(ProjectBlock.project_phase_id == project_phase_id, ProjectBlock.excluido == False)\
                      .all()

    def get_blocks_for_conclude(self) -> List[ProjectBlock]:
        return self.db.query(ProjectBlock)\
                      .filter(ProjectBlock.block_for_conclude == True, ProjectBlock.excluido == False)\
                      .all()

    def get_block_by_code(self, code: str) -> Optional[ProjectBlock]:
        return self.db.query(ProjectBlock)\
                      .filter(ProjectBlock.code == code, ProjectBlock.excluido == False)\
                      .first()

    def get_blocks_by_user(self, usuario_id: int) -> List[ProjectBlock]:
        return self.db.query(ProjectBlock)\
                      .filter(ProjectBlock.usuario_id == usuario_id, ProjectBlock.excluido == False)\
                      .all()
