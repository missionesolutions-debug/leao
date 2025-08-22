# models/user.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime
from sqlalchemy.orm import Mapped, mapped_column
from datetime import datetime
from database import Base

class User(Base):
    __tablename__ = "user"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    ativo: Mapped[bool] = mapped_column(Boolean, default=True, nullable=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False, nullable=True)
    data_criacao: Mapped[datetime] = mapped_column(DateTime, default=datetime.now, nullable=True)
    data_edicao: Mapped[datetime] = mapped_column(DateTime, default=datetime.now, nullable=True)

    email: Mapped[str] = mapped_column(String, nullable=True)
    username: Mapped[str] = mapped_column(String, nullable=True)
    password: Mapped[str] = mapped_column(String, nullable=True)
    role: Mapped[str] = mapped_column(String, nullable=True)
    avatar: Mapped[str] = mapped_column(String, nullable=True)
    name: Mapped[str] = mapped_column(String, nullable=True)
    surname: Mapped[str] = mapped_column(String, nullable=True)
    guid: Mapped[str] = mapped_column(String, nullable=True)
    birthday: Mapped[str] = mapped_column(String, nullable=True)
    gender: Mapped[str] = mapped_column(String, nullable=True)
    phone: Mapped[str] = mapped_column(String, nullable=True)
    cpf: Mapped[str] = mapped_column(String, nullable=True)
