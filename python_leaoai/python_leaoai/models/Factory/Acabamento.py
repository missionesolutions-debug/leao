# models/acabamento.py

from sqlalchemy import Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base
from .client import Client  # Certifique-se de ter o modelo Client definido

class Acabamento(Base):
    __tablename__ = "acabamento"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String, nullable=True)
    codigo: Mapped[str] = mapped_column(String, default="", nullable=True)
    imagem: Mapped[str] = mapped_column(String, default="", nullable=True)
    imagem_formulacao: Mapped[str] = mapped_column(String, default="", nullable=True)
    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)

    client_id: Mapped[int] = mapped_column(Integer, ForeignKey("client.id"))
    client: Mapped[Client] = relationship("Client", back_populates="acabamentos")
