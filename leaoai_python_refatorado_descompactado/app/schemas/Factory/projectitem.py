from pydantic import BaseModel
from typing import Optional

class ProjectItemBase(BaseModel):
    Id: int
    ProjectId: int
    Name: str
    Position: int

class ProjectItemCreate(ProjectItemBase):
    pass # Add specific fields for creation if needed

class ProjectItem(ProjectItemBase):
    Id: int

    class Config:
        from_attributes = True
