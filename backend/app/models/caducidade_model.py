from sqlalchemy import Column, Integer, String
from app.config.database import Base

class Caducidade(Base):
    __tablename__ = "caducidades"

    id = Column(Integer, primary_key=True, index=True)
    registro_marca = Column(String)
    marca_requerida = Column(String)
    classe_marca = Column(String)
    especificacoes = Column(String)
    titular_marca = Column(String)
    numero_processo = Column(String)
    nome_cliente = Column(String)
    marca_cliente = Column(String)
    data_deposito = Column(String)
    classe_marca_cliente = Column(String)