# models/project_measure_item.py

from sqlalchemy import Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from database import Base
from .project_block import ProjectBlock

class ProjectMeasureItem(Base):
    __tablename__ = "project_measure_item"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)

    project_block_id: Mapped[int] = mapped_column(ForeignKey("project_block.id"))
    project_block: Mapped["ProjectBlock"] = relationship("ProjectBlock")

    name: Mapped[str] = mapped_column(String, nullable=False)
    width: Mapped[str] = mapped_column(String, nullable=True)
    height: Mapped[str] = mapped_column(String, nullable=True)
    length: Mapped[str] = mapped_column(String, nullable=True)
    weight: Mapped[str] = mapped_column(String, nullable=True)

    name_editable: Mapped[bool] = mapped_column(Boolean, default=False)
