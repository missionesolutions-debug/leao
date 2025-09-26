from pydantic import BaseModel
from typing import Optional

class NulidadeRequest(BaseModel):
    # Dados do registro a ser anulado (matching frontend field names)
    numero_registro: str
    numero_processo: Optional[str] = None
    marca_registrada: str
    classe_registro: str
    especificacao_registro: str
    titular_registro: str
    data_concessao: Optional[str] = None
    rpi_concessao: Optional[str] = None
    
    # Dados da marca anterior (base para nulidade)
    registro_anterior: Optional[str] = None
    marca_anterior: Optional[str] = None
    classe_anterior: Optional[str] = None
    especificacao_anterior: Optional[str] = None
    titular_anterior: Optional[str] = None
    data_deposito_anterior: Optional[str] = None
    
    # Análise comparativa
    tipo_reproducao: Optional[str] = None
    elementos_similares: Optional[str] = None
    analise_visual: Optional[str] = None
    analise_fonetica: Optional[str] = None
    
    # Argumentos adicionais
    precedentes: Optional[str] = None
    ma_fe: Optional[str] = None
    danos_mercado: Optional[str] = None
    decisoes_anteriores: Optional[str] = None

class NulidadeRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str