from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey
from sqlalchemy.orm import relationship
from datetime import datetime
from database import Base


class PageSection(Base):
    __tablename__ = "page_section"

    id = Column(Integer, primary_key=True, index=True)

    titulo = Column(String, nullable=True)
    subtitulo = Column(String, nullable=True)
    descricao = Column(String, nullable=True)

    # Campos de Controle
    ativo = Column(Boolean, default=True, nullable=False)
    excluido = Column(Boolean, default=False, nullable=False)
    data_criacao = Column(DateTime, default=datetime.utcnow, nullable=False)
    data_edicao = Column(DateTime, nullable=True)
    ref = Column(String, nullable=True)

    is_blocked = Column(Boolean, default=False, nullable=True)
    is_blocked_new_item = Column(Boolean, default=False, nullable=True)
    is_blocked_remove_item = Column(Boolean, default=False, nullable=True)
    is_blocked_delete = Column(Boolean, default=False, nullable=True)

    ordem = Column(Integer, nullable=True)

    imagem = Column(String, nullable=True)
    thumbnail = Column(String, nullable=True)
    arquivo = Column(String, nullable=True)
    imagem_alt = Column(String, nullable=True)
    link = Column(String, nullable=True)
    link_text = Column(String, nullable=Tr_
