# models/usuario.py

from sqlalchemy import Column, Integer, String, DateTime, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column
from typing import Optional
from datetime import datetime
from database import Base  # Assumindo que você tenha um Base comum

class Usuario(Base):
    __tablename__ = "usuario"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    login: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    email: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    password: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    role_gate: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    avatar: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    imagem: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    password_token: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    password_token_expiry: Mapped[Optional[datetime]] = mapped_column(DateTime, nullable=True)
    subscription_id: Mapped[Optional[int]] = mapped_column(Integer, nullable=True)
    journey_id: Mapped[Optional[int]] = mapped_column(Integer, nullable=True)
    guid: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    token: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    data_nascimento: Mapped[Optional[datetime]] = mapped_column(DateTime, nullable=True)
    genero: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    logradouro: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cep: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cidade: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    estado: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    bairro: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    complemento: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    numero: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    phone: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cpf: Mapped[Optional[str]] = mapped_column(String, nullable=True)
