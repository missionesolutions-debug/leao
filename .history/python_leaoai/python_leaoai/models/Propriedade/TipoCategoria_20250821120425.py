# models/tipo_categoria.py

from sqlalchemy import Column, Integer
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import List
from database import Base
from .categoria import Categoria
from .sub_categoria import SubCategoria

class TipoCategoria(Base):
    __tablename__ = "tipo_categoria"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    # Relações
    categorias: Mapped[List[Categoria]] = relationship("Categoria", back_populates="tipo_categoria")
    sub_categorias: Mapped[List[SubCategoria]] = relationship("SubCategoria", back_populates="tipo_categoria")
