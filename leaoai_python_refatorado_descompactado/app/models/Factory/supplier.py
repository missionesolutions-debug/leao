from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:

class Supplier(Base):
    __tablename__ = "Suppliers"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    CNPJ = Column(String, nullable=True)
    Ativo = Column(Boolean)
    Excluido = Column(Boolean)

    # Define relationships here
