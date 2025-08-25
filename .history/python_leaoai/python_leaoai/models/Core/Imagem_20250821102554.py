# models/imagem.py

from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import Mapped, mapped_column
from database import Base  # Assumindo que Base vem do database.py

class Imagem(Base):
    __tablename__ = "imagens"

    id: Mapped[int] = mapped_column(primary_key=True, index=True)

    file_name: Mapped[str] = mapped_column(String, nullable=True)
    file_type: Mapped[str] = mapped_column(String, nullable=True)
    file_size: Mapped[str] = mapped_column(String, nullable=True)
    file_data: Mapped[str] = mapped_column(String, nullable=True)
    file_tags: Mapped[str] = mapped_column(String, nullable=True)
    file_image: Mapped[str] = mapped_column(String, nullable=True)
    guid: Mapped[str] = mapped_column(String, nullable=True)
    slug: Mapped[str] = mapped_column(String, nullable=True)
    place_received: Mapped[str] = mapped_column(String, nullable=True)
    table_id: Mapped[int | None] = mapped_column(Integer, nullable=True)
    table_action: Mapped[str] = mapped_column(String, nullable=True)
    alt: Mapped[str] = mapped_column(String, nullable=True)
    description_file: Mapped[str] = mapped_column(String, nullable=True)
    url: Mapped[str] = mapped_column(String, nullable=True)
