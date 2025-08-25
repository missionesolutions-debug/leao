from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .projectblock import ProjectBlock

class ProjectImage(Base):
    __tablename__ = "ProjectImages"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectId = Column(Integer, nullable=True, ForeignKey('projects.Id'))
    ProjectBlockId = Column(Integer, nullable=True, ForeignKey('projectblocks.Id'))
    UrlSource = Column(String)
    DataCadastro = Column(DateTime)
    description = Column(String, nullable=True)
    enableOnReport = Column(Boolean)

    # Define relationships here
    ProjectBlock = relationship('ProjectBlock')
