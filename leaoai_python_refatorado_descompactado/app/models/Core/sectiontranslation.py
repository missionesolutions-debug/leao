from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .section import Section

class SectionTranslation(Base):
    __tablename__ = "SectionTranslations"

    Id = Column(Integer, primary_key=True, index=True)
    SectionId = Column(Integer, ForeignKey('sections.Id'))

    # Define relationships here
    Section = relationship('Section', backref='sectiontranslations')
