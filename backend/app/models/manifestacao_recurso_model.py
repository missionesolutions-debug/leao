from sqlalchemy import Column, Integer, String
from app.config.database import Base

class ManifestacaoRecurso(Base):
    __tablename__ = "manifestacoes_recurso"

    id = Column(Integer, primary_key=True, index=True)
    cliente = Column(String, nullable=False)
    processo = Column(String, nullable=False)
    marca = Column(String, nullable=False)
    classe = Column(String, nullable=False)
    titular = Column(String, nullable=False)
    rpi_data = Column(String, nullable=True)
    numero_rpi = Column(String, nullable=True)
    fundamento_recorrente = Column(String, nullable=True)
    fundamento_legal_indeferimento = Column(String, nullable=True)