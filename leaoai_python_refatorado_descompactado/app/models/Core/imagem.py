from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:

class Imagem(Base):
    __tablename__ = "Imagens"

    Id = Column(Integer, primary_key=True, index=True)

    # Define relationships here
