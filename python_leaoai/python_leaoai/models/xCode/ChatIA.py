# models/chat_ia.py
from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import relationship, declarative_base
from datetime import datetime

Base = declarative_base()

class ChatIA(Base):
    __tablename__ = "chat_ia"

    id = Column(Integer, primary_key=True)
    usuario_id = Column(Integer, nullable=False)
    data_criacao = Column(DateTime, default=datetime.utcnow)
    conversation_id = Column(String, nullable=True)
    thread_id = Column(String, nullable=True)
    guid = Column(String, nullable=True)
    typed = Column(String, default="chat")
    is_prompt = Column(Boolean, default=False)
    processo_contestado = Column(String, default="")
    marca_contestada = Column(String, default="")
    classe_contestada = Column(String, default="")
    excluido = Column(Boolean, default=False)

    # Relacionamento 1:N com ChatIAItem
    chat_ia_items = relationship("ChatIAItem", back_populates="chat_ia", cascade="all, delete-orphan")