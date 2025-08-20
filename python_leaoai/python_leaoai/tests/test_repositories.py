
import pytest
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
import sys
import os

# Ensure the project base directory is in sys.path to allow imports
python_project_base_dir = '/content/python_leaoai'
if python_project_base_dir not in sys.path:
    sys.path.append(python_project_base_dir)

from database import Base
from models.placeholder_model import Placeholder
from repositories.placeholder_repository import PlaceholderRepository

# Setup a test database (in-memory SQLite)
@pytest.fixture(scope="module")
def test_db():
    engine = create_engine("sqlite:///:memory:")
    Base.metadata.create_all(bind=engine)
    TestingSessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
    db = TestingSessionLocal()
    try:
        yield db
    finally:
        db.close()

# Test Repository methods
def test_create_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    placeholder_data = {"name": "Test Placeholder 1", "description": "Description 1"}
    created_placeholder = repo.create_placeholder(**placeholder_data)

    assert created_placeholder is not None
    assert created_placeholder.id is not None
    assert created_placeholder.name == placeholder_data["name"]
    assert created_placeholder.description == placeholder_data["description"]

    # Verify it's in the database
    retrieved_placeholder = test_db.query(Placeholder).filter(Placeholder.id == created_placeholder.id).first()
    assert retrieved_placeholder is not None
    assert retrieved_placeholder.name == placeholder_data["name"]

def test_get_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Create a placeholder first
    placeholder_data = {"name": "Test Placeholder 2", "description": "Description 2"}
    created_placeholder = repo.create_placeholder(**placeholder_data)

    # Get the placeholder by ID
    retrieved_placeholder = repo.get_placeholder(created_placeholder.id)

    assert retrieved_placeholder is not None
    assert retrieved_placeholder.id == created_placeholder.id
    assert retrieved_placeholder.name == placeholder_data["name"]

def test_get_nonexistent_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Try to get a placeholder with a non-existent ID
    retrieved_placeholder = repo.get_placeholder(999)
    assert retrieved_placeholder is None

def test_get_all_placeholders(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Create a couple of placeholders
    repo.create_placeholder(name="Test Placeholder A", description="Desc A")
    repo.create_placeholder(name="Test Placeholder B", description="Desc B")

    # Get all placeholders
    all_placeholders = repo.get_all_placeholders()

    assert len(all_placeholders) >= 2 # Check if at least the ones we added are there
    assert any(p.name == "Test Placeholder A" for p in all_placeholders)
    assert any(p.name == "Test Placeholder B" for p in all_placeholders)


def test_update_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Create a placeholder
    placeholder_data = {"name": "Original Name", "description": "Original Description"}
    created_placeholder = repo.create_placeholder(**placeholder_data)

    # Update the placeholder
    updated_data = {"name": "Updated Name", "description": "Updated Description"}
    updated_placeholder = repo.update_placeholder(created_placeholder.id, **updated_data)

    assert updated_placeholder is not None
    assert updated_placeholder.id == created_placeholder.id
    assert updated_placeholder.name == updated_data["name"]
    assert updated_placeholder.description == updated_data["description"]

    # Verify the update in the database
    retrieved_placeholder = test_db.query(Placeholder).filter(Placeholder.id == created_placeholder.id).first()
    assert retrieved_placeholder is not None
    assert retrieved_placeholder.name == updated_data["name"]
    assert retrieved_placeholder.description == updated_data["description"]

def test_update_nonexistent_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Try to update a non-existent placeholder
    updated_placeholder = repo.update_placeholder(999, name="Should Not Update")
    assert updated_placeholder is None


def test_delete_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Create a placeholder
    placeholder_data = {"name": "To Delete", "description": "Delete Me"}
    created_placeholder = repo.create_placeholder(**placeholder_data)

    # Delete the placeholder
    delete_successful = repo.delete_placeholder(created_placeholder.id)
    assert delete_successful is True

    # Verify deletion from the database
    retrieved_placeholder = test_db.query(Placeholder).filter(Placeholder.id == created_placeholder.id).first()
    assert retrieved_placeholder is None

def test_delete_nonexistent_placeholder(test_db):
    repo = PlaceholderRepository(db=test_db)
    # Try to delete a non-existent placeholder
    delete_successful = repo.delete_placeholder(999)
    assert delete_successful is False
