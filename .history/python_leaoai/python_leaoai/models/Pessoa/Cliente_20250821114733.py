# models/cliente.py

from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base  # Supondo que você tenha um Base comum
from typing import Optional

class Cliente(Base):
    __tablename__ = "cliente"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    nome: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    razao_social: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cnpj: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cep: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    logradouro: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    numero: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    complemento: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    bairro: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cidade: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    estado: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    pais: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    telefone_empresa: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    whatsapp_empresa: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    logo: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    email: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    facebook: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    instagram: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    linkedin: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    website: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    imagem: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    imagem_alt: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    nome_responsavel: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    cpf_responsavel: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    whatsapp_responsavel: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    email_responsavel: Mapped[Optional[str]] = mapped_column(String, nullable=True)
    pagina_id: Mapped[Optional[int]] = mapped_column(Integer, ForeignKey("pagina.id"), nullable=True)
    chave: Mapped[Optional[str]] = mapped_column(String, nullable=True)

    # Caso queira relacionamento com a tabela Página
    # pagina: Mapped["Pagina"] = relationship("Pagina", back_populates="clientes")
