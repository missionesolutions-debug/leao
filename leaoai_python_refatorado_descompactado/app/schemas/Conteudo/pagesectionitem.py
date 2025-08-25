from pydantic import BaseModel
from typing import Optional

class PageSectionItemBase(BaseModel):
    Id: int
    PageSectionId: int

class PageSectionItemCreate(PageSectionItemBase):
    pass # Add specific fields for creation if needed

class PageSectionItem(PageSectionItemBase):
    Id: int

    class Config:
        from_attributes = True
