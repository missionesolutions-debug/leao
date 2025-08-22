# models/section_translation.py

from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base
from models.section import Section

class SectionTranslation(Base):
    __tablename__ = "section_translation"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    section_id: Mapped[int] = mapped_column(Integer, ForeignKey("section.id"), nullable=False)
    
    language: Mapped[str] = mapped_column(String, nullable=False)  # pt-BR, en-USA, etc.
    title: Mapped[str] = mapped_column(String, nullable=True)
    subtitle: Mapped[str] = mapped_column(String, nullable=True)
    description: Mapped[str] = mapped_column(String, nullable=True)
    link_text: Mapped[str] = mapped_column(String, nullable=True)

    section: Mapped["Section"] = relationship("Section", back_populates="section_translations")
