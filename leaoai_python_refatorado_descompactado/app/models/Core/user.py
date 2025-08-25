from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:

class User(Base):
    __tablename__ = "users"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    Email = Column(String, index=True)
    Password = Column(String)
    RoleGate = Column(String)
    Avatar = Column(String, nullable=True)

    # Define relationships here
