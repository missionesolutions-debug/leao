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
from app.models.peticao_model import Peticao
from app.models.chat_historico_model import ChatHistorico
from app.models.peticao_detalhes_model import PeticaoDetalhes

# Criação de todas as tabelas
print("Criando tabelas do banco de dados...")

# Criar todas as tabelas
Base.metadata.create_all(bind=engine)

print("✅ Tabelas criadas com sucesso!")
print("\nTabelas disponíveis:")
print("- users (usuários)")
print("- brands (marcas)")
print("- caducidade")
print("- contrarazao_nulidade") 
print("- nulidade")
print("- recurso_indeferimento")
print("- manifestacao_oposicao")
print("- manifestacao_recurso")
print("- oposicao")
print("- peticoes (petições geradas)")
print("- chat_historico (histórico de conversas)")
print("- peticao_detalhes (dados detalhados das petições)")