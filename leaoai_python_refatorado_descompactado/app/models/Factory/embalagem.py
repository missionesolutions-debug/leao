from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .embalagemtype import EmbalagemType

class Embalagem(Base):
    __tablename__ = "Embalagens"

    Id = Column(Integer, primary_key=True, index=True)
    EmbalagemTypeId = Column(Integer, ForeignKey('embalagemtypes.Id'))
    Nome = Column(String)
    Imagem = Column(String, nullable=True)
    ImagemFormulacao = Column(String, nullable=True)
    Codigo = Column(String, nullable=True)
    Ativo = Column(Boolean)
    Excluido = Column(Boolean)

    # Define relationships here
    EmbalagemType = relationship('EmbalagemType', backref='embalagens')
