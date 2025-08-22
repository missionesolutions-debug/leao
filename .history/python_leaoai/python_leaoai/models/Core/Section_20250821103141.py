# models/section.py

from sqlalchemy import Column, Integer, String, Boolean
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base
from models.section_translation import SectionTranslation
from models.metadata import Metadata

class Section(Base):
    __tablename__ = "section"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    ref: Mapped[str] = mapped_column(String, nullable=False)
    link_url: Mapped[str] = mapped_column(String, nullable=True)
    video_url: Mapped[str] = mapped_column(String, nullable=True)
    json_content: Mapped[str] = mapped_column(String, default="")
    enabled: Mapped[bool] = mapped_column(Boolean, default=True)

    translations: Mapped[list[SectionTranslation]] = relationship(
        "SectionTranslation",
        back_populates="section",
        cascade="all, delete-orphan"
    )

    images: Mapped[list[Metadata]] = relationship(
        "Metadata",
        back_populates="section",
        cascade="all, delete-orphan"
    )
