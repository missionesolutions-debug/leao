from pydantic import BaseModel
from typing import Optional

class PageSectionBase(BaseModel):
    Id: int
    PaginaId: int

class PageSectionCreate(PageSectionBase):
    pass # Add specific fields for creation if needed

class PageSection(PageSectionBase):
    Id: int

    class Config:
        from_attributes = True
