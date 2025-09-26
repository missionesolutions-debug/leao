from sqlalchemy import Column, Integer, String, Text, DateTime, ForeignKey
from datetime import datetime
from app.config.database import Base

class ChatHistorico(Base):
    __tablename__ = "chat_historico"

    id = Column(Integer, primary_key=True, index=True)
    peticao_id = Column(Integer, ForeignKey("peticoes.id"), nullable=False)
    prompt_usuario = Column(Text, nullable=False)  # Observações/refinamentos do usuário
    resposta_ia = Column(Text, nullable=False)     # Resposta da IA
    created_at = Column(DateTime, default=datetime.utcnow)