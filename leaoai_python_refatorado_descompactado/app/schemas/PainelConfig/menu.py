from pydantic import BaseModel
from typing import Optional

class MenuBase(BaseModel):
    Id: int

class MenuCreate(MenuBase):
    pass # Add specific fields for creation if needed

class Menu(MenuBase):
    Id: int

    class Config:
        from_attributes = True
