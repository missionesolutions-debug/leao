from pydantic import BaseModel
from typing import Optional

class CaducidadeBase(BaseModel):
    registro_marca: str
    marca_requerida: str
    classe_marca: str
    especificacoes: Optional[str] = None
    titular_marca: Optional[str] = None
    numero_processo: Optional[str] = None
    nome_cliente: Optional[str] = None
    marca_cliente: Optional[str] = None
    data_deposito: Optional[str] = None  # ou date, se preferir
    classe_marca_cliente: Optional[str] = None

class CaducidadeCreate(CaducidadeBase):
    pass

class CaducidadeUpdate(CaducidadeBase):
    pass

class CaducidadeResponse(BaseModel):
    id: int
    registro_marca: str
    marca_requerida: str
    classe_marca: str
    especificacoes: Optional[str] = None
    titular_marca: Optional[str] = None
    numero_processo: Optional[str] = None
    nome_cliente: Optional[str] = None
    marca_cliente: Optional[str] = None
    data_deposito: Optional[str] = None
    classe_marca_cliente: Optional[str] = None

class Caducidade(CaducidadeBase):
    id: int

    class Config:
        from_attributes = True