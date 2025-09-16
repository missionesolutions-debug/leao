from pydantic import BaseModel
from typing import Optional

class ManifestacaoOposicaoBase(BaseModel):
    numero_registro: str
    numero_processo: str
    data_deposito: Optional[str] = None
    marca_registrada: str
    classe_registro: str
    especificacao_registro: str
    titular_registro: str
    rpi_oposicao: Optional[str] = None
    data_oposicao: Optional[str] = None
    opoente: Optional[str] = None
    marca_oposicao: Optional[str] = None
    processo_oposicao: Optional[str] = None
    fundamento_oposicao: Optional[str] = None
    tipo_conflito_alegado: Optional[str] = None
    tipo_reproducao_alegada: Optional[str] = None
    analise_mercadologica_defesa: Optional[str] = None
    distintividade_fonetica: Optional[str] = None
    distintividade_ideologica: Optional[str] = None
    distintividade_visual: Optional[str] = None
    especialidade_segmento: Optional[str] = None
    especialidade_publico: Optional[str] = None
    especialidade_canais: Optional[str] = None
    coexistencia: Optional[str] = None
    decisoes_anteriores: Optional[str] = None
    uso_anterior_boa_fe: Optional[str] = None
    outros_registros: Optional[str] = None
    ma_fe: Optional[str] = None

class ManifestacaoOposicaoCreate(ManifestacaoOposicaoBase):
    pass

class ManifestacaoOposicaoUpdate(ManifestacaoOposicaoBase):
    pass

class ManifestacaoOposicaoResponse(ManifestacaoOposicaoBase):
    id: int

    class Config:
        orm_mode = True