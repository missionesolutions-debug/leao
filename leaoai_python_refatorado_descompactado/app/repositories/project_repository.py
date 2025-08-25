
from sqlalchemy.orm import Session, joinedload
from typing import List, Optional

# Import SQLAlchemy models
from app.models.Factory.project import Project as DBProject
from app.models.Factory.projectusuario import ProjectUsuario as DBProjectUsuario
from app.models.Pessoa.usuario import Usuario as DBUsuario
# Import other related models if needed for joins
from app.models.Factory.client import Client as DBClient
from app.models.Factory.supplier import Supplier as DBSupplier
from app.models.Factory.projectstatus import ProjectStatus as DBProjectStatus
from app.models.Factory.projectitem import ProjectItem as DBProjectItem
from app.models.Factory.projectphase import ProjectPhase as DBProjectPhase
from app.models.Factory.projectgroup import ProjectGroup as DBProjectGroup
from app.models.Factory.projectblock import ProjectBlock as DBProjectBlock
from app.models.Factory.projectmeasureitem import ProjectMeasureItem as DBProjectMeasureItem
from app.models.Factory.projectimage import ProjectImage as DBProjectImage


class ProjectRepository:
    def __init__(self, db: Session):
        self.db = db

    # Example method to get all projects with related data (similar to C# GetAllFullMech)
    def get_all_full(self) -> List[DBProject]:
        return self.db.query(DBProject).options(
            joinedload(DBProject.Client),
            joinedload(DBProject.Supplier),
            joinedload(DBProject.ProjectStatus),
            joinedload(DBProject.ProjectUsuarios).joinedload(DBProjectUsuario.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectMeasureItems),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectImages)
        ).all()

    # Example method to get projects for a specific user
    def get_by_user_id_full(self, user_id: int) -> List[DBProject]:
         return self.db.query(DBProject).options(
            joinedload(DBProject.Client),
            joinedload(DBProject.Supplier),
            joinedload(DBProject.ProjectStatus),
            joinedload(DBProject.ProjectUsuarios).joinedload(DBProjectUsuario.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectMeasureItems),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectImages)
        ).join(DBProject.ProjectUsuarios).filter(DBProjectUsuario.UsuarioId == user_id).all()


    # Add methods for other operations as needed (get by id, create, update, delete)
    def get_by_id(self, project_id: int) -> Optional[DBProject]:
        return self.db.query(DBProject).filter(DBProject.Id == project_id).first()

    def create_project(self, project_data: dict) -> DBProject:
        db_project = DBProject(**project_data) # Assuming project_data is a dictionary matching model fields
        self.db.add(db_project)
        self.db.commit()
        self.db.refresh(db_project)
        return db_project

    # Add methods for measure items, images, blocks etc. if they are handled directly by repository
    # Or, these might be handled by separate repositories or directly in the service if logic is complex
    # Example placeholder for getting a block by code
    def get_block_by_code(self, block_code: str) -> Optional[DBProjectBlock]:
         return self.db.query(DBProjectBlock).filter(DBProjectBlock.Code == block_code).first()

    # Example placeholder for inserting measure items
    def bulk_insert_measure_items(self, items_data: List[dict]) -> List[DBProjectMeasureItem]:
         # This is a simplified bulk insert example
         db_items = [DBProjectMeasureItem(**item_data) for item_data in items_data]
         self.db.bulk_save_objects(db_items)
         self.db.commit()
         # Refreshing bulk inserted objects might require a separate query or session option
         return db_items # Note: these objects might not have IDs populated immediately

    # Example placeholder for getting/updating images
    def get_image_by_id(self, image_id: int) -> Optional[DBProjectImage]:
         return self.db.query(DBProjectImage).filter(DBProjectImage.Id == image_id).first()

    def create_image(self, image_data: dict) -> DBProjectImage:
         db_image = DBProjectImage(**image_data)
         self.db.add(db_image)
         self.db.commit()
         self.db.refresh(db_image)
         return db_image

