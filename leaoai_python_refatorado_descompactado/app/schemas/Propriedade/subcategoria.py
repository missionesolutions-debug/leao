from pydantic import BaseModel
from typing import Optional

class SubCategoriaBase(BaseModel):
    Id: int
    CategoriaId: int

class SubCategoriaCreate(SubCategoriaBase):
    pass # Add specific fields for creation if needed

class SubCategoria(SubCategoriaBase):
    Id: int

    class Config:
        from_attributes = True
