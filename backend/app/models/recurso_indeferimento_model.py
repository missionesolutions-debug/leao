from sqlalchemy import Column, Integer, String
from app.config.database import Base

class RecursoIndeferimento(Base):
    __tablename__ = "recursos_indeferimento"

    id = Column(Integer, primary_key=True, index=True)
    numero_processo = Column(String, nullable=False)
    marca = Column(String, nullable=False)
    classe_nice = Column(String, nullable=False)
    especificacao = Column(String, nullable=False)
    titular = Column(String, nullable=False)
    rpi_indeferimento = Column(String, nullable=True)
    data_indeferimento = Column(String, nullable=True)
    base_legal_indeferimento = Column(String, nullable=True)
    motivo_indeferimento = Column(String, nullable=True)
    anterioridade_citada = Column(String, nullable=True)
    conjunto_marcario = Column(String, nullable=True)
    elementos_distintivos = Column(String, nullable=True)
    precedentes_distintividade = Column(String, nullable=True)
    registros_similares = Column(String, nullable=True)
    segmento_mercado = Column(String, nullable=True)
    publico_alvo_distinto = Column(String, nullable=True)
    outros_argumentos = Column(String, nullable=True)