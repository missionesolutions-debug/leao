from pydantic import BaseModel
from typing import Optional

class RecursoIndeferimentoBase(BaseModel):
    numero_processo: str
    marca: str
    classe_nice: str
    especificacao: str
    titular: str
    rpi_indeferimento: Optional[str] = None
    data_indeferimento: Optional[str] = None
    base_legal_indeferimento: Optional[str] = None
    motivo_indeferimento: Optional[str] = None
    anterioridade_citada: Optional[str] = None
    conjunto_marcario: Optional[str] = None
    elementos_distintivos: Optional[str] = None
    precedentes_distintividade: Optional[str] = None
    registros_similares: Optional[str] = None
    segmento_mercado: Optional[str] = None
    publico_alvo_distinto: Optional[str] = None
    outros_argumentos: Optional[str] = None

class RecursoIndeferimentoCreate(RecursoIndeferimentoBase):
    pass

class RecursoIndeferimentoUpdate(RecursoIndeferimentoBase):
    pass

class RecursoIndeferimentoResponse(RecursoIndeferimentoBase):
    id: int

    class Config:
        orm_mode = True