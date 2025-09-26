# backend/app/schemas/peticao_schema.py
from pydantic import BaseModel
from typing import Optional

class PeticaoRequest(BaseModel):
    processo: str
    marca: str
    classe: str
    especificacao: str
    titular: str
    numero_rpi: str
    data_rpi: str
    nome_cliente: str
    tipo_conflito: str
    tipo_reproducao: str
    analise_mercadologica: str
    precedentes: str
    # Adicione outros campos conforme necessário