from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .chatia import ChatIA

class ChatIAItem(Base):
    __tablename__ = "ChatIAItems"

    Id = Column(Integer, primary_key=True, index=True)
    ChatIAId = Column(Integer, ForeignKey('chatias.Id'))

    # Define relationships here
    ChatIA = relationship('ChatIA', backref='chatias')
