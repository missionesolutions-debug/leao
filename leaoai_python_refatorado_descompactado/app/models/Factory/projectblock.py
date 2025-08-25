from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .projectmeasureitem import ProjectMeasureItem
    from .projectgroup import ProjectGroup
    from .projectimage import ProjectImage

class ProjectBlock(Base):
    __tablename__ = "ProjectBlocks"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectGroupId = Column(Integer, ForeignKey('projectgroups.Id'))
    Name = Column(String)
    MinImagesAmount = Column(Integer)
    MaxImagesAmount = Column(Integer)
    ObservationsEnabled = Column(Boolean)
    Instructions = Column(String, nullable=True)
    Position = Column(Integer)
    Code = Column(String)
    BoxTypeActive = Column(Boolean)
    CardBoardTypeActive = Column(Boolean)
    ClosureTypeActive = Column(Boolean)
    VersionActive = Column(Boolean)
    BlockForConclude = Column(Boolean)
    ActionText = Column(String, nullable=True)
    ImagesLabel = Column(String, nullable=True)
    IsCompleted = Column(Boolean)
    BoxType = Column(String, nullable=True)
    CardBoardType = Column(String, nullable=True)
    ClosureType = Column(String, nullable=True)
    CodigoBarraActive = Column(Boolean)
    CodigoBarraInfo = Column(String, nullable=True)
    Observation = Column(String, nullable=True)
    Version = Column(String, nullable=True)
    VersionInfo = Column(String, nullable=True)

    # Define relationships here
    ProjectGroup = relationship('ProjectGroup')
    ProjectMeasureItems = relationship('ProjectMeasureItem', backref='projectblock')
    ProjectImages = relationship('ProjectImage', backref='projectblock')
