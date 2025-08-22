# models/client.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime
from sqlalchemy.orm import Mapped, mapped_column, relationship
from datetime import datetime
from database import Base
from .acabamento import Acabamento  # Import necessário para o relacionamento

class Client(Base):
    __tablename__ = "client"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String, nullable=False)
    cnpj: Mapped[str] = mapped_column(String, nullable=False)
    endereco: Mapped[str] = mapped_column(String, nullable=True)
    telefone: Mapped[str] = mapped_column(String, nullable=True)
    email: Mapped[str] = mapped_column(String, nullable=True)
    cep: Mapped[str] = mapped_column(String, nullable=True)
    porcentagem_caixas: Mapped[int] = mapped_column(Integer, default=0)
    porcentagem_caixas_montadas: Mapped[int] = mapped_column(Integer, default=0)
    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)
    data_criacao: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)

    # Relacionamento com Acabamento
    acabamentos: Mapped[list[Acabamento]] = relationship("Acabamento", back_populates="client")
