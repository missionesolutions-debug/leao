# models/project_item.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from datetime import datetime
from typing import List
from database import Base
from .project import Project
from .project_phase import ProjectPhase

class ProjectItem(Base):
    __tablename__ = "project_item"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    project_id: Mapped[int] = mapped_column(ForeignKey("project.id"))
    project: Mapped["Project"] = relationship("Project")

    name: Mapped[str] = mapped_column(String, nullable=False)
    pdf_report: Mapped[str] = mapped_column(String, nullable=True)
    product_name: Mapped[str] = mapped_column(String, nullable=True)
    acabamento: Mapped[str] = mapped_column(String, nullable=True)
    codigo: Mapped[str] = mapped_column(String, nullable=True)
    amount: Mapped[int] = mapped_column(Integer, nullable=True)
    
    is_approved: Mapped[bool] = mapped_column(Boolean, default=False)
    is_reproved: Mapped[bool] = mapped_column(Boolean, default=False)
    concluded_at: Mapped[DateTime] = mapped_column(DateTime, nullable=True)
    started_at: Mapped[DateTime] = mapped_column(DateTime, nullable=True)

    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)
    is_template: Mapped[bool] = mapped_column(Boolean, default=False)

    acabamento_id: Mapped[int] = mapped_column(Integer, nullable=True)
    embalagem_caixa_id: Mapped[int] = mapped_column(Integer, nullable=True)
    embalagem_papelao_id: Mapped[int] = mapped_column(Integer, nullable=True)
    embalagem_fechamento_id: Mapped[int] = mapped_column(Integer, nullable=True)

    project_phases: Mapped[List["ProjectPhase"]] = relationship(
        "ProjectPhase", back_populates="project_item"
    )
