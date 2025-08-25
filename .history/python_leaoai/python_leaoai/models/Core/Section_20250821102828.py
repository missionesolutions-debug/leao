# models/section.py

from sqlalchemy import Column, Integer, String, Boolean
from sqlalchemy.orm import relationship, Mapped, mapped_column
from typing import List
from database import Base
from models.metadata import Metadata
from models.section_translation import SectionTranslation  # Assumindo que este modelo será criado

class Section(Base):
    __tablename__ = "section"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    ref: Mapped[str] = mapped_column(String, nullable=False)
    link_url: Mapped[str] = mapped_column(String, nullable=True)
    video_url: Mapped[str] = mapped_column(String, nullable=True)
    json_content: Mapped[str] = mapped_column(String, default="", nullable=False)
    enabled: Mapped[bool] = mapped_column(Boolean, default=True, nullable=False)

    section_translations: Mapped[List["SectionTranslation"]] = relationship(
        "SectionTranslation",
        back_populates="section",
        cascade="all, delete-orphan"
    )

    images: Mapped[List["Metadata"]] = relationship(
        "Metadata",
        back_populates="section"
    )
