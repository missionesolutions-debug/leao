from pydantic import BaseModel
from typing import Optional

class ProjectPhaseBase(BaseModel):
    Id: int
    ProjectItemId: int
    UsuarioId: Optional[int] = None
    Name: str
    IsApproved: bool
    IsTaken: bool
    Position: int

class ProjectPhaseCreate(ProjectPhaseBase):
    pass # Add specific fields for creation if needed

class ProjectPhase(ProjectPhaseBase):
    Id: int

    class Config:
        from_attributes = True
