
# Add project root to sys.path
import sys
import os
project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', '..'))
if project_root not in sys.path:
    sys.path.insert(0, project_root)

from fastapi import APIRouter, Depends # , HTTPException, status # Commented out dependencies for now (status not strictly needed here)
from sqlalchemy.orm import Session # REMOVE comment to enable DB access
from app.core.database import get_db # REMOVE comment
from typing import List, Optional # Import necessary types

# Import security dependency (REMOVE comment when app.core.security is fixed and uncommented)
from app.core.security import get_current_user # REMOVE comment


# Import schemas and services (REMOVE comments when ready)
from app.schemas.Catalogo.equipe import Equipe as SchemaEquipe # Example schema
from app.services.ApplicationServices.Catalogo.catalogo_service import CatalogoService # Assuming a CatalogoService exists


router = APIRouter(
    prefix="/catalogo", # Define a common prefix
    tags=["catalogo"] # Group under a tag
)

# Example placeholder for Get Equipe endpoint (based on Catalogo/Equipe model) (Requires authentication)
@router.get("/equipe")
def get_equipe(db: Session = Depends(get_db), current_user: any = Depends(get_current_user)): # REMOVE comment
    # REMOVE comments below and uncomment the line above to enable DB access
    db: Session = Depends(get_db)
    service = CatalogoService(db) # Need CatalogoService and its dependencies
    return service.get_equipe() # Assuming a method like get_equipe exists
    return {"message": "Get Equipe endpoint placeholder (DB/Security dependencies uncommented)"}

# Add other endpoints from CatalogoController.cs here (e.g., list items, get item by id)

@router.get("/items") # Example route for listing catalog items (Requires authentication)
# # def list_catalog_items(db: Session = Depends(get_db), current_user: any = Depends(get_current_user)): # REMOVE comment to enable DB access and security
def list_catalog_items(db: Session = Depends(get_db), current_user: any = Depends(get_current_user)): # REMOVE comment
#      # REMOVE comments below and uncomment the line above to enable DB access
    db: Session = Depends(get_db)
    service = CatalogoService(db)
    return service.list_items()
#      return [] # Placeholder


@router.get("/items/{item_id}") # Example route for getting a specific item (Requires authentication)
# def get_catalog_item(item_id: int, db: Session = Depends(get_db), current_user: any = Depends(get_current_user)): # REMOVE comment to enable DB access and security
def get_catalog_item(item_id: int, db: Session = Depends(get_db), current_user: any = Depends(get_current_user)): # REMOVE comment
     # REMOVE comments below and uncomment the line above to enable DB access
     db: Session = Depends(get_db)
     service = CatalogoService(db)
     item = service.get_item_by_id(item_id)
     if not item:
          raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Item not found")
     return item
     return {} # Placeholder


