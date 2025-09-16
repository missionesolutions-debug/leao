from sqlalchemy import Column, Integer, String
from app.config.database import Base

class Nulidade(Base):
    __tablename__ = "nulidades"

    id = Column(Integer, primary_key=True, index=True)
    numero_registro = Column(String, nullable=False)
    marca_registrada = Column(String, nullable=False)
    classe_registro = Column(String, nullable=False)
    especificacao_registro = Column(String, nullable=False)
    titular_registro = Column(String, nullable=False)
    data_concessao = Column(String, nullable=True)
    rpi_concessao = Column(String, nullable=True)
    registro_anterior = Column(String, nullable=True)
    marca_anterior = Column(String, nullable=True)
    classe_anterior = Column(String, nullable=True)
    especificacao_anterior = Column(String, nullable=True)
    titular_anterior = Column(String, nullable=True)
    data_deposito_anterior = Column(String, nullable=True)
    tipo_reproducao = Column(String, nullable=True)
    elementos_similares = Column(String, nullable=True)
    analise_visual = Column(String, nullable=True)
    analise_fonetica = Column(String, nullable=True)
    precedentes = Column(String, nullable=True)
    ma_fe = Column(String, nullable=True)
    danos_mercado = Column(String, nullable=True)
    decisoes_anteriores = Column(String, nullable=True)