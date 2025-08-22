# models/pagina.py

from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship
from database import Base  # assumindo que Base vem do database.py

class Pagina(Base):
    __tablename__ = "paginas"

    id = Column(Integer, primary_key=True, index=True)

    group_pagina = Column(String, nullable=True)

    categoria_id = Column(Integer, ForeignKey("categorias.id"), nullable=True)
    autor_id = Column(Integer, nullable=True)

    # Relação com Categoria
    categoria = relationship("Categoria", back_populates="paginas")
