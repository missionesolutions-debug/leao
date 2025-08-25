from sqlalchemy import Column, Integer, String
from app.core.database import Base
from sqlalchemy.orm import relationship

class Usuario(Base):
    __tablename__ = "Usuarios"

    Id = Column(Integer, primary_key=True, index=True)
    Nome = Column(String)
    Email = Column(String, index=True)
    Password = Column(String)
    RoleGate = Column(String)
    Avatar = Column(String, nullable=True)

    # Exemplo de relação sem import circular
    # client_id = Column(Integer, ForeignKey("clients.Id"))
    # client = relationship("Client", back_populates="usuarios")
