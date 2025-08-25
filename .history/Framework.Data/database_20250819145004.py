import os
from sqlalchemy import create_engine, MetaData
from sqlalchemy.orm import sessionmaker, declarative_base, Session
from fastapi import HTTPException, Depends # Import Depends for the dependency function itself
from urllib.parse import quote_plus # Optional: For encoding connection string parts
from dotenv import load_dotenv # Optional: For loading environment variables

# --- INSTRUÇÃO MANUAL: Carregar Variáveis de Ambiente ---
# Idealmente, load_dotenv() é chamado UMA VEZ no ponto de entrada principal (main.py)
# Mas você pode descomentar a linha abaixo se este arquivo for o primeiro a ser executado
# e precisar carregar o .env aqui. Certifique-se de que o arquivo .env está na raiz do seu projeto (pasta LeaoPy).
# load_dotenv()
# print("Variáveis de ambiente carregadas.") # Opcional


# Define a SQLAlchemy Base e MetaData globalmente para ser usada pelos modelos
# TODOS os seus modelos de dados (que correspondem aos DbSets do C#) DEVE herdar desta Base.
metadata = MetaData()
Base = declarative_base(metadata=metadata)

# --- INSTRUÇÃO MANUAL: Importar seus Modelos de Dados ---
# Importe TODAS as classes de modelo de dados (que você gerou na pasta models) que herdam de Base.
# Isso é ESSENCIAL para que SQLAlchemy saiba quais tabelas criar/mapear.
# Exemplo (ajuste os caminhos e nomes conforme seus arquivos gerados):
# from LeaoPy.Framework.Data.models.usuario import Usuario
# from LeaoPy.Framework.Data.models.produto import Produto
# from LeaoPy.Framework.Data.models.sua_pasta.seu_modelo import SeuModelo # Para modelos em subpastas


# --- INSTRUÇÃO MANUAL: Obter sua String de Conexão do Banco de Dados ---
# Você pode obter a string de conexão de um arquivo .env (recomendado) ou defini-la diretamente aqui (não recomendado para credenciais).
# Se usar .env, certifique-se de que 'ConnectionStrings__DatabaseConnection' está definido lá.
DATABASE_URL_RAW = os.getenv('ConnectionStrings__DatabaseConnection')

# --- INSTRUÇÃO MANUAL: Configurar a URL do Banco de Dados para SQLAlchemy ---
# Esta é a parte que tivemos problemas de sintaxe automática.
# Construa a URL do banco de dados no formato que o SQLAlchemy e o driver pyodbc esperam.
# Use a DATABASE_URL_RAW lida do .env.
# Ajuste o 'driver={...}' conforme o driver ODBC que você instalou e configurou.
# Exemplo para SQL Server com pyodbc:
SQLALCHEMY_DATABASE_URL = None
if DATABASE_URL_RAW:
    try:
        conn_parts = {}
        for part in DATABASE_URL_RAW.split(';'):
            if '=' in part:
                key, value = part.split('=', 1)
                conn_parts[key.strip()] = value.strip()

        server = conn_parts.get('Server')
        database = conn_parts.get('Database')
        user = conn_parts.get('User')
        password = conn_parts.get('Password')

        if not all([server, database, user, password]):
             print("Aviso: String de conexão do banco de dados incompleta no arquivo .env. Faltam Server, Database, User ou Password.")
        else:
            driver = "ODBC Driver 17 for SQL Server" # --- INSTRUÇÃO MANUAL: VERIFIQUE E AJUSTE O NOME DO SEU DRIVER ODBC ---
            # É uma boa prática codificar usuário, senha e driver caso contenham caracteres especiais.
            user_encoded = quote_plus(user)
            password_encoded = quote_plus(password)
            driver_encoded = quote_plus(driver) # Encode the driver name too

            # --- INSTRUÇÃO MANUAL: CONSTRUA A URL AQUI ---
            # Use f-string ou .format() ou concatenação para construir a URL final.
            # Exemplo com f-string (verifique a sintaxe no seu editor se houver problemas):
            SQLALCHEMY_DATABASE_URL = f"mssql+pyodbc://{user_encoded}:{password_encoded}@{server}/{database}?driver={driver_encoded}"
            # Exemplo com .format():
            # SQLALCHEMY_DATABASE_URL = "mssql+pyodbc://{}:{}@{}/{}?driver={}".format(user_encoded, password_encoded, server, database, driver_encoded)

            print(f"URL de conexão SQLAlchemy construída: {SQLALCHEMY_DATABASE_URL}")

    except Exception as e:
        print(f"ERRO ao processar a string de conexão do banco de dados: {e}")
        print("Por favor, verifique o formato da string 'ConnectionStrings__DatabaseConnection' no seu arquivo .env.")


