# models/chat_ia.py

from sqlalchemy import Column, Integer, String, DateTime, Boolean, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from typing import List
from datetime import datetime
from database import Base
from .chat_ia_item import ChatIAItem

class ChatIA(Base):
    __tablename__ = "chat_ia"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    usuario_id: Mapped[int] = mapped_column(Integer, nullable=False)
    data_criacao: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    conversation_id: Mapped[str] = mapped_column(String, nullable=True)
    thread_id: Mapped[str] = mapped_column(String, nullable=True)
    guid: Mapped[str] = mapped_column(String, nullable=True)
    typed: Mapped[str] = mapped_column(String, default="chat")
    is_prompt: Mapped[bool] = mapped_column(Boolean, default=False)
    processo_contestado: Mapped[str] = mapped_column(String, default="")
    marca_contestada: Mapped[str] = mapped_column(String, default="")
    classe_contestada: Mapped[str] = mapped_column(String, default="")
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)

    # Relacionamento 1:N com ChatIAItem
    chat_ia_items: Mapped[List[ChatIAItem]] = relationship(
        "ChatIAItem",
        back_populates="chat_ia",
        cascade="all, delete-orphan"
    )
