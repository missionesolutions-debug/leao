# models/embalagem.py

from sqlalchemy import Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from database import Base
from .embalagem_type import EmbalagemType  # Import necessário para o relacionamento

class Embalagem(Base):
    __tablename__ = "embalagem"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    embalagem_type_id: Mapped[int] = mapped_column(Integer, ForeignKey("embalagem_type.id"), nullable=False)
    nome: Mapped[str] = mapped_column(String, nullable=False)
    imagem: Mapped[str] = mapped_column(String, default="")
    imagem_formulacao: Mapped[str] = mapped_column(String, default="")
    codigo: Mapped[str] = mapped_column(String, default="")
    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)

    # Relacionamento com EmbalagemType
    embalagem_type: Mapped[EmbalagemType] = relationship("EmbalagemType", back_populates="embalagens")
