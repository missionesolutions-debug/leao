from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .client import Client

class Acabamento(Base):
    __tablename__ = "Acabamentos"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    Codigo = Column(String, nullable=True)
    Imagem = Column(String, nullable=True)
    ImagemFormulacao = Column(String, nullable=True)
    Ativo = Column(Boolean)
    Excluido = Column(Boolean)
    ClientId = Column(Integer, ForeignKey('clients.Id'))

    # Define relationships here
    Client = relationship('Client', backref='acabamentos')
