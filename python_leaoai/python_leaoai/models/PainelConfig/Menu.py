# models/menu.py

from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base
from .page import Page  # assumindo que você terá o modelo Page

class Menu(Base):
    __tablename__ = "menu"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String, nullable=True)
    role_gate: Mapped[str] = mapped_column(String, nullable=True)
    icon: Mapped[str] = mapped_column(String, nullable=True)

    # Relação com Page
    page: Mapped[list[Page]] = relationship("Page", back_populates="menu", cascade="all, delete-orphan")
