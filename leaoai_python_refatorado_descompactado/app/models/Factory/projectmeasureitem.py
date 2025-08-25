from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .projectblock import ProjectBlock

class ProjectMeasureItem(Base):
    __tablename__ = "ProjectMeasureItems"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectBlockId = Column(Integer, ForeignKey('projectblocks.Id'))
    Name = Column(String)
    Width = Column(Float, nullable=True)
    Height = Column(Float, nullable=True)
    Length = Column(Float, nullable=True)
    Weight = Column(Float, nullable=True)
    NameEditable = Column(Boolean)

    # Define relationships here
    ProjectBlock = relationship('ProjectBlock')
