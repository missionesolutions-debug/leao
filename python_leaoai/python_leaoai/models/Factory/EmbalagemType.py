# models/embalagem_type.py

from sqlalchemy import Column, Integer, String, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship
from database import Base
from .embalagem import Embalagem

class EmbalagemType(Base):
    __tablename__ = "embalagem_type"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String, nullable=False)
    imagem: Mapped[str] = mapped_column(String, default="")
    imagem_formulacao: Mapped[str] = mapped_column(String, default="")
    codigo: Mapped[str] = mapped_column(String, default="")
    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)

    # Relacionamento com Embalagem
    embalagens: Mapped[list["Embalagem"]] = relationship("Embalagem", back_populates="embalagem_type")
