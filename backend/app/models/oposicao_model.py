from sqlalchemy import Column, Integer, String
from app.config.database import Base

class Oposicao(Base):
    __tablename__ = "oposicoes"

    id = Column(Integer, primary_key=True, index=True)
    processo_contestado = Column(String, nullable=False)
    marca_contestada = Column(String, nullable=False)
    classe_contestada = Column(String, nullable=False)
    especificacao_contestada = Column(String, nullable=False)
    titular_contestado = Column(String, nullable=False)
    numero_rpi = Column(String, nullable=True)
    data_rpi = Column(String, nullable=True)
    nome_cliente = Column(String, nullable=False)
    marca_anterior = Column(String, nullable=True)
    processo_anterior = Column(String, nullable=True)
    classe_anterior = Column(String, nullable=True)
    produtos_servicos_anteriores = Column(String, nullable=True)
    tipo_conflito = Column(String, nullable=True)
    tipo_reproducao = Column(String, nullable=True)
    analise_mercadologica = Column(String, nullable=True)
    precedentes = Column(String, nullable=True)
    coexistencia = Column(String, nullable=True)