# Configurar o engine do SQLAlchemy e SessionLocal
engine = None
SessionLocal = None

if SQLALCHEMY_DATABASE_URL:
    try:
        # Criar o engine do SQLAlchemy
        # pool_pre_ping=True ajuda a manter conexões saudáveis
        # connect_args={"check_same_thread": False} é comum para SQLite, geralmente NÃO necessário para SQL Server
        engine = create_engine(SQLALCHEMY_DATABASE_URL, pool_pre_ping=True)
        print("Engine do banco de dados criado.")

        # Criar uma SessionLocal Class (para criar sessões de banco de dados)
        # As sessões criadas a partir desta classe serão usadas para interagir com o DB.
        SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
        print("SessionLocal criada.")

    except Exception as e:
        print(f"ERRO ao criar o Engine ou SessionLocal do banco de dados: {e}")
        print("Verifique a URL construída acima e se o driver ODBC e a conexão estão funcionando fora deste script.")
        engine = None
        SessionLocal = None


# Dependência para obter a sessão do DB (para injeção no FastAPI)
# Esta função será usada com Depends() nos endpoints da API e serviços/repositórios.
def get_db():
    """Fornece uma sessão de banco de dados para injeção de dependência no FastAPI."""
    # Garantir que SessionLocal foi criada antes de tentar usá-la
    if SessionLocal is None:
         print("ERRO: SessionLocal não configurada. A conexão com o banco de dados falhou na inicialização.")
         raise HTTPException(status_code=500, detail="Database connection not configured.")

    db = SessionLocal()
    try:
        yield db # Retorna a sessão e permite que o FastAPI injete-a
    finally:
        # Garante que a sessão seja fechada após o término da requisição
        db.close()

# --- Opcional: Função para Criar Tabelas no Banco de Dados ---
# Útil para desenvolvimento ou testes. Cria as tabelas definidas pelos seus modelos (que herdam de Base)
# se elas ainda não existirem no banco de dados.
# Para usar, você precisará chamar esta função em algum lugar na inicialização da sua aplicação (ex: main.py)
# E IMPORTAR TODOS os seus modelos ANTES de chamar create_database_tables().
# def create_database_tables():
#     if engine:
#         print("Criando tabelas no banco de dados (se não existirem)...")
#         try:
#             # Certifique-se de que TODOS os seus modelos foram importados antes desta linha!
#             Base.metadata.create_all(bind=engine)
#             print("Tabelas criadas.")
#         except Exception as e:
#             print(f"Erro ao criar tabelas no banco de dados: {e}")
#             print("Verifique se seus modelos estão definidos corretamente e se a conexão DB está funcionando.")
#     else:
#         print("Engine do banco de dados não configurado. Não foi possível criar tabelas.")


# --- Exemplo de como usar get_db em um endpoint FastAPI (no seu router ou main.py) ---
# from fastapi import APIRouter, Depends
# from sqlalchemy.orm import Session
# from LeaoPy.Framework.Data.database import get_db # Importe get_db deste arquivo
# from LeaoPy.Framework.Data.models.usuario import Usuario # Importe seu modelo (ajuste o caminho)

# router = APIRouter()

# @router.get("/usuarios/")
# def read_users(db: Session = Depends(get_db)):
#     # Use 'db' para interagir com o banco de dados via SQLAlchemy
#     # users = db.query(Usuario).all()
#     # return users
#     pass