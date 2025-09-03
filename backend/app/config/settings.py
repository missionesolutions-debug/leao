from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    DATABASE_URL: str
    # OPENAI_API_KEY: str
    SECRET_KEY: str
    ALGORITHM: str = "HS256"
    ACCESS_TOKEN_EXPIRE_MINUTES: int = 30
    DEBUG: bool = False
    ALLOW_ORIGINS: str = "*"

    class Config:
        env_file = "app/.env"
        env_file_encoding = "utf-8"

settings = Settings()