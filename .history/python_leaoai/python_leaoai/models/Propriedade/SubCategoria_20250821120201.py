# models/sub_categoria.py

from sqlalchemy import Column, Integer, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import Optional
from database import Base
from .categoria import Categoria
from .tipo_categoria import TipoCategoria

class SubCategoria(Base):
    __tablename__ = "sub_categoria"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    # Chaves estrangeiras
    categoria_id: Mapped[Optional[int]] = mapped_column(Integer, ForeignKey("categoria.id"))
    tipo_categoria_id: Mapped[Optional[int]] = mapped_column(Integer, ForeignKey("tipo_categoria.id"))

    # Relações
    categoria: Mapped[Optional[Categoria]] = relationship("Categoria", back_populates="sub_categorias")
    tipo_categoria: Mapped[Optional[TipoCategoria]] = relationship("TipoCategoria", back_populates="sub_categorias")
