import os
from sqlalchemy import create_engine, MetaData
from sqlalchemy.orm import sessionmaker, declarative_base, Session
from fastapi import HTTPException
from urllib.parse import quote_plus
from dotenv import load_dotenv

# ----------------------------
# Carregar variáveis de ambiente
# ----------------------------
load_dotenv()

# ----------------------------
# Base SQLAlchemy
# ----------------------------
metadata = MetaData()
Base = declarative_base(metadata=metadata)

# ----------------------------
# Importar todos os modelos
# ----------------------------
# Exemplo:
# from LeaoPy.Framework.Data.models.usuario import Usuario
# from LeaoPy.Framework.Data.models.produto import Produto
# from LeaoPy.Framework.Data.models.blog import Blog
# Adicione aqui todos os seus modelos de dados

# ----------------------------
# Configurar URL do banco de dados
# ----------------------------
DATABASE_URL_RAW = os.getenv('ConnectionStrings__DatabaseConnection')

SQLALCHEMY_DATABASE_URL = None
if DATABASE_URL_RAW:
    try:
        # Separar os componentes da string "Server=...;Database=...;User=...;Password=..."
        conn_parts = dict(part.split('=', 1) for part in DATABASE_URL_RAW.split(';') if '=' in part)
        server = conn_parts.get('Server')
        database = conn_parts.get('Database')
        user = conn_parts.get('User')
        password = conn_parts.get('Password')

        if all([server, database, user, password]):
            driver = "ODBC Driver 17 for SQL Server"  # Ajuste conforme seu ODBC
            user_encoded = quote_plus(user)
            password_encoded = quote_plus(password)
            driver_encoded = quote_plus(driver)

            SQLALCHEMY_DATABASE_URL = f"mssql+pyodbc://{user_encoded}:{password_encoded}@{server}/{database}?driver={driver_encoded}"
            print(f"URL de conexão SQLAlchemy construída: {SQLALCHEMY_DATABASE_URL}")
        else:
            print("Aviso: String de conexão incompleta no .env.")
    except Exception as e:
        print(f"Erro ao processar a string de conexão: {e}")

# ----------------------------
# Criar Engine e SessionLocal
# ----------------------------
engine = None
SessionLocal = None

if SQLALCHEMY_DATABASE_URL:
    try:
        engine = create_engine(SQLALCHEMY_DATABASE_URL, pool_pre_ping=True)
        SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
        print("Engine e SessionLocal criados com sucesso.")
    except Exception as e:
        print(f"Erro ao criar engine ou SessionLocal: {e}")

# ----------------------------
# Dependência para FastAPI
# ----------------------------
def get_db():
    """Fornece uma sessão de banco de dados para injeção de dependência no FastAPI."""
    if SessionLocal is None:
        raise HTTPException(status_code=500, detail="Database connection not configured.")
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# ----------------------------
# Função opcional para criar tabelas (útil em dev/test)
# ----------------------------
def create_database_tables():
    """Cria todas as tabelas definidas nos modelos se não existirem."""
    if engine:
        try:
            Base.metadata.create_all(bind=engine)
            print("Tabelas criadas com sucesso.")
        except Exception as e:
            print(f"Erro ao criar tabelas: {e}")
    else:
        print("Engine não configurado. Não é possível criar tabelas.")
