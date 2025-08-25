from pydantic import BaseModel
from typing import Optional

class SectionBase(BaseModel):
    Id: int

class SectionCreate(SectionBase):
    pass # Add specific fields for creation if needed

class Section(SectionBase):
    Id: int

    class Config:
        from_attributes = True
