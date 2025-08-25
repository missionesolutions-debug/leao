from pydantic import BaseModel
from typing import Optional

class ProjectBlockBase(BaseModel):
    Id: int
    ProjectGroupId: int
    Name: str
    MinImagesAmount: int
    MaxImagesAmount: int
    ObservationsEnabled: bool
    Instructions: Optional[str] = None
    Position: int
    Code: str
    BoxTypeActive: bool
    CardBoardTypeActive: bool
    ClosureTypeActive: bool
    VersionActive: bool
    BlockForConclude: bool
    ActionText: Optional[str] = None
    ImagesLabel: Optional[str] = None
    IsCompleted: bool
    BoxType: Optional[str] = None
    CardBoardType: Optional[str] = None
    ClosureType: Optional[str] = None
    CodigoBarraActive: bool
    CodigoBarraInfo: Optional[str] = None
    Observation: Optional[str] = None
    Version: Optional[str] = None
    VersionInfo: Optional[str] = None

class ProjectBlockCreate(ProjectBlockBase):
    pass # Add specific fields for creation if needed

class ProjectBlock(ProjectBlockBase):
    Id: int

    class Config:
        from_attributes = True
