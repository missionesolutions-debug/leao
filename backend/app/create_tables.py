from app.config.database import engine, Base
from app.models.brand_model import Brand
from app.models.user_model import User
from app.models.caducidade_model import Caducidade
from app.models.contrarazao_nulidade_model import ContrarazaoNulidade
from app.models.nulidade_model import Nulidade
from app.models.recurso_indeferimento_model import RecursoIndeferimento
from app.models.manifestacao_oposicao_model import ManifestacaoOposicao
from app.models.manifestacao_recurso_model import ManifestacaoRecurso
from app.models.oposicao_model import Oposicao

# Adicione outros modelos aqui conforme necessário

Base.metadata.create_all(bind=engine)