
from sqlalchemy import create_engine, text # Import text for test query example
from sqlalchemy.orm import sessionmaker, declarative_base
from typing import Generator
from app.core.config import settings

# Ensure the database URL is correctly formatted for your database (e.g., MySQL)
# Example for MySQL with mysql-connector-python:
# DATABASE_URL="mysql+mysqlconnector://user:password@host:3306/dbname"
# This URL should be defined in your .env file at the project root.
SQLALCHEMY_DATABASE_URL = settings.DATABASE_URL

# Added pool_pre_ping=True for robustness in long-running applications/connections
# Consider other options like pool_size, max_overflow based on your needs
# echo=True can be useful for debugging SQL queries
engine = create_engine(SQLALCHEMY_DATABASE_URL, pool_pre_ping=True) # , echo=True)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Base class for your models
Base = declarative_base()

# Dependency to get DB session
def get_db() -> Generator:
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

