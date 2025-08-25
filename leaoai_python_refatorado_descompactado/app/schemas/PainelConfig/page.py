from pydantic import BaseModel
from typing import Optional

class PageBase(BaseModel):
    Id: int

class PageCreate(PageBase):
    pass # Add specific fields for creation if needed

class Page(PageBase):
    Id: int

    class Config:
        from_attributes = True
