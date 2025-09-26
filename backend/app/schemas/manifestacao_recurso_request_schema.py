from pydantic import BaseModel
from typing import Optional

class ManifestacaoRecursoRequest(BaseModel):
    # Dados do processo principal
    processo_numero: str
    processo_marca: str
    processo_classe: str
    processo_especificacao: str
    processo_titular: str
    
    # Dados do recurso
    recurso_numero: str
    recurso_data: Optional[str] = None
    recorrente: str
    recurso_fundamento: str
    
    # Dados da marca anterior (se aplicável)
    marca_anterior: Optional[str] = None
    processo_anterior: Optional[str] = None
    classe_anterior: Optional[str] = None
    especificacao_anterior: Optional[str] = None
    titular_anterior: str
    
    # Argumentação para manifestação
    fundamento_legal: Optional[str] = None
    analise_merito: Optional[str] = None
    precedentes_jurisprudencia: Optional[str] = None
    argumentos_tecnicos: Optional[str] = None

class ManifestacaoRecursoRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str