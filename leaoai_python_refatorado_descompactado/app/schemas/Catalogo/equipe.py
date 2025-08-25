from pydantic import BaseModel
from typing import Optional

class EquipeBase(BaseModel):
    Id: int

class EquipeCreate(EquipeBase):
    pass # Add specific fields for creation if needed

class Equipe(EquipeBase):
    Id: int

    class Config:
        from_attributes = True
