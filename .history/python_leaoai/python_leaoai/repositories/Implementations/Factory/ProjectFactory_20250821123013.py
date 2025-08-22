# repositories/project_factory.py
from sqlalchemy.orm import Session, selectinload
from typing import List, Optional
from models.factory import Project

class ProjectFactory:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD básico ---
    def get_obj(self, id: int) -> Optional[Project]:
        return self.db.query(Project).filter(Project.id == id, Project.excluido == False).first()

    def save_obj(self, obj: Project) -> Project:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: Project) -> Project:
        self.db.merge(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def get_all(self) -> List[Project]:
        return self.db.query(Project).all()

    def get_list(self) -> List[Project]:
        return self.db.query(Project).filter(Project.excluido == False).all()

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

    # --- Métodos full (com relações) ---
    def get_obj_full(self, id: int) -> Optional[Project]:
        return self.db.query(Project)\
            .options(
                selectinload(Project.project_status),
                selectinload(Project.client),
                selectinload(Project.project_usuarios),
                selectinload(Project.project_items)
                    .selectinload("project_phases")
                    .selectinload("project_groups")
                    .selectinload("project_blocks")
            )\
            .filter(Project.id == id, Project.excluido == False)\
            .first()

    def get_all_full(self) -> List[Project]:
        return self.db.query(Project)\
            .options(
                selectinload(Project.project_status),
                selectinload(Project.client),
                selectinload(Project.project_usuarios),
                selectinload(Project.project_items)
            )\
            .filter(Project.excluido == False)\
            .all()

    def get_all_full_mech(self) -> List[Project]:
        return self.db.query(Project)\
            .options(
                selectinload(Project.project_status),
                selectinload(Project.client),
                selectinload(Project.project_usuarios),
                selectinload(Project.project_items)
                    .selectinload("project_phases")
                    .selectinload("project_groups")
                    .selectinload("project_blocks")
                        .selectinload("project_measure_items"),
                selectinload(Project.project_items)
                    .selectinload("project_phases")
                    .selectinload("project_groups")
                    .selectinload("project_blocks")
                        .selectinload("project_images")
            )\
            .filter(Project.excluido == False)\
            .all()

    # --- Filtros customizados ---
    def get_projects_by_client(self, client_id: int) -> List[Project]:
        return self.db.query(Project)\
            .options(
                selectinload(Project.project_status),
                selectinload(Project.client),
                selectinload(Project.supplier),
                selectinload(Project.project_usuarios),
                selectinload(Project.project_items)
            )\
            .filter(Project.client_id == client_id, Project.excluido == False)\
            .all()

    def get_projects_by_supplier(self, supplier_id: int) -> List[Project]:
        return self.db.query(Project)\
            .options(
                selectinload(Project.project_status),
                selectinload(Project.client),
                selectinload(Project.supplier),
                selectinload(Project.project_usuarios),
                selectinload(Project.project_items)
            )\
            .filter(Project.supplier_id == supplier_id, Project.excluido == False)\
            .all()

    def get_projects_by_usuario(self, usuario_id: int) -> List[Project]:
        return self.db.query(Project)\
            .options(
                selectinload(Project.project_status),
                selectinload(Project.client),
                selectinload(Project.supplier),
                selectinload(Project.project_usuarios),
                selectinload(Project.project_items)
            )\
            .filter(Project.project_usuarios.any(usuario_id == usuario_id), Project.excluido == False)\
            .all()
