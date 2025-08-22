# models/section.py

from sqlalchemy import Column, Integer, String, Boolean, Text
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base
from models.metadata import Metadata
from models.section_translation import SectionTranslation

class Section(Base):
    __tablename__ = "section"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    ref: Mapped[str] = mapped_column(String, nullable=False)
    link_url: Mapped[str] = mapped_column(String, nullable=True)
    video_url: Mapped[str] = mapped_column(String, nullable=True)
    json_content: Mapped[str] = mapped_column(Text, nullable=False, default="")
    enabled: Mapped[bool] = mapped_column(Boolean, nullable=False, default=True)

    # Relação com SectionTranslation
    section_translations: Mapped[list["SectionTranslation"]] = relationship(
        "SectionTranslation",
        back_populates="section",
        cascade="all, delete-orphan"
    )

    # Relação com Metadata (imagens)
    images: Mapped[list["Metadata"]] = relationship(
        "Metadata",
        back_populates="section",
        cascade="all, delete-orphan"
    )
