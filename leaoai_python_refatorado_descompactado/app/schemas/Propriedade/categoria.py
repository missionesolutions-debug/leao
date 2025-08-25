from pydantic import BaseModel
from typing import Optional

class CategoriaBase(BaseModel):
    Id: int
    TipoCategoriaId: int

class CategoriaCreate(CategoriaBase):
    pass # Add specific fields for creation if needed

class Categoria(CategoriaBase):
    Id: int

    class Config:
        from_attributes = True
