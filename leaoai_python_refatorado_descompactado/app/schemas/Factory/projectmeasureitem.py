from pydantic import BaseModel
from typing import Optional

class ProjectMeasureItemBase(BaseModel):
    Id: int
    ProjectBlockId: int
    Name: str
    Width: Optional[float] = None
    Height: Optional[float] = None
    Length: Optional[float] = None
    Weight: Optional[float] = None
    NameEditable: bool

class ProjectMeasureItemCreate(ProjectMeasureItemBase):
    pass # Add specific fields for creation if needed

class ProjectMeasureItem(ProjectMeasureItemBase):
    Id: int

    class Config:
        from_attributes = True
