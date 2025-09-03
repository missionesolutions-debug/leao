import os
from sqlalchemy import create_engine
from sqlalchemy.exc import SQLAlchemyError
from dotenv import load_dotenv

# Carrega variáveis do .env
load_dotenv(dotenv_path=os.path.join(os.path.dirname(__file__), ".env"))

# Pega a string de conexão do .env
DATABASE_URL = os.getenv("DATABASE_URL")

if not DATABASE_URL:
    print("DATABASE_URL não encontrada no .env")
else:
    try:
        engine = create_engine(DATABASE_URL)
        with engine.connect() as connection:
            result = connection.execute("SELECT 1")
            print("Conexão bem-sucedida! Resultado:", result.scalar())
    except SQLAlchemyError as e:
        print("Erro ao conectar ao banco de dados:", e)