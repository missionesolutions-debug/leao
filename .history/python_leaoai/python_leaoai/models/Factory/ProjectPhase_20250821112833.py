# models/project_phase.py

from sqlalchemy import Column, Integer, String, Boolean, ForeignKey, DateTime
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import List
from datetime import datetime
from database import Base
from .project import Project
from .project_item import ProjectItem
from .project_group import ProjectGroup
from .project_block import ProjectBlock

class ProjectPhase(Base):
    __tablename__ = "project_phase"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    project_id: Mapped[int] = mapped_column(ForeignKey("project.id"))
    project: Mapped["Project"] = relationship("Project")

    project_item_id: Mapped[int] = mapped_column(ForeignKey("project_item.id"))
    project_item: Mapped["ProjectItem"] = relationship("ProjectItem")

    name: Mapped[str] = mapped_column(String, nullable=False)

    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)
    is_approved: Mapped[bool] = mapped_column(Boolean, default=False)
    is_reproved: Mapped[bool] = mapped_column(Boolean, default=False)
    usuario_id: Mapped[int] = mapped_column(Integer)
    is_taken: Mapped[bool] = mapped_column(Boolean, default=False)
    updated_at: Mapped[datetime] = mapped_column(DateTime, nullable=True)

    # Navegação
    project_groups: Mapped[List["ProjectGroup"]] = relationship("ProjectGroup", back_populates="project_phase")
    project_blocks: Mapped[List["ProjectBlock"]] = relationship("ProjectBlock", back_populates="project_phase")
