# models/project_usuario.py

from sqlalchemy import Column, Integer, DateTime, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from datetime import datetime
from database import Base
from .project import Project  # Certifique-se de ajustar o import conforme a estrutura do seu projeto

class ProjectUsuario(Base):
    __tablename__ = "project_usuario"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    
    project_id: Mapped[int] = mapped_column(Integer, ForeignKey("project.id"))
    project: Mapped["Project"] = relationship("Project", back_populates="project_usuarios")

    usuario_id: Mapped[int] = mapped_column(Integer)

    data_cadastro: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
