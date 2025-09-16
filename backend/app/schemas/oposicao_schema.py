from pydantic import BaseModel
from typing import Optional

class OposicaoBase(BaseModel):
    processo_contestado: str
    marca_contestada: str
    classe_contestada: str
    especificacao_contestada: str
    titular_contestado: str
    numero_rpi: Optional[str] = None
    data_rpi: Optional[str] = None
    nome_cliente: str
    marca_anterior: Optional[str] = None
    processo_anterior: Optional[str] = None
    classe_anterior: Optional[str] = None
    produtos_servicos_anteriores: Optional[str] = None
    tipo_conflito: Optional[str] = None
    tipo_reproducao: Optional[str] = None
    analise_mercadologica: Optional[str] = None
    precedentes: Optional[str] = None
    coexistencia: Optional[str] = None

class OposicaoCreate(OposicaoBase):
    pass

class OposicaoUpdate(OposicaoBase):
    pass

class OposicaoResponse(OposicaoBase):
    id: int

    class Config:
        orm_mode = True