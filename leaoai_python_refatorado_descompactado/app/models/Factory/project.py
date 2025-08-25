from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .projectusuario import ProjectUsuario
    from .projectstatus import ProjectStatus
    from .projectitem import ProjectItem
    from .client import Client
    from .supplier import Supplier

class Project(Base):
    __tablename__ = "projects"

    Id = Column(Integer, primary_key=True, index=True)
    Name = Column(String)
    CreatedAt = Column(DateTime)
    FinishedAt = Column(DateTime, nullable=True)
    DatePrevisioned = Column(DateTime, nullable=True)
    Observations = Column(String, nullable=True)
    PdfReportTotal = Column(String, nullable=True)
    Guid = Column(String)
    IsTemplate = Column(Boolean)
    Excluido = Column(Boolean)
    ClientId = Column(Integer, nullable=True, ForeignKey('clients.Id'))
    SupplierId = Column(Integer, nullable=True, ForeignKey('suppliers.Id'))
    ProjectStatusId = Column(Integer, nullable=True, ForeignKey('projectstatuses.Id'))

    # Define relationships here
    Client = relationship('Client', backref='projects')
    Supplier = relationship('Supplier', backref='projects')
    ProjectStatus = relationship('ProjectStatus', backref='projects')
    ProjectUsuarios = relationship('ProjectUsuario', backref='project')
    ProjectItems = relationship('ProjectItem', backref='project')
