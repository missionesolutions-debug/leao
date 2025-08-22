# models/project_block.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from datetime import datetime
from database import Base
from .project import Project
from .project_phase import ProjectPhase
from .project_item import ProjectItem
from .project_group import ProjectGroup
from .project_measure_item import ProjectMeasureItem
from .project_image import ProjectImage

class ProjectBlock(Base):
    __tablename__ = "project_block"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    project_id: Mapped[int] = mapped_column(ForeignKey("project.id"))
    project: Mapped["Project"] = relationship("Project")

    project_phase_id: Mapped[int] = mapped_column(ForeignKey("project_phase.id"))
    project_phase: Mapped["ProjectPhase"] = relationship("ProjectPhase")

    project_item_id: Mapped[int] = mapped_column(ForeignKey("project_item.id"))
    project_item: Mapped["ProjectItem"] = relationship("ProjectItem")

    project_group_id: Mapped[int] = mapped_column(ForeignKey("project_group.id"))
    project_group: Mapped["ProjectGroup"] = relationship("ProjectGroup")

    usuario_id: Mapped[int | None] = mapped_column(Integer, nullable=True)

    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    updated_at: Mapped[datetime | None] = mapped_column(DateTime, nullable=True)
    position: Mapped[int | None] = mapped_column(Integer, nullable=True)

    code: Mapped[str] = mapped_column(String, nullable=False)
    name: Mapped[str | None] = mapped_column(String, nullable=True)
    type: Mapped[str | None] = mapped_column(String, nullable=True)

    min_images_amount: Mapped[int] = mapped_column(Integer, default=0)
    max_images_amount: Mapped[int] = mapped_column(Integer, default=0)
    images_label: Mapped[str | None] = mapped_column(String, nullable=True)
    observations_enabled: Mapped[bool] = mapped_column(Boolean, default=False)
    is_completed: Mapped[bool] = mapped_column(Boolean, default=False)
    instructions: Mapped[str | None] = mapped_column(String, nullable=True)
    box_type: Mapped[str | None] = mapped_column(String, nullable=True)
    card_board_type: Mapped[str | None] = mapped_column(String, nullable=True)
    closure_type: Mapped[str | None] = mapped_column(String, nullable=True)
    action_text: Mapped[str | None] = mapped_column(String, nullable=True)
    is_measurable_items: Mapped[bool] = mapped_column(Boolean, default=False)
    version: Mapped[str | None] = mapped_column(String, nullable=True)
    observation: Mapped[str | None] = mapped_column(String, nullable=True)

    box_type_active: Mapped[bool] = mapped_column(Boolean, default=False)
    card_board_type_active: Mapped[bool] = mapped_column(Boolean, default=False)
    closure_type_active: Mapped[bool] = mapped_column(Boolean, default=False)
    version_active: Mapped[bool] = mapped_column(Boolean, default=False)
    block_for_conclude: Mapped[bool] = mapped_column(Boolean, default=False)

    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)

    # Relacionamentos
    project_measure_items: Mapped[list["ProjectMeasureItem"]] = relationship(
        "ProjectMeasureItem", back_populates="project_block"
    )
    project_images: Mapped[list["ProjectImage"]] = relationship(
        "ProjectImage", back_populates="project_block"
    )
