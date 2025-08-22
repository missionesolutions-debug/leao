# models/contact.py

from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import Mapped, mapped_column
from database import Base  # Assumindo que Base vem do database.py

class Contact(Base):
    __tablename__ = "contacts"

    id: Mapped[int] = mapped_column(primary_key=True, index=True)

    tipo_contact_id: Mapped[int | None] = mapped_column(Integer, nullable=True)
    produto_id: Mapped[int | None] = mapped_column(Integer, nullable=True)
    name: Mapped[str] = mapped_column(String, nullable=True)
    email: Mapped[str] = mapped_column(String, nullable=True)
    phone: Mapped[str] = mapped_column(String, nullable=True)
    subject: Mapped[str] = mapped_column(String, nullable=True)
    message: Mapped[str] = mapped_column(String, nullable=True)

    # Campos para arquivos (equivalente ao [NotMapped] do C#)
    curriculum_url: Mapped[str] = mapped_column(String, nullable=True)
    curriculum_file_name: Mapped[str] = mapped_column(String, nullable=True)
