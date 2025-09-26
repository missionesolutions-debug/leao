from pydantic_settings import BaseSettings
from dotenv import load_dotenv
import os


load_dotenv()
class Settings(BaseSettings):
    DATABASE_URL: str
    OPENAI_API_KEY: str = os.getenv("OPENAI_API_KEY")
    SECRET_KEY: str
    ALGORITHM: str = "HS256"
    ACCESS_TOKEN_EXPIRE_MINUTES: int = 30
    DEBUG: bool = False
    ALLOW_ORIGINS: list = [
        "https://www.leaoai.com.br", 
        "http://www.leaoai.com.br",
        "http://localhost:3000",
        "http://127.0.0.1:3000",
        "http://localhost:8000",
        "http://127.0.0.1:8000",
        "http://localhost:8001",
        "http://127.0.0.1:8001"
    ]

    class Config:
        env_file = "app/.env"
        env_file_encoding = "utf-8"

settings = Settings()