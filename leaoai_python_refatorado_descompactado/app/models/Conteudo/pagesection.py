from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Float
from sqlalchemy.orm import relationship
from app.core.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .pagina import Pagina

class PageSection(Base):
    __tablename__ = "PageSections"

    Id = Column(Integer, primary_key=True, index=True)
    PaginaId = Column(Integer, ForeignKey('paginas.Id'))

    # Define relationships here
    Pagina = relationship('Pagina', backref='pagesections')
