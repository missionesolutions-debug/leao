from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:

class TipoCategoria(Base):
    __tablename__ = "TipoCategorias"

    Id = Column(Integer, primary_key=True, index=True)

    # Define relationships here
