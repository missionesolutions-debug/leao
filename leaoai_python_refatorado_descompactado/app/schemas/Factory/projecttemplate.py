from pydantic import BaseModel
from typing import Optional
from datetime import datetime

class ProjectTemplateBase(BaseModel):
    Id: int
    Name: str
    TemplateJson: str
    Ativo: bool
    Excluido: bool
    CreatedAt: datetime

class ProjectTemplateCreate(ProjectTemplateBase):
    pass # Add specific fields for creation if needed

class ProjectTemplate(ProjectTemplateBase):
    Id: int

    class Config:
        from_attributes = True
