from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    app_name: str = "Leão Adv"
    api_version: str = "v1"
    #openai_api_key: str
    DATABASE_URL: str

    class Config:
        env_file = ".env"
        env_file_encoding = "utf-8"

settings = Settings()