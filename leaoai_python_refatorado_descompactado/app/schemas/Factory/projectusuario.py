from pydantic import BaseModel
from typing import Optional

class ProjectUsuarioBase(BaseModel):
    Id: int
    ProjectId: int
    UsuarioId: int

class ProjectUsuarioCreate(ProjectUsuarioBase):
    pass # Add specific fields for creation if needed

class ProjectUsuario(ProjectUsuarioBase):
    Id: int

    class Config:
        from_attributes = True
