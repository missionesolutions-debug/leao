
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base
from app.config.settings import settings

Base = declarative_base()

# Otimização para 10+ usuários simultâneos
engine = create_engine(
    settings.DATABASE_URL,
    pool_size=20,           # Conexões ativas no pool (para 10 usuários simultâneos)
    max_overflow=30,        # Conexões extras em picos de acesso
    pool_timeout=30,        # Timeout para conseguir conexão do pool
    pool_recycle=3600,      # Reciclar conexões após 1 hora (evita conexões mortas)
    pool_pre_ping=True,     # Testa conexão antes de usar (detecta conexões perdidas)
    echo=False              # Desabilitar logs SQL em produção
)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()