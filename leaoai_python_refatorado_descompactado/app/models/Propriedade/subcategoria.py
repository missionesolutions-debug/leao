from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .categoria import Categoria

class SubCategoria(Base):
    __tablename__ = "SubCategorias"

    Id = Column(Integer, primary_key=True, index=True)
    CategoriaId = Column(Integer, ForeignKey('categorias.Id'))

    # Define relationships here
    Categoria = relationship('Categoria', backref='subcategorias')
