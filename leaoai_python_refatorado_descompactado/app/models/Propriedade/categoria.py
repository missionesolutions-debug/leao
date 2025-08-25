from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .tipocategoria import TipoCategoria

class Categoria(Base):
    __tablename__ = "Categorias"

    Id = Column(Integer, primary_key=True, index=True)
    TipoCategoriaId = Column(Integer, ForeignKey('tipocategorias.Id'))

    # Define relationships here
    TipoCategoria = relationship('TipoCategoria', backref='categorias')
