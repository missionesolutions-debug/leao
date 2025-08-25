# Ensure the project base directory is in sys.path to allow imports
python_project_base_dir = '/content/python_leaoai'
if python_project_base_dir not in sys.path:
    sys.path.append(python_project_base_dir)
import os
import sys
from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session
from database import get_db, Base, engine # Assuming these are defined in database.py
# Import your repositories and services
from repositories.placeholder_repository import PlaceholderRepository # Example repository
from services.project_service import ProjectService # Example service
from services.authentication_service import AuthenticationService # Example service
from api.projects_ import router as projects__router
from api.authentication_ import router as authentication__router
app = FastAPI()
app.include_router(projects__router)
app.include_router(authentication__router)
print('Main FastAPI application file created.')
if __name__ == '__main__':
    import uvicorn
    uvicorn.run(app, host='0.0.0.0', port=8000)
