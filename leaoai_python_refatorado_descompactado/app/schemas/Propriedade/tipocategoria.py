from pydantic import BaseModel
from typing import Optional

class TipoCategoriaBase(BaseModel):
    Id: int

class TipoCategoriaCreate(TipoCategoriaBase):
    pass # Add specific fields for creation if needed

class TipoCategoria(TipoCategoriaBase):
    Id: int

    class Config:
        from_attributes = True
