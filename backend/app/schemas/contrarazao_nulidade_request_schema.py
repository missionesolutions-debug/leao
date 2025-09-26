from pydantic import BaseModel
from typing import Optional

class ContrarazaoNulidadeRequest(BaseModel):
    # Campos do frontend (HTML)
    cliente: str
    classe: str
    titular: Optional[str] = None
    especificacoes: Optional[str] = None
    numero: Optional[str] = None
    data: Optional[str] = None
    marca_terceiro: Optional[str] = None
    processo_terceiro: Optional[str] = None
    marca_requerida: Optional[str] = None
    processo_requerido: Optional[str] = None
    marca_cliente: Optional[str] = None
    
    # Campos para compatibilidade com o serviço
    @property
    def processo_nulidade(self) -> str:
        return self.processo_terceiro or ""
    
    @property
    def processo_marca_contestada(self) -> str:
        return self.processo_requerido or ""
    
    @property
    def registro_contestado(self) -> str:
        return self.numero or ""
    
    @property
    def classe_contestada(self) -> str:
        return self.classe
    
    @property
    def especificacao_contestada(self) -> str:
        return self.especificacoes or ""
    
    @property
    def titular_contestado(self) -> str:
        return self.titular or ""
    
    @property
    def requerente_nulidade(self) -> str:
        return self.cliente
    
    @property
    def data_pedido_nulidade(self) -> Optional[str]:
        return self.data
    
    @property
    def fundamentos_nulidade(self) -> str:
        return "Fundamentos conforme petição"
    
    @property
    def marca_anterior_alegada(self) -> Optional[str]:
        return self.marca_terceiro

class ContrarazaoNulidadeRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str