from sqlalchemy import Column, Integer, String
from app.config.database import Base

class ManifestacaoOposicao(Base):
    __tablename__ = "manifestacoes_oposicao"

    id = Column(Integer, primary_key=True, index=True)
    numero_registro = Column(String, nullable=False)
    numero_processo = Column(String, nullable=False)
    data_deposito = Column(String, nullable=True)
    marca_registrada = Column(String, nullable=False)
    classe_registro = Column(String, nullable=False)
    especificacao_registro = Column(String, nullable=False)
    titular_registro = Column(String, nullable=False)
    rpi_oposicao = Column(String, nullable=True)
    data_oposicao = Column(String, nullable=True)
    opoente = Column(String, nullable=True)
    marca_oposicao = Column(String, nullable=True)
    processo_oposicao = Column(String, nullable=True)
    fundamento_oposicao = Column(String, nullable=True)
    tipo_conflito_alegado = Column(String, nullable=True)
    tipo_reproducao_alegada = Column(String, nullable=True)
    analise_mercadologica_defesa = Column(String, nullable=True)
    distintividade_fonetica = Column(String, nullable=True)
    distintividade_ideologica = Column(String, nullable=True)
    distintividade_visual = Column(String, nullable=True)
    especialidade_segmento = Column(String, nullable=True)
    especialidade_publico = Column(String, nullable=True)
    especialidade_canais = Column(String, nullable=True)
    coexistencia = Column(String, nullable=True)
    decisoes_anteriores = Column(String, nullable=True)
    uso_anterior_boa_fe = Column(String, nullable=True)
    outros_registros = Column(String, nullable=True)
    ma_fe = Column(String, nullable=True)