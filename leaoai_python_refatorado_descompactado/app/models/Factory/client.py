from sqlalchemy import Column, Integer, String, Boolean, DateTime
from app.core.database import Base
from sqlalchemy.orm import relationship

class Client(Base):
    __tablename__ = "clients"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    CNPJ = Column(String)
    Endereco = Column(String, nullable=True)
    Telefone = Column(String, nullable=True)
    Email = Column(String, nullable=True)
    CEP = Column(String, nullable=True)
    PorcentagemCaixas = Column(Integer, nullable=True)
    PorcentagemCaixasMontadas = Column(Integer, nullable=True)
    Ativo = Column(Boolean, default=True)
    Excluido = Column(Boolean, default=False)
    DataCriacao = Column(DateTime)

    # Exemplo de relação sem import circular
    # usuarios = relationship("Usuario", back_populates="client")
