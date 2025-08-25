from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .projectblock import ProjectBlock
    from .projectphase import ProjectPhase

class ProjectGroup(Base):
    __tablename__ = "ProjectGroups"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectPhaseId = Column(Integer, ForeignKey('projectphases.Id'))
    Name = Column(String)
    Position = Column(Integer)

    # Define relationships here
    ProjectPhase = relationship('ProjectPhase')
    ProjectBlocks = relationship('ProjectBlock', backref='projectgroup')
