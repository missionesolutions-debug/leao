from pydantic import BaseModel
from typing import Optional

class PaginaBase(BaseModel):
    Id: int

class PaginaCreate(PaginaBase):
    pass # Add specific fields for creation if needed

class Pagina(PaginaBase):
    Id: int

    class Config:
        from_attributes = True
