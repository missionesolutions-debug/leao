from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    # Database URL for SQLAlchemy
    # Lê do .env, e deve ser uma string
    DATABASE_URL: str  # ⚠️ tipo necessário

    # Secret key for JWT
    SECRET_KEY: str = "bG9yZW1pcHN1bGRvbG9yc2l0YW1ldA=="

    # Configuração do Pydantic v2 para ler do .env
    model_config = SettingsConfigDict(
        env_file=".env",
        extra="ignore"  # ignora variáveis extras no .env
    )

# Instância das configurações
settings = Settings()
