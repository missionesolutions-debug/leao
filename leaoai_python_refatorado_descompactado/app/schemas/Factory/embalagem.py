from pydantic import BaseModel
from typing import Optional

class EmbalagemBase(BaseModel):
    Id: int
    EmbalagemTypeId: int
    Nome: str
    Imagem: Optional[str] = None
    ImagemFormulacao: Optional[str] = None
    Codigo: Optional[str] = None
    Ativo: bool
    Excluido: bool

class EmbalagemCreate(EmbalagemBase):
    pass # Add specific fields for creation if needed

class Embalagem(EmbalagemBase):
    Id: int

    class Config:
        from_attributes = True
