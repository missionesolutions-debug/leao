from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:

class EmbalagemType(Base):
    __tablename__ = "EmbalagemTypes"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    Excluido = Column(Boolean)

    # Define relationships here
