# models/categoria.py

from sqlalchemy import Column, Integer, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import List, Optional
from database import Base
from .tipo_categoria import TipoCategoria
from .sub_categoria import SubCategoria

class Categoria(Base):
    __tablename__ = "categoria"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    
    tipo_categoria_id: Mapped[Optional[int]] = mapped_column(Integer, ForeignKey("tipo_categoria.id"))
    tipo_categoria: Mapped[TipoCategoria] = relationship("TipoCategoria", back_populates="categorias")

    sub_categorias: Mapped[List[SubCategoria]] = relationship("SubCategoria", back_populates="categoria")
