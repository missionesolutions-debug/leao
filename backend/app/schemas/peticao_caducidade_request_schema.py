from pydantic import BaseModel
from typing import Optional

class PeticaoCaducidadeRequest(BaseModel):
    # Dados do registro a ser atacado
    registro_numero: str
    registro_marca: str
    registro_classe: str
    registro_especificacao: str
    registro_titular: str
    data_concessao: Optional[str] = None
    
    # Dados sobre o uso da marca
    periodo_nao_uso: Optional[str] = None
    data_inicio_nao_uso: Optional[str] = None
    
    # Pesquisas realizadas
    pesquisa_mercado: Optional[str] = None
    pesquisa_internet: Optional[str] = None
    consulta_orgaos: Optional[str] = None
    
    # Justificativas
    ausencia_uso_efetivo: Optional[str] = None
    falta_comprovacao: Optional[str] = None
    interesse_requerente: Optional[str] = None
    
    # Marca do requerente (se aplicável)
    marca_requerente: Optional[str] = None
    processo_requerente: Optional[str] = None
    classe_requerente: Optional[str] = None

class PeticaoCaducidadeRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str