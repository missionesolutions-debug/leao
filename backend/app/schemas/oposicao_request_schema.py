from pydantic import BaseModel
from typing import Optional

class OposicaoRequest(BaseModel):
    # Dados do processo contestado
    processo_contestado: str
    marca_contestada: str
    classe_contestada: str
    especificacao_contestada: str
    titular_contestado: str
    numero_rpi: Optional[str] = None
    data_rpi: Optional[str] = None
    
    # Dados do cliente/opoente
    nome_cliente: str
    marca_anterior: Optional[str] = None
    processo_anterior: Optional[str] = None
    classe_anterior: Optional[str] = None
    produtos_anterior: Optional[str] = None
    
    # Análise de conflito
    tipo_conflito: Optional[str] = None
    tipo_reproducao: Optional[str] = None
    analise_mercadologica: Optional[str] = None
    precedentes: Optional[str] = None
    coexistencia: Optional[str] = None

class OposicaoRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str