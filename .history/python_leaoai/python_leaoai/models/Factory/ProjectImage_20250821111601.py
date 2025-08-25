# models/project_image.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from datetime import datetime
from database import Base
from .project import Project
from .project_block import ProjectBlock

class ProjectImage(Base):
    __tablename__ = "project_image"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    project_id: Mapped[int] = mapped_column(ForeignKey("project.id"))
    project: Mapped["Project"] = relationship("Project")

    project_block_id: Mapped[int] = mapped_column(ForeignKey("project_block.id"))
    project_block: Mapped["ProjectBlock"] = relationship("ProjectBlock")

    url_source: Mapped[str] = mapped_column(String, nullable=False)
    data_cadastro: Mapped[DateTime] = mapped_column(DateTime, default=datetime.utcnow)

    enable_on_report: Mapped[bool] = mapped_column(Boolean, default=False)
    description: Mapped[str] = mapped_column(String, nullable=True)
