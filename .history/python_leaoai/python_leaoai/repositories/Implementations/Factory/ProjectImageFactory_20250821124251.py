# repositories/project_image_factory.py
from typing import List, Optional
from sqlalchemy.orm import Session
from models.factory import ProjectImage

class ProjectImageFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[ProjectImage]:
        return self.db.query(ProjectImage).filter(ProjectImage.id == id).first()

    def save_obj(self, obj: ProjectImage) -> ProjectImage:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: ProjectImage) -> ProjectImage:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[ProjectImage]:
        return self.db.query(ProjectImage).all()

    def get_list(self) -> List[ProjectImage]:
        return self.db.query(ProjectImage).all()

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
    def get_images_by_project(self, project_id: int) -> List[ProjectImage]:
        return self.db.query(ProjectImage).filter(ProjectImage.project_id == project_id).all()

    def get_images_by_project_block(self, project_block_id: int) -> List[ProjectImage]:
        return self.db.query(ProjectImage).filter(ProjectImage.project_block_id == project_block_id).all()
