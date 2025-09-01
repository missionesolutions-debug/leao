
from fastapi import FastAPI, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.core.database import get_db, engine, Base # Import get_db and Base
from app.core.config import settings # Import settings
# Import routers here as they are created
# from app.routers import projects # Example import

# Create database tables if they don't exist (for development/testing)
# In production, use migration tools like Alembic
# Base.metadata.create_all(bind=engine)


app = FastAPI(
    title="LeaoAi Refactored API",
    description="Migrated API from C# project",
    version="0.1.0",
)

# Include routers as they are defined
# app.include_router(projects.router, prefix="/api/projects", tags=["projects"]) # Example include


@app.get("/")
def read_root():
    return {"message": "Welcome to the Refactored LeaoAi API"}

# Add other root level endpoints or event handlers here
# Example of using DB dependency in a simple endpoint (for testing)
@app.get("/test-db")
def test_db_connection(db: Session = Depends(get_db)):
    try:
        # Try to execute a simple query
        db.execute(text("SELECT 1")) # Requires 'from sqlalchemy import text'
        return {"message": "Database connection successful"}
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Database connection error: {e}")

