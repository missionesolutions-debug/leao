# models/page_section_item.py

from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import relationship
from datetime import datetime

from database import Base  # assumindo que Base vem do database.py

class PageSectionItem(Base):
    __tablename__ = "page_section_items"

    id = Column(Integer, primary_key=True, index=True)

    # Campos obrigatórios
    titulo = Column(String, nullable=False)  # Required no C#

    subtitulo = Column(String, nullable=True)
    descricao = Column(String, nullable=True)

    # Campos de Controle
    ativo = Column(Boolean, default=True)
    excluido = Column(Boolean, default=False)

    imagem = Column(String, nullable=True)
    thumbnail = Column(String, nullable=True)
    arquivo = Column(String, nullable=True)
    imagem_alt = Column(String, nullable=True)
    link = Column(String, nullable=True)
    link_text = Column(String, nullable=True)

    ref = Column(String, nullable=True)
    is_blocked = Column(Boolean, default=False)
    is_blocked_new_item = Column(Boolean, default=False)
    is_blocked_remove_item = Column(Boolean, default=False)
    is_blocked_delete = Column(Boolean, default=False)

    data_criacao = Column(DateTime, default=datetime.utcnow)
    data_edicao = Column(DateTime, nullable=True)
    ordem = Column(Integer, nullable=True)

    # Relação com PageSection
    page_section_id = Column(Integer, ForeignKey("page_sections.id"), nullable=False)
    page_section = relationship("PageSection", back_populates="page_section_items")

    # Coleções (comentadas no C#)
    # style_properties = relationship("StyleProperty", back_populates="page_section_item")
    # metadatas = relationship("Metadata", back_populates="page_section_item")
