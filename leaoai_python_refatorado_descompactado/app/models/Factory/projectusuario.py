from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .project import Project
    from ..pessoa.usuario import Usuario

class ProjectUsuario(Base):
    __tablename__ = "ProjectUsuarios"

    Id = Column(Integer, primary_key=True, index=True)
    ProjectId = Column(Integer, ForeignKey('projects.Id'))
    UsuarioId = Column(Integer, ForeignKey('Usuarios.Id'))

    # Define relationships here
    Project = relationship('Project')
    Usuario = relationship('Usuario', backref='project_usuarios')
