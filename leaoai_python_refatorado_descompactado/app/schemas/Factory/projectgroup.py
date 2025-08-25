from pydantic import BaseModel
from typing import Optional

class ProjectGroupBase(BaseModel):
    Id: int
    ProjectPhaseId: int
    Name: str
    Position: int

class ProjectGroupCreate(ProjectGroupBase):
    pass # Add specific fields for creation if needed

class ProjectGroup(ProjectGroupBase):
    Id: int

    class Config:
        from_attributes = True
