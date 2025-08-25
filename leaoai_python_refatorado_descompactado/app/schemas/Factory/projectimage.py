from pydantic import BaseModel
from typing import Optional
from datetime import datetime

class ProjectImageBase(BaseModel):
    Id: int
    ProjectId: Optional[int] = None
    ProjectBlockId: Optional[int] = None
    UrlSource: str
    DataCadastro: datetime
    description: Optional[str] = None
    enableOnReport: bool

class ProjectImageCreate(ProjectImageBase):
    pass # Add specific fields for creation if needed

class ProjectImage(ProjectImageBase):
    Id: int

    class Config:
        from_attributes = True
