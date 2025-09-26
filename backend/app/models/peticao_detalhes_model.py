from sqlalchemy import Column, Integer, String, Text, JSON, ForeignKey
from app.config.database import Base

class PeticaoDetalhes(Base):
    __tablename__ = "peticao_detalhes"

    id = Column(Integer, primary_key=True, index=True)
    peticao_id = Column(Integer, ForeignKey("peticoes.id"), nullable=False)
    
    # Dados estruturados da petição em formato JSON
    dados_origem = Column(JSON, nullable=False)  # Dados originais do formulário
    
    # Campos indexáveis para consultas rápidas
    numero_processo = Column(String, index=True)
    numero_registro = Column(String, index=True)
    marca_registrada = Column(String, index=True)
    classe_marca = Column(String, index=True)
    titular_registro = Column(String, index=True)