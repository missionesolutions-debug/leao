from pydantic import BaseModel
from typing import Optional

class SectionTranslationBase(BaseModel):
    Id: int
    SectionId: int

class SectionTranslationCreate(SectionTranslationBase):
    pass # Add specific fields for creation if needed

class SectionTranslation(SectionTranslationBase):
    Id: int

    class Config:
        from_attributes = True
