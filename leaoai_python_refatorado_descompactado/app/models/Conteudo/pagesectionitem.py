from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .pagesection import PageSection

class PageSectionItem(Base):
    __tablename__ = "PageSectionItems"

    Id = Column(Integer, primary_key=True, index=True)
    PageSectionId = Column(Integer, ForeignKey('pagesections.Id'))

    # Define relationships here
    PageSection = relationship('PageSection', backref='pagesectionitems')
