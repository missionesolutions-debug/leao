from pydantic import BaseModel
from typing import Optional

class CaducidadeRequest(BaseModel):
    registro_requerida: str
    marca_requerida: str
    classe_requerida: str
    especificacoes_requerida: str
    titular_requerida: str
    processo_requerida: str
    nome_cliente: str
    marca_cliente: str
    data_deposito_cliente: str
    classe_cliente: str

class CaducidadeRepostRequest(BaseModel):
    chat_id: int
    prompt_anterior: str
    observacoes: str