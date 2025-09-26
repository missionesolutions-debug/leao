from sqlalchemy import Column, Integer, String
from app.config.database import Base

class ContrarazaoNulidade(Base):
    __tablename__ = "contrarazao_nulidade"

    id = Column(Integer, primary_key=True, index=True)
    cliente = Column(String, nullable=False)
    classe = Column(String, nullable=False)
    titular = Column(String, nullable=False)
    especificacoes = Column(String, nullable=True)
    numero = Column(String, nullable=True)
    data = Column(String, nullable=True)  # Considere usar Date se preferir
    marca_terceiro = Column(String, nullable=True)
    processo_terceiro = Column(String, nullable=True)
    marca_requerida = Column(String, nullable=True)
    processo_requerido = Column(String, nullable=True)
    marca_cliente = Column(String, nullable=True)
    comentario_diferenca_marcas = Column(String, nullable=True)
    planilha_marcas_similares = Column(String, nullable=True)
    comentario_distincao_produto_servico = Column(String, nullable=True)