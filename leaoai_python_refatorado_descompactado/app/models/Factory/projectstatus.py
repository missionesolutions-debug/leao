from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:

class ProjectStatus(Base):
    __tablename__ = "ProjectStatuses"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    Excluido = Column(Boolean)

    # Define relationships here
