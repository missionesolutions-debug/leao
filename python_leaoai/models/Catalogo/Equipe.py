from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship
from database import Base  # assumindo que Base vem do declarative_base()

class Equipe(Base):
    __tablename__ = "equipe"

    id = Column(Integer, primary_key=True, index=True)  # herdado de BaseCatalogo no C#
    
    pagina_id = Column(Integer, nullable=True)
    chave = Column(String, default="", nullable=False)

    # Relações externas
    categoria_id = Column(Integer, ForeignKey("categoria.id"), nullable=True)
    categoria = relationship("Categoria", back_populates="equipes")
