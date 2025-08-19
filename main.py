import os
import sys
from fastapi import FastAPI, Depends, HTTPException
from fastapi.staticfiles import StaticFiles
from fastapi.responses import HTMLResponse
from dotenv import load_dotenv

# Carregar variáveis de ambiente do arquivo .env
load_dotenv()
print("Variáveis de ambiente carregadas.")

# Adicionar o diretório raiz do projeto ao sys.path para importação de módulos locais
# Isso ajuda o Python a encontrar pacotes como LeaoPy.Framework, LeaoPy.Api, etc.
leo_py_root = os.path.dirname(os.path.abspath(__file__))
if leo_py_root not in sys.path:
    sys.path.insert(0, leo_py_root)
    print(f"'{leo_py_root}' adicionado ao sys.path para importação de módulos locais.")

# --- TODO: Importar configuração de banco de dados (se database.py estiver funcionando) ---
# Exemplo:
# from LeaoPy.Framework.Data.database import get_db, Base, engine # Importar get_db, Base e engine

# --- TODO: Importar seus Repositórios e Serviços ---
# Você precisará importar as classes que replicou para configurar a injeção de dependência.
# Exemplo:
# from LeaoPy.Framework.Repositories.your_repository import YourRepository
# from LeaoPy.Api.services.your_service import YourService

# --- TODO: Importar seus Routers da API ---
# Você precisará importar os objetos APIRouter que foram gerados.
# Exemplo:
# from LeaoPy.Api.routers import your_router # ou from LeaoPy.Api.routers.some_subfolder import your_router

# --- Criar instância da aplicação FastAPI ---
app = FastAPI(title="LeaoPy Backend Replicado")

# --- TODO: Configurar Middleware (Baseado na análise de Program.cs C#) ---
# Configurar CORS (se necessário, já pode estar no template básico)
# from fastapi.middleware.cors import CORSMiddleware
# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=["*"], # Ajuste para produção
#     allow_credentials=True,
#     allow_methods=["*"],
#     allow_headers=["*"],
# )

# Outros Middlewares como Autenticação, Autorização, Tratamento de Erros, Redirecionamento HTTPS

# --- TODO: Configurar Injeção de Dependência ---
# Defina "providers" (funções que retornam instâncias de classes) para seus Repositórios e Serviços
# Estes providers usarão Depends() para injetar suas próprias dependências (como a sessão DB)
# Exemplo (assumindo que get_db e YourRepository/YourService foram importados):
# def get_your_repository(db: Session = Depends(get_db)) -> YourRepository:
#     return YourRepository(db=db)
#
# def get_your_service(your_repo: YourRepository = Depends(get_your_repository)) -> YourService:
#     return YourService(your_repo=your_repo)


# --- Montar Rota para Arquivos Estáticos ---
# Caminho para a pasta de arquivos estáticos
python_static_path = os.path.join(leo_py_root, 'static')
if os.path.exists(python_static_path):
    app.mount("/static", StaticFiles(directory=python_static_path), name="static")
    print(f"Pasta estática '{python_static_path}' montada em '/static'.")
else:
    print(f"Aviso: Pasta estática '{python_static_path}' não encontrada. Arquivos estáticos não serão servidos.")


@app.get("/", response_class=HTMLResponse)
async def read_root():
    # Servir um arquivo HTML principal da pasta static (se existir)
    html_file_path = os.path.join(python_static_path, 'index.html')
    if os.path.exists(html_file_path):
        with open(html_file_path, 'r', encoding='utf-8') as f:
            html_content = f.read()
        return HTMLResponse(content=html_content, status_code=200)
    else:
        return HTMLResponse(content="<h1>Backend Python Rodando!</h1><p>Index HTML não encontrado na pasta static.</p>", status_code=404)


# --- TODO: Incluir seus Routers da API ---
# Use app.include_router() para cada router que você importou.
# Injete as dependências necessárias nos endpoints dos routers usando Depends().
# Exemplo (assumindo seu_router foi importado e get_your_service existe):
# app.include_router(your_router.router, prefix="/your-endpoint-prefix", tags=["your_tag"])
# Rotas de exemplo com dependência (assumindo get_your_service como dependência para este endpoint):
# @your_router.router.get("/some-path", dependencies=[Depends(get_your_service)])
# async def read_items(your_service: YourService = Depends(get_your_service)):
#     # Use your_service aqui
#     pass


# --- Opcional: Criar Tabelas no Banco de Dados na Inicialização ---
# Se você estiver usando modelos declarativos e quiser que o SQLAlchemy crie as tabelas
# com base neles na inicialização da aplicação (útil para desenvolvimento/testes)
# if 'engine' in globals() and engine is not None: # Verifica se o engine foi configurado
#     print("Criando tabelas no banco de dados (se não existirem)...")
#     try:
#         # Certifique-se de que todos os seus modelos foram importados ANTES desta linha
#         # from LeaoPy.Framework.Data.models import YourModel # Importe todos os seus modelos
#         # Base.metadata.create_all(bind=engine) # Cria as tabelas
#         print("Criação de tabelas concluída.")
#     except Exception as e:
#         print(f"Erro ao criar tabelas no banco de dados: {e}")


# --- Como rodar a aplicação (para uso local ou em outro ambiente) ---
# if __name__ == "__main__":
#     # Você precisará instalar uvicorn: pip install uvicorn
#     import uvicorn
#     # Configure o host e a porta, talvez a partir de variáveis de ambiente
#     # host = os.getenv("HOST", "0.0.0.0")
#     # port = int(os.getenv("PORT", 8000))
#     # uvicorn.run("main:app", host=host, port=port, reload=True) # Use reload=True para desenvolvimento
#     pass # No Colab, geralmente não rodamos o servidor diretamente assim.