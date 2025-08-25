# models/chat_ia_item.py
from sqlalchemy import Column, Integer, String, DateTime, ForeignKey
from sqlalchemy.orm import relationship
from datetime import datetime
from .ChatIA import Base

class ChatIAItem(Base):
    __tablename__ = "chat_ia_item"

    id = Column(Integer, primary_key=True)
    chat_ia_id = Column(Integer, ForeignKey("chat_ia.id"), nullable=False)
    usuario_id = Column(Integer, nullable=False)
    mensagem = Column(String, nullable=False)
    tipo = Column(String, nullable=False)  # "user" ou "openai"
    data_envio = Column(DateTime, default=datetime.utcnow)

    # Relacionamento reverso
    chat_ia = relationship("ChatIA", back_populates="chat_ia_items")