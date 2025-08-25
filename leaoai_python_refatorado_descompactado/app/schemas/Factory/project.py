from pydantic import BaseModel
from typing import Optional
from datetime import datetime

class ProjectBase(BaseModel):
    Id: int
    Name: str
    CreatedAt: datetime
    FinishedAt: Optional[datetime] = None
    DatePrevisioned: Optional[datetime] = None
    Observations: Optional[str] = None
    PdfReportTotal: Optional[str] = None
    Guid: str
    IsTemplate: bool
    Excluido: bool
    ClientId: Optional[int] = None
    SupplierId: Optional[int] = None
    ProjectStatusId: Optional[int] = None

class ProjectCreate(ProjectBase):
    pass # Add specific fields for creation if needed

class Project(ProjectBase):
    Id: int

    class Config:
        from_attributes = True
