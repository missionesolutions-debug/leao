from pydantic import BaseModel
from typing import Optional

class ManifestacaoRecursoBase(BaseModel):
    cliente: str
    processo: str
    marca: str
    classe: str
    titular: str
    rpi_data: Optional[str] = None
    numero_rpi: Optional[str] = None
    fundamento_recorrente: Optional[str] = None
    fundamento_legal_indeferimento: Optional[str] = None

class ManifestacaoRecursoCreate(ManifestacaoRecursoBase):
    pass

class ManifestacaoRecursoUpdate(ManifestacaoRecursoBase):
    pass

class ManifestacaoRecursoResponse(ManifestacaoRecursoBase):
    id: int

    class Config:
        orm_mode = True