# models/chat_ia_item.py

from sqlalchemy import Column, Integer, String, DateTime, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from datetime import datetime
from typing import Optional
from database import Base
from .chat_ia import ChatIA

class ChatIAItem(Base):
    __tablename__ = "chat_ia_item"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    chat_ia_id: Mapped[int] = mapped_column(Integer, ForeignKey("chat_ia.id"), nullable=False)
    usuario_id: Mapped[int] = mapped_column(Integer, nullable=False)
    mensagem: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    tipo: Mapped[Optional[str]] = mapped_column(String, nullable=True)  # "user" ou "openai"
    data_envio: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)

    # Relacionamento com ChatIA
    chat_ia: Mapped[ChatIA] = relationship("ChatIA", back_populates="chat_ia_items")
