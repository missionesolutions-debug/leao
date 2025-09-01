
from pydantic_settings import BaseSettings, SettingsConfigDict
from typing import Optional
from dotenv import load_dotenv 
import os

class Settings(BaseSettings):
    # Database URL for SQLAlchemy
    # Ensure this is set in your .env file, e.g., DATABASE_URL="mysql+mysqlconnector://user:password@host:3306/dbname"
    DATABASE_URL: os.getenv("DATABASE_URL")

    # Secret key for JWT
    # IMPORTANT: Change this to a strong, random key in production
    SECRET_KEY: str = os.getenv("SECRET_KEY")

    # Add other configuration settings as needed from your C# project

    model_config = SettingsConfigDict(env_file=".env", extra="ignore") # Pydantic V2

settings = Settings()

