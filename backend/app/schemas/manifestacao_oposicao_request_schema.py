from pydantic import BaseModel
from typing import Optional

class ManifestacaoOposicaoRequest(BaseModel):
    # Dados do processo
    processo_numero: str
    processo_marca: str
    processo_classe: str
    processo_especificacao: str
    processo_titular: str
    
    # Dados da oposição
    oposicao_rpi: Optional[str] = None
    oposicao_data_rpi: Optional[str] = None
    oposicao_opoente: str
    oposicao_fundamento: str
    
    # Dados da marca do requerente
    marca_anterior: Optional[str] = None
    processo_anterior: Optional[str] = None
    classe_anterior: Optional[str] = None
    especificacao_anterior: Optional[str] = None
    titular_anterior: str
    
    # Argumentação
    fundamento_legal: Optional[str] = None
    analise_distintividade: Optional[str] = None
    coexistencia_mercado: Optional[str] = None
    precedentes: Optional[str] = None

class ManifestacaoOposicaoRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str