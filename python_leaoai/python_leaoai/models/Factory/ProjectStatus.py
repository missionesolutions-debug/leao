# models/project_status.py

from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import Mapped, mapped_column
from database import Base

class ProjectStatus(Base):
    __tablename__ = "project_status"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[str] = mapped_column(String, nullable=False)
