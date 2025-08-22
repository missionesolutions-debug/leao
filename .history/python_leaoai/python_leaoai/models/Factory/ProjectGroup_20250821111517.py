# models/project_group.py

from sqlalchemy import Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from database import Base
from .project import Project
from .project_phase import ProjectPhase
from .project_item import ProjectItem
from .project_block import ProjectBlock

class ProjectGroup(Base):
    __tablename__ = "project_group"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    project_id: Mapped[int] = mapped_column(ForeignKey("project.id"))
    project: Mapped["Project"] = relationship("Project")

    project_phase_id: Mapped[int] = mapped_column(ForeignKey("project_phase.id"))
    project_phase: Mapped["ProjectPhase"] = relationship("ProjectPhase")

    project_item_id: Mapped[int] = mapped_column(ForeignKey("project_item.id"))
    project_item: Mapped["ProjectItem"] = relationship("ProjectItem")

    name: Mapped[str] = mapped_column(String, nullable=False)

    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)

    # Relacionamento
    project_blocks: Mapped[list["ProjectBlock"]] = relationship(
        "ProjectBlock", back_populates="project_group"
    )
