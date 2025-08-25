from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .projectitem import ProjectItem
    from .projectgroup import ProjectGroup
    from ..pessoa.usuario import Usuario

class ProjectPhase(Base):
    __tablename__ = "ProjectPhases"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectItemId = Column(Integer, ForeignKey('projectitems.Id'))
    UsuarioId = Column(Integer, nullable=True, ForeignKey('Usuarios.Id'))
    Name = Column(String)
    IsApproved = Column(Boolean)
    IsTaken = Column(Boolean)
    Position = Column(Integer)

    # Define relationships here
    ProjectItem = relationship('ProjectItem')
    Usuario = relationship('Usuario', backref='project_phases')
    ProjectGroups = relationship('ProjectGroup', backref='projectphase')
