from pydantic import BaseModel
from typing import Optional

class EmbalagemTypeBase(BaseModel):
    Id: int
    Nome: str
    Excluido: bool

class EmbalagemTypeCreate(EmbalagemTypeBase):
    pass # Add specific fields for creation if needed

class EmbalagemType(EmbalagemTypeBase):
    Id: int

    class Config:
        from_attributes = True
