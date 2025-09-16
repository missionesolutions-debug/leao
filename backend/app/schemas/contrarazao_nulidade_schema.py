from pydantic import BaseModel
from typing import Optional

class ContrarazaoNulidadeBase(BaseModel):
    cliente: str
    classe: str
    titular: str
    especificacoes: Optional[str] = None
    numero: Optional[str] = None
    data: Optional[str] = None  # ou date, se preferir
    marca_terceiro: Optional[str] = None
    processo_terceiro: Optional[str] = None
    marca_requerida: Optional[str] = None
    processo_requerido: Optional[str] = None
    marca_cliente: Optional[str] = None
    comentario_diferenca_marcas: Optional[str] = None
    planilha_marcas_similares: Optional[str] = None
    comentario_distincao_produto_servico: Optional[str] = None

class ContrarazaoNulidadeCreate(ContrarazaoNulidadeBase):
    pass

class ContrarazaoNulidadeUpdate(ContrarazaoNulidadeBase):
    pass

class ContrarazaoNulidadeResponse(ContrarazaoNulidadeBase):
    id: int

    class Config:
        orm_mode = True