from pydantic import BaseModel
from typing import Optional

class RecursoIndeferimentoRequest(BaseModel):
    # Dados do processo indeferido
    processo_numero: str
    processo_marca: str
    processo_classe: str
    processo_especificacao: str
    processo_titular: str
    data_indeferimento: Optional[str] = None
    
    # Fundamentos do indeferimento
    motivo_indeferimento: str
    artigo_legal: Optional[str] = None
    fundamentacao_inpi: Optional[str] = None
    
    # Dados de marcas anteriores ou colidentes (se mencionadas no indeferimento)
    marca_colidente: Optional[str] = None
    processo_colidente: Optional[str] = None
    titular_colidente: Optional[str] = None
    
    # Argumentação para o recurso
    contra_argumentacao: Optional[str] = None
    distintividade: Optional[str] = None
    ausencia_confundibilidade: Optional[str] = None
    coexistencia_pacifica: Optional[str] = None
    precedentes: Optional[str] = None
    doutrina: Optional[str] = None

class RecursoIndeferimentoRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str