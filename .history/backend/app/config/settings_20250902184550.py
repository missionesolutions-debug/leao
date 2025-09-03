from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    # openai_api_key: str
    DATABASE_URL: str
    SECRET_KEY: str
    DEBUG: bool = False

    class Config:
        env_file = "app/.env"
        env_file_encoding = "utf-8"

settings = Settings()