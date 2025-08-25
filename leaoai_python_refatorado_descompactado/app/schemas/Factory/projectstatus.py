from pydantic import BaseModel
from typing import Optional

class ProjectStatusBase(BaseModel):
    Id: int
    Nome: str
    Excluido: bool

class ProjectStatusCreate(ProjectStatusBase):
    pass # Add specific fields for creation if needed

class ProjectStatus(ProjectStatusBase):
    Id: int

    class Config:
        from_attributes = True
