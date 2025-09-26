from sqlalchemy import Column, Integer, String, Text, DateTime
from sqlalchemy.sql import func
from app.config.database import Base

class Peticao(Base):
    __tablename__ = "peticoes"

    id = Column(Integer, primary_key=True, index=True)
    tipo = Column(String, nullable=False)
    processo_contestado = Column(String, nullable=False)
    marca_contestada = Column(String, nullable=False)
    texto_peticao = Column(Text, nullable=False)
    created_at = Column(DateTime(timezone=True), server_default=func.now())
    updated_at = Column(DateTime(timezone=True), server_default=func.now(), onupdate=func.now())