# models/categoria.py

from sqlalchemy import Column, Integer, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import Optional
from database import Base  # Assumindo que você tenha um Base comum
from .tipo_categoria import TipoCategoria  # Ajuste o caminho conforme sua estrutura

class Categoria(Base):
    __tablename__ = "categoria"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    # Relação com a tabela pai
    tipo_categoria_id: Mapped[Optional[int]] = mapped_column(Integer, ForeignKey("tipo_categoria.id"))
    tipo_categoria: Mapped[Optional[TipoCategoria]] = relationship("TipoCategoria", back_populates="categorias")
