from pydantic import BaseModel
from typing import Optional

class AcabamentoBase(BaseModel):
    Id: int
    Nome: str
    Codigo: Optional[str] = None
    Imagem: Optional[str] = None
    ImagemFormulacao: Optional[str] = None
    Ativo: bool
    Excluido: bool
    ClientId: int

class AcabamentoCreate(AcabamentoBase):
    pass # Add specific fields for creation if needed

class Acabamento(AcabamentoBase):
    Id: int

    class Config:
        from_attributes = True
