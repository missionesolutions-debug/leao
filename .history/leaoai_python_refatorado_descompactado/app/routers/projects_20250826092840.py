# Add project root to sys.path to help with imports
import sys
import os # Import os to potentially get current path
# Ensure project root is in sys.path
# project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', '..'))
# if project_root not in sys.path:
#     sys.path.insert(0, project_root)

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session # REMOVE comment to enable DB access
from app.core.database import get_db # REMOVE comment to enable DB access
from typing import List, Optional # Import List and Optional for type hinting
from fastapi import File, UploadFile # REMOVE comment to enable file uploads


# Import services and schemas (REMOVE comments when DB and schemas are working)
from app.services.project_service import ProjectService
from app.schemas.Factory.project import Project as SchemaProject
from app.schemas.Factory.projectmeasureitem import ProjectMeasureItemCreate as SchemaProjectMeasureItemCreate
from app.schemas.Factory.projectimage import ProjectImageCreate as SchemaProjectImageCreate
from app.schemas.Factory.projectblock import ProjectBlockUpdate as SchemaProjectBlockUpdate
from app.schemas.Factory.projectimage import ProjectImage as SchemaProjectImage # For response model
from app.schemas.Factory.projectblock import ProjectBlock as SchemaProjectBlock # For response model


router = APIRouter(
    prefix="/projects", # Define a common prefix for these routes
    tags=["projects"] # Group these routes under a tag
)

# Define a simple test endpoint for projects
@router.get("/")
# def read_projects(db: Session = Depends(get_db)): # REMOVE comment to enable DB access
def read_projects(): # Modified function signature
    return {"message": "Projects router is working (DB dependency commented out)"}

# Add endpoints based on ProjectsApiController.cs

# Example placeholder for GetProjectsForCoordinator
# @router.get("/for-coordinator", response_model=List[SchemaProject]) # REMOVE comment when SchemaProject is ready
# # def get_projects_for_coordinator_endpoint(db: Session = Depends(get_db)): # REMOVE comment to enable DB access
# def get_projects_for_coordinator_endpoint(): # Modified function signature
#     # REMOVE comments below and uncomment the line above to enable DB access
#     # db: Session = Depends(get_db)
#     # Instantiate and use the ProjectService
#     # service = ProjectService(db) # Need ProjectService and its dependencies
#     # return service.get_projects_for_coordinator()
#     # If ProjectService needs repository, instantiate repository here and pass to service
#     # from app.repositories.project_repository import ProjectRepository
#     # repository = ProjectRepository(db)
#     # service = ProjectService(repository)
#     # return service.get_projects_for_coordinator()
#     pass # Placeholder


# Example placeholder for GetProjectsForTechnician
# @router.get("/for-technician/{user_id}", response_model=List[SchemaProject]) # REMOVE comment when SchemaProject is ready
# # def get_projects_for_technician_endpoint(user_id: int, db: Session = Depends(get_db)): # REMOVE comment to enable DB access
# def get_projects_for_technician_endpoint(user_id: int): # Modified function signature
#     # REMOVE comments below and uncomment the line above to enable DB access
#     # db: Session = Depends(get_db)
#     # Instantiate and use the ProjectService and Repository
#     # from app.repositories.project_repository import ProjectRepository
#     # repository = ProjectRepository(db)
#     # service = ProjectService(repository)
#     # return service.get_projects_for_technician(user_id)
#     pass # Placeholder


# Example placeholder for InsertMeasureItems
# @router.post("/insert-measure-items", response_model=List[SchemaProjectMeasureItem]) # REMOVE comment when SchemaProjectMeasureItem is ready
# # def insert_measure_items_endpoint(items_data: List[SchemaProjectMeasureItemCreate], db: Session = Depends(get_db)): # REMOVE comment to enable DB access
# def insert_measure_items_endpoint(items_data: List[SchemaProjectMeasureItemCreate]): # Modified function signature
#     # REMOVE comments below and uncomment the line above to enable DB access
#     # db: Session = Depends(get_db)
#     # Instantiate and use the ProjectService and Repository
#     # from app.repositories.project_repository import ProjectRepository
#     # repository = ProjectRepository(db)
#     # service = ProjectService(repository)
#     # return service.insert_measure_items(items_data)
#     pass # Placeholder


# Example placeholder for InsertProjectImages
# @router.post("/insert-project-images", response_model=List[SchemaProjectImage]) # REMOVE comment when SchemaProjectImage is ready
# # async def insert_project_images_endpoint( # REMOVE comment to enable DB access and file uploads
# #    project_image_id: Optional[int] = None,
# #    description: Optional[str] = None,
# #    enable_on_report: Optional[bool] = None,
# #    project_id: Optional[int] = None,
# #    project_block_id: Optional[int] = None,
# #    file: Optional[UploadFile] = File(None), # Requires from fastapi import File, UploadFile
# #    db: Session = Depends(get_db)
# # ):
# async def insert_project_images_endpoint( # Modified signature
#    project_image_id: Optional[int] = None,
#    description: Optional[str] = None,
#    enable_on_report: Optional[bool] = None,
#    project_id: Optional[int] = None,
#    project_block_id: Optional[int] = None # file parameter is commented out for now
#    # file: Optional[UploadFile] = File(None) # REMOVE comment to enable file uploads
# ):
#    # REMOVE comments below and uncomment the line above to enable DB access
#    # db: Session = Depends(get_db)
#    # Instantiate and use the ProjectService and Repository
#    # from app.repositories.project_repository import ProjectRepository
#    # repository = ProjectRepository(db)
#    # service = ProjectService(repository)
#    # return await service.insert_project_images(project_image_id, description, enable_on_report, project_id, project_block_id, file)
#    pass # Placeholder


# Example placeholder for EditProjectBlock
# @router.put("/edit-block/{block_code}", response_model=SchemaProjectBlock) # REMOVE comment when SchemaProjectBlock is ready
# # def edit_project_block_endpoint(block_code: str, update_data: dict, db: Session = Depends(get_db)): # REMOVE comment to enable DB access
# def edit_project_block_endpoint(block_code: str, update_data: dict): # Modified function signature
#     # REMOVE comments below and uncomment the line above to enable DB access
#     # db: Session = Depends(get_db)
#     # Instantiate and use the ProjectService and Repository
#     # from app.repositories.project_repository import ProjectRepository
#     # repository = ProjectRepository(db)
#     # service = ProjectService(repository)
#     # return service.edit_project_block(block_code, update_data)
#     pass # Placeholder
