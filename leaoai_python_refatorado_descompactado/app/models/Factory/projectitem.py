from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .project import Project
    from .projectphase import ProjectPhase

class ProjectItem(Base):
    __tablename__ = "ProjectItems"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectId = Column(Integer, ForeignKey('projects.Id'))
    Name = Column(String)
    Position = Column(Integer)

    # Define relationships here
    Project = relationship('Project')
    ProjectPhases = relationship('ProjectPhase', backref='projectitem')
