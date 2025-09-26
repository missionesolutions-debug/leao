from pydantic import BaseModel

class CaducidadeRequest(BaseModel):
    processo: str
    marca: str
    classe: str
    especificacao: str
    titular: str
    numero_rpi: str
    data_rpi: str
    nome_cliente: str
    fatos: str
    fundamentos: str
    provas: str