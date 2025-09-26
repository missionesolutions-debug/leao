from pydantic import BaseModel
from typing import Optional

class NulidadeBase(BaseModel):
    numero_registro: str
    marca_registrada: str
    classe_registro: str
    especificacao_registro: str
    titular_registro: str
    data_concessao: Optional[str] = None
    rpi_concessao: Optional[str] = None
    registro_anterior: Optional[str] = None
    marca_anterior: Optional[str] = None
    classe_anterior: Optional[str] = None
    especificacao_anterior: Optional[str] = None
    titular_anterior: Optional[str] = None
    data_deposito_anterior: Optional[str] = None
    tipo_reproducao: Optional[str] = None
    elementos_similares: Optional[str] = None
    analise_visual: Optional[str] = None
    analise_fonetica: Optional[str] = None
    precedentes: Optional[str] = None
    ma_fe: Optional[str] = None
    danos_mercado: Optional[str] = None
    decisoes_anteriores: Optional[str] = None

class NulidadeCreate(NulidadeBase):
    pass

class NulidadeUpdate(NulidadeBase):
    pass

class NulidadeResponse(NulidadeBase):
    id: int

    class Config:
        from_attributes = True