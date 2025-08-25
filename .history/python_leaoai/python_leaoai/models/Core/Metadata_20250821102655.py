# models/metadata.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.dialects.postgresql import UUID
from sqlalchemy.orm import relationship, Mapped, mapped_column
from datetime import datetime
import uuid
from database import Base
from models.section import Section  # Assumindo que Section será outro modelo

class Metadata(Base):
    __tablename__ = "metadata"

    id: Mapped[uuid.UUID] = mapped_column(UUID(as_uuid=True), primary_key=True, default=uuid.uuid4, index=True)

    url: Mapped[str] = mapped_column(String, nullable=True)
    file_name: Mapped[str] = mapped_column(String, nullable=True)
    file_length: Mapped[int] = mapped_column(Integer, nullable=True)
    file_type: Mapped[str] = mapped_column(String, nullable=True)
    metadata_ref: Mapped[str] = mapped_column(String, nullable=True)
    guid: Mapped[str] = mapped_column(String, nullable=True)
    entity_type: Mapped[str] = mapped_column(String, nullable=True)

    section_id: Mapped[int | None] = mapped_column(Integer, ForeignKey("section.id"), nullable=True)
    section: Mapped["Section"] = relationship("Section", back_populates="metadata")  # Relacionamento

    is_main: Mapped[bool] = mapped_column(Boolean, default=True)
    position: Mapped[int] = mapped_column(Integer, default=0)
    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
