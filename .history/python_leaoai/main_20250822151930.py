import os
import sys
leo_py_root = os.path.dirname(os.path.abspath(__file__))
if leo_py_root not in sys.path:
    sys.path.insert(0, leo_py_root)
from fastapi import FastAPI, Depends, HTTPException
from fastapi.staticfiles import StaticFiles
from fastapi.responses import HTMLResponse
from fastapi.middleware.cors import CORSMiddleware
from sqlalchemy.orm import Session
from dotenv import load_dotenv

# ----------------------------
# Carregar variáveis de ambiente
# ----------------------------
load_dotenv()
print("Variáveis de ambiente carregadas.")

# ----------------------------
# Configurar sys.path para módulos locais
# ----------------------------
leo_py_root = os.path.dirname(os.path.abspath(__file__))
if leo_py_root not in sys.path:
    sys.path.insert(0, leo_py_root)
    print(f"'{leo_py_root}' adicionado ao sys.path para importação de módulos locais.")

# ----------------------------
# Importar banco de dados e dependências
# ----------------------------
from database import get_db, Base, engine

# Importar serviços e routers
from services.Pages.Conteudo import BlogService


# ----------------------------
# Criar instância do FastAPI
# ----------------------------
app = FastAPI(title="LeaoPy Backend Replicado")

# ----------------------------
# Configurar CORS
# ----------------------------
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Ajustar para produção
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# ----------------------------
# Providers / Injeção de dependência
# ----------------------------
def get_blog_service(db: Session = Depends(get_db)) -> BlogService:
    return BlogService(db=db)

# ----------------------------
# Servir arquivos estáticos
# ----------------------------
python_static_path = os.path.join(leo_py_root, 'static')
if os.path.exists(python_static_path):
    app.mount("/static", StaticFiles(directory=python_static_path), name="static")
    print(f"Pasta estática '{python_static_path}' montada em '/static'.")
else:
    print(f"Aviso: Pasta estática '{python_static_path}' não encontrada. Arquivos estáticos não serão servidos.")

@app.get("/", response_class=HTMLResponse)
async def read_root():
    html_file_path = os.path.join(python_static_path, 'index.html')
    if os.path.exists(html_file_path):
        with open(html_file_path, 'r', encoding='utf-8') as f:
            html_content = f.read()
        return HTMLResponse(content=html_content, status_code=200)
    else:
        return HTMLResponse(content="<h1>Backend Python Rodando!</h1><p>Index HTML não encontrado na pasta static.</p>", status_code=404)

# ----------------------------
# Incluir routers
# ----------------------------

# ----------------------------
# Criar tabelas (opcional)
# ----------------------------
try:
    Base.metadata.create_all(bind=engine)
    print("Tabelas criadas com sucesso (se não existiam).")
except Exception as e:
    print(f"Erro ao criar tabelas: {e}")

# ----------------------------
# Rodar a aplicação localmente (opcional)
# ----------------------------
if __name__ == "__main__":
    import uvicorn
    host = os.getenv("HOST", "0.0.0.0")
    port = int(os.getenv("PORT", 8000))
    uvicorn.run("main:app", host=host, port=port, reload=True)
