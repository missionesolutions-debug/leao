
import os
import sys

# Change directory to the project root to ensure 'app' is in the path for imports
project_root = '/content/leaoai_python_refatorado_descompactado'
os.chdir(project_root)
# Add the project root to sys.path just in case
if project_root not in sys.path:
    sys.path.insert(0, project_root)

from typing import List, Optional
from datetime import datetime, timezone # Import datetime and timezone

# Import SQLAlchemy models (assuming they are created)
from app.models.Factory.project import Project as DBProject
from app.models.Factory.client import Client as DBClient
from app.models.Factory.supplier import Supplier as DBSupplier
from app.models.Factory.projectstatus import ProjectStatus as DBProjectStatus
from app.models.Factory.projectusuario import ProjectUsuario as DBProjectUsuario
from app.models.Pessoa.usuario import Usuario as DBUsuario
from app.models.Factory.projectitem import ProjectItem as DBProjectItem
from app.models.Factory.projectphase import ProjectPhase as DBProjectPhase
from app.models.Factory.projectgroup import ProjectGroup as DBProjectGroup
from app.models.Factory.projectblock import ProjectBlock as DBProjectBlock
from app.models.Factory.projectmeasureitem import ProjectMeasureItem as DBProjectMeasureItem
from app.models.Factory.projectimage import ProjectImage as DBProjectImage


# Import Pydantic schemas (assuming they are created)
from app.schemas.Factory.project import Project as SchemaProject
from app.schemas.Factory.client import Client as SchemaClient
from app.schemas.Factory.supplier import Supplier as SchemaSupplier
from app.schemas.Factory.projectstatus import ProjectStatus as SchemaProjectStatus
from app.schemas.Pessoa.usuario import User as SchemaUser # Assuming User schema for assigned users
from app.schemas.Factory.projectitem import ProjectItem as SchemaProjectItem
from app.schemas.Factory.projectphase import ProjectPhase as SchemaProjectPhase
from app.schemas.Factory.projectgroup import ProjectGroup as SchemaProjectGroup
from app.schemas.Factory.projectblock import ProjectBlock as SchemaProjectBlock
from app.schemas.Factory.projectmeasureitem import ProjectMeasureItem as SchemaProjectMeasureItem
from app.schemas.Factory.projectimage import ProjectImage as SchemaProjectImage
# Assuming schemas for nested objects like ProjectItem, ProjectPhase, etc. are also created
# and correctly linked in the main Project schema using orm_mode/from_attributes

from sqlalchemy.orm import Session, joinedload
# Need to import the BlobService if used for image uploads
from app.infrastructure.blob_service import BlobService


