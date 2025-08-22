# models/project.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from datetime import datetime
from database import Base
from .project_status import ProjectStatus
from .client import Client
from .supplier import Supplier
from .project_item import ProjectItem
from .project_usuario import ProjectUsuario

class Project(Base):
    __tablename__ = "project"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    name: Mapped[str] = mapped_column(String, nullable=False)

    project_status_id: Mapped[int] = mapped_column(ForeignKey("project_status.id"))
    project_status: Mapped["ProjectStatus"] = relationship("ProjectStatus")

    client_id: Mapped[int] = mapped_column(ForeignKey("client.id"))
    client: Mapped["Client"] = relationship("Client")

    supplier_id: Mapped[int] = mapped_column(ForeignKey("supplier.id"))
    supplier: Mapped["Supplier"] = relationship("Supplier")

    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    finished_at: Mapped[datetime | None] = mapped_column(DateTime, nullable=True)
    date_previsioned: Mapped[datetime | None] = mapped_column(DateTime, nullable=True)

    observations: Mapped[str | None] = mapped_column(String, nullable=True)
    pdf_report_total: Mapped[str | None] = mapped_column(String, nullable=True)
    guid: Mapped[str | None] = mapped_column(String, nullable=True)

    product_name: Mapped[str | None] = mapped_column(String, nullable=True)
    acabamento: Mapped[str | None] = mapped_column(String, nullable=True)
    codigo: Mapped[str | None] = mapped_column(String, nullable=True)

    ativo: Mapped[bool] = mapped_column(Boolean, default=True)
    excluido: Mapped[bool] = mapped_column(Boolean, default=False)
    is_template: Mapped[bool] = mapped_column(Boolean, default=False)

    # Relacionamentos
    project_items: Mapped[list["ProjectItem"]] = relationship("ProjectItem", back_populates="project")
    project_usuarios: Mapped[list["ProjectUsuario"]] = relationship("ProjectUsuario", back_populates="project")
