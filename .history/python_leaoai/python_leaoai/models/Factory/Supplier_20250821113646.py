# models/supplier.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime
from sqlalchemy.orm import Mapped, mapped_column
from datetime import datetime
from database import Base

class Supplier(Base):
    __tablename__ = "supplier"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String, nullable=False)
    cnpj: Mapped[str] = mapped_column(String, nullable=False)
    responsavel: Mapped[str] = mapped_column(String, nullable=True)
    endereco: Mapped[str] = mapped_column(String, nullable=True)
    telefone: Mapped[str] = mapped_column(String, nullable=True)
    email: Mapped[str] = mapped_column(String, nullable=True)
    cep: Mapped[str] = mapped_column(String, nullable=True)
    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)
    data_criacao: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