class ProjectService:
    def __init__(self, db: Session):
        self.db = db
        # self.blob_service = BlobService() # Inject or instantiate BlobService if needed

    # Translate GetProjectsForCoordinator logic
    def get_projects_for_coordinator(self) -> List[SchemaProject]:
        # Equivalent to _projectFactory.GetAllFullMech().Where(p => !p.IsTemplate && !p.Excluido)
        # Need to implement the "GetAllFullMech" equivalent using SQLAlchemy joins
        # This attempts to eagerly load related data as seen in the C# GetProjects helper
        projects_query = self.db.query(DBProject).options(
            joinedload(DBProject.Client),
            joinedload(DBProject.Supplier),
            joinedload(DBProject.ProjectStatus),
            joinedload(DBProject.ProjectUsuarios).joinedload(DBProjectUsuario.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectMeasureItems),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectImages)
        ).filter(
            DBProject.IsTemplate == False,
            DBProject.Excluido == False
        )

        projects = projects_query.all()

        # Map SQLAlchemy objects to Pydantic schemas
        # Pydantic's from_orm=True (or from_attributes=True in v2) handles mapping
        # Ensure relationships are correctly defined in SQLAlchemy models for this to work
        return [
            SchemaProject.from_orm(p) for p in projects
        ]


    # Translate GetProjectsForTechnician logic
    def get_projects_for_technician(self, user_id: int) -> List[SchemaProject]:
        # Equivalent to _projectFactory.GetAllFullMech().Where(p => !p.IsTemplate && !p.Excluido && p.ProjectUsuarios.Any(pu => pu.UsuarioId == usuarioId))
        # This attempts to eagerly load related data and filter by assigned user
        projects_query = self.db.query(DBProject).options(
            joinedload(DBProject.Client),
            joinedload(DBProject.Supplier),
            joinedload(DBProject.ProjectStatus),
            joinedload(DBProject.ProjectUsuarios).joinedload(DBProjectUsuario.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.Usuario),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectMeasureItems),
            joinedload(DBProject.ProjectItems).joinedload(DBProjectItem.ProjectPhases).joinedload(DBProjectPhase.ProjectGroups).joinedload(DBProjectGroup.ProjectBlocks).joinedload(DBProjectBlock.ProjectImages)
        ).join(DBProject.ProjectUsuarios).filter( # Join with ProjectUsuarios table
            DBProject.IsTemplate == False,
            DBProject.Excluido == False,
            DBProjectUsuario.UsuarioId == user_id # Filter by the user_id in the join table
        )

        projects = projects_query.all()

        # Map SQLAlchemy objects to Pydantic schemas
        return [
            SchemaProject.from_orm(p) for p in projects
        ]

    # Translate InsertMeasureItems logic
    def insert_measure_items(self, items_data: List[SchemaProjectMeasureItem]) -> List[DBProjectMeasureItem]:
        # Logic to insert measure items
        inserted_items = []
        for item_data in items_data:
            # Create SQLAlchemy model instance from Pydantic schema data
            db_item = DBProjectMeasureItem(
                ProjectBlockId=item_data.ProjectBlockId,
                Name=item_data.Name,
                Width=item_data.Width,
                Height=item_data.Height,
                Length=item_data.Length,
                Weight=item_data.Weight,
                NameEditable=item_data.NameEditable
            )
            self.db.add(db_item)
            inserted_items.append(db_item)

        self.db.commit()
        # Refresh objects to get generated IDs if needed
        for item in inserted_items:
             self.db.refresh(item)

        return inserted_items

    # Translate InsertProjectImages logic
    async def insert_project_images(self, project_image_id: Optional[int], description: Optional[str], enable_on_report: Optional[bool], project_id: Optional[int], project_block_id: Optional[int], file): # file type can be UploadFile or bytes
         # Logic to handle image upload and saving image info
         # This involves calling the BlobService and saving metadata to the database
         # Need to import and use the actual BlobService

         # Check if updating an existing image
         if project_image_id is not None and project_image_id > 0:
             db_image = self.db.query(DBProjectImage).filter(DBProjectImage.Id == project_image_id).first()
             if db_image:
                 # Update fields if provided
                 if description is not None:
                     db_image.description = description
                 if enable_on_report is not None:
                     db_image.enableOnReport = enable_on_report # Assuming column name is enableOnReport
                 self.db.commit()
                 self.db.refresh(db_image)
                 return [SchemaProjectImage.from_orm(db_image)] # Return a list for consistency
             else:
                 # Image not found - return empty list or raise error
                 return []

         # If not updating, insert a new image
         if file is None:
             # No file provided for insertion - return empty list or raise error
             return []

         # --- Placeholder for actual Blob Service upload ---
         # You need to implement or import a BlobService equivalent
         # from app.infrastructure.blob_service import BlobService
         # blob_service = BlobService() # Instantiate the service

         # For now, simulate upload and use a dummy URL
         # In a real implementation, handle the file object (e.g., FastAPI UploadFile)
         # and pass it to the blob_service.upload_file method.
         # The upload method should return the URL of the uploaded file.
         print("Simulating file upload...")
         # uploaded_url = await blob_service.upload_file(file) # Call the actual upload method
         uploaded_url = "http://dummy-url.com/uploaded_image_placeholder.jpg" # Dummy URL


         # Create and save the new image metadata
         image = DBProjectImage(
             ProjectId=project_id,
             ProjectBlockId=project_block_id,
             UrlSource=uploaded_url,
             DataCadastro=datetime.now(timezone.utc), # Use timezone-aware datetime
             description=description if description is not None else "",
             enableOnReport=enable_on_report if enable_on_report is not None else False
         )

         self.db.add(image)
         self.db.commit()
         self.db.refresh(image)

         # Return the newly created image as a Pydantic schema in a list
         return [SchemaProjectImage.from_orm(image)]


    # Translate EditProjectBlock logic
    def edit_project_block(self, block_code: str, update_data: dict) -> Optional[SchemaProjectBlock]:
        # Logic to edit a project block
        # Find the block by Code (GUID) and update its fields
        db_block = self.db.query(DBProjectBlock).filter(
            DBProjectBlock.Code == block_code,
            DBProjectBlock.Excluido == False # Assuming Excluido is a boolean column
        ).first()

        if db_block is None:
            return None # Block not found

        # Update fields based on update_data dictionary
        # Need to be careful with validation and allowed fields
        for field, value in update_data.items():
            if hasattr(db_block, field):
                setattr(db_block, field, value)
            else:
                print(f"Warning: Field '{field}' not found in DBProjectBlock model. Skipping update for this field.")

        self.db.commit()
        self.db.refresh(db_block)

        # Return the updated block as a Pydantic schema
        return SchemaProjectBlock.from_orm(db_block)

