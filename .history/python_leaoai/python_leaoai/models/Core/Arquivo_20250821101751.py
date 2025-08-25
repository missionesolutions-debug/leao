# models/arquivo.py

from sqlalchemy import Column, Integer, String
from database import Base  # Assumindo que Base vem do database.py

class Arquivo(Base):
    __tablename__ = "arquivos"

    id = Column(Integer, primary_key=True, index=True)

    file_name = Column(String, nullable=True)
    file_type = Column(String, nullable=True)
    file_size = Column(String, nullable=True)
    file_data = Column(String, nullable=True)
    file_tags = Column(String, nullable=True)
    file_image = Column(String, nullable=True)
    guid = Column(String, nullable=True)
    slug = Column(String, nullable=True)
    place_received = Column(String, nullable=True)

    table_id = Column(Integer, nullable=True)
    table_action = Column(String, nullable=True)
    description = Column(String, nullable=True)
