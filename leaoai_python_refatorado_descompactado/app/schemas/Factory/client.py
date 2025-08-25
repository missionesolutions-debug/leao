from pydantic import BaseModel
from typing import Optional
from datetime import datetime

class ClientBase(BaseModel):
    Id: int
    Nome: str
    CNPJ: str
    Endereco: Optional[str] = None
    Telefone: Optional[str] = None
    Email: Optional[str] = None
    CEP: Optional[str] = None
    PorcentagemCaixas: Optional[int] = None
    PorcentagemCaixasMontadas: Optional[int] = None
    Ativo: bool
    Excluido: bool
    DataCriacao: datetime

class ClientCreate(ClientBase):
    pass # Add specific fields for creation if needed

class Client(ClientBase):
    Id: int

    class Config:
        from_attributes = True
